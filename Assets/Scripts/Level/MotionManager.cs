using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    [Header("PauseMoving between moving back and repeat in seconds")]
    [SerializeField] float pauseDuration;

    private float _startTime;
    private GameObject _objectOnPlattform;

    private void Start()
    {
        Vector3 startPosition = gameObject.transform.position;
        Vector3 endPosition = startPosition + destiantion.transform.localPosition;
        
        StartCoroutine(MoveObject(
            startPosition: startPosition,
            endPosition: endPosition,
            movingObject: gameObject
            )
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        Debug.Log(other.tag);
        if (other.tag == "Seed")
        {
            _objectOnPlattform = other.gameObject;
            Vector3 startPosition = _objectOnPlattform.gameObject.transform.position;
            Vector3 endPosition = startPosition + destiantion.transform.localPosition;
            // TODO: bug, wird imer gleiche coroutine aufgerufen. Müssen unabhängige sein
            StartCoroutine(MoveObject(
                startPosition: startPosition,
                endPosition: endPosition,
                movingObject: _objectOnPlattform
                )
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit");
        Debug.Log(other.tag);
        if (other.tag == "Seed")
        {
            _objectOnPlattform = null;
        }
    }

    IEnumerator MoveObject(Vector3 startPosition, Vector3 endPosition, GameObject movingObject)
    {
        if (movingDuration == 0)
        {
            movingObject.transform.position = endPosition;
        }
        _startTime = Time.time;
        while (Vector3.Distance(movingObject.transform.position, endPosition) > Mathf.Epsilon)
        {
            float timePassed = Time.time - _startTime;
            float progress = timePassed / movingDuration;

            movingObject.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return new WaitForFixedUpdate();
        }
        if (repeatMoving)
        {
            StartCoroutine(PauseMoving(
                startPosition: startPosition,
                endPosition: endPosition,
                movingObject: movingObject
                )
            );
        }
    }

    IEnumerator PauseMoving(Vector3 startPosition, Vector3 endPosition, GameObject movingObject)
    {
        yield return new WaitForSeconds(pauseDuration);
        if (Vector3.Distance(movingObject.transform.position, endPosition) <= Mathf.Epsilon)
        {
            // switch start- and endpostion
            StartCoroutine(MoveObject(
                startPosition: endPosition,
                endPosition: startPosition,
                movingObject: movingObject
                )
            );
        } else
        {
            StartCoroutine(MoveObject(startPosition, endPosition, movingObject));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(gameObject.transform.position, destiantion.transform.position);
    }
}
