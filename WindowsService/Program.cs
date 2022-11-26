using System.ServiceProcess;

namespace SampleWindowsService
{
    internal static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SampleWindowsService()
            };
            ServiceBase.Run(ServicesToRun);
        }

        #endregion Methods
    }
}