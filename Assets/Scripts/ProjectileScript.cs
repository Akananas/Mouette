using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    Vector2 m_Target;
    bool m_HasTarget;
    float m_BaseDistance;
    float m_Speed;
    Action<Vector2> m_HitDetection;
    public void Launch(Vector2 target, float speed, Action<Vector2> hitDetection){
        m_Target = target;
        m_BaseDistance = Vector2.Distance(m_Target, transform.position);
        m_Speed = speed;
        m_HasTarget = true;
        m_HitDetection = hitDetection;
    }

    void Update(){
        if(!m_HasTarget){
            return;
        }
        transform.position = Vector2.MoveTowards((Vector2)transform.position, m_Target, Time.deltaTime * m_Speed);
        float scale = Mathf.Clamp(Vector2.Distance(m_Target, transform.position) / m_BaseDistance, 0.1f, 1.0f);
        transform.localScale = new Vector3(scale, scale, scale);
        if(scale <= 0.1f){
            m_HitDetection?.Invoke(m_Target);
            Destroy(this.gameObject);
        }
    }
}
