namespace PassTheNote.Tests.Models;

public class AccountData
{
    public AccountData() { }

    public AccountData(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
