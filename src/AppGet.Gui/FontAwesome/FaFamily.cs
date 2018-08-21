using System.Windows.Media;

namespace AppGet.Gui.FontAwesome
{
    public static class FaFamily
    {
        public static readonly FontFamily Regular = (FontFamily)System.Windows.Application.Current.Resources["FontAwesomeRegular"];
        public static readonly FontFamily Solid = (FontFamily)System.Windows.Application.Current.Resources["FontAwesomeSolid"];
    }
}