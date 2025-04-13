using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompleteLevelUI : MonoBehaviour
{
    public static event Action OnLevelComplete;
    [SerializeField] private Button _nextBtn;

    private void Start()
    {
        _nextBtn.onClick.AddListener(OnClickNextLevel);
    }

    private void OnClickNextLevel()
    {
        SceneManager.LoadScene("Home");
        
        OnLevelComplete?.Invoke();
    }
}
