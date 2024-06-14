using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SWMovement : MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] GameObject text;

    [SerializeField] float backgroundSpeed = 1f;
    [SerializeField] float textSpeed = 1f;

    [SerializeField] float endTime = 25f;

    [SerializeField] GameObject history;
    [SerializeField] GameObject settings;

    [SerializeField] Slider slidersize;
    [SerializeField] Toggle skipHistory;

    [SerializeField] AudioClip click;
    [SerializeField] AudioClip button;
    [SerializeField] AudioClip returnSound;
    [SerializeField] AudioClip toggleNo;
    [SerializeField] AudioClip toggleYes;
    [SerializeField] AudioClip soundChange;


    bool isMoving = false;
    public float volumePublic = 0.5f;
    public bool isSettings = false;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        slidersize.value = volumePublic;
        SoundVar();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveBackgroundDown();
            MoveTextUp();
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                LoadNextScene();
            }
        }
        if (isSettings)
        {
            SoundVar();
        }

        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(click);
        }
    }

    void MoveBackgroundDown()
    {
        if (background != null)
        {
            background.transform.Translate(0, -1 * backgroundSpeed * Time.deltaTime, 0);
        }
    }

    void MoveTextUp()
    {
        if (text != null)
        {
            text.transform.Translate(0, textSpeed * Time.deltaTime, 0);
        }
    }

    public void StartMovement()
    {
        if (!isMoving)
        {
            if (skipHistory.isOn)
            {
                LoadNextScene();
            }
            else
            {
                isMoving = true;
                history.SetActive(true);
                StartCoroutine(WaitAndLoadScene());
            }
        }
    }

    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(endTime);
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettingsMenu()
    {
        isSettings = true;
        settings.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        isSettings = false;
        settings.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SoundVar()
    {
        volumePublic = slidersize.value;
        audioSource.volume = volumePublic;
    }

    public void Button()
    {
        audioSource.PlayOneShot(button);
    }

    public void ReturnSound()
    {
        audioSource.PlayOneShot(returnSound);
    }

    public void ToggleSound()
    {
        if (skipHistory) audioSource.PlayOneShot(toggleYes);
        else if (!skipHistory) audioSource.PlayOneShot(toggleNo);
    }

    public void OnSliderValueChanged()
    {
        audioSource.PlayOneShot(soundChange);
    }
}
