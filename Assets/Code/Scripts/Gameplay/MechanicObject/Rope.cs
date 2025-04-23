using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform RopeFirstObject;
    public Transform RopeSecondObject;
    public Camera MainCamera;
    public float RopeLength;
    [SerializeField] private float _gravity = -1.5f;
    [SerializeField] private GameObject _ropeSegmentPrefab;

    private LineRenderer _ropeRenderer;
    private List<RopeSegment> _ropeSegments = new();
    private float _ropeSegmentLen = 0.05f;
    private float _ropeWidth = 0.03f;
    private SpringJoint2D _joint;
    private EdgeCollider2D _edgeCollider;
    private Transform[] _ropePoints;

    private void Start()
    {
        _ropeRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();

        int numberSegment = (int)(RopeLength * 20);

        Vector3 ropeStartPoint = new(RopeFirstObject.position.x, RopeFirstObject.position.y, 0f);
        _ropePoints = new Transform[numberSegment + 2];
        DistanceJoint2D previousJoint = RopeFirstObject.AddComponent<DistanceJoint2D>();
        _ropePoints[0] = RopeFirstObject;

        for (int i = 1; i < numberSegment + 1; i++)
        {
            GameObject seg = Instantiate(_ropeSegmentPrefab, previousJoint.transform.position - Vector3.up * 0.05f, Quaternion.identity, transform);
            var rb = seg.GetComponent<Rigidbody2D>();
            var joint = seg.GetComponent<DistanceJoint2D>();
            previousJoint.connectedBody = rb;
            _ropePoints[i] = seg.transform;
            previousJoint = joint;
        }
        previousJoint.connectedBody = RopeSecondObject.GetComponent<Rigidbody2D>();
        _ropePoints[^1] = RopeSecondObject;
        _ropeRenderer.positionCount = _ropePoints.Length;

        // for (int i = 0; i < RopeLength; i++)
        // {
        //     _ropeSegments.Add(new RopeSegment(ropeStartPoint));
        //     ropeStartPoint.y -= _ropeSegmentLen;
        // }

        _joint = RopeFirstObject.GetComponent<SpringJoint2D>();
        // _joint.connectedBody = RopeSecondObject.GetComponent<Rigidbody2D>();
        // _joint.autoConfigureDistance = false;
        // _joint.distance = RopeLength * 0.1f;

        MainCamera = Camera.main;

        _ropeRenderer.startWidth = _ropeWidth;
        _ropeRenderer.endWidth = _ropeWidth;

        Candy candy = RopeSecondObject.GetComponent<Candy>();
        candy.AttachRope(this);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

            if (hit != null && hit.CompareTag("Rope"))
            {
                Rope rope = hit.GetComponent<Rope>();
                Candy ropeCandy = RopeSecondObject.GetComponent<Candy>();

                if (rope != null && ropeCandy != null)
                {
                    rope._joint.connectedBody = null;
                    ropeCandy.DetachRope(this);
                }

                Destroy(hit.gameObject);
            }
        }

        DrawRope();
    }

    private void FixedUpdate()
    {
        SimulatePosition();

        // for (int i = 0; i < 5; i++)
        // {
        //     ApplyConstraint();
        // }
    }

    private void LateUpdate()
    {
        UpdateColliderForRope();
    }

    private void UpdateColliderForRope()
    {
        // int segmentStep = 3;
        // List<Vector2> edgePoints = new(_ropeSegments.Count);

        // for (int i = 0; i < _ropeRenderer.positionCount; i += segmentStep)
        // {
        //     Vector3 ropePointPos = _ropeRenderer.GetPosition(i);
        //     edgePoints.Add(new Vector2(ropePointPos.x, ropePointPos.y));
        // }

        // if ((_ropeSegments.Count - 1) % segmentStep != 0)
        // {
        //     Vector3 lastPos = _ropeRenderer.GetPosition(_ropeSegments.Count - 1);
        //     edgePoints.Add(new Vector2(lastPos.x, lastPos.y));
        // }

        // _edgeCollider.points = edgePoints.ToArray();

        Vector2[] edgePoints = new Vector2[_ropePoints.Length];
        for (int i = 0; i < _ropePoints.Length; i++)
        {
            // EdgeCollider2D expects local positions
            edgePoints[i] = transform.InverseTransformPoint(_ropePoints[i].position);
        }

        _edgeCollider.points = edgePoints;
    }

    /// <summary> Apply gravity to the rope segments </summary>
    private void SimulatePosition()
    {
        // Vector3 forceGravity = new(0, _gravity, 0);

        // for (int i = 1; i < RopeLength; i++)
        // {
        //     RopeSegment ropeSegment = _ropeSegments[i];
        //     // ropeSegment.posOld = ropeSegment.posNow;
        //     ropeSegment.posNow += forceGravity * Time.deltaTime;

        //     _ropeSegments[i] = ropeSegment;
        // }

        // ApplyConstraint();
    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = _ropeSegments[0];
        firstSegment.posNow = new Vector3(RopeFirstObject.position.x, RopeFirstObject.position.y, 0f);
        _ropeSegments[0] = firstSegment;

        RopeSegment endSegment = _ropeSegments[^1];
        endSegment.posNow = new Vector3(RopeSecondObject.position.x, RopeSecondObject.position.y, 0f);
        _ropeSegments[^1] = endSegment;

        for (int i = 0; i < RopeLength - 1; i++)
        {
            RopeSegment firstSeg = _ropeSegments[i];
            RopeSegment secondSeg = _ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - _ropeSegmentLen);
            Vector3 changeDir = Vector2.zero;

            if (dist > _ropeSegmentLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < _ropeSegmentLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector3 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                _ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                _ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                _ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope()
    {
        // Vector3[] ropePositions = new Vector3[RopeLength];

        // for (int i = 0; i < RopeLength; i++)
        // {
        //     ropePositions[i] = _ropeSegments[i].posNow;
        // }

        // _ropeRenderer.positionCount = ropePositions.Length;
        // _ropeRenderer.SetPositions(ropePositions);

        for (int i = 0; i < _ropePoints.Length; i++)
        {
            _ropeRenderer.SetPosition(i, _ropePoints[i].position);
        }
    }

    public struct RopeSegment
    {
        public Vector3 posNow;
        // public Vector3 posOld;

        public RopeSegment(Vector3 pos)
        {
            posNow = pos;
            // posOld = pos;
        }
    }
}