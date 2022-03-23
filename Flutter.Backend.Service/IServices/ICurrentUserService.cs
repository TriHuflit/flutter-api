namespace Flutter.Backend.Service.IServices
{
    public interface ICurrentUserService
    {
        public string UserId { get; }

        public string UserName { get; }
    }
}
