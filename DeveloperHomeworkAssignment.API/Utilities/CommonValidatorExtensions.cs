namespace DeveloperHomeworkAssignment.API.Utilities
{
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    public static class CommonValidatorExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidUsername(this string username)
        {
            return Regex.IsMatch(username, @"^(?!.*[._]{2})[a-zA-Z](?:[a-zA-Z0-9._]{1,18}[a-zA-Z0-9])$");
        }

        public static bool IsValidName(this string name)
        {
            return Regex.IsMatch(name, @"^[\p{L}]+([ '-][\p{L}]+)*$");
        }

        public static bool IsValidDob(this DateTime dob)
        {
            return dob.Date >= DateTime.Now.AddYears(-200) && dob.Date <= DateTime.Now.Date;
        }
    }
}