using BepInEx;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace GunLib
{
    public class ClientInput
    {
        public static bool GetInputValue(float grabValue)
        {
            return grabValue >= 0.75f;
        }
    }
    public class GunTemplate : MonoBehaviour
    {
        public static int LineCurve = 150;
        private const float PulseSpeed = 2f;
        private const float PulseAmplitude = 0.03f;

        public static GameObject spherepointer;
        public static VRRig LockedPlayer;
        public static Vector3 lr;
        public static Color32 PointerColor = new Color32(35, 35, 35, 255);
        public static Color32 TriggeredPointerColor = new Color32(34, 115, 179, 255);
        public static RaycastHit nray;
        private static bool lockToggleState = false;
        private static bool wasTriggerPressed = false;
        public static bool useHeadPosition = false;

        public static void StartVrGun(Action action, bool LockOn)
        {
            Vector3 rayOrigi;
            Vector3 rayDirection;

            if (useHeadPosition)
            {
                rayOrigi = GorillaTagger.Instance.headCollider.transform.position;
                rayDirection = (GorillaTagger.Instance.headCollider.transform.forward + Vector3.down * 0.1f).normalized;
            }
            else
            {
                rayOrigi = GorillaTagger.Instance.rightHandTransform.position;
                rayDirection = (-GorillaTagger.Instance.rightHandTransform.up + GorillaTagger.Instance.rightHandTransform.forward * 0.5f).normalized;
            }

            Physics.Raycast(rayOrigi, rayDirection, out nray, float.MaxValue);

            if (spherepointer == null)
            {
                spherepointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                spherepointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                var renderer = spherepointer.GetComponent<Renderer>();
                renderer.material.shader = Shader.Find("GUI/Text Shader");
                Collider[] colliders = spherepointer.GetComponents<Collider>();
                foreach (Collider collider in colliders)
                {
                    GameObject.Destroy(collider);
                }
                lr = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
                spherepointer.AddComponent<GunTemplate>().StartCoroutine(PulsePointer(spherepointer));
                var startPos = lr;
                var endPos = spherepointer.transform.position;
                var distance = Vector3.Distance(startPos, endPos);
                int pointCount = Mathf.CeilToInt(100 + distance * 10);
                var points = new Vector3[pointCount];
                var direction = (endPos - startPos).normalized;
                var right = Vector3.Cross(direction, Vector3.up).normalized;
                var up = Vector3.Cross(direction, right).normalized;
                var timeAngle = Time.time * 2;
                var revolutions = Mathf.Clamp(distance, 2, 15);
                var radius = .03f;
                for (int i = 0; i < pointCount; i++)
                {
                    var t = (float)i / (pointCount - 1);
                    var angle = t * revolutions * Mathf.PI * 2;
                    var currentAngle = timeAngle + angle;
                    var localOffset = CalculateOffset(currentAngle, direction, angle, right, up);
                    points[i] = startPos + direction * (t * distance) + localOffset * radius;
                }
                LineRender.positionCount = pointCount;
                LineRender.SetPositions(points);
            }

            bool isTriggerPressed = ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;

            if (isTriggerPressed && !wasTriggerPressed && LockOn)
            {
                lockToggleState = !lockToggleState;

                if (lockToggleState)
                {
                    LockedPlayer = nray.collider?.GetComponentInParent<VRRig>();
                    if (LockedPlayer != null)
                    {
                        LockedPlayerActorNumber = PhotonView.Get(LockedPlayer.gameObject)?.Owner.ActorNumber;
                    }
                    else
                    {
                        lockToggleState = false;
                    }
                }
                else
                {
                    LockedPlayer = null;
                    LockedPlayerActorNumber = null;
                }
            }
            wasTriggerPressed = isTriggerPressed;

            if (LockedPlayer != null)
            {
                spherepointer.transform.position = LockedPlayer.transform.position;
                spherepointer.GetComponent<Renderer>().material.color = TriggeredPointerColor;
                action();
            }
            else
            {
                spherepointer.transform.position = nray.point;
                spherepointer.GetComponent<Renderer>().material.color = isTriggerPressed ? TriggeredPointerColor : PointerColor;

                if (isTriggerPressed && !LockOn)
                {
                    action();
                }
            }
        }

        public static int? LockedPlayerActorNumber;
        private static LineRenderer LineRender = null;
        public static void StartPcGun(Action action, bool LockOn)
        {
            Ray ray = GameObject.Find("Shoulder Camera").activeSelf ? GameObject.Find("Shoulder Camera").GetComponent<Camera>().ScreenPointToRay(UnityInput.Current.mousePosition) : GorillaTagger.Instance.mainCamera.GetComponent<Camera>().ScreenPointToRay(UnityInput.Current.mousePosition);

            if (Mouse.current.rightButton.isPressed)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out nray, float.PositiveInfinity, -32777) && spherepointer == null)
                {
                    if (spherepointer == null)
                    {
                        spherepointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        var renderer = spherepointer.GetComponent<Renderer>();
                        renderer.material.shader = Shader.Find("GUI/Text Shader");
                        spherepointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                        Collider[] colliders = spherepointer.GetComponents<Collider>();
                        foreach (Collider collider in colliders)
                        {
                            GameObject.Destroy(collider);
                        }

                        lr = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
                        spherepointer.AddComponent<GunTemplate>().StartCoroutine(PulsePointer(spherepointer));
                        var startPos = lr;
                        var endPos = spherepointer.transform.position;
                        var distance = Vector3.Distance(startPos, endPos);
                        int pointCount = Mathf.CeilToInt(100 + distance * 10);
                        var points = new Vector3[pointCount];
                        var direction = (endPos - startPos).normalized;
                        var right = Vector3.Cross(direction, Vector3.up).normalized;
                        var up = Vector3.Cross(direction, right).normalized;
                        var timeAngle = Time.time * 2;
                        var revolutions = Mathf.Clamp(distance, 2, 15);
                        var radius = .03f;
                        for (int i = 0; i < pointCount; i++)
                        {
                            var t = (float)i / (pointCount - 1);
                            var angle = t * revolutions * Mathf.PI * 2;
                            var currentAngle = timeAngle + angle;
                            var localOffset = CalculateOffset(currentAngle, direction, angle, right, up);
                            points[i] = startPos + direction * (t * distance) + localOffset * radius;
                        }
                        LineRender.positionCount = pointCount;
                        LineRender.SetPositions(points);
                    }
                }

                bool isLeftMousePressed = Mouse.current.leftButton.isPressed;

                if (isLeftMousePressed && !wasTriggerPressed && LockOn)
                {
                    lockToggleState = !lockToggleState;

                    if (lockToggleState)
                    {
                        LockedPlayer = nray.collider?.GetComponentInParent<VRRig>();
                        if (LockedPlayer != null)
                        {
                            LockedPlayerActorNumber = PhotonView.Get(LockedPlayer.gameObject)?.Owner.ActorNumber;
                        }
                        else
                        {
                            lockToggleState = false;
                        }
                    }
                    else
                    {
                        LockedPlayer = null;
                        LockedPlayerActorNumber = null;
                    }
                }
                wasTriggerPressed = isLeftMousePressed;

                if (LockedPlayer != null)
                {
                    spherepointer.transform.position = LockedPlayer.transform.position;
                    spherepointer.GetComponent<Renderer>().material.color = TriggeredPointerColor;
                    action();
                }
                else
                {
                    spherepointer.transform.position = nray.point;
                    spherepointer.GetComponent<Renderer>().material.color = isLeftMousePressed ? TriggeredPointerColor : PointerColor;

                    if (isLeftMousePressed && !LockOn)
                    {
                        action();
                    }
                }
            }
            else if (spherepointer != null)
            {
                GameObject.Destroy(spherepointer);
                spherepointer = null;
                LockedPlayer = null;
                lockToggleState = false;
            }
        }
        private static Vector3 CalculatePoint(float t, Vector3 start, Vector3 mid, Vector3 end) => Mathf.Pow(1 - t, 2) * start + 2 * (1 - t) * t * mid + Mathf.Pow(t, 2) * end;
        private static Vector3 GetMiddle(Vector3 vector) => new Vector3(vector.x / 2f, vector.y / 2f, vector.z / 2f);
        private static Vector3 CalculateOffset(float currentAngle, Vector3 direction, float angle, Vector3 right, Vector3 up) => Quaternion.AngleAxis(Mathf.Rad2Deg * currentAngle, direction) * (right * Mathf.Cos(angle) + up * Mathf.Sin(angle));
        public static void StartBothGuns(Action action, bool locko)
        {
            if (XRSettings.isDeviceActive)
            {
                StartVrGun(action, locko);
            }
            if (!XRSettings.isDeviceActive)
            {
                StartPcGun(action, locko);
            }
        }

        private static IEnumerator PulsePointer(GameObject pointer)
        {
            Vector3 originalScale = pointer.transform.localScale;
            while (true)
            {
                float scaleFactor = 1 + Mathf.Sin(Time.time * PulseSpeed) * PulseAmplitude;
                pointer.transform.localScale = originalScale * scaleFactor;
                yield return null;
            }
        }
    }
}