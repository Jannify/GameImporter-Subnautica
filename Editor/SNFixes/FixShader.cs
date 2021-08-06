using System.Collections.Generic;
using System.IO;
using System.Linq;
using PassivePicasso.GameImporter;
using PassivePicasso.GameImporter.SN_Fixes;

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

        private static void ApplyMarmosetUBERFix(string assetPath)
        {
            string path = assetPath + @"\Shader\MarmosetUBER.shader";

            string text = File.ReadAllText(path);
            text = text.Replace("VertexLit", "Legacy Shaders/VertexLit");
            File.WriteAllText(path, text);
        }
    }
}
