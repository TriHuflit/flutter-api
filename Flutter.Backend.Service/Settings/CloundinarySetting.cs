namespace Flutter.Backend.Service.Settings
{
    public class CloundinarySetting : ICloundinarySetting
    {
        public string CloudName { get; set; }

        public string ApiKey    { get; set; }

        public string ApiSecret { get; set; }
    }
    public interface ICloundinarySetting
    {
        public string CloudName { get; set; }

        public string ApiKey { get; set; }

        public string ApiSecret { get; set; }
    }
}
