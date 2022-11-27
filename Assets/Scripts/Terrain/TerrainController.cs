using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameSystem.Instance.PlayerDie();
        GameSystem.Instance.PlayerRespawn();
    }
}
