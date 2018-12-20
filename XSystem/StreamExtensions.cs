using System;
using System.IO;

namespace XSystem
{
    /// <summary>
    ///     This class stores extension methods on stream.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        ///     Compares the content.
        /// </summary>
        /// <param name="pFirstStream">The first stream.</param>
        /// <param name="pSecondStream">The second stream.</param>
        /// <returns></returns>
        public static bool CompareContent(this Stream pFirstStream, Stream pSecondStream)
        {
            if (pFirstStream.Length != pSecondStream.Length)
            {
                return false;
            }

            var lFirstBuffer = new byte[pFirstStream.Length];
            var lSecondBuffer = new byte[pFirstStream.Length];
            pFirstStream.Read(lFirstBuffer, 0, (int) pFirstStream.Length);
            pSecondStream.Read(lSecondBuffer, 0, (int) pFirstStream.Length);
            if (lFirstBuffer != lSecondBuffer)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        ///     This method computes a SHA1 on a file info.
        /// </summary>
        /// <param name="pStream">The input file.</param>
        /// <returns></returns>
        public static string SHA1(this Stream pStream)
        {
            using (var lCrypto = System.Security.Cryptography.SHA1.Create())
            {
                return BitConverter.ToString(lCrypto.ComputeHash(pStream)).Replace("-", "").ToLower();
            }
        }
    }
}