using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Security;
using CoreLogic;
using CoreWebCommon.Dto;
using CoreWebCommon.Enum;
using Newtonsoft.Json;
using NLog;

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

        /// <summary>
        /// 取得使用者的資料
        /// </summary>
        /// <returns></returns>
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

        protected Logger GetLogger()
        {
            var fullMethodName = default(string);

            try
            {
                var method = new StackFrame(1).GetMethod();
                fullMethodName = $"{method.DeclaringType.FullName}.{method.Name}";
            }
            catch (Exception)
            {
                fullMethodName = "(unknown)";
            }

            return LogManager.GetLogger(fullMethodName);
        }

        protected T ReadUserData<T>()
        {
            if (!Request.IsAuthenticated) throw new UnauthorizedAccessException("no login");

            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var userData = FormsAuthentication.Decrypt(authCookie.Value).UserData;

            return JsonConvert.DeserializeObject<T>(userData);
        }

        protected ActionResult NotAuthorizeJson()
        {
            return JsonError("Not Auth");
        }

        protected ActionResult JsonError(string errorMessage, object returnObject = null)
        {
            return Json(new IsSuccessResult<object>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ReturnObject = returnObject
            }, JsonRequestBehavior.AllowGet);
        }

        private MasterLogic _masterLogic
        {
            get
            {
                if (@masterLogic == null) @masterLogic = new MasterLogic(GetOperation());
                return @masterLogic;
            }
        }

        private MasterLogic @masterLogic;

        protected bool HasAuthority(AuthorityKey key)
        {
            return _masterLogic.HasAuthority(LoginInfo.Id, key.ToString());
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.IsAuthenticated)
            {
                filterContext.Result = NotAuthorizeJson();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}