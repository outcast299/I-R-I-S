using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using GorillaNetworking;
using IRIS.Menu;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Iris.Menus
{
    internal class GUId : BaseUnityPlugin
    {
        private int currentTab = 0;
        private int previousTab = -1;
        private readonly string[] tabNames = { "Modules", "Computer", "Console", "Settings" };
        private Rect windowRect = new Rect(0, 30, 555, 430);
        private Vector2[] scroll = new Vector2[4]; 
        private string searchQuery = "";
        private string roomCode = "";
        private float tabTransition = 0f;
        private float contentFade = 0f;
        private Vector2 contentOffset = Vector2.zero;
        private bool isAnimating = false;
        public static bool GUIShown = false;
        private float guiAnimationProgress = 0f;
        private bool isGuiAnimating = false;
        private bool animationsEnabled = true;
        private float transitionSpeed = 5f;
        private float guiAnimationSpeed = 5f;
        private float slideDistance = 25f;
        private enum AnimationType { Pop, Fade, Slide }
        private AnimationType currentAnimationType = AnimationType.Fade;
        public static string filename = "IrisLogs1.txt";
        public static int num = 1;
        public const int MaxLogs = 100;
        public static List<string> logs = new List<string>();
        private struct Star
        {
            public Vector2 position;
            public float speed;
            public float size;
        }
        private Star[] stars;
        private const int STAR_COUNT = 70;
        private static readonly string[] iris = { " I ", " R ", " I ", " S " };
        private string animatedTitle = "";
        private int currentIndex = 0;
        private float nextCharTime = 0f;
        private float resetDelay = 0f;

        private void Start()
        {
            InitializeStars();
            Application.logMessageReceived += HandleLog;
        }

        private void InitializeStars()
        {
            stars = new Star[STAR_COUNT];
            for (int i = 0; i < STAR_COUNT; i++)
            {
                stars[i] = new Star
                {
                    position = new Vector2(Random.value * windowRect.width, Random.value * windowRect.height),
                    speed = Random.Range(20f, 50f),
                    size = Random.Range(12f, 18f)
                };
            }
        }

        private void OnGUI()
        {
            if (VaultLock.IsAuthenticated == true)
            {
                UpdateTitleAnimation();
                HandleGUIInput();
                if (GUIShown || isGuiAnimating)
                {
                    UpdateGUIAnimation();
                    GUI.color = Color.white;
                    windowRect = GUI.Window(0, windowRect, WindowFunction, animatedTitle);
                    GUI.matrix = Matrix4x4.identity;
                }
            }
        }

        private void UpdateTitleAnimation()
        {
            if (currentIndex == iris.Length)
            {
                if (Time.time > resetDelay)
                {
                    animatedTitle = "";
                    currentIndex = 0;
                    nextCharTime = Time.time + 0.1f;
                    resetDelay = Time.time + 7f;
                }
            }
            else if (Time.time > nextCharTime)
            {
                animatedTitle += iris[currentIndex];
                currentIndex++;
                nextCharTime = Time.time + 0.1f;
            }
        }

        private float lastClickTime = 0f;
        private const float CLICK_DELAY = 0.1f;
        private void HandleGUIInput()
        {
            if (UnityInput.Current.GetKeyDown(KeyCode.F1))
            {
                if (Time.time - lastClickTime >= CLICK_DELAY)
                {
                    GUIShown = !GUIShown;
                    isGuiAnimating = true;
                    guiAnimationProgress = GUIShown ? 0f : 1f;
                    lastClickTime = Time.time;
                }
            }
        }

        private void UpdateGUIAnimation()
        {
            if (isGuiAnimating && animationsEnabled)
            {
                float target = GUIShown ? 1f : 0f;
                guiAnimationProgress = Mathf.SmoothStep(guiAnimationProgress, target, Time.deltaTime * guiAnimationSpeed);

                if (Mathf.Abs(guiAnimationProgress - target) < 0.01f)
                {
                    guiAnimationProgress = target;
                    isGuiAnimating = false;
                }
            }
            else
            {
                guiAnimationProgress = GUIShown ? 1f : 0f;
            }
            ApplyGUIAnimation();
        }

        private void ApplyGUIAnimation()
        {
            if (!animationsEnabled)
            {
                GUI.color = Color.white;
                return;
            }

            switch (currentAnimationType)
            {
                case AnimationType.Pop:
                    float scale = Mathf.SmoothStep(0f, 1f, guiAnimationProgress);
                    Vector2 pivot = new Vector2(windowRect.x + windowRect.width / 2, windowRect.y + windowRect.height / 2);
                    GUI.matrix = Matrix4x4.TRS(pivot, Quaternion.identity, Vector3.one * scale) *
                               Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one);
                    GUI.color = new Color(1f, 1f, 1f, guiAnimationProgress);
                    break;

                case AnimationType.Fade:
                    GUI.color = new Color(1f, 1f, 1f, guiAnimationProgress);
                    break;

                case AnimationType.Slide:
                    float slideOffset = Mathf.SmoothStep(-slideDistance, 0f, guiAnimationProgress);
                    GUI.matrix = Matrix4x4.TRS(new Vector3(slideOffset, 0, 0), Quaternion.identity, Vector3.one);
                    GUI.color = new Color(1f, 1f, 1f, guiAnimationProgress);
                    break;
            }
        }

        private void WindowFunction(int windowID)
        {
            DrawDottedStars();
            UpdateTabAnimation();

            GUI.color = new Color(1f, 1f, 1f, contentFade);
            DrawTabs();
            DrawTabContent();
            GUI.DragWindow();
        }

        private void UpdateTabAnimation()
        {
            if (previousTab != currentTab)
            {
                isAnimating = true;
                tabTransition = 0f;
                previousTab = currentTab;
            }

            if (isAnimating && animationsEnabled)
            {
                tabTransition = Mathf.SmoothStep(tabTransition, 1f, Time.deltaTime * transitionSpeed);
                contentOffset.x = Mathf.SmoothStep(-slideDistance, 0f, tabTransition);
                contentFade = tabTransition;
                if (tabTransition > 0.99f) isAnimating = false;
            }
            else
            {
                contentFade = 1f;
                contentOffset.x = 0f;
            }
        }

        private void DrawDottedStars()
        {
            int originalDepth = GUI.depth;
            GUI.depth = 1;
            GUI.color = new Color(0.1f, 0.1f, 0.2f, 0.8f);
            GUI.Box(new Rect(0, 0, windowRect.width, windowRect.height), "");

            GUIStyle starStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };

            for (int i = 0; i < stars.Length; i++)
            {
                Star star = stars[i];
                star.position.x -= star.speed * Time.deltaTime;
                if (star.position.x < 0)
                {
                    star.position.x = windowRect.width;
                    star.position.y = Random.value * windowRect.height;
                }
                stars[i] = star;

                starStyle.fontSize = (int)star.size;
                GUI.Label(new Rect(star.position.x, star.position.y, star.size, star.size), "•", starStyle);
            }
            GUI.depth = originalDepth;
        }

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();
            GUIStyle tabStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = animationsEnabled ? 12 + (int)(Mathf.Sin(Time.time) * 2) : 12
            };
            currentTab = GUILayout.Toolbar(currentTab, tabNames, tabStyle,
                GUILayout.Height(30 + (animationsEnabled ? Mathf.Sin(Time.time * 3f) * 5 : 0)));
            GUILayout.EndHorizontal();
        }

        private void DrawTabContent()
        {
            GUILayout.BeginVertical();
            GUI.BeginGroup(new Rect(contentOffset.x, 40, windowRect.width - 10, windowRect.height - 50));

            switch (currentTab)
            {
                case 0: Tab1Content(); break;
                case 1: DrawComputerTab(); break;
                case 2: DrawConsoleTab(); break;
                case 3: DrawSettingsTab(); break;
            }

            GUI.EndGroup();
            GUILayout.EndVertical();
        }

        private void DrawConsoleTab()
        {
            GUILayout.Label("Console Logs", GUILayout.Height(20));
            GUILayout.Space(10);
            scroll[2] = GUILayout.BeginScrollView(scroll[2],
                GUILayout.Height(windowRect.height - 100));

            foreach (string log in logs)
            {
                GUILayout.Label(log);
            }

            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Logs"))
            {
                while (File.Exists(filename))
                {
                    num++;
                    filename = $"IrisLogs{num}.txt";
                }
                File.WriteAllLines(filename, logs.ToArray());
            }

            if (GUILayout.Button("Clear Logs"))
            {
                logs.Clear();
            }
            GUILayout.EndHorizontal();
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            string message = $"[{DateTime.Now:HH:mm:ss}] ";
            switch (type)
            {
                case LogType.Log:
                    message += "<color=white>[Log] " + logString + "</color>";
                    break;
                case LogType.Exception:
                    message += "<color=red>[Exception] " + logString + "\n" + stackTrace + "</color>";
                    break;
                case LogType.Error:
                    message += "<color=red>[Error] " + logString + "</color>";
                    break;
                case LogType.Warning:
                    message += "<color=yellow>[Warning] " + logString + "</color>";
                    break;
            }

            logs.Add(message);
            if (logs.Count > MaxLogs)
            {
                logs.RemoveAt(0);
            }
        }
        private void Tab1Content()
        {
            float bounce = animationsEnabled ? Mathf.Sin(Time.time * 4f) : 0f;
            GUILayout.Space(10 + bounce);
            GUILayout.Label("Welcome to Modules",
                GUILayout.Height(20 + (animationsEnabled ? Mathf.Abs(Mathf.Sin(Time.time * 2f) * 10) : 0)));

            GUILayout.Space(20);

            if (animationsEnabled)
            {

            }

            GUI.SetNextControlName("");
            GUI.matrix = Matrix4x4.TRS(Vector2.zero, Quaternion.identity,
                Vector3.one);
            scroll[0] = GUILayout.BeginScrollView(scroll[0]);
            Comp(); Move(); Rig(); Vis(); Fun(); OP();
            GUILayout.EndScrollView();
            GUI.matrix = Matrix4x4.identity;
        }

        void Comp()
        {
            foreach (var btnInfo in Buttons.buttons)
            {
                if (btnInfo == Buttons.buttons[0] || btnInfo == Buttons.buttons[1] || btnInfo == Buttons.buttons[2] || btnInfo == Buttons.buttons[3] || btnInfo == Buttons.buttons[5] || btnInfo == Buttons.buttons[6] || btnInfo == Buttons.buttons[7] || btnInfo == Buttons.buttons[8] || btnInfo == Buttons.buttons[9] || btnInfo == Buttons.buttons[10] || btnInfo == Buttons.buttons[11]) continue;

                foreach (var info in btnInfo)
                {

                    if (string.IsNullOrEmpty(searchQuery) || info.buttonText.ToLower().Contains(searchQuery.ToLower()))
                    {
                        if (GUILayout.Button(info.buttonText))
                        {
                            info.enabled = !info.enabled;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        }
                    }
                }
            }
        }

        void Move()
        {
            foreach (var btnInfo in Buttons.buttons)
            {
                if (btnInfo == Buttons.buttons[0] || btnInfo == Buttons.buttons[1] || btnInfo == Buttons.buttons[2] || btnInfo == Buttons.buttons[3] || btnInfo == Buttons.buttons[4] || btnInfo == Buttons.buttons[9] || btnInfo == Buttons.buttons[6] || btnInfo == Buttons.buttons[7] || btnInfo == Buttons.buttons[8] || btnInfo == Buttons.buttons[10] || btnInfo == Buttons.buttons[11]) continue;

                foreach (var info in btnInfo)
                {
                    if (info.buttonText.StartsWith("<-"))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(searchQuery) || info.buttonText.ToLower().Contains(searchQuery.ToLower()))
                    {
                        if (GUILayout.Button(info.buttonText))
                        {
                            info.enabled = !info.enabled;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        }
                    }
                }
            }
        }

        void Rig()
        {
            foreach (var btnInfo in Buttons.buttons)
            {
                if (btnInfo == Buttons.buttons[0] || btnInfo == Buttons.buttons[1] || btnInfo == Buttons.buttons[2] || btnInfo == Buttons.buttons[3] || btnInfo == Buttons.buttons[4] || btnInfo == Buttons.buttons[5] || btnInfo == Buttons.buttons[7] || btnInfo == Buttons.buttons[8] || btnInfo == Buttons.buttons[9] || btnInfo == Buttons.buttons[10] || btnInfo == Buttons.buttons[11]) continue;

                foreach (var info in btnInfo)
                {
                    if (info.buttonText.StartsWith("<-"))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(searchQuery) || info.buttonText.ToLower().Contains(searchQuery.ToLower()))
                    {
                        if (GUILayout.Button(info.buttonText))
                        {
                            info.enabled = !info.enabled;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        }
                    }
                }
            }
        }

        void Vis()
        {
            foreach (var btnInfo in Buttons.buttons)
            {
                if (btnInfo == Buttons.buttons[0] || btnInfo == Buttons.buttons[1] || btnInfo == Buttons.buttons[2] || btnInfo == Buttons.buttons[3] || btnInfo == Buttons.buttons[4] || btnInfo == Buttons.buttons[5] || btnInfo == Buttons.buttons[6] || btnInfo == Buttons.buttons[9] || btnInfo == Buttons.buttons[8] || btnInfo == Buttons.buttons[10] || btnInfo == Buttons.buttons[11]) continue;

                foreach (var info in btnInfo)
                {
                    if (info.buttonText.StartsWith("<-"))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(searchQuery) || info.buttonText.ToLower().Contains(searchQuery.ToLower()))
                    {
                        if (GUILayout.Button(info.buttonText))
                        {
                            info.enabled = !info.enabled;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        }
                    }
                }
            }
        }

        void Fun()
        {
            foreach (var btnInfo in Buttons.buttons)
            {
                if (btnInfo == Buttons.buttons[0] || btnInfo == Buttons.buttons[1] || btnInfo == Buttons.buttons[2] || btnInfo == Buttons.buttons[3] || btnInfo == Buttons.buttons[4] || btnInfo == Buttons.buttons[5] || btnInfo == Buttons.buttons[6] || btnInfo == Buttons.buttons[7] || btnInfo == Buttons.buttons[9] || btnInfo == Buttons.buttons[10] || btnInfo == Buttons.buttons[11]) continue;

                foreach (var info in btnInfo)
                {
                    if (info.buttonText.StartsWith("<-"))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(searchQuery) || info.buttonText.ToLower().Contains(searchQuery.ToLower()))
                    {
                        if (GUILayout.Button(info.buttonText))
                        {
                            info.enabled = !info.enabled;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        }
                    }
                }
            }
        }
        void OP()
        {
            foreach (var btnInfo in Buttons.buttons)
            {
                if (btnInfo == Buttons.buttons[0] || btnInfo == Buttons.buttons[1] || btnInfo == Buttons.buttons[2] || btnInfo == Buttons.buttons[3] || btnInfo == Buttons.buttons[4] || btnInfo == Buttons.buttons[5] || btnInfo == Buttons.buttons[6] || btnInfo == Buttons.buttons[7] || btnInfo == Buttons.buttons[8] || btnInfo == Buttons.buttons[9] || btnInfo == Buttons.buttons[11]) continue;

                foreach (var info in btnInfo)
                {
                    if (info.buttonText.StartsWith("<-"))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(searchQuery) || info.buttonText.ToLower().Contains(searchQuery.ToLower()))
                    {
                        if (GUILayout.Button(info.buttonText))
                        {
                            info.enabled = !info.enabled;
                            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.25f);
                        }
                    }
                }
            }
        }

        private void DrawComputerTab()
        {
            GUI.color = new Color(1f, 1f, 1f, animationsEnabled ? 0.5f + Mathf.Sin(Time.time * 3f) * 0.5f : 1f);
            GUILayout.Label("Computer");
            GUI.color = Color.white;

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Input:");
            roomCode = GUILayout.TextField(roomCode);
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
            if (GUILayout.Button("Join Room", GUILayout.Width(100)))
            {
                if (!string.IsNullOrEmpty(roomCode) && PhotonNetworkController.Instance != null)
                {
                    PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomCode, JoinType.Solo);
                }
            }
        }

        private void DrawSettingsTab()
        {
            GUILayout.Label("Animation Settings", GUILayout.Height(30));
            GUILayout.Space(10);

            GUILayout.BeginVertical("Box");
            animationsEnabled = GUILayout.Toggle(animationsEnabled, "Enable Animations");
            GUI.enabled = animationsEnabled;

            DrawSlider("Transition Speed", ref transitionSpeed, 1f, 10f);
            DrawSlider("Slide Distance", ref slideDistance, 0f, 50f);
            DrawSlider("GUI Animation Speed", ref guiAnimationSpeed, 1f, 10f);

            GUILayout.Label("GUI Animation Type:");
            if (GUILayout.Button($"Current: {currentAnimationType}"))
            {
                currentAnimationType = (AnimationType)(((int)currentAnimationType + 1) % 3);
            }

            GUI.enabled = true;
            GUILayout.EndVertical();

            if (GUILayout.Button("Reset to Defaults"))
            {
                ResetSettings();
            }
        }

        private void DrawSlider(string label, ref float value, float min, float max)
        {
            GUILayout.Label($"{label}: {value:F1}");
            value = GUILayout.HorizontalSlider(value, min, max);
        }

        private void ResetSettings()
        {
            transitionSpeed = 5f;
            guiAnimationSpeed = 5f;
            slideDistance = 25f;
            animationsEnabled = true;
            currentAnimationType = AnimationType.Fade;
        }
    }
}