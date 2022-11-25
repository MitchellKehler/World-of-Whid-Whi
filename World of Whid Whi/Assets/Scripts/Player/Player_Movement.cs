using Assets.HeroEditor.Common.CharacterScripts;
using MLAPI;
using UnityEngine;

public class Player_Movement : NetworkBehaviour
{
    public Character Character;
    GameManager GM;
    public bool IsAllowedToMove = false;
    public ulong MyClientId;

    private Vector3 _speed = Vector3.zero;
    public Vector3 scale;
    Rigidbody2D body;
    public float runSpeed;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Character.Animator.SetBool("Ready", true);
    }

    public void Update()
    {
        //// Set Direction
        //var direction = Vector2.zero;

        //if (Input.GetKey(KeyCode.LeftArrow)) direction.x = -1;
        //if (Input.GetKey(KeyCode.RightArrow)) direction.x = 1;
        //if (Input.GetKey(KeyCode.UpArrow)) direction.y = 1;

        if (this.NetworkObject.IsOwner) //!NetworkManager.Singleton.IsServer
        {
            Move(GetDirection());
        }

        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Character.SetState(CharacterState.DeathB);
        //}

        if (NetworkManager.Singleton.IsServer)
        {
            //Debug.Log(MyClientId);
            GM.Server.activeCharacters[MyClientId].Position_X = transform.position.x;
            GM.Server.activeCharacters[MyClientId].Position_Y = transform.position.y;
        }
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = Vector2.zero;

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) direction.x = -1;
            if (Input.GetKey(KeyCode.RightArrow)) direction.x = 1;
            if (Input.GetKey(KeyCode.UpArrow)) direction.y = 1;
            if (Input.GetKey(KeyCode.DownArrow)) direction.y = -1;
        }
        else if (Input.touches.Length != 0 && IsAllowedToMove)
        {
            foreach (Touch touch in Input.touches)
            {
                // convert mouse position into world coordinates
                Vector2 mouseScreenPosition = NetworkManager.ConnectedClients[NetworkManager.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().playerCam.ScreenToWorldPoint(touch.position);
                //Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // get direction you want to point at
                direction = (mouseScreenPosition - (Vector2)transform.position).normalized;
            }
        }
        return direction;
    }

    public void Move(Vector2 direction)
    {
        _speed = new Vector3(5 * direction.x, 5 * direction.y);
        if (direction != Vector2.zero)
        {
            Turn(_speed.x);
            Character.SetState(CharacterState.Run);
        }else if (Character.GetState() < CharacterState.DeathB)
        {
            Character.SetState(CharacterState.Idle);
        }
        body.velocity = new Vector2(direction.x * runSpeed, direction.y * runSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y - .5f);
        //Controller.Move(_speed * Time.deltaTime);
    }

    public void Turn(float direction)
    {
        Character.transform.localScale = new Vector3((Mathf.Sign(direction) * scale.x), scale.y, scale.z);
    }
}