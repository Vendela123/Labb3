using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Labb3.Models;
using System.Collections.ObjectModel;

namespace Labb3.Services
{
    public class JsonService
    {
        private readonly string _folder;
        private readonly string _file;

        public JsonService()
        {
            _folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuizConfigurator");
            _file = Path.Combine(_folder, "packs.json");
        }

        public async Task SavePacksAsync(IEnumerable<QuestionPack> packs)
        {
            try
            {
                if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
                var options = new JsonSerializerOptions { WriteIndented = true };
                using var fs = File.Open(_file, FileMode.Create, FileAccess.Write, FileShare.None);
                await JsonSerializer.SerializeAsync(fs, packs.Select(p => new SerializablePack(p)), options);
            }
            catch
            {
            }
        }

        public async Task<List<QuestionPack>?> LoadPacksAsync()
        {
            try
            {
                if (!File.Exists(_file)) return new List<QuestionPack>();
                using var fs = File.OpenRead(_file);
                var data = await JsonSerializer.DeserializeAsync<List<SerializablePack>>(fs);
                return data?.Select(sp => sp.ToQuestionPack()).ToList();
            }
            catch
            {
                return new List<QuestionPack>();
            }
        }

        private record SerializablePack(string Name, string Difficulty, int TimePerQuestion, List<Question> Questions)
        {
            public SerializablePack(QuestionPack p) : this(p.Name, p.Difficulty, p.TimePerQuestion, p.Questions.ToList()) {}
            public QuestionPack ToQuestionPack() => new QuestionPack { Name = Name, Difficulty = Difficulty, TimePerQuestion = TimePerQuestion, Questions = new ObservableCollection<Question>(Questions) };
        }
    }
}
