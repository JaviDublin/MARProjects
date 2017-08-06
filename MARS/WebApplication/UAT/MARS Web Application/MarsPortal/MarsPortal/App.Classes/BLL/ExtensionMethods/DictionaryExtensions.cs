using System;
using System.Collections.Generic;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.Classes.BLL.ExtensionMethods
{
    internal static class DictionaryExtensions
    {
        internal static void AddOrUpdateInDictionary(this Dictionary<string, string> dictionary, string key, string value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
            else
            {
                dictionary[key] = value;
            }
        }

        internal static bool ContainsValueAndIsntEmpty(this Dictionary<DictionaryParameter, string> dictionary, DictionaryParameter parameter)
        {
            return dictionary.ContainsKey(parameter) && dictionary[parameter] != string.Empty;
        }

        internal static int GetIdFromDictionary(this Dictionary<string, string> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                int returned;
                int.TryParse(dictionary[key], out returned);
                return returned;
            }
            return 0;
        }

        internal static int GetIdFromDictionary(this Dictionary<DictionaryParameter, string> dictionary, DictionaryParameter key)
        {
            if (dictionary.ContainsKey(key))
            {
                int returned;
                int.TryParse(dictionary[key], out returned);
                return returned;
            }
            return 0;
        }

        internal static DateTime GetDateFromDictionary(this Dictionary<DictionaryParameter, string> dictionary, DictionaryParameter key)
        {
            if (dictionary.ContainsKey(key))
            {
                DateTime returned;
                DateTime.TryParse(dictionary[key], out returned);
                return returned;
            }
            return DateTime.MinValue;
        }


    }
}