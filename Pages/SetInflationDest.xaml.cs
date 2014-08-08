using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Duffles.InflationDest.Content
{
    // TODO: Extract (& localise?) strings
    public partial class SetInflationDest : UserControl
    {
        public SetInflationDest()
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

            output.AddParagraph("Beginning operation SetInflationDest...");
            
            // TODO: Replace Tuple with a struct
            worker.RunWorkerAsync(new Tuple<string, string, string>(this.StellarAddress.Text, this.SecretKey.Text, this.VoteAddress.Text));
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null && e.Error is Exception)
            {
                ModernDialog.ShowMessage("An error occurred while setting the InflationDest.\r\nError details can be found in the technical information box.", "Set InflationDest", MessageBoxButton.OK);

                output.AddParagraph((e.Error as Exception).ToString());
            }
            else
            { 
                if (e.Result == null)
                {
                    ModernDialog.ShowMessage("I couldn't set the InflationDest of that address.\r\nSorry I couldn't be more help (at least in this version!).\r\nPerhaps the technical information box will have some hints about what went wrong?\r\n", "Set InflationDest", MessageBoxButton.OK);
                }
                else
                {
                    ModernDialog.ShowMessage("InflationDest is now: " + e.Result.ToString(), "Set InflationDest", MessageBoxButton.OK);
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

            var tuple = (Tuple<string, string, string>)e.Argument;

            var address = "https://live.stellar.org:9002";
            var query = "{\"method\":\"submit\",\"params\":[{\"secret\":\"" + tuple.Item2 + "\",\"tx_json\":{\"Account\":\"" + tuple.Item1 + "\",\"InflationDest\":\"" + tuple.Item3 + "\",\"TransactionType\":\"AccountSet\"}}]}";

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

                if (reader.Value != null && reader.Value.ToString() == "status")
                {
                    if (reader.Read() == true)
                    {
                        if (reader.Value != null && reader.Value.ToString() == "error")
                        {
                            throw new Exception("Stellard returned an error");
                        }
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
