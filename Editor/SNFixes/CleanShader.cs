using System;
using System.IO;
using System.Linq;
using PassivePicasso.GameImporter;
using PassivePicasso.GameImporter.SN_Fixes;
using uTinyRipper;

namespace Packages.ThunderKit.GameImporter.Editor.SNFixes
{
    public class CleanShader : SNFix
    {
        public string GetTaskName()
        {
            return "Cleaning Shaders";
        }

        public void Run(ProgressBarLogger logger, string assetPath)
        {
            string[] files = Directory.GetFiles(assetPath, "*.shader", SearchOption.AllDirectories);
            for (int index = 0; index < files.Length; index++)
            {
                string file = files[index];
                logger.Log(LogType.Info, LogCategory.General, $"Cleaning {file.Split('\\').Last()}", (float)index / files.Length);

                string text = File.ReadAllText(file);
                if (text.Contains("SubShader") && text.Contains("Fallback \""))
                {
                    string[] dividedShader = text.Split(new[] { "SubShader {" }, StringSplitOptions.RemoveEmptyEntries);
                    string fallBack = dividedShader.Last().Split(new[] { "Fallback" }, StringSplitOptions.RemoveEmptyEntries).Last();
                    string newShader = dividedShader[0] + "Fallback" + fallBack;

                    File.WriteAllText(file, newShader);
                }
                else if (text.Contains("SubShader"))
                {
                    string[] dividedShader = text.Split(new[] { "SubShader {" }, StringSplitOptions.RemoveEmptyEntries);
                    string newShader = dividedShader[0] + "\n}";

                    File.WriteAllText(file, newShader);
                }
            }
        }
    }
}
