using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsUtil
{
    public static bool GetBool(string key, bool defaultValue = false)
    {
        var value = defaultValue;
        if (HasKey(key))
        {
            value = PlayerPrefs.GetInt(key) == 1;
        }
        else
        {
            SetBool(key, defaultValue);
        }
        return value;
    }
    public static int GetInt(string key, int defaultValue = 0)
    {
        var value = defaultValue;
        if (HasKey(key))
        {
            value = PlayerPrefs.GetInt(key);
        }
        else
        {
            SetInt(key, defaultValue);
        }
        return value;
    }
    public static float GetFloat(string key, float defaultValue = 0.0f)
    {
        var value = defaultValue;
        if (HasKey(key))
        {
            value = PlayerPrefs.GetFloat(key);
        }
        else
        {
            SetFloat(key, defaultValue);
        }
        return value;
    }

    public static string GetString(string key, string defaultValue = "")
    {
        var value = defaultValue;
        if (HasKey(key))
        {
            value = PlayerPrefs.GetString(key);
        }
        else
        {
            SetString(key, defaultValue);
        }
        return value;
    }

    public static void SetBool(string key, bool val)
    {
        PlayerPrefs.SetInt(key, val ? 1 : 0);
    }
    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
    }
    public static void SetFloat(string key, float val)
    {
        PlayerPrefs.SetFloat(key, val);
    }

    public static void SetString(string key, string val)
    {
        PlayerPrefs.SetString(key, val);
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
