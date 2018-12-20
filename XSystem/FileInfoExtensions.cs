using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace XSystem
{
    /// <summary>
    ///     This class stores extension methods on FileInfo.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        ///     This method computes a SHA1 on a file info.
        /// </summary>
        /// <param name="pInputFile">The input file.</param>
        /// <returns></returns>
        public static string SHA1(this FileInfo pInputFile)
        {
            if (pInputFile.Exists)
            {
                using (var lCrypto = System.Security.Cryptography.SHA1.Create())
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
        ///     Checks if the file is managed.
        ///     taken from :
        ///     https://stackoverflow.com/questions/367761/how-to-determine-whether-a-dll-is-a-managed-assembly-or-native-prevent-loading
        ///     Bug. 6123 : x64 and x32 bits
        /// </summary>
        /// <param name="pInputFile">The input file.</param>
        /// <returns>True if the file is a managed CLI file, false otherwise.</returns>
        public static bool IsManagedAssembly(this FileInfo pInputFile)
        {
            using (Stream lFileStream = new FileStream(pInputFile.FullName, FileMode.Open, FileAccess.Read))
            using (var lBinaryReader = new BinaryReader(lFileStream))
            {
                if (lFileStream.Length < 64)
                {
                    return false;
                }

                //PE Header starts @ 0x3C (60). Its a 4 byte header.
                lFileStream.Position = 0x3C;
                var lPEHeaderPointer = lBinaryReader.ReadUInt32();
                if (lPEHeaderPointer == 0)
                {
                    lPEHeaderPointer = 0x80;
                }

                // Ensure there is at least enough room for the following structures:
                //     24 byte PE Signature & Header
                //     28 byte Standard Fields         (24 bytes for PE32+)
                //     68 byte NT Fields               (88 bytes for PE32+)
                // >= 128 byte Data Dictionary Table
                if (lPEHeaderPointer > lFileStream.Length - 256)
                {
                    return false;
                }

                // Check the PE signature.  Should equal 'PE\0\0'.
                lFileStream.Position = lPEHeaderPointer;
                var lPEHeaderSignature = lBinaryReader.ReadUInt32();
                if (lPEHeaderSignature != 0x00004550)
                {
                    return false;
                }

                // skip over the PEHeader fields
                lFileStream.Position += 20;

                // Read PE magic number from Standard Fields to determine format.
                var lPEFormat = lBinaryReader.ReadUInt16();
                if (lPEFormat != PE32 && lPEFormat != PE32_PLUS)
                {
                    return false;
                }

                // Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
                // When this is non-zero then the file contains CLI data otherwise not.
                var lDataDictionaryStart = (ushort) (lPEHeaderPointer + (lPEFormat == PE32 ? 232 : 248));
                lFileStream.Position = lDataDictionaryStart;

                var lCLIHeaderRva = lBinaryReader.ReadUInt32();
                if (lCLIHeaderRva == 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        ///     This method computes the relatie path from a folder.
        /// </summary>
        /// <param name="pThis">The current file info.</param>
        /// <param name="pDirectory">The directory info.</param>
        /// <returns>The relative path.</returns>
        public static string GetRelativeFrom(this FileInfo pThis, DirectoryInfo pDirectory)
        {
            var lCleanedPath = pThis.FullName;
            if (Path.IsPathRooted(pThis.ToString()) == false)
            {
                lCleanedPath = pDirectory.FullName + pThis;
            }

            var lFileUri = new Uri(lCleanedPath);
            var lDirectoryUri = new Uri(pDirectory.FullName);
            return Uri.UnescapeDataString(lDirectoryUri.MakeRelativeUri(lFileUri).ToString());
        }

        /// <summary>
        ///     This method is used to build a fileinfo based on current time (to avoid creation by several instances).
        /// </summary>
        /// <param name="pThis">The file info to parse</param>
        /// <returns>The new timestamped filename.</returns>
        public static FileInfo Timestamp(this FileInfo pThis)
        {
            var lPath = pThis.Directory;
            var lDate = DateTime.Now;
            var lFileName = lDate.Year + "--" + lDate.Month + "--" + lDate.Day + "--" + lDate.Hour + "--" + lDate.Minute + "--" + lDate.Second + "--" + lDate.Millisecond + "--" + pThis.Name;

            return new FileInfo(lPath.FullName + Path.DirectorySeparatorChar + lFileName);
        }

        /// <summary>
        ///     This method is used to retrieve the date time of a file info.
        /// </summary>
        /// <param name="pThis">The file info to parse</param>
        /// <param name="pIsValid">True if the parsing is valid</param>
        /// <returns>The time stamp as date time.</returns>
        public static DateTime GetTimestamp(this FileInfo pThis, out bool pIsValid)
        {
            var lFileName = pThis.Name;
            var lCleanFileName = lFileName.Replace(".monitorlog", string.Empty);
            var lDateAsString = lCleanFileName.Split(new[]
            {
                "--"
            }, StringSplitOptions.RemoveEmptyEntries);
            pIsValid = false;
            if (lDateAsString.Count() == 6)
            {
                int lYear;
                int lMonth;
                int lDay;
                int lHour;
                int lMinute;
                int lSecond;

                var lIsValid0 = int.TryParse(lDateAsString[0], out lYear);
                var lIsValid1 = int.TryParse(lDateAsString[1], out lMonth);
                var lIsValid2 = int.TryParse(lDateAsString[2], out lDay);
                var lIsValid3 = int.TryParse(lDateAsString[3], out lHour);
                var lIsValid4 = int.TryParse(lDateAsString[4], out lMinute);
                var lIsValid5 = int.TryParse(lDateAsString[5], out lSecond);

                if (lIsValid0 && lIsValid1 && lIsValid2 && lIsValid3 && lIsValid4 && lIsValid5)
                {
                    var lDate = new DateTime(lYear, lMonth, lDay, lHour, lMinute, lSecond);
                    pIsValid = true;
                    return lDate;
                }
            }

            return DateTime.Now;
        }

        #region Fields

        /// <summary>
        ///     PE32 hexadecimal constant.
        /// </summary>
        private const ushort PE32 = 0x10b;

        /// <summary>
        ///     PE32Plus hexadecimal constant.
        /// </summary>
        private const ushort PE32_PLUS = 0x20b;

        #endregion // Fields.
    }
}