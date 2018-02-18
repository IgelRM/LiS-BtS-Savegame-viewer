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

    public class GameItem
    {
        /// <summary>
        /// Unique item ID
        /// </summary>
        public string Id;

        /// <summary>
        /// Item name, eg: e1_s01_beer
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

        /// <summary>
        /// Key is a Item ID
        /// </summary>
        private Dictionary<string, GameItem> _items { get; } = new Dictionary<string, GameItem>();

        /// <summary>
        /// Key is a Item Name
        /// </summary>
        private Dictionary<string, GameItem> _itemNames { get; } = new Dictionary<string, GameItem>();

        public string Raw { get; set; }

        public void ReadFromFile(string path)
        {
            var fileContent = File.ReadAllBytes(path);
            dynamic json = JsonConverter.DecodeFileContentToJson(fileContent);
            Raw = json.ToString();

            foreach (var variable in json.variables)
            {
                AddVariable(variable.uniqueId.Value, variable.objectName.Value);
            }
            foreach (var item in json.items)
            {
                AddItem(item.uniqueId.Value, item.objectName.Value);
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

        private GameItem AddItem(string id, string name = null)
        {
            name = name ?? id;
            if (_items.ContainsKey(id))
            {
                return _items[id].Name == name ? _items[id] : null;
            }

            var newItem = new GameItem
            {
                Id = id,
                Name = name
            };

            _items[id] = newItem;
            _itemNames[name] = newItem;

            return newItem;
        }

        public Dictionary<string, GameVariable> GetVariables()
        {
            return _variables;
        }

        public Dictionary<string, GameItem> GetItems()
        {
            return _items;
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

        /// <summary>
        /// Get item name
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>Item name</returns>
        public string GetOrCreateItemNameById(string id)
        {
            // If an item with the given ID was NOT found in the initialdata, then create a new item. 
            if (_items.ContainsKey(id))
            {
                return _items[id].Name;
            }

            var unknownItemId = UnknownVariableIdPrefix + id;
            if (_items.ContainsKey(unknownItemId))
            {
                return _items[unknownItemId].Name;
            }

            return AddItem(unknownItemId)?.Name;
        }

        /// <summary>
        /// Get item ID
        /// </summary>
        /// <param name="name">Item name</param>
        /// <returns>Item ID</returns>
        public string GetItemIdByName(string name)
        {
            return _itemNames[name].Id;
        }
    }
}
