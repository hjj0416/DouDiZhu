using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GetExist(null);
                if (instance == null)
                    instance = Create(null);
                else
                    instance.Init();
            }
            return instance;
        }
    }

    public static T Create(Transform parent)
    {
        instance = GetExist(parent);
        if (instance != null)
        {
            instance.Init();
            return instance;
        }
        //create it
        Transform obj = new GameObject(typeof(T).Name).transform;
        obj.parent = parent;
        obj.localEulerAngles = Vector3.zero;
        obj.localPosition = Vector3.zero;
        instance = obj.gameObject.AddComponent<T>();
        if (!instance.Init())
        {
            Debug.LogError(instance.name + " init failed!");
        }
        return instance;
    }

    private static T GetExist(Transform parent)
    {
        if (instance) return instance;
        if (parent == null)
        {
            return FindObjectOfType<T>();
        }
        else
        {
            return parent.GetComponentInChildren<T>();
        }
    }

    public bool Inited { private set; get; }
    public virtual bool Init()
    {
        if (Inited)
        {
            return false;
        }
        DontDestroyOnLoad(this);
        Inited = true;
        return true;
    }
    public virtual bool UnInit()
    {
        if (!Inited)
        {
            return false;
        }
        Inited = false;
        return true;
    }
}
