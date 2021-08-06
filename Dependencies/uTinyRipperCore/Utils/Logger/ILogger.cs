﻿namespace uTinyRipper
{
	public interface ILogger
	{
		void Log(LogType type, LogCategory category, string message);

        void UpdateProgress(float progress);
    }
}
