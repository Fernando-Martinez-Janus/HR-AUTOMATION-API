using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Kernel.Appsettings;
using Shared.Kernel.IServices;
using Shared.Kernel.Models;
using Shared.Kernel.Services;

namespace HR_AUTOMATION.Application.Services
{
    public class OllamaRequestService(IHttpService httpService, IOptions<OllamaConfigurations> config) : IOllamaRequestService
    {
        private readonly IHttpService _httpService = httpService;
        private readonly OllamaConfigurations _config = config.Value;


        public async Task<OllamaViewModel> GenerateAsync(string model, string prompt)
        {
            var input = new OllamaInputModel
            {
                Model = model,
                Prompt = prompt,
                Stream = false
            };

            var request = new HttpRequest
            {
                Url = $"{_config.BaseUrl}{_config.GenerateEndpoint}",
                Method = HttpMethod.Post,
                Body = input,
                Timeout = _config.Timeout
            };

            var response = await _httpService.SendRequestAsync(request);
            var json = response.GetResponseAsString();
            var result = JsonConvert.DeserializeObject<OllamaViewModel>(json);

            return result!;
        }
    }
}
