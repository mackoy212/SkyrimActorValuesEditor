using SkyrimActorValueEditor.Core.Services;
using System.Runtime.InteropServices;
using System.Windows;

namespace SkyrimActorValueEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("Kernel32")]
        private static extern void AllocConsole();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (ConfigService.Get().Debug)
                AllocConsole();
        }
    }
}