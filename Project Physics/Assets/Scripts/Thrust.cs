using UnityEngine;

public class Thrust : ConnectionPoint
{

    public float thrustSpeed;
    Rigidbody2D rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        rb.AddForce(transform.up * thrustSpeed);
	}
}
