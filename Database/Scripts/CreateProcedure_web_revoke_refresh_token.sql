-- Revokes a refresh token, optionally recording the token that replaced it. Passing a
-- @p_replaced_by_token value is how rotation is tracked once the future /auth/refresh
-- endpoint issues a new token in exchange for this one.

CREATE OR ALTER PROCEDURE [auth].[web_revoke_refresh_token]
    @p_token NVARCHAR(500),
    @p_replaced_by_token NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE auth.refresh_tokens
        SET
            is_revoked = 1,
            revoked_at = SYSUTCDATETIME(),
            replaced_by_token = @p_replaced_by_token
        WHERE token = @p_token
          AND is_revoked = 0;
    END TRY
    BEGIN CATCH
        DECLARE @v_user_name NVARCHAR(100);
        DECLARE @v_error_number INT = ERROR_NUMBER();
        DECLARE @v_error_state INT = ERROR_STATE();
        DECLARE @v_error_severity INT = ERROR_SEVERITY();
        DECLARE @v_error_line INT = ERROR_LINE();
        DECLARE @v_error_procedure NVARCHAR(200) = ERROR_PROCEDURE();
        DECLARE @v_error_message NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @v_error_param NVARCHAR(MAX) = N'@p_token = <redacted>';
        DECLARE @v_error_number_app_id INT;

        EXEC dbo.proc_log_errors
             @p_user_name_db = @v_user_name,
             @p_error_number_db = @v_error_number,
             @p_error_state_db = @v_error_state,
             @p_error_severity_db = @v_error_severity,
             @p_error_line_db = @v_error_line,
             @p_error_procedure_db = @v_error_procedure,
             @p_error_param_db = @v_error_param,
             @p_error_message_db = @v_error_message,
             @p_error_number_app_id = @v_error_number_app_id;

        IF XACT_STATE() = -1 ROLLBACK;
        IF XACT_STATE() = 1 COMMIT;

        THROW;
    END CATCH;

    SET NOCOUNT OFF;
END;
GO
