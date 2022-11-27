using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField] GameObject crown;
    [SerializeField] GameObject trunk;
    [SerializeField] GameObject ground;

    private void OnTriggerEnter(Collider other)
    {
        if(GameSystem.Instance.collectedSeeds > 0)
        {
            crown.SetActive(true);
            trunk.SetActive(true);
            GameSystem.Instance.DecrementSeeds();
            GameSystem.Instance.PlantTree();
        }
        
    }
}
