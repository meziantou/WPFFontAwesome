using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Meziantou.Framework.CodeDom;
using Meziantou.Framework.Versioning;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Meziantou.WpfFontAwesome.Generator
{
    internal static class Program
    {
        private const string ProjectName = "Meziantou.WpfFontAwesome";

        private static void Main()
        {
            var files = GetPaths();
            if (files == default)
                return;

            var freeIconFile = GetIconsFileContent(files.free);
            var proIconFile = GetIconsFileContent(files.pro);

            GenerateCode(freeIconFile, proIconFile, files.version);
            CopyFonts(files.free);
            UpdateCsprojVersion(files.version);
        }

        private static string FindSourceDirectory()
        {
            var path = Environment.CurrentDirectory;
            while (true)
            {
                if (Directory.EnumerateDirectories(path).Any(d => string.Equals(Path.GetFileName(d), ProjectName, StringComparison.OrdinalIgnoreCase)))
                    return Path.Join(path, ProjectName);

                path = Path.GetDirectoryName(path);
            }
        }

        private static void UpdateCsprojVersion(SemanticVersion version)
        {
            var path = FindSourceDirectory() + "/Meziantou.WpfFontAwesome.csproj";
            var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
            document.Descendants("Version").Single().Value = version.ToString();
            document.Save(path);
        }

        private static string GetDownloadFolderPath()
        {
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", string.Empty).ToString();
        }

        private static (string free, string pro, SemanticVersion version) GetPaths()
        {
            var downloadPath = GetDownloadFolderPath();
            var files = Directory.GetFiles(downloadPath);

            (string path, SemanticVersion version) free = default;
            (string path, SemanticVersion version) pro = default;
            foreach (var file in files)
            {
                var match = Regex.Match(file, "fontawesome-(?<type>free|pro)-(?<version>[0-9]+.[0-9]+.[0-9]+)-desktop.zip", RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(1));
                if (!match.Success)
                    continue;

                if (!SemanticVersion.TryParse(match.Groups["version"].Value, out var version))
                    continue;

                var isFree = string.Equals(match.Groups["type"].Value, "free", StringComparison.OrdinalIgnoreCase);

                if (isFree && version > free.version)
                {
                    free = (file, version);
                }
                else if (!isFree && version > pro.version)
                {
                    pro = (file, version);
                }
            }

            if (free == default || pro == default)
            {
                Console.WriteLine("FontAwesome files not found");
                return default;
            }

            if (free.version != pro.version)
            {
                Console.WriteLine("Free and Pro are not on the same version");
                return default;
            }

            return (free.path, pro.path, free.version);
        }

        private static string GetIconsFileContent(string path)
        {
            using var zipFile = ZipFile.OpenRead(path);
            var entry = zipFile.Entries.First(zipEntry => zipEntry.FullName.EndsWith("metadata/icons.json", StringComparison.Ordinal));
            using var stream = entry.Open();
            using var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

        private static void CopyFonts(string path)
        {
            var files = new[]
            {
                ("brands", "Font Awesome 5 Brands-Regular-400.otf"),
                ("regular", "Font Awesome 5 Free-Regular-400.otf"),
                ("solid","Font Awesome 5 Free-Solid-900.otf"),
            };

            using var zipFile = ZipFile.OpenRead(path);
            foreach (var file in files)
            {
                var entry = zipFile.Entries.First(zipEntry => zipEntry.FullName.EndsWith(file.Item2, StringComparison.Ordinal));
                using var stream = entry.Open();
                using var fileStream = File.OpenWrite($"{FindSourceDirectory()}/Resources/{file.Item1}/{entry.Name}");
                stream.CopyTo(fileStream);
            }
        }

        private static void GenerateCode(string freeIconFile, string proIconFile, SemanticVersion version)
        {
            var freeIcons = JsonConvert.DeserializeObject<IDictionary<string, Icon>>(freeIconFile);
            var proIcons = JsonConvert.DeserializeObject<IDictionary<string, Icon>>(proIconFile);
            var allStyles = proIcons.Values.SelectMany(icon => icon.Styles).Distinct(StringComparer.Ordinal).ToList();

            var unit = new CompilationUnit();
            var ns = unit.AddNamespace("Meziantou.WpfFontAwesome");

            ns.CommentsBefore.Add(@"------------------------------------------------------------------------------
 <auto-generated>
     This code was generated by a tool.

     Changes to this file may cause incorrect behavior and will be lost if
     the code is regenerated.
 </auto-generated>
------------------------------------------------------------------------------");

            {
                var enumeration = ns.AddType(new EnumerationDeclaration($"FontAwesomeIcons") { Modifiers = Modifiers.Public });
                enumeration.BaseType = typeof(ushort);
                enumeration.XmlComments.AddSummary($"Icons of FontAwesome {version}");

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
                enumeration.XmlComments.AddSummary($"Icons of FontAwesome {style} {version}");

                foreach (var (key, value) in proIcons)
                {
                    if (!value.Styles.Contains(style, StringComparer.Ordinal))
                        continue;

                    var identifier = ToCSharpIdentifier(PascalName(key));
                    var member = new EnumerationMember(identifier, value.UnicodeIntValue);
                    enumeration.Members.Add(member);

                    member.XmlComments.Add(new XElement("summary", $"{value.Label} ({value.Unicode})"));

                    if (!freeIcons.TryGetValue(key, out var freeIcon) || !freeIcon.Styles.Contains(style, StringComparer.Ordinal))
                    {
                        member.CustomAttributes.Add(new CustomAttribute(new TypeReference("Meziantou.WpfFontAwesome.ProIconAttribute")));
                    }
                }
            }

            var codeGenerator = new CSharpCodeGenerator();
            File.WriteAllText(FindSourceDirectory() + "/FontAwesomeIcons.cs", codeGenerator.Write(unit));
        }

        private static string PascalName(string name)
        {
            var sb = new StringBuilder();
            var upperCase = true;
            foreach (var c in name)
            {
                if (c == '-')
                {
                    upperCase = true;
                    continue;
                }

                if (upperCase)
                {
                    sb.Append(char.ToUpperInvariant(c));
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
            var c = name[0];
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_')
            {
                return name;
            }

            return "Icon" + name;
        }
    }
}
