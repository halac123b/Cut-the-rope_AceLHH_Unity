using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rope : MonoBehaviour
{
    [SerializeField] private GameObject ropeFirstObject;
    [SerializeField] private GameObject ropeSecondobject;
    [SerializeField] private int ropeLength;
    [SerializeField] private float gravity = -1.5f;
    
    public bool isActiveBalloon;
    
    private float _liftForce = 2f;
    private LineRenderer _ropeRenderer;
    private List<RopeSegment> _ropeSegments = new();
    private float _ropeSegmentLen = 0.05f;
    private float _ropeWidth = 0.03f;
    private SpringJoint2D _joint;

    private void Start()
    {
        _ropeRenderer = GetComponent<LineRenderer>();

        Vector3 ropeStartPoint = new (ropeFirstObject.transform.position.x, ropeFirstObject.transform.position.y, 0.1f);

        for (int i = 0; i < ropeLength; i++)
        {
            _ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= _ropeSegmentLen;
        }

        _joint = ropeFirstObject.GetComponent<SpringJoint2D>();
        _joint.connectedBody = ropeSecondobject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ropeSecondobject.transform.CompareTag("Candy") || ropeFirstObject.transform.CompareTag("Candy"))
            {
                _joint.connectedBody = null;
            }
            
            Destroy(this.gameObject);
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

    private void Simulate()
    {
        Vector3 forceGravity = new (0f, gravity, 0f);

        for (int i = 1; i < ropeLength; i++)
        {
            RopeSegment ropeSegment = _ropeSegments[i];
            Vector3 velocity = ropeSegment.posNow - ropeSegment.posOld;
            ropeSegment.posOld = ropeSegment.posNow;
            ropeSegment.posNow += velocity;
            ropeSegment.posNow += forceGravity * Time.deltaTime;
            _ropeSegments[i] = ropeSegment;
        }

        ApplyConstraint();
    }

    private void ApplyConstraint()
    {
        RopeSegment firstSegment = _ropeSegments[0];
        firstSegment.posNow = new Vector3 (ropeFirstObject.transform.position.x, ropeFirstObject.transform.position.y, 0.1f);
        _ropeSegments[0] = firstSegment;

        RopeSegment endSegment = _ropeSegments[_ropeSegments.Count - 1];
        endSegment.posNow = new Vector3 (ropeSecondobject.transform.position.x, ropeSecondobject.transform.position.y, 0.1f);
        _ropeSegments[_ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < ropeLength - 1; i++)
        {
            RopeSegment firstSeg = _ropeSegments[i];
            RopeSegment secondSeg = _ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this._ropeSegmentLen);
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

        Vector3[] ropePositions = new Vector3[ropeLength];

        for (int i = 0; i < ropeLength; i++)
        {
            ropePositions[i] = _ropeSegments[i].posNow;
        }

        _ropeRenderer.positionCount = ropePositions.Length;
        _ropeRenderer.SetPositions(ropePositions);

        Vector3[] ropeMaxPositions = new Vector3[2];
        ropeMaxPositions[0] = new Vector3(ropeFirstObject.transform.position.x, ropeFirstObject.transform.position.y, 0.1f);
        ropeMaxPositions[1] = new Vector3(ropeSecondobject.transform.position.x, ropeSecondobject.transform.position.y, 0.1f);
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