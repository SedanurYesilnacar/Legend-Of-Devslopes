using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _smoothing = 5f;

    Vector3 offset;


    void Start()
    {
        offset = transform.position - _target.position;
    }

    void Update()
    {
        Vector3 targetCamPos = _target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, Time.deltaTime * _smoothing);
    }
}
