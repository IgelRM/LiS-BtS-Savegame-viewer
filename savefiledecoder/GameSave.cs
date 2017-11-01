using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Helpers;

namespace savefiledecoder
{

    class VariableState
    {
        public string Name { get; set; }
        public int? Value { get; set; }
    }

    class Checkpoint
    {
        dynamic m_CheckPoint;
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

        public Dictionary<String, bool> EpisodePlayed { get; } = new Dictionary<String, bool>();

        public List<Checkpoint>  Checkpoints { get; } = new List<Checkpoint>();

        public GameSave(GameData gameData)
        {
            m_GameData = gameData;
        }

        dynamic m_Data;
        public string Raw { get; private set; }

        public void Read(string path)
        {
            Checkpoints.Clear();
            EpisodePlayed.Clear();

            // read and decode Data
            byte[] file = File.ReadAllBytes(path);
            byte[] decoded = DecodeEncode.Decode(file);
            Raw = Encoding.UTF8.GetString(decoded);
            m_Data = Json.Decode(Raw); 

            // add normal checkpoints
            foreach(var checkpoint in m_Data.checkpoints)
            {
                var vars = ReadVarsForCheckpoint(checkpoint);
                Checkpoints.Add(new Checkpoint(checkpoint, vars));
            }

            // add currentcheckpoint (seems to be identical to latest checkpoint...)
            var variables = ReadVarsForCheckpoint(m_Data.currentCheckpoint.stateCheckPoint);
            Checkpoints.Add(new Checkpoint(m_Data.currentCheckpoint.stateCheckPoint, variables));

            // add global variables as a last checkpoint.. // hack...
            variables = ReadVarsForCheckpoint(m_Data);
            Checkpoints.Add(new Checkpoint(new { pointIdentifier = "Global Vars" , currentObjective = ""}, variables));

            // fill episodeState (?)
            foreach (var episode in m_Data.episodes)
            {
                string name = episode.name;
                string stateStr = episode.episodeState;
                bool played = false;
                if(stateStr== "kFinished" || stateStr== "kInProgress")
                {
                    played = true;
                }
                EpisodePlayed[name] = played;
            }

        }

        public Dictionary<String, VariableState> ReadVarsForCheckpoint(dynamic checkpoint)
        {
            var variablesInPoint = new Dictionary<String, VariableState>();

          
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
    }
}
