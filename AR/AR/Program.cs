using EloBuddy;
using EloBuddy.SDK.Events;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace AR
{
    class Program
    {
        private static string cPath = @"C:\Users\" + Environment.UserName;
        private static string dllPath = cPath + @"\AppData\Roaming\EloBuddy\Addons\Libraries\AR.dll";
        private static string dllAddress = "https://raw.githubusercontent.com/xProXseik/Addons/master/AR/AR.dll";

        // Method credits to DanThePman.
        static string GetOnlineVersion()
        {
            string version = string.Empty;
            WebRequest req = WebRequest.Create("https://raw.githubusercontent.com/xProXseik/Addons/master/AR/version.txt");
            req.Method = "GET";

            // ReSharper disable once AssignNullToNotNullAttribute
            using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    version = line;
                    break;
                }
            }
            return version;
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += eventArgs =>
            {
                if (!File.Exists(dllPath))
                {
                    Download();
                    CallAddon();
                }
                else
                {
                    Assembly axembly = Assembly.Load(File.ReadAllBytes(dllPath));
                    WebClient webC = new WebClient();

                    var localVersion = axembly.GetName().Version.ToString();
                    string webVersion = GetOnlineVersion();

                    if (!localVersion.Equals(webVersion))
                    {
                        Update();
                    }

                    CallAddon();
                }
            };
        }

        private static void CallAddon()
        {
            Assembly xAssembly = Assembly.LoadFrom(dllPath);
            Type myType = xAssembly.GetType("c");

            var main = myType.GetMethod("a", BindingFlags.Public| BindingFlags.Static);
            main.Invoke(null, null);

            Chat.Print("[AR] Loaded!");
        }

        private static void Download()
        {
            WebClient webC = new WebClient();
            Chat.Print("[Loader] Downloading...");
            webC.DownloadFile(dllAddress, dllPath);
            Chat.Print("[Loader] Successfully Downloaded.");
        }

        private static void Update()
        {
            WebClient webC = new WebClient();
            Chat.Print("[Loader] Updating...");
            webC.DownloadFile(dllAddress, dllPath);
            Chat.Print("[Loader] Successfully Updated.");
        }
    }
}
