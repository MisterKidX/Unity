﻿using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Utilities
{
#if UNITY_EDITOR

    /// <summary>
    /// This class supports automatic preset application on specific folders.
    /// </summary>
    /// <example> Add a preset to a folder and import the respective asset to it. The preset in the folder would be applied to all the assets imported automatically. </example>
    [HelpURL("https://docs.unity3d.com/Manual/DefaultPresetsByFolder.html")]
    public class PresetImportPerFolder : AssetPostprocessor
    {
        void OnPreprocessAsset()
        {
            // Make sure we are applying presets the first time an asset is imported.
            if (assetImporter.importSettingsMissing)
            {
                // Get the current imported asset folder.
                var path = Path.GetDirectoryName(assetPath);

                while (!string.IsNullOrEmpty(path))
                {
                    // Ignore certain folders
                    if (path.Contains("ProjectSettings") || path.Contains("Library"))
                        return;

                    // Find all Preset assets in this folder.
                    var presetGuids = AssetDatabase.FindAssets("t:Preset", new[] { path });
                    foreach (var presetGuid in presetGuids)
                    {
                        // Make sure we are not testing Presets in a subfolder.
                        string presetPath = AssetDatabase.GUIDToAssetPath(presetGuid);

                        if (Path.GetDirectoryName(presetPath) == path)
                        {
                            // Load the Preset and try to apply it to the importer.
                            var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                            if (preset.ApplyTo(assetImporter))
                                return;
                        }
                    }

                    // Try again in the parent folder.
                    path = Path.GetDirectoryName(path);
                }
            }
        }
    }

#endif

}