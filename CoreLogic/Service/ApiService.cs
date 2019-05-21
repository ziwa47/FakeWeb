using System.Threading.Tasks;

namespace CoreLogic.Service
{
    public class ApiService
    {
        public static async Task<TResp> PostApi<TReq, TResp>(TReq req)
        {
            return await Task.FromResult(default(TResp));
        }
    }
}