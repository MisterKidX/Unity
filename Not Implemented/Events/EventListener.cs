using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// https://unity.com/how-to/architect-game-code-scriptable-objects
namespace Utilities.Events
{
    public class EventListener : MonoBehaviour
    {
        [SerializeField] protected EventPublisher _event;
        [SerializeField] protected UnityEvent _response;

        private void OnEnable()
        {
            if (_event != null)
                _event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if(_event != null)
                _event.UnregisterListener(this);
        }

        public void EventRaisedHandler()
        { _response.Invoke(); }
    }

    #region Extesions

    //public class FlowEventListener : EventListener
    //{
    //    [SerializeField] FlowEvent _flowResponse;

    //    public void FlowEventRaisedHandler(FlowEventArgs args, object obj)
    //    {
    //        _flowResponse.Invoke(args, obj);
    //    }
    //}

    //public class FlowEventArgs : EventArgs
    //{
    //    public readonly GameObject playerContext;

    //    public FlowEventArgs(GameObject playerContext)
    //    {
    //        this.playerContext = playerContext;
    //    }
    //}

    //[System.Serializable]
    //public class FlowEvent : UnityEvent<FlowEventArgs, object> { }


    #endregion

}
