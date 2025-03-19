using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolObject
{
    public string m_lookUpString;                                                      //Crea un pool de objetos
    public List<GameObject> m_inactiveObjects = new List<GameObject>();
    public int m_currentSize = 0;
    public int m_defaultCapacity = 10;
    public int m_maxSize = 50;
    public bool m_isExpandable = true;
    public Transform m_parent;
}

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<PoolManager>();
            return instance;
        }
    }
    public enum PoolType
    {
        None = 0,
        GameObjects,
        ParticleSystems
    }

    [SerializeField] private Transform m_poolParentTransform;
    [SerializeField] private GameObject m_gameObjectsPool;
    [SerializeField] private GameObject m_particleSystemsPool;

    public List<PoolObject> m_pools = new List<PoolObject>();

    private void Awake()
    {
        SetupPoolHolders();
    }

    private void SetupPoolHolders()
    {
        if(m_gameObjectsPool == null) m_gameObjectsPool = new GameObject("GameObjectsPool");
        m_gameObjectsPool.transform.parent = (m_poolParentTransform != null) ? m_poolParentTransform : this.transform;
        if (m_particleSystemsPool == null) m_particleSystemsPool = new GameObject("ParticleSystemsPool");
        m_particleSystemsPool.transform.parent = (m_poolParentTransform != null) ? m_poolParentTransform : this.transform;
    }

    public bool SpawnObject<T>(out T returnedMonoBehaviour, GameObject _objectToSpawn, Vector3 _position, Quaternion _rotation, PoolType _poolType = PoolType.None) where T : MonoBehaviour{ 
        PoolObject pool = m_pools.Find(poolToFind => poolToFind.m_lookUpString == _objectToSpawn.name);

        if (pool == null) { 
            pool = new PoolObject() { m_lookUpString = _objectToSpawn.name};
            if (pool.m_parent == null)
            {
                GameObject poolParent = new GameObject(_objectToSpawn.name + "Pool");
                poolParent.transform.SetParent(SetParentObject(_poolType).transform);
                pool.m_parent = poolParent.transform;
            }
            m_pools.Add(pool);
        }

        returnedMonoBehaviour = null;

        if (pool.m_inactiveObjects.Count == 0 && pool.m_defaultCapacity > 0) {
            for (int i = 0; i < pool.m_defaultCapacity; i++)
            {
                returnedMonoBehaviour = Instantiate(_objectToSpawn, _position, _rotation).GetComponent<T>();
                returnedMonoBehaviour.gameObject.transform.SetParent(pool.m_parent);
                returnedMonoBehaviour.gameObject.SetActive(false);
                pool.m_inactiveObjects.Add(returnedMonoBehaviour.gameObject);
                pool.m_currentSize++;
            }
        }

        returnedMonoBehaviour = pool.m_inactiveObjects.FirstOrDefault().GetComponent<T>();

        if (returnedMonoBehaviour == null && pool.m_currentSize < pool.m_maxSize)
        {
            if (pool.m_currentSize < pool.m_maxSize || pool.m_isExpandable)
            {

                returnedMonoBehaviour = Instantiate(_objectToSpawn, _position, _rotation).GetComponent<T>();
                returnedMonoBehaviour.gameObject.transform.SetParent(pool.m_parent);
                pool.m_currentSize++;
            }
            else {
                return false;
            }
        }
        else { 
            returnedMonoBehaviour.transform.position = _position;
            returnedMonoBehaviour.transform.rotation = _rotation;
            pool.m_inactiveObjects.Remove(returnedMonoBehaviour.gameObject);
            returnedMonoBehaviour.gameObject.SetActive(true);
        }

        return true;
    }

    public bool SpawnObject<T>(out T returnedMonoBehaviour, GameObject _objectToSpawn, Transform _parent) where T : MonoBehaviour
    {
        PoolObject pool = m_pools.Find(poolToFind => poolToFind.m_lookUpString == _objectToSpawn.name);

        if (pool == null)
        {
            pool = new PoolObject() { m_lookUpString = _objectToSpawn.name };
            m_pools.Add(pool);
        }

        returnedMonoBehaviour = null;

        if (pool.m_inactiveObjects.Count == 0 && pool.m_defaultCapacity > 0)
        {
            for (int i = 0; i < pool.m_defaultCapacity; i++)
            {
                returnedMonoBehaviour = Instantiate(_objectToSpawn, _parent).GetComponent<T>();

                returnedMonoBehaviour.gameObject.SetActive(false);
                pool.m_inactiveObjects.Add(returnedMonoBehaviour.gameObject);
                pool.m_currentSize++;
            }
        }

        returnedMonoBehaviour = pool.m_inactiveObjects.FirstOrDefault().GetComponent<T>();

        if (returnedMonoBehaviour == null && pool.m_currentSize < pool.m_maxSize)
        {
            if (pool.m_currentSize < pool.m_maxSize || pool.m_isExpandable)
            {

                returnedMonoBehaviour = Instantiate(_objectToSpawn, _parent).GetComponent<T>();
                pool.m_currentSize++;
            }
            else
            {
                return false;
            }
        }
        else
        {
            pool.m_inactiveObjects.Remove(returnedMonoBehaviour.gameObject);
            returnedMonoBehaviour.gameObject.SetActive(true);
        }

        return true;
    }

    public bool SpawnObject<T>(out T returnedMonoBehaviour, GameObject _objectToSpawn, Vector3 _position, Quaternion _rotation, Transform _parent) where T : MonoBehaviour
    {
        PoolObject pool = m_pools.Find(poolToFind => poolToFind.m_lookUpString == _objectToSpawn.name);

        if (pool == null)
        {
            pool = new PoolObject() { m_lookUpString = _objectToSpawn.name };
            m_pools.Add(pool);
        }

        returnedMonoBehaviour = null;

        if (pool.m_inactiveObjects.Count == 0 && pool.m_defaultCapacity > 0)
        {
            for (int i = 0; i < pool.m_defaultCapacity; i++)
            {
                returnedMonoBehaviour = Instantiate(_objectToSpawn, _position, _rotation).GetComponent<T>();
                returnedMonoBehaviour.gameObject.transform.SetParent(_parent);
                returnedMonoBehaviour.gameObject.SetActive(false);
                pool.m_inactiveObjects.Add(returnedMonoBehaviour.gameObject);
                pool.m_currentSize++;
            }
        }

        returnedMonoBehaviour = pool.m_inactiveObjects.FirstOrDefault().GetComponent<T>();

        if (returnedMonoBehaviour == null && pool.m_currentSize < pool.m_maxSize)
        {
            if (pool.m_currentSize < pool.m_maxSize || pool.m_isExpandable)
            {

                returnedMonoBehaviour = Instantiate(_objectToSpawn, _position, _rotation).GetComponent<T>();
                pool.m_currentSize++;
                returnedMonoBehaviour.gameObject.transform.SetParent(_parent);
            }
            else
            {
                return false;
            }
        }
        else
        {
            returnedMonoBehaviour.transform.position = _position;
            returnedMonoBehaviour.transform.rotation = _rotation;
            pool.m_inactiveObjects.Remove(returnedMonoBehaviour.gameObject);
            returnedMonoBehaviour.gameObject.SetActive(true);
        }

        return true;
    }

    public void ReturnObjectToPool(GameObject _gameObject) { 
        //Take off 7 chars so that the "(Clone)" section of the name of the passed object is cut off
        string gameObjectName = _gameObject.name.Substring(0, _gameObject.name.Length - 7);

        PoolObject pool = m_pools.Find(poolToFind => poolToFind.m_lookUpString == gameObjectName);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + _gameObject.name);
        }
        else {
            _gameObject.SetActive(false);
            pool.m_inactiveObjects.Add(_gameObject);
        }
    }

    public void SetPoolmMaxSize(GameObject _objectThatSpawns, int _maxSize)
    {
        PoolObject pool = m_pools.Find(poolToFind => poolToFind.m_lookUpString == _objectThatSpawns.name);

        if (pool != null)
        {
            if (pool.m_inactiveObjects.Count < _maxSize)
            {
                pool.m_maxSize = _maxSize;
            }
            else {
                Debug.LogWarning("Tried to change the max size of a object pool that spawns: " + _objectThatSpawns.name + ". But the size of the pool is already more than the new max size.");
            }

        }
        else {
            Debug.LogWarning("Pool not found. Tried to change the max size of a object pool that spawns: " + _objectThatSpawns.name);
        }
    }

    public void SetPoolExpandable(GameObject _objectThatSpawns, bool _isExpandable)
    {
        PoolObject pool = m_pools.Find(poolToFind => poolToFind.m_lookUpString == _objectThatSpawns.name);

        if (pool != null)
        {
            pool.m_isExpandable = _isExpandable;
        }
        else
        {
            Debug.LogWarning("Pool not found. Tried to change the is expandable value of a object pool that spawns: " + _objectThatSpawns.name);
        }
    }

    private GameObject SetParentObject(PoolType _poolType)
    {
        switch (_poolType)
        {
            case PoolType.None:
                return null;
            case PoolType.GameObjects:
                return m_gameObjectsPool;
            case PoolType.ParticleSystems:
                return m_particleSystemsPool;
            default:
                return null;
        }
    }
}