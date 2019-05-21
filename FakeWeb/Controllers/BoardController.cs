using System.Threading.Tasks;
using System.Web.Mvc;
using CoreLogic;
using CoreWebCommon.Dto;
using CoreWebCommon.Enum;

namespace FakeWeb.Controllers
{
    public class BoardController : _BaseController
    {
        private BoardLogic board => new BoardLogic(GetOperation());
        
        public async Task<ActionResult> List(SearchParamDto search, int pageSize = 10)
        {
            // 檢查權限
            if (HasAuthority(AuthorityKey.Board) == false)
                return NotAuthorizeJson();

            var result = await board.GetBoardList(search, pageSize);
            return Json(result);
        }
    }
}