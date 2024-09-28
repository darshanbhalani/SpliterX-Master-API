namespace API_SpliterX.Shared
{
    public class DB
    {
        private static string Function = "SELECT * FROM ";
        private static string Schema = "PUBLIC.";

        public static string LogIn = Function + Schema + "USER_LOGIN";
        public static string SignUp = Function + Schema + "USER_SIGNUP";
    }
}
