using System;
using System.Configuration;
using System.Reflection;

namespace SaveGameEditor
{
    public class SettingManager
    {
        private readonly Configuration _config;

        public Settings Settings { get; set; }

        public SettingManager()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            Settings = new Settings();
            var props = Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var value = _config.AppSettings.Settings[prop.Name]?.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        prop.SetValue(Settings, Convert.ChangeType(value, prop.PropertyType));
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        public void SaveSettings()
        {
            _config.AppSettings.Settings.Clear();

            var props = Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                object value;
                try
                {
                    value = prop.GetValue(Settings);
                }
                catch
                {
                    continue;
                }

                if (value == null)
                {
                    continue;
                }
                _config.AppSettings.Settings.Add(prop.Name, value.ToString());
            }

            try
            {
                _config.Save(ConfigurationSaveMode.Full);
            }
            catch
            {

            }
            
            ConfigurationManager.RefreshSection(_config.AppSettings.SectionInformation.SectionName);
        }
    }
}
