using System;
using System.Collections.Generic;
using System.Linq;

namespace SyllabicZhuyinParser
{
    public static class DictionaryExtensions
    {
        public static string[] KeysArray(this Dictionary<string, string> dict)
        {
            return dict.Keys.ToArray();
        } 
    }
}