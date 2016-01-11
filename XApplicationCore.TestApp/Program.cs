namespace XApplicationCore.TestApp
{
    class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="pArgs">The arguments.</param>
        static void Main(string[] pArgs)
        {
            ConsoleApplication lApplication = new ConsoleApplication();
            lApplication.Initialize();
            lApplication.Run();
        }
    }
}
