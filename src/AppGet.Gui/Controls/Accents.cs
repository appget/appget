using System.Globalization;
using System.Windows.Media;

namespace AppGet.Gui.Controls
{
    public static class Accents
    {
        private static Color FromHex(string hex)
        {
            return (Color)ColorConverter.ConvertFromString(hex);
        }


        public static Color Success = FromHex("#64dd17");
        public static Color Warn = FromHex("#FFC107");
        public static Color Error = FromHex("#C62828");
    }
}
