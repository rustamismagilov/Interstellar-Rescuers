using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float distance;
    [SerializeField] float tumble;

    [SerializeField] bool HorizontalMovement;

    Vector3 movementAxisY = Vector3.up;

    Vector3 movementAxisX = Vector3.forward;

    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }

    private void Update()
    {
        Oscillation();
    }

    void Oscillation()
    {
        if(HorizontalMovement == false)
        {
            float newPos = Mathf.PingPong(Time.time * speed, distance);
            transform.position = startPos + movementAxisY.normalized * newPos;
        }
        else
        {
            float newPos = Mathf.PingPong(Time.time * speed, distance);
            transform.position = startPos + movementAxisX.normalized * newPos;
        }
    }
}
