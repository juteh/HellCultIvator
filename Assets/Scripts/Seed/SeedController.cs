using System.Collections;
using UnityEngine;

public class SeedController : MonoBehaviour {
    [SerializeField] float rotationSpeed = 50.0f;

    void Update() {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other) {
        // Hack: sometimes Seeds collide with "Capsule"
        // check explizit for player
        if (other.gameObject.tag == "Player") {
            GameSystem.Instance.IncrementSeeds();
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    public void StartMovingSeed(
        Vector3 startPosition, Vector3 endPosition, GameObject movingObject, float movingDuration, float? _startTime, bool repeatMoving, float pauseDuration) {
        StartCoroutine(MoveObject(
            startPosition: startPosition,
            endPosition: endPosition,
            movingObject: movingObject,
            movingDuration: movingDuration,
            _startTime: _startTime,
            repeatMoving: repeatMoving,
            pauseDuration: pauseDuration
            )
        );
    }

    private IEnumerator MoveObject(
        Vector3 startPosition, Vector3 endPosition, GameObject movingObject,
        float movingDuration, float? _startTime, bool repeatMoving, float pauseDuration) {
        if (movingDuration == 0) {
            movingObject.transform.position = endPosition;
        }
        while (Vector3.Distance(movingObject.transform.position, endPosition) > Mathf.Epsilon) {
            if (_startTime == null) {
                _startTime = Time.time;
            }
            float timePassed = Time.time - (float)_startTime;
            float progress = timePassed / movingDuration;
            movingObject.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return new WaitForFixedUpdate();
        }
        _startTime = null;
        if (repeatMoving) {
            StartCoroutine(PauseMoving(
                startPosition: startPosition,
                endPosition: endPosition,
                movingObject: movingObject,
                movingDuration: movingDuration,
                _startTime: _startTime,
                repeatMoving: repeatMoving,
                pauseDuration: pauseDuration
                ));
        }
    }

    private IEnumerator PauseMoving(Vector3 startPosition, Vector3 endPosition, GameObject movingObject,
        float movingDuration, float? _startTime, bool repeatMoving, float pauseDuration) {
        yield return new WaitForSeconds(pauseDuration);
        if (Vector3.Distance(movingObject.transform.position, endPosition) <= Mathf.Epsilon) {
            // switch start- and endpostion
            StartCoroutine(MoveObject(
                startPosition: endPosition,
                endPosition: startPosition,
                movingObject: movingObject,
                movingDuration: movingDuration,
                _startTime: _startTime,
                repeatMoving: repeatMoving,
                pauseDuration: pauseDuration
                ));
        } else {
            StartCoroutine(MoveObject(
                startPosition: startPosition,
                endPosition: endPosition,
                movingObject: movingObject,
                movingDuration: movingDuration,
                _startTime: _startTime,
                repeatMoving: repeatMoving,
                pauseDuration: pauseDuration
                ));
        }
    }
}
