using Duffles.InflationDest.Content;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Duffles.InflationDest.Pages
{
    public partial class Introduction : UserControl
    {
        public Introduction()
        {
            InitializeComponent();
        }

        private void CopyClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!SafeClipboard.SetText(Stellar.IsChecked.Value ? "gDUNatU6tFPgkgW58U5uNf41ArMDCPD6XV" : "1PAYMATT1UMKDPuHfqvwTmJuEtuYt7BspQ"))
            {
                ModernDialog.ShowMessage("Couldn't set the clipboard. Another program may be holding it open exclusively.\r\n\r\n(Examples include VNC clients and Remote Desktop programs.)", "Clipboard unavailable", MessageBoxButton.OK);
            }

            // Tick
            CopyClipboard.IconData = Geometry.Parse("F1 M 23.7501,33.25L 34.8334,44.3333L 52.2499,22.1668L 56.9999,26.9168L 34.8334,53.8333L 19.0001,38L 23.7501,33.25 Z ");

            // Reset to clipboard after 1 second
            var resetIconTimer = new Timer(1000) { AutoReset = false };
            var dispatcher = this.Dispatcher;
            resetIconTimer.Elapsed += (s, e1) => { dispatcher.Invoke(new Action(() => CopyClipboard.IconData = Geometry.Parse("F1 M 23,54L 23,26C 23,24.3432 24.3431,23 26,23L 30.5001,22.9999C 30.5001,22.9999 31.4999,22.8807 31.4999,21.5C 31.4999,20.1193 33.1191,19 34.4998,19L 41.5001,19C 42.8809,19 44.5001,20.1193 44.5001,21.5C 44.5001,22.8807 45.4999,22.9999 45.4999,22.9999L 50,23.0001C 51.6569,23.0001 53,24.3432 53,26.0001L 53,54.0001C 53,55.6569 51.6568,57 50,57.0001L 26,57C 24.3431,57 23,55.6569 23,54 Z M 35.9997,22.0002C 34.619,22.0002 33.4997,23.1195 33.4997,24.5002C 33.4997,25.8809 32.5,27.0001 32.5,27.0001L 43.5,27.0001C 43.5,27.0001 42.5002,25.8809 42.5002,24.5002C 42.5002,23.1195 41.3809,22.0002 40.0002,22.0002L 35.9997,22.0002 Z M 28.5,30.0001L 30,26L 26,26L 26,54L 50,54L 50,26L 46,26.0001L 47.5,30.0001L 28.5,30.0001 Z"))); };
            resetIconTimer.Start();
        }
    }
}
