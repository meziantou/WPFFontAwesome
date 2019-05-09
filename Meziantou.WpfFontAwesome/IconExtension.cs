using System.Windows.Markup;

namespace Meziantou.WpfFontAwesome
{
    [MarkupExtensionReturnType(typeof(string))]
    public sealed partial class IconExtension : MarkupExtension
    {
        public IconExtension()
        {
        }

        public IconExtension(FontAwesomeIcons icon)
        {
            Icon = icon;
        }

        [ConstructorArgument("icon")]
        public FontAwesomeIcons Icon { get; set; }

        public override object ProvideValue(System.IServiceProvider serviceProvider)
        {
            return ((char)Icon).ToString();
        }
    }
}
