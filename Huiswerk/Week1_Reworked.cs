using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarActivate : MonoBehaviour {

    private GameObject objectOnAltar;
    public GameObject Door { get; set; }
    public PlayerItemSearch ItemSearch { get; set; }
    public bool DoorOn { get; set; }
    public string KeyTag { get; set; }


    private void OnTriggerEnter(Collider other) {                                        //when something enters the collider
        if (other.tag == KeyTag) {                                                       //and is has the tag keyTag
            if (ItemSearch.objectInHand != null) {                                       //and the players hand is not empty
                PlayerItemSearch.DropItem();                                             //empty the players hand.
                ((Rigidbody)other.GetComponent(typeof(Rigidbody))).isKinematic = true;   //make the key kinematic
                other.transform.position = transform.position + new Vector3(0, 2, 0);    //and set it's position perfectly above the altar
                other.gameObject.layer = 0;                                              //place the object another layer to make it impossible to pick up.
                Door.SetActive(true);
            }
            else if (ItemSearch.objectInHand == null) {                                  //if the players hand is empty
                ((Rigidbody)other.GetComponent(typeof(Rigidbody))).isKinematic = true;   //do the exact same stuff except you don't empty their hand.
                other.transform.position = transform.position + new Vector3(0, 2, 0);
                other.gameObject.layer = 0;
                Door.SetActive(true);
            }
        }
    }
}
