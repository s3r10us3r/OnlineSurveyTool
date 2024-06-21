using System.Text.RegularExpressions;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.Utils;

static class Validator
{
    private const string EMAIL_REGEX = """(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])""";
    private const string LOGIN_REGEX = @"^[A-Za-z\d_]+$";
    
    public static bool ValidatePassword(string password, out string message)
    {
        message = "";
        if (password.Length < 8)
        {
            message = "Password's length must be greater than 8!";
            return false;
        }
        if (password.Length > 64)
        {
            message = "Password's length must be lower than 64!";
            return false;
        }
        if (password.All(char.IsLetterOrDigit))
        {
            message = "Password must contain at least one non alphanumeric character!";
            return false;
        }
        if (!password.Any(char.IsUpper))
        {
            message = "Password must contain at least one uppercase letter!";
            return false;
        }
        if (!password.Any(char.IsLower))
        {
            message = "Password must contain at least one lowercase letter!";
            return false;
        }
        if (!password.Any(char.IsDigit))
        {
            message = "Password must contain at least one digit!";
            return false;
        }
        if (password.Any(char.IsWhiteSpace))
        {
            message = "Password cannot contain any whitespace!";
            return false;
        }

        return true;
    }
    
    public static bool ValidateLogin(string login, out string message)
    {
        bool hasValidLength = login.Length is >= 8 and <= 64;
        bool matchesRegex = Regex.IsMatch(login, LOGIN_REGEX);
        if (!hasValidLength)
        {
            message = "Login is too short!";
            return false;
        }
        if (!matchesRegex)
        {
            message = "Login can't have any other character than letters, digits and '_' ";
            return false;
        }
        message = "";
        return true;
    }

    public static bool ValidateEMail(string email, out string message)
    {
        bool satisfiesEmailRegex = Regex.IsMatch(email, EMAIL_REGEX);
        if (!satisfiesEmailRegex)
        {
            message = "Invalid email";
            return false;
        }
        message = "";
        return true;
    }
}