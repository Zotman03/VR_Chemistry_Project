using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OrbitAroundPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player GameObject
    public float orbitSpeed = 15f; // Adjust this value to control the orbit speed
    // [SerializeField] private GameObject target;
    [SerializeField] public float degreesPerSecond = 45;
    void Update()
    {
        // Calculate the rotation axis based on the player's position and the orbiting object's position
        Vector3 rotationAxis = player.position - transform.position;

        // Rotate the object around the player
        // transform.RotateAround(player.position, rotationAxis, orbitSpeed * Time.deltaTime);
        transform.RotateAround(player.transform.position, Vector3.forward, degreesPerSecond * Time.deltaTime);
    }
}
