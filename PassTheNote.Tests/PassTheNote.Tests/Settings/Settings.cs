using System.Xml;
using PassTheNote.Tests.Models;

namespace PassTheNote.Tests.Settings;

public static class AppSettings
{
    public static readonly string File = "Settings.xml";

    private static string? _baseUrl;
    private static string? _defaultEmail;
    private static string? _defaultPassword;

    private static readonly XmlDocument Document;

    static AppSettings()
    {
        if (!System.IO.File.Exists(File))
            throw new Exception($"Файл настроек не найден: {File}");

        Document = new XmlDocument();
        Document.Load(File);
    }

    public static string BaseUrl
    {
        get
        {
            if (_baseUrl is null)
            {
                var node = Document.DocumentElement!.SelectSingleNode("BaseUrl");
                _baseUrl = node!.InnerText;
            }
            return _baseUrl;
        }
    }

    public static string DefaultEmail
    {
        get
        {
            if (_defaultEmail is null)
            {
                var node = Document.DocumentElement!.SelectSingleNode("DefaultEmail");
                _defaultEmail = node!.InnerText;
            }
            return _defaultEmail;
        }
    }

    public static string DefaultPassword
    {
        get
        {
            if (_defaultPassword is null)
            {
                var node = Document.DocumentElement!.SelectSingleNode("DefaultPassword");
                _defaultPassword = node!.InnerText;
            }
            return _defaultPassword;
        }
    }

    public static AccountData DefaultAccount => new AccountData(DefaultEmail, DefaultPassword);
}
