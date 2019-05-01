using UnityEngine;

public class Spikeball : ConnectionPoint
{

    public float speed;
    float radius;
    public float radiusAmp = 0.2f;
    Vector2 closestPoint;
    Rigidbody2D rb;
    public LayerMask layer;
    public float force = 10;
    //Collider2D n;
    //Vector3 pos;

    void Start()
    {
        radius = GetComponent<CircleCollider2D>().radius + radiusAmp;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Collider2D[] nearest = Physics2D.OverlapCircleAll(transform.position, radius, layer);
        Collider2D near = null;
        float dist = 999;
        foreach (Collider2D t in nearest)
        {
            float d = Vector2.Distance(t.transform.position, transform.position);
            if (d < dist)
            {
                dist = d;
                near = t; //nearest collider
            }
        }
        if (near != null)
        {
            rb.gravityScale = 0;
            RaycastHit2D temp = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y), radius, Vector2.zero, 0f, layer);
            Vector3 tempDir = Quaternion.Euler(0, 0, (speed > 0) ? -90 : 90) * (new Vector3(temp.point.x, temp.point.y, 0) - transform.position).normalized * 0.075f;
            closestPoint = Physics2D.CircleCast(new Vector2(transform.position.x + tempDir.x, transform.position.y + tempDir.y), radius, Vector2.zero, 0.1f, layer).point;
            rb.AddForce((closestPoint - new Vector2(transform.position.x, transform.position.y)) * force);
            rb.angularVelocity = speed;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(closestPoint, 0.1f);
    }

}
