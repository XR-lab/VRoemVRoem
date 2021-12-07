using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraPoliceLerp : MonoBehaviour
{
    [SerializeField] private float _delayAnimTime = 0;
    [SerializeField] private float _chaseSpeed = 3;
    [SerializeField] private float _driftSpeed = 3;
    [SerializeField] private float _minDistance = 0.5f;
    [SerializeField] private float _minBoundsX = -8;
    [SerializeField] private float _maxBoundsX = 8;
    [SerializeField] private float _rotSpeed = 4;

    public Vector3 targetPlayerPos;
    private Animator _anim;
    private bool _chasePlayer = false;
    private bool _stopLerp = false;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();

        Invoke(nameof(PlayDriftAnimation), _delayAnimTime);
    }

    private void PlayDriftAnimation()
    {
        _anim.SetTrigger("Drift");
    }

    // Update is called once per frame
    void Update()
    {
        if (_stopLerp)
        {
            return;
        }

        Vector3 targetPos;
        float speed = 0;

        if (!_chasePlayer)
        {
            targetPos = transform.position;
            targetPos.x = 0;
            speed = _driftSpeed;

            if (Vector3.Distance(transform.position, targetPos) < _minDistance)
            {
                _chasePlayer = true;
            }
        }
        else
        {
            targetPos = targetPlayerPos;
            speed = _chaseSpeed;
            targetPos.x = Mathf.Clamp(targetPos.x, _minBoundsX, _maxBoundsX);

            Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}
