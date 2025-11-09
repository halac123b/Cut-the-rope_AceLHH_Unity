using System;
using UnityEngine;

public class ElectronicSpark : MonoBehaviour
{
   [SerializeField] private Animator _animator;

   private void Start()
   {
      _animator.Play("Idle");
   }
   
   private void OnTriggerStay2D(Collider2D other)
   {
      if (other.CompareTag("Candy"))
      {
         EventDispatcher.Instance.Dispatch(null, EventDispatcher.TriggerElectronicSpark);
      }
   }
}
