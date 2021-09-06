﻿using System.Collections.Generic;
using System;
using MiniJSON;
using UnityEngine;
using System.Collections;

namespace UDL.Core
{
    public static class Parser
    {
        public static string GetString(Dictionary<string, object> dict, string key, string def = "")
        {
            if (dict != null)
            {
                object obj;
                dict.TryGetValue(key, out obj);
                if (obj != null) return obj.ToString();
            }
            return def;
        }

        public static int GetInt(Dictionary<string, object> dict, string key, int def = 0)
        {
            int result;
            if (int.TryParse(GetString(dict, key), out result)) return result;
            return def;
        }

        public static int GetShort(Dictionary<string, object> dict, string key, short def = 0)
        {
            short result;
            if (short.TryParse(GetString(dict, key), out result)) return result;
            return def;
        }

        public static long GetLong(Dictionary<string, object> dict, string key, long def = 0)
        {
            long result;
            if (long.TryParse(GetString(dict, key), out result)) return result;
            return def;
        }

        public static bool GetBool(Dictionary<string, object> dict, string key, bool def = false)
        {
            bool result;
            if (bool.TryParse(GetString(dict, key), out result)) return result;
            return def;
        }

        public static float GetFloat(Dictionary<string, object> dict, string key, float def = 0)
        {
            float result;
            if (float.TryParse(GetString(dict, key), out result)) return result;
            return def;
        }

        public static double GetDouble(Dictionary<string, object> dict, string key, double def = 0)
        {
            double result;
            if (double.TryParse(GetString(dict, key), out result)) return result;
            return def;
        }

        public static T GetEnum<T>(Dictionary<string, object> dict, string key)
        {
            return ParseEnum<T>(GetString(dict, key));
        }

        public static T ParseEnum<T>(string str)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), str);
            }
            catch
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        public static List<object> ConvertListValue<T>(List<T> list)
        {
            var outList = new List<object>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    outList.Add(item);
                }
            }
            return outList;
        }

        public static IList GetListEnum(Dictionary<string, object> dict, string key, Type type)
        {
            var listType = typeof(List<>).MakeGenericType(new[] { type });
            var outList = (IList)Activator.CreateInstance(listType);

            var list = GetListObj(dict, key);

            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        outList.Add(GetEnumByType(item.ToString(), type));
                    }
                }
            }

            return outList;
        }

        public static List<string> GetListString(Dictionary<string, object> dict, string key)
        {
            var outList = new List<string>();
            var list = GetListObj(dict, key);

            if (list != null)
            {
                foreach (var item in list)
                {
                    if(item != null){
                        outList.Add(item.ToString());
                    }
                }
            }

            return outList;
        }

        public static List<int> GetListInt(Dictionary<string, object> dict, string key)
        {
            var outList = new List<int>();
            var list = GetListObj(dict, key);

            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        var value = 0;

                        try
                        {
                            value = int.Parse(item.ToString());
                        }
                        catch
                        {
                            LogError("Parse int: " + key);
                        }

                        outList.Add(value);
                    }
                }
            }

            return outList;
        }

        public static List<long> GetListLong(Dictionary<string, object> dict, string key)
        {
            var outList = new List<long>();
            var list = GetListObj(dict, key);

            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        var value = 0L;

                        try
                        {
                            value = long.Parse(item.ToString());
                        }
                        catch
                        {
                            LogError("Parse long: " + key);
                        }

                        outList.Add(value);
                    }

                }
            }

            return outList;
        }

        public static List<float> GetListFloat(Dictionary<string, object> dict, string key)
        {
            var outList = new List<float>();
            var list = GetListObj(dict, key);

            if (list != null)
            {
                foreach (var item in list)
                {
                    if(item != null){
                        var value = 0f;

                        try
                        {
                            value = float.Parse(item.ToString());
                        }
                        catch
                        {
                            LogError("Parse float: " + key);
                        }

                        outList.Add(value);
                    }
                }
            }

            return outList;
        }

        public static List<bool> GetListBool(Dictionary<string, object> dict, string key)
        {
            var outList = new List<bool>();
            var list = GetListObj(dict, key);

            if (list != null)
            {
                foreach (var item in list)
                {
                    if(item != null){
                        var value = false;

                        try
                        {
                            value = bool.Parse(item.ToString());
                        }
                        catch
                        {
                            LogError("Parse bool: " + key);
                        }

                        outList.Add(value);
                    }
                }
            }

            return outList;
        }

        public static List<double> GetListDouble(Dictionary<string, object> dict, string key)
        {
            var outList = new List<double>();
            var list = GetListObj(dict, key);

            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        double value = 0;

                        try
                        {
                            value = double.Parse(item.ToString());
                        }
                        catch
                        {
                            LogError("Parse double: " + key);
                        }

                        outList.Add(value);
                    }

                }
            }

            return outList;
        }

        private static List<object> GetListObj(Dictionary<string, object> dict, string key)
        {
            if (dict != null)
            {
                object obj;
                dict.TryGetValue(key, out obj);
                if (obj != null) return obj as List<object>;
            }
            return null;
        }


        public static Dictionary<string, object> GetDict(Dictionary<string, object> dict, string key)
        {
            if (dict != null)
            {
                object item;
                dict.TryGetValue(key, out item);

                if (item != null)
                {
                    var data = item as Dictionary<string, object>;
                    if (data != null) return data;
                }
            }
            return new Dictionary<string, object>();
        }

        public static List<object> GetList(Dictionary<string, object> dict, string key)
        {
            if (dict != null)
            {
                object item;
                dict.TryGetValue(key, out item);

                if (item != null)
                {
                    var list = item as List<object>;
                    if (list != null) return list;
                }
            }
            return new List<object>();
        }       

        public static Dictionary<string, object> GetDict(string json)
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            if (dict != null) return dict;
            return new Dictionary<string, object>();
        }

        public static object GetEnumByType(Dictionary<string, object> dict, string key, Type type)
        {
            try
            {
                return Enum.Parse(type, Parser.GetString(dict, key));
            }
            catch
            {
                return Activator.CreateInstance(type);
            }
        }

        public static object GetEnumByType(string value, Type type)
        {
            try
            {
                return Enum.Parse(type, value);
            }
            catch
            {
                return Activator.CreateInstance(type);
            }
        }

        private static void LogError(object msg)
        {
        }
    }
}
