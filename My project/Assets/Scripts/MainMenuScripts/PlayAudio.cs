using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
   public GameObject audioObj;
   public void DropAudio(){
    Instantiate(audioObj, transform.position, transform.rotation);
   }
}
