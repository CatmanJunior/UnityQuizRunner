#if UNITY_STANDALONE
using System.Diagnostics;
#endif
public static class Restarter
{
    public static void RestartApplication()
    {
#if UNITY_STANDALONE
        Process.Start(Application.dataPath.Replace("_Data", ".exe")); // Windows standalone
        Application.Quit();
#endif
    }
}
