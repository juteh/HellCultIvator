using UnityEngine;

public class Singletons : MonoBehaviour {

    public static Singletons Instance {
        get; private set;
    }
    public AudioManager AudioManager {
        get; private set;
    }
    public UIManager UIManager {
        get; private set;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
        AudioManager = GetComponentInChildren<AudioManager>();
        UIManager = GetComponentInChildren<UIManager>();
    }
}
