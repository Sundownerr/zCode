﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenuManager : MonoBehaviour
{
    public Button BackButton;

	private void Start ()
    {
        BackButton = BackButton.GetComponent<Button>();
        BackButton.onClick.AddListener(BackClick);
    }	

    private void BackClick()
    {
        SceneManager.LoadScene("MainMenu");       
    }
}
