using System.Collections.Generic;
using UnityEngine;

public interface IPoolAble //被放入池的物体需要继承
{
    void SetPool<T>(ObjectPool<T> pool) where T : MonoBehaviour, IPoolAble;//泛型方法
    void OnSpawn();//初始化
    void OnDespawn();//回收
}

public class ObjectPool<T> where T : MonoBehaviour, IPoolAble
{
    private GameObject gameObjectPrefab;
    private Queue<T> pool = new();
    private Transform objectParent;//创建物体将要放到的父物体
    public int step = 10;
    public int totalCount = 0;
    public int activeCount = 0;
    public List<T> activeList = new List<T>();
    public ObjectPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        this.gameObjectPrefab = prefab;
        this.objectParent = parent;
        ExpandPool(initialSize);
    }


    public T GetObject()
    {

        if (pool.Count <= 0) ExpandPool(step);
        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
        totalCount += 1;
        activeCount += 1;
        activeList.Add(obj);
        return obj;
        
    }

    public void ReturnObject(T obj)
    {
        if (obj == null) return;
        if (!pool.Contains(obj))
        {
            activeCount -= 1;
            obj.OnDespawn();//因为限制了T必须满足IPOOLable接口，所以能调用物体的该方法
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
            activeList.Remove(obj);
        }
    }

    private void ExpandPool(int expandNumber)
    {
        if (expandNumber <= 0) expandNumber = 1;
        for (int i = 0; i < expandNumber; i++)
            InstantiateObject();
    }

    private void InstantiateObject()
    {
        GameObject newObj = GameObject.Instantiate(gameObjectPrefab, objectParent);
        T objINS = newObj.GetComponent<T>();
        if (objINS == null)
        {
            GameObject.Destroy(newObj);
            Debug.LogError("实例化的物体不满足泛型");
            return;
        }
        objINS.SetPool(this);
        newObj.SetActive(false);
        pool.Enqueue(objINS);
    }
    public void Clear()
    {
        foreach (var obj in pool)
            if (obj != null) GameObject.Destroy(obj.gameObject);
        pool.Clear();

        foreach (var obj in activeList)
            if (obj != null) GameObject.Destroy(obj.gameObject);
        activeList.Clear();

        activeCount = 0;
    }
}
