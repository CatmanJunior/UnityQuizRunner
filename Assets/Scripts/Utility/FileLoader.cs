using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class FileLoader
{
    private const string ManifestFileName = "FileManifest.txt";

    public static async Task<List<string>> LoadFiles(string directory, string pattern)
    {
        var lines = new List<string>();
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            var manifestPath = Path.Combine(Application.streamingAssetsPath, ManifestFileName);
            var manifestLines = await ReadWebGLFile(manifestPath);

            foreach (var file in manifestLines)
            {
                var filePath = Path.Combine(directory, file);
                var fileLines = await ReadWebGLFile(filePath);
                lines.AddRange(fileLines);
            }
        }
        else
        {
            var files = Directory.GetFiles(directory, pattern);
            if (files.Length == 0)
            {
                Debug.LogError("No files found in the directory: " + directory);
                return null;
            }
            foreach (var file in files)
            {
                lines.AddRange(File.ReadAllLines(file));
            }
        }
        return lines;
    }

    private static async Task<List<string>> ReadWebGLFile(string path)
    {
        var lines = new List<string>();
        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                return lines;
            }

            var fileContent = request.downloadHandler.text;
            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
        }
        return lines;
    }
}