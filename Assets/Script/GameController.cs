using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance
    {
        get => _instance;
        set
        {
            if (_instance == null)
                _instance = value;
        }
    }

    public List<GameObject> Food = new List<GameObject>();
    public List<EnemyFollow> Cats = new List<EnemyFollow>();
    public List<WaveInfo> Waves = new List<WaveInfo>();

    private IEnumerator<WaveInfo> WaveEnum;
    private int waveCount = 0;
    
    public GameObject FoodPrefab;
    public List<GameObject> CatPrefabs;
    public BoxCollider2D FoodSpawn;
    public BoxCollider2D CatSpawn;
    public float minDistance;
    private Player player;

    public void Awake() {
        // var bounds = FoodSpawn.bounds;
        // var min = bounds.min;
        // var max = bounds.max;
        //
        // Vector3 pos1 = Vector3.zero;
        // Vector3 pos2 = Vector3.zero;
        // bool valid = false;
        //
        // int maxAttempts = 30;
        // int attemps = 0;
        //
        // while (!valid && attemps <= maxAttempts) {
        //     attemps++;
        //     pos1 = new Vector3(Random.Range(min.x,max.x), Random.Range(min.y,max.y));
        //     pos2 = new Vector3(Random.Range(min.x,max.x), Random.Range(min.y,max.y));
        //
        //     var distance = Vector3.Distance(pos1, pos2);
        //
        //     valid = distance >= minDistance;
        // }
        //
        // Instantiate(FoodPrefab, pos1, Quaternion.identity);
        // Instantiate(FoodPrefab, pos2, Quaternion.identity);
        player = FindObjectOfType<Player>();
        Instance = this;
        waveCount = 0;
        WaveEnum = Waves.GetEnumerator();
    }

    private void Start()
    {
        StartCoroutine(WaitAnd(5, StartNextWave));
    }

    private void StartNextWave()
    {
        var hasNextWave = WaveEnum.MoveNext();

        if (!hasNextWave)
        {
            MainUI.Instance.Win();
            return;
        }

        player.BlueFoodCount++;
        var wave = WaveEnum.Current;
        waveCount++;
        MainUI.Instance.SetWave(waveCount);

        for (int i = 0; i < wave.FoodCount; i++)
        {
            var pos = GetRandomPositionInBounds(FoodSpawn.bounds);
            var food = Instantiate(FoodPrefab, pos, Quaternion.identity);
            Food.Add(food);
        }

        for (int i = 0; i < wave.CatCount; i++)
        {
            var pos = GetRandomPositionInBounds(CatSpawn.bounds);
            var cat = Instantiate(GetRandomCatPrefab(), pos, Quaternion.identity).GetComponent<EnemyFollow>();
            Cats.Add(cat);
        }

        StartCoroutine(CountDownWave(wave.WaveDuration));
    }
    
    private IEnumerator WaitAnd(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }

    private IEnumerator WaitFor(Func<bool> condition, Action action)
    {
        while (condition.Invoke() == false)
            yield return null;
        
        action.Invoke();
    }

    private IEnumerator CountDownWave(int seconds)
    {
        while (seconds >= 0 && Food.Any())
        {
            MainUI.Instance.SetRemainingTimeText(seconds);
            seconds--;
            yield return new WaitForSeconds(1);
        }

        MainUI.Instance.SetRemainingTimeText(0);
        
        foreach (var cat in Cats)
        {
            cat.State = EnemyFollow.CatState.Leaving;
        }

        while (Cats.Count > 0)
            yield return null;

        foreach (var food in Food)
        {
            Destroy(food);
        }

        var playerFoods = GameObject.FindGameObjectsWithTag("Food");

        foreach (var playerFood in playerFoods)
        {
            Destroy(playerFood);
        }
        
        Food.Clear();
        
        yield return new WaitForSeconds(5f);
        
        StartNextWave();
    }

    private Vector2 GetRandomPositionInBounds(Bounds bounds)
    {
        var min = bounds.min;
        var max = bounds.max;
        
        return new Vector3(Random.Range(min.x,max.x), Random.Range(min.y,max.y));
    }

    private GameObject GetRandomCatPrefab()
    {
        var size = CatPrefabs.Count;
        var index = Random.Range(0, size);
        return CatPrefabs[index];
    }
    
    [Serializable] public struct WaveInfo
    {
        public int FoodCount;
        public int CatCount;
        public int WaveDuration;
    }
}
