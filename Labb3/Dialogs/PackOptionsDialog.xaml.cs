using System.Windows;
using Labb3.Models;

namespace Labb3.Dialogs
{
    public partial class PackOptionsDialog : Window
    {
        public QuestionPack Pack { get; }

        public PackOptionsDialog(QuestionPack pack)
        {
            InitializeComponent();
            Pack = pack;
            DataContext = Pack;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
