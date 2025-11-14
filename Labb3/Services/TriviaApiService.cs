using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Labb3.Models;

namespace Labb3.Services
{
    public class TriviaApiService
    {
        private readonly HttpClient _http;

        public TriviaApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };
            _http = new HttpClient(handler);
        }

        public async Task<List<TriviaCategory>> GetCategoriesAsync()
        {
            try
            {
                var response = await _http.GetAsync("https://opentdb.com/api_category.php");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.Deserialize<CategoryResponse>(json, options);

                return data?.TriviaCategories ?? new List<TriviaCategory>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                return new List<TriviaCategory>();
            }
        }

        public async Task<List<Question>> GetQuestionsAsync(int categoryId, string difficulty, int amount)
        {
            try
            {
                string url = $"https://opentdb.com/api.php?amount={amount}&category={categoryId}&difficulty={difficulty}&type=multiple";

                var response = await _http.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = JsonSerializer.Deserialize<TriviaQuestionResponse>(json, options);

                var result = new List<Question>();
                if (data?.Results != null)
                {
                    var random = new Random();
                    foreach (var item in data.Results)
                    {
                        var answers = new List<string>(item.IncorrectAnswers);
                        answers.Insert(random.Next(answers.Count + 1), item.CorrectAnswer);

                        result.Add(new Question
                        {
                            Text = System.Net.WebUtility.HtmlDecode(item.Question),
                            Answers = answers.ToArray(),
                            CorrectIndex = answers.IndexOf(item.CorrectAnswer)
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching questions: {ex.Message}");
                return new List<Question>();
            }
        }

        private class CategoryResponse
        {
            [JsonPropertyName("trivia_categories")]
            public List<TriviaCategory>? TriviaCategories { get; set; }
        }

        public class TriviaCategory
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; } = "";
        }

        private class TriviaQuestionResponse
        {
            [JsonPropertyName("results")]
            public List<TriviaQuestion>? Results { get; set; }
        }

        private class TriviaQuestion
        {
            [JsonPropertyName("question")]
            public string Question { get; set; } = "";

            [JsonPropertyName("correct_answer")]
            public string CorrectAnswer { get; set; } = "";

            [JsonPropertyName("incorrect_answers")]
            public List<string> IncorrectAnswers { get; set; } = new();
        }
    }
}