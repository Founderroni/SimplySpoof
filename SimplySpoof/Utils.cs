namespace SimplySpoof
{
    internal class Utils
    {
        internal static void Print(string msg, ConsoleColor color = ConsoleColor.White)
        {
            object _lock = new object();
            lock (_lock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
            }
        }

        internal static async Task<string> GetString(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }

        internal static string RandomString(int length = 16, bool alphabet = true, bool numbers = false)
        {
            string chars = "";
            if (alphabet)
                chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            if (numbers)
                chars += "0123456789";
            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }

        internal static int RandomInt(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }
    }
}
