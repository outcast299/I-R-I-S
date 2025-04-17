using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
public class VaultLock : BaseUnityPlugin
{
    private const string SellerKey = "";
    private const string ApiEndpoint = "https://keyauth.win/api/seller/";
    public static bool IsAuthenticated = true;
    private string _licenseKey = "";
    private string _statusMessage = "";
    private bool _isProcessing = false;
    private Rect windowRect = new Rect(Screen.width / 2 - 125, Screen.height / 2 - 100, 250, 200);
    public bool isdone = false;

    private void OnGUI()
    {
        if (isdone)
        {
            windowRect = GUI.Window(1, windowRect, WindowFunction, "");
        }
    }

    private void WindowFunction(int windowID)
    {
        GUILayout.BeginArea(new Rect(10, 30, 230, 160));
        GUILayout.Label("Key:");
        _licenseKey = GUILayout.TextField(_licenseKey, GUILayout.Width(210), GUILayout.Height(25));
        if (GUILayout.Button("Authenticate", GUILayout.Height(25)))
        {
            if (string.IsNullOrEmpty(_licenseKey.Trim()))
            {
                _statusMessage = "Enter key.";
            }
            else if (!_isProcessing)
            {
                _isProcessing = true;
                _statusMessage = "Checking...";
                StartCoroutine(CheckLicense(_licenseKey));
            }
        }
        GUILayout.Space(10);
        GUIStyle statusStyle = new GUIStyle(GUI.skin.label);
        statusStyle.normal.textColor = _statusMessage.StartsWith("Success") ? Color.green : Color.red;
        GUILayout.Label(_statusMessage, statusStyle);
        GUILayout.EndArea();
    }

    private IEnumerator CheckLicense(string licenseKey)
    {
        var parameters = new Dictionary<string, string>
        {
            { "sellerkey", SellerKey },
            { "type", "verify" },
            { "key", licenseKey }
        };

        string query = string.Join("&", parameters.Select(p => $"{UnityWebRequest.EscapeURL(p.Key)}={UnityWebRequest.EscapeURL(p.Value)}"));
        string url = $"{ApiEndpoint}?{query}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            _isProcessing = false;

            if (request.result != UnityWebRequest.Result.Success)
            {
                _statusMessage = "No connection.";
                IsAuthenticated = false;
                yield break;
            }

            try
            {
                var data = JsonUtility.FromJson<KeyAuthResponse>(request.downloadHandler.text);

                if (data.success)
                {
                    _statusMessage = "Success!";
                    IsAuthenticated = true;
                    isdone = false; 
                }
                else
                {
                    IsAuthenticated = false;
                    _statusMessage = data.message switch
                    {
                        "Key Not Found" => "Wrong key.",
                        "Key Banned" => "Banned key.",
                        "Key Expired" => "Expired key.",
                        _ => data.message
                    };
                }
            }
            catch
            {
                _statusMessage = "Error.";
                IsAuthenticated = false;
            }
        }
    }

    [System.Serializable]
    private struct KeyAuthResponse
    {
        public bool success;
        public string message;
    }
}