using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public List<InitializedCreatureData> OwnedCreatures;
    public List<InitializedCreatureData> CurrentCreatureTeam;
    public Camera PlayerCamera; // Prefab
    public Camera playerCam;
    public bool inBattle = false;
    public string currentLocation;
    public float PlayerZOffset;

    public bool Battle_Go;

    // Start is called before the first frame update
    void Start()
    {
        Battle_Go = false;
        DontDestroyOnLoad(gameObject);
    }

    public void SetUpPlayer(string location, float posX, float posY)
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        GM.LogToServerRpc(NetworkManager.LocalClientId, "Creating Player");

        //playerCam = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        playerCam = Instantiate(PlayerCamera) as Camera;
        playerCam.gameObject.SetActive(true);
        GM.LogToServerRpc(NetworkManager.LocalClientId, "Camera Set");
        PlayerZOffset = .5f;

        //GM.Player = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject;
        //GM.LogToServerRpc(NetworkManager.LocalClientId, "GM Player Set");

        //GameObject.Find("Is_Connected_Text").GetComponent<Text>().text = "Connected!";
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().player = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.transform;
        //gameObject.GetComponent<Camera>().transform.position = new Vector3(gameObject.GetComponent<Camera>().transform.position.x, gameObject.GetComponent<Camera>().transform.position.y, -10);

        //playerCam.transform.parent = this.gameObject.transform;
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerCamera = playerCam;
        //playerCam.enabled = false;
        float height = GameManager.CAMERA_SIZE_IN_BATTLE * 2.0f;
        float width = height * playerCam.aspect;
        GM.LogToServerRpc(NetworkManager.LocalClientId, "Screen Width : " + width);
        GM.LogToServerRpc(NetworkManager.LocalClientId, "Screen Height : " + height);
        currentLocation = location;
        GM.position_manager = new PositionManager(width, height);
        GM.SetInitialMapLocation(location, posX, posY); // Later this will be which ever location the player signed out in. May need a version of change scene that also sets position.
        //SetUpPlayerServerRpc();


    }

    private void OnMouseDown()
    {
        if (NetworkManager.LocalClientId != this.OwnerClientId)
        {

            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().StartPvpBattle(NetworkManager.LocalClientId, this.OwnerClientId);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.NetworkObject.IsOwner) //!NetworkManager.Singleton.IsServer
        {
            if (inBattle)
            {
                playerCam.transform.position = new Vector3(GameManager.BATTLE_OFFSET.x, GameManager.BATTLE_OFFSET.y, -1000); // tried (transform.position.z - 100) but it broke.
                playerCam.orthographicSize = GameManager.CAMERA_SIZE_IN_BATTLE;
            }
            else
            {
                playerCam.transform.position = new Vector3(transform.position.x, transform.position.y, -1000); // tried (transform.position.z - 100) but it broke.
                playerCam.orthographicSize = GameManager.CAMERA_SIZE_IN_WORLD;
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y - .5f);
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(NetworkManager.LocalClientId, transform.position.ToString());
            }
        }
    }

    //public Camera GetPlayerCam()
    //{
    //    return playerCam;
    //}

    //[ServerRpc]
    //public void SetUpPlayerServerRpc(ServerRpcParams rpcParams = default)
    //{
    //    if (NetworkManager.Singleton.IsServer) //NetworkManager.Singleton.IsServer
    //    {

    //        Debug.Log("Setting up player for Client " + OwnerClientId + ".");
    //        NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures = new List<InitializedCreatureData>();
    //        NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().CurrentCreatureTeam = new List<InitializedCreatureData>();

    //        int count = 0;

    //        Debug.Log("creating owned creatures.");
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures.Add(new InitializedCreatureData(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllCreatures.Find(creature => creature.Name.Equals("Wasp"))));
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().CurrentCreatureTeam.Add(NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures[count++]);
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures.Add(new InitializedCreatureData(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllCreatures.Find(creature => creature.Name.Equals("Wasp"))));
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().CurrentCreatureTeam.Add(NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures[count++]);
    //        NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures.Add(new InitializedCreatureData(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllCreatures["GiantRat"]));
    //        NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().CurrentCreatureTeam.Add(NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures[count++]);
    //        Debug.Log("finished creating owned creatures.");
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures.Add(new InitializedCreatureData(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllCreatures.Find(creature => creature.Name.Equals("Pestarat"))));
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().CurrentCreatureTeam.Add(NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures[count++]);
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures.Add(new InitializedCreatureData(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllCreatures.Find(creature => creature.Name.Equals("GreyWolf"))));
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().CurrentCreatureTeam.Add(NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures[count++]);
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures.Add(new InitializedCreatureData(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllCreatures.Find(creature => creature.Name.Equals("GreaterMooBeast"))));
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().CurrentCreatureTeam.Add(NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures[count++]);
    //        //NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Player>().CurrentCreatureTeam[0]

    //        ///////////// Should get player info from the database ////////////////////

    //        //string randomName = "";
    //        //int randomNum = Random.Range(0, 2);
    //        //switch (randomNum)
    //        //{
    //        //    case 0:
    //        //        randomName = "Rat";
    //        //        break;
    //        //    case 1:
    //        //        randomName = "Rooster";
    //        //        break;
    //        //    default:
    //        //        randomName = "Frog";
    //        //        break;
    //        //}
    //        //NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures.Add(new InitializedCreatureData(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllCreatures.Find(creature => creature.Name.Equals(randomName))));
    //        //NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().Squad_Starting.Add(NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures[0]);

    //        //randomNum = Random.Range(0, 2);
    //        //switch (randomNum)
    //        //{
    //        //    case 0:
    //        //        randomName = "Rat";
    //        //        break;
    //        //    case 1:
    //        //        randomName = "Rooster";
    //        //        break;
    //        //    default:
    //        //        randomName = "Frog";
    //        //        break;
    //        //}
    //        //NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures.Add(new InitializedCreatureData(GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AllCreatures.Find(creature => creature.Name.Equals(randomName))));
    //        //NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().Squad_Starting.Add(NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().OwnedCreatures[1]);
    //    }
    //}


}
