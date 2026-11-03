using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

[InitializeOnLoad]
public static class AnalyticsSdkDefine
{
    static AnalyticsSdkDefine()
    {
        Apply(NamedBuildTarget.Android);
        Apply(NamedBuildTarget.iOS);
    }

    private static void Apply(NamedBuildTarget target)
    {
        string[] defines = PlayerSettings.GetScriptingDefineSymbols(target)
            .Split(';')
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

        string[] updated = defines;
        updated = Update(updated, "ONEUP_FIREBASE", AnalyticsManifestEditor.FirebaseFolder);
        updated = Update(updated, "ONEUP_GAMEANALYTICS", AnalyticsManifestEditor.GameAnalyticsFolder);
        updated = Update(updated, "ONEUP_APPMETRICA", AnalyticsManifestEditor.AppMetricaFolder);

        if (!updated.SequenceEqual(defines))
            PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", updated));
    }

    private static string[] Update(string[] defines, string define, string folder)
    {
        bool installed = Directory.Exists(folder);
        if (installed == defines.Contains(define))
            return defines;

        return installed ? defines.Append(define).ToArray() : defines.Where(x => x != define).ToArray();
    }
}
