using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Meziantou.WpfFontAwesome
{
    public class FontAwesomeIcon : Control
    {
        static FontAwesomeIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FontAwesomeIcon), new FrameworkPropertyMetadata(typeof(FontAwesomeIcon)));

            FreeBrandsFontFamily = Fonts.GetFontFamilies(new Uri("pack://application:,,,/Meziantou.WpfFontAwesome;component/Resources/"), "./brands/").First();
            FreeRegularFontFamily = Fonts.GetFontFamilies(new Uri("pack://application:,,,/Meziantou.WpfFontAwesome;component/Resources/"), "./regular/").First();
            FreeSolidFontFamily = Fonts.GetFontFamilies(new Uri("pack://application:,,,/Meziantou.WpfFontAwesome;component/Resources/"), "./solid/").First();
        }

        public static readonly DependencyProperty BrandIconProperty = DependencyProperty.Register("BrandIcon", typeof(FontAwesomeBrandsIcon?), typeof(FontAwesomeIcon), new FrameworkPropertyMetadata(null, OnValueChanged));
        public static readonly DependencyProperty SolidIconProperty = DependencyProperty.Register("SolidIcon", typeof(FontAwesomeSolidIcon?), typeof(FontAwesomeIcon), new FrameworkPropertyMetadata(null, OnValueChanged));
        public static readonly DependencyProperty RegularIconProperty = DependencyProperty.Register("RegularIcon", typeof(FontAwesomeRegularIcon?), typeof(FontAwesomeIcon), new FrameworkPropertyMetadata(null, OnValueChanged));
        public static readonly DependencyProperty LightIconProperty = DependencyProperty.Register("LightIcon", typeof(FontAwesomeLightIcon?), typeof(FontAwesomeIcon), new FrameworkPropertyMetadata(null, OnValueChanged));
        public static readonly DependencyProperty AnimationTypeProperty = DependencyProperty.Register("AnimationType", typeof(AnimationType), typeof(FontAwesomeIcon), new FrameworkPropertyMetadata(AnimationType.None));

        public static readonly DependencyPropertyKey IconCharacterProperty = DependencyProperty.RegisterReadOnly("IconCharacter", typeof(string), typeof(FontAwesomeIcon), new PropertyMetadata());
        public static readonly DependencyPropertyKey FinalFontFamilyProperty = DependencyProperty.RegisterReadOnly("FinalFontFamily", typeof(FontFamily), typeof(FontAwesomeIcon), new PropertyMetadata());

        public static FontFamily FreeBrandsFontFamily { get; }
        public static FontFamily FreeRegularFontFamily { get; }
        public static FontFamily FreeSolidFontFamily { get; }

        public static FontFamily ProBrandsFontFamily { get; set; }
        public static FontFamily ProSolidFontFamily { get; set; }
        public static FontFamily ProRegularFontFamily { get; set; }
        public static FontFamily ProLightFontFamily { get; set; }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs baseValue)
        {
            var obj = (FontAwesomeIcon)d;

            if (obj.BrandIcon.HasValue)
            {
                var enumValue = obj.BrandIcon.Value;
                obj.SetValue(IconCharacterProperty, ((char)enumValue).ToString());
                obj.SetValue(FinalFontFamilyProperty, GetFontFamily(enumValue, ProBrandsFontFamily, FreeBrandsFontFamily));
            }
            else if (obj.SolidIcon.HasValue)
            {
                var enumValue = obj.SolidIcon.Value;
                obj.SetValue(IconCharacterProperty, ((char)enumValue).ToString());
                obj.SetValue(FinalFontFamilyProperty, GetFontFamily(enumValue, ProSolidFontFamily, FreeSolidFontFamily));
            }
            else if (obj.RegularIcon.HasValue)
            {
                var enumValue = obj.RegularIcon.Value;
                obj.SetValue(IconCharacterProperty, ((char)enumValue).ToString());
                obj.SetValue(FinalFontFamilyProperty, GetFontFamily(enumValue, ProRegularFontFamily, FreeRegularFontFamily));
            }
            else if (obj.LightIcon.HasValue)
            {
                var enumValue = obj.LightIcon.Value;
                obj.SetValue(IconCharacterProperty, ((char)enumValue).ToString());
                obj.SetValue(FinalFontFamilyProperty, GetFontFamily(enumValue, ProLightFontFamily, null));
            }
            else
            {
                obj.SetValue(IconCharacterProperty, null);
                obj.SetValue(FinalFontFamilyProperty, null);
            }
        }

        private static FontFamily GetFontFamily<T>(T value, FontFamily proFontFamily, FontFamily freeFontFamily) where T : Enum
        {
            if (proFontFamily != null)
                return proFontFamily;

            // Check for free icon
            if (freeFontFamily == null || GetAttribute<ProIconAttribute, T>(value) != null)
                throw new NotSupportedException($"Icon '{value}' is only available with the pro version of FontAwesome");

            return freeFontFamily;
        }

        public static TAttribute GetAttribute<TAttribute, TValue>(TValue value)
            where TAttribute : Attribute
            where TValue : Enum
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            return memberInfo[0].GetCustomAttribute<TAttribute>(inherit: false);
        }

        public FontAwesomeBrandsIcon? BrandIcon
        {
            get { return (FontAwesomeBrandsIcon?)GetValue(BrandIconProperty); }
            set { SetValue(BrandIconProperty, value); }
        }

        public FontAwesomeSolidIcon? SolidIcon
        {
            get { return (FontAwesomeSolidIcon?)GetValue(SolidIconProperty); }
            set { SetValue(SolidIconProperty, value); }
        }

        public FontAwesomeRegularIcon? RegularIcon
        {
            get { return (FontAwesomeRegularIcon?)GetValue(RegularIconProperty); }
            set { SetValue(RegularIconProperty, value); }
        }

        public FontAwesomeLightIcon? LightIcon
        {
            get { return (FontAwesomeLightIcon?)GetValue(LightIconProperty); }
            set { SetValue(LightIconProperty, value); }
        }

        public AnimationType AnimationType
        {
            get { return (AnimationType)GetValue(AnimationTypeProperty); }
            set { SetValue(AnimationTypeProperty, value); }
        }
    }
}
