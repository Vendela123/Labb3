using System;
using System.Globalization;
using System.Windows.Data;
using Labb3.Models;

namespace Labb3.Converters
{
    public class AnswerCorrectConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
    

            if (values.Length < 2)
                return null;

            string answerText = values[0] as string;
            Question question = values[1] as Question;

            if (question == null || answerText == null)
                return null;

            if (string.IsNullOrEmpty(question.SelectedAnswer))
                return null;

            if (answerText == question.CorrectAnswer)
                return "Correct";

            if (question.SelectedAnswer == answerText &&
                question.SelectedAnswer != question.CorrectAnswer)
                return "Wrong";

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
