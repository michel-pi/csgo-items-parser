using System;
using System.IO;
using System.Collections.Generic;

using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Win32;

namespace CsgoItemsParser.Parser
{
    public class ItemsParser
    {
        private string _itemsFileContent;
        private string _translationFileContent;

        public string CounterStrikePath { get; private set; }

        public ItemsParser()
        {
            CounterStrikePath = GetCounterStrikeInstallationPath();

            _itemsFileContent = File.ReadAllText(Path.Combine(CounterStrikePath, "csgo", "scripts", "items", "items_game.txt"));
            _translationFileContent = File.ReadAllText(Path.Combine(CounterStrikePath, "csgo", "resource", "csgo_english.txt"));
        }

        public List<PaintKitTranslation> FindPaintKitTranslations()
        {
            string pattern = @"""([Pp]aint[Kk]it[a-zA-Z0-9_-]+ag)"".*""(.*)""";

            var matches = Regex.Matches(_translationFileContent, pattern);

            if (matches == null) return null;
            if (matches.Count == 0) return null;

            List<PaintKitTranslation> paintKitTranslations = new List<PaintKitTranslation>(matches.Count);

            foreach (Match match in matches)
            {
                paintKitTranslations.Add(new PaintKitTranslation()
                {
                    Tag = match.Groups[1].Value,
                    Translation = match.Groups[2].Value
                });
            }

            return paintKitTranslations;
        }

        public List<EntityQuality> FindEntityQualities()
        {
            string pattern = @"""([\w]+)""[\s]*{[\s]*""value""[\s]*""([\d]+)""[\s]*""weight""";

            var matches = Regex.Matches(_itemsFileContent, pattern);

            if (matches == null) return null;
            if (matches.Count == 0) return null;

            List<EntityQuality> entityQualities = new List<EntityQuality>(matches.Count);

            foreach (Match match in matches)
            {
                entityQualities.Add(new EntityQuality()
                {
                    Name = match.Groups[1].Value,
                    Value = int.Parse(match.Groups[2].Value)
                });
            }

            return entityQualities;
        }

        public List<ItemDefinition> FindItemDefinitions()
        {
            string pattern = @"}[\s]*""([\d]+)""[\s]*{[\s]*""name""[\s]*""([a-zA-Z0-9_]+)""[\s]*""prefab";

            var matches = Regex.Matches(_itemsFileContent, pattern);

            if (matches == null) return null;
            if (matches.Count == 0) return null;

            List<ItemDefinition> itemDefinitions = new List<ItemDefinition>(matches.Count);

            foreach (Match match in matches)
            {
                itemDefinitions.Add(new ItemDefinition()
                {
                    Index = int.Parse(match.Groups[1].Value),
                    Name = match.Groups[2].Value
                });
            }

            return itemDefinitions;
        }

        public List<PaintKit> FindPaintKits()
        {
            string pattern = @"""([\d]*)""[\s]*{[\s]*""name""[\s]*""(.*)""[\s]*""description_[\w]+""[\s]*""#(.*)""[\s]*""description_[\w]+""[\s]*""#(.*)""";

            var matches = Regex.Matches(_itemsFileContent, pattern);

            if (matches == null) return null;
            if (matches.Count == 0) return null;

            List<PaintKit> paintKits = new List<PaintKit>(matches.Count);

            foreach (Match match in matches)
            {
                paintKits.Add(new PaintKit()
                {
                    Index = int.Parse(match.Groups[1].Value),
                    Name = match.Groups[2].Value,
                    Description = match.Groups[3].Value,
                    Tag = match.Groups[4].Value
                });
            }

            return paintKits;
        }

        private static string GetCounterStrikeInstallationPath()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(
                    RegistryHive.LocalMachine,
                    Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);

            if (registryKey == null) throw new UnauthorizedAccessException("Failed to open registry!");

            var subKey = registryKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 730");

            if (subKey == null) throw new UnauthorizedAccessException("Registry key not found!");

            var value = subKey.GetValue("InstallLocation", null, RegistryValueOptions.None);

            if (value == null) throw new UnauthorizedAccessException("Registry value not found!");

            subKey.Dispose();
            registryKey.Dispose();

            return value.ToString();
        }
    }
}
