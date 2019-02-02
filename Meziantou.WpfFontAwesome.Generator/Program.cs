using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Meziantou.Framework.CodeDom;
using Newtonsoft.Json;

namespace Meziantou.WpfFontAwesome.Generator
{
    class Program
    {
        static void Main()
        {
            var freeIconFile = Path.GetFullPath(Path.Combine("../../../", "Free", "metadata", "icons.json"));
            var proIconFile = Path.GetFullPath(Path.Combine("../../../", "Pro", "metadata", "icons.json"));
            if (!File.Exists(freeIconFile))
            {
                Console.Error.WriteLine($"File not found: {freeIconFile}");
                return;
            }

            if (!File.Exists(proIconFile))
            {
                Console.Error.WriteLine($"File not found: {proIconFile}");
                return;
            }

            var freeIcons = JsonConvert.DeserializeObject<IDictionary<string, Icon>>(File.ReadAllText(freeIconFile));
            var proIcons = JsonConvert.DeserializeObject<IDictionary<string, Icon>>(File.ReadAllText(proIconFile));
            var allStyles = proIcons.Values.SelectMany(icon => icon.Styles).Distinct().ToList();

            var unit = new CompilationUnit();
            var ns = unit.AddNamespace("Meziantou.WpfFontAwesome");

            {
                var enumeration = ns.AddType(new EnumerationDeclaration($"FontAwesomeIcons") { Modifiers = Modifiers.Public });
                enumeration.BaseType = typeof(ushort);

                foreach (var (key, value) in proIcons)
                {
                    var identifier = ToCSharpIdentifier(PascalName(key));
                    var member = new EnumerationMember(identifier, value.UnicodeIntValue);
                    enumeration.Members.Add(member);

                    member.XmlComments.Add(new XElement("summary", $"{value.Label} ({value.Unicode})"));
                }
            }

            foreach (var style in allStyles)
            {
                var enumeration = ns.AddType(new EnumerationDeclaration($"FontAwesome{PascalName(style)}Icon") { Modifiers = Modifiers.Public });
                enumeration.BaseType = typeof(ushort);

                foreach (var (key, value) in proIcons)
                {
                    if (!value.Styles.Contains(style))
                        continue;

                    var identifier = ToCSharpIdentifier(PascalName(key));
                    var member = new EnumerationMember(identifier, value.UnicodeIntValue);
                    enumeration.Members.Add(member);

                    member.XmlComments.Add(new XElement("summary", $"{value.Label} ({value.Unicode})"));

                    if (!freeIcons.TryGetValue(key, out var freeIcon) || !freeIcon.Styles.Contains(style))
                    {
                        member.CustomAttributes.Add(new CustomAttribute(new TypeReference("Meziantou.WpfFontAwesome.ProIconAttribute")));
                    }
                }
            }

            var codeGenerator = new CSharpCodeGenerator();
            File.WriteAllText("../../../../Meziantou.WpfFontAwesome/FontAwesomeIcons.cs", codeGenerator.Write(unit));

            // Copy free fonts
            File.Copy("../../../Free/otfs/Font Awesome 5 Brands-Regular-400.otf", "../../../../Meziantou.WpfFontAwesome/Resources/brands/Font Awesome 5 Brands-Regular-400.otf", overwrite: true);
            File.Copy("../../../Free/otfs/Font Awesome 5 Free-Regular-400.otf", "../../../../Meziantou.WpfFontAwesome/Resources/regular/Font Awesome 5 Free-Regular-400.otf", overwrite: true);
            File.Copy("../../../Free/otfs/Font Awesome 5 Free-Solid-900.otf", "../../../../Meziantou.WpfFontAwesome/Resources/solid/Font Awesome 5 Free-Solid-900.otf", overwrite: true);

        }

        private static string PascalName(string name)
        {
            var sb = new StringBuilder();
            bool upperCase = true;
            foreach (char c in name)
            {
                if (c == '-')
                {
                    upperCase = true;
                    continue;
                }

                if (upperCase)
                {
                    sb.Append(char.ToUpper(c));
                }
                else
                {
                    sb.Append(c);
                }

                upperCase = false;
            }

            return sb.ToString();
        }

        private static string ToCSharpIdentifier(string name)
        {
            char c = name[0];
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_')
            {
                return name;
            }

            return "Icon" + name;
        }
    }

    internal class Icon
    {
        public string[] Styles { get; set; }
        public string Unicode { get; set; }
        public int UnicodeIntValue => int.Parse(Unicode, System.Globalization.NumberStyles.HexNumber);
        public string Label { get; set; }
    }
}
