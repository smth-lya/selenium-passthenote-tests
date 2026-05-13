using System.Xml;
using PassTheNote.Tests.Models;

namespace PassTheNote.Tests.Settings;

/// <summary>
/// Статический класс настроек. Читает конфигурацию из Settings.xml,
/// который копируется в выходную папку при сборке.
/// </summary>
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

    /// <summary>Возвращает AccountData с дефолтными данными из Settings.xml.</summary>
    public static AccountData DefaultAccount => new AccountData(DefaultEmail, DefaultPassword);
}
