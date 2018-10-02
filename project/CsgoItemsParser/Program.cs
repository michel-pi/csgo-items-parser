using System;
using System.IO;

using CsgoItemsParser.Parser;

namespace CsgoItemsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            ItemsParser parser = new ItemsParser();

            EntityQuality(parser);
            ItemDefinitionIndex(parser);
            PaintKit(parser);

            Console.WriteLine("Done!");

            Console.ReadLine();
        }

        private static void EntityQuality(ItemsParser parser)
        {
            var entityQualities = parser.FindEntityQualities();

            var csharpEnum = CSharpEnumHelper.CreateEntityQualityEnum(entityQualities);
            var cppEnum = CppEnumHelper.CreateEntityQualityEnum(entityQualities);

            File.WriteAllText("EntityQuality.cs", csharpEnum);
            File.WriteAllText("EntityQuality.h", cppEnum);
        }

        private static void ItemDefinitionIndex(ItemsParser parser)
        {
            var itemDefinitions = parser.FindItemDefinitions();

            var csharpEnum = CSharpEnumHelper.CreateItemDefinitionIndexEnum(itemDefinitions);
            var cppEnum = CppEnumHelper.CreateItemDefinitionIndexEnum(itemDefinitions);

            File.WriteAllText("ItemDefinitionIndex.cs", csharpEnum);
            File.WriteAllText("ItemDefinitionIndex.h", cppEnum);
        }

        private static void PaintKit(ItemsParser parser)
        {
            var paintKits = parser.FindPaintKits();
            var paintKitTranslations = parser.FindPaintKitTranslations();

            var csharpEnum = CSharpEnumHelper.CreatePaintKitEnum(paintKits, paintKitTranslations);
            var cppEnum = CppEnumHelper.CreatePaintKitEnum(paintKits, paintKitTranslations);

            File.WriteAllText("PaintKit.cs", csharpEnum);
            File.WriteAllText("PaintKit.h", cppEnum);
        }
    }
}
