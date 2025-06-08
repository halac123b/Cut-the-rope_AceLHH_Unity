using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelSceneLoader : MonoBehaviour
{
    private LevelData _levelData;
    [SerializeField] private GameObject _staticPointPrefab;
    [SerializeField] private GameObject _candyPrefab;
    [SerializeField] private GameObject _ropePrefab;
    [SerializeField] private GameObject _frogPrefab;
    [SerializeField] private GameObject _starPrefab;

    public Transform ParentObject;
    private List<GameObject> _listLoadedObj = new();

    private void Start()
    {
        LoadLevelData(UserProfile.Instance.SelectedLevelIndex);
        LoadLevelMap();
        EventDispatcher.Instance.AddEvent(gameObject, _ => ReloadLevel(), EventDispatcher.RestartLevel);
        EventDispatcher.Instance.AddEvent(gameObject, _ => LoadNextLevel(), EventDispatcher.LoadNextLevel);
    }


    /// <summary>
    /// Reload level - GameOver
    /// </summary>
    private void ReloadLevel()
    {
        ClearMap();
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.DisableCompleteUI);
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.OnResetStars);
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.PlayLevelTextAnimation);
        LoadLevelMap();
    }

    private void LoadNextLevel()
    {
        string levelIndex = UserProfile.Instance.SelectedLevelIndex; //"Level1_1"
        string[] parts = levelIndex.Split('_'); // => {[Level1], [1]}
        int firstNumber = int.Parse(parts[0]); // "Level1"
        int secondNumber = int.Parse(parts[1]); // "1"
        int nextLvIndex = secondNumber + 1; //1+1 = 2

        int seasonIdx = UserProfile.Instance.SeasonIndex;
        bool finalLevel = IsFinalLevel(nextLvIndex);

        Debug.Log($"final level: {finalLevel} - {nextLvIndex}");

        if (finalLevel)
        {
            UserProfile.Instance.IsCompleteBox = true;
            EventDispatcher.Instance.Dispatch(seasonIdx, EventDispatcher.LoadBoxUI);
            SceneManager.LoadScene("Home");
        }
        else
        {
            string levelName = $"{firstNumber}_{nextLvIndex}"; // => "Level1_2"

            ClearMap();
            EventDispatcher.Instance.Dispatch(null, EventDispatcher.DisableCompleteUI);
            LoadLevelData(levelName);
            LoadLevelMap();
        }
    }

    private bool IsFinalLevel(int nextLevel)
    {
        int totalBoxDataLevel = UserProfile.Instance.SelectedBoxData.NumberOfLevels;
        Debug.Log($"Final level: {totalBoxDataLevel}");

        if (nextLevel >= totalBoxDataLevel)
        {
            return true;
        }

        return false;
    }

    private void LoadLevelData(string levelName)
    {
        string levelLoad = $"Level{levelName}";
        //levelName = "Level1_1";
        _levelData = Resources.Load<LevelData>($"Level/{levelLoad}");
    }

    private void LoadLevelMap()
    {
        for (int i = 0; i < _levelData.ListEntities.Length; i++)
        {
            BaseEntity entity = _levelData.ListEntities[i];

            CreateStaticPoint(entity);
        }
    }

    private void CreateStaticPoint(BaseEntity entity)
    {
        GameObject createdObj = null;
        switch (entity.Category)
        {
            case ObjectCategory.StaticPoint:
                createdObj = Instantiate(_staticPointPrefab, entity.Position, Quaternion.identity);
                break;
            case ObjectCategory.Candy:
                createdObj = Instantiate(_candyPrefab, entity.Position, Quaternion.identity);
                break;
            case ObjectCategory.Rope:
                JObject obj = JObject.Parse(entity.ExpandProperty);
                int firstIndex = (int)obj["FirstNailIndex"];
                int secondIndex = (int)obj["SecondNailIndex"];
                int lengthRope = (int)obj["LengthRope"];

                createdObj = Instantiate(_ropePrefab, entity.Position, Quaternion.identity);
                Rope rope = createdObj.GetComponent<Rope>();
                rope.RopeFirstObject = _listLoadedObj[firstIndex].transform;
                rope.RopeSecondObject = _listLoadedObj[secondIndex].transform;
                rope.RopeLength = lengthRope;
                break;
            case ObjectCategory.Frog:
                createdObj = Instantiate(_frogPrefab, entity.Position, Quaternion.identity);
                break;
            case ObjectCategory.Star:
                createdObj = Instantiate(_starPrefab, entity.Position, Quaternion.identity);
                break;
            default:
                Debug.LogError($"Unknown category: {entity.Category}");
                break;
        }

        if (createdObj != null)
        {
            _listLoadedObj.Add(createdObj);
            createdObj.transform.SetParent(ParentObject);
        }
    }

    private void ClearMap()
    {
        foreach (Transform obj in ParentObject)
        {
            Destroy(obj.gameObject);
        }

        _listLoadedObj.Clear();
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }

    public enum ObjectCategory
    {
        StaticPoint,
        Candy,
        Rope,
        Frog,
        Star
    }
}