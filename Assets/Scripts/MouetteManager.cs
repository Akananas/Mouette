using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouetteManager : MonoBehaviour
{
    HashSet<MouetteScript> m_Alive = new HashSet<MouetteScript>();
    [SerializeField]GameObject prefab;
    [SerializeField]Vector2 spawnRate;

    void Start(){
        RandomSpawnTimer();
    }

    void SpawnMouette(){
        Vector3 spawnPoint = GetRandomSpawnPoint();
        MouetteScript mouette = Instantiate(prefab, spawnPoint, Quaternion.identity).GetComponent<MouetteScript>();
        if(spawnPoint.x > 0){
            mouette.SetNextPoint(new Vector2(-10, spawnPoint.y));
        }
        else{
            mouette.GetComponent<MouetteScript>().SetNextPoint(new Vector2(10, spawnPoint.y));
        }

        mouette.OnDeath = RemoveFromAlive;
        m_Alive.Add(mouette);
        RandomSpawnTimer();
    }

    void RandomSpawnTimer(){
        float time = Random.Range(spawnRate.x, spawnRate.y);
        GameManager.Inst.AddTimer(SpawnMouette, time);
    }

    Vector3 GetRandomSpawnPoint(){
        int right = Random.Range(0,2);
        float x = right == 1 ? 10 : -10;
        float y = Random.Range(1.0f, 4.5f);
        return new Vector3(x, y, 0);
    }

    void RemoveFromAlive(MouetteScript mouette){
        m_Alive.Remove(mouette);
    }

    public void Reset(){
        foreach(var alive in m_Alive){
            Destroy(alive.gameObject);
        }
    }
}
