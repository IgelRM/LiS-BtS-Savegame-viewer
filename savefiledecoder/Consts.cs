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
            public const string ReadyToStartEpisode = "GLOBAL_CODE_READYTOSTARTEPISODE";
            public const string StoryComplete = "GLOBAL_CODE_STORYCOMPLETE";
            public const string SaveJustStarted = "GLOBAL_CODE_SAVEJUSTSTARTED";
        }
    }
}
