using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManger : MonoBehaviour
{
    public TMP_Text scoreText;
    private int totalScore = 0;

    public GameObject monster;
    public float createTime = 3.0f;
    public List<Transform> points = new List<Transform>();
    private bool isOver;
    public List<GameObject> monsterPool = new List<GameObject>();
    public int maxMonsters = 10;

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
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        DisplayScore(0);
        CreateMonsterPool();

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
