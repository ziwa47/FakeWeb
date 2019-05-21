using CoreWebCommon.Dto;

namespace CoreService.Dto
{
    public class BoardQueryDto
    {
        public int PageSize { get; set; }
        public SearchParamDto Search { get; set; }
    }
}