using UnityEngine;

public enum XYZ
{
    X,
    Y,
    Z
}

public class SpinXYZ : MonoBehaviour
{
    public XYZ xyz = XYZ.X;
    public float speed;
	
	void FixedUpdate ()
    {
        switch (xyz)
        {
            case XYZ.X:
                transform.Rotate(speed, 0, 0);
                break;
            case XYZ.Y:
                transform.Rotate(0, speed, 0);
                break;
            case XYZ.Z:
                transform.Rotate(0, 0, speed);
                break;
        }
	}
}