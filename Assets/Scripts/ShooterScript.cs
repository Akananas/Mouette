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
    [SerializeField]float m_BaseFireRate;
    float m_FireRateMultiplier;
    Action<Vector2> m_CurrentShoot;
    Timer m_ReloadTimer;

    Animator animator;
    
    void Start(){
        m_CanShoot = true;
        m_UseBomb = false;
        m_CurrentShoot = BasicShoot;
        m_FireRateMultiplier = 1.0f;
        animator = GetComponent<Animator>();
    }
    
    void Update(){
        if(!GameManager.Inst.IsPlaying){
            return;
        }

        if(Input.GetMouseButton(0) && m_CanShoot){
            m_CanShoot = false;
            Vector2 _mousePosition = Input.mousePosition;
            _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
            Debug.Log("Shoot");
            m_CurrentShoot?.Invoke(_mousePosition);
        }

        if(Input.GetKeyDown(KeyCode.Space) && !m_UseBomb){
            m_UseBomb = true;
            m_CurrentShoot = BombShoot;
            //Problème si le firerate est égal à 0
            if(m_ReloadTimer != null && m_ReloadTimer.IsActive()){
                m_ReloadTimer.AddTime(2.5f - m_ReloadTimer.GetRemainingTime());
                Debug.Log("Already exist");
            }
            else{
                m_CanShoot = false;
                m_ReloadTimer = GameManager.Inst.AddTimer(Reload, 2.5f);
            }
        }
    }
    
    void BasicShoot(Vector2 position)
    {
        animator.SetBool("CanShoot", false);
        animator.SetTrigger("Shoot");
        Collider2D[] _hitObjects = Physics2D.OverlapCircleAll(position, m_Radius, m_HittableObjects);
        foreach (var obj in _hitObjects){
            obj.GetComponentInParent<IHitComp>().Hit();
        }
        m_ReloadTimer = GameManager.Inst?.AddTimer(Reload, m_BaseFireRate * m_FireRateMultiplier);
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
        m_ReloadTimer = GameManager.Inst?.AddTimer(Reload, m_BaseFireRate * m_FireRateMultiplier);
        m_UseBomb = false;
        m_CurrentShoot = BasicShoot;
    }

    void Reload(){
        m_CanShoot = true;
        animator.SetBool("CanShoot", true);
    }
}
