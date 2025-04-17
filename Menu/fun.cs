using GorillaTag;
using GunLib;
using HarmonyLib;
using Iris.Classes;
using Photon.Pun;
using StupidTemplate.Classes;
using UnityEngine;
using UnityEngine.InputSystem;
using static Iris.Menu.Exploits.LagProperties;
using static Iris.Menu.Exploits;
using System.Drawing;
using Color = UnityEngine.Color;
using static Iris.Menu.Exploits.KickProperties;
using Critters.Scripts;
using Photon.Voice;
using UnityEngine.UIElements;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TagEffects;
using Photon.Realtime;
using System.Collections;
using Iris.Mods;
using ExitGames.Client.Photon;
using System.Linq;
using GorillaLocomotion;
using System.Runtime.CompilerServices;
using Iris.Menu;
using BepInEx;
using Liv.Lck.GorillaTag;

namespace IRIS.Menu
{
    internal class fun
    {
        public static bool IsLocked;

        public static void NiggerGun()
        {
            GunLib.GunTemplate.StartBothGuns(() =>
            {
                GunTemplate.LockedPlayer.mainSkin.material.color = Color.black;
            }, true);
        }
        public static void CrackerGun()
        {
            GunLib.GunTemplate.StartBothGuns(() =>
            {
                GunTemplate.LockedPlayer.mainSkin.material.color = Color.white;
            }, true);
        }
        private static float hoverboardSpamDelay;
        public static void HoverboardSpam()
        {
            if (!CanActivate) return;
            hoverboardSpamDelay = Time.time + 0.5f;
            FreeHoverboardManager.instance.SendDropBoardRPC(GorillaTagger.Instance.rightHandTransform.transform.position, Quaternion.identity, GorillaTagger.Instance.offlineVRRig.rightHandPlayer.transform.position * 15f, GorillaTagger.Instance.rightHandTransform.transform.position * 15f, Color.white);
        }
        public static void GliderPSammer()
        {
            if (!CanActivate) return;
            foreach (GliderHoldable instance in UnityEngine.Object.FindObjectsOfType<GliderHoldable>())
            {
                if (instance.GetView.Owner == PhotonNetwork.LocalPlayer)
                {
                    instance.transform.position = GorillaTagger.Instance.offlineVRRig.rightHandTransform.transform.position;
                    instance.GetComponent<Rigidbody>().velocity = GorillaTagger.Instance.offlineVRRig.rightHandTransform.transform.position * 15f;
                    instance.RequestOwnership();
                }
                else
                {
                    instance.OnHover(null, null);
                }
            }
        }
        public static void GliderGun()
        {
            GunTemplate.StartBothGuns(() =>
            {
                foreach (GliderHoldable instance in UnityEngine.Object.FindObjectsOfType<GliderHoldable>())
                {
                    if (instance.GetView.Owner == PhotonNetwork.LocalPlayer)
                    {
                        instance.transform.position = GunTemplate.spherepointer.transform.position;
                        instance.RequestOwnership();
                    }
                    else
                    {
                        instance.OnHover(null, null);
                    }
                }
            }, false);
        }
        public static float critters = 0;
        public static void SpamSpawnCritters()
        {
            foreach (NetPlayer p in NetworkSystem.Instance.AllNetPlayers)
            {
                if (Time.time > critters)
                {
                    critters = Time.time + 0.1f;
                    foreach (CrittersPawn pawn in CrittersPawn.FindObjectsOfType<CrittersPawn>())
                    {
                        pawn.currentState = CrittersPawn.CreatureState.Captured;
                        pawn.currentState = CrittersPawn.CreatureState.Despawning;
                        pawn.currentState = CrittersPawn.CreatureState.Spawning;
                        pawn.despawnDelay = 0;
                        pawn.despawnTime = 0f;
                    }
                }
            }
        }
        public static void RequestCritterOwnerShip()
        {
            CrittersStunBomb crittersStunBomb = (CrittersStunBomb)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.StunBomb);
            if (crittersStunBomb.GetComponent<RequestableOwnershipGuard>().currentOwner != NetworkSystem.Instance.LocalPlayer)
            {
                var ownershipGuard = crittersStunBomb.GetComponent<RequestableOwnershipGuard>();
                var photonView = crittersStunBomb.GetComponent<PhotonView>();
                ownershipGuard.RequestOwnership(null, null);
                ownershipGuard.currentOwner = NetworkSystem.Instance.LocalPlayer;
                ownershipGuard.actualOwner = NetworkSystem.Instance.LocalPlayer;
                photonView.OwnerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                photonView.ControllerActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
                photonView.RequestOwnership();
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        public static List<CrittersPawn> ActiveCritters = new List<CrittersPawn>();
        public static List<CrittersPawn> SkeletonCritters = new List<CrittersPawn>();
        public static List<CrittersStunBomb> ActiveBalls = new List<CrittersStunBomb>();
        public static List<CrittersStickyTrap> ActiveOrangeBalls = new List<CrittersStickyTrap>();
        public static int ReuseIndex;
        public static float Spam;

        private static bool CanActivate => (ControllerInputPoller.instance.rightGrab || Mouse.current.rightButton.isPressed);

        public static LckCococamManager CocoCamera = null;     
        public static void SSCameraSpammer()
        {
            if (!CanActivate) return;
            CocoCamera.Instance.RequestOwnership();
            CocoCamera.Instance.Spawned();
            CocoCamera.Instance.recording = true;
            Serialize();

            CocoCamera.Instance.visible = true;
            CocoCamera.Instance.transform.position = TrueRightHand().Item1;
            CocoCamera.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Serialize();
        }
        public static void SSCameraMinigun()
        {
            if (!CanActivate) return;
            CocoCamera.Instance.RequestOwnership();
            CocoCamera.Instance.Spawned();
            CocoCamera.Instance.recording = true;
            Serialize();

            CocoCamera.Instance.visible = true;
            CocoCamera.Instance.transform.position = TrueRightHand().Item1;
            CocoCamera.Instance.GetComponent<Rigidbody>().velocity = TrueRightHand().Item1 * 1000f;
            Serialize();
        }
        public static void SpawnCritter()
        {
            OwnershipOfCritters();
            Serialize();
            if (!CanActivate || Time.time <= Spam && CrittersManager.instance.guard.photonView.IsMine) return;

            Spam = Time.time + 0.05f;
            Serialize();
            var position = TrueRightHand().Item1;
            CrittersPawn critter;

            if (ActiveCritters.Count < 99)
            {
                critter = (CrittersPawn)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.Creature, -1);
                var visuals = critter.visuals;
                var appearance = default(CritterAppearance);
                appearance.size = 100f;
                appearance.hatName = "PaperCrown";
                visuals.SetAppearance(appearance);
                critter.SetTemplate(0);
                critter.currentState = CrittersPawn.CreatureState.AttractedTo;
                CrittersManager.instance.GetView.RPC("RemoteSpawnCreature", 0, new object[]
                {
                critter.actorId,
                critter.regionIndex,
                critter.visuals.Appearance.WriteToRPCData()
                });
                if (!ActiveCritters.Contains(critter))
                {
                    ActiveCritters.Add(critter);
                }
            }
            else
            {
                critter = ActiveCritters[ReuseIndex];
                ReuseIndex++;
                if (ReuseIndex >= 99)
                {
                    ReuseIndex = 0;
                }
            }

            Serialize();
            critter.MoveActor(position, Quaternion.identity, false, true, true);
            critter.rb.velocity = Vector3.zero;
            critter.UpdateImpulseVelocity();
            Serialize();
        }

        public static void LaunchCritter()
        {
            OwnershipOfCritters();
            Serialize();
            if (!CanActivate || Time.time <= Spam && CrittersManager.instance.guard.photonView.IsMine) return;

            Spam = Time.time + 0.05f;
            Serialize();
            var position = TrueRightHand().Item1;
            CrittersPawn critter;

            if (ActiveCritters.Count < 99)
            {
                critter = (CrittersPawn)CrittersManager.instance.SpawnActor(0, -1);
                var visuals = critter.visuals;
                var appearance = visuals.Appearance;
                appearance.size = 100f;
                appearance.hatName = "PaperCrown";
                visuals.SetAppearance(appearance);
                critter.SetTemplate(0);
                critter.currentState = CrittersPawn.CreatureState.AttractedTo;
                CrittersManager.instance.GetView.RPC("RemoteSpawnCreature", 0, new object[]
                {
                critter.actorId,
                critter.regionIndex,
                critter.visuals.Appearance.WriteToRPCData()
                });
                if (!ActiveCritters.Contains(critter))
                {
                    ActiveCritters.Add(critter);
                }
            }
            else
            {
                critter = ActiveCritters[ReuseIndex];
                ReuseIndex++;
                if (ReuseIndex >= 99)
                {
                    ReuseIndex = 0;
                }
            }

            Serialize();
            critter.MoveActor(position, Quaternion.identity, false, true, true);
            critter.rb.velocity = TrueRightHand().Item4 * Time.deltaTime * 1000f;
            critter.UpdateImpulseVelocity();
            Serialize();
        }

        public static void SpawnStunBomb()
        {
            OwnershipOfCritters();
            if (!CanActivate || Time.time <= Spam && CrittersManager.instance.guard.photonView.IsMine) return;

            Spam = Time.time + 0.05f;
            Serialize();
            var position = TrueRightHand().Item1;
            CrittersStunBomb bomb;

            if (ActiveBalls.Count < 90)
            {
                bomb = (CrittersStunBomb)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.StunBomb, -1);
                if (!ActiveBalls.Contains(bomb))
                {
                    ActiveBalls.Add(bomb);
                }
            }
            else
            {
                bomb = ActiveBalls[ReuseIndex];
                bomb.MoveActor(position, Quaternion.identity, false, true, true);
                bomb.lastImpulseVelocity = Vector3.zero;
                ReuseIndex++;
                if (ReuseIndex >= 90)
                {
                    ReuseIndex = 0;
                }
            }

            Serialize();
            bomb.MoveActor(position, Quaternion.identity, false, true, true);
            bomb.rb.velocity = Vector3.zero;
            bomb.UpdateImpulseVelocity();
            Serialize();
        }

        public static void LaunchStunBomb()
        {
            OwnershipOfCritters();
            if (!CanActivate || Time.time <= Spam && CrittersManager.instance.guard.photonView.IsMine) return;

            Spam = Time.time + 0.05f;
            Serialize();
            var position = TrueRightHand().Item1;
            CrittersStunBomb bomb;

            if (ActiveBalls.Count < 90)
            {
                bomb = (CrittersStunBomb)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.StunBomb, -1);
                if (!ActiveBalls.Contains(bomb))
                {
                    ActiveBalls.Add(bomb);
                }
            }
            else
            {
                bomb = ActiveBalls[ReuseIndex];
                bomb.MoveActor(position, Quaternion.identity, false, true, true);
                bomb.rb.velocity = TrueRightHand().Item4 * Time.deltaTime * 1000f;
                bomb.UpdateImpulseVelocity();
                ReuseIndex++;
                if (ReuseIndex >= 90)
                {
                    ReuseIndex = 0;
                }
            }

            Serialize();
            bomb.MoveActor(position, Quaternion.identity, false, true, true);
            bomb.rb.velocity = TrueRightHand().Item4 * Time.deltaTime * 1000f;
            bomb.UpdateImpulseVelocity();
            Serialize();
        }

        public static void SpawnStickyTrap()
        {
            OwnershipOfCritters();
            if (!CanActivate || Time.time <= Spam && CrittersManager.instance.guard.photonView.IsMine) return;

            Spam = Time.time + 0.05f;
            Serialize();
            var position = TrueRightHand().Item1;
            CrittersStickyTrap trap;

            if (ActiveOrangeBalls.Count < 90)
            {
                trap = (CrittersStickyTrap)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.StickyTrap, -1);
                if (!ActiveOrangeBalls.Contains(trap))
                {
                    ActiveOrangeBalls.Add(trap);
                }
            }
            else
            {
                trap = ActiveOrangeBalls[ReuseIndex];
                trap.MoveActor(position, Quaternion.identity, false, true, true);
                trap.rb.velocity = Vector3.zero;
                trap.UpdateImpulseVelocity();
                ReuseIndex++;
                if (ReuseIndex >= 90)
                {
                    ReuseIndex = 0;
                }
            }

            Serialize();
            trap.MoveActor(position, Quaternion.identity, false, true, true);
            trap.rb.velocity = Vector3.zero;
            trap.UpdateImpulseVelocity();
            Serialize();
        }

        public static void LaunchStickyTrap()
        {
            OwnershipOfCritters();
            if (!CanActivate || Time.time <= Spam && CrittersManager.instance.guard.photonView.IsMine) return;

            Spam = Time.time + 0.05f;
            Serialize();
            var position = TrueRightHand().Item1;
            CrittersStickyTrap trap;

            if (ActiveOrangeBalls.Count < 90)
            {
                trap = (CrittersStickyTrap)CrittersManager.instance.SpawnActor(CrittersActor.CrittersActorType.StickyTrap, -1);
                if (!ActiveOrangeBalls.Contains(trap))
                {
                    ActiveOrangeBalls.Add(trap);
                }
            }
            else
            {
                trap = ActiveOrangeBalls[ReuseIndex];
                trap.MoveActor(position, Quaternion.identity, false, true, true);
                trap.rb.velocity = TrueRightHand().Item4 * Time.deltaTime * 1000f;
                trap.UpdateImpulseVelocity();
                ReuseIndex++;
                if (ReuseIndex >= 90)
                {
                    ReuseIndex = 0;
                }
            }

            Serialize();
            trap.MoveActor(position, Quaternion.identity, false, true, true);
            trap.rb.velocity = TrueRightHand().Item4 * Time.deltaTime * 2000f;
            trap.UpdateImpulseVelocity();
            Serialize();
        }
        public static ValueTuple<Vector3, Quaternion, Vector3, Vector3, Vector3> TrueRightHand()
        {
            Quaternion quaternion = GorillaTagger.Instance.rightHandTransform.rotation * GTPlayer.Instance.rightHandRotOffset;
            return new ValueTuple<Vector3, Quaternion, Vector3, Vector3, Vector3>(GorillaTagger.Instance.rightHandTransform.position + GorillaTagger.Instance.rightHandTransform.rotation * GTPlayer.Instance.rightHandOffset, quaternion, quaternion * Vector3.up, quaternion * Vector3.forward, quaternion * Vector3.right);
        }
        public static ValueTuple<Vector3, Quaternion, Vector3, Vector3, Vector3> TrueLeftHand()
        {
            Quaternion quaternion = GorillaTagger.Instance.leftHandTransform.rotation * GTPlayer.Instance.leftHandRotOffset;
            return new ValueTuple<Vector3, Quaternion, Vector3, Vector3, Vector3>(GorillaTagger.Instance.leftHandTransform.position + GorillaTagger.Instance.leftHandTransform.rotation * GTPlayer.Instance.leftHandOffset, quaternion, quaternion * Vector3.up, quaternion * Vector3.forward, quaternion * Vector3.right);
        }

        public static void OwnershipOfCritters()
        {
            CrittersManager.instance.guard.photonView.RPC("OwnershipRequested", 0, new object[]
            {
                    CrittersManager.instance.guard.ownershipRequestNonce + "1"
            });
        }

        public static void Serialize()
        {
            typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());
        }

     
        public static void QuestScore(int number)
        {
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_RequestQuestScore", RpcTarget.All, new object[]
            {
                number 
            });
            GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateQuestScore", RpcTarget.All, new object[]
           {
                number
           });
        }
       
        private int ninjastar = -1291863839;
        private int hornyslingshot = 693334698;
        private int snowball = -675036877;
        private int cupid = 825718363;
        private int elfbow = 1705139863;
        private int waterballoon = -1674517839;
        private int lavarock = -622368518;
        private int moltenrock = -1280105888;
        private int spidrbow = -790645151;
        private int bucketgiftcane = 2061412059;
        private int bucketgiftroll = -1433633837;
        private int bucketgiftround = -160604350;
        private int bucketgiftsquare = -666337545;
        private int sciencecandy = -716425086;
        private int paperairplane = -1405953129;
        private int devilbow = -1140939310;
        private int fishfood = -1509512060;
        private int dragon = 526153556;
        private int yumibow = -2146558070;
        private int cyberninjastar = 1936767506;
        private int playingcard = -1299387895;
        private int rottenpunkin = -767500737;
        public static void GayGun()
        {

            Color color = new Color(
                Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f,
                Mathf.Sin(Time.time * 1.5f) * 0.5f + 0.5f,
                Mathf.Sin(Time.time * 2.5f) * 0.5f + 0.5f
            );
            GunLib.GunTemplate.StartBothGuns(() =>
            {
                GunTemplate.LockedPlayer.mainSkin.material.color = color;
            }, true);
        }
    }
}
