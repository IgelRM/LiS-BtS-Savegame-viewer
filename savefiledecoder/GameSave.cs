using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Helpers;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace savefiledecoder
{

    public class VariableState
    {
        public string Name { get; set; }
        public int? Value { get; set; } //? is nullable. it means that the variable can be empty (without value)
    }

    public class Checkpoint
    {
        dynamic m_CheckPoint; //checkpoint object. possibly holding checkpoint name
        private Dictionary<String, VariableState> m_Variables = null;
        private List<string> m_flags = null;

        public Checkpoint(dynamic checkpoint, Dictionary<String, VariableState> variables)
        {
            m_CheckPoint = checkpoint;
            m_Variables = variables;
        }

        public Checkpoint(dynamic checkpoint, Dictionary<String, VariableState> variables, List<string> flags)
        {
            m_CheckPoint = checkpoint;
            m_Variables = variables;
            m_flags = flags;
        }
        public string PointIdentifier
        {
            get
            {
                return m_CheckPoint.pointIdentifier;
            }

        }
        public string Objective
        {
            get
            {
                return m_CheckPoint.currentObjective;
            }
        }

        public Dictionary<String, VariableState> Variables
        {
            get
            {
                return m_Variables;
            }
        }

        public List<string> Flags
        {
            get
            {
                return m_flags;
            }
        }
    }


    public class GameSave
    {
        GameData m_GameData;

        public Dictionary<String, bool> EpisodePlayed { get; } = new Dictionary<String, bool>(); //string is the name of an episode, e.g E1, E2. bool is -played or not.

        public List<Checkpoint> Checkpoints { get; } = new List<Checkpoint>();

        public List<string> list_EpStates = new List<string>();
        public int[] dateofSave = new int[3];

        public string[] episodeNames = new string[]
        {
            "Epsiode 1: Awake",
            "Episode 2: Brave New World",
            "Epsiode 3: Hell is Empty",
            "Bonus Episode: Farewell"
        };

        public string[] pointNames = new string[]
        {
            "Old Mill - Exterior", //episode 1
            "Old Mill - Interior",
            "Price House - Upstairs",
            "Price House - Downstairs",
            "First Dream",
            "School Campus",
            "School Drama Lab",
            "Train",
            "Overlook",
            "Junkyard",
            "Second Dream",
            "Junkyard - Night",
            "Overlook - Night",
            "Episode 1 Ending",
            "Principal's Office", //episode 2
            "Blackwell Parking Lot",
            "Junkyard",
            "Dream",
            "Junkyard - Later",
            "Frank's RV",
            "Dormitories (Outside)",
            "Boys' Dormitories",
            "Campus - Backstage",
            "The Tempest",
            "Neighborhood",
            "Amber House",
            "Episode 2 Ending"
         };

        public Dictionary<string, string> varStartDict = new Dictionary<string, string>()
        {
            {"E1_S01_A", "E1_" },
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
            {"Episode2End", "E3_"}
        };

        public GameSave(GameData gameData)
        {
            m_GameData = gameData; //this is the initialdata, NOT savefile
        }

        public dynamic m_Data;
        public dynamic m_Header;
        public string Raw { get; set; }
        public string h_Raw { get; set; }
        public bool SaveEmpty {get; set;}
        private MD5 contenthash =MD5.Create();
        public void Read(string path)
        {
            SaveEmpty = false;
            Checkpoints.Clear();
            EpisodePlayed.Clear();
            list_EpStates.Clear();

            // read and decode Data
            byte[] file = File.ReadAllBytes(path);
            byte[] decoded = DecodeEncode.Decode(file); // decoded is - dexored content only (since file starts with header)
            Raw = Encoding.UTF8.GetString(decoded);

            try
            {
                m_Data = Json.Decode(Raw);  //this is the save file, NOT initialdata
            }
            catch
            {
                SaveEmpty = true;
                return;
            }

            // add normal checkpoints
            foreach (var checkpoint in m_Data.checkpoints)
            {
                List<string> flags = new List<string>();
                var vars = ReadVarsForCheckpoint(checkpoint);
                foreach (var fl in checkpoint.flags)
                {
                    flags.Add(fl);
                }
                Checkpoints.Add(new Checkpoint(checkpoint, vars, flags));
            }

            // add currentcheckpoint (seems to be identical to latest checkpoint...)

            Dictionary<string, VariableState> variables;
            try
            {
                List<string> flags = new List<string>();
                foreach (var fl in m_Data.currentCheckpoint.stateCheckPoint.flags)
                {
                    flags.Add(fl);
                }
                variables = ReadVarsForCheckpoint(m_Data.currentCheckpoint.stateCheckPoint);
                Checkpoints.Add(new Checkpoint(m_Data.currentCheckpoint.stateCheckPoint, variables, flags));
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                SaveEmpty = true;
                return;
            }

            // add global variables as a last checkpoint.. // hack...
            variables = ReadVarsForCheckpoint(m_Data);
            Checkpoints.Add(new Checkpoint(new { pointIdentifier = "Global Vars", currentObjective = "" }, variables));

            // fill episodeState (?)
            foreach (var episode in m_Data.episodes)
            {
                string name = episode.name;
                string stateStr = episode.episodeState;
                bool played = false;
                if (stateStr == "kFinished" || stateStr == "kInProgress")
                {
                    played = true;
                }
                EpisodePlayed[name] = played;
            }
            if (File.Exists(Path.GetDirectoryName(path) + @"\Header.Save"))
            {
                ReadHeader(Path.GetDirectoryName(path) + @"\Header.Save");
            }
            else
            {
                m_Header = null;
            }
        }

        public void ReadHeader (string h_path)
        {
            //read and decode data
            byte[] h_file = File.ReadAllBytes(h_path);
            byte[] h_decoded = DecodeEncode.Decode(h_file); // decoded is - dexored content only (since file starts with those 4 bytes)
            h_Raw = Encoding.UTF8.GetString(h_decoded);

            try
            {
                m_Header = Json.Decode(h_Raw);  //this is the header file
            }
            catch
            {
                SaveEmpty = true;
            }

            //fill episodestates
            foreach (var episode in m_Header.cachedEpisodes)
            {
                list_EpStates.Add(episode);
            }

            //read the date of the save
            for (int i = 0; i < m_Header.saveDate.Length; i++)
            {
                dateofSave[i] = m_Header.saveDate[i]; //need to test if it's possible to write to this dynamic array without an intermediate one
            }
        }

        public bool editsSaved = true, h_editsSaved = true;
        public void WriteData (string path, dynamic json_data)
        {
            Raw = Newtonsoft.Json.JsonConvert.SerializeObject(json_data, Newtonsoft.Json.Formatting.Indented); //Raw is a utf8 string.
            byte[] content = Encoding.UTF8.GetBytes(Raw); //dexored (for now) new content
            byte[] chash = contenthash.ComputeHash(content); //md5 hash of dexored new content
            byte[] encoded = DecodeEncode.Decode(content); //xor-ed new content. XOR functions can be applied on data to repeatedly encryptt and decrypt it.
            byte[] modded_file = new byte[20 + encoded.Length];
            byte[] file_header = new byte[4] { 81, 55, 110, 170 };

            int num = 0;
            int i;
            for (i = 0; i < file_header.Length + num; i++)
            {
                modded_file[i] = file_header[i - num];
            }
            num = i;
            while (i < 16 + num)
            {
                modded_file[i] = chash[i - num];
                i++;
            }
            num = i;
            while (i < encoded.Length + num)
            {
                modded_file[i] = encoded[i - num];
                i++;
            }
            if (!File.Exists(path + @".bkp"))
            {
                File.Copy(path, path + @".bkp", false);
            }
            
            File.WriteAllBytes(path, modded_file); //write changes to Data.Save
            editsSaved = true;
        }

        public void WriteHeader (string h_path, dynamic h_json_data)
        {
            h_Raw = Newtonsoft.Json.JsonConvert.SerializeObject(h_json_data, Newtonsoft.Json.Formatting.Indented);
            byte[] content = Encoding.UTF8.GetBytes(h_Raw);
            byte[] chash = contenthash.ComputeHash(content);
            byte[] encoded = DecodeEncode.Decode(content);
            byte[] modded_hfile = new byte[20 + encoded.Length];
            byte[] file_header = new byte[4] { 81, 55, 110, 170 };

            int num = 0;
            int i;
            for (i = 0; i < file_header.Length + num; i++)
            {
                modded_hfile[i] = file_header[i - num];
            }
            num = i;
            while (i < 16 + num)
            {
                modded_hfile[i] = chash[i - num];
                i++;
            }
            num = i;
            while (i < encoded.Length + num)
            {
                modded_hfile[i] = encoded[i - num];
                i++;
            }
            if (!File.Exists(h_path + @".bkp"))
            {
                File.Copy(h_path, h_path + @".bkp", false);
            }

            File.WriteAllBytes(h_path, modded_hfile); //write changes to Header.Save
            h_editsSaved = true;
        }

        public Dictionary<String, VariableState> ReadVarsForCheckpoint(dynamic checkpoint)
        {
            var variablesInPoint = new Dictionary<String, VariableState>(); //string is the variable name, VariableState is a name-value pair
        
            
            List<dynamic> checkpoints = new List<dynamic>(m_Data.checkpoints);
            dynamic variables = checkpoint.variables;

            foreach (var variable in variables)
            {
                int value = variable.currentValue;
                string gameVariableId = variable.storyVariable;
                string name = m_GameData.GetVariableName(gameVariableId);
                variablesInPoint[name]= (new VariableState() { Value = value, Name = name });
            }

            return variablesInPoint;
        }
        
        public bool FindAndUpdateVarValue (string checkpoint_id, string var_name, int? orig_value, int? new_value, string cell_type) //value gets updated inside JSON object (m_Data)
        {
            dynamic goodpoint;
            string var_id = m_GameData.GetVariableID(var_name);
            bool pointFound = false, success = false;
             
            if (cell_type == "global")
            {
                goodpoint = m_Data;
                pointFound = true;
                
            }
            else if (cell_type == "current")
            {
                goodpoint = m_Data.currentCheckpoint.stateCheckPoint;
                pointFound = true;
            }
            
            else
            {
                goodpoint = m_Data.checkpoints[0]; //assign some value to the variable;
                foreach (var checkpoint in m_Data.checkpoints)
                {
                    if (checkpoint.pointIdentifier == checkpoint_id)
                    {
                        goodpoint = checkpoint;
                        pointFound = true;
                        break;
                    }
                }
            }
            
            if (pointFound)
            {
                if (orig_value == null) //add new variable
                {
                    string guid = Guid.NewGuid().ToString();
                    if (cell_type == "current")
                    {
                        foreach (var variable in m_Data.checkpoints[m_Data.checkpoints.Length-1].variables)
                        {
                            if (variable.storyVariable == var_id)
                            {
                                guid = variable.uniqueId;
                            }
                        }
                    }
                    Dictionary<string, object> var_body = new Dictionary<string, object>()
                    {
                        {"uniqueId", guid},
                        {"storyVariable", var_id},
                        {"currentValue", new_value},
                        {"$type", "GameStateVariableModel"}
                    };

                    DynamicJsonObject fresh_var = new DynamicJsonObject(var_body);
                    List<DynamicJsonObject> objlist = new List<DynamicJsonObject>();
                    foreach (var variable in goodpoint.variables)
                    {
                        objlist.Add(variable);
                    }
                    objlist.Add(fresh_var);
                    DynamicJsonArray modded_array = new DynamicJsonArray(objlist.ToArray<object>());
                    goodpoint.variables = modded_array;
                    success = true;
                }
                else if (new_value == null) //remove variable
                {
                    List<DynamicJsonObject> objlist = new List<DynamicJsonObject>();
                    foreach (var variable in goodpoint.variables)
                    {
                        if (variable.storyVariable != var_id) // add all variables except the one we want to remove
                        {
                            objlist.Add(variable);
                        }
                    }
                    DynamicJsonArray modded_array = new DynamicJsonArray(objlist.ToArray<object>());
                    goodpoint.variables = modded_array;
                    success = true;
                }
                else //change variable value
                {
                    foreach (var variable in goodpoint.variables)
                    {
                        if (variable.storyVariable == var_id)
                        {
                            variable.currentValue = new_value;
                            success = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Could not find checkpoint with pointid" + checkpoint_id + "!");
            }
            if (!success)
            {
                MessageBox.Show("Could not find and replace variable with ID " + var_id + "!");
            }
            else
            {
                editsSaved = false;
            }
            return success;
        }

        public bool FindAndUpdateFlagValue(string checkpoint_id, string flag_name, bool origValue, string cell_type)
        {
            dynamic goodpoint;
            bool pointFound = false, success = false;
            if (cell_type == "current")
            {
                goodpoint = m_Data.currentCheckpoint.stateCheckPoint;
                pointFound = true;
            }

            else
            {
                goodpoint = m_Data.checkpoints[0]; //assign some value to the variable;
                foreach (var checkpoint in m_Data.checkpoints)
                {
                    if (checkpoint.pointIdentifier == checkpoint_id)
                    {
                        goodpoint = checkpoint;
                        pointFound = true;
                        break;
                    }
                }
            }
            if (pointFound)
            {
                List<string> flaglist = new List<string>();
                foreach (var flag in goodpoint.flags)
                {
                    flaglist.Add(flag);
                }
                if (origValue == false) //add new flag
                {
                    flaglist.Add(flag_name);
                }
                else //remove one of the existing flags
                {
                    flaglist.Remove(flag_name);
                }
                DynamicJsonArray modded_array = new DynamicJsonArray(flaglist.ToArray<object>());
                goodpoint.flags = modded_array;
                success = true;
            }
            else
            {
                MessageBox.Show("Could not find checkpoint with pointid" + checkpoint_id + "!");
            }
            if (!success)
            {
                MessageBox.Show("Could not find and update flag with ID " + flag_name + "!");
            }
            else
            {
                editsSaved = false;
            }
            return success;
        }
        
        public void RestartFromCheckpoint (string varStart, dynamic destPoint, int epNumber)
        {
            rw_CleanMinorAndMajorVars(varStart); //remove variables of future scenes from the minor and major variable list

            //erase future checkpoints from the checkpoint list
            List<DynamicJsonObject> checkpointsList = new List<DynamicJsonObject>();
            foreach (var checkpoint in m_Data.checkpoints)
            {
                checkpointsList.Add(checkpoint);
                if (checkpoint == destPoint)
                {
                    break;
                }
            }
            m_Data.Checkpoints = checkpointsList.ToArray();

            m_Data.flags = destPoint.flags; //copy flags from last checkpoint to global flags
            m_Data.floatValuesDict = destPoint.floatValuesDict; //copy floatvalues from last checkpoint to global floatvalues

            //set episodestates
            string destPointID = destPoint.pointIdentifier;
            for (int i=0; i<m_Data.episodes.Length; i++)
            {
                if (i < epNumber)
                {
                    m_Data.episodes[i].episodeState = "kFinished";
                    continue;
                }
                else if (i == epNumber && !destPointID.Contains("End"))
                {
                    m_Data.episodes[i].episodeState = "kInProgress";
                    continue;
                }
                else if (i > epNumber && m_Data.episodes[i].episodeState != "kUnavailable")
                {
                    m_Data.episodes[i].episodeState = "kNotPlayed";
                    continue;
                }
            }
            //syncronise last checkpoint and global variables
            foreach (var globalvar in m_Data.variables)
            {
                foreach (var variable in destPoint.variables)
                {
                    if (variable.storyVariable == globalvar.storyVariable)
                    {
                        globalvar.currentValue = variable.currentValue;
                        break;
                    }
                    else if (rw_ShouldGlobalVarBeAltered(globalvar.storyVariable, destPointID))
                    {
                        globalvar.currentValue = 0;
                    }
                }
            }

            rw_SyncLastAndCurrent(destPoint, epNumber);
            m_Data.uniqueId = System.Guid.NewGuid();
            m_Data.currentObjective = destPoint.currentObjective;

            RewindHeader();
        }

        public void RewindHeader()
        {
            m_Header.uniqueId = System.Guid.NewGuid();
            for (int i=0; i< m_Data.episodes.Length; i++)
            {
                m_Header.cachedEpisodes[i] = m_Data.episodes[i].episodeState;
            }
            m_Header.saveDate = dateofSave;
            if (m_Data.currentCheckpoint.stateCheckpoint.pointIdentifier == "Episode1End" || m_Data.currentCheckpoint.stateCheckpoint.pointIdentifier == "Episode2End")
            {
                m_Header.currentScene = "GLOBAL_CODE_READYTOSTARTEPISODE";
                m_Header.currentEpisode = "GLOBAL_CODE_READYTOSTARTEPISODE";
            }
            else
            {
                m_Header.currentScene = m_Data.currentCheckpoint.currentScene;
                m_Header.currentEpisode = m_Data.currentCheckpoint.currentEpisode;
            }
        }

        //sub-functions
        private void rw_CleanMinorAndMajorVars (string varStart)
        {
            List<string> minorVarList = new List<string>();
            List<string> majorVarList = new List<string>();

            if (varStart == "E1_")
            {
                m_Data.minorChoiceVariables = null;
                m_Data.majorChoiceVariables = null;
            }
            else
            {
                foreach (string variable in m_Data.minorChoiceVariables)
                {
                    if (variable.StartsWith(varStart))
                    {
                        break;
                    }
                    else minorVarList.Add(variable);
                }
                foreach (string variable in m_Data.majorChoiceVariables)
                {
                    if (variable.StartsWith(varStart))
                    {
                        break;
                    }
                    else majorVarList.Add(variable);
                }
                m_Data.minorChoiceVariables = minorVarList.ToArray();
                m_Data.majorChoiceVariables = majorVarList.ToArray();
            }
        }

        private void rw_SyncLastAndCurrent (dynamic destPoint, int epNumber)
        {
            m_Data.currentCheckpoint.stateCheckpoint = destPoint;
            if (m_Data.currentCheckpoint.hasMidLevelData == true)
            {
                m_Data.currentCheckpoint.hasMidLevelData = false;
                m_Data.currentCheckpoint.visitedNodes = null;
            }

            if (destPoint.pointIdentifier == "Episode1End")
            {
                m_Data.currentCheckpoint.currentScene = "e1_s10_b";
            }
            else if (destPoint.pointIdentifier == "Episode2End")
            {
                m_Data.currentCheckpoint.currentScene = "e2_s07";
            }
            //add ep3 and ep4 info here when they are out
            else
            {
                string id = destPoint.pointIdentifier;
                m_Data.currentCheckpoint.currentScene = id.ToLowerInvariant();
            }
            m_Data.currentCheckpoint.currentEpisode = "E" + (epNumber+1).ToString();
        }

        public Dictionary<string, string> globalVarStartDict = new Dictionary<string, string>()
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
            {"Episode2End", "E3_"}
        };

        string[] grafiitiVars = new string[]
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
            "E2_S07_JAMESGRAFFITI"
        };

        private bool rw_ShouldGlobalVarBeAltered (string storyID, string pointID)
        {
            string limit = String.Empty;
            List<string> goodVars = new List<string>();

            globalVarStartDict.TryGetValue(pointID, out limit);
            foreach (string variable in grafiitiVars)
            {
                if (variable.StartsWith(limit))
                {
                    break;
                }
                else goodVars.Add(variable);
            }

            if (storyID == m_GameData.GetVariableID("E1_S01_CI_PUNKJACKET") || storyID == m_GameData.GetVariableID("E1_S06_CHLOESTICKSUPFORHERSELF"))
            {
                return false;
            }
            else if (goodVars.Contains(m_GameData.GetVariableName(storyID)))
            {
                return false;
            }
            else return true;
        }
    }
}
