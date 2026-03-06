using UnityEditor;
using UnityEditor.AddressableAssets;
using System.IO;

class AddressableAutoName : AssetPostprocessor
{
    static void OnPostprocessAllAssets(
        string[] imported,
        string[] deleted,
        string[] moved,
        string[] movedFrom)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null) return;

        foreach (var path in imported)
        {
            var entry = settings.FindAssetEntry(AssetDatabase.AssetPathToGUID(path));
            if (entry != null)
            {
                entry.address = Path.GetFileNameWithoutExtension(path);
            }
        }
    }
}