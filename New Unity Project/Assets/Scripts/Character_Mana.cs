using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Mana : MonoBehaviour {

    public float currentMana{ get; set; }
    public float maxMana{ get; set;}
    public float manaRegen = 1.0f;

    public Slider manaBar;

	// Use this for initialization
	void Start () {
        maxMana = 100f;
        currentMana = maxMana;

        manaBar.value = CalculateMana();
	}
	
	// Update is called once per frame
	void Update () {
        RegenMana();
        manaBar.value = CalculateMana();
	}
    public bool SpendMana(float manaToSpend){
        if(currentMana >= manaToSpend){
            currentMana -= manaToSpend;
            return true;
        }
        else{
            return false;
        }
    }
    
    float CalculateMana(){
        return currentMana / maxMana;
    }


    void RegenMana(){
        if (GetComponent<Player_Move>().grounded){
            if (currentMana < maxMana){
                currentMana += manaRegen * Time.deltaTime;
            }
            else{
                currentMana = maxMana;
            }
        }
    }
}
