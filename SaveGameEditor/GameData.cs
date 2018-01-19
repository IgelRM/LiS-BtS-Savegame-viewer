using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace SaveGameEditor
{
    public class GameVariable
    {
        /// <summary>
        /// Unique variable ID
        /// </summary>
        public string Id;

        /// <summary>
        /// Variable name, eg: E1_APOLOGIZETOBOUNCER
        /// </summary>
        public string Name;
    }

    public class GameData
    {
        private const string UnknownVariableIdPrefix = "UNKNOWN-";

        /// <summary>
        /// Key is a Variable ID
        /// </summary>
        private Dictionary<string, GameVariable> _variables { get; } = new Dictionary<string, GameVariable>();

        /// <summary>
        /// Key is a Variable Name
        /// </summary>
        private Dictionary<string, GameVariable> _varNames { get; } = new Dictionary<string, GameVariable>();

        public string Raw { get; set; }

        public void ReadFromFile(string path)
        {
            var fileContent = File.ReadAllBytes(path);
            dynamic json = JsonConverter.DecodeFileContentToJson(fileContent);
            Raw = json.ToString();

            dynamic variables = json.variables;
            foreach (var variable in variables)
            {
                if (variable["$type"] == "StoryVariable")
                {
                    AddVariable(variable.uniqueId.Value, variable.objectName.Value);
                }
            }
        }

        /// <summary>
        /// Add a new variable to a list with given ID and Name. 
        /// If Name is not specified, uses ID as a Name.
        /// If variable with given ID and Name already exists, returns found entry.
        /// </summary>
        /// <param name="id">Variable ID</param>
        /// <param name="name">Variable Name</param>
        /// <returns>Created or found variable</returns>
        private GameVariable AddVariable(string id, string name = null)
        {
            name = name ?? id;
            if (_variables.ContainsKey(id))
            {
                return _variables[id].Name == name ? _variables[id] : null;
            }

            var newVariable = new GameVariable
            {
                Id = id,
                Name = name
            };

            _variables[id] = newVariable;
            _varNames[name] = newVariable;

            return newVariable;
        }

        public Dictionary<string, GameVariable> GetVariables()
        {
            return _variables;
        }

        /// <summary>
        /// Get variable name
        /// </summary>
        /// <param name="id">Variable ID</param>
        /// <returns>Variable name</returns>
        public string GetOrCreateVariableNameById(string id)
        {
            // If a variable with the given ID was NOT found in the initialdata, then create a new variable. 
            if (_variables.ContainsKey(id))
            {
                return _variables[id].Name;
            }

            var unknownVarId = UnknownVariableIdPrefix + id;
            if (_variables.ContainsKey(unknownVarId))
            {
                return _variables[unknownVarId].Name;
            }

            return AddVariable(unknownVarId)?.Name;
        }

        /// <summary>
        /// Get variable ID
        /// </summary>
        /// <param name="name">Variable name</param>
        /// <returns>Variable ID</returns>
        public string GetVariableIdByName(string name)
        {
            return _varNames[name].Id;
        }
    }
}
