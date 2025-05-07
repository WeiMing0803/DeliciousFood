namespace AI.DeliciousFood.Core.Common.ClientHelper;

public class UserHelper
{
    public static string GeneratedPassword()
    {
        int passwordLength = 8;
        string allowedNum = "0123456789";
        Random rd = new();
        List<char> password = [.. GeneratePassword(passwordLength - 1, 1)];
        char[] restrictSymbols = ['!', '@', '#', '$', '%', '&', '*', '(', ')', '[', ']', ';', '.', '/', ','];
        char[] denySymbols = ['^', '_', '-', '+', '=', ':', '<', '>', '?', '|', '{', '}'];
        List<char> generatedPassword = [];
        for (int i = 0; i < password.Count; i++)
        {
            char symbol = password[i];
            if (denySymbols.Contains(symbol))
                symbol = restrictSymbols[rd.Next(restrictSymbols.Length - 1)];
            generatedPassword.Add(symbol);
        }
        generatedPassword.Insert(rd.Next(passwordLength), allowedNum[rd.Next(allowedNum.Length)]);
        return new(generatedPassword.ToArray());
    }

    private static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
    {
        string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        string allowedNonAlphanumericChars = "!@$?_-";
        Random rd = new();

        if (numberOfNonAlphanumericCharacters > length || length <= 0 || numberOfNonAlphanumericCharacters < 0)
            throw new ArgumentOutOfRangeException();

        char[] pass = new char[length];

        for (int i = 0; i < numberOfNonAlphanumericCharacters; i++)
        {
            pass[i] = allowedNonAlphanumericChars[rd.Next(0, allowedNonAlphanumericChars.Length)];
        }
        for (int i = numberOfNonAlphanumericCharacters; i < length; i++)
        {
            pass[i] = allowedChars[rd.Next(0, allowedChars.Length)];
        }

        return new(pass);
    }
}
