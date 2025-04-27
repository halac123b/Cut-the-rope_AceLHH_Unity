using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rope : MonoBehaviour
{
    public Transform RopeFirstObject;
    public Transform RopeSecondObject;
    public Camera mainCamera;
    public int RopeLength;
    [SerializeField] private float gravity = -1.5f;

    private LineRenderer _ropeRenderer;
    private List<RopeSegment> _ropeSegments = new();
    private float _ropeSegmentLen = 0.05f;
    private float _ropeWidth = 0.03f;
    private SpringJoint2D _joint;
    private EdgeCollider2D _edgeCollider;

    private void Start()
    {
        _ropeRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();

        Vector3 ropeStartPoint = new(RopeFirstObject.position.x, RopeFirstObject.position.y, 0f);

        for (int i = 0; i < RopeLength; i++)
        {
            _ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= _ropeSegmentLen;
        }

        _joint = RopeFirstObject.GetComponent<SpringJoint2D>();
        _joint.connectedBody = RopeSecondObject.GetComponent<Rigidbody2D>();
        _joint.distance = RopeLength * 0.1f;

        mainCamera = Camera.main;

        Candy candy = RopeSecondObject.GetComponent<Candy>();
        candy.AttachRope(this);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Rope"))
            {
                Rope rope = hit.collider.GetComponent<Rope>();
                Candy ropeCandy = RopeSecondObject.GetComponent<Candy>();

                if (rope != null && ropeCandy != null)
                {
                    rope._joint.connectedBody = null;
                    ropeCandy.DetachRope(this);
                }

                Destroy(hit.collider.gameObject);
            }
        }

        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();

        for (int i = 0; i < 5; i++)
        {
            ApplyConstraint();
        }
    }

    private void LateUpdate()
    {
        if (_ropeRenderer == null || _edgeCollider == null) 
            return;

        UpdateColliderForRope();
    }

    private void UpdateColliderForRope()
    {
        int segmentStep = 3;
        List<Vector2> edgePoints = new(_ropeSegments.Count);

        for (int i = 0; i < _ropeRenderer.positionCount; i += segmentStep)
        {
            Vector3 ropePointPos = _ropeRenderer.GetPosition(i);
            edgePoints.Add(new Vector2(ropePointPos.x, ropePointPos.y));
        }

        if ((_ropeSegments.Count - 1) % segmentStep != 0)
        {
            Vector3 lastPos = _ropeRenderer.GetPosition(_ropeRenderer.positionCount - 1);
            edgePoints.Add(new Vector2(lastPos.x, lastPos.y));
        }

        _edgeCollider.points = edgePoints.ToArray();
    }

    private void Simulate()
    {
        Vector3 forceGravity = new Vector3(0, gravity, 0);

        for (int i = 1; i < RopeLength; i++)
        {
            RopeSegment ropeSegment = _ropeSegments[i];
            Vector3 velocity = ropeSegment.posNow - ropeSegment.posOld;
            ropeSegment.posOld = ropeSegment.posNow;
            //ropeSegment.posNow += velocity;
            ropeSegment.posNow += forceGravity * Time.deltaTime;
            // Nếu muốn mô phỏng thêm lực như gió, lực bay nhẹ có thể thêm ở đây

            _ropeSegments[i] = ropeSegment;
        }

        ApplyConstraint();
    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = _ropeSegments[0];
        firstSegment.posNow = new Vector3(RopeFirstObject.position.x, RopeFirstObject.position.y, 0f);
        _ropeSegments[0] = firstSegment;

        RopeSegment endSegment = _ropeSegments[_ropeSegments.Count - 1];
        endSegment.posNow = new Vector3(RopeSecondObject.position.x, RopeSecondObject.position.y, 0f);
        _ropeSegments[_ropeSegments.Count - 1] = endSegment;

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
        float lineWidth = _ropeWidth;
        _ropeRenderer.startWidth = lineWidth;
        _ropeRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[RopeLength];

        for (int i = 0; i < RopeLength; i++)
        {
            ropePositions[i] = _ropeSegments[i].posNow;
        }

        _ropeRenderer.positionCount = ropePositions.Length;
        _ropeRenderer.SetPositions(ropePositions);

        Vector3[] ropeMaxPositions = new Vector3[2];
        ropeMaxPositions[0] = new Vector3(RopeFirstObject.position.x, RopeFirstObject.position.y, 0f);
        ropeMaxPositions[1] = new Vector3(RopeSecondObject.position.x, RopeSecondObject.position.y, 0f);
    }

    public struct RopeSegment
    {
        public Vector3 posNow;
        public Vector3 posOld;

        public RopeSegment(Vector3 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}