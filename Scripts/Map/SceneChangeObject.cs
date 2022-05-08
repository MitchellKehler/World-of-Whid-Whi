using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeObject : MonoBehaviour
{
    public string MyScene;
    public float x;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Changing scenes to '" + MyScene + "'.");
        //NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.GetComponent<NetworkBehaviour>().
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ChangeMapLocation(MyScene, x, y);
    }
}
