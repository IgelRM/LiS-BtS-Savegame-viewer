using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace SaveGameEditor
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

    public class ItemState
    {
        public string Name { get; set; }

        public string Owner { get; set; }
    }

    public class Checkpoint
    {
        private readonly dynamic _checkpoint; // Checkpoint object. Possibly holding checkpoint name.

        public string PointIdentifier => _checkpoint.pointIdentifier;

        public string Objective => _checkpoint.currentObjective;

        public Dictionary<string, VariableState> Variables { get; }

        public Dictionary<string, FloatState> Floats { get; }

        public Dictionary<string, ItemState> Items { get; }

        public List<string> Flags { get; }

        public Checkpoint(dynamic checkpoint, Dictionary<string, VariableState> variables, List<string> flags, Dictionary<string, FloatState> floats, Dictionary<string, ItemState> items)
        {
            _checkpoint = checkpoint;
            Variables = variables;
            Flags = flags;
            Floats = floats;
            Items = items;
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

        public List<Checkpoint> MainCheckpoints { get; } = new List<Checkpoint>();

        public List<Checkpoint> FarewellCheckpoints { get; } = new List<Checkpoint>();

        public dynamic MainData;

        public dynamic FarewellData;

        public dynamic Header;

        public string RawMainSave { get; set; }

        public string RawFarewellSave { get; set; }

        public string RawHeader { get; set; }

        public bool MainSaveChangesSaved { get; set; } = true;

        public bool FarewellSaveChangesSaved { get; set; } = true;

        public bool HeaderChangesSaved { get; set; } = true;

        public bool MainSaveIsEmpty { get; set; }

        public bool MainSaveHasFarewellData
        {
            get
            {
                return MainData != null && MainData.e4Checkpoint != null;
            }
        }

        public bool FarewellSaveIsEmpty { get; set; }

        public bool IsMainAtMidLevel
        {
            get
            {
                if (MainData.currentCheckpoint == null)
                {
                    return false;
                }
                return MainData.currentCheckpoint.hasMidLevelData;
            }
            set
            {
                MainData.currentCheckpoint.hasMidLevelData = value;
            }
        }

        public bool IsFarewellAtMidLevel
        {
            get
            {
                if (FarewellData.currentCheckpoint == null)
                {
                    return false;
                }
                return FarewellData.currentCheckpoint.hasMidLevelData;
            }
            set
            {
                FarewellData.currentCheckpoint.hasMidLevelData = value;
            }
        }

        public List<string> EpisodeStates = new List<string>();
        public int[] SaveDate = new int[3];

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

        private bool IsFarewellCheckpoint (string pointID)
        {
            return pointID.StartsWith("E4_") || pointID.Equals("Episode4End");
        }

        public void ReadMainSaveFromFile(string savePath)
        {
            MainSaveIsEmpty = false;
            MainCheckpoints.Clear();
            PlayedEpisodes.Clear();

            // Read and decode Data
            var fileContent = File.ReadAllBytes(savePath);
            try
            {
                MainData = JsonConverter.DecodeFileContentToJson(fileContent);
                RawMainSave = MainData.ToString();
            }
            catch
            {
                MainSaveIsEmpty = true;
                goto SkipMain;
            }

            if (MainData["$type"] == "T_881EAB14") //opened a Bonus save instead of main
            {
                MainSaveIsEmpty = true;
                MainData = null;
                goto SkipMain;
            }

            Dictionary<string, VariableState> variables;
            Dictionary<string, FloatState> floats;
            Dictionary<string, ItemState> items;

            // Add regular checkpoints
            foreach (var checkpoint in MainData.checkpoints)
            {
                if (!IsFarewellCheckpoint (checkpoint.pointIdentifier.Value)) //ignore Farewell checkpoint data in main save
                {
                    var cpFlags = new List<string>();
                    variables = GetCheckpointVariables(checkpoint);
                    floats = GetCheckpointFloats(checkpoint);
                    items = GetCheckpointItems(checkpoint);
                    foreach (var flag in checkpoint.flags)
                    {
                        cpFlags.Add(flag.Value);
                    }
                    MainCheckpoints.Add(new Checkpoint(checkpoint, variables, cpFlags, floats, items));
                }
                
            }

            // Add currentcheckpoint (seems to be identical to latest checkpoint...)
            try
            {
                var currentCpFlags = new List<string>();
                foreach (var flag in MainData.currentCheckpoint.stateCheckpoint.flags)
                {
                    currentCpFlags.Add(flag.Value);
                }
                variables = GetCheckpointVariables(MainData.currentCheckpoint.stateCheckpoint);
                floats = GetCheckpointFloats(MainData.currentCheckpoint.stateCheckpoint);
                items = GetCheckpointItems(MainData.currentCheckpoint.stateCheckpoint);
                MainCheckpoints.Add(new Checkpoint(MainData.currentCheckpoint.stateCheckpoint, variables, currentCpFlags, floats, items));
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                MainSaveIsEmpty = true;
                goto SkipMain;
            }
            
            // Add global variables as a last checkpoint.. // hack...
            variables = GetCheckpointVariables(MainData);
            floats = GetCheckpointFloats(MainData);
            var globalFlags = new List<string>();
            foreach (var flag in MainData.flags)
                {
                    globalFlags.Add(flag.Value);
                }
            MainCheckpoints.Add(new Checkpoint(new
            {
                pointIdentifier = "Global Vars",
                currentObjective = MainData.currentObjective
            }, variables, globalFlags, floats, null));

            // Fill episodeState (?)
            foreach (var episode in MainData.episodes)
            {
                string episodeName = episode.name;
                string episodeState = episode.episodeState;
                var isPlayed = episodeState == Consts.EpisodeStates.Finished ||
                               episodeState == Consts.EpisodeStates.InProgress;
                PlayedEpisodes[episodeName] = isPlayed;
            }

            SkipMain:

            if (File.Exists(Path.GetDirectoryName(savePath) + @"\Header.Save"))
            {
                ReadHeaderFromFile(Path.GetDirectoryName(savePath) + @"\Header.Save");
            }
            else
            {
                Header = null;
            }

            if (File.Exists(savePath.Replace("SLOT_", "Bonus")))
            {
                ReadFarewellSaveFromFile(Path.GetDirectoryName(savePath).Replace("SLOT_", "Bonus") + @"\Data.Save");
            }

            else
            {
                FarewellData = null;
                FarewellSaveIsEmpty = true;
            }
        }

        public void ReadFarewellSaveFromFile(string savePath)
        {
            FarewellSaveIsEmpty = false;
            FarewellCheckpoints.Clear();

            // Read and decode Data
            var fileContent = File.ReadAllBytes(savePath);
            try
            {
                FarewellData = JsonConverter.DecodeFileContentToJson(fileContent);
                RawFarewellSave = FarewellData.ToString();
            }
            catch
            {
                FarewellSaveIsEmpty = true;
                return;
            }

            Dictionary<string, VariableState> variables;
            Dictionary<string, FloatState> floats;
            Dictionary<string, ItemState> items;

            // Add regular checkpoints
            foreach (var checkpoint in FarewellData.checkpoints)
            {
                var cpFlags = new List<string>();
                variables = GetCheckpointVariables(checkpoint);
                floats = GetCheckpointFloats(checkpoint);
                items = GetCheckpointItems(checkpoint);
                foreach (var flag in checkpoint.flags)
                {
                    cpFlags.Add(flag.Value);
                }
                FarewellCheckpoints.Add(new Checkpoint(checkpoint, variables, cpFlags, floats, items));
            }

            // Add currentcheckpoint (seems to be identical to latest checkpoint...)
            try
            {
                var currentCpFlags = new List<string>();
                foreach (var flag in FarewellData.currentCheckpoint.stateCheckpoint.flags)
                {
                    currentCpFlags.Add(flag.Value);
                }
                variables = GetCheckpointVariables(FarewellData.currentCheckpoint.stateCheckpoint);
                floats = GetCheckpointFloats(FarewellData.currentCheckpoint.stateCheckpoint);
                items = GetCheckpointItems(FarewellData.currentCheckpoint.stateCheckpoint);
                FarewellCheckpoints.Add(new Checkpoint(FarewellData.currentCheckpoint.stateCheckpoint, variables, currentCpFlags, floats, items));
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                FarewellSaveIsEmpty = true;
                return;
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
                MainSaveIsEmpty = true;
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
                SaveDate[i] = Header.saveDate[i];
            }
        }

        public void WriteSaveToFile(string savePath, dynamic saveJsonObject, SaveType type)
        {
            var fileContent = JsonConverter.EncodeJsonToFileContent(saveJsonObject);

            if (!File.Exists(savePath + @".bkp"))
            {
                File.Copy(savePath, savePath + @".bkp", false);
            }

            File.WriteAllBytes(savePath, fileContent); // Write changes to Data.Save

            if (type == SaveType.Bonus)
            {
                FarewellSaveChangesSaved = true;
            }
            else
            {
                MainSaveChangesSaved = true;
            }
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


        private Dictionary<string, ItemState> GetCheckpointItems(dynamic checkpoint)
        {
            var pointItems = new Dictionary<string, ItemState>(); // Key is a variable name; Value is a name-value pair

            foreach (var item in checkpoint.items)
            {
                string owner = item.currentOwnedBy;
                string gameItemId = item.storyItem;
                string name = _gameData.GetOrCreateItemNameById(gameItemId);
                pointItems[name] = new ItemState { Name = name, Owner = owner };
            }

            return pointItems;
        }

        #region Main Save Edit Functions
        // Value gets updated inside JSON object (m_Data)
        public bool FindAndUpdateMainVarValue(string checkpointId, string varName, object origValue, object newValue, VariableScope varScope)
        {
            if (IsFarewellCheckpoint(checkpointId))
            {
                if (!MainSaveHasFarewellData)
                {
                    return false;
                }
                
            }
            else if (MainSaveIsEmpty)
            {
                return false;
            }

            var success = false;
            dynamic editingPoint = FindMainEditPoint(varScope, checkpointId);

            var varId = varName == "Objective" ? varName : _gameData.GetVariableIdByName(varName);

            if (editingPoint != null)
            {
                if (varName == "Objective")
                {
                    editingPoint.currentObjective.Value = newValue?.ToString() ?? "";
                    success = true;
                }
                else
                {
                    // Add new variable
                    if (origValue == null)
                    {
                        var guid = Guid.NewGuid().ToString();
                        if (varScope == VariableScope.CurrentMainCheckpoint)
                        {
                            for (int i = MainData.checkpoints.Count - 1; i == 0; i--)
                            {
                                if (!IsFarewellCheckpoint(MainData.checkpoints[i].pointIdentifier.Value))//find last checkpoint from main game
                                {
                                    foreach (var variable in MainData.checkpoints[i].variables)
                                    {
                                        if (variable.storyVariable.Value == varId)
                                        {
                                            guid = variable.uniqueId.Value;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }

                        }
                        else if (varScope == VariableScope.CurrentFarewellCheckpoint)
                        {
                            for (int i = MainData.checkpoints.Count - 1; i == 0; i--)
                            {
                                if (IsFarewellCheckpoint(MainData.checkpoints[i].pointIdentifier.Value))//find last checkpoint from Farewell data in main save
                                {
                                    foreach (var variable in MainData.checkpoints[i].variables)
                                    {
                                        if (variable.storyVariable.Value == varId)
                                        {
                                            guid = variable.uniqueId.Value;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }

                        }
                        var varBody = new Dictionary<string, object>
                        {
                            {"uniqueId", guid},
                            {"storyVariable", varId},
                            {"overridesDLC", false },
                            {"currentValue", Convert.ToInt32(newValue)},
                            {"$type", "GameStateVariableModel"}
                        };

                        var freshVar = JObject.FromObject(varBody);
                        ((JArray)editingPoint.variables).Add(freshVar);
                        success = true;
                    }
                    // Remove variable/objective
                    else if (newValue == null)
                    {
                        foreach (var variable in editingPoint.variables)
                        {
                            if (variable.storyVariable.Value == varId)
                            {
                                ((JArray)editingPoint.variables).Remove(variable);
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
                                variable.currentValue.Value = Convert.ToInt32(newValue);
                                success = true;
                                break;
                            }
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
                MainSaveChangesSaved = false;
            }

            return success;
        }

        // Value gets updated inside JSON object (m_Data)
        public bool FindAndUpdateMainFloatValue(string checkpointId, string floatName, float? origValue, float? newValue,
            VariableScope varScope)
        {
            if (IsFarewellCheckpoint(checkpointId))
            {
                if (!MainSaveHasFarewellData)
                {
                    return false;
                }

            }
            else if (MainSaveIsEmpty)
            {
                return false;
            }

            dynamic editingPoint = FindMainEditPoint(varScope, checkpointId);

            if (editingPoint != null)
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

                MainSaveChangesSaved = false;
                return true;
            }

            MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
            return false;
        }

        public bool FindAndUpdateMainFlagValue(string checkpointId, string flagName, bool origValue, VariableScope varScope)
        {

            if (MainSaveIsEmpty)
            {
                return false;
            }

            dynamic editingPoint = FindMainEditPoint(varScope, checkpointId);

            if (editingPoint != null)
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

                MainSaveChangesSaved = false;
                return true;
            }

            MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
            return false;
        }

        public bool FindAndUpdateMainItemValue(string checkpointId, string itemName, bool origValue, VariableScope varScope)
        {
            if (IsFarewellCheckpoint(checkpointId))
            {
                if (!MainSaveHasFarewellData)
                {
                    return false;
                }

            }
            else if (MainSaveIsEmpty)
            {
                return false;
            }

            var itemId = _gameData.GetItemIdByName(itemName);
            dynamic editingPoint = FindMainEditPoint(varScope, checkpointId);

            if (editingPoint != null)
            {
                JArray items = editingPoint.items;
                int? targetIndex = null;

                for (int i=0; i<items.Count; i++)
                {
                    if (((JObject)items[i]).Property("storyItem").Value.ToString() == itemId)
                    {
                        targetIndex = i;
                        break;
                    }
                }

                var newOwner = IsFarewellCheckpoint(checkpointId) ? Consts.Uids.Maxine : Consts.Uids.Chloe;

                if (origValue == false)
                {
                    if (targetIndex == null) // Add new item and make Chloe or Max the owner
                    {
                        var freshItem = new Dictionary<string, object>()
                        {
                            { "uniqueId", Guid.NewGuid().ToString() },
                            { "currentOwnedBy", newOwner },
                            { "overridesDLC", false },
                            { "storyItem", itemId },
                            { "$type", "GameStateItemModel"}
                        };
                        items.Add(JToken.FromObject(freshItem));
                    }
                    else //change the owner of an existing item to Chloe or Max
                    {
                        ((JObject)items[targetIndex]).Property("currentOwnedBy").Value = newOwner;
                    }
                }
                // Remove one of the existing items
                else
                {
                    items[targetIndex].Remove();
                }

                MainSaveChangesSaved = false;
                return true;
            }

            MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
            return false;
        }

        public dynamic FindMainEditPoint(VariableScope varScope, string pointId)
        {
            switch (varScope)
            {
                case VariableScope.Global:
                    return MainData;
                case VariableScope.CurrentMainCheckpoint:
                    return MainData.currentCheckpoint.stateCheckpoint;
                case VariableScope.CurrentFarewellCheckpoint:
                    return MainData.e4Checkpoint.stateCheckpoint;
                default:
                    foreach (var checkpoint in MainData.checkpoints)
                    {
                        if (checkpoint.pointIdentifier.Value == pointId)
                        {
                            return checkpoint;
                        }
                    }
                    break;
            }
            return null;
        }

        public void RestartFromMainCheckpoint(string variablePrefix, dynamic destPoint, int epNumber)
        {
            // Remove variables of future scenes from the minor and major variable list
            FillMinorAndMajorVars(variablePrefix);

            // Erase future main checkpoints from the checkpoint list
            var checkpointsList = new List<JObject>();
            //Add all main checkpoints before the target to a list
            foreach (var checkpoint in MainData.checkpoints)
            {
                if (!IsFarewellCheckpoint(checkpoint.pointIdentifier.Value))
                {
                    checkpointsList.Add(checkpoint);
                    if (checkpoint == destPoint)
                    {
                        break;
                    }
                }
                
            }
            //Add all Farewell checkpoints to that same list
            foreach (var checkpoint in MainData.checkpoints)
            {
                if (IsFarewellCheckpoint(checkpoint.pointIdentifier.Value))
                {
                    checkpointsList.Add(checkpoint);
                }
            }

            MainData.checkpoints = JArray.FromObject(checkpointsList);

            // Copy flags from last (i.e target) checkpoint to global flags
            ((JToken) MainData.flags).Replace(destPoint.flags);
            // Copy float values from last checkpoint to global float values
            ((JToken) MainData.floatValuesDict).Replace(destPoint.floatValuesDict);

            // Set episode states
            string destPointId = destPoint.pointIdentifier;
            for (var i = 0; i < MainData.episodes.Count; i++)
            {
                if (i == (int)Episode.Bonus)
                {
                    continue;
                }

                if (i < epNumber)
                {
                    MainData.episodes[i].episodeState = Consts.EpisodeStates.Finished;
                }
                else if (i == epNumber && !destPointId.EndsWith("End"))
                {
                    MainData.episodes[i].episodeState = Consts.EpisodeStates.InProgress;
                }
                else if (i > epNumber && MainData.episodes[i].episodeState != Consts.EpisodeStates.Unavailable)
                {
                    MainData.episodes[i].episodeState = Consts.EpisodeStates.NotPlayed;
                }
            }

            // Syncronise last checkpoint and global variables
            foreach (var globalVar in MainData.variables)
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
            MainData.uniqueId = Guid.NewGuid();
            MainData.currentObjective = destPoint.currentObjective;

            RewindHeader();
        }
        #endregion

        #region Farewell Save Edit Functions
        public bool FindAndUpdateFarewellVarValue(string checkpointId, string varName, object origValue, object newValue, VariableScope varScope)
        {
            if (FarewellSaveIsEmpty)
            {
                return false;
            }

            var varId = varName == "Objective" ? varName : _gameData.GetVariableIdByName(varName);
            var success = false;
            dynamic editingPoint = FindFarewellEditPoint(varScope, checkpointId);

            if (editingPoint != null)
            {
                if (varName == "Objective")
                {
                    editingPoint.currentObjective.Value = newValue?.ToString() ?? "";
                    success = true;
                }
                else
                {
                    // Add new variable
                    if (origValue == null)
                    {
                        var guid = Guid.NewGuid().ToString();
                        if (varScope == VariableScope.CurrentFarewellCheckpoint)
                        {
                            foreach (var variable in FarewellData.checkpoints[FarewellData.checkpoints.Count - 1].variables)
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
                        {"overridesDLC", false },
                        {"currentValue", Convert.ToInt32(newValue)},
                        {"$type", "GameStateVariableModel"}
                    };

                        var freshVar = JObject.FromObject(varBody);
                        ((JArray)editingPoint.variables).Add(freshVar);
                        success = true;
                    }
                    // Remove variable
                    else if (newValue == null)
                    {
                        foreach (var variable in editingPoint.variables)
                        {
                            if (variable.storyVariable.Value == varId)
                            {
                                ((JArray)editingPoint.variables).Remove(variable);
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
                                variable.currentValue.Value = Convert.ToInt32(newValue);
                                success = true;
                                break;
                            }
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
                FarewellSaveChangesSaved = false;
            }

            return success && FindAndUpdateMainVarValue(checkpointId, varName, origValue, newValue, varScope);
        }

        public bool FindAndUpdateFarewellFloatValue(string checkpointId, string floatName, float? origValue, float? newValue,
            VariableScope varScope)
        {
            if (FarewellSaveIsEmpty)
            {
                return false;
            }

            dynamic editingPoint = FindFarewellEditPoint(varScope, checkpointId);

            if (editingPoint != null)
            {
                // Add new float value
                if (origValue == null)
                {
                    ((JObject)editingPoint.floatValuesDict).Add(floatName, newValue);
                }
                // Remove float value
                else if (newValue == null)
                {
                    ((JObject)editingPoint.floatValuesDict).Remove(floatName);
                }
                // Change float value
                else
                {
                    editingPoint.floatValuesDict[floatName].Value = newValue;
                }

                FarewellSaveChangesSaved = false;
                return true && FindAndUpdateMainFloatValue(checkpointId, floatName, origValue, newValue, varScope);
            }

            MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
            return false;
        }

        public bool FindAndUpdateFarewellFlagValue(string checkpointId, string flagName, bool origValue, VariableScope varScope)
        {
            if (FarewellSaveIsEmpty)
            {
                return false;
            }

            dynamic editingPoint = FindFarewellEditPoint(varScope, checkpointId);

            if (editingPoint != null)
            {
                // Add new flag
                if (origValue == false)
                {
                    ((JArray)editingPoint.flags).Add(flagName);
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

                FarewellSaveChangesSaved = false;
                return true && FindAndUpdateMainFlagValue(checkpointId, flagName, origValue, varScope);
            }

            MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
            return false;
        }

        public bool FindAndUpdateFarewellItemValue(string checkpointId, string itemName, bool origValue, VariableScope varScope)
        {
            if (FarewellSaveIsEmpty)
            {
                return false;
            }
            var itemId = _gameData.GetItemIdByName(itemName);
            dynamic editingPoint = FindFarewellEditPoint(varScope, checkpointId);

            if (editingPoint != null)
            {
                JArray items = editingPoint.items;
                int? targetIndex = null;

                for (int i = 0; i < items.Count; i++)
                {
                    if (((JObject)items[i]).Property("storyItem").Value.ToString() == itemId)
                    {
                        targetIndex = i;
                        break;
                    }
                }

                if (origValue == false)
                {
                    if (targetIndex == null) // Add new item and make Max the owner
                    {
                        var freshItem = new Dictionary<string, object>()
                        {
                            { "uniqueId", Guid.NewGuid().ToString() },
                            { "currentOwnedBy", Consts.Uids.Maxine },
                            { "overridesDLC", false },
                            { "storyItem", itemId },
                            { "$type", "GameStateItemModel"}
                        };
                        items.Add(JToken.FromObject(freshItem));
                    }
                    else //change the owner of an existing item to Max
                    {
                        ((JObject)items[targetIndex]).Property("currentOwnedBy").Value = Consts.Uids.Maxine;
                    }
                }
                // Remove one of the existing items
                else
                {
                    items[targetIndex].Remove();
                }

                FarewellSaveChangesSaved = false;
                return true && FindAndUpdateMainItemValue(checkpointId, itemName, origValue, varScope);
            }

            MessageBox.Show("Could not find checkpoint with pointId " + checkpointId + "!");
            return false;
        }

        public dynamic FindFarewellEditPoint(VariableScope varScope, string pointId)
        {
            switch (varScope)
            {
                case VariableScope.CurrentFarewellCheckpoint:
                    return FarewellData.currentCheckpoint.stateCheckpoint;
                default:
                    foreach (var checkpoint in FarewellData.checkpoints)
                    {
                        if (checkpoint.pointIdentifier.Value == pointId)
                        {
                            return checkpoint;
                        }
                    }
                    break;
            }
            return null;
        }

        public void RestartFromFarewellCheckpoint(string variablePrefix, dynamic destPoint)
        {
            // Erase future checkpoints from the checkpoint list
            var checkpointsList = new List<JObject>();
            
            foreach (var checkpoint in FarewellData.checkpoints)
            {
                checkpointsList.Add(checkpoint);
                if (checkpoint == destPoint)
                {
                    break;
                }
            }

            FarewellData.checkpoints = JArray.FromObject(checkpointsList);

            //Remove all future Farewell checkpoints from main save
            JArray MainCheckpoints = ((JArray)MainData.checkpoints).ToObject<JArray>();

            foreach (var checkpoint in MainData.checkpoints)
            {
                if (IsFarewellCheckpoint(checkpoint.pointIdentifier.Value) && !checkpointsList.Contains(checkpoint))
                {
                    MainCheckpoints.Remove(checkpoint);
                }
            }

            MainData.checkpoints = MainCheckpoints;

            // Copy flags from last (i.e target) checkpoint to global main flags
            ((JToken)MainData.flags).Replace(destPoint.flags);
            // Copy float values from last checkpoint to global main float values
            ((JToken)MainData.floatValuesDict).Replace(destPoint.floatValuesDict);

            // Set EP4 episode state
            if (destPoint.pointIdentifier.Value != "Epiode4End" && MainData.episodes[3].episodeState != Consts.EpisodeStates.Unavailable)
            {
                FarewellData.ep4State = 1;
                MainData.episodes[(int)Episode.Bonus].episodeState = Consts.EpisodeStates.InProgress;
            }

            SyncLastAndCurrentCheckpoints(destPoint, (int)Episode.Bonus);
            MainData.uniqueId = Guid.NewGuid();
            MainData.currentObjective = destPoint.currentObjective;

            RewindHeader();
        }
        #endregion

        public void RewindHeader()
        {
            Header.uniqueId.Value = Guid.NewGuid();
            for (int i = 0; i < MainData.episodes.Count; i++)
            {
                Header.cachedEpisodes[i] = MainData.episodes[i].episodeState;
            }

            Header.saveDate = JArray.FromObject(SaveDate);

            if (MainData.currentCheckpoint.stateCheckpoint.pointIdentifier == "Episode1End" || 
                MainData.currentCheckpoint.stateCheckpoint.pointIdentifier == "Episode2End")
            {
                Header.currentScene = Consts.GlobalCodes.ReadyToStartEpisode;
                Header.currentEpisode = Consts.GlobalCodes.ReadyToStartEpisode;
            }
            else if (MainData.currentCheckpoint.stateCheckpoint.pointIdentifier == "Episode3End")
            {
                Header.currentScene = Consts.GlobalCodes.StoryComplete;
                Header.currentEpisode = Consts.GlobalCodes.StoryComplete;
            }
            else
            {
                Header.currentScene = MainData.currentCheckpoint.currentScene;
                Header.currentEpisode = MainData.currentCheckpoint.currentEpisode;
            }
        }

        #region Rewind sub-functions
        private void FillMinorAndMajorVars(string variablePrefix)
        {
            var minorVarList = new List<string>();
            var majorVarList = new List<string>();

            if (variablePrefix == "E1_")
            {
                MainData.minorChoiceVariables = new JArray();
                MainData.majorChoiceVariables = new JArray();
            }
            else
            {
                foreach (string variable in MainData.minorChoiceVariables)
                {
                    if (variable.StartsWith(variablePrefix))
                    {
                        break;
                    }

                    minorVarList.Add(variable);
                }
                foreach (string variable in MainData.majorChoiceVariables)
                {
                    if (variable.StartsWith(variablePrefix))
                    {
                        break;
                    }

                    majorVarList.Add(variable);
                }
                MainData.minorChoiceVariables = JArray.FromObject(minorVarList);
                MainData.majorChoiceVariables = JArray.FromObject(majorVarList);
            }
        }

        private void SyncLastAndCurrentCheckpoints(dynamic destPoint, int epNumber)
        {
            if (!IsFarewellCheckpoint(destPoint.pointIdentifier.Value))
            {
                MainData.currentCheckpoint.stateCheckpoint = destPoint;

                if (IsMainAtMidLevel)
                {
                    IsMainAtMidLevel = false;
                    ((JArray)MainData.currentCheckpoint.visitedNodes).Replace(new JArray());
                }

                if (destPoint.pointIdentifier == "Episode1End")
                {
                    MainData.currentCheckpoint.currentScene = "e1_s10_b";
                }
                else if (destPoint.pointIdentifier == "Episode2End")
                {
                    MainData.currentCheckpoint.currentScene = "e2_s07";
                }
                else if (destPoint.pointIdentifier == "Episode3End")
                {
                    MainData.currentCheckpoint.currentScene = "e3_s08";
                }
                else
                {
                    string id = destPoint.pointIdentifier;
                    MainData.currentCheckpoint.currentScene = id.ToLowerInvariant();
                }
                MainData.currentCheckpoint.currentEpisode = "E" + (epNumber + 1);
            }

            else
            {
                FarewellData.currentCheckpoint.stateCheckpoint = destPoint;

                if (IsFarewellAtMidLevel)
                {
                    IsFarewellAtMidLevel = false;
                    ((JArray)FarewellData.currentCheckpoint.visitedNodes).Replace(new JArray());
                }
                string id = destPoint.pointIdentifier;
                FarewellData.currentCheckpoint.currentScene = id.ToLowerInvariant();

                MainData.e4Checkpoint = FarewellData.currentCheckpoint;
            }
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
            {"E3_S06", "E3_S07_"}
        };

        private bool ShouldGlobalVarBeReseted(string storyVarId, string pointId)
        {
            var untouchableVars = new List<string>();
            string prefix;

            if (IsFarewellCheckpoint(pointId))
            {
                return false;
            }

            if (!_globalVarPrefixes.TryGetValue(pointId, out prefix))
            {
                return false;
            }

            foreach (var graffitiVar in Consts.GraffitiVariableNames)
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
