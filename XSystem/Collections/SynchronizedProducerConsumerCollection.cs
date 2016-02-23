using System.Collections;
using System.Threading;

namespace XSystem.Collections
{
    /// <summary>
    /// This collection can be used in two threads. 
    /// One thread produces data (in Next property), this other one can consume the data without any problem (in Current Property).
    /// </summary>
    public class SynchronizedProducerConsumerCollection<T> where T : ICollection
    {
        #region Fields

        /// <summary>
        /// This field store sthe current index of the buffer.
        /// </summary>
        private int mCurrentIndex;

        /// <summary>
        /// This field stores the double buffer for the collection.
        /// </summary>
        private readonly T[] mBuffers;

        #endregion // Fields.

        #region Properties

        /// <summary>
        ///     Next buffer waiting in line (no active usage should be here)
        /// </summary>
        public T Next
        {
            get
            {
                return this.mBuffers[(this.mCurrentIndex + 1) & 1];
            }
        }

        /// <summary>
        ///     Currently active buffer (active usage should be here)
        /// </summary>
        public T Current
        {
            get
            {
                return this.mBuffers[this.mCurrentIndex & 1];
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedProducerConsumerCollection{T}"/> class.
        /// </summary>
        public SynchronizedProducerConsumerCollection()
        {
            this.mBuffers = new T[2];
            this.mCurrentIndex = 0;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        ///     Swaps between the buffers
        /// </summary>
        /// <returns>Returns the buffer previously used before the swap</returns>
        public void Swap()
        {
            Interlocked.Increment(ref mCurrentIndex);
        }

        #endregion // Methods.
    }
}
