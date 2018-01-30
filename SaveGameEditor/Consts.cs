using System.Collections.Generic;
using System.Linq;

namespace SaveGameEditor
{
    public static class Consts
    {
        public static readonly Dictionary<Episode, string> EpisodeNames = new Dictionary<Episode, string>
        {
            {Episode.First, "Epsiode 1: Awake"},
            {Episode.Second, "Episode 2: Brave New World"},
            {Episode.Third, "Epsiode 3: Hell is Empty"},
            {Episode.Bonus, "Bonus Episode: Farewell"}
        };

        #region Graffities
        public static readonly string[] GraffitiVariableNames =
        {
            "E1_S01A_GRAFFITIRV",
            "E1_S01B_BLANKSPOT",
            "E1_S02A_GRAFFITIWALL",
            "E1_S02_GRAFFITITOOLBOX",
            "E1_S04_GRAFFITIRANGUS",
            "E1_S04D_GRAFFITITEMPESTPOSTER",
            "E1_S05_GRAFFITIEXIT",
            "E1_S06_GRAFFITISTATUE",
            "E1_S08_SIGNGRAFFITI",
            "E1_S09_GRAFFITIDREAM",
            "E2_S01D_LIGHTGRAFFITI",
            "E2_S02_HOODGRAFFITI",
            "E2_S02_GRAFFITITRUCK",
            "E2_S02_SHACKGRAFFITO",
            "E2_S03_RVGRAFFITI",
            "E2_S04A_GRAFFITICONCRETE",
            "E2_S04A_GRAFFITISAMUELDOOR",
            "E2_S04B_ELIOTGRAFFITI",
            "E2_S05_MIRRORGRAFFITI",
            "E2_S07_JAMESGRAFFITI",
            "E3_S01B_MAPGRAFFITI",
            "E3_S01_PLANNERGRAFFITI",
            "E3_S02A_GRAFFITIPHOTO",
            "E3_S02C_GRAFFITICALENDAR",
            "E3_S03_GOBULEGRAFFITI",
            "E3_S04A_VENDINGMACHINEGRAFFITI",
            "E3_S04A_GRAFFITIPOSTER",
            "E3_S04_GRAFFITINORTHCAST",
            "E3_S05_GRAFFITI",
            "E3_S07_CARVE"
        };
        #endregion

        #region CheckPoints
        public class CheckPointDescriptor
        {
            public Episode Episode { get; }

            public string Code { get; }

            public string Name { get; }

            public CheckPointDescriptor(Episode episode, string code, string name)
            {
                Episode = episode;
                Code = code;
                Name = name;
            }
        }

        public class CheckPointDescriptorCollection
        {
            private static readonly List<CheckPointDescriptor> CheckPointDescriptors = new List<CheckPointDescriptor>()
            {
                #region Episode 1
                new CheckPointDescriptor(Episode.First, "E1_S01_A", "Old Mill - Exterior"),
                new CheckPointDescriptor(Episode.First, "E1_S01_B", "Old Mill - Interior"),
                new CheckPointDescriptor(Episode.First, "E1_S02_BUILD_AB", "Price House - Upstairs"),
                new CheckPointDescriptor(Episode.First, "E1_S02_BUILD_CD", "Price House - Downstairs"),
                new CheckPointDescriptor(Episode.First, "E1_S03", "First Dream"),
                new CheckPointDescriptor(Episode.First, "E1_S04_A", "School Campus"),
                new CheckPointDescriptor(Episode.First, "E1_S04_D", "School Drama Lab"),
                new CheckPointDescriptor(Episode.First, "E1_S05", "Train"),
                new CheckPointDescriptor(Episode.First, "E1_S06", "Overlook"),
                new CheckPointDescriptor(Episode.First, "E1_S08", "Junkyard"),
                new CheckPointDescriptor(Episode.First, "E1_S09", "Second Dream"),
                new CheckPointDescriptor(Episode.First, "E1_S10_A", "Junkyard - Night"),
                new CheckPointDescriptor(Episode.First, "E1_S10_B", "Overlook - Night"),
                new CheckPointDescriptor(Episode.First, "Episode1End", "Episode 1 Ending"),
                #endregion

                #region Episode 2
                new CheckPointDescriptor(Episode.Second, "E2_S01_ABC", "Principal's Office"),
                new CheckPointDescriptor(Episode.Second, "E2_S01_D", "Blackwell Parking Lot"),
                new CheckPointDescriptor(Episode.Second, "E2_S02_A", "Junkyard"),
                new CheckPointDescriptor(Episode.Second, "E2_S02_B", "Dream"),
                new CheckPointDescriptor(Episode.Second, "E2_S02_C", "Junkyard - Later"),
                new CheckPointDescriptor(Episode.Second, "E2_S03", "Frank's RV"),
                new CheckPointDescriptor(Episode.Second, "E2_S04_A", "Dormitories (Outside)"),
                new CheckPointDescriptor(Episode.Second, "E2_S04_B", "Boys' Dormitories"),
                new CheckPointDescriptor(Episode.Second, "E2_S05_A", "Campus - Backstage"),
                new CheckPointDescriptor(Episode.Second, "E2_S05_B", "The Tempest"),
                new CheckPointDescriptor(Episode.Second, "E2_S06", "Neighborhood"),
                new CheckPointDescriptor(Episode.Second, "E2_S07", "Amber House"),
                new CheckPointDescriptor(Episode.Second, "Episode2End", "Episode 2 Ending"),
                #endregion

                #region Episode 3
                new CheckPointDescriptor(Episode.Third, "E3_S01_A", "Amber House"),
                new CheckPointDescriptor(Episode.Third, "E3_S01_B", "Rachel's Room"),
                new CheckPointDescriptor(Episode.Third, "E3_S01_C", "Dream"),
                new CheckPointDescriptor(Episode.Third, "E3_S02_A", "Price House - Upstairs"),
                new CheckPointDescriptor(Episode.Third, "E3_S02_B", "Price House - Downstairs"),
                new CheckPointDescriptor(Episode.Third, "E3_S03_AC", "Junkyard"),
                new CheckPointDescriptor(Episode.Third, "E3_S04_AEBC", "Hospital"),
                new CheckPointDescriptor(Episode.Third, "E3_S04_D", "Hospital - Rachel's Room"),
                new CheckPointDescriptor(Episode.Third, "E3_S05", "Amber House - Office"),
                new CheckPointDescriptor(Episode.Third, "E3_S06", "Burned Forest"),
                new CheckPointDescriptor(Episode.Third, "E3_S07_B", "Old Mill"),
                new CheckPointDescriptor(Episode.Third, "E3_S08", "Hospital - Rachel's Room"),
                new CheckPointDescriptor(Episode.Third, "Episode3End", "Episode 3 Ending"),
                #endregion
            };

            public static List<CheckPointDescriptor> GetCheckPointDescriptors(Episode episode)
            {
                return CheckPointDescriptors.Where(cp => cp.Episode == episode).ToList();
            }

            public static CheckPointDescriptor GetCheckPointDescriptor(string code)
            {
                return CheckPointDescriptors.FirstOrDefault(cp => cp.Code == code);
            }
        }
        #endregion

        #region Episode States
        public static class EpisodeStates
        {
            public const string InProgress = "kInProgress";
            public const string Finished = "kFinished";
            public const string NotPlayed = "kNotPlayed";
            public const string Unavailable = "kUnavailable";
        }
        #endregion

        #region Global Codes
        public static class GlobalCodes
        {
            public const string ReadyToStartEpisode = "GLOBAL_CODE_READYTOSTARTEPISODE";
            public const string StoryComplete = "GLOBAL_CODE_STORYCOMPLETE";
            public const string SaveJustStarted = "GLOBAL_CODE_SAVEJUSTSTARTED";
        }
        #endregion
    }
}
