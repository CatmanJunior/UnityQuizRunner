using System;
using System.IO;

public static class Logger
{
    private static readonly string logFilePath = "log.txt";

    public static void Log(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to log message: {ex.Message}");
        }
    }
}