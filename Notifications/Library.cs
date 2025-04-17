using System;
using System.Linq;
using BepInEx;
using Pathfinding.RVO.Sampled;
using UnityEngine;
using UnityEngine.UI;
using static StupidTemplate.Settings;

namespace StupidTemplate.Notifications
{
    [BepInPlugin("org.gorillatag.lars.notifications2", "NotificationLibrary", "1.0.5")]
    public class NotifiLib : BaseUnityPlugin
    {
        private void Awake()
        {
            base.Logger.LogInfo("Plugin NotificationLibrary is loaded!");
        }

        private void Init()
        {
            MainCamera = GameObject.Find("Main Camera");
            HUDObj = new GameObject();
            HUDObj2 = new GameObject();
            HUDObj2.name = "NOTIFICATIONLIB_HUD_OBJ";
            HUDObj.name = "NOTIFICATIONLIB_HUD_OBJ";
            HUDObj.AddComponent<Canvas>();
            HUDObj.AddComponent<CanvasScaler>();
            HUDObj.AddComponent<GraphicRaycaster>();
            HUDObj.GetComponent<Canvas>().enabled = true;
            HUDObj.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            HUDObj.GetComponent<Canvas>().worldCamera = MainCamera.GetComponent<Camera>();
            HUDObj.GetComponent<RectTransform>().sizeDelta = new Vector2(5f, 5f);
            HUDObj.GetComponent<RectTransform>().position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z);
            HUDObj2.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, MainCamera.transform.position.z - 4.6f);
            HUDObj.transform.parent = HUDObj2.transform;
            HUDObj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 1.6f);
            Vector3 eulerAngles = HUDObj.GetComponent<RectTransform>().rotation.eulerAngles;
            eulerAngles.y = -270f;
            HUDObj.transform.localScale = new Vector3(1f, 1f, 1f);
            HUDObj.GetComponent<RectTransform>().rotation = Quaternion.Euler(eulerAngles);
            Testtext = new GameObject
            {
                transform =
                {
                    parent = HUDObj.transform
                }
            }.AddComponent<Text>();
            Testtext.text = "";
            Testtext.fontSize = 30;
            Testtext.font = currentFont;
            Testtext.rectTransform.sizeDelta = new Vector2(450f, 210f);
            Testtext.alignment = TextAnchor.LowerLeft;
            Testtext.verticalOverflow = VerticalWrapMode.Overflow;
            Testtext.rectTransform.localScale = new Vector3(0.00333333333f, 0.00333333333f, 0.33333333f);
            Testtext.rectTransform.localPosition = new Vector3(-1f, -1f, -0.5f);
            Testtext.material = AlertText;
            NotifiText = Testtext;

            Text Text2 = new GameObject
            {
                transform =
                {
                    parent = HUDObj.transform
                }
            }.AddComponent<Text>();
            Text2.text = "";
            Text2.fontSize = 20;
            Text2.font = currentFont;
            Text2.rectTransform.sizeDelta = new Vector2(450f, 1000f);
            Text2.alignment = TextAnchor.UpperLeft;
            Text2.rectTransform.localScale = new Vector3(0.00333333333f, 0.00333333333f, 0.33333333f);
            Text2.rectTransform.localPosition = new Vector3(-1f, -0.7f, -0.5f);
            Text2.material = AlertText;
        }

        private void FixedUpdate()
        {
            bool flag = !this.HasInit && GameObject.Find("Main Camera") != null;
            if (flag)
            {
                this.Init();
                this.HasInit = true;
            }
            this.HUDObj2.transform.position = new Vector3(this.MainCamera.transform.position.x, this.MainCamera.transform.position.y, this.MainCamera.transform.position.z);
            this.HUDObj2.transform.rotation = this.MainCamera.transform.rotation;
            if (this.Testtext.text != "")
            {
                this.NotificationDecayTimeCounter++;
                if (this.NotificationDecayTimeCounter > this.NotificationDecayTime)
                {
                    this.Notifilines = null;
                    this.newtext = "";
                    this.NotificationDecayTimeCounter = 0;
                    this.Notifilines = Enumerable.ToArray<string>(Enumerable.Skip<string>(this.Testtext.text.Split(Environment.NewLine.ToCharArray()), 1));
                    foreach (string text in this.Notifilines)
                    {
                        if (text != "")
                        {
                            this.newtext = this.newtext + text + "\n";
                        }
                    }
                    this.Testtext.text = this.newtext;
                }
            }
            else
            {
                this.NotificationDecayTimeCounter = 0;
            }
        }

        public static void SendNotification(string NotificationText)
        {
            if (!disableNotifications)
            {
                try
                {
                    if (NotifiLib.IsEnabled && NotifiLib.PreviousNotifi != NotificationText)
                    {
                        if (!NotificationText.Contains(Environment.NewLine))
                        {
                            NotificationText += Environment.NewLine;
                        }
                        NotifiLib.NotifiText.text = NotifiLib.NotifiText.text + NotificationText;
                        NotifiLib.NotifiText.supportRichText = true;
                        NotifiLib.PreviousNotifi = NotificationText;
                    }
                }
                catch
                {
                    UnityEngine.Debug.LogError("Notification failed, object probably nil due to third person ; " + NotificationText);
                }
            }
        }

        public static void ClearAllNotifications()
        {
            //NotifiLib.NotifiText.text = "<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> <color=white>Notifications cleared.</color>" + Environment.NewLine;
            NotifiLib.NotifiText.text = "";
        }

        public static void ClearPastNotifications(int amount)
        {
            string text = "";
            foreach (string text2 in Enumerable.ToArray<string>(Enumerable.Skip<string>(NotifiLib.NotifiText.text.Split(Environment.NewLine.ToCharArray()), amount)))
            {
                if (text2 != "")
                {
                    text = text + text2 + "\n";
                }
            }
            NotifiLib.NotifiText.text = text;
        }

        private GameObject HUDObj;

        private GameObject HUDObj2;

        private GameObject MainCamera;

        private Text Testtext;

        private Material AlertText = new Material(Shader.Find("GUI/Text Shader"));

        private int NotificationDecayTime = 144;

        private int NotificationDecayTimeCounter;

        public static int NoticationThreshold = 30;

        private string[] Notifilines;

        private string newtext;

        public static string PreviousNotifi;

        private bool HasInit;

        private static Text NotifiText;

        public static bool IsEnabled = true;
    }
}
