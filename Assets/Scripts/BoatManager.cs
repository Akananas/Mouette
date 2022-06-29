using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    [SerializeField] private bool spawn = true;
    public Boat boat;
    public Vector2 range;

 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        int dir = 1;
        if (dir < 0)
        {
            Instantiate(boat, new Vector3(-boat.GetComponent<SpriteRenderer>().size.x, 0, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(boat, new Vector3(Screen.width + boat.GetComponent<SpriteRenderer>().size.x, 0, 0), Quaternion.identity);
        }
    }

    IEnumerator Spawning()
    {
        while (spawn)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(range.x, range.y));
        }
    }
}
