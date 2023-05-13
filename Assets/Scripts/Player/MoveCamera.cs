using UnityEngine;

public class MoveCamera : MonoBehaviour {
    [SerializeField] Transform cameraPosition;

    private void Update() {
        transform.position = cameraPosition.position;
        transform.rotation = cameraPosition.rotation;
    }

    public void SetCameraPosition(Transform cameraPosition) {
        this.cameraPosition = cameraPosition;
    }
}
