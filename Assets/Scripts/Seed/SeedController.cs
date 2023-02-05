using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedController : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 50.0f;

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hack: sometimes Seeds collide with "Capsule"
        // check explizit for player
        if (other.gameObject.tag == "Player")
        {
            GameSystem.Instance.IncrementSeeds();
            Destroy(gameObject);
        }
    }
}
