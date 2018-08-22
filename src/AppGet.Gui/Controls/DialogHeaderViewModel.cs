using System.Windows.Media;
using AppGet.Gui.FontAwesome;
using Caliburn.Micro;

namespace AppGet.Gui.Controls
{
    public class DialogHeaderViewModel : Screen
    {
        public DialogHeaderViewModel(string title, string message, string icon, Color iconBrush)
        {
            char unicode;
            if (FaIcons.Regular.ContainsKey(icon))
            {
                unicode = FaIcons.Regular[icon];
                IconFamily = FaFamily.Regular;
            }
            else
            {
                unicode = FaIcons.Solid[icon];
                IconFamily = FaFamily.Solid;
            }

            Title = title;
            Message = message;
            Icon = unicode.ToString();

            IconColor = new SolidColorBrush(iconBrush);
            IconColor.Freeze();
        }

        public string Title { get; }
        public string Message { get; }
        public string Icon { get; }
        public Brush IconColor { get; }
        public FontFamily IconFamily { get; set; }
    }
}