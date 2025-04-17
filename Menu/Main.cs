using BepInEx;
using HarmonyLib;
using Iris.Mods;
using IRIS.Menu;
using Photon.Pun;
using StupidTemplate.Classes;
using StupidTemplate.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static IRIS.Menu.Buttons;
using static StupidTemplate.Settings;

namespace StupidTemplate.Menu
{
    [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    public class Main : MonoBehaviour
    {
        // Constant
        public static void Prefix()
        {
            if (VaultLock.IsAuthenticated == true)
            {
                // Initialize Menu
                try
                {
                    bool toOpen = (!rightHanded && ControllerInputPoller.instance.leftControllerSecondaryButton) || (rightHanded && ControllerInputPoller.instance.rightControllerSecondaryButton);
                    bool keyboardOpen = UnityInput.Current.GetKey(keyboardButton);

                    if (menu == null)
                    {
                        if (toOpen || keyboardOpen)
                        {
                            CreateMenu();
                            RecenterMenu(rightHanded, keyboardOpen);
                            if (reference == null)
                            {
                                CreateReference(rightHanded);
                            }
                        }
                    }
                    else
                    {
                        if ((toOpen || keyboardOpen))
                        {
                            RecenterMenu(rightHanded, keyboardOpen);
                        }
                        else
                        {
                            GameObject.Find("Shoulder Camera").transform.Find("CM vcam1").gameObject.SetActive(true);

                            Rigidbody comp = menu.AddComponent(typeof(Rigidbody)) as Rigidbody;
                            if (rightHanded)
                            {
                                comp.velocity = GorillaLocomotion.GTPlayer.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                            }
                            else
                            {
                                comp.velocity = GorillaLocomotion.GTPlayer.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0);
                            }

                            UnityEngine.Object.Destroy(menu, 2);
                            menu = null;

                            UnityEngine.Object.Destroy(reference);
                            reference = null;
                        }
                    }
                }
                catch (Exception exc)
                {
                    UnityEngine.Debug.LogError(string.Format("{0} // Error initializing at {1}: {2}", PluginInfo.Name, exc.StackTrace, exc.Message));
                }

                // Constant
                try
                {
                    // Pre-Execution
                    if (fpsObject != null)
                    {
                        fpsObject.text = "FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
                    }

                    // Execute Enabled mods
                    foreach (ButtonInfo[] buttonlist in buttons)
                    {
                        foreach (ButtonInfo v in buttonlist)
                        {
                            if (v.enabled)
                            {
                                if (v.method != null)
                                {
                                    try
                                    {
                                        v.method.Invoke();
                                    }
                                    catch (Exception exc)
                                    {
                                        UnityEngine.Debug.LogError(string.Format("{0} // Error with mod {1} at {2}: {3}", PluginInfo.Name, v.buttonText, exc.StackTrace, exc.Message));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    UnityEngine.Debug.LogError(string.Format("{0} // Error with executing mods at {1}: {2}", PluginInfo.Name, exc.StackTrace, exc.Message));
                }
                try
                {
                    GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motd (1)").GetComponent<TextMeshPro>().text = "Iris.wtf";
                    TextMeshPro Nigg = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/motdtext").GetComponent<TextMeshPro>();
                    Nigg.text = "<color=#ff00ff>DEVELOPERS NOTICE</color>\n\nTHE OWNERS/DEVELOPERS OF THIS MENU ARE NOT RESPONSIBLE FOR ANY BANS CAUSED BY THIS MENU. CERTAIN FEATURES WILL NOT STAY UNDETECTED FOREVER.\n\n\n<color=#ff0000>USE AT YOUR OWN RISK IF THE GAME HAS RECENTLY UPDATED</color>";
                    GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConduct").GetComponent<TextMeshPro>().text = "ROOM INFO";
                    TextMeshPro Nigg2 = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COC Text").GetComponent<TextMeshPro>();
                    if (PhotonNetwork.InRoom)
                    {
                        string roomName = PhotonNetwork.CurrentRoom.Name.ToUpper();
                        string playerCount = PhotonNetwork.CurrentRoom.PlayerCount.ToString().ToUpper();
                        string maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers.ToString().ToUpper();
                        string ping = PhotonNetwork.GetPing().ToString().ToUpper();
                        string isMaster = PhotonNetwork.IsMasterClient ? "YES" : "NO";
                        string masterClient = PhotonNetwork.MasterClient.NickName.ToUpper();
                        Nigg2.text = $"\nIN ROOM: {roomName}\nPLAYERS: {playerCount}/{maxPlayers}\n" + $"PING: {ping}ms\nAM I MASTER CLIENT?: {isMaster}\nMASTER CLIENT: {masterClient}\n\nDEVELOPERS:\nFLYINGUNDERTHERADAR\nOUTCAST\nCHA554";
                    }
                    else
                    {
                        Nigg2.text = "\nNOT CONNECTED TO A ROOM\n\nDEVELOPERS:\nFLYINGUNDERTHERADAR\nOUTCAST\nCHA554";
                    }
                    Nigg2.alignment = TextAlignmentOptions.Top;

                    GameObject Screen = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/GorillaComputerObject/ComputerUI/monitor/monitorScreen");
                    if (Screen != null)
                    {
                        Renderer renderer = Screen.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material = Hello;
                            renderer.material.color = backgroundColor.colors[0].color;
                        }
                    }
                    GameObject Forest = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomBoundaryStones/BoundaryStoneSet_Forest/wallmonitorforestbg");
                    if (Forest != null)
                    {
                        Renderer renderer = Forest.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material = Hello;
                            renderer.material.color = backgroundColor.colors[0].color;
                        }
                    }

                    GameObject Keyboard = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/GorillaComputerObject/ComputerUI/keyboard (1)");
                    if (Keyboard != null)
                    {
                        Renderer renderer = Keyboard.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material = Hello;
                            renderer.material.color = backgroundColor.colors[0].color;
                        }
                    }
                    GameObject City = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomBoundaryStones/BoundaryStoneSet_City/Wallmonitor_Small_Prefab/wallmonitorscreen_small");
                    if (City != null)
                    {
                        Renderer renderer = City.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material = Hello;
                            renderer.material.color = backgroundColor.colors[0].color;
                        }
                    }
                }
                catch
                {
                }
            }
        }
        public static Material Hello = new Material(Shader.Find("GorillaTag/UberShader"));
        private static bool rightTriggerPressed = false;
        private static bool leftTriggerPressed = false;
        public static void NextPage()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f || UnityInput.Current.GetKeyDown(KeyCode.R) && !rightTriggerPressed)
            {
                Toggle("NextPage");
                rightTriggerPressed = true;
            }
            else if (ControllerInputPoller.instance.rightControllerIndexFloat <= 0.1f)
            {
                rightTriggerPressed = false;
            }
        }

        public static void PreviousPage()
        {
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f || UnityInput.Current.GetKeyDown(KeyCode.E) && !leftTriggerPressed)
            {
                Toggle("PreviousPage");
                leftTriggerPressed = true;
            }
            else if (ControllerInputPoller.instance.leftControllerIndexFloat <= 0.1f)
            {
                leftTriggerPressed = false;
            }
        }

        public static float TriggTime = 0f;
        public static float TriggCooldown = 0.4f;
        public static void Page()
        {
            if (Time.time >= TriggTime)
            {
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f || UnityInput.Current.GetKey(KeyCode.R))
                {
                    Main.Toggle("NextPage");
                    TriggTime = Time.time + TriggCooldown;
                }

                if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f || UnityInput.Current.GetKey(KeyCode.E))
                {
                    Main.Toggle("PreviousPage");
                    TriggTime = Time.time + TriggCooldown;
                }
            }
        }
        private static IEnumerator AnimateTitle(Text text)
        {
            string targetText = PluginInfo.Name;
            string currentText = "";

            while (true)
            {
                for (int i = 0; i <= targetText.Length; i++)
                {
                    currentText = targetText.Substring(0, i);
                    text.text = currentText;
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(0.7f);
                text.text = "";
                yield return new WaitForSeconds(0.100f);
            }
        }
        public static bool pingcounter = true;
        public static IEnumerator UpdatePingText(Text pingText)
        {
            while (pingcounter)
            {
                pingText.text = "Ping: " + PhotonNetwork.GetPing().ToString();
                yield return new WaitForSeconds(0.1f);
            }
        }
        public class CoroutineHandler : MonoBehaviour
        {
            public static CoroutineHandler instance;
            public static CoroutineHandler Instance
            {
                get
                {
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("CoroutineHandler");
                        obj.hideFlags = HideFlags.HideAndDontSave;
                        instance = obj.AddComponent<CoroutineHandler>();
                        GameObject.DontDestroyOnLoad(obj);
                    }
                    return instance;
                }
            }
        }

        [AddComponentMenu("UI/Effects/TextGradient")]
        public class UITextGradient : BaseMeshEffect
        {
            public Color topColor = new Color(0.1f, 0.3f, 0.6f); // Light cool blue
            public Color bottomColor = new Color(0f, 0.05f, 0.2f); // Dark cool blue

            public override void ModifyMesh(VertexHelper vh)
            {
                if (!IsActive())
                    return;

                List<UIVertex> vertices = new List<UIVertex>();
                vh.GetUIVertexStream(vertices);

                if (vertices.Count == 0)
                    return;

                // Find the bounds of the text
                float minY = float.MaxValue;
                float maxY = float.MinValue;
                foreach (var vertex in vertices)
                {
                    minY = Mathf.Min(minY, vertex.position.y);
                    maxY = Mathf.Max(maxY, vertex.position.y);
                }

                float height = maxY - minY;
                if (height <= 0)
                    return;

                // Apply gradient to vertices
                for (int i = 0; i < vertices.Count; i++)
                {
                    UIVertex vertex = vertices[i];
                    float t = (vertex.position.y - minY) / height;
                    vertex.color = Color.Lerp(bottomColor, topColor, t);
                    vertices[i] = vertex;
                }

                vh.Clear();
                vh.AddUIVertexTriangleStream(vertices);
            }
        }
        
        public static void RoundObj(GameObject toRound) // yea its iis so what, fuck you gonna do your still gonna skid it anyway
        {
            float Bevel = 0.02f;

            Renderer ToRoundRenderer = toRound.GetComponent<Renderer>();
            GameObject BaseA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            BaseA.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            UnityEngine.Object.Destroy(BaseA.GetComponent<Collider>());

            BaseA.transform.parent = menu.transform;
            BaseA.transform.rotation = Quaternion.identity;
            BaseA.transform.localPosition = toRound.transform.localPosition;
            BaseA.transform.localScale = toRound.transform.localScale + new Vector3(0f, Bevel * -2.55f, 0f);

            GameObject BaseB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            BaseB.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            UnityEngine.Object.Destroy(BaseB.GetComponent<Collider>());

            BaseB.transform.parent = menu.transform;
            BaseB.transform.rotation = Quaternion.identity;
            BaseB.transform.localPosition = toRound.transform.localPosition;
            BaseB.transform.localScale = toRound.transform.localScale + new Vector3(0f, 0f, -Bevel * 2f);

            GameObject RoundCornerA = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            RoundCornerA.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            UnityEngine.Object.Destroy(RoundCornerA.GetComponent<Collider>());

            RoundCornerA.transform.parent = menu.transform;
            RoundCornerA.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 0f, 90f);

            RoundCornerA.transform.localPosition = toRound.transform.localPosition + new Vector3(0f, (toRound.transform.localScale.y / 2f) - (Bevel * 1.275f), (toRound.transform.localScale.z / 2f) - Bevel);
            RoundCornerA.transform.localScale = new Vector3(Bevel * 2.55f, toRound.transform.localScale.x / 2f, Bevel * 2f);

            GameObject RoundCornerB = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            RoundCornerB.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            UnityEngine.Object.Destroy(RoundCornerB.GetComponent<Collider>());

            RoundCornerB.transform.parent = menu.transform;
            RoundCornerB.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 0f, 90f);

            RoundCornerB.transform.localPosition = toRound.transform.localPosition + new Vector3(0f, -(toRound.transform.localScale.y / 2f) + (Bevel * 1.275f), (toRound.transform.localScale.z / 2f) - Bevel);
            RoundCornerB.transform.localScale = new Vector3(Bevel * 2.55f, toRound.transform.localScale.x / 2f, Bevel * 2f);

            GameObject RoundCornerC = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            RoundCornerC.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            UnityEngine.Object.Destroy(RoundCornerC.GetComponent<Collider>());

            RoundCornerC.transform.parent = menu.transform;
            RoundCornerC.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 0f, 90f);

            RoundCornerC.transform.localPosition = toRound.transform.localPosition + new Vector3(0f, (toRound.transform.localScale.y / 2f) - (Bevel * 1.275f), -(toRound.transform.localScale.z / 2f) + Bevel);
            RoundCornerC.transform.localScale = new Vector3(Bevel * 2.55f, toRound.transform.localScale.x / 2f, Bevel * 2f);

            GameObject RoundCornerD = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            RoundCornerD.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            UnityEngine.Object.Destroy(RoundCornerD.GetComponent<Collider>());

            RoundCornerD.transform.parent = menu.transform;
            RoundCornerD.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 0f, 90f);

            RoundCornerD.transform.localPosition = toRound.transform.localPosition + new Vector3(0f, -(toRound.transform.localScale.y / 2f) + (Bevel * 1.275f), -(toRound.transform.localScale.z / 2f) + Bevel);
            RoundCornerD.transform.localScale = new Vector3(Bevel * 2.55f, toRound.transform.localScale.x / 2f, Bevel * 2f);

            GameObject[] ToChange = new GameObject[]
            {
                BaseA,
                BaseB,
                RoundCornerA,
                RoundCornerB,
                RoundCornerC,
                RoundCornerD
            };

            foreach (GameObject Changed in ToChange)
            {
                ClampColor TargetChanger = Changed.AddComponent<ClampColor>();
                TargetChanger.targetRenderer = ToRoundRenderer;

                TargetChanger.Start();
            }

            ToRoundRenderer.enabled = false;
        }
        public class ClampColor : MonoBehaviour
        {
            public void Start()
            {
                gameObjectRenderer = GetComponent<Renderer>();
                Update();
            }

            public void Update()
            {
                gameObjectRenderer.material.color = targetRenderer.material.color;
            }

            public Renderer gameObjectRenderer;
            public Renderer targetRenderer;
        }
        public static void CreateMenu()
        {

            // Menu Holder
            menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
            menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);

            // Menu Background
            menuBackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(menuBackground.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(menuBackground.GetComponent<BoxCollider>());
            menuBackground.transform.parent = menu.transform;
            menuBackground.transform.rotation = Quaternion.identity;
            menuBackground.transform.localScale = menuSize;
            menuBackground.transform.position = new Vector3(0.05f, 0f, 0f);
            RoundObj(menuBackground);
            ColorChanger colorChanger = menuBackground.AddComponent<ColorChanger>();
            colorChanger.colorInfo = backgroundColor;
            colorChanger.Start();

            // Canvas
            canvasObject = new GameObject();
            canvasObject.transform.parent = menu.transform;
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvasScaler.dynamicPixelsPerUnit = 1000f;

            // Title and FPS
            Text text = new GameObject
            {
                transform =
                        {
                            parent = canvasObject.transform
                        }
            }.AddComponent<Text>();
            text.font = currentFont;
            text.text = "";
            text.fontSize = 1;
            text.color = textColors[0];
            text.supportRichText = true;
            text.fontStyle = FontStyle.Normal;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.3f, 0.07f);
            component.position = new Vector3(0.06f, 0f, 0.145f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            CoroutineHandler.Instance.StartCoroutine(AnimateTitle(text));
            fpsObject = new GameObject
            {
                transform =
                    {
                        parent = canvasObject.transform
                        }
            }.AddComponent<Text>();
            fpsObject.font = currentFont;
            fpsObject.text = "FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
            fpsObject.color = textColors[0];
            fpsObject.fontSize = 1;
            fpsObject.supportRichText = true;
            fpsObject.fontStyle = FontStyle.Italic;
            fpsObject.alignment = TextAnchor.MiddleCenter;
            fpsObject.horizontalOverflow = UnityEngine.HorizontalWrapMode.Overflow;
            fpsObject.resizeTextForBestFit = true;
            fpsObject.resizeTextMinSize = 0;
            RectTransform component2 = fpsObject.GetComponent<RectTransform>();
            component2.localPosition = Vector3.zero;
            component2.sizeDelta = new Vector2(0.09f, 0.01f);
            component2.position = new Vector3(0.06f, 0.12f, -0.185f);
            component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            Text text1 = new GameObject
            {
                transform =
        {
            parent = canvasObject.transform
        }
            }.AddComponent<Text>();

            text1.font = currentFont;
            text1.fontSize = 1;
            text1.color = textColors[0];
            text1.supportRichText = true;
            text1.fontStyle = FontStyle.Italic;
            text1.alignment = TextAnchor.MiddleCenter;
            text1.resizeTextForBestFit = true;
            text1.resizeTextMinSize = 0;
            text1.text = "Ping: " + PhotonNetwork.GetPing().ToString();

            RectTransform component22 = text1.GetComponent<RectTransform>();
            component22.localPosition = Vector3.zero;
            component22.sizeDelta = new Vector2(0.09f, 0.01f);
            component22.position = new Vector3(0.06f, 0.08f, -0.185f);
            component22.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            CoroutineHandler.Instance.StartCoroutine(UpdatePingText(text1));
            if (buttonsType == 1  || buttonsType == 2 || buttonsType == 3 || buttonsType == 4 || buttonsType == 5 || buttonsType == 6 || buttonsType == 7 || buttonsType == 8 || buttonsType == 9 | buttonsType == 10 || buttonsType == 11 || buttonsType == 12 || buttonsType == 13)
            {
                GameObject disconnectbutton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (!UnityInput.Current.GetKey(KeyCode.Q))
                {
                    disconnectbutton.layer = 2;
                }
                UnityEngine.Object.Destroy(disconnectbutton.GetComponent<Rigidbody>());
                disconnectbutton.GetComponent<BoxCollider>().isTrigger = true;
                disconnectbutton.transform.parent = menu.transform;
                disconnectbutton.transform.rotation = Quaternion.identity;
                disconnectbutton.transform.localScale = new Vector3(0.09f, 0.9f, 0.08f);
                disconnectbutton.transform.localPosition = new Vector3(0.56f, 0f, 0.499f);
                disconnectbutton.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                disconnectbutton.AddComponent<Classes.Button>().relatedText = "Home";
                RoundObj(disconnectbutton);
                colorChanger = disconnectbutton.AddComponent<ColorChanger>();
                colorChanger.colorInfo = buttonColors[0];
                colorChanger.Start();

                Text discontext = new GameObject
                {
                    transform =
                            {
                                parent = canvasObject.transform
                            }
                }.AddComponent<Text>();
                discontext.text = "Home";
                discontext.font = currentFont;
                discontext.fontSize = 1;
                discontext.color = textColors[0];
                discontext.alignment = TextAnchor.MiddleCenter;
                discontext.resizeTextForBestFit = true;
                discontext.resizeTextMinSize = 0;

                RectTransform rectt = discontext.GetComponent<RectTransform>();
                rectt.localPosition = Vector3.zero;
                rectt.sizeDelta = new Vector2(0.2f, 0.03f);
                rectt.localPosition = new Vector3(0.064f, 0f, 0.19f);
                rectt.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }
            ButtonInfo[] activeButtons = buttons[buttonsType].Skip(pageNumber * buttonsPerPage).Take(buttonsPerPage).ToArray();
            for (int i = 0; i < activeButtons.Length; i++)
            {
                CreateButton(i * 0.1f, activeButtons[i]);
            }
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.parent = menu.transform;
            sphere.transform.localScale = new Vector3(-0.2f, -0.2f, -0.2f);
            sphere.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
            Destroy(sphere.GetComponent<Collider>());
            sphere.AddComponent<thspherethatorbnits>();
            TrailRenderer trail = sphere.AddComponent<TrailRenderer>();
            trail.time = 1f;
            trail.startWidth = 0.05f;
            trail.endWidth = 0f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            trail.startColor = backgroundColor.colors[0].color;
            trail.endColor = backgroundColor.colors[0].color;
        }
    

        public static float orbitSpeed = 1f;
        public static float orbitRadius = 1f;
        public static float angle = 0f;
        public static Vector3 orbitAxis;
        public class thspherethatorbnits : MonoBehaviour
        {
            void Start()
            {
                orbitAxis = UnityEngine.Random.onUnitSphere;
            }

            void Update()
            {
                Vector3 positionInOrbitPlane = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * orbitRadius;
                transform.localPosition = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, orbitAxis) * positionInOrbitPlane;
                angle += orbitSpeed * Time.deltaTime;

                if (angle > 2 * Mathf.PI)
                {
                    angle -= 2 * Mathf.PI;
                }
            }
        }

        public static void CreateButton(float offset, ButtonInfo method)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (!UnityInput.Current.GetKey(KeyCode.Q))
            {
                gameObject.layer = 2;
            }
            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.09f, 0.9f, 0.125f);
            gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.24f - offset * 1.5f);
            gameObject.AddComponent<Classes.Button>().relatedText = method.buttonText;
            RoundObj(gameObject);
            ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
            if (method.enabled)
            {
                colorChanger.colorInfo = buttonColors[1];
            }
            else
            {
                colorChanger.colorInfo = buttonColors[0];
            }
            colorChanger.Start();

            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            text.font = currentFont;
            text.text = method.buttonText;
            if (method.overlapText != null)
            {
                text.text = method.overlapText;
            }
            text.supportRichText = true;
            text.fontSize = 1;
            if (method.enabled)
            {
                text.color = textColors[1];
            }
            else
            {
                text.color = textColors[0];
            }
            text.alignment = TextAnchor.MiddleCenter;
            text.fontStyle = FontStyle.Normal;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(.2f, .03f);
            component.localPosition = new Vector3(.064f, 0, .095f - offset / 1.75f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        public static void RecreateMenu()
        {
            if (menu != null)
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;

                CreateMenu();
                RecenterMenu(rightHanded, UnityInput.Current.GetKey(keyboardButton));
            }
        }

        public static void RecenterMenu(bool isRightHanded, bool isKeyboardCondition)
        {
            if (!isKeyboardCondition)
            {
                if (!isRightHanded)
                {
                    menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                }
                else
                {
                    menu.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    Vector3 rotation = GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles;
                    rotation += new Vector3(0f, 0f, 180f);
                    menu.transform.rotation = Quaternion.Euler(rotation);
                }
            }
            else
            {
                try
                {
                    TPC = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera").GetComponent<Camera>();
                }
                catch { }

                GameObject.Find("Shoulder Camera").transform.Find("CM vcam1").gameObject.SetActive(false);

                if (TPC != null)
                {
                    TPC.transform.position = new Vector3(-62.2708f, 8.0385f, -65.579f);
                    TPC.transform.rotation = Quaternion.identity;
                    menu.transform.parent = TPC.transform;
                    menu.transform.position = (TPC.transform.position + (Vector3.Scale(TPC.transform.forward, new Vector3(0.5f, 0.5f, 0.5f)))) + (Vector3.Scale(TPC.transform.up, new Vector3(-0.02f, -0.02f, -0.02f)));
                    Vector3 rot = TPC.transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x - 90, rot.y + 90, rot.z);
                    menu.transform.rotation = Quaternion.Euler(rot);

                    if (reference != null)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            Ray ray = TPC.ScreenPointToRay(Mouse.current.position.ReadValue());
                            RaycastHit hit;
                            bool worked = Physics.Raycast(ray, out hit, 100);
                            if (worked)
                            {
                                Classes.Button collide = hit.transform.gameObject.GetComponent<Classes.Button>();
                                if (collide != null)
                                {
                                    collide.OnTriggerEnter(buttonCollider);
                                }
                            }
                        }
                        else
                        {
                            reference.transform.position = new Vector3(999f, -999f, -999f);
                        }
                    }
                }
            }
        }

        public static void CreateReference(bool isRightHanded)
        {
            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (isRightHanded)
            {
                reference.transform.parent = GorillaTagger.Instance.leftHandTransform;
            }
            else
            {
                reference.transform.parent = GorillaTagger.Instance.rightHandTransform;
            }
            reference.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
            reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            buttonCollider = reference.GetComponent<SphereCollider>();

            ColorChanger colorChanger = reference.AddComponent<ColorChanger>();
            colorChanger.colorInfo = backgroundColor;
            colorChanger.Start();
        }

        public static void Toggle(string buttonText)
        {
            int lastPage = ((buttons[buttonsType].Length + buttonsPerPage - 1) / buttonsPerPage) - 1;
            if (buttonText == "PreviousPage")
            {
                pageNumber--;
                if (pageNumber < 0)
                {
                    pageNumber = lastPage;
                }
            }
            else
            {
                if (buttonText == "NextPage")
                {
                    pageNumber++;
                    if (pageNumber > lastPage)
                    {
                        pageNumber = 0;
                    }
                }
                else
                {
                    ButtonInfo target = GetIndex(buttonText);
                    if (target != null)
                    {
                        if (target.isTogglable)
                        {
                            target.enabled = !target.enabled;
                            if (target.enabled)
                            {
                                if (target.enableMethod != null)
                                {
                                    try { target.enableMethod.Invoke(); } catch { }
                                }
                            }
                            else
                            {
                                if (target.disableMethod != null)
                                {
                                    try { target.disableMethod.Invoke(); } catch { }
                                }
                            }
                        }
                        else
                        {
                            if (target.method != null)
                            {
                                try { target.method.Invoke(); } catch { }
                            }
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError(buttonText + " does not exist");
                    }
                }
                if(buttonText == "Home")
                {
                    Global.ReturnHome();
                }
            }
            RecreateMenu();
        }

        public static GradientColorKey[] GetSolidGradient(Color color)
        {
            return new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
        }

        public static ButtonInfo GetIndex(string buttonText)
        {
            foreach (ButtonInfo[] buttons in Buttons.buttons)
            {
                foreach (ButtonInfo button in buttons)
                {
                    if (button.buttonText == buttonText)
                    {
                        return button;
                    }
                }
            }

            return null;
        }

        // Variables
        // Important
        // Objects
        public static GameObject menu;
        public static GameObject menuBackground;
        public static GameObject reference;
        public static GameObject canvasObject;

        public static SphereCollider buttonCollider;
        public static Camera TPC;
        public static Text fpsObject;

        // Data
        public static int pageNumber = 0;
        public static int buttonsType = 0;
    }
}
