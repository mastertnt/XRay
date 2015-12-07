using System;

namespace XApplicationCore
{
    /// <summary>
    /// This application is the base class for a non-GUI application.
    /// </summary>
    public class ConsoleApplication : AApplication
    {
        /// <summary>
        /// Runs this instance.
        /// </summary>
        public override void Run()
        {
            Console.WriteLine("Press any key to quit.");
            ConsoleKeyInfo lKey = Console.ReadKey(true);
        }
    }
}
