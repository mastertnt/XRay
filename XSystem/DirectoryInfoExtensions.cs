using System;
using System.IO;

namespace XSystem
{
    /// <summary>
    /// This class stores extension methods on DirectoryInfo.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// This method is used to build a fileinfo based on current time (to avoid creation by several instances).
        /// </summary>
        /// <param name="pSource">The source directory.</param>
        /// <param name="pExtension">The pExtension to use.</param>
        /// <returns>The file info with time stamp</returns>
        public static System.IO.FileInfo CreateTimestampedFile(this DirectoryInfo pSource, string pExtension)
        {
            DateTime lNow = DateTime.Now;
            string lFilename = pSource.FullName + Path.DirectorySeparatorChar + lNow.Year + "--" + lNow.Month + "--" + lNow.Day + "--" + lNow.Hour + "--" + lNow.Minute + "--" + lNow.Second + "--" + pExtension;
            return new System.IO.FileInfo(lFilename);
        }

        /// <summary>
        /// This method is used to build a fileinfo based on current time (to avoid creation by several instances).
        /// </summary>
        /// <param name="pSource">The source directory.</param>
        /// <param name="pDirectoryName">The pExtension to use.</param>
        /// <returns>The file info with time stamp</returns>
        public static System.IO.DirectoryInfo CreateTimestampedDirectory(this DirectoryInfo pSource, string pDirectoryName)
        {
            DateTime lNow = DateTime.Now;
            string lPath = pSource.FullName + Path.DirectorySeparatorChar + lNow.Year + "--" + lNow.Month + "--" + lNow.Day + "--" + lNow.Hour + "--" + lNow.Minute + "--" + lNow.Second + "--" + pDirectoryName;
            return new System.IO.DirectoryInfo(lPath);
        }
    }
}
