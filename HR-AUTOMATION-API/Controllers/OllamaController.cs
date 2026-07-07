using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Kernel.Models;
using Shared.Kernel.Responses;

namespace HR_AUTOMATION_API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/ollama")]
    [EnableRateLimiting("general")]
    public class OllamaController : ControllerBase
    {
        private readonly IOllamaRequestService _ollamaService;

        public OllamaController(IOllamaRequestService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [HttpPost("generate")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Generate([FromBody] OllamaRequestInput input)
        {
            var result = await _ollamaService.GenerateAsync(input.Model, input.Prompt);
            var response = new Response<OllamaViewModel>
            {
                Code = StatusCodes.Status200OK,
                DataResponse = result
            };
            return Ok(response);
        }
    }

    public class OllamaRequestInput
    {
        public string Model { get; set; } = "llama3.2:3b";
        public string Prompt { get; set; } = string.Empty;
    }
}
