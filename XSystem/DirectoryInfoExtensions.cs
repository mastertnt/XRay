using System;
using System.IO;
using System.Linq;

namespace XSystem
{
    /// <summary>
    ///     This class stores extension methods on DirectoryInfo.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        ///     This method is used to build a fileinfo based on current time (to avoid creation by several instances).
        /// </summary>
        /// <param name="pSource">The source directory.</param>
        /// <param name="pFileName">The file name to use.</param>
        /// <returns>The file info with time stamp</returns>
        public static FileInfo CreateTimestampedFile(this DirectoryInfo pSource, string pFileName)
        {
            return new FileInfo(pSource.CreateTimestampedFileFullName(pFileName));
        }

        /// <summary>
        ///     This method is used to build a file fullname based on current time (to avoid creation by several instances).
        /// </summary>
        /// <param name="pSource">The source directory.</param>
        /// <param name="pFileName">The file name to use.</param>
        /// <returns>The file full name with time stamp</returns>
        private static string CreateTimestampedFileFullName(this DirectoryInfo pSource, string pFileName)
        {
            var lNow = DateTime.Now;
            var lFilename = pSource.FullName + Path.DirectorySeparatorChar + lNow.Year + "--" + lNow.Month + "--" + lNow.Day + "--" + lNow.Hour + "--" + lNow.Minute + "--" + lNow.Second + "--" + pFileName;

            return lFilename;
        }

        /// <summary>
        ///     This method builds a fileinfo based on current time (to avoid creation by several instances) and makes sure the
        ///     file name is unique upon creation.
        /// </summary>
        /// <param name="pSource">The source directory.</param>
        /// <param name="pFileName">The file name to use.</param>
        /// <returns>The file info with time stamp</returns>
        public static FileInfo CreateUniqueTimestampedFile(this DirectoryInfo pSource, string pFileName)
        {
            var lPath = pSource.CreateTimestampedFileFullName(pFileName);

            var lCounter = 0;
            while (File.Exists(lPath))
            {
                lPath = pSource.CreateTimestampedFileFullName(string.Format("{0}--{1}", lCounter++, pFileName));
            }

            return new FileInfo(lPath);
        }

        /// <summary>
        ///     This method is used to build a fileinfo based on current time (to avoid creation by several instances).
        /// </summary>
        /// <param name="pSource">The source directory.</param>
        /// <param name="pDirectoryName">The pExtension to use.</param>
        /// <returns>The file info with time stamp</returns>
        public static DirectoryInfo CreateTimestampedDirectory(this DirectoryInfo pSource, string pDirectoryName)
        {
            var lNow = DateTime.Now;
            var lPath = pSource.FullName + Path.DirectorySeparatorChar + lNow.Year + "--" + lNow.Month + "--" + lNow.Day + "--" + lNow.Hour + "--" + lNow.Minute + "--" + lNow.Second + "--" + pDirectoryName;
            return new DirectoryInfo(lPath);
        }

        /// <summary>
        ///     Deletes the old files matching pSearchPattern (same format as DirectoryInfo.EnumerateFiles), while keeping the most
        ///     recents (count of file to keep is pNumberOfFilesToKeep)
        /// </summary>
        /// <param name="pSource">The source.</param>
        /// <param name="pSearchPattern">The search pattern.</param>
        /// <param name="pNumberOfFilesToKeep">The number of files to keep.</param>
        /// <returns>The deleted files count.</returns>
        public static int DeleteOldFiles(this DirectoryInfo pSource, string pSearchPattern, int pNumberOfFilesToKeep)
        {
            if (pSource == null)
            {
                return 0;
            }

            var lDeletedFilesCount = 0;

            // First, retrieve all files (keep only the most recent).
            if (pSource.Exists)
            {
                //Dictionary<string, DateTime> lFileByDate = new Dictionary<string, DateTime>();
                var lFiles = pSource.GetFiles(pSearchPattern).OrderBy(pElt => pElt.CreationTime).ToList();

                //use  MAX_FILE_COUNT * 2 because there are 2 log file, one for output, the other for error
                if (lFiles.Count > pNumberOfFilesToKeep)
                {
                    var lToRemoveCount = lFiles.Count - pNumberOfFilesToKeep;

                    for (var lFileIdx = 0; lFileIdx < lToRemoveCount; lFileIdx++)
                    {
                        try
                        {
                            lFiles[lFileIdx].Delete();
                            lDeletedFilesCount++;
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                            // Nothing to do.
                        }
                    }
                }
            }

            return lDeletedFilesCount;
        }
    }
}