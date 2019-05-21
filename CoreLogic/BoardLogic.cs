using System.Linq;
using System.Threading.Tasks;
using CoreDAL;
using CoreLogic.Dto;
using CoreService;
using CoreService.Dto;
using CoreWebCommon.Dto;

namespace CoreLogic
{
    public class BoardLogic : _BaseLogic
    {
        private readonly BoardDa _boardDa;
        private ApiService ApiService => new ApiService(GetLogger());

        public BoardLogic(Operation operation, BoardDa da = null)
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

            if (!resp.IsSuccess || resp.ReturnObject == null) return new IsSuccessResult<BoardListDto>("Error");

            var result = resp.ReturnObject;

            // 使用 http 的資料 從 DB 取得資料
            var settings = _boardDa.GetBoardData(result.Items.Select(r => r.Id));

            var boardListDto = new BoardListDto
            {
                BoardListItems = settings.Select(r => new BoardListItem
                {
                    Id = r.Id,
                    Name = r.Name
                })
            };

            return new IsSuccessResult<BoardListDto>
            {
                ReturnObject = boardListDto
            };
        }
    }
}