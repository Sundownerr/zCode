using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;



public class MenuManager : MonoBehaviour {

    public Button StartButton, UpgradesButton, QuitButton;
   
    void Start () {
        StartButton = StartButton.GetComponent<Button>();
        UpgradesButton = UpgradesButton.GetComponent<Button>();
        QuitButton = QuitButton.GetComponent<Button>();
     
        StartButton.onClick.AddListener(StartClick);
        UpgradesButton.onClick.AddListener(UpgradeClick);
        QuitButton.onClick.AddListener(QuitClick);

        Screen.SetResolution(1440, 900, false);
        
    }


    void Update()
    {
    
    }

    private void QuitClick()
    {
        Application.Quit();
    }

    private void StartClick()
    {
        SceneManager.LoadScene("ClassChooseMenu");
    }
	
    private void UpgradeClick()
    {
    }  

}
