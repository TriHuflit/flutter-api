
namespace Flutter.Backend.DAL.Domains
{
    public class AppActionResultMessage<TData> : AppActionResult<object, string>
    {
        public AppActionResultMessage()
        {
        }

        public AppActionResultMessage(TData data)
            : base(data)
        {
        }
        public new AppActionResultMessage<TData> BuildResult(TData data, string detail = null)
        {
            SetInfo(success: true, detail);
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