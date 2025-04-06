using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private LevelData[] _season1st;
    [SerializeField] private LevelData[] _season2nd;
    [SerializeField] private LevelData[] _season3rd;

    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private Transform _gridLayoutGroup;

    public void LoadLevel(int seasonIndex)
    {
        for (int i = 0; i < _season1st.Length; i++)
        {
            Instantiate(_levelPrefab, _gridLayoutGroup);
        }
    }
}
