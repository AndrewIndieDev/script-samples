using UnityEngine;
using System.Collections;

public class GrapplingHook : ConnectionPoint
{
    public GameObject hook;
    public float time = 5f;
    public LayerMask worldLayer;
    public LayerMask environmentLayer;
    public int shotSpeed;
    public GameObject rope;

    private void Start()
    {
        rope.transform.parent = null;
        //foreach (var index in GetComponents<HingeJoint2D>())
        //{
        //    index.useLimits = true;
        //    index.limits = new JointAngleLimits2D() { min = 0, max = 0 };
        //}
        
        StartCoroutine(FireHook());
    }

    IEnumerator FireHook()
    {
        yield return new WaitForSeconds(time);
        Rigidbody2D rbTemp = hook.GetComponent<Rigidbody2D>();
        rbTemp.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rbTemp.isKinematic = false;
        hook.transform.parent = null;
        GrapplingHookHook temp = hook.AddComponent<GrapplingHookHook>();
        temp.speed = shotSpeed;
        temp.worldLayer = worldLayer;
        temp.environmentLayer = environmentLayer;
        temp.startRigid = GetComponent<Rigidbody2D>();
        temp.FireHook();
    }

    public void OnHit()
    {
        //TODO: do stuff when the hook hits (if needed)
    }

    private void OnDestroy()
    {
        if (hook != null && hook.transform.parent == null)
        {
            Destroy(hook);
        }
        if (rope != null && rope.transform.parent == null)
        {
            Destroy(rope);
        }
    }
}
