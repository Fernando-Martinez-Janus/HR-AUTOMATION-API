using Asp.Versioning;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION_API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing vacancies.
    /// </summary>
    /// <param name="vacancyService">Instance of Vacancy service.</param>
    [ApiController]
    [Produces(MediaTypes.Json)]
    [EnableRateLimiting(RateLimitConstants.DefaultPolicy)]
    [Tags("Vacancies")]
    [Route("api/v{version:apiVersion}/vacancies")]
    public class VacancyController(IVacancyService vacancyService) : ControllerBase
    {
        private readonly IVacancyService _vacancyService = vacancyService;

        /// <summary>
        /// Retrieves vacancies matching the specified search criteria.
        /// </summary>
        /// <param name="model">The search criteria.</param>
        /// <returns>A paginated collection of matching vacancies.</returns>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<PaginationResponse<VacancyViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] VacancySearchInputModel model)
        {
            PaginationResponse<VacancyViewModel> result = await _vacancyService.SearchAsync(model);

            Response<PaginationResponse<VacancyViewModel>> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Retrieves a vacancy by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the vacancy.</param>
        /// <returns>The requested <see cref="VacancyViewModel"/>.</returns>
        /// <exception cref="ResponseExceptionFactory">Thrown when the specified vacancy does not exist.</exception>
        [HttpGet("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<VacancyViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            VacancyViewModel result = await _vacancyService.GetAsync(id);

            Response<VacancyViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates a new vacancy.
        /// </summary>
        /// <param name="model">The vacancy information.</param>
        /// <returns>The identifier of the newly created vacancy.</returns>
        [HttpPost]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] VacancyInputModel model)
        {
            int result = await _vacancyService.CreateAsync(model);

            Response<int> response = new()
            {
                Code = StatusCodes.Status201Created,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Creates or updates a vacancy (draft). If the vacancy identifier is provided, the existing vacancy is updated; otherwise, a new one is created.
        /// </summary>
        /// <param name="model">The vacancy information.</param>
        /// <returns>The updated <see cref="VacancyViewModel"/>.</returns>
        [HttpPost("draft")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<VacancyViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upsert([FromBody] VacancyInputModel model)
        {
            VacancyViewModel result = await _vacancyService.UpsertAsync(model);

            Response<VacancyViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Updates an existing vacancy.
        /// </summary>
        /// <param name="id">The identifier of the vacancy to update.</param>
        /// <param name="model">The updated vacancy information.</param>
        [HttpPut("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] VacancyInputModel model)
        {
            await _vacancyService.UpdateAsync(id, model);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Deletes an existing vacancy.
        /// </summary>
        /// <param name="id">The identifier of the vacancy to delete.</param>
        [HttpDelete("{id:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _vacancyService.DeleteAsync(id);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{vacancyId:int}/sourcing-searches")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateSourcingSearch(int vacancyId, [FromBody] ActiveSearchInputModel model)
        {
            await Task.Delay(0);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("{vacancyId:int}/sourcing-results")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<ActiveSearchViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSourcingResults(int vacancyId, [FromQuery] SourcingResultSearchInputModel model)
        {
            await Task.Delay(0);
            ActiveSearchViewModel result = new()
            {
                UnSeenCount = 0,
                Result = new(1,
                    [
                        new()
                        {
                            SearchRequestId = 1,
                            CandidateTitle = "Desarrollador Full Stack (.NET Core / Angular)",
                            AiScore = 85,
                            AiRecommended = true,
                            AiShortComment = "Desarrollador Full Stack con experiencia en .NET, Angular, React, Vue, C#, APIs REST y bases de datos, actualmente trabajando en soluciones para el sector seguros. Ha participado en desarrollo frontend, servicios web, optimización de procesos y pruebas manuales con Postman. Cuenta con licenciatura en Informática Administrativa e inglés básico.",
                            AiExtendedComment = "",
                            ReferenceLink = "https://www.occ.com.mx/empresas/candidatos/cv/12203967?o=4",
                            OriginalResumeLink = "", //"Candidato con perfil de Desarrollador Full Stack (.NET Core / Angular) ubicado en Monterrey, Nuevo León. Actualmente se desempeña como desarrollador de software en Raspberry Seguros, donde ha trabajado en soluciones personalizadas para seguros, mejora de procesos, reducción de errores de validación, optimización de aplicaciones de gestión de pólizas y diseño de interfaces para cotizadores.\nPreviamente trabajó como Desarrollador Jr en Connectit, participando en la implementación de interfaces, consumo y desarrollo de servicios web, APIs REST, modificaciones en bases de datos y pruebas manuales con Postman. Su experiencia técnica incluye tecnologías como HTML, CSS, JavaScript, Vue, React, Angular, C#, .NET, Python Flask y MySQL.\nSu objetivo profesional está orientado a integrarse a equipos colaborativos donde pueda aportar valor técnico, resolver problemas complejos y contribuir a productos eficientes, escalables y de calidad. Cuenta con estudios titulados en Licenciatura en Informática Administrativa por la Universidad Ciudadana de Nuevo León e idiomas: español nativo e inglés básico.",
                            CreatedAt = DateTime.UtcNow,
                            Seen = true
                        }
                    ]
                )
            };

            Response<ActiveSearchViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyId"></param>
        /// <param name="resultId"></param>
        /// <returns></returns>
        [HttpGet("{vacancyId:int}/sourcing-results/{resultId:int}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response<SearchResultViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResult(int vacancyId, int resultId)
        {
            SearchResultViewModel result = new()
            {
                SearchRequestId = 1,
                CandidateTitle = "Desarrollador Full Stack (.NET Core / Angular)",
                AiScore = 85,
                AiRecommended = true,
                AiShortComment = "Desarrollador Full Stack con experiencia en .NET, Angular, React, Vue, C#, APIs REST y bases de datos, actualmente trabajando en soluciones para el sector seguros. Ha participado en desarrollo frontend, servicios web, optimización de procesos y pruebas manuales con Postman. Cuenta con licenciatura en Informática Administrativa e inglés básico.",
                AiExtendedComment = "Candidato con perfil de Desarrollador Full Stack (.NET Core / Angular) ubicado en Monterrey, Nuevo León. Actualmente se desempeña como desarrollador de software en Raspberry Seguros, donde ha trabajado en soluciones personalizadas para seguros, mejora de procesos, reducción de errores de validación, optimización de aplicaciones de gestión de pólizas y diseño de interfaces para cotizadores.\nPreviamente trabajó como Desarrollador Jr en Connectit, participando en la implementación de interfaces, consumo y desarrollo de servicios web, APIs REST, modificaciones en bases de datos y pruebas manuales con Postman. Su experiencia técnica incluye tecnologías como HTML, CSS, JavaScript, Vue, React, Angular, C#, .NET, Python Flask y MySQL.\nSu objetivo profesional está orientado a integrarse a equipos colaborativos donde pueda aportar valor técnico, resolver problemas complejos y contribuir a productos eficientes, escalables y de calidad. Cuenta con estudios titulados en Licenciatura en Informática Administrativa por la Universidad Ciudadana de Nuevo León e idiomas: español nativo e inglés básico.",
                ReferenceLink = "https://www.occ.com.mx/empresas/candidatos/cv/12203967?o=4",
                OriginalResumeLink = "",
                CreatedAt = DateTime.UtcNow,
                Seen = true
            };

            Response<SearchResultViewModel> response = new()
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyId"></param>
        /// <param name="resultId"></param>
        /// <returns></returns>
        [HttpPatch("{vacancyId:int}/sourcing-results/{resultId:int}/viewed")]
        [MapToApiVersion("1")]
        [ProducesResponseType(typeof(Response), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SetResultAsViewed(int vacancyId, int resultId)
        {
            await Task.Delay(0);

            Response response = new()
            {
                Code = StatusCodes.Status204NoContent
            };

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyId"></param>
        /// <param name="resultId"></param>
        /// <returns></returns>
        [HttpGet("{vacancyId:int}/sourcing-results/{resultId:int}/cv")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResultCv(int vacancyId, int resultId)
        {
            string base64 = "JVBERi0xLjMKJbrfrOAKMyAwIG9iago8PC9UeXBlIC9QYWdlCi9QYXJlbnQgMSAwIFIKL1Jlc291cmNlcyAyIDAgUgovTWVkaWFCb3ggWzAgMCA2MTIuIDc5Mi5dCi9Db250ZW50cyA0IDAgUgo+PgplbmRvYmoKNCAwIG9iago8PAovTGVuZ3RoIDM3MzYKPj4Kc3RyZWFtCjAuMjAwMDI1IHcKMCBHCnEKMjAuIDAgMCAxNi4gNDAuIDEyLiBjbQovSTAgRG8KUQpCVAovRjEgMTAgVGYKMTEuNSBUTAowLjQgZwo2NC4gMjIuIFRkCih8IHd3dy5vY2MuY29tLm14KSBUagpFVApCVAovRjIgMTYgVGYKMTguMzk5OTk5OTk5OTk5OTk4NiBUTAowLiBnCjQwLiA3MzIuIFRkCihDdXJy7WN1bG8gZGUgRGVzYXJyb2xsYWRvciBGdWxsIFN0YWNrIFwoLk5FVCBDb3JlIC8gQW5ndWxhclwpKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA3MTYuIFRkCihNb250ZXJyZXksIE51ZXZvIExl824pIFRqCkVUCjAuODYgRwo0MC4gNjg2LiBtCjU3Mi4gNjg2LiBsClMKQlQKL0YyIDEyIFRmCjEzLjc5OTk5OTk5OTk5OTk5ODkgVEwKMC4gZwo0MC4gNjU0LiBUZAooRXhwZXJpZW5jaWEgcHJvZmVzaW9uYWwpIFRqCkVUCkJUCi9GMiAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDYyOC4gVGQKKC0gRGVzYXJyb2xsYWRvciBkZSBzb2Z0d2FyZSBlbiBSQVNQQkVSUlkgU0VHVVJPUykgVGoKRVQKQlQKL0YxIDEwIFRmCjExLjUgVEwKMC4gZwo0MC4gNjE0LiBUZAooc2VwdGllbWJyZSAyMDIzIC0gQWN0dWFsKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA2MDAuIFRkCihEZXNhcnJvbGzpIHNvbHVjaW9uZXMgcGVyc29uYWxpemFkYXMgcGFyYSBzZWd1cm9zLCBvcHRpbWl6YW5kbyBwcm9jZXNvcyB5IHJlZHVjaWVuZG8gc2lnbmlmaWNhdGl2YW1lbnRlKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA1ODYuIFRkCihlcnJvcmVzIGRlIHZhbGlkYWNp824uIE1lam9y6SBsYSBlZmljaWVuY2lhIGRlIGFwbGljYWNpb25lcyBkZSBnZXN0afNuIGRlIHDzbGl6YXMsIGFkZW3hcyBkZSBkaXNl8WFyKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA1NzIuIFRkCihpbnRlcmZhY2VzIGRlIHVzdWFyaW8gaW50dWl0aXZhcyBwYXJhIGNvdGl6YWRvcmVzIGRlIHNlZ3Vyb3MuKSBUagpFVApCVAovRjIgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA1NDIuIFRkCigtIERlc2Fycm9sbGFkb3IganIgZW4gQ29ubmVjdGl0KSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA1MjguIFRkCihhYnJpbCAyMDIyIC0gc2VwdGllbWJyZSAyMDIzKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA1MTQuIFRkCigtIEltcGxlbWVudGFjafNuIGRlIGxhIHBhcnRlIGRlbCBjbGllbnRlIGVuIGxvcyBzZXJ2aWNpb3MgZGUgbGEgZW1wcmVzYSwgZGVzZGUgbGEgY3JlYWNp824geSBkaXNl8W8gZGUgbGEpIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDUwMC4gVGQKKGludGVyZmF6IGhhc3RhIGxhIGltcGxlbWVudGFjafNuIGRlIHNlcnZpY2lvcyB3ZWIgeSBmdW5jaW9uYWxpZGFkLCBjb24gdGVjbm9sb2ftYXMgY29tbzogSFRNTCwgQ1NTLCBKUywpIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDQ4Ni4gVGQKKFZVRSwgUkVBQ1QsIEFOR1VMQVIsIEMjLCAuTkVUIGV0Yy4gLSBEZXNhcnJvbGxvIGRlIHNlcnZpY2lvcyBwYXJhIEFQSXMgUkVTVCB5IG1vZGlmaWNhY2nzbiBkZSBiYXNlcykgVGoKRVQKQlQKL0YxIDEwIFRmCjExLjUgVEwKMC4gZwo0MC4gNDcyLiBUZAooZGUgZGF0b3MgYmFzYWRvcyBlbiByZXF1ZXJpbWllbnRvcyBkZWwgY2xpZW50ZSwgcHJpbmNpcGFsbWVudGUgY29uIFB5dGhvbiBcKEZsYXNrXCkgeSBNeVNRTFwoSGVpZGlTUUxcKS4gLSkgVGoKRVQKQlQKL0YxIDEwIFRmCjExLjUgVEwKMC4gZwo0MC4gNDU4LiBUZAooUHJ1ZWJhIG1hbnVhbCBkZSBzZXJ2aWNpb3MgZGVzYXJyb2xsYWRvcyB1c2FuZG8gbGEgaGVycmFtaWVudGEgUG9zdG1hbikgVGoKRVQKQlQKL0YyIDEyIFRmCjEzLjc5OTk5OTk5OTk5OTk5ODkgVEwKMC4gZwo0MC4gNDIwLiBUZAooT2JqZXRpdm8gcHJvZmVzaW9uYWwpIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDM5NC4gVGQKKFB1ZXN0bzogRGVzYXJyb2xsYWRvciBGdWxsIFN0YWNrIFwoLk5FVCBDb3JlIC8gQW5ndWxhclwpKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiAzODAuIFRkCihTYWxhcmlvIGFjdHVhbCBhcHJveGltYWRvOiAkMjUsMDAwICkgVGoKRVQKQlQKL0YxIDEwIFRmCjExLjUgVEwKMC4gZwo0MC4gMzU4LiBUZAooRGVzYXJyb2xsYWRvciBkZSBzb2Z0d2FyZSBlbmZvY2FkbyBlbiBsYSBjcmVhY2nzbiBkZSBzb2x1Y2lvbmVzIGVmaWNpZW50ZXMsIGVzY2FsYWJsZXMgeSBkZSBhbHRhIGNhbGlkYWQpIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDM0NC4gVGQKKG1lZGlhbnRlIGVsIHVzbyBkZSBtZXRvZG9sb2ftYXMg4WdpbGVzIHkgYnVlbmFzIHBy4WN0aWNhcyBkZSBkZXNhcnJvbGxvLiBCdXNjbyBpbnRlZ3Jhcm1lIGEgdW4gZXF1aXBvKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiAzMzAuIFRkCihjb2xhYm9yYXRpdm8gZG9uZGUgcHVlZGEgYXBvcnRhciB2YWxvciB06WNuaWNvLCByZXNvbHZlciBwcm9ibGVtYXMgY29tcGxlam9zIHkgY29udHJpYnVpciBhbCBjdW1wbGltaWVudG8gZGUpIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDMxNi4gVGQKKG9iamV0aXZvcyBlc3RyYXTpZ2ljb3MgZGVsIG5lZ29jaW8uICBNZSBtb3RpdmEgdHJhYmFqYXIgZW4gZW50b3Jub3MgcXVlIHByb211ZXZhbiBsYSBtZWpvcmEgY29udGludWEsIGVsKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiAzMDIuIFRkCihhcHJlbmRpemFqZSBjb25zdGFudGUgeSBsYSBpbm5vdmFjafNuIHRlY25vbPNnaWNhLCBtYW50ZW5pZW5kbyBzaWVtcHJlIHVuIGVuZm9xdWUgZW4gcmVzdWx0YWRvcywpIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDI4OC4gVGQKKHJlbmRpbWllbnRvIHkgY2FsaWRhZCBkZWwgcHJvZHVjdG8uKSBUagpFVApCVAovRjIgMTIgVGYKMTMuNzk5OTk5OTk5OTk5OTk4OSBUTAowLiBnCjQwLiAyNTAuIFRkCijBcmVhIGRlIGVzcGVjaWFsaWRhZCkgVGoKRVQKQlQKL0YxIDEwIFRmCjExLjUgVEwKMC4gZwo0MC4gMjI0LiBUZAooVGVjbm9sb2ftYXMgZGUgbGEgSW5mb3JtYWNp824gLSBTaXN0ZW1hcyBcKERlc2Fycm9sbG8gZGUgc29mdHdhcmUgLSBQcm9ncmFtYWRvclwpLCBUZWNub2xvZ+1hcyBkZSBsYSkgVGoKRVQKQlQKL0YxIDEwIFRmCjExLjUgVEwKMC4gZwo0MC4gMjEwLiBUZAooSW5mb3JtYWNp824gLSBTaXN0ZW1hcyBcKEluZm9ybeF0aWNhXCksIFRlY25vbG9n7WFzIGRlIGxhIEluZm9ybWFjafNuIC0gU2lzdGVtYXMgXChEZXNhcnJvbGxvIHdlYlwpKSBUagpFVApCVAovRjIgMTIgVGYKMTMuNzk5OTk5OTk5OTk5OTk4OSBUTAowLiBnCjQwLiAxNzIuIFRkCihIYWJpbGlkYWRlcykgVGoKRVQKQlQKL0YxIDEwIFRmCjExLjUgVEwKMC4gZwo0MC4gMTQ2LiBUZAoobmV0IGZyYW1ld29yaywgYW5ndWxhciBtYXRlcmlhbCwgYW5ndWxhciwgUGhwNSwgTmV0IGMjLCBQcm9ncmFtYWNpb24sIE5ldCBkZXZlbG9wZXIsIC5ORVQsIEMrKywgQyMpIFRqCkVUCjAuODYgRwo0MC4gMTI0LiBtCjU3Mi4gMTI0LiBsClMKQlQKL0YyIDEyIFRmCjEzLjc5OTk5OTk5OTk5OTk5ODkgVEwKMC4gZwo0MC4gOTIuIFRkCihFZHVjYWNp824pIFRqCkVUCkJUCi9GMiAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDY2LiBUZAooLSBVbml2ZXJzaWRhZCBDaXVkYWRhbmEgZGUgTnVldm8gTGXzbikgVGoKRVQKZW5kc3RyZWFtCmVuZG9iago1IDAgb2JqCjw8L1R5cGUgL1BhZ2UKL1BhcmVudCAxIDAgUgovUmVzb3VyY2VzIDIgMCBSCi9NZWRpYUJveCBbMCAwIDYxMi4gNzkyLl0KL0NvbnRlbnRzIDYgMCBSCj4+CmVuZG9iago2IDAgb2JqCjw8Ci9MZW5ndGggOTQ5Cj4+CnN0cmVhbQowLjIwMDAyNSB3CjAuODYgRwpxCjIwLiAwIDAgMTYuIDQwLiAxMi4gY20KL0kwIERvClEKQlQKL0YyIDEwIFRmCjExLjUgVEwKMC40IGcKNjQuIDIyLiBUZAoofCB3d3cub2NjLmNvbS5teCkgVGoKRVQKQlQKL0YxIDEwIFRmCjExLjUgVEwKMC4gZwo0MC4gNzMyLiBUZAooMjAyMSAtIDIwMjMpIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDcxOC4gVGQKKEVzdHVkaW9zIFVuaXZlcnNpdGFyaW9zIC0gdGl0dWxhZG8gZW4gTGljZW5jaWF0dXJhIGVuIEluZm9ybWF0aWNhIEFkbWluaXN0cmF0aXZhKSBUagpFVApCVAovRjIgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA2ODguIFRkCigtIFVuaXZlcnNpZGFkIEF1dPNub21hIGRlIE51ZXZvIExl824pIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDY3NC4gVGQKKDIwMTUgLSAyMDE5KSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA2NjAuIFRkCihFc3R1ZGlvcyBVbml2ZXJzaXRhcmlvcyAtIG5vIHRpdHVsYWRvIGVuIExpY2VuY2lhdHVyYSBlbiBjaWVuY2lhcyBkZWwgZWplcmNpY2lvKSBUagpFVApCVAovRjIgMTIgVGYKMTMuNzk5OTk5OTk5OTk5OTk4OSBUTAowLiBnCjQwLiA2MjIuIFRkCihJZGlvbWFzKSBUagpFVApCVAovRjEgMTAgVGYKMTEuNSBUTAowLiBnCjQwLiA1OTYuIFRkCihJbmds6XMgQuFzaWNvLCBFc3Bh8W9sIExlbmd1YSBuYXRpdmEpIFRqCkVUCjAuODYgRwo0MC4gNTc0LiBtCjU3Mi4gNTc0LiBsClMKQlQKL0YyIDEyIFRmCjEzLjc5OTk5OTk5OTk5OTk5ODkgVEwKMC4gZwo0MC4gNTQyLiBUZAooTGlnYSBkZSBjdXJy7WN1bG8pIFRqCkVUCkJUCi9GMSAxMCBUZgoxMS41IFRMCjAuIGcKNDAuIDUxNi4gVGQKKGh0dHBzOi8vd3d3Lm9jYy5jb20ubXgvZW1wcmVzYXMvY2FuZGlkYXRvcy9jdi8xMjIwMzk2Nz9vPTQmdXRtX3NvdXJjZT1wZGYpIFRqCkVUCmVuZHN0cmVhbQplbmRvYmoKMSAwIG9iago8PC9UeXBlIC9QYWdlcwovS2lkcyBbMyAwIFIgNSAwIFIgXQovQ291bnQgMgo+PgplbmRvYmoKNyAwIG9iago8PAovVHlwZSAvRm9udAovQmFzZUZvbnQgL0hlbHZldGljYQovU3VidHlwZSAvVHlwZTEKL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcKL0ZpcnN0Q2hhciAzMgovTGFzdENoYXIgMjU1Cj4+CmVuZG9iago4IDAgb2JqCjw8Ci9UeXBlIC9Gb250Ci9CYXNlRm9udCAvSGVsdmV0aWNhLUJvbGQKL1N1YnR5cGUgL1R5cGUxCi9FbmNvZGluZyAvV2luQW5zaUVuY29kaW5nCi9GaXJzdENoYXIgMzIKL0xhc3RDaGFyIDI1NQo+PgplbmRvYmoKOSAwIG9iago8PAovVHlwZSAvRm9udAovQmFzZUZvbnQgL0hlbHZldGljYS1PYmxpcXVlCi9TdWJ0eXBlIC9UeXBlMQovRW5jb2RpbmcgL1dpbkFuc2lFbmNvZGluZwovRmlyc3RDaGFyIDMyCi9MYXN0Q2hhciAyNTUKPj4KZW5kb2JqCjEwIDAgb2JqCjw8Ci9UeXBlIC9Gb250Ci9CYXNlRm9udCAvSGVsdmV0aWNhLUJvbGRPYmxpcXVlCi9TdWJ0eXBlIC9UeXBlMQovRW5jb2RpbmcgL1dpbkFuc2lFbmNvZGluZwovRmlyc3RDaGFyIDMyCi9MYXN0Q2hhciAyNTUKPj4KZW5kb2JqCjExIDAgb2JqCjw8Ci9UeXBlIC9Gb250Ci9CYXNlRm9udCAvQ291cmllcgovU3VidHlwZSAvVHlwZTEKL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcKL0ZpcnN0Q2hhciAzMgovTGFzdENoYXIgMjU1Cj4+CmVuZG9iagoxMiAwIG9iago8PAovVHlwZSAvRm9udAovQmFzZUZvbnQgL0NvdXJpZXItQm9sZAovU3VidHlwZSAvVHlwZTEKL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcKL0ZpcnN0Q2hhciAzMgovTGFzdENoYXIgMjU1Cj4+CmVuZG9iagoxMyAwIG9iago8PAovVHlwZSAvRm9udAovQmFzZUZvbnQgL0NvdXJpZXItT2JsaXF1ZQovU3VidHlwZSAvVHlwZTEKL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcKL0ZpcnN0Q2hhciAzMgovTGFzdENoYXIgMjU1Cj4+CmVuZG9iagoxNCAwIG9iago8PAovVHlwZSAvRm9udAovQmFzZUZvbnQgL0NvdXJpZXItQm9sZE9ibGlxdWUKL1N1YnR5cGUgL1R5cGUxCi9FbmNvZGluZyAvV2luQW5zaUVuY29kaW5nCi9GaXJzdENoYXIgMzIKL0xhc3RDaGFyIDI1NQo+PgplbmRvYmoKMTUgMCBvYmoKPDwKL1R5cGUgL0ZvbnQKL0Jhc2VGb250IC9UaW1lcy1Sb21hbgovU3VidHlwZSAvVHlwZTEKL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcKL0ZpcnN0Q2hhciAzMgovTGFzdENoYXIgMjU1Cj4+CmVuZG9iagoxNiAwIG9iago8PAovVHlwZSAvRm9udAovQmFzZUZvbnQgL1RpbWVzLUJvbGQKL1N1YnR5cGUgL1R5cGUxCi9FbmNvZGluZyAvV2luQW5zaUVuY29kaW5nCi9GaXJzdENoYXIgMzIKL0xhc3RDaGFyIDI1NQo+PgplbmRvYmoKMTcgMCBvYmoKPDwKL1R5cGUgL0ZvbnQKL0Jhc2VGb250IC9UaW1lcy1JdGFsaWMKL1N1YnR5cGUgL1R5cGUxCi9FbmNvZGluZyAvV2luQW5zaUVuY29kaW5nCi9GaXJzdENoYXIgMzIKL0xhc3RDaGFyIDI1NQo+PgplbmRvYmoKMTggMCBvYmoKPDwKL1R5cGUgL0ZvbnQKL0Jhc2VGb250IC9UaW1lcy1Cb2xkSXRhbGljCi9TdWJ0eXBlIC9UeXBlMQovRW5jb2RpbmcgL1dpbkFuc2lFbmNvZGluZwovRmlyc3RDaGFyIDMyCi9MYXN0Q2hhciAyNTUKPj4KZW5kb2JqCjE5IDAgb2JqCjw8Ci9UeXBlIC9Gb250Ci9CYXNlRm9udCAvWmFwZkRpbmdiYXRzCi9TdWJ0eXBlIC9UeXBlMQovRmlyc3RDaGFyIDMyCi9MYXN0Q2hhciAyNTUKPj4KZW5kb2JqCjIwIDAgb2JqCjw8Ci9UeXBlIC9Gb250Ci9CYXNlRm9udCAvU3ltYm9sCi9TdWJ0eXBlIC9UeXBlMQovRmlyc3RDaGFyIDMyCi9MYXN0Q2hhciAyNTUKPj4KZW5kb2JqCjIxIDAgb2JqCjw8Ci9UeXBlIC9YT2JqZWN0Ci9TdWJ0eXBlIC9JbWFnZQovV2lkdGggODAKL0hlaWdodCA2NAovQ29sb3JTcGFjZSAvRGV2aWNlUkdCCi9CaXRzUGVyQ29tcG9uZW50IDgKL0xlbmd0aCAyNTEwCi9GaWx0ZXIgL0RDVERlY29kZQo+PgpzdHJlYW0K/9j/4AAQSkZJRgABAQAAAQABAAD/4gHYSUNDX1BST0ZJTEUAAQEAAAHIAAAAAAQwAABtbnRyUkdCIFhZWiAH4AABAAEAAAAAAABhY3NwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAA9tYAAQAAAADTLQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAlkZXNjAAAA8AAAACRyWFlaAAABFAAAABRnWFlaAAABKAAAABRiWFlaAAAB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UgovT3BlbkFjdGlvbiBbMyAwIFIgL0ZpdEggbnVsbF0KL1BhZ2VMYXlvdXQgL09uZUNvbHVtbgo+PgplbmRvYmoKeHJlZgowIDI0CjAwMDAwMDAwMDAgNjU1MzUgZiAKMDAwMDAwNTAxMyAwMDAwMCBuIAowMDAwMDA5NTE1IDAwMDAwIG4gCjAwMDAwMDAwMTUgMDAwMDAgbiAKMDAwMDAwMDEyMCAwMDAwMCBuIAowMDAwMDAzOTA4IDAwMDAwIG4gCjAwMDAwMDQwMTMgMDAwMDAgbiAKMDAwMDAwNTA3NiAwMDAwMCBuIAowMDAwMDA1MjAxIDAwMDAwIG4gCjAwMDAwMDUzMzEgMDAwMDAgbiAKMDAwMDAwNTQ2NCAwMDAwMCBuIAowMDAwMDA1NjAyIDAwMDAwIG4gCjAwMDAwMDU3MjYgMDAwMDAgbiAKMDAwMDAwNTg1NSAwMDAwMCBuIAowMDAwMDA1OTg3IDAwMDAwIG4gCjAwMDAwMDYxMjMgMDAwMDAgbiAKMDAwMDAwNjI1MSAwMDAwMCBuIAowMDAwMDA2Mzc4IDAwMDAwIG4gCjAwMDAwMDY1MDcgMDAwMDAgbiAKMDAwMDAwNjY0MCAwMDAwMCBuIAowMDAwMDA2NzQyIDAwMDAwIG4gCjAwMDAwMDY4MzggMDAwMDAgbiAKMDAwMDAwOTc3NiAwMDAwMCBuIAowMDAwMDA5ODYyIDAwMDAwIG4gCnRyYWlsZXIKPDwKL1NpemUgMjQKL1Jvb3QgMjMgMCBSCi9JbmZvIDIyIDAgUgovSUQgWyA8RDBFM0E3RjVCQURBRTBCQjA5NDgzQTJGNENBNDM1QzQ+IDxEMEUzQTdGNUJBREFFMEJCMDk0ODNBMkY0Q0E0MzVDND4gXQo+PgpzdGFydHhyZWYKOTk2NgolJUVPRg==";
            byte[] bytes = Convert.FromBase64String(base64);

            return File(bytes, "application/pdf", "CV_12203967.pdf");
        }
    }
}
