namespace CoreWebCommon.Dto
{
    public class IsSuccessResult<T>
            where T : class
    {
        public IsSuccessResult(string msg)
        {
        }

        public IsSuccessResult()
        {
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T ReturnObject { get; set; }
    }
}