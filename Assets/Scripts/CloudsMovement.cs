using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;

    private void Update()
    {
        CloudsMove();
    }

    void CloudsMove()
    {
        transform.Translate(0, 0, -1 * movementSpeed * Time.deltaTime);
    }
}
