using Photon.Realtime;
using Photon.Pun;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace StupidTemplate.Classes
{
    internal class RigManager : BaseUnityPlugin
    {
        public static VRRig GetVRRigFromPlayer(NetPlayer p)
        {
            return GorillaGameManager.instance.FindPlayerVRRig(p);
        }

        public static VRRig GetRandomVRRig(bool includeSelf)
        {
            Player randomPlayer;
            if (includeSelf)
                randomPlayer = PhotonNetwork.PlayerList[UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length - 1)];
            else
                randomPlayer = PhotonNetwork.PlayerListOthers[UnityEngine.Random.Range(0, PhotonNetwork.PlayerListOthers.Length - 1)];

            return GetVRRigFromPlayer(randomPlayer);
        }

        public static VRRig GetClosestVRRig()
        {
            float num = float.MaxValue;
            VRRig outRig = null;
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position) < num && vrrig != GorillaTagger.Instance.offlineVRRig)
                {
                    num = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position);
                    outRig = vrrig;
                }
            }
            return outRig;
        }

        public static PhotonView GetPhotonViewFromVRRig(VRRig p)
        {
            return GetNetworkViewFromVRRig(p).GetView;
        }

        public static NetworkView GetNetworkViewFromVRRig(VRRig p)
        {
            return (NetworkView)Traverse.Create(p).Field("netView").GetValue();
        }

        public static Photon.Realtime.Player GetRandomPlayer(bool includeSelf)
        {
            if (includeSelf)
                return PhotonNetwork.PlayerList[UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length - 1)];
            else
                return PhotonNetwork.PlayerListOthers[UnityEngine.Random.Range(0, PhotonNetwork.PlayerListOthers.Length - 1)];
        }

        public static Player NetPlayerToPlayer(NetPlayer p)
        {
            return p.GetPlayerRef();
        }

        public static NetPlayer GetPlayerFromVRRig(VRRig p)
        {
            //return GetPhotonViewFromVRRig(p).Owner;
            return p.Creator;
        }

        public static NetPlayer GetPlayerFromID(string id)
        {
            NetPlayer found = null;
            foreach (Photon.Realtime.Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found;
        }
        public static VRRig GetClosestTagged()
        {
            VRRig vrrig = null;
            float num = float.MaxValue;
            foreach (VRRig vrrig2 in GorillaParent.instance.vrrigs)
            {
                float num2 = Vector3.Distance(vrrig2.transform.position, GorillaTagger.Instance.offlineVRRig.transform.position);
                bool flag = num2 < num && vrrig2.mainSkin.material.name.Contains("fected") && vrrig2 != GorillaTagger.Instance.offlineVRRig;
                if (flag)
                {
                    vrrig = vrrig2;
                    num = num2;
                }
            }
            return vrrig;
        }
    }
}