using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour, IHitComp
{
    public int direction = 1;
    [SerializeField] private float speed;
    [SerializeField] int health = 100;
    [SerializeField] int maxHealth = 100;
    List<MouetteScript> attackers = new List<MouetteScript>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * (direction * speed * Time.deltaTime);
        if (CheckFinish())
            direction *= -1;
            
        for(int i = 0; i < attackers.Count; ++i){
            Hit();
        }
    }

    bool CheckFinish()
    {
        float X = Camera.main.WorldToScreenPoint(transform.position).x;
        if (direction > 0)
        {
            return Camera.main.WorldToScreenPoint(transform.position - Vector3.right * GetComponentInParent<SpriteRenderer>().size.x).x > Screen.width;
        }
        else
        {
            return Camera.main.WorldToScreenPoint(transform.position + Vector3.right * GetComponentInParent<SpriteRenderer>().size.x).x < 0;
        }
    }
    
    public void AddAttacker(MouetteScript mouette){
        mouette.OnHit = RemoveAttacker;
        attackers.Add(mouette);
    }
    
    void RemoveAttacker(MouetteScript mouette){
        attackers.Remove(mouette);
    }
    
    public float GetCurrentSpeed(){
        return speed * direction;
    }

    public void Hit()
    {
        Debug.Log("Boat Hit");
        health -= 10;
        if (health < 0)
        {
            foreach(var mouette in attackers){
                mouette.RemoveFromBoat();
                mouette.OnHit -= RemoveAttacker;
            }
            BoatManager.Instance.DestroyBoat(this);
        }

        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.HSVToRGB(200 / 255.0f, 0, 0), new Color(30 / 255.0f, 150 / 255.0f, 220 / 255.0f), 1.0f * health / maxHealth);
    }
}
