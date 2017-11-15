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

    class VariableState
    {
        public string Name { get; set; }
        public int? Value { get; set; } //? is nullable. it means that the variable can be empty (without value)
    }

    class Checkpoint
    {
        dynamic m_CheckPoint; //checkpoint object. possibly holding checkpoint name
        private Dictionary<String, VariableState> m_Variables = null;

        public Checkpoint(dynamic checkpoint, Dictionary<String, VariableState> variables)
        {
            m_CheckPoint = checkpoint;
            m_Variables = variables;
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
    }


    class GameSave
    {
        GameData m_GameData;

        public Dictionary<String, bool> EpisodePlayed { get; } = new Dictionary<String, bool>(); //string is the name of an episode, e.g E1, E2. bool is -played or not.

        public List<Checkpoint> Checkpoints { get; } = new List<Checkpoint>();

        public GameSave(GameData gameData)
        {
            m_GameData = gameData; //this is the initialdata, NOT savefile
        }

        public dynamic m_Data;
        public string Raw { get; private set; }
        public static bool SaveEmpty {get; set;}
        private MD5 contenthash =MD5.Create();
        public void Read(string path)
        {
            Checkpoints.Clear();
            EpisodePlayed.Clear();

            // read and decode Data
            byte[] file = File.ReadAllBytes(path);
            byte[] decoded = DecodeEncode.Decode(file); // decoded is - dexored content only (since file starts with header)
            Raw = Encoding.UTF8.GetString(decoded);

            m_Data = Json.Decode(Raw);  //this is the save file, NOT initialdata


            // add normal checkpoints
            foreach (var checkpoint in m_Data.checkpoints)
            {
                var vars = ReadVarsForCheckpoint(checkpoint);
                Checkpoints.Add(new Checkpoint(checkpoint, vars));
            }

            // add currentcheckpoint (seems to be identical to latest checkpoint...)

            Dictionary<string, VariableState> variables;
            SaveEmpty = false;
            try
            {
                variables = ReadVarsForCheckpoint(m_Data.currentCheckpoint.stateCheckPoint);
                Checkpoints.Add(new Checkpoint(m_Data.currentCheckpoint.stateCheckPoint, variables));
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                SaveEmpty = true;
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

        }
        public bool editsSaved = true;
        public void Write (string path, dynamic json_data)
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
        
        public bool FindAndUpdateVarValue (string checkpoint_id, string var_name, int? new_value, string cell_type) //value gets updated inside JSON object (m_Data)
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
                    }
                }
            }

            if (pointFound)
            {
                foreach (var variable in goodpoint.variables)
                {
                    if (variable.storyVariable == var_id)
                    {
                        variable.currentValue = new_value;
                        success = true;
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
            editsSaved = false;
            return success;
        }
    }
}
