using System.Collections;
using UnityEngine;

public class FrogBox : MonoBehaviour
{
    private RectTransform rect;

    public Vector3 OriginalScreenPoint;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        OriginalScreenPoint = rect.position;
    }

    void Update()
    {
        rect.position = OriginalScreenPoint;
    }
}