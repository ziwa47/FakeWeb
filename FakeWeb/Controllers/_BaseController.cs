using System;
using System.Web.Mvc;
using System.Web.Security;
using CoreWebCommon.Dto;
using Newtonsoft.Json;

namespace FakeWeb.Controllers
{
    public class _BaseController : CoreWebCommon._BaseController
    {
        public LoginInfo LoginInfo
        {
            get
            {
                if (_loginInfo == null) _loginInfo = ReadUserData<LoginInfo>();
                return _loginInfo;
            }
        }

        LoginInfo _loginInfo;

        protected Operation GetOperation()
        {
            var result = new Operation
            {
                IP = GetClientIP()
            };

            if (Request.IsAuthenticated == false)
            {
                result.Display = "_guest";
            }
            else
            {
                result.Display = User.Identity.Name;
                result.UserId = LoginInfo.Id;
            }

            return result;
        }
        
        protected T ReadUserData<T>()
        {
            if (!Request.IsAuthenticated)
                throw new UnauthorizedAccessException("尚未登入");

            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var userData = FormsAuthentication.Decrypt(authCookie.Value).UserData;

            return JsonConvert.DeserializeObject<T>(userData);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.IsAuthenticated
                && !Request.Url.ToString().Contains("SignOut"))
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/XX.cshtml"
                };
            }

            base.OnActionExecuting(filterContext);
        }
    }
}