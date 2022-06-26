using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManger : MonoBehaviour
{
    public TMP_Text scoreText;
    private int totalScore = 0;
    public int TotalScore
    {
        get { return totalScore; }
    }
    [SerializeField]
    private TextMeshProUGUI timer;

    private float time = 0;
    public float GetTime
    {
        get { return time; }
    }
    public GameObject totem;
    public GameObject nexus;
    public GameObject monster;
    public float createTime = 3.0f;
    public List<Transform> points = new List<Transform>();
    private bool isOver;
    public List<GameObject> monsterPool = new List<GameObject>();
    public int maxMonsters = 10;
    public bool isClear=false;

    public bool IsOver
    {
        get { return isOver; }
        set
        {
            isOver = value;
            if(isOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    private static GameManger instance;
    public static GameManger Instance()
    {
        if(instance==null)
        {
            instance = FindObjectOfType<GameManger>();
            if(instance==null)
            {
                GameObject container = new GameObject("GameManger");
                instance = container.AddComponent<GameManger>();
            }
        }
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (GameManger.Instance().IsOver) return;
        time += Time.deltaTime;
        timer.SetText($"{time:0.0}");
    }

    void Start()
    {
        DisplayScore(0);
        SummonPortal();
        CreateMonsterPool();
    }
    void SummonPortal()
    {
        int _rand = Random.Range(0, points.Count);
        GameObject _nexus = Instantiate(nexus, points[_rand].position, Quaternion.Euler(0, Random.Range(0, 360), 0));
        points[_rand] = points[points.Count - 1];
        _nexus.transform.SetParent(null);
        for (int i=1; i<=3; i++)
        {
            int rand = Random.Range(0, points.Count - i);
            GameObject _totem = Instantiate(totem, points[rand].position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            points[rand] = points[points.Count - i - 1];
            _totem.name = "Totem";
            _totem.transform.SetParent(_nexus.transform);
        }
    }
    void CreateMonsterPool()
    {
        for(int i=0; i<maxMonsters; i++)
        {
            var _monster = Instantiate<GameObject>(monster);

            _monster.name = $"Monster_{i:00}";

            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        foreach(var _monster in monsterPool)
        {
            if (_monster.activeSelf == false)
                return _monster;
        }
        return null;
    }
    public void DisplayScore(int score)
    {
        totalScore += score;
        scoreText.text = $"<color=#00ff00>SCORE : </color><color=#ff0000>{totalScore:#,##0}</color>";
    }
}
