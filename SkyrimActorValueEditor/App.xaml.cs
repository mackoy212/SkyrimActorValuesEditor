using SkyrimActorValueEditor.Core.Services.Config;
using Syncfusion.Licensing;
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
            if (ConfigService.Get().Debug)
                AllocConsole();

            SyncfusionLicenseProvider.RegisterLicense(SyncfusionKeyGetter.GetKey());

            base.OnStartup(e);
        }
    }
}