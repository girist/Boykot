namespace Boykot.WebApp.Extensions
{
    public static class StringExtensions
    {
        public static string GetStringReplace(this string input)
        {
            char[] turkish = { 'ı', 'ğ', 'İ', 'Ğ', 'ç', 'Ç', 'ş', 'Ş', 'ö', 'Ö', 'ü', 'Ü', ' ' };
            char[] english = { 'i', 'g', 'I', 'G', 'c', 'C', 's', 'S', 'o', 'O', 'u', 'U', '_' };
            for (int i = 0; i < turkish.Length; i++)
            {

                input = input.Replace(turkish[i], english[i]);

            }
            return input;
        }
    }
}
