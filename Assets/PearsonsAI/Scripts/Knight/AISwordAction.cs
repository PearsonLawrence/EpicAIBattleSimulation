using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AISwordAction : MonoBehaviour {

    public Transform Sword, Sword_Ueq, Sword_Eq;
    public bool sword_is_equipped;
    Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Sword_i");
        }

        if (sword_is_equipped)
        {
            Sword.position = Sword_Eq.position;
            Sword.rotation = Sword_Eq.rotation;
        }
        else
        {
            Sword.position = Sword_Ueq.position;
            Sword.rotation = Sword_Ueq.rotation;
        }
	}

    public void Sword_Equip()
    {
        if (sword_is_equipped == false)
        {
            sword_is_equipped = true;
        }
        else
        {
            sword_is_equipped = false;
        }
    }
    public void Sword_UnEquip()
    {
        if (sword_is_equipped == true)
        {
            sword_is_equipped = false;
        }
    }
}
