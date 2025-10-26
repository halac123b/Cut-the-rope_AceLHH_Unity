using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFollowRope : MonoBehaviour
{
    [SerializeField] private bool _loop;
    [SerializeField] private Animator _animator;
    
    private float _attachThreshold = 0.2f;
    private float _moveSpeed = 0.2f;
    private Rope _rope;
    private LineRenderer _line;
    private float _traveledDistance;
    private List<Vector3> _ropePoints = new();
    private bool _isOnRope;
    private bool _initialized;
    private bool _isDead;
    private bool _isFinished;

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, (action) =>
        {
            OnRopeReceived((Rope)action);
        }, EventDispatcher.GetRopeComponent);
        
        EventDispatcher.Instance.AddEvent(gameObject, (action) =>
        {
            Rope cutRope = (Rope)action;
            if (_rope == cutRope)
                OnRopeCut();
        }, EventDispatcher.RopeCut);
        
        StartCoroutine(RequestRopeIfNone());
    }

    private IEnumerator RequestRopeIfNone()
    {
        yield return new WaitForSeconds(0.1f);
        if (_rope == null)
        {
            EventDispatcher.Instance.Dispatch(this, EventDispatcher.GetRopeComponent);
        }
    }

    private void OnRopeReceived(Rope rope)
    {
        if (_initialized) return;

        var line = rope.GetComponent<LineRenderer>();
        if (line == null || line.positionCount < 2) return;

        List<Vector3> ropePositions = new();
        for (int i = 0; i < line.positionCount; i++)
            ropePositions.Add(line.GetPosition(i));

        float minDist, startDist;
        (minDist, startDist) = GetClosestPointOnRope(ropePositions, transform.position);

        if (minDist <= _attachThreshold)
        {
            _rope = rope;
            _line = line;
            _traveledDistance = startDist;
            StartCoroutine(StartMoveAfterDelay());
        }
    }
    private IEnumerator StartMoveAfterDelay()
    {
        yield return new WaitForSeconds(1.8f);
        _isOnRope = true;
        _initialized = true;
        _animator?.Play("spider_already");
        
        yield return new WaitForSeconds(0.5f);
        _animator?.Play("spider_walking");
    }

    private (float dist, float traveled) GetClosestPointOnRope(List<Vector3> ropePoints, Vector3 spiderPos)
    {
        float minDist = float.MaxValue;
        float accumulated = 0f;
        float traveledDist = 0f;

        for (int i = 0; i < ropePoints.Count - 1; i++)
        {
            Vector3 a = ropePoints[i];
            Vector3 b = ropePoints[i + 1];
            Vector3 ab = b - a;

            float t = Vector3.Dot(spiderPos - a, ab) / ab.sqrMagnitude;
            t = Mathf.Clamp01(t);
            Vector3 proj = a + t * ab;
            float dist = Vector3.Distance(spiderPos, proj);

            if (dist < minDist)
            {
                minDist = dist;
                traveledDist = accumulated + Vector3.Distance(a, proj);
            }

            accumulated += Vector3.Distance(a, b);
        }

        return (minDist, traveledDist);
    }
    
    private void OnRopeCut()
    {
        if (_isDead) return;
        _isDead = true;

        Debug.Log($"{name} rơi khỏi dây {_rope.name}!");
        _animator?.Play("spider_fail_target");
        
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            rb.gravityScale = 1f;
        }
    }

    private void Update()
    {
        if (!_isOnRope || _rope == null || _line == null) return;

        int count = _line.positionCount;
        if (count < 2) return;

        _ropePoints.Clear();
        for (int i = 0; i < count; i++)
            _ropePoints.Add(_line.GetPosition(i));

        float totalLength = 0f;
        for (int i = 0; i < _ropePoints.Count - 1; i++)
            totalLength += Vector3.Distance(_ropePoints[i], _ropePoints[i + 1]);

        _traveledDistance += _moveSpeed * Time.deltaTime;
        // if (_traveledDistance > totalLength)
        // {
        //     if (_loop)
        //         _traveledDistance = 0f;
        //     else
        //         _traveledDistance = totalLength;
        // }
        if (_traveledDistance >= totalLength)
        {
            _isFinished = true;
            _animator.Play("spider_achievements");
            Debug.Log($"{name} đã tới đích!");
            return;
        }

        float currentLength = 0f;
        for (int i = 0; i < _ropePoints.Count - 1; i++)
        {
            float segLen = Vector3.Distance(_ropePoints[i], _ropePoints[i + 1]);
            if (currentLength + segLen >= _traveledDistance)
            {
                float ratio = (_traveledDistance - currentLength) / segLen;
                Vector3 pos = Vector3.Lerp(_ropePoints[i], _ropePoints[i + 1], ratio);
                transform.position = pos;

                Vector3 dir = (_ropePoints[i + 1] - _ropePoints[i]).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
                break;
            }
            currentLength += segLen;
        }
    }
}
