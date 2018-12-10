namespace XControls.NotifierButton
{
    /// <summary>
    /// Delegate handling the notification closed event.
    /// </summary>
    /// <param name="pSource">The event source.</param>
    /// <param name="pEventArgs">The event arguments.</param>
    public delegate void NotificationClosedEventHandler<TSource>(TSource pSource, NotificationClosedEventArgs pEventArgs);
}
