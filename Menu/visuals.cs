using System;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using UnityEngine;

namespace IRIS.Menu
{
    internal class visuals
    {
        public static bool IsPlayerInfected(VRRig player)
        {
            return player.mainSkin.material.name.Contains("fected");
        }
        public static void Chams()
        {
            Shader shader = Shader.Find("GUI/Text Shader");
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (!vrrig.isOfflineVRRig)
                {
                    vrrig.mainSkin.material.shader = shader;
                    vrrig.mainSkin.material.color = vrrig.mainSkin.material.name.Contains("fected") || vrrig.mainSkin.material.name.Contains("It") ? new Color32(255, 0, 0, 255) : new Color32(0, 255, 0, 255);
                }
            }
        }

        public static void DisableChams()
        {
            Shader shader = Shader.Find("GUI/Text Shader");
            Shader uberShader = Shader.Find("GorillaTag/UberShader");
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig && vrrig.mainSkin.material.shader == shader)
                {
                    vrrig.mainSkin.material.shader = uberShader;
                }
            }
        }
        public static void WireFrameESP()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        DrawCube(vrrig.transform.position, Vector3.one * 0.5f, vrrig.mainSkin.material.color);
                    }
                }
            }
        }



        public static void DrawCube(Vector3 pos, Vector3 cubeSize, Color color)
        {
            Vector3[] array = new Vector3[]
            {
                pos + new Vector3(-cubeSize.x / 2f, -cubeSize.y / 2f, -cubeSize.z / 2f),
                pos + new Vector3(cubeSize.x / 2f, -cubeSize.y / 2f, -cubeSize.z / 2f),
                pos + new Vector3(cubeSize.x / 2f, -cubeSize.y / 2f, cubeSize.z / 2f),
                pos + new Vector3(-cubeSize.x / 2f, -cubeSize.y / 2f, cubeSize.z / 2f),
                pos + new Vector3(-cubeSize.x / 2f, cubeSize.y / 2f, -cubeSize.z / 2f),
                pos + new Vector3(cubeSize.x / 2f, cubeSize.y / 2f, -cubeSize.z / 2f),
                pos + new Vector3(cubeSize.x / 2f, cubeSize.y / 2f, cubeSize.z / 2f),
                pos + new Vector3(-cubeSize.x / 2f, cubeSize.y / 2f, cubeSize.z / 2f)
            };
            int[,] array2 = new int[,]
            {
                {
                    0,
                    1
                },
                {
                    1,
                    2
                },
                {
                    2,
                    3
                },
                {
                    3,
                    0
                },
                {
                    4,
                    5
                },
                {
                    5,
                    6
                },
                {
                    6,
                    7
                },
                {
                    7,
                    4
                },
                {
                    0,
                    4
                },
                {
                    1,
                    5
                },
                {
                    2,
                    6
                },
                {
                    3,
                    7
                }
            };
            for (int i = 0; i < array2.GetLength(0); i++)
            {
                DrawLine(array[array2[i, 1]], color, color, array[array2[i, 0]], 0.02f, 1f);
            }
        }
        public static LineRenderer lineRenderer;

        public static void DrawLine(Vector3 p, Color col1, Color col2, Vector3 startingPos, float thickness, float transparency)
        {
            Material material = new Material(Shader.Find("GUI/Text Shader"));
            material.color = Color.Lerp(col1, col2, Mathf.PingPong(Time.time, 0.9f));
            material.SetFloat("_Mode", 2f);
            Color color = material.color;
            color.a = transparency;
            material.color = color;
            GameObject gameObject = new GameObject("Line");
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material.shader = Shader.Find("GUI/Text Shader");
            lineRenderer.startColor = material.color;
            lineRenderer.endColor = material.color;
            lineRenderer.startWidth = thickness;
            lineRenderer.endWidth = thickness;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.SetPosition(0, startingPos);
            lineRenderer.SetPosition(1, p);
            lineRenderer.material.shader = Shader.Find("GUI/Text Shader");
            UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
            UnityEngine.Object.Destroy(gameObject, Time.deltaTime);
            UnityEngine.Object.Destroy(lineRenderer, Time.deltaTime);
        }
        public static void BeaconESP()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != GorillaTagger.Instance.offlineVRRig)
                    {
                        DrawBeacon(vrrig.transform.position, vrrig.mainSkin.material.color);
                    }
                }
            }
        }
        public static void DrawBeacon(Vector3 pos, Color color)
        {
            float height = 500f;
            float width = 0.3f;

            GameObject beacon = new GameObject("Beacon");
            LineRenderer lineRenderer = beacon.AddComponent<LineRenderer>();

            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = color;
            lineRenderer.endColor = new Color(color.r, color.g, color.b, 0.2f);
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;

            lineRenderer.SetPosition(0, pos);
            lineRenderer.SetPosition(1, pos + Vector3.up * height);

            UnityEngine.Object.Destroy(beacon, Time.deltaTime);
        }
        public static void Names()
        {
            foreach (VRRig player in GorillaParent.instance.vrrigs)
            {
                if (player == GorillaTagger.Instance.offlineVRRig) continue;
                GameObject nameTag = new GameObject($"{player.Creator.NickName}");
                TextMesh textMesh = nameTag.AddComponent<TextMesh>();
                textMesh.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                textMesh.fontSize = 20;
                textMesh.fontStyle = FontStyle.Normal;
                textMesh.characterSize = 0.1f;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Center;
                textMesh.color = player.playerColor;
                float distance = Vector3.Distance(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position, player.transform.position);
                textMesh.text =$"{Convert.ToInt32(distance)}m\n" + $"{player.Creator.NickName}\n" + $"{player.Creator.UserId}\n" + $"{player.playerColor}\n";
                nameTag.transform.position = player.headMesh.transform.position + new Vector3(0f, 1.75f, 0f);
                nameTag.transform.LookAt(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position);
                nameTag.transform.Rotate(0f, 180f, 0f);

                UnityEngine.Object.Destroy(nameTag, Time.deltaTime);
            }
        }












    }
}
