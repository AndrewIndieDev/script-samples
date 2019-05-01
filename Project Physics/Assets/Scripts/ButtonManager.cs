using UnityEngine;

public class ButtonManager : MonoBehaviour {

    public GameObject playButtonObject;
    public GameObject settingsButtonObject;
    public GameObject rssButtonObject;

    void Start()
    {
        DisableButtons();
    }
    
    public void PlayButton()
    {
        if (!playButtonObject.activeSelf)
        {
            DisableButtons();
            playButtonObject.SetActive(true);
        }
        else
        {
            DisableButtons();
        }
    }

    public void SettingsButton()
    {
        if (!settingsButtonObject.activeSelf)
        {
            DisableButtons();
            settingsButtonObject.SetActive(true);
        }
        else
        {
            DisableButtons();
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void RSSButton()
    {
        if (!rssButtonObject.activeSelf)
        {
            DisableButtons();
            rssButtonObject.SetActive(true);
            FindObjectOfType<NewsFeed>().Start();
        }
        else
        {
            DisableButtons();
        }
    }

    void DisableButtons()
    {
        playButtonObject.SetActive(false);
        settingsButtonObject.SetActive(false);
        rssButtonObject.SetActive(false);
    }
}
