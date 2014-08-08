using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Windows;

namespace Duffles.InflationDest
{
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the default skin to be the Stellar image and a light blue theme
            AppearanceManager.Current.ThemeSource = new Uri("/Duffles.InflationDest;component/Assets/InflationDest.Stellar.xaml", UriKind.Relative);
        }
    }
}