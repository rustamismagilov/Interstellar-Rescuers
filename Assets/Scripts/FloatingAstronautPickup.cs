using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAstronautPickup : MonoBehaviour
{
    UIController uIController;
    public bool isRescued = false;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip rescue;

    private void Awake()
    {
        uIController = FindObjectOfType<UIController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isRescued)
        {
            uIController.RescueUpdate();
            uIController.rescuedOnce = false;
            isRescued = true;
            StartCoroutine(ResetRescuedFlag());
            audioSource.PlayOneShot(rescue);
            Destroy(gameObject);
        }
    }

    private IEnumerator ResetRescuedFlag()
    {
        yield return new WaitForSeconds(1);
        isRescued = false;
    }
}
