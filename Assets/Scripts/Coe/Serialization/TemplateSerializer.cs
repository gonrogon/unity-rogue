using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace Rogue.Coe.Serialization
{
    public static class TemplateSerializer
    {
        public static TemplateDatabase LoadDatabaseFromFile(string file, TemplateDatabase database)
        {
            using StreamReader stream = new (file);

            return LoadDatabase(stream, database);
        }

        public static TemplateDatabase LoadDatabaseFromText(string text, TemplateDatabase database)
        {
            using StringReader reader = new (text);

            return LoadDatabase(reader, database);
        }

        public static TemplateDatabase LoadDatabase(TextReader stream, TemplateDatabase database)
        {
                  JsonSerializer serializer = CreateSerializer(database);
            using JsonTextReader reader     = new (stream);

            return serializer.Deserialize<TemplateDatabase>(reader);
        }

        public static Template LoadTemplate(string file, TemplateDatabase database)
        {
            using StreamReader stream = new (file);

            return LoadTemplate(stream, database);
        }

        public static Template LoadTemplate(TextReader stream, TemplateDatabase database)
        {
                  JsonSerializer serializer = CreateSerializer(database);
            using JsonTextReader reader     = new (stream);

            return serializer.Deserialize<Template>(reader);
        }

        public static JsonSerializer CreateSerializer(TemplateDatabase database)
        {
            JsonSerializer serializer = new()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting        = Formatting.Indented
            };

            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.Converters.Add(new Core.Serialization.MathConverter());
            serializer.Converters.Add(new TemplateDatabaseConverter(database));
            serializer.Converters.Add(new TemplateConverter(database));
            serializer.Converters.Add(new GameComponentConverter());
            serializer.Converters.Add(new GameBehaviourConverter());

            return serializer;
        }
    }
}
