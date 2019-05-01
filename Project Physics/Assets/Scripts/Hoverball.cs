using UnityEngine;

public class Hoverball : ConnectionPoint
{

    float startHeight;
    Rigidbody2D rb;
    public float amp;

	// Use this for initialization
	void Start () {
        startHeight = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rb.AddForce(new Vector2(0, (startHeight - transform.position.y) * amp));
        rb.velocity *= 0.99f;
	}
}
