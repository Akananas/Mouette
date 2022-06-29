using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouetteScript : MonoBehaviour, IHitComp
{
    enum MouetteState{
        LookingForBoat, Chasing, Hitted,
    };

    MouetteState mouetteState;

    public void Hit(){
    }
}
