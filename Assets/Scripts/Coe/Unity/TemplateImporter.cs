using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Rogue.Coe.Unity
{
    [ScriptedImporter(1, "tpl")]
    public class TemplateImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            TextAsset text = new (File.ReadAllText(ctx.assetPath));
            ctx.AddObjectToAsset("template", text);
            ctx.SetMainObject(text);
        }
    }
}
