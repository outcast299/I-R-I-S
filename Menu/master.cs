using System;
using System.Collections.Generic;
using System.Text;
using Fusion;
using Iris.Classes;
using Photon.Pun;
using StupidTemplate.Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Iris.Menu
{
    internal class master
    {
        public static void InstantTag(NetPlayer plr)
        {
            GorillaTagManager nigger = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Manager").GetComponent<GorillaTagManager>();
            if (nigger.isCurrentlyTag)
            {
                nigger.ChangeCurrentIt(plr);
            }
            else
            {
                if (!nigger.currentInfected.Contains(plr))
                {
                    nigger.AddInfectedPlayer(plr);
                }
            }
        }
        public static void RemoveThingy(NetPlayer plr)
        {
            GorillaTagManager nigger = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Manager").GetComponent<GorillaTagManager>();
            if (nigger.isCurrentlyTag)
            {
                if (nigger.currentIt == plr)
                {
                    nigger.currentIt = null;
                }
            }
            else
            {
                if (nigger.currentInfected.Contains(plr))
                {
                    nigger.currentInfected.Remove(plr);
                }
            }
        }
        public static float delay = 0;
        public static void MatPlayer(NetPlayer plr)
        {
            if(Time.time > delay)
            {
                delay = Time.time+ 1.2f;
                GorillaTagManager nigger = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Manager").GetComponent<GorillaTagManager>();
                if (nigger.isCurrentlyTag)
                {
                    nigger.ChangeCurrentIt(plr);
                }
                else
                {
                    if (!nigger.currentInfected.Contains(plr))
                    {
                        nigger.AddInfectedPlayer(plr);
                    }
                }
            }
        }
        public static void MatGun()
        {
            if (ControllerInputPoller.instance.rightGrab || Mouse.current.rightButton.isPressed)
            {
                var data = GunLib_again.ShootLocked();
                if (data != null)
                {
                    if (data.lockedPlayer != null && data.isLocked && RigManager.GetPhotonViewFromVRRig(data.lockedPlayer) != null)
                    {
                        if (data.lockedPlayer != null)
                        {
                            for (int i = 0; i < 50; i++)
                            {
                                MatPlayer(RigManager.GetPlayerFromVRRig(data.lockedPlayer));
                            }
                        }
                    }

                }
            }
            else
            {
                GunLib_again.GunCleanUp();
            }
        }
        public static void MatAll()
        {
            foreach(NetPlayer p in NetworkSystem.Instance.AllNetPlayers)
            {
                MatPlayer(p);
            }
        }
    }
}
