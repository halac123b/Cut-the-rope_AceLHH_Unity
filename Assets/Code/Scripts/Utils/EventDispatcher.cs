using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoSingleton<EventDispatcher>
{
    public class IEventData
    {
        public GameObject GameObject { get; set; }
        public Action<object> ActionCallBack { get; set; }

        public IEventData(GameObject gameObject, Action<object> actionCallBack)
        {
            GameObject = gameObject;
            ActionCallBack = actionCallBack;
        }
    }

    private readonly Dictionary<string, List<IEventData>> dispatchMaps = new();

    public static string LoadLevelUI = "LoadLevelUI";

    public void AddEvent(GameObject gameObject, Action<object> action, string key)
    {
        if (Instance == null)
        {
            return;
        }

        bool is_existed = false;
        var eventDatas = Instance.GetEventDatasByScope(key);
        foreach (IEventData eventData in eventDatas)
        {
            if (eventData.GameObject == gameObject)
            {
                is_existed = true;
                break;
            }
        }

        if (!is_existed)
        {
            eventDatas.Add(new IEventData(gameObject, action));
        }
    }

    public void Dispatch(object action, string key)
    {
        List<IEventData> eventDatas = GetEventDatasByScope(key);
        foreach (IEventData eventData in eventDatas)
        {
            eventData.ActionCallBack?.Invoke(action);
        }
    }

    private List<IEventData> GetEventDatasByScope(string scope)
    {
        if (dispatchMaps.ContainsKey(scope))
        {
            return dispatchMaps[scope];
        }
        else
        {
            dispatchMaps[scope] = new List<IEventData>();
            return dispatchMaps[scope];
        }
    }

    public void RemoveEvent(GameObject gameObject)
    {
        if (Instance == null)
        {
            return;
        }

        foreach (List<IEventData> eventList in Instance.dispatchMaps.Values)
        {
            foreach (IEventData eventData in eventList)
            {
                if (eventData.GameObject == gameObject)
                {
                    eventList.Remove(eventData);
                    break;
                }
            }
        }
    }
}
