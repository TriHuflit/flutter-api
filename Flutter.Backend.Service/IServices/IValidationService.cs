namespace Flutter.Backend.Service.IServices
{
    public interface IValidationService
    {
        bool ValidatePasswordFormat(string password, int minLength = 8, int maxLenght = 16, bool withSpecialCharacter = true);

        bool ValidateEmailFormat(string email, string pattern = "^(\\s+)?\\w+([-+.']\\w+)*@[a-z0-9A-Z]+([-.][a-z0-9A-Z]+)*\\.[a-z0-9A-Z]+([-.][a-z0-9A-Z]+)*(\\s+)?$");

        bool ValidatePhoneNumberFormat(string phone, string pattern = "(\\+[0-9]{2}|\\+[0-9]{2}\\(0\\)|\\(\\+[0-9]{2}\\)\\(0\\)|00[0-9]{2}|0)([0-9]{9}|[0-9\\-\\s]{9,18})");

        bool ValidateVnPhoneNumberFormat(string phoneNumber, string pattern = "^(0|\\+84)(\\d{3})(\\d{3})(\\d{3,5})$$");

        bool IsMatchRegularExpression(string input, string pattern);
    }
}
