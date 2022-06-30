using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    public static BoatManager Instance;
    [SerializeField] private bool spawn = true;
    public Boat boat;
    public Vector2 range;

 
    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
            Instance = this;
        StartCoroutine(Spawning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn boat Function
    void Spawn()
    {
        Instantiate(boat, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 90));
    }

    // Spawning Loop
    IEnumerator Spawning()
    {
        while (spawn)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(range.x, range.y));
        }
    }
}
