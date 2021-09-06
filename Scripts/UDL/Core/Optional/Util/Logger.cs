using System.Diagnostics;

public static class Logger
{
    [Conditional("DEBUG_LOGGER")]
    public static void Log(string log)
    {
        UnityEngine.Debug.Log("[Logger] : " + log);
    }
}