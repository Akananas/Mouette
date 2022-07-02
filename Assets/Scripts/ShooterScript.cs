using System;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    bool m_CanShoot;
    bool m_UseBomb;
    public float shootForce = 50.0f;
    float currentShootForce;
    [SerializeField]float m_Radius;
    [SerializeField]float m_BombRadius;
    [SerializeField]LayerMask m_HittableObjects;
    [SerializeField]float m_BaseFireRate;
    float m_FireRateMultiplier;
    Action<Vector2> m_CurrentShoot;
    Timer m_ReloadTimer;
    ProjectileScript m_CurrentProjectile;
    [SerializeField]Transform m_ProjectileSpawnPoint;
    [SerializeField]GameObject m_ProjectilePrefab;
    [SerializeField]AnimationCurve m_FireRateMultiplierCurve;
    int m_UpgradeLevel;

    Animator animator;
    
    void Start(){
        m_CanShoot = true;
        m_UseBomb = false;
        m_CurrentShoot = BasicShoot;
        animator = GetComponent<Animator>();
        GameManager.Inst.OnRestart += Restart; 
        Reload();
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
        AudioManager.Instance.PlayClip("Shoot");
        GameManager.Inst.CamShaker.StartShaking(0.25f);
        m_CurrentProjectile?.Launch(position, currentShootForce, BasicShootHitDetection);
        m_ReloadTimer = GameManager.Inst?.AddTimer(Reload, m_BaseFireRate / m_FireRateMultiplier);
    }

    void BasicShootHitDetection(Vector2 position){
        Collider2D[] _hitObjects = Physics2D.OverlapCircleAll(position, m_Radius, m_HittableObjects);
        foreach (var obj in _hitObjects){
            obj.GetComponentInParent<IHitComp>().Hit();
        }
    }

    void BombShoot(Vector2 position){
        m_CurrentProjectile?.Launch(position, currentShootForce, BombShootHitDetection);
        m_ReloadTimer = GameManager.Inst?.AddTimer(Reload, m_BaseFireRate / m_FireRateMultiplier);
        m_UseBomb = false;
        m_CurrentShoot = BasicShoot;
    }

    void BombShootHitDetection(Vector2 position){
        Collider2D[] _hitObjects = Physics2D.OverlapCircleAll(position, m_BombRadius, m_HittableObjects);
        HashSet<Collider2D> _hitObjectsCenter = new HashSet<Collider2D>(Physics2D.OverlapCircleAll(position, m_Radius, m_HittableObjects));
        foreach (var obj in _hitObjects){
            if(_hitObjectsCenter.Contains(obj)){
                obj.GetComponentInParent<IHitComp>()?.Hit();
            }
            else{
                obj.GetComponentInParent<IHitComp>()?.BombHit();
            }
        }
    }

    void Reload(){
        m_CanShoot = true;
        animator.SetBool("CanShoot", true);
        m_CurrentProjectile = Instantiate(m_ProjectilePrefab, m_ProjectileSpawnPoint.position, Quaternion.identity).GetComponent<ProjectileScript>();
    }

    void Restart(){
        m_UpgradeLevel = 0;
        m_FireRateMultiplier = m_FireRateMultiplierCurve.Evaluate(0);
        currentShootForce = shootForce;
    }

    public void Upgrade()
    {
        m_UpgradeLevel++;
        m_FireRateMultiplier = m_FireRateMultiplierCurve.Evaluate(m_UpgradeLevel);
        currentShootForce *= 1.3f;
    }
}
