using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace savefiledecoder
{
    public class VariableState
    {
        public string Name { get; set; }

        public int? Value { get; set; } //? is nullable. it means that the variable can be empty (without value)
    }

    public class FloatState
    {
        public string Name { get; set; }

        public float? Value { get; set; }
    }

    public class Checkpoint
    {
        private readonly dynamic _checkpoint; // Checkpoint object. Possibly holding checkpoint name.

        public string PointIdentifier => _checkpoint.pointIdentifier;

        public string Objective => _checkpoint.currentObjective;

        public Dictionary<string, VariableState> Variables { get; }

        public Dictionary<string, FloatState> Floats { get; }

        public List<string> Flags { get; }

        public Checkpoint(dynamic checkpoint, Dictionary<string, VariableState> variables, List<string> flags, Dictionary<string, FloatState> floats)
        {
            _checkpoint = checkpoint;
            Variables = variables;
            Flags = flags;
            Floats = floats;
        }
    }

    public class GameSave
    {
        private readonly GameData _gameData;

        /// <summary>
        /// Holds info about played episodes.
        /// Key is a name of an episode, e.g E1, E2; Value is true if episode was played and false otherwise.
        /// </summary>
        public Dictionary<string, bool> PlayedEpisodes { get; } = new Dictionary<string, bool>();

        public List<Checkpoint> Checkpoints { get; } = new List<Checkpoint>();

        public dynamic Data;

        public dynamic Header;

        public string RawSave { get; set; }

        public string RawHeader { get; set; }

        public bool SaveChangesSaved { get; set; } = true;

        public bool HeaderChangesSaved { get; set; } = true;

        public bool SaveIsEmpty { get; set; }

        public bool IsAtMidLevel
        {
            get
            {
                return Data.currentCheckpoint.hasMidLevelData;
            }
            set
            {
                Data.currentCheckpoint.hasMidLevelData = value;
            }
        }

        public List<string> EpisodeStates = new List<string>();
        public int[] SaveDate = new int[3];

        public readonly string[] EpisodeNames =
        {
            "Epsiode 1: Awake",
            "Episode 2: Brave New World",
            "Epsiode 3: Hell is Empty",
            "Bonus Episode: Farewell"
        };

        public readonly OrderedDictionary PointNames = new OrderedDictionary()
        {
            // Episode 1
            {"E1_S01_A", "Old Mill - Exterior"},
            {"E1_S01_B", "Old Mill - Interior"},
            {"E1_S02_BUILD_AB", "Price House - Upstairs"},
            {"E1_S02_BUILD_CD", "Price House - Downstairs"},
            {"E1_S03", "First Dream"},
            {"E1_S04_A", "School Campus"},
            {"E1_S04_D", "School Drama Lab"},
            {"E1_S05", "Train"},
            {"E1_S06", "Overlook"},
            {"E1_S08", "Junkyard"},
            {"E1_S09", "Second Dream"},
            {"E1_S10_A", "Junkyard - Night"},
            {"E1_S10_B", "Overlook - Night"},
            {"Episode1End", "Episode 1 Ending"},

            // Episode 2
            {"E2_S01_ABC", "Principal's Office"},
            {"E2_S01_D", "Blackwell Parking Lot"},
            {"E2_S02_A", "Junkyard"},
            {"E2_S02_B", "Dream"},
            {"E2_S02_C", "Junkyard - Later"},
            {"E2_S03", "Frank's RV"},
            {"E2_S04_A", "Dormitories (Outside)"},
            {"E2_S04_B", "Boys' Dormitories"},
            {"E2_S05_A", "Campus - Backstage"},
            {"E2_S05_B", "The Tempest"},
            {"E2_S06", "Neighborhood"},
            {"E2_S07", "Amber House"},
            {"Episode2End", "Episode 2 Ending"},

            // Episode 3
            {"E3_S01_A", "Amber House"},
            {"E3_S01_B", "Rachel's Room"},
            {"E3_S01_C", "Dream"},
            {"E3_S02_A", "Price House - Upstairs"},
            {"E3_S02_B", "Price House - Downstairs"},
            {"E3_S03_AC", "Junkyard"},
            {"E3_S04_AEBC", "Hospital"},
            {"E3_S04_D", "Hospital - Rachel's Room"},
            {"E3_S05", "Amber House - Office"},
            {"E3_S06", "Burned Forest"},
            {"E3_S07_B", "Old Mill"},
            {"E3_S08", "Hospital - Rachel's Room"},
            {"Episode3End", "Episode 3 Ending"}
         };

        public readonly Dictionary<string, string> PointVariablePrefixes = new Dictionary<string, string>()
        {
            {"E1_S01_A", "E1_"},
            {"E1_S01_B", "E1_S01B_"},
            {"E1_S02_BUILD_AB", "E1_S02B_"},
            {"E1_S02_BUILD_CD", "E1_S02C_"},
            {"E1_S03", "E1_S04A_"},
            {"E1_S04_A", "E1_S04A_"},
            {"E1_S04_D", "E1_S04D_"},
            {"E1_S05", "E1_S05_"},
            {"E1_S06", "E1_S08_"},
            {"E1_S08", "E1_S08_"},
            {"E1_S09", "E2_S01A_"},
            {"E1_S10_A", "E2_S01A_"},
            {"E1_S10_B", "E2_S01A_"},
            {"Episode1End", "E2_S01A_"},
            {"E2_S01_ABC", "E2_S01A_"},
            {"E2_S01_D", "E2_S01D_"},
            {"E2_S02_A", "E2_S02A_"},
            {"E2_S02_B", "E2_S03_"},
            {"E2_S02_C", "E2_S03_"},
            {"E2_S03", "E2_S03_"},
            {"E2_S04_A", "E2_S04A_"},
            {"E2_S04_B", "E2_S04B_"},
            {"E2_S05_A", "E2_S05A_"},
            {"E2_S05_B", "E2_S06_"},
            {"E2_S06", "E2_S06_"},
            {"E2_S07", "E2_S07_"},
            {"Episode2End", "E3_S02A_"},
            {"E3_S01_A", "E3_S02A_"},
            {"E3_S01_B", "E3_S02A_"},
            {"E3_S01_C", "E3_S02A_"},
            {"E3_S02_A", "E3_S02A_"},
            {"E3_S02_B", "E3_S02B_"},
            {"E3_S03_AC", "E3_S03_"},
            {"E3_S04_AEBC", "E3_S04A_"},
            {"E3_S04_D", "E3_S05A_"},
            {"E3_S05", "E3_S05A_"},
            {"E3_S06", "E3_S07B_"},
            {"E3_S07_B", "E3_S07B_"},
            {"E3_S08", "E3_S08_"},
            {"Episode3End", "E4_"}
        };

        public GameSave(GameData gameData)
        {
            _gameData = gameData; // This is the initialdata, NOT savefile
        }

        public void ReadSaveFromFile(string savePath)
        {
            SaveIsEmpty = false;
            Checkpoints.Clear();
            PlayedEpisodes.Clear();

            // Read and decode Data
            var fileContent = File.ReadAllBytes(savePath);
            try
            {
                Data = JsonConverter.DecodeFileContentToJson(fileContent);
                RawSave = Data.ToString();
            }
            catch
            {
                SaveIsEmpty = true;
                return;
            }

            Dictionary<string, VariableState> variables;
            Dictionary<string, FloatState> floats;

            // Add regular checkpoints
            foreach (var checkpoint in Data.checkpoints)
            {
                var cpFlags = new List<string>();
                variables = GetCheckpointVariables(checkpoint);
                floats = GetCheckpointFloats(checkpoint);
                foreach (var flag in checkpoint.flags)
                {
                    cpFlags.Add(flag.Value);
                }
                Checkpoints.Add(new Checkpoint(checkpoint, variables, cpFlags, floats));
            }

            // Add currentcheckpoint (seems to be identical to latest checkpoint...)
            try
            {
                var currentCpFlags = new List<string>();
                foreach (var fl in Data.currentCheckpoint.stateCheckpoint.flags)
                {
                    currentCpFlags.Add(fl.Value);
                }
                variables = GetCheckpointVariables(Data.currentCheckpoint.stateCheckpoint);
                floats = GetCheckpointFloats(Data.currentCheckpoint.stateCheckpoint);
                Checkpoints.Add(new Checkpoint(Data.currentCheckpoint.stateCheckpoint, variables, currentCpFlags, floats));
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                SaveIsEmpty = true;
                return;
            }

            // Add global variables as a last checkpoint.. // hack...
            variables = GetCheckpointVariables(Data);
            floats = GetCheckpointFloats(Data);
            Checkpoints.Add(new Checkpoint(new
            {
                pointIdentifier = "Global Vars",
                currentObjective = Data.currentObjective
            }, variables, null, floats));

            // Fill episodeState (?)
            foreach (var episode in Data.episodes)
            {
                string episodeName = episode.name;
                string episodeState = episode.episodeState;
                var isPlayed = episodeState == "kFinished" || episodeState == "kInProgress";
                PlayedEpisodes[episodeName] = isPlayed;
            }
            if (File.Exists(Path.GetDirectoryName(savePath) + @"\Header.Save"))
            {
                ReadHeaderFromFile(Path.GetDirectoryName(savePath) + @"\Header.Save");
            }
            else
            {
                Header = null;
            }
        }

        public void ReadHeaderFromFile(string headerPath)
        {
            // Read and decode data
            var fileContent = File.ReadAllBytes(headerPath);
            try
            {
                Header = JsonConverter.DecodeFileContentToJson(fileContent);
                RawHeader = Header.ToString();
            }
            catch
            {
                SaveIsEmpty = true;
            }

            // Fill episodestates
            EpisodeStates.Clear();
            foreach (var episode in Header.cachedEpisodes)
            {
                EpisodeStates.Add(episode.Value);
            }

            // Read the date of the save
            for (var i = 0; i < Header.saveDate.Count; i++)
            {
                SaveDate[i] = Header.saveDate[i]; // Need to test if it's possible to write to this dynamic array without an intermediate one
            }
        }

        public void WriteSaveToFile(string savePath, dynamic saveJsonObject)
        {
            var fileContent = JsonConverter.EncodeJsonToFileContent(saveJsonObject);

            if (!File.Exists(savePath + @".bkp"))
            {
                File.Copy(savePath, savePath + @".bkp", false);
            }

            File.WriteAllBytes(savePath, fileContent); // Write changes to Data.Save
            SaveChangesSaved = true;
        }

        public void WriteHeaderToFile(string headerPath, dynamic headerJsonObject)
        {
            var fileContent = JsonConverter.EncodeJsonToFileContent(headerJsonObject);

            if (!File.Exists(headerPath + @".bkp"))
            {
                File.Copy(headerPath, headerPath + @".bkp", false);
            }

            File.WriteAllBytes(headerPath, fileContent); // Write changes to Header.Save
            HeaderChangesSaved = true;
        }

        private Dictionary<string, VariableState> GetCheckpointVariables(dynamic checkpoint)
        {
            var pointVariables = new Dictionary<string, VariableState>(); // Key is a variable name; Value is a name-value pair

            foreach (var variable in checkpoint.variables)
            {
                int value = variable.currentValue;
                string gameVariableId = variable.storyVariable;
                string name = _gameData.GetOrCreateVariableNameById(gameVariableId);
                pointVariables[name] = new VariableState {Name = name, Value = value};
            }

            return pointVariables;
        }

        private Dictionary<string, FloatState> GetCheckpointFloats(dynamic checkpoint)
        {
            var pointFloats = new Dictionary<string, FloatState>(); // Key is a variable name; Value is a name-value pair

            foreach (var flt in checkpoint.floatValuesDict)
            {
                if (flt.Name == "$type")
                {
                    continue;
                }
                var value = (float?) flt.Value;
                string name = flt.Name;
                pointFloats[name] = new FloatState {Name = name, Value = value};
            }

            return pointFloats;
        }

        // Value gets updated inside JSON object (m_Data)
        public bool FindAndUpdateVarValue(string checkpointId, string varName, int? origValue, int? newValue, VariableScope varScope)
        {
            dynamic editingPoint = null;
            var varId = _gameData.GetVariableIdByName(varName);
            var pointFound = false;
            var success = false;
             
            switch (varScope)
            {
                case VariableScope.Global:
                    editingPoint = Data;
                    pointFound = true;
                    break;
                case VariableScope.CurrentCheckpoint:
                    editingPoint = Data.currentCheckpoint.stateCheckpoint;
                    pointFound = true;
                    break;
                default:
                    foreach (var checkpoint in Data.checkpoints)
                    {
                        if (checkpoint.pointIdentifier.Value == checkpointId)
                        {
                            editingPoint = checkpoint;
                            pointFound = true;
                            break;
                        }
                    }
                    break;
            }
            
            if (pointFound)
            {
                // Add new variable
                if (origValue == null)
                {
                    var guid = Guid.NewGuid().ToString();
                    if (varScope == VariableScope.CurrentCheckpoint)
                    {
                        foreach (var variable in Data.checkpoints[Data.checkpoints.Count-1].variables)
                        {
                            if (variable.storyVariable.Value == varId)
                            {
                                guid = variable.uniqueId.Value;
                            }
                        }
                    }
                    var varBody = new Dictionary<string, object>
                    {
                        {"uniqueId", guid},
                        {"storyVariable", varId},
                        {"currentValue", newValue},
                        {"$type", "GameStateVariableModel"}
                    };

                    var freshVar = JObject.FromObject(varBody);
                    ((JArray) editingPoint.variables).Add(freshVar);
                    success = true;
                }
                // Remove variable
                else if (newValue == null)
                {
                    foreach (var variable in editingPoint.variables)
                    {
                        if (variable.storyVariable.Value == varId)
                        {
                            ((JArray) editingPoint.variables).Remove(variable);
                            success = true;
                            break;
                        }
                    }
                }
                // Change variable value
                else
                {
                    foreach (var variable in editingPoint.variables)
                    {
                        if (variable.storyVariable.Value == varId)
                        {
                            variable.currentValue.Value = newValue;
                            success = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
                return false;
            }

            if (!success)
            {
                MessageBox.Show("Could not find and replace variable with ID " + varId + "!");
            }
            else
            {
                SaveChangesSaved = false;
            }

            return success;
        }

        // Value gets updated inside JSON object (m_Data)
        public bool FindAndUpdateFloatValue(string checkpointId, string floatName, float? origValue, float? newValue,
            VariableScope varScope)
        {
            dynamic editingPoint = null;
            var pointFound = false;

            switch (varScope)
            {
                case VariableScope.Global:
                    editingPoint = Data;
                    pointFound = true;
                    break;
                case VariableScope.CurrentCheckpoint:
                    editingPoint = Data.currentCheckpoint.stateCheckpoint;
                    pointFound = true;
                    break;
                default:
                    foreach (var checkpoint in Data.checkpoints)
                    {
                        if (checkpoint.pointIdentifier.Value == checkpointId)
                        {
                            editingPoint = checkpoint;
                            pointFound = true;
                            break;
                        }
                    }
                    break;
            }

            if (pointFound)
            {
                // Add new float value
                if (origValue == null)
                {
                    ((JObject) editingPoint.floatValuesDict).Add(floatName, newValue);
                }
                // Remove float value
                else if (newValue == null)
                {
                    ((JObject) editingPoint.floatValuesDict).Remove(floatName);
                }
                // Change float value
                else
                {
                    editingPoint.floatValuesDict[floatName].Value = newValue;
                }

                SaveChangesSaved = false;
                return true;
            }

            MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
            return false;
        }

        public bool FindAndUpdateFlagValue(string checkpointId, string flagName, bool origValue, VariableScope varScope)
        {
            dynamic editingPoint = null;
            var pointFound = false;

            if (varScope == VariableScope.CurrentCheckpoint)
            {
                editingPoint = Data.currentCheckpoint.stateCheckpoint;
                pointFound = true;
            }
            else
            {
                foreach (var checkpoint in Data.checkpoints)
                {
                    if (checkpoint.pointIdentifier.Value == checkpointId)
                    {
                        editingPoint = checkpoint;
                        pointFound = true;
                        break;
                    }
                }
            }

            if (pointFound)
            {
                // Add new flag
                if (origValue == false)
                {
                    ((JArray) editingPoint.flags).Add(flagName);
                }
                // Remove one of the existing flags
                else
                {
                    JToken flagToDelete = null;
                    foreach (var flag in editingPoint.flags)
                    {
                        if (flag.Value == flagName)
                        {
                            flagToDelete = flag;
                        }
                    }
                    flagToDelete.Remove();
                }

                SaveChangesSaved = false;
                return true;
            }

            MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
            return false;
        }

        public void RestartFromCheckpoint(string variablePrefix, dynamic destPoint, int epNumber)
        {
            // Remove variables of future scenes from the minor and major variable list
            FillMinorAndMajorVars(variablePrefix);

            // Erase future checkpoints from the checkpoint list
            var checkpointsList = new List<JObject>();
            foreach (var checkpoint in Data.checkpoints)
            {
                checkpointsList.Add(checkpoint);
                if (checkpoint == destPoint)
                {
                    break;
                }
            }
            Data.checkpoints = JArray.FromObject(checkpointsList);

            // Copy flags from last checkpoint to global flags
            ((JToken) Data.flags).Replace(destPoint.flags);
            // Copy float values from last checkpoint to global float values
            ((JToken) Data.floatValuesDict).Replace(destPoint.floatValuesDict);

            // Set episode states
            string destPointId = destPoint.pointIdentifier;
            for (var i = 0; i < Data.episodes.Count; i++)
            {
                if (i < epNumber)
                {
                    Data.episodes[i].episodeState = "kFinished";
                }
                else if (i == epNumber && !destPointId.EndsWith("End"))
                {
                    Data.episodes[i].episodeState = "kInProgress";
                }
                else if (i > epNumber && Data.episodes[i].episodeState != "kUnavailable")
                {
                    Data.episodes[i].episodeState = "kNotPlayed";
                }
            }

            // Syncronise last checkpoint and global variables
            foreach (var globalVar in Data.variables)
            {
                foreach (var variable in destPoint.variables)
                {
                    if (variable.storyVariable == globalVar.storyVariable)
                    {
                        globalVar.currentValue = variable.currentValue;
                        break;
                    }

                    if (ShouldGlobalVarBeReseted(globalVar.storyVariable.Value, destPointId))
                    {
                        globalVar.currentValue = 0;
                    }
                }
            }

            SyncLastAndCurrentCheckpoints(destPoint, epNumber);
            Data.uniqueId = Guid.NewGuid();
            Data.currentObjective = destPoint.currentObjective;

            RewindHeader();
        }

        public void RewindHeader()
        {
            Header.uniqueId.Value = Guid.NewGuid();
            for (int i = 0; i < Data.episodes.Count; i++)
            {
                Header.cachedEpisodes[i] = Data.episodes[i].episodeState;
            }

            Header.saveDate = JArray.FromObject(SaveDate);

            if (Data.currentCheckpoint.stateCheckpoint.pointIdentifier == "Episode1End" || 
                Data.currentCheckpoint.stateCheckpoint.pointIdentifier == "Episode2End")
            {
                Header.currentScene = "GLOBAL_CODE_READYTOSTARTEPISODE";
                Header.currentEpisode = "GLOBAL_CODE_READYTOSTARTEPISODE";
            }
            else if (Data.currentCheckpoint.stateCheckpoint.pointIdentifier == "Episode3End")
            {
                Header.currentScene = "GLOBAL_CODE_STORYCOMPLETE";
                Header.currentEpisode = "GLOBAL_CODE_STORYCOMPLETE";
            }
            else
            {
                Header.currentScene = Data.currentCheckpoint.currentScene;
                Header.currentEpisode = Data.currentCheckpoint.currentEpisode;
            }
        }

        #region Sub-functions
        private void FillMinorAndMajorVars(string variablePrefix)
        {
            var minorVarList = new List<string>();
            var majorVarList = new List<string>();

            if (variablePrefix == "E1_")
            {
                Data.minorChoiceVariables = new JArray();
                Data.majorChoiceVariables = new JArray();
            }
            else
            {
                foreach (string variable in Data.minorChoiceVariables)
                {
                    if (variable.StartsWith(variablePrefix))
                    {
                        break;
                    }

                    minorVarList.Add(variable);
                }
                foreach (string variable in Data.majorChoiceVariables)
                {
                    if (variable.StartsWith(variablePrefix))
                    {
                        break;
                    }

                    majorVarList.Add(variable);
                }
                Data.minorChoiceVariables = JArray.FromObject(minorVarList);
                Data.majorChoiceVariables = JArray.FromObject(majorVarList);
            }
        }

        private void SyncLastAndCurrentCheckpoints(dynamic destPoint, int epNumber)
        {
            Data.currentCheckpoint.stateCheckpoint = destPoint;
            if (IsAtMidLevel)
            {
                IsAtMidLevel = false;
                ((JArray) Data.currentCheckpoint.visitedNodes).Replace(new JArray());
            }

            if (destPoint.pointIdentifier == "Episode1End")
            {
                Data.currentCheckpoint.currentScene = "e1_s10_b";
            }
            else if (destPoint.pointIdentifier == "Episode2End")
            {
                Data.currentCheckpoint.currentScene = "e2_s07";
            }
            else if (destPoint.pointIdentifier == "Episode3End")
            {
                Data.currentCheckpoint.currentScene = "e3_s08";
            }
            else
            {
                string id = destPoint.pointIdentifier;
                Data.currentCheckpoint.currentScene = id.ToLowerInvariant();
            }
            Data.currentCheckpoint.currentEpisode = "E" + (epNumber + 1);
        }

        // For graffiti variables
        private readonly Dictionary<string, string> _globalVarPrefixes = new Dictionary<string, string>
        {
            {"E1_S01_A", "E1_"},
            {"E1_S01_B", "E1_S01B_"},
            {"E1_S02_BUILD_AB", "E1_S06_"},
            {"E1_S02_BUILD_CD", "E1_S06_"},
            {"E1_S03", "E1_S06_"},
            {"E1_S04_A", "E1_S06_"},
            {"E1_S04_D", "E1_S06_"},
            {"E1_S05", "E1_S06_"},
            {"E1_S06", "E1_S06_"},
            {"E1_S08", "E1_S08_"},
            {"E1_S09", "E2_S02_"},
            {"E1_S10_A", "E2_S02_"},
            {"E1_S10_B", "E2_S02_"},
            {"Episode1End", "E2_S02_"},
            {"E2_S01_ABC", "E2_S02_"},
            {"E2_S01_D", "E2_S02_"},
            {"E2_S02_A", "E2_S02_"},
            {"E2_S02_B", "E2_S02_"},
            {"E2_S02_C", "E2_S02_"},
            {"E2_S03", "E2_S03_"},
            {"E2_S04_A", "E2_S04A_"},
            {"E2_S04_B", "E2_S04B_"},
            {"E2_S05_A", "E2_S05_"},
            {"E2_S05_B", "E2_S05_"},
            {"E2_S06", "E2_S07_"},
            {"E2_S07", "E2_S07_"},
            {"Episode2End", "E3_"},
            {"E3_S01_A", "E3_S01_"},
            {"E3_S01_B", "E3_S01B_"},
            {"E3_S01_C", "E3_S02A_"},
            {"E3_S02_A", "E3_S02A_"},
            {"E3_S02_B", "E3_S02C_"},
            {"E3_S03_AC", "E3_S03_"},
            {"E3_S04_AEBC", "E3_S04_"},
            {"E3_S04_D",  "E3_S04A_"},
            {"E3_S05", "E3_S05_"},
            {"E3_S06", "E3_S07_"},
            {"E3_S07_B", "E4_ "},
            {"E3_S08", " E4_"},
            {"Episode3End", "E4_"}
        };

        private readonly string[] _graffitiVars =
        {
            "E1_S01A_GRAFFITIRV",
            "E1_S01B_BLANKSPOT",
            "E1_S06_GRAFFITISTATUE",
            "E1_S08_SIGNGRAFFITI",
            "E2_S02_GRAFFITITRUCK",
            "E2_S02_HOODGRAFFITI",
            "E2_S02_SHACKGRAFFITO",
            "E2_S03_RVGRAFFITI",
            "E2_S04A_GRAFFITICONCRETE",
            "E2_S05_MIRRORGRAFFITI",
            "E2_S07_JAMESGRAFFITI",
            "E3_S01_PLANNERGRAFFITI",
            "E3_S01B_MAPGRAFFITI",
            "E3_S02A_GRAFFITIPHOTO",
            "E3_S02C_GRAFFITICALENDAR",
            "E3_S03_GOBULEGRAFFITI",
            "E3_S04_GRAFFITINORTHCAST",
            "E3_S04A_GRAFFITIPOSTER",
            "E3_S04A_VENDINGMACHINEGRAFFITI",
            "E3_S05_GRAFFITI",
            "E3_S07_CARVE"
        };

        private bool ShouldGlobalVarBeReseted(string storyVarId, string pointId)
        {
            var untouchableVars = new List<string>();
            string prefix;
            if (!_globalVarPrefixes.TryGetValue(pointId, out prefix))
            {
                return false;
            }

            foreach (var graffitiVar in _graffitiVars)
            {
                if (graffitiVar.StartsWith(prefix))
                {
                    break;
                }

                untouchableVars.Add(graffitiVar);
            }

            if (storyVarId == _gameData.GetVariableIdByName("E1_S01_CI_PUNKJACKET") || 
                storyVarId == _gameData.GetVariableIdByName("E1_S06_CHLOESTICKSUPFORHERSELF"))
            {
                return false;
            }

            return !untouchableVars.Contains(_gameData.GetOrCreateVariableNameById(storyVarId));
        }
        #endregion
    }
}
