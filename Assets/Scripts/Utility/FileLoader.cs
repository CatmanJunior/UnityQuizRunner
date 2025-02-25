using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Provides methods to load files from a directory, with support for different platforms such as WebGL and Android.
/// </summary>
public static class FileLoader
{
    private const string ManifestFileName = "FileManifest.txt";

    /// <summary>
    /// Loads files from the specified directory matching the given pattern.
    /// Handles both standard platforms, WebGL, and Android by reading files appropriately.
    /// </summary>
    /// <param name="directory">The directory from which to load files.</param>
    /// <param name="pattern">The search pattern to match files.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of strings
    /// read from the matching files.
    /// </returns>
    public static async Task<List<string>> LoadFiles(string directory, string pattern)
    {
        var lines = new List<string>();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // WEBGL CASE: Must load files using UnityWebRequest from StreamingAssets
            var manifestPath = Path.Combine(Application.streamingAssetsPath, ManifestFileName);
            var manifestLines = await ReadViaUnityWebRequest(manifestPath);

            foreach (var file in manifestLines)
            {
                var filePath = Path.Combine(directory, file);
                var fileLines = await ReadViaUnityWebRequest(filePath);
                lines.AddRange(fileLines);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            // ANDROID CASE: Also needs UnityWebRequest if files are in StreamingAssets.
            // If your 'directory' is inside StreamingAssets, you must use UnityWebRequest.
            // Here, we assume you're doing something similar to WebGL:
            var manifestPath = Path.Combine(Application.streamingAssetsPath, ManifestFileName);
            var manifestLines = await ReadViaUnityWebRequest(manifestPath);

            foreach (var file in manifestLines)
            {
                // Combine with StreamingAssets path, or `directory` if it's also StreamingAssets
                var filePath = Path.Combine(directory, file);
                // On Android, path in StreamingAssets is effectively a URL, so we do the same approach:
                var fileLines = await ReadViaUnityWebRequest(filePath);
                lines.AddRange(fileLines);
            }
        }
        else
        {
            // DEFAULT CASE (PC, Mac, Linux, etc.):
            // Can read files normally from the file system
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

    /// <summary>
    /// Reads the content of a file using UnityWebRequest.
    /// Useful on platforms where StreamingAssets is compressed inside the build (e.g. Android, WebGL).
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a list of strings
    /// read from the file.
    /// </returns>
    private static async Task<List<string>> ReadViaUnityWebRequest(string path)
    {
        var lines = new List<string>();

        using (UnityWebRequest request = UnityWebRequest.Get(path))
        {
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to read file via UnityWebRequest at: {path}\nError: {request.error}");
                return lines;
            }

            var fileContent = request.downloadHandler.text;
            using (var reader = new StringReader(fileContent))
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
