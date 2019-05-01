using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLerp : MonoBehaviour
{
    public Transform child;
    public float speed = 5;

    void Start()
    {
        child.parent = transform.root;
    }

    void Update()
    {
        child.transform.position = Vector3.Lerp(child.transform.position, transform.position, Time.deltaTime * speed);
        child.transform.rotation = Quaternion.Lerp(child.transform.rotation, transform.rotation, Time.deltaTime * speed);
    }
}
