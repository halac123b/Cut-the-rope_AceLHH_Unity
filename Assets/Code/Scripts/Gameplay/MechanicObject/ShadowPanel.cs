using System;
using UnityEngine;

public class ShadowPanel : MonoBehaviour
{
   public RectTransform ShadowPanelRect;
   public float ShadowSpeed = 30f; // degrees per second

   private float _shadowSpeedUp = 1f;

   private void Update()
   {
       float deltaAngle = Time.deltaTime * ShadowSpeed * _shadowSpeedUp;
       
       ShadowPanelRect.Rotate(0f, 0f, deltaAngle);
   }
}
