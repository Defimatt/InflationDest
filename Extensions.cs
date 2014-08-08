using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Duffles.InflationDest
{
    public static class Extensions
    {
        // Add string to a RichTextBox as a new paragraph and scroll to the end of the text
        public static void AddParagraph(this RichTextBox textBox, string paragraph)
        {
            textBox.Document.Blocks.Add(new Paragraph(new Run(paragraph)));
            textBox.ScrollToEnd();
        }
    }

    //Wrap the clipboard so it doesn't throw Exceptions if it's locked by another program
    public static class SafeClipboard
    {
        public static bool SetText(string value)
        {
            try
            {
                Clipboard.SetText(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
