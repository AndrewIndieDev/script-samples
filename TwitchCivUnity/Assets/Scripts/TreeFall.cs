using UnityEngine;
using System.Collections;

public class TreeFall : MonoBehaviour
{
    private Transform tree;
	// Use this for initialization
	void Start ()
	{
	    tree = transform.GetChild(0);
	    StartCoroutine(FallOver());
	}
	
	// Update is called once per frame
	IEnumerator FallOver()
	{
	    while (tree.localRotation.eulerAngles.x+10 < 360)
	    {
	        tree.rotation *= Quaternion.Euler(0, Time.deltaTime*100, 0);
	        yield return 0;
	    }
	    float i = 1;
	    while (i > 0)
	    {
	        i -= Time.deltaTime * 0.25f;
            tree.localPosition += Vector3.down * Time.deltaTime * 0.5f;
	        yield return 0;
	    }
        transform.GetChild(1).transform.parent = GameData.manager.environmentParent.transform;
	    Destroy(tree.gameObject);
        Destroy(this.gameObject);
	}
}
