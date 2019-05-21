using System;
using System.Diagnostics;
using NLog;

namespace CoreDAL
{
    public class _BaseDA : IDisposable
    {
        protected Logger GetLogger()
        {
            var fullMethodName = default(string);

            try
            {
                var method = new StackFrame(1).GetMethod();
                fullMethodName = $"{method.DeclaringType.FullName}.{method.Name}";
            }
            catch (Exception ex)
            {
                fullMethodName = ex.GetBaseException().Message;
            }

            return LogManager.GetLogger(fullMethodName);
        }

        protected DbContext DBContext
        {
            get
            {
                return _casinoCashContext.Value;
            }
        }

        private Lazy<DbContext> _casinoCashContext = new Lazy<DbContext>(() => new DbContext());

        public void Dispose()
        {
        }
    }
}