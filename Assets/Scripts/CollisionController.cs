using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CollisionController : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    [SerializeField] AudioClip finishAudio;
    [SerializeField] AudioClip explosionAudio;
    [SerializeField] AudioClip fuelSound;

    [SerializeField] float finishRotationSpeed = 360f;
    [SerializeField] float shrinkDuration = 1f;
    [SerializeField] Vector3 finalScale = Vector3.zero;

    [SerializeField] ParticleSystem explosion;
    [SerializeField] ParticleSystem finishParticles;
    [SerializeField] GameObject rocketoObj;

    PlayerController playerController;
    UIController uIController;
    AudioSource audioSource;

    [SerializeField] CinemachineVirtualCamera virtualCamera;

    bool hasCrashed;

    float maxFuelAmount;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        uIController = FindObjectOfType<UIController>();

        maxFuelAmount = playerController.fuelAmount;
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Respawn":
                break;
            case "Obstacles":
                PlayerCrash();
                StartCoroutine(Respawn());
                playerController.hasCrashed = true;
                break;
            default:
                PlayerCrash();
                StartCoroutine(Respawn());
                playerController.hasCrashed = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Fuel":
                Destroy(other.gameObject);
                playerController.fuelAmount = Mathf.Min(playerController.fuelAmount + 100f, maxFuelAmount);
                audioSource.PlayOneShot(fuelSound);
                break;
            case "Finish":
                finishParticles.Play();
                audioSource.PlayOneShot(finishAudio);
                playerController.rb.isKinematic = true;
                gameObject.transform.position = other.gameObject.transform.position;
                uIController.FinishPanel();
                FinishLevel();
                playerController.hasCrashed = true;
                break;
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        string thisScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(thisScene);
    }

    public void FinishLevel()
    {
        StartCoroutine(RotateAndShrinkPlayer());
        //StartCoroutine(LoadNextLevel());
    }

    IEnumerator RotateAndShrinkPlayer()
    {
        Vector3 initialScale = transform.localScale;
        float elapsed = 0f;
        while (elapsed < shrinkDuration)
        {
            float deltaTime = Time.deltaTime;
            float rotationThisFrame = finishRotationSpeed * deltaTime;
            transform.Rotate(Vector3.left, rotationThisFrame);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / shrinkDuration);
            elapsed += deltaTime;
            yield return null;
        }
        transform.localScale = finalScale;
    }

    void PlayerCrash()
    {
        if(hasCrashed)
        { return; }
        audioSource.PlayOneShot(explosionAudio);
        explosion.Play();
        playerController.mainEngineParticles.Stop();
        playerController.leftThrusterParticles.Stop();
        playerController.rightThrusterParticles.Stop();
        rocketoObj.SetActive(false);
        hasCrashed = true;
        virtualCamera.Follow = null;
    }
}
