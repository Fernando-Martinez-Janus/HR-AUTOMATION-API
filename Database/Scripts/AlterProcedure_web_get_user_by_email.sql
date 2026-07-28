-- Script para alinear el nombre de columna con la propiedad C# User.IsActive
-- Aplica: alias is_enabled AS is_active

CREATE OR ALTER PROCEDURE [auth].[web_get_user_by_email]
	@p_email NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		SELECT
			t.user_id,
			t.organization_id,
			o.organization_name,
			t.username,
			t.email,
			t.is_enabled AS is_active,
			t.created_at,
			t.created_by,
			t.updated_at,
			t.updated_by
		FROM auth.users AS t WITH (NOLOCK)
		INNER JOIN auth.organizations AS o WITH (NOLOCK)
			ON t.organization_id = o.organization_id
		WHERE t.email = @p_email;
	END TRY
	BEGIN CATCH
		DECLARE @v_user_name NVARCHAR(100);
		DECLARE @v_error_number INT = ERROR_NUMBER();
		DECLARE @v_error_state INT = ERROR_STATE();
		DECLARE @v_error_severity INT = ERROR_SEVERITY();
		DECLARE @v_error_line INT = ERROR_LINE();
		DECLARE @v_error_procedure NVARCHAR(200) = ERROR_PROCEDURE();
		DECLARE @v_error_message NVARCHAR(MAX) = ERROR_MESSAGE();
		DECLARE @v_error_param NVARCHAR(MAX) = CONCAT('@p_email = ', @p_email);
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
