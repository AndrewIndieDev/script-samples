using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewsFeed : MonoBehaviour {

	// Use this for initialization
	public void Start ()
    {
        StartCoroutine(GetFeed());
	}

    IEnumerator GetFeed()
    {
        WWW myWWW = new WWW("https://www.dropbox.com/s/newwya8ushynroj/PWUpdate.txt?dl=1");
        yield return myWWW;
        GetComponent<Text>().text = myWWW.text;
    }
}
