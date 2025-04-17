using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using StupidTemplate.Menu;
using UnityEngine.InputSystem;
using UnityEngine;
using StupidTemplate.Classes;
namespace Iris.Classes
{
    internal class sig : BaseUnityPlugin
    {
        Rect windowRectangle = new Rect(100, 20, 800, 400);
        bool isGuiEnabled = true;
        public Vector2[] scroll = new Vector2[2];
        private string searchQuery = "", roomStr = "text here";
        Texture2D windowBackgroundT2D;
        Texture2D tabAreaT2D;
        Texture2D tabButtonT2D;
        Texture2D featureToggleT2D;
        Texture2D featureToggleBarT2D;
        Texture2D featureToggleBoxOffT2D;
        Texture2D featureToggleBoxOnT2D;
        Texture2D watermarkT2D;

        Vector2 watermarkVec2;
        Texture2D gradientBorderT2D;
        float gradientTime = 0f;
        GUIStyle tabButtonStyle;
        GUIStyle featureToggleStyle;
        GUIStyle featureDescLabelStyle;

        int selectedTab = 0;

        class Tab
        {
            public string Name;
            public int ID;

            public Tab(string name, int Id)
            {
                Name = name;
                ID = Id;
            }
        }
        List<Tab> tabs = new List<Tab>();

        class ButtonInfo
        {
            public string Name;
            public string Description;
            public Action OnClick;
            public int TabID;
            public bool IsRunning = false;
            public Coroutine RunningCoroutine = null;

            public ButtonInfo(string name, string description, Action onClick, int tabID)
            {
                Name = name;
                Description = description;
                OnClick = onClick;
                TabID = tabID;
            }
        }
        List<ButtonInfo> buttons = new List<ButtonInfo>();
        public static ExtGradient backgroundColor = new ExtGradient { isRainbow = false };
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient { colors = GetSolidGradient(new Color32(35,35,35,255))},//off
            new ExtGradient { colors = GetSolidGradient(new Color32(138, 43, 226,255))} // on
        };
        public static Color[] textColors = new Color[]
        {
            Color.white, // Disabled
            Color.white // Enabled
        };
        public static GradientColorKey[] GetSolidGradient(Color color)
        {
            return new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
        }
        void Start()
        {
            windowBackgroundT2D = MakeRoundedTexture(800, 400, backgroundColor.colors[0].color, 10);
            tabAreaT2D = MakeRoundedTexture(120, 400, backgroundColor.colors[0].color, 10, leftOnly: true);
            tabButtonT2D = MakeSolidColorTexture(buttonColors[0].colors[0].color);
            featureToggleT2D = MakeRoundedTexture(800, 400, Color.black, 5);
            featureToggleBarT2D = MakeRoundedTexture(800, 400, Color.white, 5);
            featureToggleBoxOnT2D = MakeRoundedTexture(15, 15, buttonColors[1].colors[0].color, 2);
            featureToggleBoxOffT2D = MakeRoundedTexture(15, 15, buttonColors[0].colors[0].color, 2);
            AddTab("Test 1", 0);
            AddTab("Test 2", 1);
            AddTab("Test 3", 2);
            AddTab("Test 4", 3);
            AddTab("Test 5", 4);
            AddTab("Test 6", 5);
            AddTab("Test 7", 6);
            AddTab("Test 8", 7);


            AddButton("", "", () =>
            {

            }, 0);
        }

        void AddTab(string name, int ID) => tabs.Add(new Tab(name, ID));

        void AddButton(string name, string description, Action onClick, int tabID) => buttons.Add(new ButtonInfo(name, description, onClick, tabID));

        void DrawButton(Rect position, ButtonInfo buttonInfo)
        {
            if (GUI.Button(position, buttonInfo.Name, featureToggleStyle))
            {
                buttonInfo.IsRunning = !buttonInfo.IsRunning;

                if (buttonInfo.IsRunning) { if (buttonInfo.RunningCoroutine == null) buttonInfo.RunningCoroutine = StartCoroutine(ButtonActionLoop(buttonInfo)); }
                else
                {
                    if (buttonInfo.RunningCoroutine != null)
                    {
                        StopCoroutine(buttonInfo.RunningCoroutine);
                        buttonInfo.RunningCoroutine = null;
                    }
                }
            }

            GUI.DrawTexture(new Rect(position.x, position.y + 24, position.width, 1), featureToggleBarT2D);
            GUI.DrawTexture(new Rect(position.x + 5, position.y + 28, 18, 18), buttonInfo.IsRunning ? featureToggleBoxOnT2D : featureToggleBoxOffT2D);

            GUI.Label(new Rect(position.x + 28, position.y + 28, 100, 22), "Toggle", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.UpperLeft });
            GUI.Label(new Rect(position.x + 5, position.y, position.width - 5, position.height), buttonInfo.Description, featureDescLabelStyle);
        }

        IEnumerator ButtonActionLoop(ButtonInfo buttonInfo)
        {
            while (buttonInfo.IsRunning)
            {
                buttonInfo.OnClick?.Invoke();
                yield return null;
            }
        }

        void MainGUI(int windowID)
        {
            tabButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 15,
                normal = { background = tabButtonT2D },
                hover = { background = tabButtonT2D },
                active = { background = tabButtonT2D }
            };
            GUI.DrawTexture(new Rect(0, 0, windowRectangle.width, 5), gradientBorderT2D);
            featureToggleStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 15,
                normal = { background = featureToggleT2D },
                hover = { background = featureToggleT2D },
                active = { background = featureToggleT2D },
                alignment = TextAnchor.UpperLeft
            };


            GUI.DrawTexture(new Rect(0, 0, 120, 400), tabAreaT2D);
            GUI.Label(new Rect(0, 5, 120, 40), "I R I S", new GUIStyle(GUI.skin.label) { fontSize = 20, alignment = TextAnchor.LowerCenter });

            for (int i = 0; i < tabs.Count; i++) if (GUI.Button(new Rect(5, 50 + i * 35, 110, 30), tabs[i].Name, tabButtonStyle)) selectedTab = i;

            foreach (var button in buttons) if (button.TabID == selectedTab) DrawButton(new Rect(140 + (buttons.IndexOf(button) % 3) * 218, 10 + (buttons.IndexOf(button) / 3) * 76, 203, 66), button);


            GUI.DragWindow();
        }

        void Update()
        {
            gradientTime += Time.deltaTime;

            if (Keyboard.current.insertKey.wasReleasedThisFrame) isGuiEnabled = !isGuiEnabled;
        }

        void OnGUI()
        {
            GUIStyle windowStyle = new GUIStyle(GUI.skin.window)
            {
                fontStyle = FontStyle.Bold,
                normal = { background = windowBackgroundT2D },
                onNormal = { background = windowBackgroundT2D },
                onActive = { background = windowBackgroundT2D },
            };
            if (isGuiEnabled) windowRectangle = GUI.Window(1684115495, windowRectangle, MainGUI, "", windowStyle);
        }



        static Texture2D MakeSolidColorTexture(Color color, int width = 1, int height = 1)
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = color;
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        Color GetColorFromTime(float time /* https://github.com/iidk */)
        {
            float r = Mathf.PingPong(time * 0.1f, 1f);
            float g = Mathf.PingPong(time * 0.1f + 0.3f, 1f);
            float b = Mathf.PingPong(time * 0.1f + 0.6f, 1f);

            return new Color(r, g, b);
        }

        static Texture2D CreateGradientTexture(int width, int height, Color startColor, Color endColor)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                Color color = Color.Lerp(startColor, endColor, (float)x / width);
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            return texture;
        }
        static Texture2D MakeRoundedTexture(int width, int height, Color color, int borderRadius, bool leftOnly = false, bool bottomOnly = false)
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isCorner;

                    if (leftOnly)
                    {
                        isCorner = (x < borderRadius && y < borderRadius && (x - borderRadius) * (x - borderRadius) + (y - borderRadius) * (y - borderRadius) > borderRadius * borderRadius) ||
                                   (x < borderRadius && y > height - borderRadius - 1 && (x - borderRadius) * (x - borderRadius) + (y - (height - borderRadius - 1)) * (y - (height - borderRadius - 1)) > borderRadius * borderRadius);
                    }
                    else if (bottomOnly)
                    {
                        isCorner = (x < borderRadius && y < borderRadius && (x - borderRadius) * (x - borderRadius) + (y - borderRadius) * (y - borderRadius) > borderRadius * borderRadius) ||
                                   (x > width - borderRadius - 1 && y < borderRadius && (x - (width - borderRadius - 1)) * (x - (width - borderRadius - 1)) + (y - borderRadius) * (y - borderRadius) > borderRadius * borderRadius);
                    }
                    else
                    {
                        isCorner = (x < borderRadius && y < borderRadius && (x - borderRadius) * (x - borderRadius) + (y - borderRadius) * (y - borderRadius) > borderRadius * borderRadius) ||
                                   (x > width - borderRadius - 1 && y < borderRadius && (x - (width - borderRadius - 1)) * (x - (width - borderRadius - 1)) + (y - borderRadius) * (y - borderRadius) > borderRadius * borderRadius) ||
                                   (x < borderRadius && y > height - borderRadius - 1 && (x - borderRadius) * (x - borderRadius) + (y - (height - borderRadius - 1)) * (y - (height - borderRadius - 1)) > borderRadius * borderRadius) ||
                                   (x > width - borderRadius - 1 && y > height - borderRadius - 1 && (x - (width - borderRadius - 1)) * (x - (width - borderRadius - 1)) + (y - (height - borderRadius - 1)) * (y - (height - borderRadius - 1)) > borderRadius * borderRadius);
                    }

                    pixels[x + y * width] = isCorner ? new Color(0, 0, 0, 0) : color;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}