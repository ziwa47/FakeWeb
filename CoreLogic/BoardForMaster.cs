using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDAL;
using CoreWebCommon.Dto;

namespace CoreLogic
{
    public class BoardForMaster : _BaseLogic
    {
        private readonly BoardDa _boardDa;

        public BoardForMaster(Operation operation, BoardDa da = null)
                : base(operation)
        {
            _boardDa = da ?? new BoardDa();
        }

        public async Task<IsSuccessResult<BoardListDto>> GetBoardList(SearchParamDto search, int pageSize)
        {
            var queryDto = new BoardQueryDto
            {
                PageSize = pageSize,
                Search = search
            };

            // Http 呼叫 Service 取得資料
            var resp = await ApiService.PostApi<BoardQueryDto, BoardQueryResp>(queryDto);
            
            if (!resp.IsSuccess || resp.ReturnObject == null)
                return new IsSuccessResult<BoardListDto>("Error");

            var result = resp.ReturnObject;

            // DB 取得資料
            var settings = _boardDa.GetData(result.Items.Select(r => r.Id).ToList());

            // Merger Service 和 DB 的資料
            var boardListDto = new BoardListDto
            {
                BoardListItems = new List<BoardListItem>(result.Items.Select(r => new BoardListItem
                {
                    Id = settings[r.SettingId].Id,
                    Name = settings[r.SettingId].Name,
                })),
            };
            
            return new IsSuccessResult<BoardListDto>
            {
                ReturnObject = boardListDto
            };
        }
    }

    public class BoardQueryResp
    {
        public bool IsSuccess { get; set; }
        public BoardQueryRespObject ReturnObject { get; set; }
    }

    public class BoardQueryRespObject
    {
        public List<BoardQueryRespItem> Items { get; set; }
    }

    public class BoardQueryRespItem
    {
        public string Id { get; set; }
        public int SettingId { get; set; }
    }

    public class ApiService
    {
        public static async Task<TResp> PostApi<TReq, TResp>(TReq req)
        {
           return default(TResp) ;
        }
    }

    public class BoardQueryDto
    {
        public int PageSize { get; set; }
        public SearchParamDto Search { get; set; }
    }

    public class BoardListDto
    {
        public List<BoardListItem> BoardListItems { get; set; }
        public long Timestamp { get; set; }
    }

    public class BoardListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Setting
    {
        public string Id { get; set; }
        public string SettingName { get; set; }
    }
}