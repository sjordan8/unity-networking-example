
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace StarterAssets
{
    public class TutorialGui : MonoBehaviour
    {
        void Start()
        {
            NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        void HandleServerStarted()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                HandleClientConnected(NetworkManager.Singleton.LocalClientId);
            }
        }

        void HandleClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                NetworkObject localPlayer = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;

                CinemachineVirtualCamera camera = localPlayer.GetComponentInChildren(typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
                camera.gameObject.SetActive(true);

                PlayerInput inputs = localPlayer.GetComponent<PlayerInput>();
                inputs.enabled = true;
            }
        }

        void HandleClientDisconnect(ulong clientId)
        {

        }
    }
}