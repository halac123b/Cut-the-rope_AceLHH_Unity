using UnityEngine;

public class SpotlightEffect : MonoBehaviour
{
    private float _rotationSpeed = 10f;

    private void Update()
    {
        transform.Rotate(Vector3.back, _rotationSpeed * Time.deltaTime);
    }
}