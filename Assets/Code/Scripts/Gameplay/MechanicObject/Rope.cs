using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class Rope : MonoBehaviour
{
    public Transform RopeFirstObject;
    public Transform RopeSecondObject;
    public Camera mainCamera;
    public int RopeLength;
    [SerializeField] private float gravity = -1f;
    [SerializeField] private Material _whiteMaterial;
    [SerializeField] private GameObject _ropeNutPrefab;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _redMaterial;

    private LineRenderer _ropeRenderer;
    private List<RopeSegment> _ropeSegments = new();
    private float _ropeSegmentLen = 0.1f;
    private float _ropeWidth = 0.03f;
    private SpringJoint2D _springJoint;
    private EdgeCollider2D _edgeCollider;
    private Material _originalMat;
    private Candy _candy;
    private float _jointDistance;

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

        Rigidbody2D rb = RopeSecondObject.GetComponent<Rigidbody2D>();
        _jointDistance = (RopeLength + 5) * 0.1f;

        _springJoint = RopeFirstObject.GetComponent<SpringJoint2D>();
        _springJoint.connectedBody = rb;
        _springJoint.distance = _jointDistance;

        mainCamera = Camera.main;

        _candy = RopeSecondObject.GetComponent<Candy>();
        _candy.AttachRope(this);
    }

    private void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        if (UIController.Instance.IsCompleteLevel)
        {
            return;
        }

        Simulate();

        for (int i = 0; i < 10; i++)
        {
            ApplyConstraint();
        }

        UpdateJoint();
    }

    private void LateUpdate()
    {
        if (UIController.Instance.IsCompleteLevel)
        {
            return;
        }

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
            edgePoints.Add(lastPos);
        }

        _edgeCollider.points = edgePoints.ToArray();
    }

    private void Simulate()
    {
        Vector3 forceGravity = new(0, gravity, 0);

        for (int i = 1; i < RopeLength; i++)
        {
            RopeSegment ropeSegment = _ropeSegments[i];
            // Vector3 velocity = ropeSegment.posNow - ropeSegment.posOld;
            ropeSegment.posOld = ropeSegment.posNow;
            //ropeSegment.posNow += velocity;
            ropeSegment.posNow += forceGravity * Time.deltaTime;
            // Nếu muốn mô phỏng thêm lực như gió, lực bay nhẹ có thể thêm ở đây

            _ropeSegments[i] = ropeSegment;
        }
    }

    private void ApplyConstraint()
    {
        // Cố định 2 segment đầu cuối
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
            Vector3 delta = firstSeg.posNow - secondSeg.posNow;
            float f = (dist - _ropeSegmentLen) / dist;
            // float error = Mathf.Abs(dist - _ropeSegmentLen);
            // Vector3 changeDir = Vector2.zero;

            // if (dist > _ropeSegmentLen)
            // {
            //     changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            // }
            // else if (dist < _ropeSegmentLen)
            // {
            //     changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            // }

            // Vector3 changeAmount = changeDir * error;
            if (i != 0)
            {
                // firstSeg.posNow -= changeAmount * 0.5f;
                firstSeg.posNow -= f * 0.5f * delta;
                _ropeSegments[i] = firstSeg;
                // secondSeg.posNow += changeAmount * 0.5f;
                secondSeg.posNow += f * 0.5f * delta;
                _ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += f * delta;
                _ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope()
    {
        if (UIController.Instance != null && UIController.Instance.IsCompleteLevel || RopeSecondObject == null)
        {
            return;
        }

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

    public void CutAtPoint(Vector2 cutPoint)
    {
        StartCoroutine(FlashWhite(cutPoint));
    }

    private IEnumerator FlashWhite(Vector2 cutPoint)
    {
        Material originalStart = _ropeRenderer.material;

        _ropeRenderer.material = _whiteMaterial;

        int index = GetCutIndex(cutPoint, out Vector3 cutOnLine);

        yield return new WaitForSeconds(0.1f);

        _ropeRenderer.material = originalStart;
        Candy ropeCandy = RopeSecondObject.GetComponent<Candy>();

        if (ropeCandy != null)
        {
            _springJoint.connectedBody = null;
            ropeCandy.DetachRope(this);
            _ropeRenderer.material = _normalMaterial;
        }

        CutRope(index);
    }

    private int GetCutIndex(Vector2 hitPoint, out Vector3 cutOnLine)
    {
        int cutIndex = -1;
        cutOnLine = Vector3.zero;
        float minDist = float.MaxValue;

        for (int i = 0; i < _ropeRenderer.positionCount - 1; i++)
        {
            Vector3 a = _ropeRenderer.GetPosition(i);
            Vector3 b = _ropeRenderer.GetPosition(i + 1);

            Vector3 ab = b - a;

            float t = Vector2.Dot(hitPoint - (Vector2)a, (Vector2)ab) / ab.sqrMagnitude;
            t = Mathf.Clamp01(t);

            Vector3 proj = a + t * ab;

            float dist = Vector2.Distance(hitPoint, proj);

            if (dist < minDist)
            {
                minDist = dist;
                cutIndex = i;
                cutOnLine = proj;
            }
        }

        return cutIndex;
    }

    public void CutRope(int cutIndex)
    {
        if (_ropeRenderer.positionCount < 2 || cutIndex <= 0 || cutIndex >= _ropeRenderer.positionCount - 1)
        {
            Destroy(_ropeRenderer.gameObject);
            return;
        }

        GameObject ropeNut = Instantiate(_ropeNutPrefab);
        LineRenderer ropeCutted = ropeNut.GetComponent<LineRenderer>();
        ropeCutted.positionCount = cutIndex + 1;

        for (int i = 0; i <= cutIndex; i++)
        {
            ropeCutted.SetPosition(i, _ropeRenderer.GetPosition(i));
        }

        // Vector3 freeEnd = lrNut.GetPosition(lrNut.positionCount - 1);

        ropeNut.GetComponent<RopeEndWiggle>().Init();

        Destroy(_ropeRenderer.gameObject);
    }

    private void UpdateJoint()
    {
        float distance2D = Vector2.Distance(
            new Vector2(RopeSecondObject.position.x, RopeSecondObject.position.y),
            new Vector2(RopeFirstObject.position.x, RopeFirstObject.position.y)
        );

        if (distance2D <= _jointDistance)
        {
            _springJoint.enabled = false;
        }
        else
        {
            _springJoint.enabled = true;
        }

        if (distance2D >= _jointDistance * 1.4f)
        {
            _ropeRenderer.material = _redMaterial;
        }
        else
        {
            _ropeRenderer.material = _normalMaterial;
        }
    }
}