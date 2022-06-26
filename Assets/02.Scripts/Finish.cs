using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Finish : MonoBehaviour
{
    [SerializeField]
    private GameObject ground;
    [SerializeField] private TextMeshProUGUI score, time, clear;
    [SerializeField] private TextMeshProUGUI bestscore, besttime;

    private void Start()
    {
        bestscore.SetText($"BestScore : {PlayerPrefs.GetInt("bestScore", 0)}");
        besttime.SetText($"BestTime : {PlayerPrefs.GetFloat("bestTime", 0):0.0}");
    }

    private void Update()
    {
        if (GameManger.Instance().IsOver)
        {
            ground.SetActive(true);
            Cursor.visible = true;
            time.SetText($"{GameManger.Instance().GetTime:0.0} S");
            score.SetText($"{GameManger.Instance().TotalScore} P");
            if (GameManger.Instance().isClear)
            {
                clear.SetText("Game Clear!");
            }
            else
            {
                clear.SetText("Game Over...");
            }
            Time.timeScale = 0;
            if(PlayerPrefs.GetFloat("bestTime")==0)
            {
                PlayerPrefs.SetFloat("bestTime", GameManger.Instance().GetTime);
            }
            if(PlayerPrefs.GetInt("bestScore")< GameManger.Instance().TotalScore&& GameManger.Instance().isClear&& PlayerPrefs.GetFloat("bestTime") >= GameManger.Instance().GetTime)
            {
                PlayerPrefs.SetInt("bestScore", GameManger.Instance().TotalScore);
                PlayerPrefs.SetFloat("bestTime", GameManger.Instance().GetTime);
                PlayerPrefs.Save();
            }
        }
    }
}
