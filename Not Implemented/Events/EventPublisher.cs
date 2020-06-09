using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Events
{
    [CreateAssetMenu(menuName = "Events/Event Publisher")]
    public class EventPublisher : ScriptableObject
    {
        protected List<EventListener> _listeners = new List<EventListener>();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].EventRaisedHandler();
        }

        public virtual void RegisterListener(EventListener listener)
        { _listeners.Add(listener); }

        public void UnregisterListener(EventListener listener)
        { _listeners.Remove(listener); }

    }

    #region Extensions

    //[CreateAssetMenu(menuName = "Events/Flow Event Publisher")]
    //public class FlowEventPublisher : EventPublisher
    //{
        //public override void RegisterListener(EventListener listener)
        //{
        //    base.RegisterListener(listener);
        //}
        //public void RaiseFlowEvent()
        //{
        //    for (int i = _listeners.Count - 1; i >= 0; i--)
        //        _listeners[i].EventRaisedHandler();
        //}
    //}

    #endregion
}