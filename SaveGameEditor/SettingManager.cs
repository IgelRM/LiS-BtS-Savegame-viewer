using System;
using System.Configuration;

namespace SaveGameEditor
{
    public static class SettingManager
    {
        static Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public static T Get<T>(string settingName, T defaultValue)
        {
            var value = configFile.AppSettings.Settings[settingName]?.Value;
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
            if (configFile.AppSettings.Settings[settingName] == null)
            {
                configFile.AppSettings.Settings.Add(settingName, value);
            }
            else
            {
                configFile.AppSettings.Settings[settingName].Value = value;
            }
        }

        public static void Save()
        {
            configFile.Save(ConfigurationSaveMode.Full);
        }
    }
}
