using Newtonsoft.Json;
using Rogue.Core.Serialization;

namespace Rogue.Coe.Serialization
{
    internal class TemplateUtil
    {
        private const char CharOverrideAdd = '+';

        private const char CharOverrideRemove = '-';

        private const char CharOverrideReplace = '%';

        private const char CharFlyweight = '_';

        private const TemplateOverride DefaultOverride = TemplateOverride.Replace;

        private static TemplateComponentListConverter _tplCompListCvt = new ();

        private static TemplateBehaviourListConverter _tplBehaListCvt = new ();

        private static TemplateComponentConverter _tplCompCvt = new ();

        private static TemplateBehaviourConverter _tplBehaCvt = new ();

        public static void PushConverters(JsonSerializer serializer, TemplateDatabase database, Template template)
        {
            _tplCompListCvt.Reset(database, template);
            _tplBehaListCvt.Reset(database, template);
            _tplCompCvt.Reset(database, template);
            _tplBehaCvt.Reset(database, template);

            serializer.Converters.Add(_tplCompListCvt);
            serializer.Converters.Add(_tplBehaListCvt);
            serializer.Converters.Add(_tplCompCvt);
            serializer.Converters.Add(_tplBehaCvt);
        }

        public static void PopConverters(JsonSerializer serializer)
        {
            serializer.Converters.Remove(_tplCompListCvt);
            serializer.Converters.Remove(_tplBehaListCvt);
            serializer.Converters.Remove(_tplCompCvt);
            serializer.Converters.Remove(_tplBehaCvt);

            _tplCompListCvt.Reset(null, null);
            _tplBehaListCvt.Reset(null, null);
            _tplCompCvt.Reset(null, null);
            _tplBehaCvt.Reset(null, null);
        }

        public static bool ParseComponentName(string str, out string name) => ParseComponentName(str, out name, out _, out _, out _);

        public static bool ParseComponentName(string str, out string name, out bool flyweight, out TemplateOverride overrideType, out int overrideIndex)
        {
            name          = null;
            flyweight     = false;
            overrideType  = TemplateOverride.None;
            overrideIndex = -1;

            int nameBeg = -1;
            int nameEnd = -1;
            int cur     =  0;
            // Skip the initial white spaces.
            for (; cur < str.Length; cur++)
            {
                if (char.IsWhiteSpace(str[cur]))
                {
                    continue;
                }

                break;
            }
            // Read the prefix.
            switch (str[cur])
            {
                case CharOverrideAdd:    { overrideType = TemplateOverride.None;   cur++; } break;
                case CharOverrideRemove: { overrideType = TemplateOverride.Remove; cur++; } break;
                case CharOverrideReplace: 
                {
                    overrideType = TemplateOverride.Replace;
                    int indexBeg = cur + 1;
                    int indexEnd = cur + 1;
                    // Read the index to replace.
                    for (cur++; cur < str.Length;)
                    {
                        if (char.IsDigit(str[cur]))
                        {
                            indexEnd = cur;
                            cur++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    // Parse the override index.
                    if (indexEnd > indexBeg)
                    {
                        overrideIndex = int.Parse(str.Substring(indexBeg, indexEnd - indexBeg + 1));
                    }
                    else
                    {
                        overrideIndex = -1;
                    }
                }
                break;
                // If there is no prefix character, the default override is applied.
                default:
                {
                    overrideType = DefaultOverride;
                }
                break;
            }
            // Find the beginning of the component name.
            for (; cur < str.Length; cur++)
            {
                if (char.IsWhiteSpace(str[cur]))
                {
                    continue;
                }

                nameBeg = cur;
                nameEnd = cur;

                break;
            }
            // Find the end of the component name.
            if (nameBeg >= 0)
            {
                for (; cur < str.Length; cur++)
                {
                    if (char.IsWhiteSpace(str[cur]))
                    {
                        break;
                    }

                    nameEnd = cur;
                }
            }
            // Check if the name has the prefix and suffix for flyweights.
            if (nameEnd > nameBeg && str[nameBeg] == CharFlyweight && str[nameEnd] == CharFlyweight) 
            {
                nameBeg++;
                nameEnd--;
                flyweight = true;
            }
            // Get final name.
            if (nameEnd > nameBeg)
            {
                name = char.ToUpper(str[nameBeg]) + str.Substring(nameBeg + 1, nameEnd - nameBeg);
            }
            // Done.
            return name != null;
        }

        public static bool ParseBehaviourName(string str, out string name, out TemplateOverride overrideType)
        {
            name          = null;
            overrideType  = TemplateOverride.None;

            int nameBeg = -1;
            int nameEnd = -1;
            int cur     =  0;
            // Skip the initial white spaces.
            for (; cur < str.Length; cur++)
            {
                if (char.IsWhiteSpace(str[cur]))
                {
                    continue;
                }

                break;
            }
            // Read the prefix.
            switch (str[cur])
            {
                case CharOverrideAdd:     { overrideType = TemplateOverride.None;   cur++; } break;
                case CharOverrideRemove:  { overrideType = TemplateOverride.Remove; cur++; } break;
                case CharOverrideReplace: { overrideType = TemplateOverride.None;   cur++; } break;
            }
            // Find the beginning of the component name.
            for (; cur < str.Length; cur++)
            {
                if (char.IsWhiteSpace(str[cur]))
                {
                    continue;
                }

                nameBeg = cur;
                nameEnd = cur;

                break;
            }
            // Find the end of the component name.
            if (nameBeg >= 0)
            {
                for (; cur < str.Length; cur++)
                {
                    if (char.IsWhiteSpace(str[cur]))
                    {
                        break;
                    }

                    nameEnd = cur;
                }
            }
            // Get final name.
            if (nameEnd > nameBeg)
            {
                name = char.ToUpper(str[nameBeg]) + str.Substring(nameBeg + 1, nameEnd - nameBeg);
            }
            // Done.
            return name != null;
        }
    }
}
