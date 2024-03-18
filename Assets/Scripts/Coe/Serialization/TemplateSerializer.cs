using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace Rogue.Coe.Serialization
{
    public class TemplateSerializer
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

        public static void SaveDatabaseToFile(string file, TemplateDatabase database)
        {
            using StreamWriter stream = new (file);
            SaveDatabase(stream, database);
        }

        public static void SaveDatabase(TextWriter stream, TemplateDatabase database)
        {
                  JsonSerializer serializer = CreateSerializer(database);
            using JsonTextWriter writer     = new (stream);

            serializer.Serialize(writer, database);
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

        public static void SaveTemplate(string file, TemplateDatabase database, Template template)
        {
            using StreamWriter stream = new(file);

            SaveTemplate(stream, database, template);
        }

        public static void SaveTemplate(TextWriter stream, TemplateDatabase database, Template template)
        {
                  JsonSerializer serializer = CreateSerializer(database);
            using JsonTextWriter writer     = new (stream);

            serializer.Serialize(writer, template);
        }

        public static JsonSerializer CreateSerializer(TemplateDatabase database)
        {
            JsonSerializer serializer = new ();

            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Formatting        = Formatting.Indented;
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
