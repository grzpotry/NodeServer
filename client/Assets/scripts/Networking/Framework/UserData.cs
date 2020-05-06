namespace Networking.Framework
{
    public struct UserData
    {
        public string Login { get; }

        public UserData(string login)
        {
            Login = login;
        }
    }
}