using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] private float _lerpSpeed = 3;
    [SerializeField] private float _minDistance = 0.1f;

    private bool _moveCamera = false;
    private Vector3 _startPos;
    private Vector3 _targetPos;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (!_moveCamera)
    //    {
    //        return;
    //    }

    //    _targetPos.z = transform.position.z;
    //    transform.position = Vector3.Lerp(transform.position, _targetPos, _lerpSpeed * Time.deltaTime);

    //    if (Vector3.Distance(transform.position, _targetPos) < _minDistance)
    //    {
    //        _moveCamera = false;
            
    //    }
    //}

    public void MoveToStart()
    {
        MoveCameraToPos(_startPos);
    }

    public void MoveCameraToPos(Vector3 pos)
    {
        pos.z = transform.position.z;
        transform.position = pos;
    }
}
