using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using LitMotion;
using UnityEngine.SceneManagement;

public class LevelSceneLoader : MonoBehaviour
{
    private LevelData _levelData;
    [SerializeField] private GameObject _staticPointPrefab;
    [SerializeField] private GameObject _potentialPoint;
    [SerializeField] private GameObject _candyPrefab;
    [SerializeField] private GameObject _ropePrefab;
    [SerializeField] private GameObject _frogPrefab;
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private GameObject _balloonPrefab;
    [SerializeField] private GameObject _tutorialSignPrefab;
    [SerializeField] private GameObject _spikePrefab;
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private GameObject _spiderPrefab;
    
    public Transform ParentObject;
    private List<GameObject> _listLoadedObj = new();
    private List<BaseEntity> _pendingTutorialSigns = new();

    private void Awake()
    {
        LoadLevelData(UserProfile.Instance.SelectedLevelIndex);
        LoadLevelMap();
    }

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, _ => ReloadLevel(), EventDispatcher.RestartLevel);
        EventDispatcher.Instance.AddEvent(gameObject, _ => LoadNextLevel(), EventDispatcher.LoadNextLevel);
        EventDispatcher.Instance.AddEvent(gameObject, (action) => { TriggerTutorialSign((int)action); },
            EventDispatcher.TriggerTutorial);
        EventDispatcher.Instance.AddEvent(gameObject, (action) =>
        {
            AddToListObjsLevel((GameObject) action);
        }, EventDispatcher.AddToListObjsLevel);
        EventDispatcher.Instance.AddEvent(gameObject, (action) => { TriggerTutorialSign((int)action); },
            EventDispatcher.TriggerTutorial);
    }


    /// <summary>
    /// Reload level - GameOver
    /// </summary>
    private void ReloadLevel()
    {
        UIController.Instance.ResetUI(); //Reset trạng thái UI 
        ClearMap();
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.DisableCompleteUI);
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.OnResetStars);
        EventDispatcher.Instance.Dispatch(null, EventDispatcher.PlayLevelTextAnimation);
        LoadLevelMap();
    }

    private void LoadNextLevel()
    {
        string levelIndex = UserProfile.Instance.SelectedLevelIndex; //"1_1"
        string[] parts = levelIndex.Split('_'); // => {[1], [1]}
        int firstNumber = int.Parse(parts[0]); // "1"
        int secondNumber = int.Parse(parts[1]); // "1"
        int nextLvIndex = secondNumber + 1; //1+1 = 2

        // UserProfile.Instance.SetLevel(secondNumber + "_" + nextLvIndex.ToString());
        int seasonIdx = UserProfile.Instance.SeasonIndex;
        bool finalLevel = IsFinalLevel(nextLvIndex);

        if (finalLevel)
        {
            UserProfile.Instance.IsCompleteBox = true;
            EventDispatcher.Instance.Dispatch(seasonIdx, EventDispatcher.LoadBoxUI);
            UIController.Instance.ResetUI();
            SceneManager.LoadScene("Home");
        }
        else
        {
            string levelName = $"{firstNumber}_{nextLvIndex}"; // => "1_2"
            UserProfile.Instance.SetLevel(levelName);
            UIController.Instance.ResetUI();
            ClearMap();
            EventDispatcher.Instance.Dispatch(null, EventDispatcher.DisableCompleteUI);
            LoadLevelData(levelName);
            LoadLevelMap();
        }
    }

    private bool IsFinalLevel(int nextLevel)
    {
        int totalBoxDataLevel = UserProfile.Instance.SelectedBoxData.NumberOfLevels;

        if (nextLevel > totalBoxDataLevel)
        {
            return true;
        }

        return false;
    }

    private void GetScrollLevel()
    {
        UserProfile.Instance.ScrollLevelData = _levelData.ScrollLevelData;
    }
    
    private void LoadLevelData(string levelName)
    {
        string levelLoad = $"Level{levelName}";
        //levelName = "Level1_1";
        _levelData = Resources.Load<LevelData>($"Level/{levelLoad}");
    }

    private void LoadLevelMap()
    {
        GetScrollLevel();

        EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.LoadScrollNextLevel);
        
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
            case ObjectCategory.PotentialPoint:
                JObject pointObj = JObject.Parse(entity.ExpandProperty);
                int ropeLength = (int)pointObj["RopeLength"];
                float scale   = (float)pointObj["Scale"];

                createdObj = Instantiate(_potentialPoint, entity.Position, Quaternion.identity);
                PotentialPoint potentialPoint = createdObj.GetComponent<PotentialPoint>();
                potentialPoint.RopeLength = ropeLength;
                potentialPoint.Scale = scale;
                break;
            case ObjectCategory.Candy:
                createdObj = Instantiate(_candyPrefab, entity.Position, Quaternion.identity);
                break;
            case ObjectCategory.Rope:
                JObject obj = JObject.Parse(entity.ExpandProperty);
                int firstIndex = (int)obj["FirstNailIndex"];
                int secondIndex = (int)obj["SecondNailIndex"];
                float lengthRope = (float)obj["LengthRope"];

                createdObj = Instantiate(_ropePrefab, entity.Position, Quaternion.identity);
                Rope rope = createdObj.GetComponent<Rope>();
                rope.RopeFirstObject = _listLoadedObj[firstIndex].transform;
                rope.RopeSecondObject = _listLoadedObj[secondIndex].transform;
                rope.RopeLength = (int)lengthRope;
                break;
            case ObjectCategory.Frog:
                createdObj = Instantiate(_frogPrefab, entity.Position, Quaternion.identity);
                UserProfile.Instance.PosFrog = entity.Position;
                break;
            case ObjectCategory.Star:
                createdObj = Instantiate(_starPrefab, entity.Position, Quaternion.identity);

                if (!string.IsNullOrEmpty(entity.ExpandProperty))
                {
                    JObject starData = JObject.Parse(entity.ExpandProperty);
                    bool disappearOnTrigger = (bool?)starData["IsLimitTime"] ?? false;
                    StarEffect starComp = createdObj.GetComponent<StarEffect>();
                    starComp.DisappearOnTrigger = disappearOnTrigger;
                    
                    if (disappearOnTrigger)
                        starComp.TriggerDisappear();
                }
                break;
            case ObjectCategory.Bubble:
                createdObj = Instantiate(_bubblePrefab, entity.Position, Quaternion.identity);
                break;
            case ObjectCategory.TutorialSign:
                JObject tutorialData = JObject.Parse(entity.ExpandProperty);

                bool showLater = (bool?)(tutorialData["ShowLater"]) ?? false;;

                if (showLater)
                    _pendingTutorialSigns.Add(entity);
                else
                    SpawnTutorialSign(entity);
                break;
            case ObjectCategory.Spike:
                createdObj = Instantiate(_spikePrefab, entity.Position, Quaternion.identity);
                JObject spikeData = JObject.Parse(entity.ExpandProperty);
                string spriteName = (string)spikeData["SpriteName"];
                SpriteRenderer sr = createdObj.GetComponent<SpriteRenderer>();
                BoxCollider2D collider = createdObj.GetComponent<BoxCollider2D>();

                if (!string.IsNullOrEmpty(spriteName))
                {
                    sr.sprite = Resources.Load<Sprite>($"SpikeSprites/spikes_0{spriteName}");
                    float colliderX = 0.5f;
                    switch (spriteName)
                    {
                        case "1":
                            colliderX = 0.5f;
                            break;
                        case "2":
                            colliderX = 1.0f;
                            break;
                        case "3":
                            colliderX = 1.5f;
                            break;
                        case "4":
                            colliderX = 2.0f;
                            break;
                    }

                    collider.size = new Vector2(colliderX, collider.size.y);
                    //Debug.Log($"[Spike],size of collider is: {colliderX}");
                    
                    bool rotateLoop = (bool?)spikeData["RotateLoop"] ?? false;
                    var spikeGo = createdObj;

                    if (rotateLoop)
                    {
                        float rotateSpeed = 40.0f;
                        float duration = 360f / rotateSpeed;

                        LMotion.Create(0f, -360f, duration)
                            .WithEase(Ease.Linear)
                            .WithLoops(-1, LoopType.Restart)
                            .Bind(x =>
                            {
                                if (spikeGo == null) return;
                                spikeGo.transform.localRotation = Quaternion.Euler(0, 0, x);
                            })
                            .AddTo(spikeGo);
                    }
                    
                    JArray posAArray = (JArray)spikeData["MovePointA"];
                    JArray posBArray = (JArray)spikeData["MovePointB"];

                    if (posAArray != null && posBArray != null)
                    {
                        Vector3 pointA = new Vector3((float)posAArray[0], (float)posAArray[1], 0);
                        Vector3 pointB = new Vector3((float)posBArray[0], (float)posBArray[1], 0);
                        float moveSpeed = (float?)(spikeData["MoveDuration"] ?? 3.0f) ?? 3.0f;

                        LMotion.Create(pointA, pointB, moveSpeed)
                            .WithEase(Ease.Linear)
                            .WithLoops(-1, LoopType.Flip)
                            .Bind(pos =>
                            {
                                if (spikeGo == null) return;
                                spikeGo.transform.position = pos;
                            })
                            .AddTo(spikeGo); 
                    }
                    
                    float rotationZ = (float)(spikeData["Rotation"] ?? 0f);
                    createdObj.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
                }

                break;
            case ObjectCategory.Balloon:
                createdObj = Instantiate(_balloonPrefab, entity.Position, Quaternion.identity);

                if (!string.IsNullOrEmpty(entity.ExpandProperty))
                {
                    JObject bubbleData = JObject.Parse(entity.ExpandProperty);
                    float rotationZ = (float)(bubbleData["Rotation"] ?? 0f);

                    createdObj.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
                }
                break;
            case ObjectCategory.Spider:
                createdObj = Instantiate(_spiderPrefab, entity.Position, Quaternion.identity);
                SpiderFollowRope.ResetStartDelay();
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
    
    private void AddToListObjsLevel(GameObject createdObj)
    {
        _listLoadedObj.Add(createdObj);
        createdObj.transform.SetParent(ParentObject);
    }

    private void TriggerTutorialSign(int id)
    {
        BaseEntity entity = _pendingTutorialSigns.Find(e => e.Id == id);

        if (entity == null)
            return;

        SpawnTutorialSign(entity);
        _pendingTutorialSigns.Remove(entity);
    }


    private void SpawnTutorialSign(BaseEntity entity)
    {
        GameObject createdObj = Instantiate(_tutorialSignPrefab, entity.Position, Quaternion.identity, ParentObject);

        if (!string.IsNullOrEmpty(entity.ExpandProperty))
        {
            JObject tutorialData = JObject.Parse(entity.ExpandProperty);

            string title = (string)tutorialData["Title"] ?? "";
            string spriteName = (string)tutorialData["Sprite"] ?? "";

            TutorialSign tutorialSign = createdObj.GetComponent<TutorialSign>();
            if (tutorialSign != null)
            {
                float rotationZ = (float)(tutorialData["Rotation"] ?? 0f);
                tutorialSign.IconTutorialSign.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);

                Sprite bodySprite = null;
                if (!string.IsNullOrEmpty(spriteName))
                {
                    Sprite[] allSprites = Resources.LoadAll<Sprite>("TutorialSprites/tutorial_signs");
                    foreach (Sprite sprite in allSprites)
                    {
                        if (sprite.name == $"tutorial_signs_{spriteName}")
                        {
                            bodySprite = sprite;
                            break;
                        }
                    }

                    if (bodySprite == null)
                    {
                        Debug.LogWarning($"Sprite not found: tutorial_signs_{spriteName}");
                    }
                    else
                    {
                        Transform iconTransform = tutorialSign.IconTutorialSign.transform;

                        float posX = (float)(tutorialData["SpritePosX"] ?? iconTransform.localPosition.x);
                        float posY = (float)(tutorialData["SpritePosY"] ?? iconTransform.localPosition.y);
                        float posZ = (float)(tutorialData["SpritePosZ"] ?? iconTransform.localPosition.z);
                        iconTransform.localPosition = new Vector3(posX, posY, posZ);

                        float scale = (float)(tutorialData["SpriteScale"] ?? iconTransform.localScale.x);
                        iconTransform.localScale = new Vector3(scale, scale, scale);
                    }
                }

                tutorialSign.SetContent(title, bodySprite, false);
            }
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
        Star,
        Balloon,
        TutorialSign,
        Spike,
        PotentialPoint,
        Bubble,
        Spider
    }
}