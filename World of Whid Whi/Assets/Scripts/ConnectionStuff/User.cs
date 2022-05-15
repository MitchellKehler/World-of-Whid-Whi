using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    //public Text ConnectedText;
    //public Camera MainCamera;
    float Timer = 0.0f;
    public Text ErrorText;
    public Connection Connection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ErrorText.text = ;

        Timer += Time.deltaTime;
        if (Timer >= 1) // a second has elapsed 
        {
            Timer = 0;
            if (!NetworkManager.Singleton.IsConnectedClient)
            {
                //ErrorText.text = "Not connected to the Server.";
                Connection.ConnectToServer();
            }
        }
    }


    //[ServerRpc]
    //public void OnClientConnectedCallback(int val)
    //{
    //    Debug.Log("Creating Client.");
    //    this.gameObject.GetComponent<User>().SetCameraFollowClientRpc(0);
    //}

    //[ClientRpc]
    //public void SetCameraFollowClientRpc(int randomInteger, ClientRpcParams clientRpcParams = default)
    //{
    //    ConnectedText.text = "Connected!";
    //    MainCamera.GetComponent<CameraFollow>().player = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.transform;
    //}

    //[ServerRpc]
    //public void SubmitLogRequestServerRpc(ServerRpcParams rpcParams = default)
    //{
    //    if (NetworkManager.Singleton.IsConnectedClient && IsClient)
    //    {
    //        Debug.Log("Client " + OwnerClientId + " is talking to the Server.");
    //        Debug.Log("Message: isConnected = " + NetworkManager.Singleton.IsConnectedClient.ToString());
    //    }
    //}



}
