namespace CoreWebCommon.Dto
{
    public class IsSuccessResult<T>
    {
        public IsSuccessResult(string msg)
        {
            
        }

        public IsSuccessResult()
        {
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public object ReturnObject { get; set; }
    }
}