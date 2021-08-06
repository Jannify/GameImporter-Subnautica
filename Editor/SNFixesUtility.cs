using Packages.ThunderKit.GameImporter.Editor.SNFixes;
using PassivePicasso.GameImporter.SN_Fixes;
using UnityEditor;
using UnityEngine;

namespace Packages.ThunderKit.GameImporter.Editor
{
    public class SNFixesUtility
    {
        private static readonly SNFix[] fixes = { new CleanShader(), new FixShader(), new UnityUIReference() };

        [MenuItem("Tools/SubnauticaImporter/Fix Imported Asset", false, 10)]
        static void RunFixes()
        {
            using (PassivePicasso.GameImporter.ProgressBarLogger logger = new PassivePicasso.GameImporter.ProgressBarLogger())
            {
                foreach (SNFix fix in fixes)
                {
                    logger.UpdateTask(fix.GetTaskName());
                    fix.Run(logger, Application.dataPath);
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
