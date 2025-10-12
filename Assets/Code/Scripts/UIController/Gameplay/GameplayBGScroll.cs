using UnityEngine;

public class GameplayBGScroll : MonoBehaviour
{
   [SerializeField] private SpriteRenderer _bgSprite;
   [SerializeField] private SpriteRenderer _tileBGSprite;

   public void SetBG()
   {
      _bgSprite.sprite = UserProfile.Instance.SelectedBoxData.BGGameplay;
      _tileBGSprite.sprite = UserProfile.Instance.SelectedBoxData.TiledBG;
      _tileBGSprite.transform.position = new Vector3(0f, - UserProfile.Instance.SelectedBoxData.AdjustPosition, 0f);
   }
}
