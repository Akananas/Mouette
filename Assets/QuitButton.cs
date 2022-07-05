using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    //A tester
    #if UNITY_WEBGL
    private void Awake(){
        this.gameObject.SetActive(false);
    }
    #endif
}
