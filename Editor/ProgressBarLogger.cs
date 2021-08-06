using System;
using UnityEditor;
using uTinyRipper;

namespace PassivePicasso.GameImporter
{
    public class ProgressBarLogger : ILogger, IDisposable
    {
        private string task = "Analyzing Game";
        private float progress;

        public ProgressBarLogger()
        {
            EditorUtility.DisplayProgressBar(task, "", 0);
        }

        public void Dispose()
        {
            EditorUtility.ClearProgressBar();
        }

        public void Log(LogType type, LogCategory category, string message)
        {
            switch (type)
            {
                case LogType.Error:
                    throw new Exception(message);
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                default:
                    EditorUtility.DisplayProgressBar(task, message, progress);
                    break;
            }
        }

        public void UpdateProgress(float progress)
        {
            this.progress = progress;
        }

        public void UpdateTask(string task)
        {
            this.task = task;
        }
    }
}
