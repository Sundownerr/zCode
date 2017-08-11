using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Button StartButton, OptionsButton, LoadButton, QuitButton;
    
    private void Start ()
    {
        StartButton = StartButton.GetComponent<Button>();
        OptionsButton = OptionsButton.GetComponent<Button>();
        LoadButton = LoadButton.GetComponent<Button>();
        QuitButton = QuitButton.GetComponent<Button>();

        StartButton.onClick.AddListener(StartClick);
        OptionsButton.onClick.AddListener(OptionsClick);
        LoadButton.onClick.AddListener(LoadClick);
        QuitButton.onClick.AddListener(QuitClick);

        Screen.SetResolution(1440, 900, false);    
    }

    private void QuitClick()
    {
        Application.Quit();
    }

    private void StartClick()
    {
        SceneManager.LoadScene("ClassChooseMenu");
    }

    private void LoadClick()
    {
        SceneManager.LoadScene("LoadMenu");
    }

    private void OptionsClick()
    {
        SceneManager.LoadScene("OptionsMenu");
    }  

}
