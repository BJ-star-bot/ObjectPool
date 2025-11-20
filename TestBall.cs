using UnityEngine;

public class TestBall : MonoBehaviour, IPoolAble
{
    public Vector3 direction = new Vector3(0, 0, 0);
    public float force = 15f;
    public Vector3 velocity = Vector3.zero;
    public bool usegravity = false;
    public float gravity = -9.8f;
    public float damping = 0.9f;
    public float radius = 0.5f;
    public LayerMask bounceMask = Physics.DefaultRaycastLayers;
    private float existTime = 10f;
    private float timer = 0;
    public float damage = 100f;
    public GameObject Ownner;
    private ObjectPool<TestBall> pool;

    public void ReturnPool()
    {
        if (pool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            pool.ReturnObject(this);
            timer = 0;
        }
    }
    void HitTarget(Collider target)
    {
        var t = target.GetComponentInParent<IDamageable>();
        if (t != null)
        {
            var dct = new DamageContext
            {
                damage = damage,
                penetration = 0,
                damageSource = Ownner
            };
            t.TakeDamage(dct);

        }

    }
    public void SetPool<T>(ObjectPool<T> pool) where T : MonoBehaviour, IPoolAble
    {
        this.pool = pool as ObjectPool<TestBall>;
    }

    public void OnSpawn()
    {
        transform.position = Vector3.zero;
        direction = UnityEngine.Random.onUnitSphere;
        velocity = direction.normalized * force;
        timer = 0;
    }

    public void OnDespawn()
    {
        transform.position = Vector3.zero;
        direction = Vector3.zero;
        velocity = Vector3.zero;
        timer = 0;
    }

    private void Tick(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= existTime)
        {
            ReturnPool();
            return;
        }

        SimulateMovement(deltaTime);
    }

    private void SimulateMovement(float deltaTime)
    {
        if (deltaTime <= 0f) return;

        if (usegravity)
            velocity.y += gravity * deltaTime;

        Vector3 displacement = velocity * deltaTime;
        if (displacement.sqrMagnitude <= Mathf.Epsilon) return;

        float distance = displacement.magnitude;
        Vector3 moveDir = displacement / distance;

        if (Physics.SphereCast(transform.position, radius, moveDir, out RaycastHit hit,
                distance, bounceMask, QueryTriggerInteraction.Ignore))
        {
            transform.position = hit.point + hit.normal * radius;
            HitTarget(hit.collider);
            velocity = Vector3.Reflect(velocity, hit.normal) * damping;
        }
        else
        {
            transform.position += displacement;
        }
    }
    void OnEnable()
    {
        CustomUpdateManager.customUpdate += Tick;
    }
    void OnDisable()
    {
        CustomUpdateManager.customUpdate -= Tick;
    }
}
