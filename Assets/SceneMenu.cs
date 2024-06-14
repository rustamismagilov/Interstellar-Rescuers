using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SceneMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rescuedText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI fuelText;

    [SerializeField] Image rescuedStar;
    [SerializeField] Image timeStar;
    [SerializeField] Image fuelStar;

    [SerializeField] int maxRescues;
    [SerializeField] int maxSeconds;
    [SerializeField] int maxPercentage;

    [SerializeField] int score = 0;
    [SerializeField] int rescuedMultiplier = 100;
    [SerializeField] int timeMultiplier = 1000;
    [SerializeField] int fuelMultiplier = 10;

    [SerializeField] TextMeshProUGUI scoreText;

    float fuelPercentage;
    [SerializeField]int totalSeconds;
    UIController ui;

    private void Awake()
    {
        ui = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        Values();
        Stars();
    }

    void TimeToSeconds()
    {
        int minutes = ui.min;
        int seconds = ui.sec;
        totalSeconds = (minutes * 60) + seconds;

        timeText.text = totalSeconds.ToString() + " s";
    }

    void Values()
    {
        rescuedText.text = ui.rescuedNumber.ToString();
        score += ui.rescuedNumber * rescuedMultiplier;

        TimeToSeconds();

        if (totalSeconds <= maxSeconds)
        {
            score += (maxSeconds - totalSeconds) * timeMultiplier;
        }

        fuelPercentage = (ui.fuelSlider.value / 2f);
        fuelText.text = fuelPercentage.ToString("F2") + "%";

        // Calculate fuel score
        if (fuelPercentage >= maxPercentage)
        {
            score += Mathf.RoundToInt((fuelPercentage / maxPercentage) * fuelMultiplier);
        }

        scoreText.text = score.ToString();
    }

    void Stars()
    {
        if (ui.rescuedNumber == maxRescues)
        {
            rescuedStar.color = Color.yellow;
        }

        if (totalSeconds <= maxSeconds)
        {
            timeStar.color = Color.yellow;
        }

        if (fuelPercentage >= maxPercentage)
        {
            fuelStar.color = Color.yellow;
        }
    }
}
