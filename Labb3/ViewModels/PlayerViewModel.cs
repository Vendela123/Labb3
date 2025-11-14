using Labb3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Labb3.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        private QuestionPack? _activePack;
        private List<Question> _shuffledQuestions = new List<Question>();
        private int _currentIndex = -1;
        private int _score = 0;

        private Timer _timer = new Timer(1000);
        private int _timeLeft;

        public Question? CurrentQuestion =>
            (_currentIndex >= 0 && _currentIndex < _shuffledQuestions.Count)
                ? _shuffledQuestions[_currentIndex]
                : null;

        public ObservableCollection<string> ShuffledAnswers { get; } = new ObservableCollection<string>();

        public string TimerText => $"{_timeLeft}s";
        public string CurrentStatus => $"Question {Math.Max(0, _currentIndex + 1)} / {_shuffledQuestions.Count}";

        public RelayCommand AnswerCommand { get; }

        public PlayerViewModel(MainViewModel mainVM)
        {
            _mainViewModel = mainVM;

            AnswerCommand = new RelayCommand(param => OnAnswer(param?.ToString()));

            _timer.Elapsed += (s, e) =>
            {
                _timeLeft--;
                Raise(nameof(TimerText));

                if (_timeLeft <= 0)
                {
                    _timer.Stop();
                    ProcessTimeout();
                }
            };
        }

        public void StartPack(QuestionPack pack)
        {
            _activePack = pack;
            _score = 0;

            _shuffledQuestions = pack.Questions.OrderBy(x => Guid.NewGuid()).ToList();
            _currentIndex = -1;

            NextQuestion();
        }

        private void NextQuestion()
        {
            _currentIndex++;

            if (_currentIndex >= _shuffledQuestions.Count)
            {
                FinishQuiz();
                return;
            }

            var q = _shuffledQuestions[_currentIndex];

            q.SelectedAnswer = null;
            q.IsCorrect = null;

            var answers = q.Answers.OrderBy(x => Guid.NewGuid()).ToList();

            ShuffledAnswers.Clear();
            foreach (var a in answers)
                ShuffledAnswers.Add(a);

            _timeLeft = _activePack?.TimePerQuestion ?? 20;
            _timer.Start();

            Raise(nameof(CurrentQuestion));
            Raise(nameof(CurrentStatus));
            Raise(nameof(TimerText));
        }

        private async void OnAnswer(string? answer)
        {
            if (CurrentQuestion == null || answer == null)
                return;

            _timer.Stop();

            CurrentQuestion.SelectedAnswer = answer;

            if (answer == CurrentQuestion.CorrectAnswer)
            {
                CurrentQuestion.IsCorrect = true;
                _score++;
            }
            else
            {
                CurrentQuestion.IsCorrect = false;
            }

            Raise(nameof(CurrentQuestion));
            Raise(nameof(ShuffledAnswers));

            await Task.Delay(1500);

            NextQuestion();
        }

        private void ProcessTimeout()
        {
            if (CurrentQuestion != null)
            {
                CurrentQuestion.SelectedAnswer = "TIMEOUT";
                CurrentQuestion.IsCorrect = false;

                Raise(nameof(CurrentQuestion));
                Raise(nameof(ShuffledAnswers));
            }

            NextQuestion();
        }

        private void FinishQuiz()
        {
            _timer.Stop();

            int correct = _score;
            int total = _shuffledQuestions.Count;

            var resultVM = new ResultViewModel(_mainViewModel, correct, total);
            _mainViewModel.CurrentView = resultVM;
        }
    }
}
