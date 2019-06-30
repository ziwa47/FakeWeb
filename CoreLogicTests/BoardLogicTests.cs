using CoreDAL;
using CoreDAL.Dto;

using CoreLogic;
using CoreLogic.Dto;

using CoreService;
using CoreService.Dto;

using CoreWebCommon.Dto;

using ExpectedObjects;

using NSubstitute;

using NUnit.Framework;
using NUnit.Framework.Internal;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CoreLogicTests
{
    [TestFixture]
    public class BoardLogicTests
    {
        private IApiService _apiService;
        private IBoardDa _boardDa;
        private Operation _operation;
        private BoardLogic _sut;
        private IMyLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _operation = new Operation();
            _apiService = Substitute.For<IApiService>();
            _boardDa = Substitute.For<IBoardDa>();
            _logger = Substitute.For<IMyLogger>();
            _sut = new BoardLogic(_operation, _apiService, _boardDa,_logger);
        }

        [Test]
        public async Task get_board_success_when_2_test_data_and_3_real_data()
        {
            GivenCallApiSuccess();
            GiveBoardDataFromDb(
                new BoardDto() { Id = "11", IsTest = false },
                new BoardDto() { Id = "12", IsTest = true },
                new BoardDto() { Id = "13", IsTest = false },
                new BoardDto() { Id = "14", IsTest = true },
                new BoardDto() { Id = "15", IsTest = false });

            var boardList = await WhenGetBoardList();

            var expected = new IsSuccessResult<BoardListDto>()
            {
                IsSuccess = true,
                ReturnObject = new BoardListDto
                {
                    BoardListItems = new BoardListItem[]
                    {
                        new BoardListItem(){Id = "11"},
                        new BoardListItem(){Id = "13"},
                        new BoardListItem(){Id = "15"},
                    }
                }
            };
            expected.ToExpectedObject().ShouldMatch(boardList);
        }

        private void GiveBoardDataFromDb(params BoardDto[] boardDto)
        {
            _boardDa.GetBoardData(new List<string>()).ReturnsForAnyArgs(boardDto.ToList());
        }

        [Test]
        public async Task get_api_return_false()
        {
            GivenCallApiFailed();
            var boardList = await WhenGetBoardList();

            var expected = new IsSuccessResult<BoardListDto>()
            {
                IsSuccess = false,
                ErrorMessage = "Error",
            };

            expected.ToExpectedObject().ShouldMatch(boardList);
        }

        [Test]
        public async Task get_api_return_true_but_item_null()
        {
            GivenCallApiNull();
            var boardList = await WhenGetBoardList();

            var expected = new IsSuccessResult<BoardListDto>()
            {
                IsSuccess = false,
                ErrorMessage = "Error",
            };

            expected.ToExpectedObject().ShouldMatch(boardList);
        }

        [Test]
        public async Task log_if_has_warning_member()
        {
            GivenCallApiSuccess();
            _boardDa.GetBoardData(new List<string>()).ReturnsForAnyArgs(new List<BoardDto>()
            {
                new BoardDto()
                {
                    IsWarning = true,
                    Name = "TestName",
                }
            });
            await WhenGetBoardList();

            _logger.Received().Info(Arg.Is<string>(m => m.Contains("TestName")));
        }

        private async Task<IsSuccessResult<BoardListDto>> WhenGetBoardList()
        {
            return await _sut.GetBoardList(new SearchParamDto(), 10);
        }

        private void GivenCallApiSuccess()
        {
            _apiService.PostApi<BoardQueryDto, BoardQueryResp>(Arg.Any<BoardQueryDto>())
                .Returns(new BoardQueryResp()
                {
                    IsSuccess = true,
                    Items = new List<BoardQueryRespItem>(),
                });
        }

        private void GivenCallApiFailed()
        {
            _apiService.PostApi<BoardQueryDto, BoardQueryResp>(Arg.Any<BoardQueryDto>())
                .Returns(new BoardQueryResp()
                {
                    IsSuccess = false,
                    Items = null
                });
        }

        private void GivenCallApiNull()
        {
            _apiService.PostApi<BoardQueryDto, BoardQueryResp>(Arg.Any<BoardQueryDto>())
                .Returns(new BoardQueryResp()
                {
                    IsSuccess = true,
                    Items = null
                });
        }
    }
}