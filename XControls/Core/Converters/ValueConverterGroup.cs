using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace XControls.Core.Converters
{
    /// <summary>
    ///     A value lConverter which contains a list of IValueConverters and invokes their Convert or ConvertBack methods
    ///     in the order that they exist in the list.  The output of one lConverter is piped into the next lConverter
    ///     allowing for modular value mConverters to be chained together.  If the ConvertBack method is invoked, the
    ///     value mConverters are executed in reverse order (highest to lowest index).  Do not leave an element in the
    ///     Converters property collection null, every element must reference a valid IValueConverter instance. If a
    ///     value lConverter's type is not decorated with the ValueConversionAttribute, an InvalidOperationException will be
    ///     thrown when the lConverter is added to the Converters collection.
    /// </summary>
    [ContentProperty("Converters")]
    public class ValueConverterGroup : IValueConverter
    {
        #region Constructor

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public ValueConverterGroup()
        {
            this.mConverters.CollectionChanged += this.OnConvertersCollectionChanged;
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        ///     Gets the list of IValueConverters contained in this lConverter.
        /// </summary>
        public ObservableCollection<IValueConverter> Converters => this.mConverters;

        #endregion // Properties.

        #region Fields

        /// <summary>
        ///     This fields stores the lConverter list.
        /// </summary>
        private readonly ObservableCollection<IValueConverter> mConverters = new ObservableCollection<IValueConverter>();

        /// <summary>
        ///     This fields stores the attributes.
        /// </summary>
        private readonly Dictionary<IValueConverter, ValueConversionAttribute> mCachedAttributes = new Dictionary<IValueConverter, ValueConversionAttribute>();

        #endregion // Data

        #region Methods

        /// <summary>
        ///     Convert from Source to Destination.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        object IValueConverter.Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            var lOutput = pValue;

            for (var i = 0; i < this.Converters.Count; ++i)
            {
                var lConverter = this.Converters[i];
                var lCurrentTargetType = this.GetTargetType(i, pTargetType, true);
                lOutput = lConverter.Convert(lOutput, lCurrentTargetType, pExtraParameter, pCulture);

                // If the lConverter returns 'DoNothing' then the binding operation should terminate.
                if (lOutput == Binding.DoNothing)
                {
                    break;
                }
            }

            return lOutput;
        }

        /// <summary>
        ///     Convert from Destination to Source.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        object IValueConverter.ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            var lOutput = pValue;

            for (var i = this.Converters.Count - 1; i > -1; --i)
            {
                var lConverter = this.Converters[i];
                var lCurrentTargetType = this.GetTargetType(i, pTargetType, true);
                lOutput = lConverter.ConvertBack(lOutput, lCurrentTargetType, pExtraParameter, pCulture);

                // When a lConverter returns 'DoNothing' the binding operation should terminate.
                if (lOutput == Binding.DoNothing)
                {
                    break;
                }
            }

            return lOutput;
        }

        /// <summary>
        ///     Returns the target type for a conversion operation.
        /// </summary>
        /// <param name="converterIndex">The index of the current lConverter about to be executed.</param>
        /// <param name="finalTargetType">The 'targetType' argument passed into the conversion method.</param>
        /// <param name="convert">Pass true if calling from the Convert method, or false if calling from ConvertBack.</param>
        protected virtual Type GetTargetType(int converterIndex, Type finalTargetType, bool convert)
        {
            // If the current lConverter is not the last/first in the list, 
            // get a reference to the next/previous lConverter.
            IValueConverter nextConverter = null;
            if (convert)
            {
                if (converterIndex < this.Converters.Count - 1)
                {
                    nextConverter = this.Converters[converterIndex + 1];
                    if (nextConverter == null)
                    {
                        throw new InvalidOperationException("The Converters collection of the ValueConverterGroup contains a null reference at index: " + (converterIndex + 1));
                    }
                }
            }
            else
            {
                if (converterIndex > 0)
                {
                    nextConverter = this.Converters[converterIndex - 1];
                    if (nextConverter == null)
                    {
                        throw new InvalidOperationException("The Converters collection of the ValueConverterGroup contains a null reference at index: " + (converterIndex - 1));
                    }
                }
            }

            if (nextConverter != null)
            {
                var conversionAttribute = this.mCachedAttributes[nextConverter];

                // If the Convert method is going to be called, we need to use the SourceType of the next 
                // lConverter in the list.  If ConvertBack is called, use the TargetType.
                return convert ? conversionAttribute.SourceType : conversionAttribute.TargetType;
            }

            // If the current lConverter is the last one to be executed return the target type passed into the conversion method.
            return finalTargetType;
        }

        /// <summary>
        ///     This method is called when the collection is changed.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void OnConvertersCollectionChanged(object pSender, NotifyCollectionChangedEventArgs pEventArgs)
        {
            // The 'Converters' collection has been modified, so validate that each value lConverter it now
            // contains is decorated with ValueConversionAttribute and then cache the attribute value.

            IList lConvertersToProcess = null;
            if (pEventArgs.Action == NotifyCollectionChangedAction.Add || pEventArgs.Action == NotifyCollectionChangedAction.Replace)
            {
                lConvertersToProcess = pEventArgs.NewItems;
            }
            else if (pEventArgs.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IValueConverter converter in pEventArgs.OldItems)
                {
                    this.mCachedAttributes.Remove(converter);
                }
            }
            else if (pEventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                this.mCachedAttributes.Clear();
                lConvertersToProcess = this.mConverters;
            }

            if (lConvertersToProcess != null && lConvertersToProcess.Count > 0)
            {
                foreach (IValueConverter lConverter in lConvertersToProcess)
                {
                    var lAttributes = lConverter.GetType().GetCustomAttributes(typeof(ValueConversionAttribute), false);

                    if (lAttributes.Length != 1)
                    {
                        throw new InvalidOperationException("All value converters added to a ValueConverterGroup must be decorated with the ValueConversionAttribute attribute exactly once.");
                    }

                    this.mCachedAttributes.Add(lConverter, lAttributes[0] as ValueConversionAttribute);
                }
            }
        }

        #endregion // Methods.
    }
}