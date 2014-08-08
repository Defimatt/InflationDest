using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Duffles.InflationDest.Content
{
    // TODO: Extract (& localise?) strings
    public partial class Lookup : UserControl
    {
        public Lookup()
        {
            InitializeComponent();
        }

        private void DoInflationOperation_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Change this to use ICommand.CanExecute
            Spinner.IsActive = true;
            DoInflationOperation.IsEnabled = false;

            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            output.AddParagraph("Beginning operation Lookup...");

            worker.RunWorkerAsync(this.StellarAddress.Text);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null && e.Error is Exception)
            {
                if (e.Error.ToString().Contains("404"))
                {
                    ModernDialog.ShowMessage("The Federation server returned HTTP error 404.\r\nThat means it doesn't believe that username exists.", "Lookup", MessageBoxButton.OK);
                }
                else
                {
                    ModernDialog.ShowMessage("An error occurred while getting the Stellar address.\r\nError details can be found in the technical information box.", "Lookup", MessageBoxButton.OK);
                }

                output.AddParagraph((e.Error as Exception).ToString());
            }
            else
            { 
                if (e.Result == null)
                {
                    ModernDialog.ShowMessage("I couldn't get the Stellar address of that username.\r\nSorry I couldn't be more help (at least in this version!).\r\nPerhaps the technical information box will have some hints about what went wrong?\r\n", "Lookup", MessageBoxButton.OK);
                }
                else
                {
                    if (ModernDialog.ShowMessage("Stellar address is: " + e.Result.ToString() + "\r\nDo you wish to copy this to the clipboard?", "Lookup", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (!SafeClipboard.SetText(e.Result.ToString()))
                        {
                            ModernDialog.ShowMessage("Couldn't set the clipboard. Another program may be holding it open exclusively.\r\n\r\n(Examples include VNC clients and Remote Desktop programs.)", "Clipboard unavailable", MessageBoxButton.OK);
                        }
                    }
                }
            }

            // TODO: Change this to use ICommand.CanExecute
            Spinner.IsActive = false;
            DoInflationOperation.IsEnabled = true;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            output.AddParagraph((string)e.UserState);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // TODO: Do some decent exception-handling rather than allowing it to get passed up to worker_RunWorkerCompleted
            var me = (BackgroundWorker)sender;

            me.ReportProgress(0, "Initialising WebClient");

            var client = new WebClient();

            me.ReportProgress(0, "Building query");

            var address = "https://api.stellar.org/federation?destination=" + (string)e.Argument + "&domain=stellar.org&type=federation";

            me.ReportProgress(0, "Querying " + address);

            var response = client.DownloadString(address);

            me.ReportProgress(0, "Received response from stellard: " + response);
            me.ReportProgress(0, "Parsing JSON");

            var root = Newtonsoft.Json.Linq.JToken.Parse(response);
            var reader = root.CreateReader();
            string result = null;

            while (reader.Read() == true)
            {
                if (reader.Value != null && reader.Value.ToString() == "destination_address")
                {
                    if (reader.Read() == true)
                    {
                        result = reader.Value.ToString();
                    }
                }

                if (reader.Value != null && reader.Value.ToString() == "result")
                {
                    if (reader.Read() == true)
                    {
                        if (reader.Value != null && reader.Value.ToString() == "error")
                        {
                            throw new Exception("Federation server returned an error");
                        }
                    }
                }
            }

            if (result != null)
            {
                me.ReportProgress(0, "Stellar address is " + result);
                e.Result = result;
            }
        }
    }
}