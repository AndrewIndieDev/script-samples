using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    private string text;

    void Start()
    {
        StartCoroutine(GetFeed());
    }

    IEnumerator GetFeed()
    {
        WWW myWWW = new WWW("https://www.dropbox.com/s/newwya8ushynroj/PWUpdate.txt?dl=1");
        yield return myWWW;
        text = myWWW.text;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            alignment = TextAnchor.UpperLeft,
            wordWrap = true
        };
        Rect rect = new Rect(10, 10, Screen.width / 2f, Screen.height / 2f);
        GUI.Box(rect, "");
        GUI.Label(rect, text, style);
    }
}