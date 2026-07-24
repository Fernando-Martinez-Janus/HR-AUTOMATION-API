using Shared.Kernel.ViewModels;

namespace HR_AUTOMATION.Application.ViewModels
{
    public class ActiveSearchViewModel
    {
        public int UnSeenCount { get; set; }
        public PaginationResponse<SearchResultViewModel> Result { get; set; } = null!;
    }
}