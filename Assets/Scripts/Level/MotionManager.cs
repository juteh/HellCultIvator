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

    private float? _startTime;
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private void Start()
    {
        _startPosition = gameObject.transform.position;
        _endPosition = _startPosition + destiantion.transform.localPosition;
        StartCoroutine(
            MoveObject(
                startPosition: _startPosition,
                endPosition: _endPosition,
                movingObject: gameObject
            )
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Seed")
        {
            Vector3 startPosition = new Vector3(
                other.gameObject.transform.position.x,
                _startPosition.y + Mathf.Abs(gameObject.transform.position.y - other.gameObject.transform.position.y),
                other.gameObject.transform.position.z
            );
            Vector3 endPosition = startPosition + destiantion.transform.localPosition;
            StartCoroutine(
                MoveObject(
                    startPosition: startPosition,
                    endPosition: endPosition,
                    movingObject: other.gameObject
                )
           );
        }
    }

    IEnumerator MoveObject(Vector3 startPosition, Vector3 endPosition, GameObject movingObject)
    {
        if (movingDuration == 0)
        {
            movingObject.transform.position = endPosition;
        }
        while (Vector3.Distance(movingObject.transform.position, endPosition) > Mathf.Epsilon)
        {
            if (_startTime == null)
            {
                _startTime = Time.time;
            }
            float timePassed = Time.time - (float)_startTime;
            float progress = timePassed / movingDuration;

            movingObject.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            yield return new WaitForFixedUpdate();
        }
        _startTime = null;
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
