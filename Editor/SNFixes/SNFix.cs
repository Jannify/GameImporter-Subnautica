namespace PassivePicasso.GameImporter.SN_Fixes
{
    public interface SNFix
    {
        string GetTaskName();
        void Run(ProgressBarLogger logger, string assetsPath);
    }
}
