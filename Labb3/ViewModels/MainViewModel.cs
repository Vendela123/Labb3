using Labb3.Models;
using System.Windows;

namespace Labb3.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ConfigurationViewModel _configViewModel;
        private readonly PlayerViewModel _playerViewModel;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; Raise(nameof(CurrentView)); }
        }

        public ConfigurationViewModel ConfigViewModel => _configViewModel;

        public RelayCommand SwitchToEditCommand { get; }
        public RelayCommand SwitchToPlayCommand { get; }
        public RelayCommand ImportQuestionsCommand { get; }
        public RelayCommand NewPackCommand { get; }
        public RelayCommand DeletePackCommand { get; }
        public RelayCommand SelectPackCommand { get; }

        public MainViewModel()
        {
            _configViewModel = new ConfigurationViewModel();
            _playerViewModel = new PlayerViewModel(this);

            CurrentView = _configViewModel;

            SwitchToEditCommand = new RelayCommand(_ => SwitchToEdit());
            SwitchToPlayCommand = new RelayCommand(_ => SwitchToPlay());
            ImportQuestionsCommand = new RelayCommand(_ => _configViewModel.RunImport());
            NewPackCommand = new RelayCommand(_ => _configViewModel.CreateNewPack());
            DeletePackCommand = new RelayCommand(_ => _configViewModel.RemoveSelectedPack(), _ => _configViewModel.SelectedPack != null);
            SelectPackCommand = new RelayCommand(pack => _configViewModel.SelectedPack = pack as QuestionPack);
        }

        private void SwitchToPlay()
        {
            if (_configViewModel.SelectedPack != null)
            {
                _playerViewModel.StartPack(_configViewModel.SelectedPack);
                CurrentView = _playerViewModel;
            }
            else
            {
                MessageBox.Show("No question pack selected.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void SwitchToEdit()
        {
            CurrentView = _configViewModel;
        }
    }
}
