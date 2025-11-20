using UnityEngine;
using UnityEngine.UI;

public class AddObjectManager : MonoBehaviour
{
    [Tooltip("每0.02秒产生多少object")]
    public int objectNumber = 10;
    public TestBall ball;

    public ShowFrameuI SFI;
    public bool start = false;
    public int totalCount = 0;
    public int activeCount = 0;
    public Toggle poolTick;

    [Tooltip("最多同时存在物体数")]
    public int maxObjectNumber = 100000;

    private ObjectPool<TestBall> ballPool;

    void Start()
    {
        Transform ballParent = new GameObject("PoolOfBall").transform;
        ballPool = new ObjectPool<TestBall>(ball.gameObject, 100, ballParent);
    }

    void FixedUpdate()
    {
        if (!start || totalCount > maxObjectNumber) return;

        for (int index = 0; index < objectNumber; index++)
        {
            if (poolTick.isOn)
            {
                ballPool.GetObject();
                totalCount = ballPool.totalCount;
                activeCount = ballPool.activeCount;
            }
            else
            {
                totalCount += 1;
                TestBall newBullet = Instantiate(ball);
                newBullet.OnSpawn();
            }
        }
    }

    void Update()
    {
        if (SFI != null && SFI.hide) return;
        if (Input.GetKeyDown(KeyCode.F)) start = !start;
    }
}
