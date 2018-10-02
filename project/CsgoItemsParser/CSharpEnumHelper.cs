using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using CsgoItemsParser.Parser;

namespace CsgoItemsParser
{
    public static class CSharpEnumHelper
    {
        private static CultureInfo _cultureInfo = new CultureInfo("en-US");

        public static string CreateEntityQualityEnum(List<EntityQuality> entityQualities)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("public enum EntityQuality");
            sb.AppendLine("{");

            foreach (var entityQuality in entityQualities)
                sb.AppendLine("\t" + BetterNaming(entityQuality.Name) + " = " + entityQuality.Value.ToString() + ",");

            sb.AppendLine("}");

            return sb.ToString();
        }

        public static string CreateItemDefinitionIndexEnum(List<ItemDefinition> itemDefinitions)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("public enum ItemDefinitionIndex : short");
            sb.AppendLine("{");

            foreach (var itemDefinition in itemDefinitions)
                sb.AppendLine("\t" + BetterNaming(itemDefinition.Name) + " = " + itemDefinition.Index.ToString() + ",");

            sb.AppendLine("}");

            return sb.ToString();
        }

        public static string CreatePaintKitEnum(List<PaintKit> paintKits, List<PaintKitTranslation> paintKitTranslations)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("public enum PaintKit");
            sb.AppendLine("{");

            foreach (var paintKit in paintKits)
            {
                bool hasTranslation = false;

                foreach (var translation in paintKitTranslations)
                {
                    if (Compare(translation.Tag, paintKit.Tag))
                    {
                        hasTranslation = true;
                        sb.AppendLine("\t" + BetterNaming(translation.Translation) + " = " + paintKit.Index.ToString());
                        break;
                    }
                }

                if (!hasTranslation)
                {
                    sb.AppendLine("\t" + BetterNaming(paintKit.Name) + " = " + paintKit.Index.ToString());
                }
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private static bool Compare(string left, string right)
        {
            if (left == null || right == null) return false;
            if (left.Length != right.Length) return false;

            return left.ToLower() == right.ToLower();
        }

        private static string BetterNaming(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            if (name == "-") return "Default";

            name = name.Replace("_", " ");

            name = _cultureInfo.TextInfo.ToTitleCase(name);

            name = name.Replace(" ", "").Replace("-", "").Replace("'", "");
            
            return name;
        }
    }
}
