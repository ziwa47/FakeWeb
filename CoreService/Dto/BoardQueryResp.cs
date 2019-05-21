using System.Collections.Generic;

namespace CoreService.Dto
{
    public class BoardQueryResp
    {
        public bool IsSuccess { get; set; }

        public List<BoardQueryRespItem> Items { get; set; }
    }
}