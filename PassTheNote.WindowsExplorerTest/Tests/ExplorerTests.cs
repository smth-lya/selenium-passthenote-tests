using System;
using System.IO;
using System.Threading;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA3;
using NUnit.Framework;
using PassTheNote.WindowsExplorerTest.Helpers;

namespace PassTheNote.WindowsExplorerTest.Tests
{
    [TestFixture]
    public class ExplorerTests
    {
        [Test]
        public void CreateAndDeleteFolderTest()
        {
            var folderName = "TestFolder";
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), folderName);

            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);

            Keyboard.Press(VirtualKeyShort.LWIN);
            Keyboard.Press(VirtualKeyShort.KEY_E);
            Keyboard.Release(VirtualKeyShort.KEY_E);
            Keyboard.Release(VirtualKeyShort.LWIN);
            Thread.Sleep(1500);

            using var automation = new UIA3Automation();
            
            var window = ExplorerHelper.FindExplorerWindow(automation);
            if (window == null)
            {
                Console.WriteLine("Не найдено окно Проводника");
                return;
            }
            window.Focus();
            
            Keyboard.Press(VirtualKeyShort.CONTROL);
            Keyboard.Press(VirtualKeyShort.KEY_L);
            Keyboard.Release(VirtualKeyShort.KEY_L);
            Keyboard.Release(VirtualKeyShort.CONTROL);
            Thread.Sleep(200);
            Keyboard.Type(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            Keyboard.Press(VirtualKeyShort.RETURN);
            Keyboard.Release(VirtualKeyShort.RETURN);
            Thread.Sleep(700);

            Keyboard.Press(VirtualKeyShort.CONTROL);
            Keyboard.Press(VirtualKeyShort.SHIFT);
            Keyboard.Press(VirtualKeyShort.KEY_N);
            Keyboard.Release(VirtualKeyShort.KEY_N);
            Keyboard.Release(VirtualKeyShort.SHIFT);
            Keyboard.Release(VirtualKeyShort.CONTROL);
            Thread.Sleep(500);
            Keyboard.Type(folderName);
            Keyboard.Press(VirtualKeyShort.RETURN);
            Keyboard.Release(VirtualKeyShort.RETURN);
            Thread.Sleep(700);

            if (Directory.Exists(folderPath))
            {
                Console.WriteLine($"Папка {folderName} успешно создана.");
                Directory.Delete(folderPath, true);
                Console.WriteLine($"Папка {folderName} удалена.");
            }
            else
            {
                Console.WriteLine($"Папка {folderName} не создана");
            }
            window.Close();
        }
    }
}
