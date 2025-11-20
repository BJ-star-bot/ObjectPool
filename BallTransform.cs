using UnityEngine;

public class BallTransform : MonoBehaviour
{
    public float gravity = 9.8f;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public float damping = 0.9f;//反弹速度衰减
    public float radius = 1f;//小球反弹半径
    public LayerMask bounceMask;//反弹检查层
    void CustomUpdate(float dt)
    {
        velocity.y += gravity * dt;
        Vector3 nextPos = transform.position + velocity * dt;
        if (Physics.SphereCast(transform.position, radius, velocity.normalized, out RaycastHit hit, velocity.magnitude * dt, bounceMask))
        {
            // 反弹：速度向量对法线进行反射
            velocity = Vector3.Reflect(velocity, hit.normal) * damping;

            // 修正位置：放在碰撞点附近
            nextPos = hit.point + hit.normal * radius;
        }
        transform.position = nextPos;
    }
}
