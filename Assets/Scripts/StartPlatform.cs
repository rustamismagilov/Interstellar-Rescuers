using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlatform : MonoBehaviour
{
    [SerializeField] float delayToShrink = 1f;
    [SerializeField] float xScale = 0.5f;
    [SerializeField] float zScale = 2f;

    [SerializeField] GameObject humans;
    [SerializeField] Animator humansAnimator;
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip landingSound;
    [SerializeField] AudioClip rescueSound;

    PlayerController playerController;
    Transform visualChild;
    UIController uIController;

    bool Entered;

    private void OnEnable()
    {
        playerController.enabled = false;
    }

    private void Awake()
    {
        uIController = FindObjectOfType<UIController>();
        playerController = FindObjectOfType<PlayerController>();
        visualChild = transform.GetChild(0);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Entered == false)
        {
            Vector3 playerPos = other.gameObject.transform.position;
            Vector3 dockingPos = gameObject.transform.parent.position;

            other.gameObject.transform.SetPositionAndRotation(new Vector3(dockingPos.x, playerPos.y, dockingPos.z), Quaternion.identity);
            //playerController.rb.isKinematic = true;
            //audioSource.PlayOneShot(landingSound);
            //playerController.enabled = false;
            if (humansAnimator != null)
            {
                humansAnimator.SetTrigger("RescueAnim");
            }
            StartCoroutine(ShrinkPlatformer());
        }
        else if (other.gameObject.CompareTag("Player") && Entered == true)
        {
            Destroy(transform.parent.gameObject);
            uIController.rescuedOnce = false;
        }
    }

    IEnumerator ShrinkPlatformer()
    {
        Vector3 initialScale = visualChild.localScale;
        float elapsed = 0f;
        Vector3 finalScale = new Vector3(xScale, initialScale.y, zScale);

        while (elapsed < delayToShrink)
        {
            visualChild.localScale = Vector3.Lerp(initialScale, finalScale, elapsed / delayToShrink);
            elapsed += Time.deltaTime;
            yield return null;
        }

        visualChild.localScale = finalScale;
        Destroy(humans);
        //audioSource.PlayOneShot(rescueSound);
        uIController.RescueUpdate();
        playerController.enabled = true;
        //playerController.rb.isKinematic = false;
        Entered = true;
    }
}
