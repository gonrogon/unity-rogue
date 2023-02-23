using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rogue.Core.Serialization
{
    public static class Serializer
    {
        public static void Save<T>(string file, T obj)
        {
            var serializer = new JsonSerializer();

            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            //serializer.TypeNameHandling = TypeNameHandling.Objects;
            serializer.Formatting = Formatting.Indented;

            using (var stream = new StreamWriter(file))
            {
                using (var writer = new JsonTextWriter(stream))
                {
                    serializer.Serialize(writer, obj);
                }
            }
        }

        public static T Load<T>(string file)
        {
            var serializer = new JsonSerializer();

            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            //serializer.TypeNameHandling = TypeNameHandling.Objects;

            using (var stream = new StreamReader(file))
            {
                using (var reader = new JsonTextReader(stream))
                {
                    //serializer.Serialize(reader, obj);
                    return serializer.Deserialize<T>(reader);
                }
            }
        }
        /*
        public static void Save<T>(string file, T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream     = new StreamWriter(file);
            
            serializer.Serialize(stream, obj);
            stream.Close();
        }

        public static T Load<T>(string file)
        {
            var serializer = new XmlSerializer(typeof(T));
            var stream     = new FileStream(file, FileMode.Open);

            return (T)serializer.Deserialize(stream);
        }
        */
    }
/*
    public interface LoadSerializer : Serializer
    {
        bool TryGetInt(string name, out int value, int def = 0);

        bool TryGetFloat(string name, out float value, float def = 0.0f);

        bool TryGetString(string name, out string value, string def = "");
    }

    public interface SaveSerializer : Serializer
    {
        void SetInt(string name, int value);

        void SetFloat(string name, float value);

        void SetString(string name, float value);
    }
    */
}
