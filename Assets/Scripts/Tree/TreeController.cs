using UnityEngine;

public class TreeController : MonoBehaviour {
    [SerializeField] GameObject crown;
    [SerializeField] GameObject trunk;
    [SerializeField] GameObject ground;
    private bool isPlanted = false;
    private void OnTriggerEnter(Collider other) {
        if (isPlanted)
            return;

        if (GameSystem.Instance.collectedSeeds >= 3) {
            crown.SetActive(true);
            trunk.SetActive(true);
            if (GameSystem.Instance.DecrementSeeds()) {
                GameSystem.Instance.PlantTree();
                isPlanted = true;
            }
        }

    }
}
