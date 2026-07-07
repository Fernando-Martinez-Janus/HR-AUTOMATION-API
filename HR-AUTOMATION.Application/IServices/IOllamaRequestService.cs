using Shared.Kernel.Models;

public interface IOllamaRequestService
{
    Task<OllamaViewModel> GenerateAsync(string model, string prompt);
}
