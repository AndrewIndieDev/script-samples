using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentManager : MonoBehaviour
{
    public enum EAttachmentType
    {
        PLACE,
        PAINT,
        REMOVE
    }

    public class AttachmentVisual
    {
        public GameObject gameObject;
        public EAttachmentType type;
        public Vector3 startPos;
        public Quaternion startRot;

        public AttachmentVisual(GameObject go, EAttachmentType t)
        {
            gameObject = go;
            type = t;
            startPos = go.transform.position;
            startRot = go.transform.rotation;
        }
    }

    [System.Serializable]
    public class Attachment
    {
        public GameObject visual;
        public GameObject functional;

        public Attachment(GameObject v, GameObject f)
        {
            visual = v;
            functional = f;
        }
    }

    [SerializeField] private Attachment[] attachments;
    [SerializeField] private float attachmentDisplayAngle = 25;
    private List<AttachmentVisual> currentAttachmentVisuals = new List<AttachmentVisual>();
    private GameObject currentAttachment;
    private float attachmentSelectionDistance = 0.005f;
    private bool attachmentsOpen = false;

    void Start()
    {
        VRInputManager.inputEventMenuRDown += ShowAttachments;
        VRInputManager.inputEventMenuR += UpdateAttachments;
        VRInputManager.inputEventMenuRUp += CloseAttachments;
        SetAttachment(attachments[0].functional);
    }

    void OnDestroy()
    {
        VRInputManager.inputEventMenuRDown -= ShowAttachments;
        VRInputManager.inputEventMenuR -= UpdateAttachments;
        VRInputManager.inputEventMenuRUp -= CloseAttachments;
    }

    void ShowAttachments()
    {
        float currentAng = -((attachments.Length-1) * attachmentDisplayAngle)/2f;
        for (var i = 0; i < attachments.Length; i++)
        {
            var index = attachments[i];
            GameObject temp = Instantiate(index.visual, transform);
            temp.transform.localPosition = Quaternion.AngleAxis(currentAng, Vector3.up) * (Vector3.forward * 0.2f);
            currentAng += attachmentDisplayAngle;
            temp.transform.parent = null;
            temp.transform.rotation = Quaternion.LookRotation(temp.transform.position - transform.position);
            temp.transform.localScale = Vector3.zero;
            currentAttachmentVisuals.Add(new AttachmentVisual(temp, (EAttachmentType)i));
        }
        attachmentsOpen = true;
    }

    void UpdateAttachments()
    {
        if (!attachmentsOpen) return;
        for (var i = 0; i < attachments.Length; i++)
        {
            float lerpValue = Mathf.Clamp01((0.1f - Vector3.Distance(transform.position, currentAttachmentVisuals[i].gameObject.transform.position)) * 10f);
            currentAttachmentVisuals[i].gameObject.transform.position = Vector3.Lerp(currentAttachmentVisuals[i].startPos, transform.position, lerpValue);
            currentAttachmentVisuals[i].gameObject.transform.rotation = Quaternion.Lerp(currentAttachmentVisuals[i].startRot, transform.rotation, lerpValue);
            currentAttachmentVisuals[i].gameObject.transform.localScale = Vector3.Lerp(currentAttachmentVisuals[i].gameObject.transform.localScale, Vector3.one, Time.deltaTime*10f);
            if (Vector3.Distance(transform.position, currentAttachmentVisuals[i].gameObject.transform.position) <= attachmentSelectionDistance)
            {
                SetAttachment(attachments[i].functional);
                return;
            }
        }
    }

    void CloseAttachments()
    {
        if (!attachmentsOpen) return;
        for (var i = currentAttachmentVisuals.Count - 1; i >= 0; i--)
        {
            Destroy(currentAttachmentVisuals[i].gameObject);
            currentAttachmentVisuals.RemoveAt(i);
        }
        attachmentsOpen = false;
    }

    void SetAttachment(GameObject go)
    {
        Destroy(currentAttachment);
        currentAttachment = Instantiate(go, transform);
        CloseAttachments();
    }
}