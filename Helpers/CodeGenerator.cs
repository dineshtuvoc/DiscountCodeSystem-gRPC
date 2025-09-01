using System.Text;
using System.Text.Json;
using DiscountCode_App.Models;

namespace DiscountCode_App.Helpers
{
    public static class CodeGenerator
    {
        private static readonly char[] _chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        private static readonly string FilePath = "GeneratedCodes.json";
        private static readonly object _fileLock = new();

        // Generate a new unique code and save it
        public static string GenerateCode(int length)
        {
            string code;

            lock (_fileLock)
            {
                var codes = LoadCodes();

                // keep generating until we find a unique code
                do
                {
                    code = CreateRandomCode(length);
                }
                while (codes.Any(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));

                // add new record
                codes.Add(new DiscountCode
                {
                    Code = code,
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                });

                SaveCodes(codes);
            }

            return code;
        }

        // ----------------- Helpers -----------------

        private static string CreateRandomCode(int length)
        {
            var random = new Random();
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(_chars[random.Next(_chars.Length)]);
            }
            return sb.ToString();
        }

        private static List<DiscountCode> LoadCodes()
        {
            if (!File.Exists(FilePath))
                return new List<DiscountCode>();

            string json = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<DiscountCode>();

            return JsonSerializer.Deserialize<List<DiscountCode>>(json) ?? new List<DiscountCode>();
        }

        private static void SaveCodes(List<DiscountCode> codes)
        {
            string json = JsonSerializer.Serialize(codes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static bool CodeExists(string code)
        {
            lock (_fileLock)
            {
                var codes = LoadCodes();
                return codes.Any(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static bool UseCode(string code)
        {
            lock (_fileLock)
            {
                var codes = LoadCodes();
                var discount = codes.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

                if (discount == null) return false;
                if (discount.IsUsed) return false;

                discount.IsUsed = true;
                SaveCodes(codes);
                return true;
            }
        }

    }
}
