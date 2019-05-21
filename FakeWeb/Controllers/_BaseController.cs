using System;
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
        
        protected ActionResult NotAuthorizeJson()
        {
            return JsonError("Not Auth");
        }

        
        protected ActionResult JsonError(Exception ex)
        {
            var logger = LogManager.GetLogger("Exception");
            logger.Fatal(ex, "");

            return JsonError(ex.GetBaseException().Message);
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
        
        protected bool HasAuthority(string name)
        {
            AuthorityKey key;
            try
            {
                key = (AuthorityKey)Enum.Parse(typeof(AuthorityKey), name);
            }
            catch (Exception)
            {
                GetLogger().Warn($"未经定义的权限代码: [{name}]");
                return false;
            }

            return HasAuthority(key);
        }
       
        /// <summary>
        /// MasterLogic
        /// </summary>
        private MasterLogic _masterLogic
        {
            get
            {
                if (@masterLogic == null)
                    @masterLogic = new MasterLogic(GetOperation());
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