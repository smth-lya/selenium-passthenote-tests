using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace PassTheNote.WindowsExplorerTest.Helpers
{
    public static class ExplorerHelper
    {
        public static Window FindExplorerWindow(UIA3Automation automation)
        {
            foreach (var win in automation.GetDesktop().FindAllChildren(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Window)))
            {
                var window = win.AsWindow();
                if (window != null && (window.Title.Contains("Проводник") || window.Title.Contains("Explorer") || window.Title.Contains("Этот компьютер") || window.Title.Contains("This PC")))
                    return window;
            }
            return null;
        }
    }
}
