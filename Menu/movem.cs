using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using GorillaLocomotion;
using GunLib;
using Iris.Classes;
using Iris.Menu;
using StupidTemplate;
using StupidTemplate.Classes;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace IRIS.Menu
{
    internal class movem
    {
        public static GameObject leftplat = null;
        public static GameObject rightplat = null;
        private static readonly int FlySpeedMultiplier;

        public static void Platforms()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (leftplat == null)
                {
                    leftplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftplat.transform.localScale = new Vector3(0.01f, 0.3f, 0.3f);
                    leftplat.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    leftplat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    leftplat.GetComponent<Renderer>().material.color = Settings.backgroundColor.colors[1].color;
                }
            }
            else
            {
                if (leftplat != null)
                {
                    Object.Destroy(leftplat);
                    leftplat = null;
                }
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (rightplat == null)
                {
                    rightplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightplat.transform.localScale = new Vector3(0.01f, 0.3f, 0.3f);
                    rightplat.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    rightplat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    rightplat.GetComponent<Renderer>().material.color = Settings.backgroundColor.colors[1].color;
                }
            }
            else
            {
                if (rightplat != null)
                {
                    Object.Destroy(rightplat);
                    rightplat = null;
                }
            }

        }
        public static void TPlatforms()
        {
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f)
            {
                if (leftplat == null)
                {
                    leftplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftplat.transform.localScale = new Vector3(0.01f, 0.3f, 0.3f);
                    leftplat.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    leftplat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    leftplat.GetComponent<Renderer>().material.color = Settings.backgroundColor.colors[1].color;//Color.magenta;
                }
            }
            else
            {
                if (leftplat != null)
                {
                    Object.Destroy(leftplat);
                    leftplat = null;
                }
            }
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f)
            {
                if (rightplat == null)
                {
                    rightplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightplat.transform.localScale = new Vector3(0.01f, 0.3f, 0.3f);
                    rightplat.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    rightplat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    rightplat.GetComponent<Renderer>().material.color = Settings.backgroundColor.colors[1].color;//Color.magenta;
                }
            }
            else
            {
                if (rightplat != null)
                {
                    Object.Destroy(rightplat);
                    rightplat = null;
                }
            }
        }

        public static void TogglePlatforms()
        {
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f && ControllerInputPoller.instance.leftGrab)
            {
                if (leftplat == null)
                {
                    leftplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftplat.transform.localScale = new Vector3(0.01f, 0.3f, 0.3f);
                    leftplat.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    leftplat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    leftplat.GetComponent<Renderer>().material.color = Settings.backgroundColor.colors[1].color;//Color.magenta;
                }
            }
            else
            {
                if (leftplat != null)
                {
                    Object.Destroy(leftplat);
                    leftplat = null;
                }
            }
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f && ControllerInputPoller.instance.rightGrab)
            {
                if (rightplat == null)
                {
                    rightplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightplat.transform.localScale = new Vector3(0.01f, 0.3f, 0.3f);
                    rightplat.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    rightplat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    rightplat.GetComponent<Renderer>().material.color = Settings.backgroundColor.colors[1].color;//Color.magenta;
                }
            }
            else
            {
                if (rightplat != null)
                {
                    Object.Destroy(rightplat);
                    rightplat = null;
                }
            }
        }
        public static void NoclipMod()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f || UnityInput.Current.GetKey(KeyCode.E))
            {
                foreach (MeshCollider meshCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    meshCollider.enabled = false;
                }
            }
            else
            {
                foreach (MeshCollider meshCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    meshCollider.enabled = true;
                }
            }
        }

        public static void Flight()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.fly;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public static void TFlighty()
        {
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.1f)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.fly;
                GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }

        internal static Vector3 previousMousePosition;


        public static void AdvancedWASD(float speed)
        {
            GorillaTagger.Instance.rigidbody.useGravity = false;
            GorillaTagger.Instance.rigidbody.velocity = new Vector3(0, 0, 0);
            float NSpeed = speed * Time.deltaTime;
            if (UnityInput.Current.GetKey(KeyCode.LeftShift) || UnityInput.Current.GetKey(KeyCode.RightShift))
            {
                NSpeed *= 10f;
            }
            if (UnityInput.Current.GetKey(KeyCode.LeftArrow) || UnityInput.Current.GetKey(KeyCode.A))
            {
                Camera.main.transform.position += Camera.main.transform.right * -1f * NSpeed;
            }
            if (UnityInput.Current.GetKey(KeyCode.RightArrow) || UnityInput.Current.GetKey(KeyCode.D))
            {
                Camera.main.transform.position += Camera.main.transform.right * NSpeed;
            }
            if (UnityInput.Current.GetKey(KeyCode.UpArrow) || UnityInput.Current.GetKey(KeyCode.W))
            {
                Camera.main.gameObject.transform.position += Camera.main.transform.forward * NSpeed;
            }
            if (UnityInput.Current.GetKey(KeyCode.DownArrow) || UnityInput.Current.GetKey(KeyCode.S))
            {
                Camera.main.transform.position += Camera.main.transform.forward * -1f * NSpeed;
            }
            if (UnityInput.Current.GetKey(KeyCode.Space) || UnityInput.Current.GetKey(KeyCode.PageUp))
            {
                Camera.main.transform.position += Camera.main.transform.up * NSpeed;
            }
            if (UnityInput.Current.GetKey(KeyCode.LeftControl) || UnityInput.Current.GetKey(KeyCode.PageDown))
            {
                Camera.main.transform.position += Camera.main.transform.up * -1f * NSpeed;
            }
            if (UnityInput.Current.GetMouseButton(1))
            {
                Vector3 val = UnityInput.Current.mousePosition - previousMousePosition;
                float num2 = Camera.main.transform.localEulerAngles.y + val.x * 0.3f;
                float num3 = Camera.main.transform.localEulerAngles.x - val.y * 0.3f;
                Camera.main.transform.localEulerAngles = new Vector3(num3, num2, 0f);
            }
            previousMousePosition = UnityInput.Current.mousePosition;
        }

        public static void NoTagFreeze()
        {
            GTPlayer.Instance.disableMovement = false;
        }
        public static float GhostCooldown;
        public static bool GhostToggled;
        public static void GhostMonke()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton && Time.time >= GhostCooldown + 0.2f || UnityInput.Current.GetKey(KeyCode.R) && Time.time >= GhostCooldown + 0.2f)
            {
                GhostCooldown = Time.time;
                if (GhostToggled == true)
                {
                    GhostToggled = !GhostToggled;
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                }
                else
                {
                    if (GhostToggled == false)
                    {
                        GhostToggled = !GhostToggled;
                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                    }
                }
            }
        }
        public static bool InvisToggled;
        public static float InvisCooldown;
        public static void InvisibleMonke()
        {

            if (ControllerInputPoller.instance.leftControllerPrimaryButton && Time.time >= InvisCooldown + 0.2f || UnityInput.Current.GetKey(KeyCode.T) && Time.time >= InvisCooldown + 0.2f)
            {
                InvisCooldown = Time.time;
                if (InvisToggled == true)
                {
                    InvisToggled = !InvisToggled;
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(100f, 0f, 100f);
                }
                else
                {
                    if (InvisToggled == false)
                    {
                        InvisToggled = !InvisToggled;
                        GorillaTagger.Instance.offlineVRRig.enabled = true;
                    }
                }
            }
        }
        public static void HeilHitler()
        {
            if (ControllerInputPoller.instance.rightGrab || Mouse.current.rightButton.isPressed)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.headCollider.transform.position;
                GorillaTagger.Instance.offlineVRRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.up * 0.45f + GorillaTagger.Instance.offlineVRRig.transform.forward * 0.9f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation * Quaternion.Euler(155f, 75f, 140f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void SArms()
        {
            GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        public static void RArms()
        {
            GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(2f, 2f, 2f);
        }
        public static void ResetArms()
        {
            GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        public static bool IsGTPlayerInfected(VRRig GTPlayer)
        {
            return GTPlayer.mainSkin.material.name.Contains("fected");
        }
        public static bool IsLocked;


        public static void Speedboost()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GTPlayer.Instance.maxJumpSpeed = Settings.Speed;
                GTPlayer.Instance.jumpMultiplier = 1f;
            }
        }
        public static void RGSpeedboost()
        {

            GTPlayer.Instance.maxJumpSpeed = Settings.Speed;
            GTPlayer.Instance.jumpMultiplier = 1f;
        }
        public static void TpGun()
        {
            if (ControllerInputPoller.instance.rightGrab || Mouse.current.rightButton.isPressed)
            {

                var data = GunLib_again.Shoot();
                if (data != null)
                {
                    if (data.isShooting && data.isTriggered)
                    {
                        GorillaTagger.Instance.transform.position = GunTemplate.spherepointer.transform.position;
                        GTPlayer.Instance.transform.position = GunTemplate.spherepointer.transform.position;
                    }
                }
            }
            else
            {
                GunLib_again.GunCleanUp();
            }
        }














    }
}
