using Packages.ThunderKit.GameImporter.Editor.SNFixes;
using PassivePicasso.GameImporter;
using PassivePicasso.GameImporter.SN_Fixes;
using UnityEditor;
using UnityEngine;

namespace Packages.ThunderKit.GameImporter.Editor
{
    public class SNFixesUtility
    {
        public static readonly string AssetPath = Application.dataPath + "/../Packages/Subnautica/Assets";
        public static ProgressBarLogger Logger;

        private static readonly SNFix[] fixes = { new CleanShader(), new FixShader(), new UnityUIReference() };

        [MenuItem("Tools/SubnauticaImporter/Run all SN-Asset Fixes", false, 10)]
        private static void RunFixes()
        {
            Logger = new ProgressBarLogger();
            foreach (SNFix fix in fixes)
            {
                Logger.UpdateTask(fix.GetTaskName());
                fix.Run();
            }
            Logger.Dispose();

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/SubnauticaImporter/Selected Fixes/CleanShader")]
        private static void CleanShaderFix()
        {
            Logger = new ProgressBarLogger();
            Logger.UpdateTask("Cleaning Shaders");
            CleanShader.CleanShaderFix();
            Logger.Dispose();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/SubnauticaImporter/Selected Fixes/InternalDeferredshadingcustomShaderFix")]
        private static void ApplyInternalDeferredshadingcustomShaderFix()
        {
            Logger = new ProgressBarLogger();
            Logger.UpdateTask("Fixing Shaders");
            FixShader.ApplyInternalDeferredshadingcustomShaderFix();
            Logger.Dispose();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/SubnauticaImporter/Selected Fixes/UWEParticlesUBERFix")]
        private static void ApplyUWEParticlesUBERFix()
        {
            Logger = new ProgressBarLogger();
            Logger.UpdateTask("Fixing Shaders");
            FixShader.ApplyUWEParticlesUBERFix();
            Logger.Dispose();
            AssetDatabase.Refresh();
        }
        [MenuItem("Tools/SubnauticaImporter/Selected Fixes/MarmosetUBERFix")]
        private static void ApplyMarmosetUBERFix()
        {
            Logger = new ProgressBarLogger();
            Logger.UpdateTask("Fixing Shaders");
            FixShader.ApplyMarmosetUBERFix();
            Logger.Dispose();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/SubnauticaImporter/Selected Fixes/OverrideStandardShader")]
        private static void OverrideStandardShader()
        {
            Logger = new ProgressBarLogger();
            Logger.UpdateTask("Fixing Shaders");
            FixShader.OverrideStandardShader();
            Logger.Dispose();
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/SubnauticaImporter/Selected Fixes/UnityUIReference")]
        private static void UnityUIReferenceFix()
        {
            Logger = new ProgressBarLogger();
            Logger.UpdateTask("Reassigning missing UnityEngine.UI references");
            UnityUIReference.UnityUIReferenceFix();
            Logger.Dispose();
            AssetDatabase.Refresh();
        }
    }
}
