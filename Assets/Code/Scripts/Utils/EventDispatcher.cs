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

    public static void AddEvent(GameObject gameObject, Action<object> action, params string[] scopes)
    {
        if (Instance == null)
        {
            return;
        }

        if (scopes.Length == 0)
        {
            scopes = new string[] { "" };
        }

        foreach (string scope in scopes)
        {
            bool is_existed = false;
            var eventDatas = Instance.GetEventDatasByScope(scope);
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
    }

    public void Dispatch(object action, params Event[] scopes)
    {
        // if (scopes.Length == 0)
        //     scopes = new Event[] { Event.NONE }; //default
        foreach (Event scope in scopes)
        {
            List<IEventData> eventDatas = GetEventDatasByScope(scope.ToString());
            foreach (IEventData eventData in eventDatas)
            {
                eventData.ActionCallBack?.Invoke(action);
            }
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
}
