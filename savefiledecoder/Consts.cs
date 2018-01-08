namespace savefiledecoder
{
    public static class Consts
    {
        public static class EpisodeStates
        {
            public const string InProgress = "kInProgress";
            public const string Finished = "kFinished";
            public const string NotPlayed = "kNotPlayed";
            public const string Unavailable = "kUnavailable";
        }

        public static class GlobalCodes
        {
            public const string GlobalCodeReadyToStartEpisode = "GLOBAL_CODE_READYTOSTARTEPISODE";
            public const string GlobalCodeStoryComplete = "GLOBAL_CODE_STORYCOMPLETE";
            public const string GlobalCodeSaveJustStarted = "GLOBAL_CODE_SAVEJUSTSTARTED";
        }
    }
}
