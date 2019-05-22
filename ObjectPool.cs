using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Fields & Properties

    public bool initializeOnAwake = true;
    public int growFactor = 2;
    public int initialPoolSize = 20;
    [Tooltip("Optional. Otherwise will automaticaly attach to the pooler.")]
    public Transform parent;

    private Queue _queue;

    [SerializeField]
    private GameObject _poolTypePrefab;
    public GameObject PooltypePrefab
    {
        get { return _poolTypePrefab; }
        private set { _poolTypePrefab = value; }
    }

    #endregion

    #region UnityLocals

    private void Awake()
    {
        _queue = new Queue(initialPoolSize, growFactor);
        parent = parent == null ? transform : parent;

        if (initializeOnAwake)
        {
            Initialize(initialPoolSize);
        }
    }

    #endregion

    #region Methods

    private void Initialize(int numberOfElements)
    {
        for (int i = 0; i < numberOfElements; i++)
        {
            CreateInstance();
        }
    }

    private GameObject CreateInstance()
    {
        var g = Instantiate(_poolTypePrefab, parent);
        g.name = _poolTypePrefab + "_" + _queue.Count;
        _queue.Enqueue(g);
        g.SetActive(false);

        return g;
    }

    private void Grow()
    {
        Initialize(_queue.Count * growFactor - _queue.Count);
        for (int i = 0; i < _queue.Count; i++)
        {
            if ((_queue.Peek() as GameObject).activeSelf)
                _queue.Enqueue(_queue.Dequeue());
            else
                return;
        }
    }

    public GameObject Pull()
    {
        try
        {
            var obj = _queue.Dequeue() as GameObject;
            _queue.Enqueue(obj);

            if (obj.activeSelf)
                Grow();

            obj.SetActive(true);

            return obj;
        }
        catch (System.InvalidOperationException)
        {
            Debug.Log("You have not initialized the pool, therefore you cannot use it.", this);
            return null;
        }
        catch (System.Exception)
        {
            Debug.Log("An unknown exception occured");
            throw;
        }
    }

    #endregion

    #region DEBUG
#if UNITY_EDITOR

    [Header("DEBUG")]
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1)
        {
            timer = 0;
            Pull();
        }
    }

#endif
    #endregion

}
