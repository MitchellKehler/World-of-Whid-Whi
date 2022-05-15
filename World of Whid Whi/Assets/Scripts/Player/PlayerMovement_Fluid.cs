using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerMovement_Fluid : NetworkBehaviour
{
    Rigidbody2D body;
    public bool IsAllowedToMove = true;

    Direction currentDir;
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public Sprite northSprite;
    public Sprite eastSprite;
    public Sprite southSprite;
    public Sprite westSprite;

    public float runSpeed = 0.0f; // 5f

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        currentDir = Direction.East;
    }

    void Update()
    {
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
    }

    void FixedUpdate()
    {
        if (IsAllowedToMove)
        {
            if (horizontal != 0 && vertical != 0) // Check for diagonal movement
            {
                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            if (horizontal != 0 || vertical != 0)
            {
                if (horizontal < 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = westSprite;
                }
                else if (horizontal > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = eastSprite;
                }
                else if (vertical < 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = southSprite;
                }
                else if (vertical > 0)
                    gameObject.GetComponent<SpriteRenderer>().sprite = northSprite;


            }

            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        } else
        {
            body.velocity = new Vector2(0, 0);
        }
    }
}

enum Direction
{
    North,
    South,
    East,
    West
}