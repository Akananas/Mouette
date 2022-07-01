using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Mouette")){
            MouetteScript mouette = collision.gameObject.GetComponentInParent<MouetteScript>();
            if (mouette)
            {
                mouette.Plouf(mouette);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("Mouette")){
            MouetteScript mouette = col.gameObject.GetComponentInParent<MouetteScript>();
            if (mouette)
            {
                mouette.RemovePlouf();
            }
        }
    }
}
