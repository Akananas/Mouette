using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MouetteScript mouette = collision.attachedRigidbody.gameObject.GetComponent<MouetteScript>();
        if (mouette)
        {
            mouette.Plouf();
        }
    }
}
