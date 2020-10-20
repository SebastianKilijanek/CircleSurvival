using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Assets.Scripts.Infrastructure
{
    public class GameEventBus
    {
        [Serializable]
        private class ParametrizedEvent<T> : UnityEvent<T>
        {
        }

        private Dictionary<Type, UnityEventBase> events = new Dictionary<Type, UnityEventBase>();

        public void Trigger<TEvent>(TEvent @event) where TEvent : IGameEvent
        {
            var type = typeof(TEvent);
            if (events.ContainsKey(type))
            {
                ((ParametrizedEvent<TEvent>)events[type]).Invoke(@event);
            }
        }

        public void On<TEvent>(UnityAction<TEvent> listener) where TEvent : IGameEvent
        {
            var type = typeof(TEvent);
            if (!events.ContainsKey(type))
            {
                events.Add(type, new ParametrizedEvent<TEvent>());
            }

            ((ParametrizedEvent<TEvent>)events[type]).AddListener(listener);
        }

        public void Off<TEvent>(UnityAction<TEvent> listener) where TEvent : IGameEvent
        {
            var type = typeof(TEvent);
            if (events.ContainsKey(type))
            {
                ((ParametrizedEvent<TEvent>)events[type]).RemoveListener(listener);
            }
        }
    }
}
