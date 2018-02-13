using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spells : MonoBehaviour {

    public float slowfallManaCost = 0.25f;
    public float slowfallGravityConst = 0.1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.R)){
            Slowfall(true);
        }
        if(Input.GetKey(KeyCode.R)){
            GetComponent<Character_Mana>().SpendMana(slowfallManaCost);
            if (!GetComponent<Character_Mana>().SpendMana(slowfallManaCost)){
                Slowfall(false);
            }
        }
        if (Input.GetKeyUp(KeyCode.R)){
            Slowfall(false);
        }
        if (GetComponent<Player_Move>().grounded){
            Slowfall(false);
        }

	}

    void Slowfall(bool active){
        if (active){
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<Rigidbody2D>().gravityScale * slowfallGravityConst;
        }
        else{
            GetComponent<Rigidbody2D>().gravityScale = 10;

        }
    }
}
