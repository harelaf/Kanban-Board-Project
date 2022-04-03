namespace KanbanTesting
{
    internal struct TestUser
    {
        internal string Email;
        internal string Nickname;
        internal string Password;
        internal TestUser(string email, string password, string nickname)
        {
            Email = email;
            Nickname = nickname;
            Password = password;
        }
    }
}