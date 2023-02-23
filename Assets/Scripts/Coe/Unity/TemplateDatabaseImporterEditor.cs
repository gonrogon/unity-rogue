using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Rogue.Coe.Unity
{
    [CustomEditor(typeof(TemplateDatabaseImporter))]
    public class TemplateDatabaseImporterEditor : AssetImporterEditor
    {
        protected override bool needsApplyRevert => false;

        public override void OnInspectorGUI()
        {}
    }
}
