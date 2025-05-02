using System.Runtime.InteropServices;
using System.Windows;

namespace SkyrimActorValueEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("Kernel32")]
        private static extern void AllocConsole();

        public MainWindow()
        {
            AllocConsole();
            InitializeComponent();
        }
    }
}