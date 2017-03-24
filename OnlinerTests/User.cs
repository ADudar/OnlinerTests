namespace OnlinerTests
{
    public class User
    {
        public string Mailbox { get; set; }
        public string Password { get; set; }
        public static User Create(string email, string password)
        {
            return new User() { Mailbox = email, Password = password };
        }
    }
}
