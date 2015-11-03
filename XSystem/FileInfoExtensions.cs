using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace XSystem
{
    /// <summary>
    /// This class adds news methods to create FileInfo.
    /// </summary>
    public static class FileInfoHelpers
    {
        /// <summary>
        /// This method is used to build a fileinfo based on current time (to avoid creation by several instances).
        /// </summary>
        /// <param name="pSource">The source directory.</param>
        /// <param name="pExtension">The pExtension to use.</param>
        /// <returns>The file info with time stamp</returns>
        public static System.IO.FileInfo CreateTimeStampFileInfo(DirectoryInfo pSource, string pExtension)
        {
            DateTime lNow = DateTime.Now;
            string lFilename = pSource.FullName + Path.DirectorySeparatorChar + lNow.Year + "--" + lNow.Month + "--" + lNow.Day + "--" + lNow.Hour + "--" + lNow.Minute + "--" + lNow.Second + "--" + pExtension;
            return new System.IO.FileInfo(lFilename);
        }
    }

    /// <summary>
    /// This class stores extension methods on FileInfo.
    /// </summary>
    public static class FileInfoExtensions
    {
        #region Methods

        /// <summary>
        /// This method computes an MD5 on a file info.
        /// </summary>
        /// <param name="pInputFile">The input file.</param>
        /// <returns></returns>
        public static string MD5(this FileInfo pInputFile)
        {
            if (pInputFile.Exists)
            {
                using (var lCrypto = System.Security.Cryptography.MD5.Create())
                {
                    using (var lStream = File.OpenRead(pInputFile.FullName))
                    {
                        return BitConverter.ToString(lCrypto.ComputeHash(lStream)).Replace("-", "").ToLower();
                    }
                }
            }
            return new Random().Next().ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Checks if the file is managed.
        /// </summary>
        /// <param name="pInputFile">The input file.</param>
        /// <returns>True if the file is a managed CLI file, false otherwise.</returns>
        public static bool IsManagedAssembly(this FileInfo pInputFile)
        {
            uint[] lDataDictionaryRva = new uint[16];
            uint[] lDataDictionarySize = new uint[16];

            Stream lFileStream = new FileStream(pInputFile.FullName, FileMode.Open, FileAccess.Read);
            try
            {
                BinaryReader lReader = new BinaryReader(lFileStream);

                //PE Header starts @ 0x3C (60). Its a 4 byte header.
                lFileStream.Position = 0x3C;

                uint lPeHeader = lReader.ReadUInt32();

                // Moving to PE Header start location...
                lFileStream.Position = lPeHeader;

                // Read PE signature
                lReader.ReadUInt32();

                // Read Machine.
                lReader.ReadUInt16();

                // Read Sections.
                lReader.ReadUInt16();

                // Read Timestamp.
                lReader.ReadUInt32();

                // Read SymbolTable.
                lReader.ReadUInt32();

                // Read SymbolCount.
                lReader.ReadUInt32();

                // Read OptionalHeaderSize.
                lReader.ReadUInt16();

                // Read Characteristics.
                lReader.ReadUInt16();

                // Now we are at the end of the PE Header and from here, the
                //            PE Optional Headers starts...
                //      To go directly to the datadictionary, we'll increase the      
                //      stream’s current position to with 96 (0x60). 96 because,
                //            28 for Standard fields
                //            68 for NT-specific fields
                // From here DataDictionary starts...and its of total 128 bytes. DataDictionay has 16 directories in total, doing simple maths 128/16 = 8.
                // So each directory is of 8 bytes. In this 8 bytes, 4 bytes is of RVA and 4 bytes of Size. 
                //    By the way, the 15th directory consist of CLR header! if its 0, its not a CLR file :)

                ushort lDataDictionaryStart = Convert.ToUInt16(Convert.ToUInt16(lFileStream.Position) + 0x60);
                lFileStream.Position = lDataDictionaryStart;
                for (int lIndex0 = 0; lIndex0 < 15; lIndex0++)
                {
                    lDataDictionaryRva[lIndex0] = lReader.ReadUInt32();
                    lDataDictionarySize[lIndex0] = lReader.ReadUInt32();
                }

                lFileStream.Close();

                return (lDataDictionaryRva[14] != 0);
            }
            catch
            {
            }
            lFileStream.Close();

            return true;
        }

        /// <summary>
        /// This method computes the relatie path from a folder.
        /// </summary>
        /// <param name="pThis">The current file info.</param>
        /// <param name="pDirectory">The directory info.</param>
        /// <returns>The relative path.</returns>
        public static string GetRelativeFrom(this FileInfo pThis, DirectoryInfo pDirectory)
        {
            Uri lFileUri = new Uri(pThis.FullName);
            Uri lDirectoryUri = new Uri(pDirectory.FullName);
            return Uri.UnescapeDataString(lDirectoryUri.MakeRelativeUri(lFileUri).ToString());
        }

        /// <summary>
        /// This method is used to build a fileinfo based on current time (to avoid creation by several instances).
        /// </summary>
        /// <param name="pThis">The file info to parse</param>
        /// <param name="pIsValid">True if the parsing is valid</param>
        /// <returns>The time stamp as date time.</returns>
        public static DateTime GetTimestamp(this FileInfo pThis, out bool pIsValid)
        {
            string lFileName = pThis.Name;
            string lCleanFileName = lFileName.Replace(".monitorlog", string.Empty);
            string[] lDateAsString = lCleanFileName.Split(new[] { "--" }, StringSplitOptions.RemoveEmptyEntries);
            pIsValid = false;
            if (lDateAsString.Count() == 6)
            {
                int lYear;
                int lMonth;
                int lDay;
                int lHour;
                int lMinute;
                int lSecond;

                bool lIsValid0 = Int32.TryParse(lDateAsString[0], out lYear);
                bool lIsValid1 = Int32.TryParse(lDateAsString[1], out lMonth);
                bool lIsValid2 = Int32.TryParse(lDateAsString[2], out lDay);
                bool lIsValid3 = Int32.TryParse(lDateAsString[3], out lHour);
                bool lIsValid4 = Int32.TryParse(lDateAsString[4], out lMinute);
                bool lIsValid5 = Int32.TryParse(lDateAsString[5], out lSecond);

                if (lIsValid0 && lIsValid1 && lIsValid2 && lIsValid3 && lIsValid4 && lIsValid5)
                {
                    DateTime lDate = new DateTime(lYear, lMonth, lDay, lHour, lMinute, lSecond);
                    pIsValid = true;
                    return lDate;
                }
            }
            return DateTime.Now;
        }

        #endregion // Methods
    }
}
