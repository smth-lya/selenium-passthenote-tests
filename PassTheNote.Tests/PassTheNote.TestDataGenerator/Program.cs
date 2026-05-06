using System.Text;
using System.Xml.Serialization;
using PassTheNote.TestDataGenerator;

if (args.Length < 4)
{
    Console.WriteLine("Использование: PassTheNote.TestDataGenerator.exe <тип> <количество> <имя_файла> <формат>");
    Console.WriteLine("Пример:        PassTheNote.TestDataGenerator.exe g 3 accounts.xml xml");
    return;
}

var dataType = args[0];

if (!int.TryParse(args[1], out int count) || count <= 0)
{
    Console.WriteLine("Ошибка: количество должно быть положительным целым числом.");
    return;
}

var fileName = args[2];
var format = args[3].ToLower();

if (dataType == "g" && format == "xml")
{
    var accounts = GenerateAccounts(count);
    SaveToXml(accounts, fileName);
    Console.WriteLine($"Готово: сгенерировано {count} запис(ей) → {fileName}");
}
else
{
    Console.WriteLine($"Неизвестный тип данных '{dataType}' или формат '{format}'.");
    Console.WriteLine("Поддерживается: тип 'g', формат 'xml'");
}

return;

static List<AccountData> GenerateAccounts(int count)
{
    var random = new Random();
    const string letters = "abcdefghijklmnopqrstuvwxyz";
    var list = new List<AccountData>(count);

    for (int i = 0; i < count; i++)
    {
        string prefix = new string(
            Enumerable.Range(0, 6)
                      .Select(_ => letters[random.Next(letters.Length)])
                      .ToArray());
        string suffix = random.Next(10000, 99999).ToString();

        list.Add(new AccountData
        {
            Email = $"{prefix}{suffix}@passthenote.com",
            Password = $"Test{suffix}@Pass"
        });
    }

    return list;
}

static void SaveToXml(List<AccountData> data, string fileName)
{
    var serializer = new XmlSerializer(typeof(List<AccountData>));
    using var writer = new StreamWriter(fileName, append: false, encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    serializer.Serialize(writer, data);
}
