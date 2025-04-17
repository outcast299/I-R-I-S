using StupidTemplate.Classes;
using StupidTemplate.Menu;
using Iris.Mods;
using UnityEngine;
using GorillaNetworking;
using Photon.Pun;
using IRIS.Mods;
using Iris.Menu;
using StupidTemplate;

namespace IRIS.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods
                new ButtonInfo { buttonText = "Settings", method =()=> Main.buttonsType=1, isTogglable=false },
                new ButtonInfo { buttonText = "Computer", method =()=>Main.buttonsType=4, isTogglable = false},
                new ButtonInfo { buttonText = "Movement", method =()=>Main.buttonsType = 5, isTogglable = false},
                new ButtonInfo { buttonText = "Rig", method =()=> Main.buttonsType = 6, isTogglable = false},
                new ButtonInfo { buttonText = "Visuals", method =()=> Main.buttonsType = 7, isTogglable = false},
                new ButtonInfo { buttonText = "Fun", method =()=>  Main.buttonsType = 8, isTogglable = false},
                new ButtonInfo { buttonText = "Master", method=() => Main.buttonsType = 9, isTogglable = false},
                new ButtonInfo { buttonText = "Overpowered", method =()=> Main.buttonsType = 10, isTogglable = false},

            },

            new ButtonInfo[] { // settings
                new ButtonInfo { buttonText = "Menu Settings", method =() => Main.buttonsType = 2, isTogglable = false},
                new ButtonInfo { buttonText = "Mod Settings", method =() => Main.buttonsType = 3, isTogglable = false},
                new ButtonInfo { buttonText = "RPCS", method = () => Settings.DumpRPCData(), isTogglable = false },
            },

            new ButtonInfo[] { // menu
                new ButtonInfo { buttonText = "Change Button Sound", method = () => SettingsMods.ButtonSound(), isTogglable = false },
                new ButtonInfo { buttonText = "Right Hand", enableMethod =() => SettingsMods.LeftHand(), disableMethod =() => SettingsMods.RightHand()},
            },

            new ButtonInfo[] { // mod
                //new ButtonInfo { buttonText = "change fly speed", method = () => SettingsMods.flyspeed(), isTogglable = false },
                new ButtonInfo { buttonText = "Change Platform Size", overlapText = "Change Platform Size <color=grey>[</color><color=red></color><color=grey>]</color>", method =() => Global.ChangePlatformSize(), isTogglable = false},
                new ButtonInfo { buttonText = "Change Fly Speed", overlapText = "Change Fly Speed <color=grey>[</color><color=red></color><color=grey>]</color>", method =() => Global.ChangeFlySpeed(), isTogglable = false},

                new ButtonInfo { buttonText = "Change Speedboost Speed", method = () => SettingsMods.speedspeed(), isTogglable = false },


            },


            new ButtonInfo[] { // Computer/room/safty
                new ButtonInfo { buttonText = "Quit Game", method = () => Application.Quit(), isTogglable = false },
                new ButtonInfo { buttonText = "Disconenct", method = () => PhotonNetwork.Disconnect(), isTogglable = false },
                new ButtonInfo { buttonText = "Disconenct [lp | backspace]", isTogglable = true, method =() => Global.LPD()},

                new ButtonInfo { buttonText = "Anti-Report", method = () => Global.AntiReportD(), isTogglable = true },
                
                //new ButtonInfo { buttonText = "Join Random", method = () => PhotonNetworkController.Instance.AttemptToJoinPublicRoom(GameObject.Find("Environment Objects/TriggerZones_Prefab/JoinRoomTriggers_Prefab/JoinPublicRoom - Forest, Tree Exit").GetComponent<GorillaNetworkJoinTrigger>(), 0), isTogglable = false },
                new ButtonInfo { buttonText = "USW Servers", method = () => PhotonNetwork.ConnectToRegion("usw"), isTogglable = false },
                new ButtonInfo { buttonText = "EU Servers", method = () => PhotonNetwork.ConnectToRegion("eu"), isTogglable = false },
                new ButtonInfo { buttonText = "Disable Network Triggers", enableMethod =() => Global.DisableNetworkTriggers(), disableMethod =() => Global.EnableNetworkTriggers()},
                new ButtonInfo { buttonText = "Disable Map Triggers", enableMethod =() => Global.DisableMapTriggers(), disableMethod =() => Global.EnableMapTriggers()},
                new ButtonInfo { buttonText = "Disable Quit Box", enableMethod =() => Global.DisableQuitBox(), disableMethod =() => Global.EnableQuitBox()},

            },

            new ButtonInfo[] { // player
                new ButtonInfo { buttonText = "Platforms", method = () => Global.PlatformsbyCha(), isTogglable = true },
                new ButtonInfo { buttonText = "Trigger Platforms", method = () => Global.PlatformsbyChaTrigger(), isTogglable = true },
                new ButtonInfo { buttonText = "LT Toggle Platforms", method = () => Global.TogglePlatforms(), isTogglable = true },
                new ButtonInfo { buttonText = "Noclip [RT | E]", method = () => movem.NoclipMod(), isTogglable = true },
                new ButtonInfo { buttonText = "Speedboost", isTogglable = true, method =() => movem.Speedboost()},
                new ButtonInfo { buttonText = "RG Speedboost", isTogglable = true, method =() => movem.RGSpeedboost()},

                new ButtonInfo { buttonText = "Fly", method = () => Global.Fly(), isTogglable = true },
                new ButtonInfo { buttonText = "Trigger fly", method = () => Global.TriggerFly1(), isTogglable = true },
                new ButtonInfo { buttonText = "Joystick Fly", method = () => Global.JoystickFly1(), isTogglable = true },
                new ButtonInfo { buttonText = "Superman Fly", method = () => Global.SlingFly1(), isTogglable = true },
                new ButtonInfo { buttonText = "Sling No G Fly", method = () => Global.ZeroGravitySlingshotFly1(), isTogglable = true },
                new ButtonInfo { buttonText = "Iron Man", method = () => Global.IronMan(), isTogglable = true },
                new ButtonInfo { buttonText = "Bark Fly", method = () => Global.BarkFly1(), isTogglable = true },
                new ButtonInfo { buttonText = "Up And Down", method = () => Global.UpAndDown(), isTogglable = true },
                new ButtonInfo { buttonText = "Forward And Back", method = () => Global.ForwardsAndBackwards(), isTogglable = true },
                new ButtonInfo { buttonText = "Left And Right", method = () => Global.LeftAndRight(), isTogglable = true },
                new ButtonInfo { buttonText = "Tleleport gun", method = () => movem.TpGun(), isTogglable = true },
                new ButtonInfo { buttonText = "No tag freeze", method = () => movem.NoTagFreeze(), isTogglable = true },


            },
            new ButtonInfo[] { // rig
                new ButtonInfo { buttonText = "Ghost [A | R]", isTogglable = true, method =() => movem.GhostMonke()},
                new ButtonInfo { buttonText = "Invis [X | T]", isTogglable = true, method =() => movem.InvisibleMonke()},
                new ButtonInfo { buttonText = "Hitler salute [RG | RMB]", isTogglable = true, method =() => movem.HeilHitler()},
                new ButtonInfo { buttonText = "Steam long arms", isTogglable = true, enableMethod =() => movem.SArms(), disableMethod =() => movem.ResetArms() },
                new ButtonInfo { buttonText = "Really long arms", isTogglable = true, enableMethod =() => movem.RArms(), disableMethod =() => movem.ResetArms() },
                new ButtonInfo { buttonText = "Tag Gun", method =() => Global.TagGun(), enabled = false},
                new ButtonInfo { buttonText = "Tag All", method =() => Global.TagAll(), enabled = false},
            },

            new ButtonInfo[] { // visuals
                new ButtonInfo { buttonText = "Chams", enableMethod = () => visuals.Chams(), disableMethod = () => visuals.DisableChams(),isTogglable = true },
                new ButtonInfo { buttonText = "Beacons", isTogglable = true, method =() => visuals.BeaconESP()},
                new ButtonInfo { buttonText = "Wireframe", isTogglable = true, method =() => visuals.WireFrameESP()},
                new ButtonInfo { buttonText = "Nametags", isTogglable = true, method =() => visuals.Names()},

                new ButtonInfo { buttonText = "Head ESP", method =() => Global.HeadChams(), disableMethod =() => Global.UndoChams(),enabled = false},
                new ButtonInfo { buttonText = "Box ESP", method =() => Global.BoxESP(), enabled = false},
                new ButtonInfo { buttonText = "Hit Boxes", method =() => Global.Hitboxes(), enabled = false},
                new ButtonInfo { buttonText = "Sphere Frame ESP", method =() => Global.CircleFrame(), isTogglable = true},
                new ButtonInfo { buttonText = "Triangle Frame ESP", method =() => Global.TriangleFrame(), isTogglable = true},
                new ButtonInfo { buttonText = "Square Frame ESP", method =() => Global.SquareFrame(), isTogglable = true},
                new ButtonInfo { buttonText = "Square/Graph Frame ESP", method =() => Global.SquareGraphFrame(), isTogglable = true},
                new ButtonInfo { buttonText = "Swastica Test ESP", method =() => Global.SwasticatestFrame(), isTogglable = true},
                new ButtonInfo { buttonText = "Player Trail", method =() => Global.PlayerTrail(), enabled = false},
            },

            new ButtonInfo[] { // fun
                new ButtonInfo { buttonText = "SS Camera Spam", method = () => fun.SSCameraSpammer(), isTogglable = true },
                new ButtonInfo { buttonText = "SS Camera MiniGun", method = () => fun.SSCameraMinigun(), isTogglable = true },
                new ButtonInfo { buttonText = "Hoverboard Spammer", method = () => fun.HoverboardSpam(), isTogglable = true },
                new ButtonInfo { buttonText = "Grab Bug", method =() => Global.GrabBug1(), enabled = false},
                new ButtonInfo { buttonText = "Bug Halo", method =() => Global.BugHalo1(), enabled = false},
                new ButtonInfo { buttonText = "Break Bug", method =() => Global.BreakBug1(), enabled = false},
                new ButtonInfo { buttonText = "Steal Bug", method =() => Global.StealBug1(), enabled = false},
                new ButtonInfo { buttonText = "Ride Bug", method =() => Global.RideBug1(), enabled = false},
                new ButtonInfo { buttonText = "Spaz Bug", method =() => Global.SpazBug1(), enabled = false},
                new ButtonInfo { buttonText = "Bug Tracers", method =() => Global.BugTraces(), isTogglable = true},
                new ButtonInfo { buttonText = "BugSizeChanger[LG & RG & A]", method =() => Global.BugSizeChanger(), isTogglable = true},

                new ButtonInfo { buttonText = "Grab Bat", method =() => Global.GrabBat1(), enabled = false},
                new ButtonInfo { buttonText = "Bat Halo", method =() => Global.BatHalo1(), enabled = false},
                new ButtonInfo { buttonText = "Steal Bat", method =() => Global.StealBat1(), enabled = false},
                new ButtonInfo { buttonText = "Break Bat", method =() => Global.BreakBat1(), enabled = false},
                new ButtonInfo { buttonText = "Ride Bat", method =() => Global.RideBat1(), enabled = false},
                new ButtonInfo { buttonText = "Spaz Bat", method =() => Global.SpazBat1(), enabled = false},
                new ButtonInfo { buttonText = "Bat Tracers", method =() => Global.BatTraces(), isTogglable = true},
                new ButtonInfo { buttonText = "BatSizeChanger[LG & RG & A]", method =() => Global.BatSizeChanger(), isTogglable = true},
                new ButtonInfo { buttonText = "Old Ramp", method =() => Global.OldRamp(), disableMethod =() => Global.UnOldRamp()},
                new ButtonInfo { buttonText = "100 Quest Score", method =() => fun.QuestScore(100)},
                new ButtonInfo { buttonText = "Custom Quest Score", method =() => fun.QuestScore(99999)},
                new ButtonInfo { buttonText = "Critter Spam [Max 40]", method =() => fun.SpawnCritter()},
                new ButtonInfo { buttonText = "Critter Minigun [Max 40]", method =() => fun.LaunchCritter()},
                new ButtonInfo { buttonText = "StunBomb Spam [Max 99]", method =() => fun.SpawnStunBomb()},
                new ButtonInfo { buttonText = "Stun Bomb Minigun [Max 99]", method =() => fun.LaunchStunBomb()},
                new ButtonInfo { buttonText = "StickTrap Spam [Max 99]", method =() => fun.SpawnStickyTrap()},
                new ButtonInfo { buttonText = "StickTrap Minigun [Max 99]", method =() => fun.LaunchStickyTrap()},
                new ButtonInfo { buttonText = "Glider Gun", method =() => fun.GliderGun(), enabled = false},
                new ButtonInfo { buttonText = "Glider Spammer", method =() => fun.GliderPSammer(), enabled = false},
            },

            new ButtonInfo[] { // master
                new ButtonInfo { buttonText = "Soon............", method =() => master.MatAll(), isTogglable = true},
            },

            new ButtonInfo[] { // op
                 new ButtonInfo { buttonText = "Lag Gun", method = () => Exploits.LagGun(), isTogglable = true },
                 new ButtonInfo { buttonText = "Lag All", method = () => Exploits.LagAll(), isTogglable = true },
                 new ButtonInfo { buttonText = "Kick Gun [Join Party]", method = () => Exploits.KickGun(), isTogglable = true },
                 new ButtonInfo { buttonText = "Kick All [Join Party]", method = () => Exploits.KickAll(), isTogglable = true },
                 new ButtonInfo { buttonText = "Crash Gun [Held too long invovles with rpc kick]", method = () => Exploits.CrashGun(), isTogglable = true },
                 new ButtonInfo { buttonText = "Crash Al [Held too long invovles with rpc kick]", method = () => Exploits.CrashAll(), isTogglable = true },
                 new ButtonInfo { buttonText = "App Quit All Violet Users [creds cosmic]", method = () => Exploits.AppQuitAllVioletUsers(), isTogglable = true },
                 new ButtonInfo { buttonText = "App Quit Gun Violet Users [creds cosmic]", method = () => Exploits.AppQuitGunVioletusers(), isTogglable = true },
                 new ButtonInfo { buttonText = "Shop Lift", method = () => Exploits.popAllballons(), isTogglable = true },
                 new ButtonInfo { buttonText = "Spam Balloon [qquip the balloon unless its cs]", method = () => Exploits.BecomeBalloon(), isTogglable = true },
            },


            new ButtonInfo[] { // ignore
                new ButtonInfo { buttonText = "Return to Main", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Returns to the main page of the menu."},
                new ButtonInfo { buttonText = "nextpage", method =() => Main.Page(), isTogglable = true,enabled = true, toolTip = "Opens the home for the menu."},
            },

        };
    }
}
