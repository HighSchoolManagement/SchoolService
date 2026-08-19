


namespace SchoolService.Application.Schools
{
    public static class SchoolValidator
    {
        public static void ValidateNameRequired(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");
        }

    }
}
