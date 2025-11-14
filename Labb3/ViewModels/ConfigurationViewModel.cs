using Labb3.Models;
using Labb3.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Labb3.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {


        private readonly JsonService _jsonService = new JsonService();

        public ObservableCollection<QuestionPack> Packs { get; } = new ObservableCollection<QuestionPack>();

        private QuestionPack? _selectedPack;
        public QuestionPack? SelectedPack
        {
            get => _selectedPack;
            set
            {
                _selectedPack = value;
                Raise(nameof(SelectedPack));
                UpdateCommands();
            }
        }

        private Question? _selectedQuestion;
        public Question? SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                Raise(nameof(SelectedQuestion));
                UpdateCommands();
            }
        }

        public RelayCommand AddPackCommand { get; }
        public RelayCommand RemovePackCommand { get; }
        public RelayCommand AddQuestionCommand { get; }
        public RelayCommand DeleteQuestionCommand { get; }
        public RelayCommand EditPackOptionsCommand { get; }
        public RelayCommand ImportCommand { get; }




        public ConfigurationViewModel()
        {
            AddPackCommand = new RelayCommand(_ => AddPack(), _ => SelectedPack != null);
            RemovePackCommand = new RelayCommand(_ => RemoveSelectedPack(), _ => SelectedPack != null);
            AddQuestionCommand = new RelayCommand(_ => AddQuestion(), _ => SelectedPack != null);
            DeleteQuestionCommand = new RelayCommand(_ => DeleteQuestion(), _ => SelectedQuestion != null);
            EditPackOptionsCommand = new RelayCommand(_ => EditPackOptions(), _ => SelectedPack != null);
            ImportCommand = new RelayCommand(_ => ImportQuestions(), _ => SelectedPack != null);


            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var loaded = await _jsonService.LoadPacksAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Packs.Clear();

                if (loaded != null && loaded.Count > 0)
                {
                    foreach (var p in loaded)
                        Packs.Add(p);

                    SelectedPack = Packs[0];
                }
                else
                {
                    var defaultPack = new QuestionPack
                    {
                        Name = "Default Question Pack",
                        Difficulty = "Medium",
                        TimePerQuestion = 20
                    };

                    defaultPack.Questions.Add(new Question
                    {
                        Text = "New Question",
                        Answers = new[] { "Correct answer", "Wrong 1", "Wrong 2", "Wrong 3" },
                        CorrectIndex = 0
                    });

                    Packs.Add(defaultPack);
                    SelectedPack = defaultPack;
                    _ = _jsonService.SavePacksAsync(Packs);
                }

                UpdateCommands(); 
            });
        }

        private void AddPack()
        {
            var newPack = new QuestionPack
            {
                Name = "New Question Pack",
                Difficulty = "Medium",
                TimePerQuestion = 20
            };

            Packs.Add(newPack);
            SelectedPack = newPack;
            _ = _jsonService.SavePacksAsync(Packs);
        }

        public void RemoveSelectedPack()
        {
            if (SelectedPack == null) return;

            if (MessageBox.Show($"Delete '{SelectedPack.Name}'?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Packs.Remove(SelectedPack);
                SelectedPack = Packs.Count > 0 ? Packs[0] : null;
                _ = _jsonService.SavePacksAsync(Packs);
            }
        }

        private void AddQuestion()
        {
            if (SelectedPack == null) return;

            var q = new Question
            {
                Text = "New Question",
                Answers = new[] { "Correct answer", "Wrong 1", "Wrong 2", "Wrong 3" },
                CorrectIndex = 0
            };

            SelectedPack.Questions.Add(q);
            SelectedQuestion = q;
            _ = _jsonService.SavePacksAsync(Packs);
        }

        private void DeleteQuestion()
        {
            if (SelectedPack == null || SelectedQuestion == null) return;

            if (MessageBox.Show("Delete this question?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                SelectedPack.Questions.Remove(SelectedQuestion);
                SelectedQuestion = null;
                _ = _jsonService.SavePacksAsync(Packs);
            }
        }

        private void EditPackOptions()
        {
            if (SelectedPack == null) return;

            var dialog = new Dialogs.PackOptionsDialog(SelectedPack)
            {
                Owner = Application.Current.MainWindow
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                _ = _jsonService.SavePacksAsync(Packs);
                Raise(nameof(SelectedPack));
            }
        }

        public void CreateNewPack()
        {
            var newPack = new QuestionPack
            {
                Name = "New Question Pack",
                Difficulty = "Medium",
                TimePerQuestion = 20
            };

            Packs.Add(newPack);
            SelectedPack = newPack;
            _ = _jsonService.SavePacksAsync(Packs);
        }
        private async void ImportQuestions()
        {
            var dialog = new Dialogs.ImportQuestionsDialog
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true && dialog.ImportedQuestions != null)
            {
                foreach (var q in dialog.ImportedQuestions)
                    SelectedPack?.Questions.Add(q);

                await _jsonService.SavePacksAsync(Packs);
                MessageBox.Show($"Imported {dialog.ImportedQuestions.Count} questions!", "Import Complete");
            }
        }

        private void UpdateCommands()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public void RunImport()
        {
            ImportQuestions();
        }

    }
}
