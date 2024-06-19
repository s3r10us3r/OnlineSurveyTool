using System.Text.RegularExpressions;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.Utils;

static class Validator
{
    private const string EMAIL_REGEX = """(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])""";
    
    public static bool ValidatePassword(string password, out string message)
    {
        message = "";

        bool hasNonAlphanumeric = Regex.IsMatch(password, @"[^a-zA-Z0-9]");
        bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
        bool hasLowerCase = Regex.IsMatch(password, @"[a-z]");
        bool hasDigit = Regex.IsMatch(password, @"\d");

        if (password.Length < 8)
        {
            message = "Password's length must be greater than 8!";
        }
        if (!hasNonAlphanumeric)
        {
            message = "Password must contain at least one non alphanumeric character!";
        }
        if (!hasUpperCase)
        {
            message = "Password must contain at least one uppercase letter!";
        }
        if (!hasLowerCase)
        {
            message = "Password must contain at least one lowercase letter!";
        }
        if (!hasDigit)
        {
            message = "Password must contain at least one digit!";
        }

        return password.Length > 8 && hasNonAlphanumeric && hasUpperCase && hasLowerCase && hasDigit;
    }
    
    public static bool ValidateLogin(string login, out string message)
    {
        bool hasValidLength = login.Length >= 8;
        if (!hasValidLength)
        {
            message = "Login is too short!";
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
            message = "Invalid eMail";
            return false;
        }
        message = "";
        return true;
    }
}