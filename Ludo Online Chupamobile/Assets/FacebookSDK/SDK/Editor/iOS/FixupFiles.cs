/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace UnityEditor.FacebookEditor
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class FixupFiles
    {
        private static string didFinishLaunchingWithOptions =
@"(?x)                                  # Verbose mode
  (didFinishLaunchingWithOptions.+      # Find this function...
    (?:.*\n)+?                          # Match as few lines as possible until...
    \s*return\ )NO(\;\n                 #   return NO;
  \})                                   # }";

        public static void FixSimulator(string path)
        {
            string fullPath = Path.Combine(path, Path.Combine("Libraries", "RegisterMonoModules.cpp"));
            string data = Load(fullPath);

            data = Regex.Replace(data, @"\s+void\s+mono_dl_register_symbol\s+\(const\s+char\*\s+name,\s+void\s+\*addr\);", string.Empty);
            data = Regex.Replace(data, "typedef int gboolean;", "typedef int gboolean;\n\tvoid mono_dl_register_symbol (const char* name, void *addr);");

            // this only need to be done for unity 4, unity 5 declares user functions correctly
            if (GetUnityVersionNumber() < 500)
            {
                data = Regex.Replace(
                    data,
                    @"#endif\s+//\s*!\s*\(\s*TARGET_IPHONE_SIMULATOR\s*\)\s*}\s*void RegisterAllStrippedInternalCalls\s*\(\s*\)",
                    "}\n\nvoid RegisterAllStrippedInternalCalls()");
                data = Regex.Replace(
                    data,
                    @"mono_aot_register_module\(mono_aot_module_mscorlib_info\);",
                    "mono_aot_register_module(mono_aot_module_mscorlib_info);\n#endif // !(TARGET_IPHONE_SIMULATOR)");
            }

            Save(fullPath, data);
        }

        public static void AddVersionDefine(string path)
        {
            int versionNumber = GetUnityVersionNumber();

            string fullPath = Path.Combine(path, Path.Combine("Libraries", "RegisterMonoModules.h"));
            string data = Load(fullPath);

            if (versionNumber >= 430)
            {
                data += "\n#define HAS_UNITY_VERSION_DEF 1\n";
            }
            else
            {
                data += "\n#define UNITY_VERSION ";
                data += versionNumber;
                data += "\n";
            }

            Save(fullPath, data);
        }

        public static void FixColdStart(string path)
        {
            string fullPath = Path.Combine(path, Path.Combine("Classes", "UnityAppController.mm"));
            string data = Load(fullPath);

            data = Regex.Replace(
                data,
                didFinishLaunchingWithOptions,
                "$1YES$2");

            Save(fullPath, data);
        }

        protected static string Load(string fullPath)
        {
            string data;
            FileInfo projectFileInfo = new FileInfo(fullPath);
            StreamReader fs = projectFileInfo.OpenText();
            data = fs.ReadToEnd();
            fs.Close();

            return data;
        }

        protected static void Save(string fullPath, string data)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fullPath, false);
            writer.Write(data);
            writer.Close();
        }

        private static int GetUnityVersionNumber()
        {
            string version = Application.unityVersion;
            string[] versionComponents = version.Split('.');

            int majorVersion = 0;
            int minorVersion = 0;

            try
            {
                if (versionComponents != null && versionComponents.Length > 0 && versionComponents[0] != null)
                {
                    majorVersion = Convert.ToInt32(versionComponents[0]);
                }

                if (versionComponents != null && versionComponents.Length > 1 && versionComponents[1] != null)
                {
                    minorVersion = Convert.ToInt32(versionComponents[1]);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error parsing Unity version number: " + e);
            }

            return (majorVersion * 100) + (minorVersion * 10);
        }
    }
}
