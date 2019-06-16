using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Fields & Properties

    public bool initializeOnAwake = true;
    public bool toGrow = true;
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
        if (toGrow)
            Initialize(_queue.Count * growFactor - _queue.Count);
        
        // See this? this goes through the entire queue just to find the new objects. It is pure shit.
        // This is why this script needs to change to two generic stacks. It'll be highly efficient.
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
            var obj = _queue.Peek() as GameObject;
            if (obj.activeSelf)
                Grow();

            obj = _queue.Dequeue() as GameObject;
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
    public bool createObjectsAtInterval = false;
    public float interval = 1;

    private float timer = 0f;
    Camera _mainCam;
    Vector3 _mainCamPose;

    private void Start()
    {
        _mainCam = Camera.main;
        _mainCamPose = Camera.main.transform.position;
    }

    private void Update()
    {
        if (createObjectsAtInterval)
        {
            timer += Time.deltaTime;

            if (timer > interval)
            {
                timer = 0;
                Pull().transform.position = new Vector3(Random.Range(_mainCamPose.x - _mainCam.orthographicSize * _mainCam.aspect, _mainCamPose.x + _mainCam.orthographicSize * _mainCam.aspect),
                    Random.Range(_mainCamPose.y - _mainCam.orthographicSize * _mainCam.aspect, _mainCamPose.y + _mainCam.orthographicSize * _mainCam.aspect),0);
            }
        }
    }

#endif
    #endregion

}
