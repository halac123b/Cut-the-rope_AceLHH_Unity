using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class PoolObjs
{
    public string PoolName;
    public GameObject Prefab;

    [HideInInspector] public ObjectPool<GameObject> Pool;
}

public class ObjectSpawner : MonoSingleton<ObjectSpawner>
{
    [SerializeField] private List<PoolObjs> _objs;

    private Dictionary<string, PoolObjs> _lookup;

    protected override void Awake()
    {
        base.Awake();
        _lookup = new Dictionary<string, PoolObjs>();

        foreach (var entry in _objs)
        {
            var poolEntry = entry;
            poolEntry.Pool = new ObjectPool<GameObject>(
                () => Instantiate(entry.Prefab),
                obj => obj.SetActive(true),
                obj => obj.SetActive(false),
                obj => Destroy(obj),
                defaultCapacity: 10,
                maxSize: 100
            );

            _lookup[entry.PoolName] = poolEntry;
        }
    }

    public GameObject Spawn(string poolName, Vector3 pos)
    {
        if (!_lookup.TryGetValue(poolName, out var entry))
        {
            Debug.LogError($"Pool '{poolName}' not found!");
            return null;
        }

        GameObject obj = entry.Pool.Get();
        obj.transform.position = pos;
        return obj;
    }

    public GameObject Spawn(string poolName, Transform parent,Vector3 eulerAngle,  Vector3? localPos = null)
    {
        if (!_lookup.TryGetValue(poolName, out var entry))
        {
            Debug.LogError($"Pool '{poolName}' not found!");
            return null;
        }

        GameObject obj = entry.Pool.Get();
        obj.transform.SetParent(parent);

        obj.transform.position = localPos ?? Vector3.zero;
        obj.transform.localEulerAngles = eulerAngle;
        obj.transform.localScale = Vector3.one;

        return obj;
    }
    
    public void Release(string poolName, GameObject obj)
    {
        if (_lookup.TryGetValue(poolName, out var entry))
        {
            entry.Pool.Release(obj);
        }
        else
        {
            Debug.LogError($"Pool '{poolName}' not found!");
            Destroy(obj); // fallback nếu pool không tồn tại
        }
    }
}