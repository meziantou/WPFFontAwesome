using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace Meziantou.WpfFontAwesome.Demo
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadProIcons();
            AddProIcons();
        }

        private static void LoadProIcons()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "*.otf|*.otf",
                Multiselect = true,
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (var file in dialog.FileNames)
                {
                    if (!File.Exists(file))
                        continue;

                    var fonts = Fonts.GetFontFamilies(file);
                    if (fonts == null || fonts.Count == 0)
                        continue;

                    var fileName = Path.GetFileName(file);
                    if (string.Equals(fileName, "Font Awesome 5 Brands-Regular-400.otf", System.StringComparison.OrdinalIgnoreCase))
                    {
                        FontAwesomeIcon.ProBrandsFontFamily = fonts.First();
                    }
                    else if (string.Equals(fileName, "Font Awesome 5 Duotone-Solid-900.otf", System.StringComparison.OrdinalIgnoreCase))
                    {
                        FontAwesomeIcon.ProDuotoneFontFamily = fonts.First();
                    }
                    else if (string.Equals(fileName, "Font Awesome 5 Pro-Light-300.otf", System.StringComparison.OrdinalIgnoreCase))
                    {
                        FontAwesomeIcon.ProLightFontFamily = fonts.First();
                    }
                    else if (string.Equals(fileName, "Font Awesome 5 Pro-Regular-400.otf", System.StringComparison.OrdinalIgnoreCase))
                    {
                        FontAwesomeIcon.ProRegularFontFamily = fonts.First();
                    }
                    else if (string.Equals(fileName, "Font Awesome 5 Pro-Solid-900.otf", System.StringComparison.OrdinalIgnoreCase))
                    {
                        FontAwesomeIcon.ProSolidFontFamily = fonts.First();
                    }
                }
            }
        }

        private void AddProIcons()
        {
            ProIcons.Children.Clear();
            if (FontAwesomeIcon.ProBrandsFontFamily != null)
            {
                var control = new FontAwesomeIcon()
                {
                    BrandIcon = FontAwesomeBrandsIcon.Icon500px,
                    FontSize = 60,
                };

                ProIcons.Children.Add(control);
            }

            if (FontAwesomeIcon.ProDuotoneFontFamily != null)
            {
                var control = new FontAwesomeIcon()
                {
                    DuotoneIcon = FontAwesomeDuotoneIcon.Abacus,
                    FontSize = 60,
                };

                ProIcons.Children.Add(control);
            }

            if (FontAwesomeIcon.ProLightFontFamily != null)
            {
                var control = new FontAwesomeIcon()
                {
                    LightIcon = FontAwesomeLightIcon.Abacus,
                    FontSize = 60,
                };

                ProIcons.Children.Add(control);
            }

            if (FontAwesomeIcon.ProRegularFontFamily != null)
            {
                var control = new FontAwesomeIcon()
                {
                    RegularIcon = FontAwesomeRegularIcon.Abacus,
                    FontSize = 60,
                };

                ProIcons.Children.Add(control);
            }

            if (FontAwesomeIcon.ProSolidFontFamily != null)
            {
                var control = new FontAwesomeIcon()
                {
                    SolidIcon = FontAwesomeSolidIcon.Abacus,
                    FontSize = 60,
                };

                ProIcons.Children.Add(control);
            }
        }
    }
}
