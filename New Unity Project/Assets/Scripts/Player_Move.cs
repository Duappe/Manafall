using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour {

    private readonly float WalkSpeed = 5f, FallSpeed = 3f;

    private enum PlayerState { IDLE, WALKING, JUMPING }

    private PlayerState currentState;

    private bool input_MoveLeft, input_MoveRight, input_Jump;

    private SpriteRenderer sprite;

    private float velocityX, VelocityY;

    public bool grounded;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        currentState = PlayerState.IDLE;
    }

    void Update()
    {
        RegisterInput();

        switch (currentState)
        {
            case PlayerState.IDLE:
                UpdateIdle();
                break;
            case PlayerState.WALKING:
                UpdateWalking();
                break;
            case PlayerState.JUMPING:
                UpdateJumping();
                break;
        }
    }

    void UpdateIdle()
    {
        if (!CheckFloorCollision())
        {
            InitFalling();
            return;
        }

        if (input_Jump)
        {
            InitJumping();
            return;
        }

        if (input_MoveLeft || input_MoveRight)
        {
            currentState = PlayerState.WALKING;
            return;
        }
    }

    void UpdateWalking()
    {
        if (!CheckFloorCollision())
        {
            InitFalling();
            return;
        }

        if (input_Jump)
        {
            InitJumping();
            return;
        }

        if (input_MoveLeft && !CheckLeftWallCollision())
        {
            sprite.flipX = true;
            transform.Translate(Vector3.left * WalkSpeed * Time.deltaTime);
        }
        else if (input_MoveRight && !CheckRightWallCollision())
        {
            sprite.flipX = false;
            transform.Translate(Vector3.right * WalkSpeed * Time.deltaTime);
        }
        else
        {
            currentState = PlayerState.IDLE;
        }
    }

    void InitFalling()
    {
        ResetVelocity();
        currentState = PlayerState.JUMPING;
    }

    void InitJumping()
    {
        ResetVelocity();
        VelocityY = 14f;
        currentState = PlayerState.JUMPING;
        grounded = false;
    }

    void UpdateJumping()
    {
        if (CheckRoofCollision())
        {
            VelocityY = -1f;
        }

        
        float gravity = GetComponent<Player_Spells>().slowfalling ? 9.82f * GetComponent<Player_Spells>().slowfallGravityConst : 9.82f;

        transform.Translate(Vector3.up * VelocityY * Time.deltaTime);
        if (input_Jump)
        {
            VelocityY -= gravity * 3 * Time.deltaTime;
        }
        else
        {
            VelocityY -= gravity * 6 * Time.deltaTime;
        }

        if (input_MoveLeft && !CheckLeftWallCollision())
        {
            sprite.flipX = true;
            transform.Translate(Vector3.left * WalkSpeed * Time.deltaTime);
        }
        else if (input_MoveRight && !CheckRightWallCollision())
        {
            sprite.flipX = false;
            transform.Translate(Vector3.right * WalkSpeed * Time.deltaTime);
        }

        if (VelocityY < 0f && CheckFloorCollision())
        {
            SnapToGround();
            currentState = PlayerState.IDLE;
            return;
        }
    }

    void RegisterInput()
    {
        input_MoveLeft = Input.GetKey(KeyCode.A) ? true : false;
        input_MoveRight = Input.GetKey(KeyCode.D) ? true : false;
        input_Jump = Input.GetKey(KeyCode.Space) ? true : false;
    }

    bool CheckFloorCollision()
    {
        RaycastHit2D floorHit;

        int bitmask = 1 << LayerMask.NameToLayer("Terrain");
        floorHit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, Vector3.down, 0.2f, bitmask);

        if (floorHit)
        {
            return true;
        }

        return false;
    }

    void SnapToGround()
    {
        RaycastHit2D floorHit;

        int bitmask = 1 << LayerMask.NameToLayer("Terrain");

        floorHit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, Vector3.down, 0.2f, bitmask);

        if (floorHit)
        {
            grounded = true;
            transform.localPosition = new Vector3(transform.localPosition.x, floorHit.point.y + 0.4f, 0);
        }
    }

    bool CheckLeftWallCollision()
    {
        RaycastHit2D wallHit;

        int bitmask = 1 << LayerMask.NameToLayer("Terrain");

        wallHit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, Vector3.left, 0.2f, bitmask);

        return wallHit ? true : false;
    }

    bool CheckRightWallCollision()
    {
        RaycastHit2D wallHit;

        int bitmask = 1 << LayerMask.NameToLayer("Terrain");

        wallHit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, Vector3.right, 0.2f, bitmask);

        return wallHit ? true : false;
    }

    bool CheckRoofCollision()
    {
        RaycastHit2D roofHit;

        int bitmask = 1 << LayerMask.NameToLayer("Terrain");

        roofHit = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, Vector3.up, 0.2f, bitmask);

        return roofHit ? true : false;
    }

    void ResetVelocity()
    {
        velocityX = 0;
        VelocityY = 0;
    }

    public bool isDescending()
    {
        return currentState == PlayerState.JUMPING && VelocityY < 0;
    }
}
