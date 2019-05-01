using UnityEngine;

public class GlueBall : ConnectionPoint
{
    private bool isFree = true;
    public LayerMask worldLayer;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isFree)
        {
            if (other.gameObject.layer == Mathf.Log(worldLayer, 2))
            {
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<Rigidbody2D>().angularVelocity = 0f;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                isFree = false;
            }
            else //if (other.gameObject.GetComponent<ConnectionLine>())
            {
                HingeJoint2D temp = gameObject.AddComponent<HingeJoint2D>();
                temp.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
                isFree = false;
            }
        }
    }

}
