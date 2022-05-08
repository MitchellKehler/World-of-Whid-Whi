using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class CameraFollow : NetworkBehaviour

{
    //public Transform player;
    public int zOffset = -10;

    //public CameraFollow() {
    //    player = this.gameObject.transform;
    //}

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("At CameraFollow");
        Debug.Log("LocalClientId is " + NetworkManager.LocalClientId);
        Debug.Log("IsServer is " + NetworkManager.IsServer);
        Debug.Log("IsClient is " + NetworkManager.IsClient + "\n");
        //if (!NetworkManager.Singleton.IsServer)
        //{
        //player = this.gameObject.transform;
        //    //player = GameObject.Find("Player(Clone)").transform;
        //}
    }

    public void SetPlayerTransform()
    {
        //player = gameObject.transform;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zOffset);
        //transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
    }
}
