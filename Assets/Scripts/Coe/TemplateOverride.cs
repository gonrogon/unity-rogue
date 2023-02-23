using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rogue.Coe
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TemplateOverride
    {
        None, Replace, Remove
    }
}
