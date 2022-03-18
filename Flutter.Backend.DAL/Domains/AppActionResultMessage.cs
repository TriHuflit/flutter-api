
namespace Flutter.Backend.DAL.Domains
{
    public class AppActionResultMessage<TData> : AppActionResult<TData, string>
    {
        public AppActionResultMessage()
        {
        }

        public AppActionResultMessage(TData data)
            : base(data)
        {
        }
        public AppActionResultMessage<TData> BuildResult(TData data, string message = null)
        {
            SetInfo(success: true, message);
            base.Data = data;
            return this;
        }

        public new AppActionResultMessage<TData> BuildError(string error)
        {
            SetInfo(success: false, error);
            return this;
        }
    }
}