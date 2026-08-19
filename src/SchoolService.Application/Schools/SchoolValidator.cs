
using System.Net.Mail;


namespace SchoolService.Application.Schools
{
    public static class SchoolValidator
    {
        public static void ValidateNameRequired(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");
        }

        public static void ValidateEmailFormat(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!IsValidEmail(email))
                {
                    throw new ArgumentException("Email is invalid");
                }
            }
        }

        public static void ValidateNameLength(string? name)
        {
            if (name?.Length > 100)
                throw new ArgumentException("Name is invalid length");
        }

        public static void ValidateSchoolCodeLength(string? schoolCode)
        {
            if (schoolCode?.Length > 20)
                throw new ArgumentException("School Code is invalid length");
        }
        public static void ValidateSchoolCodeRequired(string? schoolCode)
        {
            if (string.IsNullOrWhiteSpace(schoolCode))
                throw new ArgumentException("School Code is required");
        }
        public static void ValidateEmailLength(string? email)
        {
            if (email?.Length > 150)
                throw new ArgumentException("Email is invalid length");
        }
        public static void ValidatePhoneNumberLength(string? phoneNumber)
        {
            if (phoneNumber?.Length > 30)
                throw new ArgumentException("Phone Number is invalid length");
        }

        public static void ValidateAddressLength(string? address)
        {
            if (address?.Length > 250)
                throw new ArgumentException("Address is invalid length");
        }

        public static void ValidateCityLength(string? city)
        {
            if (city?.Length > 50)
                throw new ArgumentException("City is invalid length");
        }

        public static void ValidatePostalCodeLength(string? postalCode)
        {
            if (postalCode?.Length > 10)
                throw new ArgumentException("Postal Code is invalid length");
        }

        public static void ValidateRegionLength(string? region)
        {
            if (region?.Length > 50)
                throw new ArgumentException("Region is invalid length");
        }

        public static void ValidateCountryLength(string? country)
        {
            if (country?.Length > 50)
                throw new ArgumentException("Country is invalid length");
        }


        private static bool IsValidEmail(string email)
        {
            try
            {
                var address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
