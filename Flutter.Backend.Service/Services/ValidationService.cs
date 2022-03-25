using Flutter.Backend.Service.IServices;
using System.Linq;
using System.Text.RegularExpressions;

namespace Flutter.Backend.Service.Services
{
    public class ValidationService : IValidationService
    {
        public bool ValidatePasswordFormat(string password, int minLength = 8, int maxLenght = 16, bool withSpecialCharacter = true)
        {
            Regex regex = new Regex("[0-9]+");
            Regex regex2 = new Regex(".{" + minLength + ",}");
            Regex regex3 = new Regex(".{" + maxLenght + ",}");
            Regex regex4 = new Regex("[A-Z]+");
            Regex regex5 = new Regex("[a-z]+");
            bool flag = regex.IsMatch(password) && (regex2.IsMatch(password) || regex3.IsMatch(password)) && regex4.IsMatch(password) && regex5.IsMatch(password);
            if (flag && withSpecialCharacter)
            {
                flag = password.Any((char ch) => !char.IsLetterOrDigit(ch));
            }

            return flag;
        }

        public bool ValidateEmailFormat(string email, string pattern = "^(\\s+)?\\w+([-+.']\\w+)*@[a-z0-9A-Z]+([-.][a-z0-9A-Z]+)*\\.[a-z0-9A-Z]+([-.][a-z0-9A-Z]+)*(\\s+)?$")
        {
            return IsMatchRegularExpression(email, pattern);
        }

        public bool ValidatePhoneNumberFormat(string phoneNumber, string pattern = "(\\+[0-9]{2}|\\+[0-9]{2}\\(0\\)|\\(\\+[0-9]{2}\\)\\(0\\)|00[0-9]{2}|0)([0-9]{9}|[0-9\\-\\s]{9,18})")
        {
            return IsMatchRegularExpression(phoneNumber, pattern);
        }

        public bool ValidateUserName(string userName)
        {
            string[] splitUserName = userName.Split(' ');
            if(splitUserName.Length > 1)
            {
                return false;
            }
            return true;
        }

        public bool ValidateVnPhoneNumberFormat(string phoneNumber, string pattern = "^(0|\\+84)(\\d{3})(\\d{3})(\\d{3,5})$$")
        {
            return IsMatchRegularExpression(phoneNumber, pattern);
        }

        public bool IsMatchRegularExpression(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
    }
}
