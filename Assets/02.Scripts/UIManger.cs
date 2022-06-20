using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManger : MonoBehaviour
{

    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    void Start()
    {
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

        optionButton.onClick.AddListener(delegate { OnButtonClick(optionButton.name); });

        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
    }
    
    void OnStartClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void OnButtonClick(string str)
    {
        Debug.Log($"Click Button : {str}");
    }
}
