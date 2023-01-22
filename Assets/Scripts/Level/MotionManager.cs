using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MotionManager : MonoBehaviour
{
    [SerializeField] GameObject destiantion;
    [Space(5)]
    [Header("Moving plattform to destination in seconds")]
    [SerializeField] float movingDuration;
    [Space(5)]
    [Header("Repeat moving betwenn start- and endpostion")]
    [SerializeField] bool repeatMoving;
    [Space(5)]
    [Header("Pause between moving back and repeat in seconds")]
    [SerializeField] float pauseDuration;

    private Vector3 startPosition;
    private float startTime;
    private Vector3 endPosition;

    private void Start()
    {
        startPosition = gameObject.transform.position;
        endPosition = gameObject.transform.position + destiantion.transform.localPosition;
        StartCoroutine(MovePlattform(startPosition, endPosition));
    }

    IEnumerator MovePlattform(Vector3 startPosition, Vector3 endPosition)
    {
        if (movingDuration == 0)
        {
            gameObject.transform.position = endPosition;
        }
        startTime = Time.time;
        while (Vector3.Distance(gameObject.transform.position, endPosition) > Mathf.Epsilon)
        {
            float timePassed = Time.time - startTime;
            float progress = timePassed / movingDuration;

            gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return new WaitForFixedUpdate();
        }
        if (repeatMoving)
        {
            StartCoroutine(Pause());
        }
    }
    IEnumerator Pause()
    {
        yield return new WaitForSeconds(pauseDuration);
        if (Vector3.Distance(gameObject.transform.position, endPosition) <= Mathf.Epsilon)
        {
            // switch start- and endpostion
            StartCoroutine(MovePlattform(endPosition, startPosition));
        } else
        {
            StartCoroutine(MovePlattform(startPosition, endPosition));
        }
    }
}
