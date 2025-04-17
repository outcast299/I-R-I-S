using Photon.Pun;
using System.Diagnostics;
using System.IO;
using StupidTemplate.Classes;
using StupidTemplate.Menu;
using UnityEngine;

namespace StupidTemplate
{
    internal class Settings
    {
        public static ExtGradient backgroundColor = new ExtGradient { isRainbow = false };
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient { colors = Main.GetSolidGradient(new Color32(27, 27, 27, 255))},//off
            new ExtGradient { colors = Main.GetSolidGradient(new Color32(34, 115, 179, 255))} // on
        };
        public static Color[] textColors = new Color[]
        {
            Color.white, // Disabled
            Color.white // Enabled
        };

        public static Font currentFont = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
        public static bool fpsCounter = false;
        public static bool disconnectButton = true;
        public static bool rightHanded = true;
        public static bool disableNotifications = false;

        public static KeyCode keyboardButton = KeyCode.Q;
        internal static int pageshit;
        public static int ButtonSound = 62;
        public static Vector3 menuSize = new Vector3(0.1f, 1f, 0.95f); // Depth, Width, Heightd
        public static int buttonsPerPage = 5;

        public static Vector3 PageButtonSize = new Vector3(0.09f, 0.9f, 0.13f); // Depth, Width, Heightd
        public static Vector3 ReturnPos = new Vector3(0.56f, 0, -0.53f);
        public static Vector3 ReturnSca = new Vector3(0.09f, 0.8f, 0.1f);
        public static Vector3 TextPos = new Vector3(0.06f, 0f, 0.155f);

        public static Vector3 ReturnTextPos = new Vector3(0.064f, 0, -0.18f);
        public static float buttonspace = 1.6f;

        public static float buttonspace2 = 0.1f;
        public static float textspace = 1.65f;

        public static Vector3 buttonpos = new Vector3(0.56f, 0f, 0.26f);
        public static Vector3 buttontpos = new Vector3(.064f, 0, .1045f);
      
        public static Vector3 NPpos = new Vector3(0.56f, -0.65f, 0);
        public static Vector3 NPsca = new Vector3(0.09f, 0.2f, 0.85f);
        public static Vector3 NPTpos = new Vector3(0.064f, -0.195f, 0f);

        public static Vector3 PPpos = new Vector3(0.56f, 0.65f, 0);
        public static Vector3 PPsca = new Vector3(0.09f, 0.2f, 0.85f);
        public static Vector3 PPTpos = new Vector3(0.064f, 0.195f, 0f);
        public static void DumpRPCData()
        {
            string text = "RPC Data\n(from PhotonNetwork.PhotonServerSettings.RpcList)";
            int i = 0;
            foreach (string name in PhotonNetwork.PhotonServerSettings.RpcList)
            {
                try
                {
                    text += "\n====================================\n";
                    text += i.ToString() + " ; " + name;
                }
                catch { UnityEngine.Debug.Log("Failed to log RPC"); }
                i++;
            }
            text += "\n====================================\n";
            text += "Text file generated with IRIS but its iis code";
            string fileName = "IRIS/RPCData.txt";
            if (!Directory.Exists("IRIS"))
            {
                Directory.CreateDirectory("IRIS");
            }
            File.WriteAllText(fileName, text);
            string filePath = System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, fileName);
            filePath = filePath.Split("BepInEx\\")[0] + fileName;
            try
            {
                Process.Start(filePath);
            }
            catch
            {
                UnityEngine.Debug.Log("Could not open process " + filePath);
            }
        }


        public static float fly = 6f;
        public static float Speed = 8f;
    }
}
