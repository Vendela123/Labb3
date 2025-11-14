using System.Collections.ObjectModel;

namespace Labb3.Models
{
    public class QuestionPack
    {
        public string Name { get; set; } = "New Question Pack";
        public string Difficulty { get; set; } = "Medium";
        public int TimePerQuestion { get; set; } = 20;

        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
    }
}
