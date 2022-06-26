using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManger : MonoBehaviour
{
    public Button startButton;
    private UnityAction action;

    void Start()
    {
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);
    }
    
    void OnStartClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
