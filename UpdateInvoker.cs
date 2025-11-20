using System;
using UnityEngine;

public static class CustomUpdateManager
{

    public static event Action<float> customUpdate;
    public static void RaiseUpdate(float dt)
    {
        customUpdate?.Invoke(dt);
    }

}

public class UpdateInvoker : MonoBehaviour
{
    public float updateTime = 0.1f;
    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        while (timer >= updateTime)
        {
            CustomUpdateManager.RaiseUpdate(updateTime);
            timer -= updateTime;
        }
    }
}
