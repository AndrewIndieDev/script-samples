using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrapplingHookHook : MonoBehaviour
{
    private bool isFree = true;
    public LayerMask worldLayer;
    public LayerMask environmentLayer;
    public List<GameObject> connections = new List<GameObject>();
    public Rigidbody2D startRigid;
    public float speed = 30f;
    Rigidbody2D rb;

    private void Update()
    {
        //if (Vector3.Distance(transform.position, startRigid.transform.position) > 2.6)
        //{
        //    rb.velocity = Vector2.zero;
        //}
    }

    public void FireHook()
    {
        StartCoroutine(EnableCollider());
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(-transform.right * speed, ForceMode2D.Impulse);
        startRigid.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.05f);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFree)
        {
            if (other.gameObject.layer == (int)Mathf.Log(worldLayer, 2))
            {
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<Rigidbody2D>().angularVelocity = 0f;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                startRigid.gameObject.GetComponent<GrapplingHook>().OnHit();
                isFree = false;
            }
            else if (other.gameObject.layer == (int)Mathf.Log(environmentLayer, 2))
            {
                HingeJoint2D temp = gameObject.AddComponent<HingeJoint2D>();
                temp.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
                temp.useLimits = true;
                JointAngleLimits2D jal = temp.limits;
                jal.min = 0;
                jal.max = 0;
                //GetComponent<Rigidbody2D>().freezeRotation = false;
                startRigid.gameObject.GetComponent<GrapplingHook>().OnHit();
                isFree = false;
            }
        }
    }
}
