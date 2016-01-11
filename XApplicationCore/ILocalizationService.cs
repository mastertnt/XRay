namespace XApplicationCore
{
    /// <summary>
    /// This interface defines a localization service.
    /// </summary>
    interface ILocalizationService : IService
    {
        /// <summary>
        /// Gets the localized display string.
        /// </summary>
        /// <param name="pContext">The localization context.</param>
        /// <param name="pKey">The localization key.</param>
        /// <returns>The localization description.</returns>
        string GetLocalizedDisplayString(string pContext, string pKey);

        /// <summary>
        /// Gets the localized description.
        /// </summary>
        /// <param name="pContext">The localization context.</param>
        /// <param name="pKey">The localization key.</param>
        /// <returns>The localization description.</returns>
        string GetLocalizedDescription(string pContext, string pKey);
    }
}
