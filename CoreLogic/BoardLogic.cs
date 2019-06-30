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
        private readonly IApiService _apiService;
        private readonly IBoardDa _boardDa;
        private readonly IMyLogger _logger;

        public BoardLogic(Operation operation, IBoardDa da = null)
            : base(operation)
        {
            _boardDa = da ?? new BoardDa(operation);
        }

        public BoardLogic(Operation operation,IApiService apiService, IBoardDa boardDa,IMyLogger logger) : base(operation)
        {
            _apiService = apiService;
            _boardDa = boardDa;
            _logger = logger;
        }

        public async Task<IsSuccessResult<BoardListDto>> GetBoardList(SearchParamDto search, int pageSize)
        {
            var queryDto = new BoardQueryDto
            {
                PageSize = pageSize,
                Search = search
            };

            // Http 呼叫 Service 取得資料
            var resp = await _apiService.PostApi<BoardQueryDto, BoardQueryResp>(queryDto);

            if (!resp.IsSuccess || resp.Items == null)
                return new IsSuccessResult<BoardListDto>() {ErrorMessage = "Error", IsSuccess = false};

            // 使用 http 的資料 從 DB 取得資料
            var settings = _boardDa.GetBoardData(resp.Items.Select(r => r.Id));

            _logger.Info(string.Join(",", settings.Where(s => s.IsWarning).Select(s => s.Name).ToArray()));

            var boardListDto = new BoardListDto
            {
                BoardListItems = settings
                                 .Where(s => !s.IsTest)
                                 .Select(r => new BoardListItem
                                 {
                                     Id = r.Id,
                                     Name = r.Name
                                 })
            };

            return new IsSuccessResult<BoardListDto>
            {
                IsSuccess = true,
                ReturnObject = boardListDto
            };
        }
    }
}