using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour {

    public float playerSpeed;
    private bool facingRight;
    public float playerJumpPower;
    private float moveX;
    int terrainLayerMask;
    public bool grounded;
    public float distToGround;
    public int againstWall;


	// Use this for initialization
	void Start () {
        playerSpeed = 10;
        playerJumpPower = 1250;
        facingRight = true;
        distToGround = GetComponent<BoxCollider2D>().bounds.extents.y;
        grounded = false;
        againstWall = 0;
        terrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!grounded){
            grounded = IsGrounded();
            againstWall = isAgainstWall();
        }
        Player_Input();
	}

    void Player_Input(){
        //CONTROLS
        moveX = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump")){
            Jump();
        }
        //ANIMATIONS
        //PLAYER DIRECTION
        if (moveX > 0.0f && facingRight == false){
            FlipPlayer();
        }
        else if(moveX <= 0.0f && facingRight == true){
            FlipPlayer();
        }
        //PHYSICS
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

    }
    void Jump(){
        //JUMPING CODE
        if (CheckJump()){
            if (againstWall == 0){
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpPower);
                grounded = false;
            }
            else if(againstWall == 1){
                GetComponent<Rigidbody2D>().AddForce(new Vector2(1.0f,1.0f).normalized * playerJumpPower*2);
                grounded = false;
            }
            else if (againstWall == 2){
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-1.0f, 1.0f).normalized * playerJumpPower*2);
                grounded = false;
            }
            else { }
        }
    }
    void FlipPlayer(){
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    bool CheckJump(){
        // SKA KOMMA MER I DENNA IFFEN OM MANA OCH SLOWFALL OCH STUFF
        if (grounded || againstWall == 1 || againstWall == 2){
            return true;
        }
        else{
            return false;
        }
    }
    bool IsGrounded(){
        
        return Physics2D.Raycast(transform.position, Vector2.down, distToGround + 0.1f, terrainLayerMask);
    }

    int isAgainstWall(){

        if (Physics2D.Raycast(transform.position, Vector2.left, distToGround + 0.1f, terrainLayerMask)){
            return 1;
        }
        else if (Physics2D.Raycast(transform.position, Vector2.right, distToGround + 0.1f, terrainLayerMask)){
            return 2;
        }
        else{
            return 0;
        }
    }
}
