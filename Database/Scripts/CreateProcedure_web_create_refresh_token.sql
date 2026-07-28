-- Persists a newly generated refresh token for a user. The token value itself is generated
-- application-side (RefreshTokenService); this procedure only stores it.

CREATE OR ALTER PROCEDURE [auth].[web_create_refresh_token]
    @p_user_id INT,
    @p_token NVARCHAR(500),
    @p_expires_at DATETIME2,
    @p_ip_address NVARCHAR(45) = NULL,
    @p_user_agent NVARCHAR(500) = NULL,
    @p_created_by INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO auth.refresh_tokens
        (
            user_id,
            token,
            expires_at,
            created_by,
            ip_address,
            user_agent
        )
        VALUES
        (
            @p_user_id,
            @p_token,
            @p_expires_at,
            @p_created_by,
            @p_ip_address,
            @p_user_agent
        );
    END TRY
    BEGIN CATCH
        DECLARE @v_user_name NVARCHAR(100);
        DECLARE @v_error_number INT = ERROR_NUMBER();
        DECLARE @v_error_state INT = ERROR_STATE();
        DECLARE @v_error_severity INT = ERROR_SEVERITY();
        DECLARE @v_error_line INT = ERROR_LINE();
        DECLARE @v_error_procedure NVARCHAR(200) = ERROR_PROCEDURE();
        DECLARE @v_error_message NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @v_error_param NVARCHAR(MAX) = CONCAT('@p_user_id = ', @p_user_id);
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
