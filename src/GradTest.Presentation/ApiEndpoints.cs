namespace GradTest.Presentation;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Users
    {
        private const string UserBase = $"{ApiBase}/users";

        public const string GetUserById = $"{UserBase}/{{userId}}";
        public const string CreateUser = UserBase;
        public const string GetMyUser = $"{UserBase}/me";
    }
}