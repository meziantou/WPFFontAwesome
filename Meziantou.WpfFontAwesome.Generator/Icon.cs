using System.Globalization;

namespace Meziantou.WpfFontAwesome.Generator
{
    internal class Icon
    {
        public string[] Styles { get; set; }
        public string Unicode { get; set; }
        public int UnicodeIntValue => int.Parse(Unicode, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        public string Label { get; set; }
    }
}
