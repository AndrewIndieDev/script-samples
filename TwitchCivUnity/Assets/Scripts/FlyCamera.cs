using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class FlyCamera : MonoBehaviour
{
    public AnimationCurve curve;
    private Vector2 prevMousePos;
    public float camSpeed = 0.25f;
    public float camRotateSpeed = 0.25f;
    public float sprintAmp = 2;
    public bool spectate;
    public Transform spectatePlayer;

	// Use this for initialization
	void Start ()
	{
	    spectate = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (!spectate)
	    {
            Camera.main.transform.parent = null;

            transform.position += transform.forward*Input.GetAxis("Vertical")*camSpeed*
	                              ((Input.GetKey(KeyCode.LeftShift)) ? sprintAmp : 1);
	        transform.position += transform.right*Input.GetAxis("Horizontal")*camSpeed*
	                              ((Input.GetKey(KeyCode.LeftShift)) ? sprintAmp : 1);
	        transform.position += transform.up*Input.GetAxis("Depth")*camSpeed*
	                              ((Input.GetKey(KeyCode.LeftShift)) ? sprintAmp : 1);

	        float scroll = Input.GetAxis("Mouse ScrollWheel");
	        if (scroll != 0)
	        {
	            camSpeed *= (scroll > 0) ? 1.1f : 0.9f;
	            camSpeed = Mathf.Clamp(camSpeed, 0.01f, 10);
	        }

	        if (Input.GetMouseButtonDown(1))
	        {
	            prevMousePos = Input.mousePosition;
	        }

	        if (Input.GetMouseButton(1))
	        {
	            transform.rotation *= Quaternion.Euler(-(Input.mousePosition.y - prevMousePos.y)*camRotateSpeed,
	                (Input.mousePosition.x - prevMousePos.x)*camRotateSpeed, 0);
	            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
	            prevMousePos = Input.mousePosition;
	        }
	    }
	    else
	    {
            float dist = Mathf.Clamp(Vector3.Distance(Camera.main.transform.position, spectatePlayer.transform.position), 0, 100);
            float spd = -0.15f*dist + 20;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, spectatePlayer.transform.position + Vector3.up * 2.5f, Time.deltaTime * spd);
            //Camera.main.transform.rotation = spectatePlayer.transform.FindChild("Armature").FindChild("Bone").transform.rotation;
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, spectatePlayer.transform.Find("Armature").Find("Bone").transform.rotation, Time.deltaTime * spd);
            Camera.main.transform.parent = spectatePlayer.transform.Find("Armature").Find("Bone");
        }
	}
}
