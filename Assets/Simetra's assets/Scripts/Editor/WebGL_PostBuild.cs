using System.IO;

using UnityEditor;
using UnityEditor.Callbacks;

public class PostBuildActions
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string targetPath)
    {
        if (target == BuildTarget.WebGL)
        {
            // disabling webgl warning on mobile
            string jsPath = Path.Combine(targetPath, @"Build/UnityLoader.js");
            string jsText = File.ReadAllText(jsPath);

            jsText = jsText.Replace("UnityLoader.SystemInfo.mobile", "false");

            File.WriteAllText(jsPath, jsText);

            // changing html tag to reduce blurriness of game canvas when scaling in/out
            string htmlPath = Path.Combine(targetPath, @"index.html");
            string htmlText = File.ReadAllText(htmlPath);

            htmlText = htmlText.Replace(@"<body>", @"<body style=""image-rendering: pixelated"">");

            File.WriteAllText(htmlPath, htmlText);
        }
    }
}