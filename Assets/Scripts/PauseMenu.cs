using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    SaveValues saveValues;
    [SerializeField] Slider soundSlider;

    [SerializeField] AudioClip pause;
    [SerializeField] AudioClip restart;
    [SerializeField] AudioClip maiMenu;

    [SerializeField] AudioSource audioSource;

    private void Awake()
    {
        saveValues = FindObjectOfType<SaveValues>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(saveValues != null)
        soundSlider.value = saveValues.savedVolume;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ClosePause();
            }
            else
            {
                OpenPause();
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && !isPaused)
        {
            RestartLevel();
        }

        if(isPaused)
        {
            if(saveValues != null)
            saveValues.savedVolume = soundSlider.value;
        }

    }

    public void OpenPause()
    {
        audioSource.PlayOneShot(pause);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ClosePause()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        audioSource.PlayOneShot(pause);
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        audioSource.PlayOneShot(restart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        audioSource.PlayOneShot(maiMenu);
        SceneManager.LoadScene(0);
    }
}

