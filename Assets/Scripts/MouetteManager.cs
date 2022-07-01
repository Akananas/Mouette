using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouetteManager : MonoBehaviour
{
    HashSet<MouetteScript> m_Alive = new HashSet<MouetteScript>();
    [SerializeField]GameObject prefab;
    [SerializeField]Vector2 spawnRate;
    [SerializeField]AnimationCurve spawnRateCurve;
    float spawnRateDivider;
    public static MouetteManager Instance;
    Timer m_SpawnTimer, m_LevelUpTimer;
    int m_SpawningLevel;

    private void Awake()
    {
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(this);
        }
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
        float time = Random.Range(spawnRate.x, spawnRate.y) / spawnRateDivider;
        m_SpawnTimer = GameManager.Inst.AddTimer(SpawnMouette, time);
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
        m_SpawnTimer?.ForceStop(false);
        m_LevelUpTimer?.ForceStop(false);
        foreach(var alive in m_Alive){
            Destroy(alive.gameObject);
        }
        m_Alive.Clear();
        m_SpawningLevel = 0;
        spawnRateDivider = spawnRateCurve.Evaluate(m_SpawningLevel);
        m_LevelUpTimer = GameManager.Inst.AddTimer(LevelUp, 10.0f, true, -1);
        RandomSpawnTimer();
    }

    void LevelUp(){
        m_SpawningLevel++;
        spawnRateDivider = spawnRateCurve.Evaluate(m_SpawningLevel);
    }
}
