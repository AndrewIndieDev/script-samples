using UnityEngine;
using System.Collections;

public class SpawnBox : MonoBehaviour
{
    private GameObject player;
    public GameObject particles;
    public GameObject[] unfoldParticles;
    void Start()
    {
        player = GetComponentInChildren<PlayerBehaviour>().gameObject;
        player.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Terrain")
        {
            OpenBox();
        }
    }

    void OpenBox()
    {
        particles.SetActive(true);
        GetComponent<Animator>().Play("PlayerBoxOpen");
        StartCoroutine(PlayPoofParticles());
        if (player != null)
        {
            player.SetActive(true);
            player.GetComponent<PlayerBehaviour>().enabled = false;
            StartCoroutine(PlayerStart());
        }
    }

    IEnumerator PlayerStart()
    {
        yield return new WaitForSeconds(2);
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<BoxCollider>());
        if (player != null)
        {
            player.GetComponent<PlayerBehaviour>().enabled = true;
            player.transform.parent = null;
        }
        StartCoroutine(Descend());
    }

    IEnumerator Descend()
    {
        yield return new WaitForSeconds(5f);
        float i = 1.5f;
        while (i > 0)
        {
            i -= Time.deltaTime;
            transform.localScale = Vector3.one * i;
            yield return 0;
        }
        Destroy(this.gameObject);
    }

    IEnumerator PlayPoofParticles()
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject i in unfoldParticles)
        {
            i.SetActive(true);
        }
    }
}
