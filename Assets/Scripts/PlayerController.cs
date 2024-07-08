using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 10f;

    [SerializeField] AudioClip mainEngine;

    [SerializeField] public ParticleSystem mainEngineParticles;
    [SerializeField] public ParticleSystem leftThrusterParticles;
    [SerializeField] public ParticleSystem rightThrusterParticles;

    [SerializeField] public float fuelAmount = 100f;

    [SerializeField] AudioSource ambience;

    public Rigidbody rb;
    AudioSource audioSource;
    UIController uiController;
    public bool hasCrashed = false;
    SaveValues saveValues;

    private void Awake()
    {
        saveValues = FindObjectOfType<SaveValues>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        uiController = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        if (saveValues != null)
        {
            audioSource.volume = saveValues.savedVolume;
            ambience.Play();
        }
        else
        {
            ambience.volume = 0.25f;
            ambience.Play();
        }
    }

    private void Update()
    {
        ProcessRotation();
        ProcessThrusters();
        if (saveValues != null)
        {
            audioSource.volume = saveValues.savedVolume;
            ambience.volume = saveValues.savedVolume;
        }
    }

    void ProcessThrusters()
    {
        if (!hasCrashed && fuelAmount > 0)
        {
            bool thrusting = Input.GetKey(KeyCode.Space);

            // Check for touch input for thrusting
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    if (touch.position.y > Screen.height / 2)
                    {
                        thrusting = true;
                        break;
                    }
                }
            }

            if (thrusting)
            {
                if (rb.isKinematic == true)
                {
                    rb.isKinematic = false;
                }
                rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(mainEngine);
                }
                if (!mainEngineParticles.isPlaying)
                {
                    mainEngineParticles.Play();
                }
                StartCoroutine(ConsumeFuel());
            }
            else
            {
                audioSource.Stop();
                mainEngineParticles.Stop();
            }
        }
    }

    IEnumerator ConsumeFuel()
    {
        while (Input.GetKey(KeyCode.Space) && fuelAmount > 0)
        {
            yield return new WaitForFixedUpdate();
            fuelAmount -= 0.01f;
        }
    }

    void ProcessRotation()
    {
        if (!hasCrashed)
        {
            bool rotateLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            bool rotateRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

            // Check for touch input for rotation
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    if (touch.position.y < Screen.height / 2)
                    {
                        if (touch.position.x < Screen.width / 2)
                        {
                            rotateLeft = true;
                        }
                        else
                        {
                            rotateRight = true;
                        }
                    }
                }
            }

            if (rotateLeft)
            {
                ApplyRotation(rotationThrust);
                if (!rightThrusterParticles.isPlaying)
                {
                    rightThrusterParticles.Play();
                }
            }
            else if (rotateRight)
            {
                ApplyRotation(-rotationThrust);
                if (!leftThrusterParticles.isPlaying)
                {
                    leftThrusterParticles.Play();
                }
            }
            else
            {
                rightThrusterParticles.Stop();
                leftThrusterParticles.Stop();
            }
        }
    }

    void ApplyRotation(float rotationFrame)
    {
        transform.Rotate(Vector3.left * rotationFrame * Time.deltaTime);
    }
}
