using System.Globalization;

namespace Meziantou.WpfFontAwesome.Generator
{
    internal sealed class Icon
    {
        public string[] Styles { get; set; }
        public string Unicode { get; set; }
        public int UnicodeIntValue => int.Parse(Unicode, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        public string Label { get; set; }
    }
}
