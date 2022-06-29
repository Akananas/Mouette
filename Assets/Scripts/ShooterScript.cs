using System;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    bool m_CanShoot;
    bool m_UseBomb;
    [SerializeField]float m_Radius;
    [SerializeField]float m_BombRadius;
    [SerializeField]LayerMask m_HittableObjects;
    Action<Vector2> m_CurrentShoot;
    
    void Start(){
        m_CanShoot = true;
        m_UseBomb = false;
        m_CurrentShoot = BasicShoot;
    }
    
    void Update(){
        if(Input.GetMouseButton(0) && m_CanShoot){
            m_CanShoot = false;
            Vector2 _mousePosition = Input.mousePosition;
            _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
            Debug.Log("Shoot");
            m_CurrentShoot?.Invoke(_mousePosition);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            m_UseBomb = true;
            m_CurrentShoot = BombShoot;
        }
    }
    
    void BasicShoot(Vector2 position){
        Collider2D[] _hitObjects = Physics2D.OverlapCircleAll(position, m_Radius, m_HittableObjects);
        foreach (var obj in _hitObjects){
            Debug.Log("Hit object");
        }
        GameManager.Inst?.AddTimer(Reload, 0.5f);
    }

    void BombShoot(Vector2 position){
        Collider2D[] _hitObjects = Physics2D.OverlapCircleAll(position, m_BombRadius, m_HittableObjects);
        HashSet<Collider2D> _hitObjectsCenter = new HashSet<Collider2D>(Physics2D.OverlapCircleAll(position, m_Radius, m_HittableObjects));
        foreach (var obj in _hitObjects){
            if(_hitObjectsCenter.Contains(obj)){
                Debug.Log("In center");
            }
            else{
                Debug.Log("In bomb radius");
            }
        }
        GameManager.Inst?.AddTimer(Reload, 0.5f);
        m_CurrentShoot = BasicShoot;
    }

    void Reload(){
        m_CanShoot = true;
    }
}
