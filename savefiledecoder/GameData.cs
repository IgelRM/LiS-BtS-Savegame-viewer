using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace savefiledecoder
{
    public class GameVariable
    {
        public string id; //unique-id
        public string name; //variable name, eg: E1_APOLOGIZETOBOUNCER
    }


    public class GameData
    {
        public Dictionary<string, GameVariable> Variables { get { return m_Variables; } }
        public Dictionary<string, GameVariable> Varnames { get { return m_Varnames; } }
        Dictionary<string, GameVariable> m_Variables  = new Dictionary<string, GameVariable>(); //string is the variable ID here
        Dictionary<string, GameVariable> m_Varnames = new Dictionary<string, GameVariable>(); //string is the variable name here

        public string Raw { get; set; }
        public dynamic json;
        public void Read(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            byte[] decoded = DecodeEncode.Decode(file);
            Raw = Encoding.UTF8.GetString(decoded); //convert the byte array to a string
            json = JsonConvert.DeserializeObject(Raw);
            dynamic variables = json.variables;
            foreach(dynamic variable in variables)
            {
                if(variable["$type"] == "StoryVariable")
                {
                    m_Variables[variable.uniqueId.Value] = new GameVariable { name = variable.objectName.Value, id = variable.uniqueId.Value};
                    m_Varnames[variable.objectName.Value] = new GameVariable { name = variable.objectName.Value, id = variable.uniqueId.Value};

                }
            }
        }

        public string GetVariableName(string id) //this is called from elsewhere to get variable names.
        {
            if(!m_Variables.ContainsKey(id)) //if a variable with the given ID was NOT found in the initialdata, then create a new variable. 
            {
                string name = "UNKNOWN-" + id;
                m_Variables["UNKNOWN-id"] = new GameVariable { name = name, id = id };
                return name;
            }
            else
                return m_Variables[id].name;
        }
        public string GetVariableID(string vname) //this is called from elsewhere to get variable IDs.
        {
                return m_Varnames[vname].id;
        }
    }
}
