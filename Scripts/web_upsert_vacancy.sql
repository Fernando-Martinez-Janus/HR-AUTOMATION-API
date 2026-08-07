CREATE OR ALTER PROCEDURE [recruitment].[web_upsert_vacancy]
    @p_vacancy_id            BIGINT        = NULL,
    @p_organization_id       INT,
    @p_profile_id            INT           = NULL,
    @p_criticality_level_id  INT           = NULL,
    @p_vacancy_status_id     INT           = NULL,
    @p_vacancy_title         NVARCHAR(200) = NULL,
    @p_client_name           NVARCHAR(200) = NULL,
    @p_project_name          NVARCHAR(200) = NULL,
    @p_vacancy_location      NVARCHAR(200) = NULL,
    @p_position_count        INT           = NULL,
    @p_salary_range_min      DECIMAL(12,2) = NULL,
    @p_salary_range_max      DECIMAL(12,2) = NULL,
    @p_request_date          DATETIME2     = NULL,
    @p_deadline_date         DATETIME2     = NULL,
    @p_modality_id           INT           = NULL,
    @p_contract_type_id      INT           = NULL,
    @p_currency_id           INT           = NULL,
    @p_pay_frequency_id      INT           = NULL,
    @p_notes                 NVARCHAR(MAX) = NULL,
    @p_created_by            INT           = NULL,
    @p_updated_by            INT           = NULL,
    @p_minimum_experience    INT           = NULL,
    @p_maximum_experience    INT           = NULL,
    @p_scolarity_id          INT           = NULL,
    @p_skills_profile        INT           = NULL,
    @p_excluded              NVARCHAR(MAX) = NULL,
    @p_included              NVARCHAR(MAX) = NULL,
    @p_sources               INT           = NULL,
    @p_cv_max_age            NVARCHAR(MAX) = NULL,
    @p_request_cooldown_ms   INT           = 5000
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @p_vacancy_id IS NULL
        BEGIN
            INSERT INTO recruitment.vacancies (
                organization_id,
                profile_id,
                criticality_level_id,
                vacancy_status_id,
                vacancy_title,
                client_name,
                project_name,
                vacancy_location,
                position_count,
                salary_range_min,
                salary_range_max,
                request_date,
                deadline_date,
                modality_id,
                contract_type_id,
                currency_id,
                pay_frequency_id,
                notes,
                is_enabled,
                minimum_experience,
                maximum_experience,
                scolarity_id,
                skills_profile,
                excluded,
                included,
                sources,
                cv_max_age,
                request_cooldown_ms,
                created_by
            )
            VALUES (
                @p_organization_id,
                @p_profile_id,
                @p_criticality_level_id,
                @p_vacancy_status_id,
                @p_vacancy_title,
                @p_client_name,
                @p_project_name,
                @p_vacancy_location,
                ISNULL(@p_position_count, 1),
                @p_salary_range_min,
                @p_salary_range_max,
                @p_request_date,
                @p_deadline_date,
                @p_modality_id,
                @p_contract_type_id,
                @p_currency_id,
                @p_pay_frequency_id,
                @p_notes,
                1,
                @p_minimum_experience,
                @p_maximum_experience,
                @p_scolarity_id,
                @p_skills_profile,
                @p_excluded,
                @p_included,
                @p_sources,
                @p_cv_max_age,
                ISNULL(@p_request_cooldown_ms, 5000),
                @p_created_by
            );

            SELECT SCOPE_IDENTITY() AS vacancy_id;
        END
        ELSE
        BEGIN
            UPDATE recruitment.vacancies
            SET
                profile_id            = ISNULL(@p_profile_id,           profile_id),
                criticality_level_id  = ISNULL(@p_criticality_level_id, criticality_level_id),
                vacancy_status_id     = ISNULL(@p_vacancy_status_id,    vacancy_status_id),
                vacancy_title         = ISNULL(@p_vacancy_title,        vacancy_title),
                client_name           = ISNULL(@p_client_name,          client_name),
                project_name          = ISNULL(@p_project_name,         project_name),
                vacancy_location      = ISNULL(@p_vacancy_location,     vacancy_location),
                position_count        = ISNULL(@p_position_count,       position_count),
                salary_range_min      = ISNULL(@p_salary_range_min,     salary_range_min),
                salary_range_max      = ISNULL(@p_salary_range_max,     salary_range_max),
                request_date          = ISNULL(@p_request_date,         request_date),
                deadline_date         = ISNULL(@p_deadline_date,        deadline_date),
                modality_id           = ISNULL(@p_modality_id,          modality_id),
                contract_type_id      = ISNULL(@p_contract_type_id,     contract_type_id),
                currency_id           = ISNULL(@p_currency_id,          currency_id),
                pay_frequency_id      = ISNULL(@p_pay_frequency_id,     pay_frequency_id),
                notes                 = ISNULL(@p_notes,                notes),
                minimum_experience    = ISNULL(@p_minimum_experience,   minimum_experience),
                maximum_experience    = ISNULL(@p_maximum_experience,   maximum_experience),
                scolarity_id          = ISNULL(@p_scolarity_id,         scolarity_id),
                skills_profile        = ISNULL(@p_skills_profile,       skills_profile),
                excluded              = ISNULL(@p_excluded,             excluded),
                included              = ISNULL(@p_included,             included),
                sources               = ISNULL(@p_sources,              sources),
                cv_max_age            = ISNULL(@p_cv_max_age,           cv_max_age),
                request_cooldown_ms   = ISNULL(@p_request_cooldown_ms,  5000),
                updated_at            = SYSUTCDATETIME(),
                updated_by            = @p_updated_by
            WHERE vacancy_id = @p_vacancy_id;

            SELECT @p_vacancy_id AS vacancy_id;
        END
    END TRY
    BEGIN CATCH
        DECLARE @v_user_name NVARCHAR(100);
        DECLARE @v_error_number INT = ERROR_NUMBER();
        DECLARE @v_error_state INT = ERROR_STATE();
        DECLARE @v_error_severity INT = ERROR_SEVERITY();
        DECLARE @v_error_line INT = ERROR_LINE();
        DECLARE @v_error_procedure NVARCHAR(200);
        DECLARE @v_error_message NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @v_error_param NVARCHAR(MAX);
        DECLARE @v_error_number_app_id INT;

        EXEC dbo.proc_log_errors @p_user_name_db = @v_user_name,
            @p_error_number_db = @v_error_number,
            @p_error_state_db = @v_error_state,
            @p_error_severity_db = @v_error_severity,
            @p_error_line_db = @v_error_line,
            @p_error_procedure_db = @v_error_procedure,
            @p_error_param_db = @v_error_param,
            @p_error_message_db = @v_error_message,
            @p_error_number_app_id = @v_error_number_app_id;

        IF XACT_STATE() IN (-1, 1) ROLLBACK;

        THROW;
    END CATCH

    SET NOCOUNT OFF;
END;
