using MarlinAPI.Domain.Entities;
using System.Globalization;
using System.Text;

namespace MarlinAPI.Service
{
    public static class Helpers
    {
        public static string? CreateStudentRegistry(string fullName)
        {
            DateTime dateTime = DateTime.Now;

            string firstChars = fullName.Length > 3 ? fullName.Substring(0, 3) : "ABC";
            string year = dateTime.Year.ToString();
            string[] guid = Guid.NewGuid().ToString().Split('-');

            return $"{firstChars.RemoveDiacritics().ToUpperInvariant()}-{year}-{guid[1].ToUpperInvariant()}";
        }

        public static string CreateClassRegistry()
        {
            string[] guid = Guid.NewGuid().ToString().Split('-');
            return $"CL{guid[0].ToUpper()}";
        }

        public static bool IsValidCPF(this string cpf)
        {
            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            var digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public static string? RemoveDiacritics(this string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var length = normalizedString.Length;
            var stringBuilder = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                var c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }
    }
}
