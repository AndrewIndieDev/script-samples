using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FlyCamera : MonoBehaviour
{
    private Vector2 prevMousePos;
    public float camSpeed = 0.25f;
    public float camRotateSpeed = 0.25f;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Input.GetAxis("Vertical") * camSpeed;
        transform.position += transform.right * Input.GetAxis("Horizontal") * camSpeed;
        transform.position += transform.up * Input.GetAxis("Depth") * camSpeed;

        if (Input.GetMouseButtonDown(1))
	    {
	        prevMousePos = Input.mousePosition;
	    }

	    if (Input.GetMouseButton(1))
	    {
	        transform.rotation *= Quaternion.Euler(-(Input.mousePosition.y - prevMousePos.y) * camRotateSpeed, (Input.mousePosition.x - prevMousePos.x) * camRotateSpeed, 0);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
	        prevMousePos = Input.mousePosition;
	    }
    }
}
