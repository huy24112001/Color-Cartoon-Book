using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace MiracleLambda
{
    public static class EventID
    {
        public static string ONMONSTERKILLED = "OnMonsterKilled";
        public static string ONSURVIVEDMINUTES = "OnSurvivedMinutes";
        public static string ONBOSSKILLED = "OnBossKilled";
        public static string ONINGAMELEVELREQUIRED = "OnIngameLevelRequired";
    }

    public static class MiracleLambdaEventManager
    {
        #region Fields

        private static Dictionary<string, UnityEvent> _eventDictionary = new Dictionary<string, UnityEvent>();
        private static Dictionary<string, object> _storage = new Dictionary<string, object>();
        private static Dictionary<string, object> _sender = new Dictionary<string, object>();

        #endregion

        #region Start Listening

        public static void StartListening(string eventName, UnityAction callBack)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(callBack);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(callBack);
                _eventDictionary.Add(eventName, thisEvent);
            }
        }

        #endregion

        #region Stop Listening
        public static void StopListening(string eventName, UnityAction callBack)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.RemoveListener(callBack);
            }
        }

        #endregion

        #region Post Event
        public static void PostEvent(string eventName)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        public static void PostEvent(string eventName, float delay)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                if (delay <= 0)
                {
                    thisEvent.Invoke();
                }
                else
                {
                    var d = (int)(delay * 1000);
                    DelayedInvoke(thisEvent, d);
                }
            }
        }

        public static void PostEvent(string eventName, object sender)
        {
            if (_sender.ContainsKey(eventName))
            {
                _sender[eventName] = sender;
            }
            else
            {
                _sender.Add(eventName, sender);
            }

            PostEvent(eventName);
        }

        public static void PostEvent(string eventName, float delay, object sender)
        {
            if (_sender.ContainsKey(eventName))
            {
                _sender[eventName] = sender;
            }
            else
            {
                _sender.Add(eventName, sender);
            }

            PostEvent(eventName, delay);
        }

        public static void PostEventData(string eventName, object data)
        {
            SetData(eventName, data);
            PostEvent(eventName);
        }

        public static void PostEventData(string eventName, float delay, object data)
        {
            SetData(eventName, data);
            PostEvent(eventName, delay);
        }

        #endregion

        #region Get/Set Data

        public static void SetData(string eventName, object data)
        {
            if (_storage.ContainsKey(eventName))
            {
                _storage[eventName] = data;
            }
            else
            {
                _storage.Add(eventName, data);
            }
        }

        public static object GetData(string eventName)
        {
            try
            {
                return _storage.ContainsKey(eventName) ? _storage[eventName] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static GameObject GetDataGameObject(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (GameObject)_storage[eventName];
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int GetDataInt(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (int)_storage[eventName];
                }

                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static float GetDataFloat(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (float)_storage[eventName];
                }

                return 0f;
            }
            catch (Exception)
            {
                return 0f;
            }
        }

        public static bool GetDataBool(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (bool)_storage[eventName];
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetDataString(string eventName)
        {
            try
            {
                if (_storage.ContainsKey(eventName))
                {
                    return (string)_storage[eventName];
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static object GetSender(string eventName)
        {
            try
            {
                return _sender.ContainsKey(eventName) ? _sender[eventName] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Methods

        private static async void DelayedInvoke(UnityEvent thisEvent, int delay)
        {
            await Task.Delay(delay);
            thisEvent.Invoke();
        }

        public static void StopAllListeners()
        {
            foreach (var unityEvent in _eventDictionary)
            {
                unityEvent.Value.RemoveAllListeners();
            }

            _eventDictionary = new Dictionary<string, UnityEvent>();
        }

        public static bool IsListening()
        {
            return _eventDictionary.Count > 0;
        }

        public static bool EventExists(string eventName)
        {
            return _eventDictionary.ContainsKey(eventName);
        }

        #endregion
    }
}