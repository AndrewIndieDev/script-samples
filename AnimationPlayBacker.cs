using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayBacker : MonoBehaviour
{
    public TextAsset recording;

    public GameObject fakeHmd;
    public GameObject fakeLeftController;
    public GameObject fakeRightController;

    public float fps;
    [Range(-5,5)]
    public int speed;
    public bool looped;

    private List<MoCap.MoCapFrame> anim = new List<MoCap.MoCapFrame>();

	// Use this for initialization
	void Start () {
	    if (recording != null)
	    {
            string s = recording.text;
            anim = JsonFile.DeserializeMoCapList(recording);
	        PlayAnimation(looped);
	    }
	}

    void PlayAnimation(bool loop)
    {
        StartCoroutine(Play(loop));
    }

    IEnumerator Play(bool loop)
    {
        float frameIndex = 0;
        while (frameIndex < anim.Count)
        {
            int frame = Mathf.FloorToInt(frameIndex);
            float frac = frameIndex % 1;
            int nextFrame = frame+speed;
            if (nextFrame > anim.Count - 1)
                nextFrame = 0;
            if (nextFrame < 0)
                nextFrame = anim.Count - 1;

            fakeHmd.transform.localPosition = Vector3.Lerp(anim[frame].hmdPos.ToVector3(), anim[nextFrame].hmdPos.ToVector3(), frac);
            fakeLeftController.transform.localPosition = Vector3.Lerp(anim[frame].leftPos.ToVector3(), anim[nextFrame].leftPos.ToVector3(), frac);
            fakeRightController.transform.localPosition = Vector3.Lerp(anim[frame].rightPos.ToVector3(), anim[nextFrame].rightPos.ToVector3(), frac);

            fakeHmd.transform.localRotation = Quaternion.Lerp(anim[frame].hmdRot.ToQuaternion(), anim[nextFrame].hmdRot.ToQuaternion(), frac);
            fakeLeftController.transform.localRotation = Quaternion.Lerp(anim[frame].leftRot.ToQuaternion(), anim[nextFrame].leftRot.ToQuaternion(), frac);
            fakeRightController.transform.localRotation = Quaternion.Lerp(anim[frame].rightRot.ToQuaternion(), anim[nextFrame].rightRot.ToQuaternion(), frac);

            frameIndex += Time.deltaTime*fps*speed;
            if (frameIndex < 0) frameIndex = anim.Count - 1;
            if (frameIndex > anim.Count - 1) frameIndex = 0;

            yield return 0;
        }
    }
}
