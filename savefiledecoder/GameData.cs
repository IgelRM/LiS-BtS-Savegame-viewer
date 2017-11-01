using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Helpers;


namespace savefiledecoder
{
    class GameVariable
    {
        public string id;
        public string name;
    }


    class GameData
    {
        public Dictionary<string, GameVariable> Variables { get { return m_Variables; } }
        Dictionary<string, GameVariable> m_Variables  = new Dictionary<string, GameVariable>();


        public void Read(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            byte[] decoded = DecodeEncode.Decode(file);
            string str = Encoding.UTF8.GetString(decoded);
            dynamic json = Json.Decode(str);
            dynamic variables = json.variables;
            foreach(dynamic variable in variables)
            {
                if(variable["$type"] == "StoryVariable")
                {
                    m_Variables[variable.uniqueId] = new GameVariable { name = variable.objectName, id = variable.uniqueId };
                }
            }
        }

        public string GetVariableName(string id)
        {
            if(!m_Variables.ContainsKey(id))
            {
                string name = "UNKNOWN-" + id;
                m_Variables["UNKNOWN-id"] = new GameVariable { name = name, id = id };
                return name;
            }
            else
                return m_Variables[id].name;
        }
    }
}
