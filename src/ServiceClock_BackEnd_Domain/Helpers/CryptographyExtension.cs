
namespace ServiceClock_BackEnd.Domain.Helpers;

public static class CryptographyExtension
{
    public static string PasswordEncryption(this string password)
    {
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            string encryptedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return encryptedPassword;
        }
    }
}

