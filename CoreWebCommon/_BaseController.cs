using System.Diagnostics;
using System.Web.Mvc;

namespace CoreWebCommon
{
    public abstract class _BaseController : Controller
    {
        protected string GetClientIP()
        {
            return "IP";
        }

        private NLog.Logger GetLogger()
        {
            var method = new StackFrame(1).GetMethod();
            var fullMethodName = $"{method.DeclaringType.FullName}.{method.Name}";
            return NLog.LogManager.GetLogger(fullMethodName);
        }
    }
}