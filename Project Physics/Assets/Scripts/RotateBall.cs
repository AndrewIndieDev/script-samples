using UnityEngine;

public class RotateBall : ConnectionPoint {

    public float rotationSpeed;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.maxRotationSpeed = 100;
    }
	
	void FixedUpdate () {
        rb.AddTorque(rotationSpeed);
	}
}
