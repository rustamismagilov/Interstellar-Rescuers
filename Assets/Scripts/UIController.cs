using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Fuel")]
    [SerializeField] public Slider fuelSlider;

    [Header("Finish")]
    [SerializeField] Image finishPanel;

    [Header("Rescued")]
    [SerializeField] TextMeshProUGUI rescuedText;

    [Header("Timer")]
    [SerializeField] public TextMeshProUGUI timeText;

    [Header("Score Screen")]
    [SerializeField] GameObject sceneUI;

    PlayerController playerController;
    SaveValues save;
    CollisionController collisionController;
    public int rescuedNumber = 0;
    public bool rescuedOnce;

    private float startTime;
    private bool isTimerRunning = true;

    public int min;
    public int sec;

    public string time;

    private void Awake()
    {
        collisionController = FindObjectOfType<CollisionController>();
        save = FindObjectOfType<SaveValues>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        fuelSlider.maxValue = playerController.fuelAmount;
        rescuedText.text = rescuedNumber.ToString();
        startTime = Time.time;
        StartCoroutine(UpdateTimer());
    }

    private void Update()
    {
        fuelSlider.value = playerController.fuelAmount;
    }

    private IEnumerator UpdateTimer()
    {
        while (isTimerRunning)
        {
            float elapsedTime = Time.time - startTime;
            min = Mathf.FloorToInt(elapsedTime / 60F);
            sec = Mathf.FloorToInt(elapsedTime % 60F);
            time = string.Format("{0:00}:{1:00}", min, sec);
            timeText.text = time;
            yield return new WaitForSeconds(1f);
        }
    }

    public void FinishPanel()
    {
        isTimerRunning = false;
        StartCoroutine(FadeInFinishPanel());
    }

    private IEnumerator FadeInFinishPanel()
    {
        float duration = 1f;
        float elapsedTime = 0f;
        Color color = finishPanel.color;
        color.a = 0;
        finishPanel.color = color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / duration);
            finishPanel.color = color;
            yield return null;
        }
        color.a = 1;
        finishPanel.color = color;
        sceneUI.SetActive(true);
    }

    public void StartPanel()
    {
        StartCoroutine(FadeInStartPanel());
    }

    private IEnumerator FadeInStartPanel()
    {
        float duration = 1f;
        float elapsedTime = 0f;
        Color color = finishPanel.color;
        color.a = 1;
        finishPanel.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (elapsedTime / duration));
            finishPanel.color = color;
            yield return null;
        }
        color.a = 0;
        finishPanel.color = color;
    }

    public void RescueUpdate()
    {
        if (rescuedOnce == false)
        {
            rescuedNumber++;
            rescuedText.text = rescuedNumber.ToString();
            rescuedOnce = true;
        }
    }

    public void NextLevel()
    {
        int thisScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = thisScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }
}
