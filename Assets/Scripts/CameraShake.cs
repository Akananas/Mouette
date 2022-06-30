using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 basePos;
    [SerializeField]float shakeTimer;
    [SerializeField]float shakeMagnitude;
    private void Awake() {
        basePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = basePos + Random.insideUnitSphere * shakeMagnitude;
            
            shakeTimer -= Time.deltaTime;
            }
            else
            {
            shakeTimer = 0f;
            transform.localPosition = basePos;
        }
    }

    public void StartShaking(float newTime){
        shakeTimer = newTime;
    }
}