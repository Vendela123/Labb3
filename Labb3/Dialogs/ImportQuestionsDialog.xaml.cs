using Labb3.Models;
using Labb3.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Labb3.Dialogs
{
    public partial class ImportQuestionsDialog : Window
    {
        private readonly TriviaApiService _api = new TriviaApiService();
        public List<Question>? ImportedQuestions { get; private set; }

        public ImportQuestionsDialog()
        {
            InitializeComponent();
            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            StatusText.Text = "Loading categories...";
            var categories = await _api.GetCategoriesAsync();
            CategoryCombo.ItemsSource = categories;
            CategoryCombo.DisplayMemberPath = "Name";
            CategoryCombo.SelectedValuePath = "Id";
            StatusText.Text = $"Loaded {categories.Count} categories.";
        }

        private async void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryCombo.SelectedValue is not int categoryId)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

            string difficulty = (DifficultyCombo.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "medium";
            int amount = int.TryParse(AmountBox.Text, out int a) ? a : 5;

            StatusText.Text = "Importing...";
            ImportedQuestions = await _api.GetQuestionsAsync(categoryId, difficulty, amount);

            if (ImportedQuestions.Count > 0)
            {
                StatusText.Text = $"Imported {ImportedQuestions.Count} questions!";
                DialogResult = true;
            }
            else
            {
                StatusText.Text = "Import failed or returned 0 questions.";
            }
        }
    }
}
