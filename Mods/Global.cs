using System.Reflection;
using System;
using BepInEx;
using GunLib;
using Photon.Pun;
using StupidTemplate.Classes;
using StupidTemplate.Menu;
using StupidTemplate.Notifications;
using UnityEngine;
using Valve.VR;
using static StupidTemplate.Menu.Main;
using Iris.Classes;
using UnityEngine.InputSystem;
using Iris.Menu;
using StupidTemplate;

namespace Iris.Mods
{
    internal class Global
    {
        public static void ReturnHome()
        {
            buttonsType = 0;
            pageNumber = 0;
        }
        public static void LPD()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton || UnityInput.Current.GetKey(KeyCode.Backspace))
            {
                PhotonNetwork.Disconnect();
            }
        }

        public static void DisableNetworkTriggers()
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab").SetActive(false);
        }

        public static void EnableNetworkTriggers()
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab").SetActive(true);
        }

        public static void DisableMapTriggers()
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab").SetActive(false);
        }

        public static void EnableMapTriggers()
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab").SetActive(true);
        }

        public static void DisableQuitBox()
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/QuitBox").SetActive(false);
        }

        public static void EnableQuitBox()
        {
            GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/QuitBox").SetActive(true);
        }

        public static bool rp = false;
        public static bool lp = false;

        public static GameObject rightPlat;

        public static GameObject leftPlat;

        public static void PlatformsbyCha()
        {
            {
                if (ControllerInputPoller.instance.rightGrab)
                {
                    if (!rp)
                    {
                        rightPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        rightPlat.transform.localScale = PlatSize;//new Vector3(0.01f, 0.3f, 0.4f); // Depth, Width, Height
                        rightPlat.GetComponent<Renderer>().material.color = Color.gray;//GetPlatformColor(0f);//new Color32(0, 198, 255, 255);
                        rightPlat.transform.position = GorillaTagger.Instance.rightHandTransform.position - Vector3.up * 0.045f;
                        rightPlat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;


                        rp = true;
                    }
                }
                else
                {
                    GameObject.Destroy(rightPlat);
                    rp = false;
                }

                if (ControllerInputPoller.instance.leftGrab)
                {
                    if (!lp)
                    {
                        leftPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        leftPlat.transform.localScale = PlatSize;//new Vector3(0.01f, 0.3f, 0.4f);// Depth, Width, Height
                        leftPlat.GetComponent<Renderer>().material.color = Color.gray;//GetPlatformColor(0f);//new Color32(0, 198, 255, 255);
                        leftPlat.transform.position = GorillaTagger.Instance.leftHandTransform.position - Vector3.up * 0.045f;
                        leftPlat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;


                        lp = true;
                    }
                }
                else
                {
                    GameObject.Destroy(leftPlat);

                    lp = false;
                }
            }
        }

        public static int PlatIndex = 2;
        private static Vector3 PlatSize;

        [Obsolete]
        public static void ChangePlatformSize()
        {
            PlatIndex++;
            if (PlatIndex > 4)
            {
                PlatIndex = 0;
            }
            string[] names = new string[]
            {
                "Small",
                "Normal",
                "Medium",
                "Large",
                "Massive!?"
            };
            Vector3[] distances = new Vector3[]
            {
                new Vector3(0.01f, 0.1f, 0.2f),//Small
                new Vector3(0.01f, 0.3f, 0.4f),//Normal
                new Vector3(0.01f, 0.45f, 0.55f),//Medium
                new Vector3(0.01f, 0.6f, 0.7f),//Large
                new Vector3(0.01f, 0.9f, 0.99f)//XL

            };

            PlatSize = distances[PlatIndex];
            Main.GetIndex("Change Platform Size").overlapText = "Change Platform Size <color=grey>[</color><color=green>" + names[PlatIndex] + "</color><color=grey>]</color>";
        }

        public static void PlatformsbyChaTrigger()
        {
            {
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
                {
                    if (!rp)
                    {
                        rightPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        rightPlat.transform.localScale = PlatSize; // Depth, Width, Height
                        rightPlat.GetComponent<Renderer>().material.color = Color.gray;//GetPlatformColor(0f);//new Color32(0, 198, 255, 255);
                        rightPlat.transform.position = GorillaTagger.Instance.rightHandTransform.position - Vector3.up * 0.045f;
                        rightPlat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;

                        rp = true;
                    }
                }
                else
                {
                    GameObject.Destroy(rightPlat);
                    rp = false;
                }

                if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f)
                {
                    if (!lp)
                    {
                        leftPlat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        leftPlat.transform.localScale = PlatSize;// Depth, Width, Height
                        leftPlat.GetComponent<Renderer>().material.color = Color.gray;//GetPlatformColor(0f);//new Color32(0, 198, 255, 255);
                        leftPlat.transform.position = GorillaTagger.Instance.leftHandTransform.position - Vector3.up * 0.045f;
                        leftPlat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;

                        lp = true;
                    }
                }
                else
                {
                    GameObject.Destroy(leftPlat);

                    lp = false;
                }
            }
        }
        public static GameObject leftplat = null;
        public static GameObject rightplat = null;

        public static void TogglePlatforms()
        {
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f && ControllerInputPoller.instance.leftGrab)
            {
                if (leftplat == null)
                {
                    leftplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftplat.transform.localScale = PlatSize;//new Vector3(0.01f, 0.3f, 0.3f);
                    leftplat.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    leftplat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    leftplat.GetComponent<Renderer>().material.color = Settings.backgroundColor.colors[1].color;//Color.magenta;
                }
            }
            else
            {
                if (leftplat != null)
                {
                    UnityEngine.Object.Destroy(leftplat);
                    leftplat = null;
                }
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f && ControllerInputPoller.instance.rightGrab)
            {
                if (rightplat == null)
                {
                    rightplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightplat.transform.localScale = PlatSize;//new Vector3(0.01f, 0.3f, 0.3f);
                    rightplat.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    rightplat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    rightplat.GetComponent<Renderer>().material.color = Settings.backgroundColor.colors[1].color;//Color.magenta;
                }
            }
            else
            {
                if (rightplat != null)
                {
                    UnityEngine.Object.Destroy(rightplat);
                    rightplat = null;
                }
            }
        }



        public static void Fly()
        {
            {
                if (ControllerInputPoller.instance.rightControllerPrimaryButton)
                {
                    GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * FlySpeed;
                    GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
        }
        public static void TriggerFly1()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat == 1f)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * FlySpeed;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;

            }
        }
        public static void JoystickFly1()
        {
            Vector2 joy = ControllerInputPoller.instance.rightControllerPrimary2DAxis;

            if (Mathf.Abs(joy.x) > 0.3 || Mathf.Abs(joy.y) > 0.3)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += (GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * (joy.y * FlySpeed)) + (GorillaTagger.Instance.headCollider.transform.right * Time.deltaTime * (joy.x * FlySpeed));
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;

            }
        }
        public static void BarkFly1()
        {
            {
                ZeroG();
                var rb = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody;
                Vector2 xz = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
                float y = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis.y;

                Vector3 inputDirection = new Vector3(xz.x, y, xz.y);
                var playerForward = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.forward;
                playerForward.y = 0;
                var playerRight = GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.right;
                playerRight.y = 0;

                var velocity = inputDirection.x * playerRight + y * Vector3.up + inputDirection.z * playerForward;
                velocity *= GorillaLocomotion.GTPlayer.Instance.scale * FlySpeed;
                rb.velocity = Vector3.Lerp(rb.velocity, velocity, 0.12875f);
            }

        }
        public static void ZeroG()
        {
            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);

        }


        public static void SlingFly1()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * (20 * 2);

            }
        }

        public static void ZeroGravitySlingshotFly1()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                ZeroG();
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * 25;

            }
        }

        public static int FlySpeedIndex = 0;
        private static float FlySpeed = 5f;


        public static void ChangeFlySpeed()
        {
            FlySpeedIndex++;
            if (FlySpeedIndex > 4)
            {
                FlySpeedIndex = 0;
            }
            string[] names = new string[]
            {
                "Slow",
                "Normal",
                "Fast",
                "Super Fast",
                "Max"
            };
            float[] distances = new float[]
            {
                5f,
                10f,
                15f,
                25f,
                60f
            };

            FlySpeed = distances[FlySpeedIndex];
            Main.GetIndex("Change Fly Speed").overlapText = "Change Fly Speed <color=grey>[</color><color=green>" + names[FlySpeedIndex] + "</color><color=grey>]</color>";
        }

        public static void IronMan()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(15 * -GorillaTagger.Instance.leftHandTransform.right, ForceMode.Acceleration);
                GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tapHapticStrength / 50f * GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity.magnitude, GorillaTagger.Instance.tapHapticDuration);
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(15 * GorillaTagger.Instance.rightHandTransform.right, ForceMode.Acceleration);
                GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 50f * GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity.magnitude, GorillaTagger.Instance.tapHapticDuration);
            }
        }

        public static void UpAndDown()
        {
            if ((ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f) || ControllerInputPoller.instance.rightGrab)
            {
                ZeroG();
            }
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.up * Time.deltaTime * 15 * 3f;
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += Vector3.up * Time.deltaTime * 15 * -3f;
            }
        }

        public static void LeftAndRight()
        {
            if ((ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f) || ControllerInputPoller.instance.rightGrab)
            {
                ZeroG();
            }
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.bodyCollider.transform.right * Time.deltaTime * 15 * -3f;
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.bodyCollider.transform.right * Time.deltaTime * 15 * 3f;
            }
        }

        public static void ForwardsAndBackwards()
        {
            if ((ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f) || ControllerInputPoller.instance.rightGrab)
            {
                ZeroG();
            }
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.bodyCollider.transform.forward * Time.deltaTime * 15 * 3f;
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity += GorillaTagger.Instance.bodyCollider.transform.forward * Time.deltaTime * 15 * -3f;
            }
        }


        public static void NoRespawnBug1()
        {
            GameObject.Find("Floating Bug Holdable").GetComponent<ThrowableBug>().maxDistanceFromOriginBeforeRespawn = float.MaxValue;
            GameObject.Find("Floating Bug Holdable").GetComponent<ThrowableBug>().maxDistanceFromTargetPlayerBeforeRespawn = float.MaxValue;
        }

        public static void PleaseRespawnBug1()
        {
            GameObject.Find("Floating Bug Holdable").GetComponent<ThrowableBug>().maxDistanceFromOriginBeforeRespawn = 50f;
            GameObject.Find("Floating Bug Holdable").GetComponent<ThrowableBug>().maxDistanceFromTargetPlayerBeforeRespawn = 50f;
        }

        public static void NoRespawnBat1()
        {
            GameObject.Find("Cave Bat Holdable").GetComponent<ThrowableBug>().maxDistanceFromOriginBeforeRespawn = float.MaxValue;
            GameObject.Find("Cave Bat Holdable").GetComponent<ThrowableBug>().maxDistanceFromTargetPlayerBeforeRespawn = float.MaxValue;
        }

        public static void PleaseRespawnBat1()
        {
            GameObject.Find("Cave Bat Holdable").GetComponent<ThrowableBug>().maxDistanceFromOriginBeforeRespawn = 50f;
            GameObject.Find("Cave Bat Holdable").GetComponent<ThrowableBug>().maxDistanceFromTargetPlayerBeforeRespawn = 50f;
        }

        public static float lastTime = 0f;

        public static void GrabBug1()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GameObject.Find("Floating Bug Holdable").transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
        }

        public static void GrabBat1()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GameObject.Find("Cave Bat Holdable").transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
        }



        public static void DestroyBug1()
        {
            GameObject.Find("Floating Bug Holdable").transform.position = new Vector3(99999f, 99999f, 99999f);
        }

        public static void DestroyBat1()
        {
            GameObject.Find("Cave Bat Holdable").transform.position = new Vector3(99999f, 99999f, 99999f);
        }


        public static void SpazBug1()
        {
            GameObject.Find("Floating Bug Holdable").transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
        }

        public static void SpazBat1()
        {
            GameObject.Find("Cave Bat Holdable").transform.rotation = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
        }



        public static void BugHalo1()
        {
            float offset = 0;
            GameObject.Find("Floating Bug Holdable").transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(Mathf.Cos(offset + ((float)Time.frameCount / 30)), 2, Mathf.Sin(offset + ((float)Time.frameCount / 30)));
        }

        public static void BatHalo1()
        {
            float offset = 360f / 3f;
            GameObject.Find("Cave Bat Holdable").transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(Mathf.Cos(offset + ((float)Time.frameCount / 30)), 2, Mathf.Sin(offset + ((float)Time.frameCount / 30)));
        }



        public static void RideBug1()
        {
            GorillaTagger.Instance.rigidbody.transform.position = GameObject.Find("Floating Bug Holdable").transform.position;
            GorillaTagger.Instance.rigidbody.velocity = Vector3.zero;
        }

        public static void RideBat1()
        {
            GorillaTagger.Instance.rigidbody.transform.position = GameObject.Find("Cave Bat Holdable").transform.position;
            GorillaTagger.Instance.rigidbody.velocity = Vector3.zero;
        }



        public static void BreakBug1()
        {
            GameObject.Find("Floating Bug Holdable").GetComponent<ThrowableBug>().allowPlayerStealing = false;
        }

        public static void BreakBat1()
        {
            GameObject.Find("Cave Bat Holdable").GetComponent<ThrowableBug>().allowPlayerStealing = false;
        }

        public static void FixBug1()
        {
            GameObject.Find("Floating Bug Holdable").GetComponent<ThrowableBug>().allowPlayerStealing = false;
        }

        public static void FixBat1()
        {
            GameObject.Find("Cave Bat Holdable").GetComponent<ThrowableBug>().allowPlayerStealing = false;
        }

        public static void StealBug1()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GameObject.Find("Floating Bug Holdable").GetComponent<ThrowableBug>().WorldShareableRequestOwnership();
                ThrowableBug bug = GameObject.Find("Floating Bug Holdable").GetComponent<ThrowableBug>();
                bug.currentState = TransferrableObject.PositionState.Dropped;
                System.Type type = bug.GetType();
                FieldInfo fieldInfo = type.GetField("locked", BindingFlags.NonPublic | BindingFlags.Instance);
                fieldInfo.SetValue(bug, false);
                GameObject.Find("Floating Bug Holdable").transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
        }

        public static void StealBat1()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GameObject.Find("Cave Bat Holdable").GetComponent<ThrowableBug>().WorldShareableRequestOwnership();
                ThrowableBug bug = GameObject.Find("Cave Bat Holdable").GetComponent<ThrowableBug>();
                bug.currentState = TransferrableObject.PositionState.Dropped;
                System.Type type = bug.GetType();
                FieldInfo fieldInfo = type.GetField("locked", BindingFlags.NonPublic | BindingFlags.Instance);
                fieldInfo.SetValue(bug, false);
                GameObject.Find("Cave Bat Holdable").transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
        }

        public static void BatSizeChanger()
        {

            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GameObject.Find("Cave Bat Holdable").transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                GameObject.Find("Cave Bat Holdable").transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }

            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject.Find("Cave Bat Holdable").transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
        }

        public static void BugTraces()
        {
            GameObject gameObject = GameObject.Find("Floating Bug Holdable");
            GameObject gameObject2 = GameObject.CreatePrimitive((PrimitiveType)2);
            UnityEngine.Object.Destroy(gameObject2.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Collider>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<MeshCollider>());
            gameObject2.transform.rotation = Quaternion.identity;
            gameObject2.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            gameObject2.transform.position = gameObject.transform.position;
            Global.UpdateScaleForBeacons(GorillaTagger.Instance.rightHandTransform.gameObject, gameObject.gameObject, gameObject2);
            Renderer component = gameObject2.GetComponent<Renderer>();
            component.material.color = Color.Lerp(new Color(0f, 1f, 0f, 0.5f), new Color(0f, 1f, 1f, 0.5f), Mathf.PingPong(Time.time, 1f));
            UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
        }
        public static void BatTraces()
        {
            GameObject gameObject = GameObject.Find("Cave Bat Holdable");
            GameObject gameObject2 = GameObject.CreatePrimitive((PrimitiveType)2);
            UnityEngine.Object.Destroy(gameObject2.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Collider>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<MeshCollider>());
            gameObject2.transform.rotation = Quaternion.identity;
            gameObject2.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            gameObject2.transform.position = gameObject.transform.position;
            Global.UpdateScaleForBeacons(GorillaTagger.Instance.rightHandTransform.gameObject, gameObject.gameObject, gameObject2);
            Renderer component = gameObject2.GetComponent<Renderer>();
            component.material.color = Color.Lerp(new Color(0f, 1f, 0f, 0.5f), new Color(0f, 1f, 1f, 0.5f), Mathf.PingPong(Time.time, 1f));
            UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
        }

        public static void BugSizeChanger()
        {

            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GameObject.Find("Floating Bug Holdable").transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                GameObject.Find("Floating Bug Holdable").transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }

            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject.Find("Floating Bug Holdable").transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
        public static void SeizureShark()
        {
            GameObject gameObject = GameObject.Find("Swimming Shark prefab");
            ThrowableBug component = GameObject.Find("Swimming Shark prefab").GetComponent<ThrowableBug>();

            if (component.IsMyItem())
            {
                if (ControllerInputPoller.instance.rightGrab)
                {
                    float num = 500f;
                    gameObject.transform.Rotate(Vector3.up, num * Time.deltaTime);
                    gameObject.transform.Rotate(Vector3.forward, num * Time.deltaTime);
                    gameObject.transform.Rotate(Vector3.right, num * Time.deltaTime);
                }
            }
        }

        public static float orbitSpeed = 5f;
        private static void UpdateScaleForBeacons(GameObject startObj, GameObject endObj, GameObject beaconObj)
        {
            float num = Vector3.Distance(startObj.transform.position, endObj.transform.position);
            beaconObj.transform.localScale = new Vector3(beaconObj.transform.localScale.x, num / 2f, beaconObj.transform.localScale.z);
            Vector3 position = (startObj.transform.position + endObj.transform.position) / 2f;
            beaconObj.transform.position = position;
            Vector3 up = endObj.transform.position - startObj.transform.position;
            beaconObj.transform.up = up;
        }
        private static float angle;
        public static void OrbitShark()
        {
            float num = 1f;
            Global.angle += Global.orbitSpeed * Time.deltaTime;
            float num2 = GorillaTagger.Instance.offlineVRRig.transform.position.x + num * Mathf.Cos(Global.angle);
            float num3 = GorillaTagger.Instance.offlineVRRig.transform.position.y + num * Mathf.Sin(Global.angle);
            float num4 = GorillaTagger.Instance.offlineVRRig.transform.position.z + num * Mathf.Sin(Global.angle);
            GameObject.Find("Swimming Shark prefab").transform.position = new Vector3(num2, num3, num4);
        }

        public static void HaloShark()
        {
            Global.angle += Global.orbitSpeed * Time.deltaTime;
            float num = GorillaTagger.Instance.offlineVRRig.headConstraint.transform.position.x + 0.5f * Mathf.Cos(Global.angle);
            float num2 = GorillaTagger.Instance.offlineVRRig.headConstraint.transform.position.y + 0.5f;
            float num3 = GorillaTagger.Instance.offlineVRRig.headConstraint.transform.position.z + 0.5f * Mathf.Sin(Global.angle);
            GameObject.Find("Swimming Shark prefab").transform.position = new Vector3(num, num2, num3);
        }

        public static void GrabShark2()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaTagger.Instance.transform.position = GameObject.Find("Swimming Shark prefab").transform.position;
                GorillaTagger.Instance.rightHandTransform.transform.position = GameObject.Find("Swimming Shark prefab").transform.position;

            }
        }

        public static void SharkTraces()
        {
            GameObject gameObject = GameObject.Find("Swimming Shark prefab");
            GameObject gameObject2 = GameObject.CreatePrimitive((PrimitiveType)2);
            UnityEngine.Object.Destroy(gameObject2.GetComponent<BoxCollider>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<Collider>());
            UnityEngine.Object.Destroy(gameObject2.GetComponent<MeshCollider>());
            gameObject2.transform.rotation = Quaternion.identity;
            gameObject2.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            gameObject2.transform.position = gameObject.transform.position;
            Global.UpdateScaleForBeacons(GorillaTagger.Instance.rightHandTransform.gameObject, gameObject.gameObject, gameObject2);
            Renderer component = gameObject2.GetComponent<Renderer>();
            component.material.color = Color.Lerp(new Color(0f, 1f, 0f, 0.5f), new Color(0f, 1f, 1f, 0.5f), Mathf.PingPong(Time.time, 1f));
            UnityEngine.Object.Destroy(gameObject2, Time.deltaTime);
        }

        public static void SharkSizeChanger()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GameObject.Find("Swimming Shark prefab").transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                GameObject.Find("Swimming Shark prefab").transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            }
            if (ControllerInputPoller.instance.leftGrab)
            {
                GameObject.Find("Swimming Shark prefab").transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
        }

        public static void OldRamp()
        {
            GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/tree/stubbranch").SetActive(true);
        }
        public static void UnOldRamp()
        {
            GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/tree/stubbranch").SetActive(false);
        }

        public static void AntiReportD()
        {
            try
            {
                foreach
                    (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                {
                    if (line.linePlayer ==
                        NetworkSystem.Instance.LocalPlayer)
                    {
                        Transform report =
                            line.reportButton.gameObject.transform;
                        foreach
                            (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if
                                (vrrig != GorillaTagger.Instance.offlineVRRig)
                            {
                                float yx = Vector3.Distance(vrrig.rightHandTransform.position,
                                    report.position);
                                float yt = Vector3.Distance(vrrig.leftHandTransform.position,
                                    report.position);

                                if (yx < 0.5 || yt < 0.5)
                                {
                                    {
                                        PhotonNetwork.Disconnect();
                                        RPCP();
                                        NotifiLib.SendNotification
                                            ("Kicked For Report");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { } // Not connected
        }
        public static void RPCP()
        {

            GorillaNot.instance.rpcErrorMax = int.MaxValue;
            GorillaNot.instance.rpcCallLimit = int.MaxValue;
            GorillaNot.instance.logErrorMax = int.MaxValue;

            PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
            PhotonNetwork.QuickResends = int.MaxValue;
            PhotonNetwork.OpCleanActorRpcBuffer(PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.SendAllOutgoingCommands();

            GorillaNot.instance.OnPlayerLeftRoom(PhotonNetwork.LocalPlayer);
        }

        public static void Hitboxes()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (!vrrig.isLocal)
                    {
                        Color32 visualColor = Global.VisualColor(vrrig);
                        Global.CreateBox(visualColor, vrrig);
                    }
                }
            }
            else
            {
                VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
                Color32 color = Global.Infected(offlineVRRig) ? Global.red : offlineVRRig.playerColor;
                Global.CreateBox(color, offlineVRRig);
            }
        }

        public static string visualIndex = "casual";


        public static Color32 VisualColor(VRRig target)
        {
            bool isInfected = Global.Infected(target);

            if (Global.visualIndex == "casual")
            {
                return target.playerColor;
            }
            else if (Global.visualIndex == "fection")
            {
                return isInfected ? Global.red : target.playerColor;
            }
            else
            {
                return isInfected ? Global.red : Global.green;
            }
        }

        public static Color red = new Color32(255, 0, 0, 255);
        public static Color green = new Color32(0, 255, 0, 255);

        public static bool Infected(VRRig p)
        {
            return p.mainSkin.material.name.Contains("It") || p.mainSkin.material.name.Contains("fected");
        }

        public static void CreateBox(Color32 color, VRRig player)
        {
            Vector2 initialScaledHitbox = new Vector2(player.transform.localScale.x * 0.35f, player.transform.localScale.y * 0.7f);
            Vector3 center = player.transform.position - new Vector3(0f, 0.075f, 0f);


            Vector3 minBound = center;
            Vector3 maxBound = center;

            Vector3[] handPositions = { player.rightHand.rigTarget.transform.position, player.leftHand.rigTarget.transform.position };
            foreach (Vector3 handPos in handPositions)
            {
                minBound = Vector3.Min(minBound, handPos);
                maxBound = Vector3.Max(maxBound, handPos);
            }

            Vector2 finalSize = new Vector2(maxBound.x - minBound.x, maxBound.y - minBound.y);
            finalSize.x = Mathf.Max(finalSize.x, initialScaledHitbox.x);
            finalSize.y = Mathf.Max(finalSize.y, initialScaledHitbox.y);


            try
            {
                Camera cam = Camera.main;
                GameObject lineFollow = new GameObject("Line");
                LineRenderer lineUser = lineFollow.AddComponent<LineRenderer>();

                lineUser.startColor = color;
                lineUser.endColor = color;
                lineUser.startWidth = 0.0225f;
                lineUser.endWidth = 0.0225f;
                lineUser.useWorldSpace = false;

                Vector3[] points = new Vector3[5];
                points[0] = new Vector3(-finalSize.x / 2, finalSize.y / 2, 0f);
                points[1] = new Vector3(finalSize.x / 2, finalSize.y / 2, 0f);
                points[2] = new Vector3(finalSize.x / 2, -finalSize.y / 2, 0f);
                points[3] = new Vector3(-finalSize.x / 2, -finalSize.y / 2, 0f);
                points[4] = points[0];

                lineUser.positionCount = points.Length;
                lineUser.SetPositions(points);
                lineUser.material = new Material(Shader.Find("GUI/Text Shader"));

                lineFollow.transform.position = center;
                lineFollow.transform.LookAt(cam.transform.position);

                UnityEngine.Object.Destroy(lineFollow, Time.deltaTime);
            }
            catch
            {
             
            }
        }

        public static void BoxESP()
        {
            {
                if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject gameObject = GameObject.CreatePrimitive((PrimitiveType)3);
                            UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
                            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                            UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
                            gameObject.transform.localScale = new Vector3(0.65f, 1f, 0.65f);
                            gameObject.transform.position = vrrig.transform.position;
                            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            if (vrrig.mainSkin.material.name.Contains("fected") || vrrig.mainSkin.material.name.Contains("It"))
                            {
                                gameObject.GetComponent<Renderer>().material.color = new Color(122.5f, 0f, 0f, 100f);
                            }
                            else
                            {
                                gameObject.GetComponent<Renderer>().material.color = new Color(0f, 122.5f, 0f, 100f);
                            }
                        }
                    }
                }
            }
        }

        public static void CircleFrame()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    Color playerColor = vrrig.playerColor;
                    GameObject circleWireframe = new GameObject("CircleWireframe");
                    circleWireframe.transform.position = vrrig.transform.position;

                    LineRenderer lineRenderer = circleWireframe.AddComponent<LineRenderer>();
                    Shader alwaysVisibleShader = Shader.Find("GUI/Text Shader");
                    if (!alwaysVisibleShader) alwaysVisibleShader = Shader.Find("Unlit/Color");
                    Material seeThroughMaterial = new Material(alwaysVisibleShader);
                    seeThroughMaterial.color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.8f);

                    lineRenderer.material = seeThroughMaterial;
                    lineRenderer.startWidth = 0.025f;
                    lineRenderer.endWidth = 0.025f;
                    lineRenderer.useWorldSpace = true;

                    int segments = 32;
                    float radius = 0.6f;

                    lineRenderer.positionCount = segments + 1;

                    Vector3[] circlePoints = new Vector3[segments + 1];

                    for (int i = 0; i <= segments; i++)
                    {
                        float angle = (i / (float)segments) * Mathf.PI * 2;
                        float x = Mathf.Cos(angle) * radius;
                        float z = Mathf.Sin(angle) * radius;

                        circlePoints[i] = vrrig.transform.position + new Vector3(x, 0, z);
                    }

                    lineRenderer.SetPositions(circlePoints);

                    GameObject.Destroy(circleWireframe, Time.deltaTime);
                }
            }
        }

        public static void TriangleFrame()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    Color playerColor = vrrig.playerColor;
                    GameObject triangleWireframe = new GameObject("TriangleWireframe");
                    triangleWireframe.transform.SetParent(vrrig.transform, false); 

                    LineRenderer lineRenderer = triangleWireframe.AddComponent<LineRenderer>();
                    Shader shader = Shader.Find("Hidden/Internal-Colored");
                    if (!shader) shader = Shader.Find("Unlit/Color");
                    Material seeThroughMaterial = new Material(shader);
                    seeThroughMaterial.color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.8f);

                    lineRenderer.material = seeThroughMaterial;
                    lineRenderer.startWidth = 0.025f;
                    lineRenderer.endWidth = 0.025f;
                    lineRenderer.useWorldSpace = false;
                    lineRenderer.loop = true; 

                    float radius = 0.6f;
                    Vector3[] trianglePoints = new Vector3[3];

                    trianglePoints[0] = new Vector3(0, 0, radius); // Top point
                    trianglePoints[1] = new Vector3(-radius * Mathf.Sin(Mathf.PI / 3), 0, -radius / 2); 
                    trianglePoints[2] = new Vector3(radius * Mathf.Sin(Mathf.PI / 3), 0, -radius / 2); 

                    lineRenderer.positionCount = 3;
                    lineRenderer.SetPositions(trianglePoints);

                    GameObject.Destroy(triangleWireframe, 2f);
                }
            }
        }

        public static void SquareFrame()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    Color playerColor = vrrig.playerColor;
                    GameObject squareWireframe = new GameObject("SquareWireframe");
                    squareWireframe.transform.SetParent(vrrig.transform, false); 

                    LineRenderer lineRenderer = squareWireframe.AddComponent<LineRenderer>();
                    Shader shader = Shader.Find("Hidden/Internal-Colored"); 
                    if (!shader) shader = Shader.Find("Unlit/Color");
                    Material seeThroughMaterial = new Material(shader);
                    seeThroughMaterial.color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.8f);

                    lineRenderer.material = seeThroughMaterial;
                    lineRenderer.startWidth = 0.025f;
                    lineRenderer.endWidth = 0.025f;
                    lineRenderer.useWorldSpace = false;
                    lineRenderer.loop = true; // Close the square

                    float halfSize = 0.5f; 
                    Vector3[] squarePoints = new Vector3[4];

                    squarePoints[0] = new Vector3(-halfSize, 0, halfSize); 
                    squarePoints[1] = new Vector3(halfSize, 0, halfSize);  
                    squarePoints[2] = new Vector3(halfSize, 0, -halfSize); 
                    squarePoints[3] = new Vector3(-halfSize, 0, -halfSize); 

                    lineRenderer.positionCount = 4;
                    lineRenderer.SetPositions(squarePoints);

                    GameObject.Destroy(squareWireframe, 2f);
                }
            }
        }

        public static void SquareGraphFrame()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    Color playerColor = vrrig.playerColor;
                    GameObject squareWireframe = new GameObject("SquareWireframe");
                    squareWireframe.transform.SetParent(vrrig.transform, false); 

                    Shader shader = Shader.Find("Hidden/Internal-Colored");
                    if (!shader) shader = Shader.Find("Unlit/Color");
                    Material seeThroughMaterial = new Material(shader);
                    seeThroughMaterial.color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.8f);

                    float halfSize = 0.5f;
                    int divisions = 5; 

                    LineRenderer squareRenderer = squareWireframe.AddComponent<LineRenderer>();
                    squareRenderer.material = seeThroughMaterial;
                    squareRenderer.startWidth = 0.025f;
                    squareRenderer.endWidth = 0.025f;
                    squareRenderer.useWorldSpace = false;
                    squareRenderer.loop = true; 
                    squareRenderer.positionCount = 4;
                    squareRenderer.SetPositions(new Vector3[]
                    {
                new Vector3(-halfSize, 0, halfSize),
                new Vector3(halfSize, 0, halfSize),   
                new Vector3(halfSize, 0, -halfSize), 
                new Vector3(-halfSize, 0, -halfSize)  
                    });
                    for (int i = 1; i < divisions; i++)
                    {
                        float step = (i / (float)divisions) * (halfSize * 2) - halfSize;
                        GameObject vLine = new GameObject("VGridLine");
                        vLine.transform.SetParent(squareWireframe.transform, false);
                        LineRenderer vLineRenderer = vLine.AddComponent<LineRenderer>();
                        vLineRenderer.material = seeThroughMaterial;
                        vLineRenderer.startWidth = 0.01f;
                        vLineRenderer.endWidth = 0.01f;
                        vLineRenderer.useWorldSpace = false;
                        vLineRenderer.positionCount = 2;
                        vLineRenderer.SetPositions(new Vector3[] { new Vector3(step, 0, halfSize), new Vector3(step, 0, -halfSize) });
                        GameObject hLine = new GameObject("HGridLine");
                        hLine.transform.SetParent(squareWireframe.transform, false);
                        LineRenderer hLineRenderer = hLine.AddComponent<LineRenderer>();
                        hLineRenderer.material = seeThroughMaterial;
                        hLineRenderer.startWidth = 0.01f;
                        hLineRenderer.endWidth = 0.01f;
                        hLineRenderer.useWorldSpace = false;
                        hLineRenderer.positionCount = 2;
                        hLineRenderer.SetPositions(new Vector3[] { new Vector3(-halfSize, 0, step), new Vector3(halfSize, 0, step) });
                    }

                    GameObject.Destroy(squareWireframe, 2f);
                }
            }
        }
        public static void SwasticatestFrame()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    Color playerColor = vrrig.playerColor;
                    GameObject squareWireframe = new GameObject("SquareWireframe");
                    squareWireframe.transform.SetParent(vrrig.transform, false); 

                    Shader shader = Shader.Find("Hidden/Internal-Colored"); 
                    if (!shader) shader = Shader.Find("Unlit/Color");
                    Material material = new Material(shader);
                    material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 0.8f);

                    float halfSize = 0.5f;
                    float edgeSplit = 0.25f;

                    GameObject topEdge = new GameObject("TopEdge");
                    topEdge.transform.SetParent(squareWireframe.transform, false);
                    LineRenderer topLine = topEdge.AddComponent<LineRenderer>();
                    topLine.material = material;
                    topLine.startWidth = 0.025f;
                    topLine.endWidth = 0.025f;
                    topLine.useWorldSpace = false;
                    topLine.positionCount = 2;
                    topLine.SetPositions(new Vector3[]
                    {
                new Vector3(-halfSize, 0, halfSize),
                new Vector3(0, 0, halfSize)
                    });

                    GameObject rightEdge = new GameObject("RightEdge");
                    rightEdge.transform.SetParent(squareWireframe.transform, false);
                    LineRenderer rightLine = rightEdge.AddComponent<LineRenderer>();
                    rightLine.material = material;
                    rightLine.startWidth = 0.025f;
                    rightLine.endWidth = 0.025f;
                    rightLine.useWorldSpace = false;
                    rightLine.positionCount = 2;
                    rightLine.SetPositions(new Vector3[]
                    {
                new Vector3(halfSize, 0, halfSize),
                new Vector3(halfSize, 0, 0)
                    });

                    GameObject bottomEdge = new GameObject("BottomEdge");
                    bottomEdge.transform.SetParent(squareWireframe.transform, false);
                    LineRenderer bottomLine = bottomEdge.AddComponent<LineRenderer>();
                    bottomLine.material = material;
                    bottomLine.startWidth = 0.025f;
                    bottomLine.endWidth = 0.025f;
                    bottomLine.useWorldSpace = false;
                    bottomLine.positionCount = 2;
                    bottomLine.SetPositions(new Vector3[]
                    {
                new Vector3(halfSize, 0, -halfSize),
                new Vector3(0, 0, -halfSize)
                    });

                    GameObject leftEdge = new GameObject("LeftEdge");
                    leftEdge.transform.SetParent(squareWireframe.transform, false);
                    LineRenderer leftLine = leftEdge.AddComponent<LineRenderer>();
                    leftLine.material = material;
                    leftLine.startWidth = 0.025f;
                    leftLine.endWidth = 0.025f;
                    leftLine.useWorldSpace = false;
                    leftLine.positionCount = 2;
                    leftLine.SetPositions(new Vector3[]
                    {
                new Vector3(-halfSize, 0, -halfSize),
                new Vector3(-halfSize, 0, 0)
                    });

                    GameObject verticalLine = new GameObject("VerticalLine");
                    verticalLine.transform.SetParent(squareWireframe.transform, false);
                    LineRenderer vLine = verticalLine.AddComponent<LineRenderer>();
                    vLine.material = material;
                    vLine.startWidth = 0.01f;
                    vLine.endWidth = 0.01f;
                    vLine.useWorldSpace = false;
                    vLine.positionCount = 2;
                    vLine.SetPositions(new Vector3[]
                    {
                new Vector3(0, 0, halfSize),
                new Vector3(0, 0, -halfSize)
                    });

                    GameObject horizontalLine = new GameObject("HorizontalLine");
                    horizontalLine.transform.SetParent(squareWireframe.transform, false);
                    LineRenderer hLine = horizontalLine.AddComponent<LineRenderer>();
                    hLine.material = material;
                    hLine.startWidth = 0.01f;
                    hLine.endWidth = 0.01f;
                    hLine.useWorldSpace = false;
                    hLine.positionCount = 2;
                    hLine.SetPositions(new Vector3[]
                    {
                new Vector3(-halfSize, 0, 0),
                new Vector3(halfSize, 0, 0)
                    });

                    GameObject.Destroy(squareWireframe, 2f);
                }
            }
        }


        public static float l;
        public static void PlayerTrail()
        {

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                Global.l += 2f * Time.deltaTime;
                GameObject gameObject = GameObject.CreatePrimitive(0);
                gameObject.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                UnityEngine.Object.Destroy(gameObject.gameObject.GetComponent<SphereCollider>());
                gameObject.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(new Color(0f, 1f, 0f, 0.5f), new Color(0f, 1f, 1f, 0.5f), Mathf.PingPong(Time.time, 1f));
                gameObject.gameObject.transform.position = vrrig.transform.position + new Vector3(0f * Mathf.Cos(Global.l), -1f, 0f * Mathf.Sin(Global.l)) + new Vector3(0f, 1f, 0f);

                Color endColor = Color.Lerp(new Color(0f, 1f, 0f, 0.5f), new Color(0f, 1f, 1f, 0.5f), Mathf.PingPong(Time.time, 1f));
                Color startColor = Color.Lerp(new Color(0f, 1f, 0f, 0.5f), new Color(0f, 1f, 1f, 0.5f), Mathf.PingPong(Time.time, 1f));

                UnityEngine.Object.Destroy(gameObject.gameObject, 1.4f);
            }
        }

        public static void HeadChams()
        {
            {
                if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != GorillaTagger.Instance.offlineVRRig)
                        {
                            GameObject gameObject = GameObject.CreatePrimitive(0);
                            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                            if (vrrig.mainSkin.material.name.Contains("fected") || vrrig.mainSkin.material.name.Contains("It"))
                            {
                                gameObject.GetComponent<Renderer>().material.color = new Color(122.5f, 0f, 0f, 100f);
                            }
                            else
                            {
                                gameObject.GetComponent<Renderer>().material.color = new Color(0f, 122.5f, 0f, 100f);
                            }
                            gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            gameObject.transform.position = vrrig.transform.position + new Vector3(0f, 0.08f, 0f);
                            UnityEngine.Object.Destroy(gameObject.GetComponent<BoxCollider>());
                            UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
                            UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
                        }
                    }
                }
            }
        }

        public static void UndoChams()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    vrrig.mainSkin.material.shader = Shader.Find("GorillaTag/UberShader");
                }
            }
        }

        public static void GayAll()
        {
            float elapsedTime = Time.time;
            Vector3 colorValues = new Vector3(
                Mathf.Sin(elapsedTime * 2f),
                Mathf.Sin(elapsedTime * 1.5f),
                Mathf.Sin(elapsedTime * 2.5f)
            ) * 0.5f + Vector3.one * 0.5f;

            Color calculatedColor = new Color(colorValues.x, colorValues.y, colorValues.z);

            foreach (VRRig vrig in GorillaParent.instance.vrrigs)
            {
                if (vrig && vrig != GorillaTagger.Instance.offlineVRRig)
                {
                    vrig.mainSkin.material.color = calculatedColor;
                }
            }
        }

        public static void GaySelf()
        {
            float elapsedTime = Time.time;
            Vector3 colorValues = new Vector3(
                Mathf.Sin(elapsedTime * 2f),
                Mathf.Sin(elapsedTime * 1.5f),
                Mathf.Sin(elapsedTime * 2.5f)
            ) * 0.5f + Vector3.one * 0.5f;

            Color calculatedColor = new Color(colorValues.x, colorValues.y, colorValues.z);

            GorillaTagger.Instance.offlineVRRig.mainSkin.material.color = calculatedColor;
        }
        public static void TagGun()
        {
            GunTemplate.StartBothGuns(() =>
            {

                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GunTemplate.LockedPlayer.transform.position;
                GorillaTagger.Instance.myVRRig.transform.position = GunTemplate.LockedPlayer.transform.position;
                if (GorillaTagger.Instance.offlineVRRig.enabled == false)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
                if (PhotonNetwork.IsMasterClient)
                {
                    master.InstantTag(RigManager.GetPlayerFromVRRig(GunTemplate.LockedPlayer));
                }
            }, true);
        }
        public static void TagAll()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig.mainSkin.material.name.Contains("fected"))
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = rig.transform.position;
                    GorillaTagger.Instance.myVRRig.transform.position = rig.transform.position;
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
                if (PhotonNetwork.IsMasterClient)
                {
                    master.InstantTag(RigManager.GetPlayerFromVRRig(rig));
                }
            }
        }
    }
}
