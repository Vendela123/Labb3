using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Labb3.Models
{
    public class Question : INotifyPropertyChanged
    {
        public string Text { get; set; } = "";
        public string[] Answers { get; set; } = new string[4];
        public int CorrectIndex { get; set; }

        private string? _selectedAnswer;
        public string? SelectedAnswer
        {
            get => _selectedAnswer;
            set
            {
                _selectedAnswer = value;
                OnPropertyChanged();
            }
        }


        private bool? _isCorrect;
        public bool? IsCorrect
        {
            get => _isCorrect;
            set
            {
                _isCorrect = value;
                OnPropertyChanged();
            }
        }

        public string CorrectAnswer => Answers[CorrectIndex];

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
