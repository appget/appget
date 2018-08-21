using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace AppGet.Gui.Controls
{
    [UsedImplicitly]
    public class DialogHeaderViewModel : Screen
    {
        public DialogHeaderViewModel(string title, string message, string icon, Color iconBrush)
        {
            var unicode = int.Parse(icon, System.Globalization.NumberStyles.HexNumber);
            Title = title;
            Message = message;
            Icon = ((char)unicode).ToString();

            IconColor = new SolidColorBrush(iconBrush);
            IconColor.Freeze();
        }

        public string Title { get; }
        public string Message { get; }
        public string Icon { get; }
        public Brush IconColor { get; }
    }
}