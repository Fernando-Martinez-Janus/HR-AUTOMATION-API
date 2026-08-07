CREATE OR ALTER PROCEDURE [recruitment].[web_get_vacancy_dispatch]
    @p_created_by        INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @max_cvs INT = 20;
    DECLARE @min_match_score INT = 60;

    BEGIN TRY
        IF @p_created_by IS NULL THROW 50001, 'created_by es requerido.', 1;

        DECLARE @target_v_id INT;

        BEGIN TRANSACTION

        SELECT TOP 1
            @target_v_id = v.[vacancy_id]
        FROM [recruitment].[vacancies] v
        INNER JOIN [config].[criticality_levels] cl ON v.[criticality_level_id] = cl.[criticality_level_id]
        WHERE (
              v.[vacancy_status_id] < 3
              OR 
              (v.[vacancy_status_id] = 3 
               AND v.[date_until] IS NOT NULL
               AND SYSUTCDATETIME() >= V.[date_until])
          )
          AND v.[is_enabled] = 1
        ORDER BY cl.[sort_order] DESC, v.[deadline_date] ASC;

        IF @target_v_id IS NOT NULL
            UPDATE [recruitment].[vacancies] SET
                [vacancy_status_id] = 3,
                [date_until] = DATEADD(MINUTE, 10, SYSUTCDATETIME()),
                [updated_at] = SYSUTCDATETIME(),
                [updated_by] = @p_created_by
            WHERE [vacancy_id] = @target_v_id;

        WITH previous_cte AS (
            SELECT
                sr.[vacancy_id],
                JSON_QUERY(
                    COALESCE(
                        '[' + STRING_AGG(
                            JSON_OBJECT(
                                'candidate_title': rslt.[candidate_title],
                                'reference_link': rslt.[reference_link]
                            ),
                            ','
                        ) + ']',
                        '[]'
                    )
                ) AS [previous_candidates]
            FROM [recruitment].[search_requests] sr
            INNER JOIN [recruitment].[search_results] rslt
                ON sr.[search_request_id] = rslt.[search_request_id]
            WHERE rslt.[is_enabled] = 1
            GROUP BY sr.[vacancy_id]
        )
        SELECT TOP 1
            sr.[search_request_id],
            v.[vacancy_id],
            v.[vacancy_title],
            v.[client_name],
            v.[project_name],
            v.[vacancy_location],
            v.[deadline_date],
            v.[criticality_level_id],
            cl.[level_name]             AS [criticality_level_name],
            cl.[sort_order]             AS [criticality_sort_order],
            v.[profile_id],
            p.[profile_name],
            p.[seniority_level_id],
            snl.[seniority_name]        AS [seniority_level_name],
            p.[area_level_id],
            al.[level_name]             AS [area_level_name],
            v.[minimum_experience],
            v.[maximum_experience],
            v.[scolarity_id],
            scol.[level_name]           AS [scolarity_name],
            v.[modality_id],
            wm.[modality_name]          AS [work_modality],
            v.[contract_type_id],
            et.[type_name]              AS [employment_type],
            v.[salary_range_min],
            v.[salary_range_max],
            COALESCE(v.[skills_profile], '[]') AS [skills_profile],
            v.[excluded],
            v.[included],
            v.[cv_max_age]              AS [max_profile_age_days],
            v.[request_cooldown_ms],
            @max_cvs                    AS [max_cvs],
            @min_match_score            AS [min_match_score],
            CASE 
                WHEN v.[sources] IS NOT NULL AND ISJSON(v.[sources]) = 1 
                THEN JSON_QUERY(v.[sources])
                ELSE NULL 
            END AS [sources],
            COALESCE(pc.[previous_candidates], '[]') AS [previous_candidates]
        FROM [recruitment].[vacancies] v
        INNER JOIN [recruitment].[profiles] p ON v.[profile_id] = p.[profile_id]
        INNER JOIN [config].[criticality_levels] cl ON v.[criticality_level_id] = cl.[criticality_level_id]
        LEFT JOIN [config].[seniority_levels] snl ON p.[seniority_level_id] = snl.[seniority_level_id]
        LEFT JOIN [config].[area_levels] al ON p.[area_level_id] = al.[area_level_id]
        LEFT JOIN [config].[scolarity_levels] scol ON v.[scolarity_id] = scol.[scolarity_level_id]
        LEFT JOIN [config].[work_modalities] wm ON v.[modality_id] = wm.[work_modality_id]
        LEFT JOIN [config].[employment_types] et ON v.[contract_type_id] = et.[employment_type_id]
        LEFT JOIN previous_cte pc ON v.[vacancy_id] = pc.[vacancy_id]
        LEFT JOIN [recruitment].[search_requests] sr ON v.[vacancy_id] = sr.[vacancy_id]
        WHERE v.[vacancy_id] = @target_v_id
        
        COMMIT TRANSACTION

    END TRY
    BEGIN CATCH
        DECLARE @v_user_name NVARCHAR(100);
        DECLARE @v_error_number INT = ERROR_NUMBER();
        DECLARE @v_error_state INT = ERROR_STATE();
        DECLARE @v_error_severity INT = ERROR_SEVERITY();
        DECLARE @v_error_line INT = ERROR_LINE();
        DECLARE @v_error_procedure NVARCHAR(200) = ERROR_PROCEDURE();
        DECLARE @v_error_message NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @v_error_param NVARCHAR(MAX);
        DECLARE @v_error_number_app_id INT;

        IF XACT_STATE() <> -1 ROLLBACK TRANSACTION;
        IF XACT_STATE() = 1 COMMIT TRANSACTION;

        EXEC [dbo].[proc_log_errors]
            @p_user_name_db = @v_user_name,
            @p_error_number_db = @v_error_number,
            @p_error_state_db = @v_error_state,
            @p_error_severity_db = @v_error_severity,
            @p_error_line_db = @v_error_line,
            @p_error_procedure_db = @v_error_procedure,
            @p_error_param_db = @v_error_param,
            @p_error_message_db = @v_error_message,
            @p_error_number_app_id = @v_error_number_app_id;

        THROW;
    END CATCH
    SET NOCOUNT OFF;
END;
