using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Player_Movement_Android : NetworkBehaviour
{
    Direction currentDir;
    public Sprite northSprite;
    public Sprite eastSprite;
    public Sprite southSprite;
    public Sprite westSprite;

    public float runSpeed;
    Rigidbody2D body;
    public bool IsAllowedToMove = false;

    Vector2 mouseScreenPosition;
    Vector2 direction;
    GameManager GM;
    public ulong MyClientId;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        runSpeed = 5f; // In the futre this should be taken from the player's data based on items, mounts, ect. then it should be adjusted based on terrain, weather, ect.
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    void Update()
    {
        if (this.NetworkObject.IsOwner) //!NetworkManager.Singleton.IsServer
        {
            if (Input.touches.Length != 0 && IsAllowedToMove)
            {
                foreach (Touch touch in Input.touches)
                {
                    // convert mouse position into world coordinates
                    mouseScreenPosition = NetworkManager.ConnectedClients[NetworkManager.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().playerCam.ScreenToWorldPoint(touch.position);
                    //Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    // get direction you want to point at
                    direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

                    // set vector of transform directly
                    //transform.up = direction;

                }
                if (direction.x != 0 || direction.y != 0)
                {
                    if (direction.x < 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        SpriteRenderer[] MySprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer sprite in MySprites)
                        {
                            sprite.sprite = westSprite;
                        }
                        //gameObject.GetComponentInChildren<SpriteRenderer>().sprite = westSprite;
                    }
                    else if (direction.x > 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        SpriteRenderer[] MySprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer sprite in MySprites)
                        {
                            sprite.sprite = eastSprite;
                        }
                        //gameObject.GetComponentInChildren<SpriteRenderer>().sprite = eastSprite;
                    }
                    else if (direction.y < 0 && Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                    {
                        SpriteRenderer[] MySprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer sprite in MySprites)
                        {
                            sprite.sprite = southSprite;
                        }
                        //gameObject.GetComponentInChildren<SpriteRenderer>().sprite = southSprite;
                    }
                    else if (direction.y > 0 && Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                    {
                        SpriteRenderer[] MySprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
                        foreach (SpriteRenderer sprite in MySprites)
                        {
                            sprite.sprite = northSprite;
                        }
                        //gameObject.GetComponentInChildren<SpriteRenderer>().sprite = northSprite;
                    }


                }
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(NetworkManager.LocalClientId, "runSpeed: " + runSpeed);
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(NetworkManager.LocalClientId, "direction.x: " + direction.x);
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(NetworkManager.LocalClientId, "direction.x * runSpeed = " + direction.x * runSpeed);
                body.velocity = new Vector2(direction.x * runSpeed, direction.y * runSpeed);
                //SubmitMoveRequestServerRpc(new Vector2(direction.x * runSpeed, direction.y * runSpeed));
            }
            else
            {
                body.velocity = Vector2.zero;
                //SubmitMoveRequestServerRpc(Vector2.zero);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y - .5f);
            
        } if (NetworkManager.Singleton.IsServer)
        {
            //Debug.Log(MyClientId);
            GM.Server.activeCharacters[MyClientId].Position_X = transform.position.x;
            GM.Server.activeCharacters[MyClientId].Position_Y = transform.position.y;
        }
    }

    //[ServerRpc(RequireOwnership = false)]
    //public void UpdatePositionServerRpc(Vector2 vector, ServerRpcParams rpcParams = default)
    //{

    //    Debug.Log("Client " + OwnerClientId + " has moved.");
    //    NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * runSpeed, direction.y * runSpeed);
    //    //    NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    //}


}
