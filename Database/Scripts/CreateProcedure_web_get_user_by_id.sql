-- NOTE: this is the one new stored procedure introduced by the Refresh Token feature.
-- auth.refresh_tokens only stores user_id (not email), and no existing procedure could
-- re-hydrate full user + role + organization + permission data from a user_id alone
-- (web_get_user_by_email and web_login both require an email). This mirrors web_login's
-- join shape exactly, swapping the email filter for a user_id filter, and omits
-- password_hash since the refresh flow never needs it.

CREATE OR ALTER PROCEDURE [auth].[web_get_user_by_id]
    @p_user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        SELECT
            u.user_id,
            u.organization_id,
            o.organization_name,

            ur.role_id,
            r.role_name,

            u.username,
            u.email,

            u.is_enabled AS is_active,

            p.permission_id,
            p.permission_name,

            CAST
            (
                CASE
                    WHEN rp.role_permission_id IS NULL THEN 0
                    ELSE 1
                END
            AS BIT) AS is_allowed

        FROM auth.users u WITH (NOLOCK)

        INNER JOIN auth.organizations o
            ON o.organization_id = u.organization_id

        INNER JOIN auth.user_roles ur
            ON ur.user_id = u.user_id
           AND ur.is_enabled = 1

        INNER JOIN auth.roles r
            ON r.role_id = ur.role_id
           AND r.is_enabled = 1

        CROSS JOIN auth.permissions p

        LEFT JOIN auth.role_permissions rp
            ON rp.role_id = ur.role_id
           AND rp.permission_id = p.permission_id
           AND rp.is_enabled = 1

        WHERE
            u.user_id = @p_user_id
            AND u.is_enabled = 1
            AND p.is_enabled = 1

        ORDER BY
            p.permission_name;

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

        IF XACT_STATE() = -1
            ROLLBACK;

        IF XACT_STATE() = 1
            COMMIT;

        THROW;

    END CATCH;

    SET NOCOUNT OFF;
END;
GO
