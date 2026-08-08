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
    using System.Diagnostics;
    using System.Text;
    using UnityEngine;

    public class FacebookAndroidUtil
    {
        public const string ErrorNoSDK = "no_android_sdk";
        public const string ErrorNoKeystore = "no_android_keystore";
        public const string ErrorNoKeytool = "no_java_keytool";
        public const string ErrorNoOpenSSL = "no_openssl";
        public const string ErrorKeytoolError = "java_keytool_error";

        private static string debugKeyHash;
        private static string setupError;

        public static bool SetupProperly
        {
            get
            {
                return DebugKeyHash != null;
            }
        }

        public static string DebugKeyHash
        {
            get
            {
                if (debugKeyHash == null)
                {
                    if (!HasAndroidSDK())
                    {
                        setupError = ErrorNoSDK;
                        return null;
                    }

                    if (!HasAndroidKeystoreFile())
                    {
                        setupError = ErrorNoKeystore;
                        return null;
                    }

                    if (!DoesCommandExist("echo \"xxx\" | openssl base64"))
                    {
                        setupError = ErrorNoOpenSSL;
                        return null;
                    }

                    if (!DoesCommandExist("keytool"))
                    {
                        setupError = ErrorNoKeytool;
                        return null;
                    }

                    debugKeyHash = GetKeyHash("androiddebugkey", DebugKeyStorePath, "android");
                }

                return debugKeyHash;
            }
        }

        public static string SetupError
        {
            get
            {
                return setupError;
            }
        }

        private static string DebugKeyStorePath
        {
            get
            {
                return (Application.platform == RuntimePlatform.WindowsEditor) ?
                    System.Environment.GetEnvironmentVariable("HOMEDRIVE") + System.Environment.GetEnvironmentVariable("HOMEPATH") + @"\.android\debug.keystore" :
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + @"/.android/debug.keystore";
            }
        }

        public static bool HasAndroidSDK()
        {
            return EditorPrefs.HasKey("AndroidSdkRoot") && System.IO.Directory.Exists(EditorPrefs.GetString("AndroidSdkRoot"));
        }

        public static bool HasAndroidKeystoreFile()
        {
            return System.IO.File.Exists(DebugKeyStorePath);
        }

        private static string GetKeyHash(string alias, string keyStore, string password)
        {
            var proc = new Process();
            var arguments = @"""keytool -storepass {0} -keypass {1} -exportcert -alias {2} -keystore {3} | openssl sha1 -binary | openssl base64""";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                proc.StartInfo.FileName = "cmd";
                arguments = @"/C " + arguments;
            }
            else
            {
                proc.StartInfo.FileName = "bash";
                arguments = @"-c " + arguments;
            }

            proc.StartInfo.Arguments = string.Format(arguments, password, password, alias, keyStore);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            var keyHash = new StringBuilder();
            while (!proc.HasExited)
            {
                keyHash.Append(proc.StandardOutput.ReadToEnd());
            }

            switch (proc.ExitCode)
            {
                case 255: setupError = ErrorKeytoolError;
                    return null;
            }

            return keyHash.ToString().TrimEnd('\n');
        }

        private static bool DoesCommandExist(string command)
        {
            var proc = new Process();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.Arguments = @"/C" + command;
            }
            else
            {
                proc.StartInfo.FileName = "bash";
                proc.StartInfo.Arguments = @"-c " + command;
            }

            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            proc.WaitForExit();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return proc.ExitCode == 0;
            }
            else
            {
                return proc.ExitCode != 127;
            }
        }
    }
}
