using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PassivePicasso.GameImporter;
using PassivePicasso.GameImporter.SN_Fixes;
using Unity.SharpZipLib.Utils;
using UnityEngine;
using UnityEngine.Networking;
using uTinyRipper;

namespace Packages.ThunderKit.GameImporter.Editor.SNFixes
{
    public class FixShader : SNFix
    {
        public string GetTaskName()
        {
            return "Fixing Shaders";
        }

        public void Run(ProgressBarLogger logger, string assetPath)
        {
            logger.UpdateTask("InternalDeferredshadingcustomShaderFix");
            ApplyInternalDeferredshadingcustomShaderFix(assetPath);
            logger.UpdateTask("MarmosetUBERFix");
            ApplyMarmosetUBERFix(assetPath);
            logger.UpdateTask("UWEParticlesUBERFix");
            ApplyUWEParticlesUBERFix(assetPath);
            logger.UpdateTask("OverrideStandardShader");
            OverrideStandardShader(assetPath, logger);
        }

        private static void ApplyInternalDeferredshadingcustomShaderFix(string assetPath)
        {
            string path = assetPath + @"\Resources\internal-deferredshadingcustom.shader";

            List<string> lines = File.ReadAllLines(path).ToList();

            lines.RemoveAt(7);
            lines.Add("	Fallback \"Hidden/Internal-DeferredShading\""); 
            lines.Add("}");

            File.WriteAllLines(path, lines);
        }

        public static void ApplyUWEParticlesUBERFix(string assetPath)
        {
            string path = assetPath + @"\Shader\UWEParticlesUBER.shader";

            List<string> lines = File.ReadAllLines(path).ToList();

            lines[51] = "	Fallback \"Particles/Standard Unlit\"";

            File.WriteAllLines(path, lines);
        }

        private static void ApplyMarmosetUBERFix(string assetPath)
        {
            string path = assetPath + @"\Shader\MarmosetUBER.shader";

            string text = File.ReadAllText(path);
            text = text.Replace("VertexLit", "Legacy Shaders/VertexLit");
            File.WriteAllText(path, text);
        }

        private static void OverrideStandardShader(string assetPath, ProgressBarLogger logger)
        {
            //const string URL_PREFIX = "https://raw.githubusercontent.com/TwoTailsGames/Unity-Built-in-Shaders/master/DefaultResourcesExtra/";
            //string[,] shaderByUrl = new string[,]
            //{
            //    { "Standard", "Standard" },
            //    { "Normal-Bumped", "Legacy ShadersBumped Diffuse" },
            //    { "Normal-BumpSpec", "Legacy ShadersBumped Specular" },
            //    { "Particle Add", "Legacy ShadersParticlesAdditive" },
            //    { "Particle Premultiply Blend", "Legacy ShadersParticlesAlpha Blended Premultiply" },
            //    { "Particle AddMultiply", "Legacy ShadersParticles~Additive-Multiply" },
            //    { "Reflect-Diffuse", "Legacy ShadersReflectiveDiffuse" },
            //    { "Reflect-VertexLit", "Legacy ShadersReflectiveVertexLit" },
            //    { "Illumin-Diffuse", "Legacy ShadersSelf-IlluminDiffuse" },
            //    { "Illumin-VertexLit", "Legacy ShadersSelf-IlluminVertexLit" },
            //    { "", "Legacy ShadersSpecular" }, //Missing
            //    { "AlphaTest-VertexLit", "Legacy ShadersTransparentCutoutVertexLit" },
            //    { "Alpha-Diffuse", "Legacy ShadersTransparentDiffuse" },
            //    { "Alpha-VertexLit", "Legacy ShadersTransparentVertexLit" },
            //};

            string downloadPath = Path.Combine(Environment.CurrentDirectory, "DownloadedBuiltinShaders");
            string extractPath = Path.Combine(downloadPath, "builtInShader-unpacked");
            string downloadFile = Path.Combine(downloadPath, "builtInShader.zip");

            if (Directory.Exists(downloadPath))
            {
                Directory.Delete(downloadPath, true);
            }
            Directory.CreateDirectory(downloadPath);

            DownloadBuiltInShader(logger, downloadFile);

            ZipUtility.UncompressFromZip(downloadFile, null, extractPath);

            Dictionary<string, string> builtInShaderByPath = new Dictionary<string, string>();
            Dictionary<string, string> snShaderByPath = new Dictionary<string, string>();

            RegisterShader(logger, builtInShaderByPath, extractPath);
            RegisterShader(logger, snShaderByPath, assetPath);

            for (int i = 0; i < snShaderByPath.Count; i++)
            {
                logger.Log(uTinyRipper.LogType.Info, LogCategory.General, "Fixing Shader", (float)i / snShaderByPath.Count);

                KeyValuePair<string, string> keyValuePair = snShaderByPath.ElementAt(i);

                if (builtInShaderByPath.ContainsKey(keyValuePair.Key))
                {
                    byte[] byteCache = File.ReadAllBytes(builtInShaderByPath[keyValuePair.Key]);
                    File.WriteAllBytes(keyValuePair.Value, byteCache);
                }
            }

            Directory.Delete(downloadPath, true);
        }

        private static void DownloadBuiltInShader(ProgressBarLogger logger, string filePath)
        {
            UnityWebRequest www = UnityWebRequest.Get("https://download.unity3d.com/download_unity/8e603399ca02/builtin_shaders-2019.2.17f1.zip");
            www.SendWebRequest();

            while (!www.isDone)
            {
                logger.Log(uTinyRipper.LogType.Info, LogCategory.General, "Downloading Built-In-Shader", www.downloadProgress);

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    return;
                }
            }

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                return;
            }

            File.WriteAllBytes(filePath, www.downloadHandler.data);
        }

        private static string[] linesCache;
        private static void RegisterShader(ProgressBarLogger logger, Dictionary<string, string> dictionary, string path)
        {
            string[] builtInFiles = Directory.GetFiles(path, "*.shader", SearchOption.AllDirectories);
            for (int index = 0; index < builtInFiles.Length; index++)
            {
                logger.Log(uTinyRipper.LogType.Info, LogCategory.General, "Registering Shader", (float)index / builtInFiles.Length);
                linesCache = File.ReadAllLines(builtInFiles[index]);

                foreach (string line in linesCache)
                {
                    if (line.TrimStart().StartsWith("Shader \""))
                    {
                        dictionary.Add(line.Split('\"')[1], builtInFiles[index]);
                    }
                }
            }
        }
    }
}
