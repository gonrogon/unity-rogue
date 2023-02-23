using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Rogue.Coe.Unity
{
    [ScriptedImporter(1, "tdb")]
    public class TemplateDatabaseImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            TextAsset text = new (File.ReadAllText(ctx.assetPath));
            ctx.AddObjectToAsset("database", text);
            ctx.SetMainObject(text);
        }
    }
}
