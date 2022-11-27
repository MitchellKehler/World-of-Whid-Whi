using Assets.HeroEditor.Common.CharacterScripts;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Prototyping;
using UnityEngine;

public class Player_Movement : NetworkBehaviour
{
    public Character Character;
    GameManager GM;
    public bool IsAllowedToMove = false;
    public ulong MyClientId;

    private Vector3 _speed = Vector3.zero;
    Rigidbody2D body;
    public float runSpeed;
    public Vector3 CharacterSize;

    private NetworkVariable<Vector3> scale = new NetworkVariable<Vector3>();
    private NetworkVariable<CharacterState> state = new NetworkVariable<CharacterState>();

    private void OnEnable()
    {
        scale.OnValueChanged += OnScaleChanged;
        state.OnValueChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        scale.OnValueChanged -= OnScaleChanged;
        state.OnValueChanged -= OnStateChanged;
    }

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Character.Animator.SetBool("Ready", true);
        TurnServerRpc(1);
        ChangeStateServerRpc(CharacterState.Idle);
    }

    public void Update()
    {
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
            TurnServerRpc(_speed.x);
            ChangeStateServerRpc(CharacterState.Run);
        }
        else if (Character.GetState() < CharacterState.DeathB)
        {
            ChangeStateServerRpc(CharacterState.Idle);
        }
        body.velocity = new Vector2(direction.x * runSpeed, direction.y * runSpeed);
        Vector2 NewPostion = new Vector2(transform.position.x, transform.position.y);
        Character.transform.position = NewPostion;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TurnServerRpc(float direction)
    {
        scale.Value = new Vector3(Mathf.Sign(direction) * CharacterSize.x, CharacterSize.y, CharacterSize.z);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeStateServerRpc(CharacterState NewState)
    {
        state.Value = NewState;
    }

    public void OnScaleChanged(Vector3 OldScale, Vector3 NewScale)
    {
        Character.transform.localScale = NewScale;
    }

    public void OnStateChanged(CharacterState OldState, CharacterState NewState)
    {
        Character.SetState(NewState);
    }
}