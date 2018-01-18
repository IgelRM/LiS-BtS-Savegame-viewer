using System;
using System.Configuration;

namespace savefiledecoder
{
    public static class SettingManager
    {
        public static T Get<T>(string settingName, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            try
            {
                return (T) Convert.ChangeType(value, typeof (T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static T Get<T>(string settingName)
        {
            return Get(settingName, default(T));
        }

        public static void Set(string settingName, string value)
        {
            ConfigurationManager.AppSettings[settingName] = value;
        }
    }
}
