using UnityEngine;
using UnityEngine.UI;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] private BoxData[] _season1st;
    [SerializeField] private BoxData[] _season2nd;
    [SerializeField] private BoxData[] _season3rd;

    [SerializeField] private GameObject _boxPrefab;
    [SerializeField] private Transform _gridLayoutGroup;

    public void LoadBox(int seasonIndex)
    {
        BoxData[] levelList = null;
        switch (seasonIndex)
        {
            case 0:
                levelList = _season1st;
                break;
            case 1:
                levelList = _season2nd;
                break;
            case 2:
                levelList = _season3rd;
                break;
        }

        for (int i = 0; i < levelList.Length; i++)
        {
            GameObject boxGameObject = Instantiate(_boxPrefab, _gridLayoutGroup);
            LevelObject levelObj = boxGameObject.GetComponent<LevelObject>();
        }
    }
}