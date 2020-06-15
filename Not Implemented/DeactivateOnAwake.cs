using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class DeactivateOnAwake : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}

