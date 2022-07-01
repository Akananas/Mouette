using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    [SerializeField] private Boat boat;
    public static BoatManager Instance;

    [SerializeField] private bool spawn = true;
    public Vector2 range;
    [SerializeField] private int boatLeft = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
            Instance = this;
        EnableSpawning(true);
    }

    public void EnableSpawning(bool val)
    {
        if (val)
        {
            StartCoroutine(Spawning());
        }
        else
            spawn = false;
    }

    // Spawn boat Function
    void Spawn()
    {
        float depth = Random.Range(-2.2f, -1.0f);
        int rand = Random.Range(0, 10);
        GameObject go;
        if (rand % 2 == 0)
        {
            go = Instantiate(boat, new Vector3(-10, depth, 0), Quaternion.identity).gameObject;

        }
        else
        {
            Boat boat = Instantiate(this.boat, new Vector3(10, depth, 0), Quaternion.identity);
            boat.direction = -1;
            go = boat.gameObject;
        }
        go.transform.localScale = Vector3.Lerp(Vector3.one * .5f, Vector3.one, -(depth + 1.2f));
        go.transform.position += Vector3.back * Mathf.Lerp(-.1f, .1f, -(depth + 1.2f));
    }

    // Spawning Loop
    IEnumerator Spawning()
    {
        spawn = true;
        while (spawn)
        {
            Spawn();
            yield return new WaitForSeconds(Random.Range(range.x, range.y));
        }
    }

    public void DestroyBoat(Boat boat)
    {
        boatLeft--;
        UIManager.Instance.BoatLeft(boatLeft);
        if (boatLeft <= 0) {
            UIManager.Instance.Defeat();
            EnableSpawning(false);
        }
    }

    
    // Reset Spawning (for begin play)
    public void Reset()
    {
        boatLeft = 3;
        foreach (Boat b in FindObjectsOfType<Boat>())
            Destroy(b.gameObject);
        UIManager.Instance.BoatLeft(boatLeft);
        EnableSpawning(true);
    }

}
