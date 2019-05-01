using UnityEngine;
using System.Collections;

public class Dynamite : Item {

    public float timer;
    public float radius;
    public float force;
    public float jointBreakRadius;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(Explode());
	}
	
	IEnumerator Explode()
    {
        yield return new WaitForSeconds(timer);
        Vector2 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos,radius);
        Collider2D[] hingejoints = Physics2D.OverlapCircleAll(explosionPos, jointBreakRadius);

        foreach (Collider2D hit in hingejoints)
        {
            if (hit.GetComponent<HingeJoint2D>() != null)
            {
                for (int i = 0; i < hit.GetComponents<HingeJoint2D>().Length; i++)
                {
                    Destroy(hit.GetComponents<HingeJoint2D>()[i]);
                }
            }
        }

        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce((new Vector2(rb.transform.position.x, rb.transform.position.y) - explosionPos).normalized * (force * (-Vector2.Distance(transform.position, explosionPos) + radius)));
            }
        }
        Destroy(gameObject);
    }
}
