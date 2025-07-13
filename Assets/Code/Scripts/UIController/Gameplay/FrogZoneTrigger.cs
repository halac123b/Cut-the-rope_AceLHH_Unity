using System;
using UnityEngine;

public class FrogZoneTrigger : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Candy"))
        {
            Debug.Log("[FrogZoneTrigger] Candy vào vùng sẵn sàng ăn");

            if (_animator != null)
            {
                _animator.SetBool("Eating Ready", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Candy"))
        {
            Debug.Log("[FrogZoneTrigger] Candy rời vùng sẵn sàng ăn");

            if (_animator != null)
            {
                _animator.SetBool("Eating Ready", false);
            }
        }
    }

}

