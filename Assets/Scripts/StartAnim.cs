using System.Collections;
using UnityEngine;

public class StartAnim : MonoBehaviour
{
    [SerializeField] float timeToPortal = 0.5f;
    [SerializeField] float timeToPlayer = 0.5f;
    [SerializeField] float timePlayerAnim = 1f;

    [SerializeField] Vector3 initialScale = Vector3.zero;
    [SerializeField] float finishRotationSpeed = 360f;

    [SerializeField] GameObject playerObject;
    [SerializeField] ParticleSystem finishParticles;

    SpriteRenderer spriteRenderer;

    PlayerController playerController;
    UIController uIController;

    [SerializeField] Rigidbody playerRb;

    private void Awake()
    {
        uIController = FindObjectOfType<UIController>();
        playerController = FindObjectOfType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        playerRb.useGravity = false;
    }

    private void Start()
    {
        uIController.StartPanel();
        playerController.enabled = false;
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(timeToPortal);
        spriteRenderer.enabled = true;
        StartCoroutine(RotateAndSizeUpObject());

        yield return new WaitForSeconds(timeToPlayer);
        playerObject.transform.position = gameObject.transform.position;
        playerObject.SetActive(true);
        finishParticles.Play();

        StartCoroutine(RotateAndSizeUpPlayer());

        yield return new WaitForSeconds(timePlayerAnim);
        playerRb.useGravity = true;
        Destroy(gameObject);
        playerController.enabled = true;
    }

    private IEnumerator RotateAndSizeUpObject()
    {
        Vector3 finalScale = transform.localScale;
        float elapsed = 0f;
        while (elapsed < timeToPortal)
        {
            float deltaTime = Time.deltaTime;
            float rotationThisFrame = finishRotationSpeed * deltaTime;
            transform.Rotate(Vector3.forward, rotationThisFrame);
            transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / timeToPortal);
            elapsed += deltaTime;
            yield return null;
        }
        transform.localScale = finalScale;
    }

    private IEnumerator RotateAndSizeUpPlayer()
    {
        Vector3 finalScale = playerObject.transform.localScale;
        float elapsed = 0f;
        while (elapsed < timePlayerAnim)
        {
            float deltaTime = Time.deltaTime;
            float rotationThisFrame = finishRotationSpeed * deltaTime;
            playerObject.transform.Rotate(Vector3.up, rotationThisFrame);
            playerObject.transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / timePlayerAnim);
            elapsed += deltaTime;
            yield return null;
        }
        playerObject.transform.localScale = finalScale;
        playerRb.useGravity = true;
        playerController.enabled = true;
    }
}
