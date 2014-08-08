using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Duffles.InflationDest.Content
{
    // TODO: Extract (& localise?) strings
    public partial class GetInflationDest : UserControl
    {
        public GetInflationDest()
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

            output.AddParagraph("Beginning operation GetInflationDest...");

            worker.RunWorkerAsync(this.StellarAddress.Text);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null && e.Error is Exception)
            {
                ModernDialog.ShowMessage("An error occurred while getting the InflationDest.\r\nError details can be found in the technical information box.", "Get InflationDest", MessageBoxButton.OK);

                output.AddParagraph((e.Error as Exception).ToString());
            }
            else
            { 
                if (e.Result == null)
                {
                    ModernDialog.ShowMessage("I couldn't get the InflationDest of that address.\r\nSorry I couldn't be more help (at least in this version!).\r\nPerhaps the technical information box will have some hints about what went wrong?\r\n\r\n[b]N.B.[/b] Some addresses don't have InflationDests. For example, addresses created using stellard instead of the web client won't have by default.", "Get InflationDest", MessageBoxButton.OK);
                }
                else
                {
                    if (ModernDialog.ShowMessage("InflationDest is: " + e.Result.ToString() + "\r\nDo you wish to copy this to the clipboard?", "Get InflationDest", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

            var address = "https://live.stellar.org:9002";
            var query = "{\"method\":\"account_info\",\"params\":[{\"account\":\"" + (string)e.Argument + "\"}]}";

            me.ReportProgress(0, "Sending " + query + " to " + address);

            var response = client.UploadString(address, query);

            me.ReportProgress(0, "Received response from stellard: " + response);
            me.ReportProgress(0, "Parsing JSON");

            var root = Newtonsoft.Json.Linq.JToken.Parse(response);
            var reader = root.CreateReader();
            string result = null;

            while (reader.Read() == true)
            {
                if (reader.Value != null && reader.Value.ToString() == "InflationDest")
                {
                    if (reader.Read() == true)
                    {
                        result = reader.Value.ToString();
                    }
                }
            }

            if (result != null)
            {
                me.ReportProgress(0, "InflationDest is " + result);
                e.Result = result;
            }
        }
    }
}