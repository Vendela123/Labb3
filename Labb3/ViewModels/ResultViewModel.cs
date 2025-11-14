using System.Windows.Input;

namespace Labb3.ViewModels
{
    public class ResultViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        public string ResultText { get; }
        public RelayCommand RestartCommand { get; }

        public ResultViewModel(MainViewModel mainViewModel, int correctAnswers, int totalQuestions)
        {
            _mainViewModel = mainViewModel;
            ResultText = $"You got {correctAnswers} out of {totalQuestions} answers correct!";
            RestartCommand = new RelayCommand(_ => Restart());
        }

        private void Restart()
        {
            _mainViewModel.SwitchToEdit();
        }
    }
}
