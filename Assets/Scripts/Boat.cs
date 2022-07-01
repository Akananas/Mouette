using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour, IHitComp
{
    public int direction = 1;
    [SerializeField] private float speed;
    [SerializeField] float health = 100;
    [SerializeField] float maxHealth = 100;
    List<MouetteScript> attackers = new List<MouetteScript>();
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] SpriteMask mask;

    // Start is called before the first frame update
    void Start()
    {
        if (direction < 0)
            sprite.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * (direction * speed * Time.deltaTime);
        if (CheckFinish())
        {
            ScoreManager.Instance.ScoreBoat();
            Destroy(gameObject);
        }
        for (int i = 0; i < attackers.Count; ++i)
        {
            Hit(10.0f * Time.deltaTime);
        }
    }

    bool CheckFinish()
    {
        float X = Camera.main.WorldToScreenPoint(transform.position).x;
        if (direction > 0)
        {
            return Camera.main.WorldToScreenPoint(transform.position - Vector3.right * sprite.size.x).x > Screen.width;
        }
        else
        {
            return Camera.main.WorldToScreenPoint(transform.position + Vector3.right * sprite.size.x).x < 0;
        }
    }

    public void AddAttacker(MouetteScript mouette)
    {
        mouette.OnHit += RemoveAttacker;
        attackers.Add(mouette);
    }

    void RemoveAttacker(MouetteScript mouette)
    {
        attackers.Remove(mouette);
    }

    public float GetCurrentSpeed()
    {
        return speed * direction;
    }

    public void Hit(float damage = 10.0f)
    {
        Debug.Log("Boat Hit");
        if (health < 0) return;
        health -= damage;
        if (health < 0)
        {
            foreach (var mouette in attackers)
            {
                mouette.RemoveFromBoat();
                mouette.OnHit -= RemoveAttacker;
            }
            BoatManager.Instance.DestroyBoat(this);
            StartCoroutine(Sink());
            AudioManager.Instance.PlayClip("Sink");
        }

        sprite.color = Color.Lerp(Color.grey, Color.white, 1.0f * health / maxHealth);
    }

    public void BombHit(float damage = 10.0f)
    {

    }

    IEnumerator Sink()
    {
        if (direction < 0)
        {
            GetComponent<Animator>().SetTrigger("ReversePlouf");
            yield return new WaitForSeconds(.2f);
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Plouf");
        }
        yield return new WaitForSeconds(.2f);
        for (float i = -2.32f; i < 0; i += Time.deltaTime)
        {
            transform.position += Vector3.down * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
