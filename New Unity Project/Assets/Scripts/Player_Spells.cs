using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spells : MonoBehaviour {

    // Variables for Slowfall------------------------
    public float slowfallManaCost = 20f;
    public float slowfallGravityConst = 0.1f;
    public bool slowfalling = false;

    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        bool slowfallingInput = Input.GetKey(KeyCode.R);
        if ( 
            GetComponent<Player_Move>().isDescending() &&
            !GetComponent<Player_Move>().grounded && 
            slowfallingInput && 
            GetComponent<Character_Mana>().SpendMana(slowfallManaCost * Time.deltaTime))
        {
            slowfalling = true;
        }
        else
        {
            slowfalling = false;
        }
    }
}
