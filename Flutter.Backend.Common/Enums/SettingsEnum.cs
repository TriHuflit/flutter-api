using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Common.Enums
{
    public static class SettingsEnum
    {
        public const string ICloundinarySetting = "ICloundinarySetting";

        public const string IMailSetting = "IMailSetting";

        public const string IRedisSettings = "IRedisSettings";

        public enum ISettings
        {
            ICloundinarySetting,
            IMailSetting,
            IRedisSettings
        }
    }
}
