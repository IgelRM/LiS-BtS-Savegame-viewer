namespace SaveGameEditor
{
    public class Settings
    {
        public Settings()
        {
            CheckForUpdatesAtStartup = true;
        }

        public string SavePath { get; set; }

        public string GamePath { get; set; }

        public bool EditModeIntroShown { get; set; }

        public bool RewindNotesShown { get; set; }

        public bool FindHintShown { get; set; }

        public bool CheckForUpdatesAtStartup { get; set; }
    }
}
