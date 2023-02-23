using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Rogue.Coe.Unity
{
    [CustomEditor(typeof(TemplateImporter))]
    public class TemplateImporterEditor : AssetImporterEditor
    {
        protected override bool needsApplyRevert => false;

        public override void OnInspectorGUI()
        {}
    }
}
