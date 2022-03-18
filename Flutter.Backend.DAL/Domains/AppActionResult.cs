namespace Flutter.Backend.DAL.Domains
{
    public class AppActionResult<TData, TDetail> where TDetail : class
    {
        public bool IsSuccess { get; set; }

        public TData Data { get; set; }

        public TDetail Message { get; set; }


        public AppActionResult()
        {
            IsSuccess = false;
            Data = default(TData);
            Message = null;
        }

        public AppActionResult(TData data)
        {
            BuildResult(data);
        }

        public AppActionResult<TData, TDetail> BuildResult(TData data, TDetail message = null)
        {
            SetInfo(success: true, message);
            Data = data;
            return this;
        }

        public AppActionResult<TData, TDetail> BuildError(TDetail error)
        {
            SetInfo(success: false, error);
            return this;
        }

        public AppActionResult<TData, TDetail> SetInfo(bool success, TDetail message = null)
        {
            IsSuccess = success;
            Message = message;
            return this;
        }
    }
}