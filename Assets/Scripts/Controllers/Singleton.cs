using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = GetComponent<T>();
            InitializeFields();
        }
        else
        {
            Debug.Log("Instance of type " + typeof(T) + " already exists");
            Destroy(this);
        }
    }

    protected abstract void InitializeFields();
}