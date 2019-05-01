using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
    private Light light;    
    public float intensity;

	// Use this for initialization
	void Start ()
	{
	    light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localRotation = (transform.gameObject.name == "Sun") ?
            Quaternion.Euler(Time.time + 45, 0, 0) :
            Quaternion.Euler(Time.time + 225, 0, 0);
        //light.intensity = (Time.time+45%360 > 180) ? Mathf.Abs((Time.time+45 % 360f) - 270f) / 22.5f - 3 : 1;
    }
}
