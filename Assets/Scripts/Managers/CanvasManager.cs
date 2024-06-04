using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CanvasManager : MonoBehaviour
{
    [Header("Button")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;
    public Button resumeButton;
    public Button returnToMenu;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("Text")]
    public TMP_Text livesText;
    public TMP_Text volSliderText;

    [Header("Slider")]
    public Slider volSlider;


    // Start is called before the first frame update
    void Start()
    {
        if (quitButton)
            quitButton.onClick.AddListener(Quit);

        if (resumeButton)
            resumeButton.onClick.AddListener(() => SetMenus(null, pauseMenu));

        if (returnToMenu)
            returnToMenu.onClick.AddListener(() => GameManager.Instance.LoadScene("Title"));

        if (playButton)
            playButton.onClick.AddListener(() => GameManager.Instance.LoadScene("Game"));

        if (settingsButton)
            settingsButton.onClick.AddListener(() => SetMenus(settingsMenu, mainMenu));

        if (backButton)
            backButton.onClick.AddListener(() => SetMenus(mainMenu, settingsMenu));

        if (volSlider)
        {
            volSlider.onValueChanged.AddListener(OnSliderValueChanged);
            if (volSliderText)
                volSliderText.text = volSlider.value.ToString();
        }

        if (livesText)
        {
            GameManager.Instance.OnLifeValueChange += OnLifeValueChanged;
            livesText.text = $"Lives: {GameManager.Instance.lives}";
        }

    }

    void OnSliderValueChanged(float value)
    {
        if (volSliderText)
            volSliderText.text = value.ToString();
    }

    void OnLifeValueChanged(int value)
    {
        if (livesText)
            livesText.text = $"Lives: {value}";
    }

    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);

        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
    }    

    void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            //hints for the lab
            if (pauseMenu.activeSelf)
            {
                //do something to pause
            }
            else
            {
                //do something else
            }
        }
    }
}

