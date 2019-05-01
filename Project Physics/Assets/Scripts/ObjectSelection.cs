using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    enum ETransformMode
    {
        NONE,
        MOVE,
        ROTATE,
        SCALEUL,
        SCALEUM,
        SCALEUR,
        SCALELM,
        SCALERM,
        SCALEDL,
        SCALEDM,
        SCALEDR
    }

    public GameObject go;
    public SpriteRenderer sr;
    BoxCollider2D[] bc;
    ETransformMode mode = ETransformMode.NONE;
    bool dragging;
    int worldBlockIndex = -1;
    Vector3 prevMousePos;

    void Start()
    {
        bc = GetComponents<BoxCollider2D>();
    }

    void Update ()
    {
        sr = GetComponent<SpriteRenderer>();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // changes mode depending on what is clicked
            if (hit.collider == bc[0]) mode = ETransformMode.SCALEUL;
            else if (hit.collider == bc[1]) mode = ETransformMode.SCALEUM;
            else if (hit.collider == bc[2]) mode = ETransformMode.SCALEUR;
            else if (hit.collider == bc[3]) mode = ETransformMode.SCALELM;
            else if (hit.collider == bc[4]) mode = ETransformMode.MOVE;
            else if (hit.collider == bc[5]) mode = ETransformMode.SCALERM;
            else if (hit.collider == bc[6]) mode = ETransformMode.SCALEDL;
            else if (hit.collider == bc[7]) mode = ETransformMode.SCALEDM;
            else if (hit.collider == bc[8]) mode = ETransformMode.SCALEDR;
            else mode = ETransformMode.ROTATE;
            //Debug.Log(mode.ToString());
            dragging = true;
            prevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (dragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool chained = Master.manager.editorItems[worldBlockIndex].chainedScaling;

            switch (mode)
            {
                case ETransformMode.MOVE:
                    {
                        transform.position += mousePos - prevMousePos;
                        //go.transform.position = transform.position;
                        prevMousePos = mousePos;
                        break;
                    }

                case ETransformMode.ROTATE:
                    {
                        float prevAng = Mathf.Rad2Deg * Mathf.Atan2(prevMousePos.y - transform.position.y, prevMousePos.x - transform.position.x);
                        float currAng = Mathf.Rad2Deg * Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);

                        transform.rotation *= Quaternion.AngleAxis(currAng - prevAng, Vector3.forward);
                        prevMousePos = mousePos;

                        break;
                    }

                case ETransformMode.SCALEUL:
                    {
                        Vector3 mouseOffset = mousePos - prevMousePos;
                        mouseOffset = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * mouseOffset;

                        if (!chained)
                        {
                            ScaleUp(mouseOffset.y);
                            ScaleLeft(mouseOffset.x);
                        }
                        else
                        {
                            ScaleAll(mouseOffset);
                        }

                        prevMousePos = mousePos;

                        break;
                    }

                case ETransformMode.SCALEUM:
                    {
                        Vector3 mouseOffset = mousePos - prevMousePos;
                        mouseOffset = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * mouseOffset;

                        if (!chained)
                        {
                            ScaleUp(mouseOffset.y);
                        }
                        else
                        {
                            ScaleAll(mouseOffset);
                        }

                        prevMousePos = mousePos;

                        break;
                    }

                case ETransformMode.SCALEUR:
                    {
                        Vector3 mouseOffset = mousePos - prevMousePos;
                        mouseOffset = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * mouseOffset;

                        if (!chained)
                        {
                            ScaleUp(mouseOffset.y);
                            ScaleRight(mouseOffset.x);
                        }
                        else
                        {
                            ScaleAll(mouseOffset);
                        }

                        prevMousePos = mousePos;

                        break;
                    }

                case ETransformMode.SCALELM:
                    {
                        Vector3 mouseOffset = mousePos - prevMousePos;
                        mouseOffset = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * mouseOffset;

                        if (!chained)
                        {
                            ScaleLeft(mouseOffset.x);
                        }
                        else
                        {
                            ScaleAll(mouseOffset);
                        }

                        prevMousePos = mousePos;

                        break;
                    }

                case ETransformMode.SCALERM:
                    {
                        Vector3 mouseOffset = mousePos - prevMousePos;
                        mouseOffset = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * mouseOffset;

                        if (!chained)
                        {
                            ScaleRight(mouseOffset.x);
                        }
                        else
                        {
                            ScaleAll(mouseOffset);
                        }

                        prevMousePos = mousePos;

                        break;
                    }

                case ETransformMode.SCALEDL:
                    {
                        Vector3 mouseOffset = mousePos - prevMousePos;
                        mouseOffset = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * mouseOffset;

                        if (!chained)
                        {
                            ScaleDown(mouseOffset.y);
                            ScaleLeft(mouseOffset.x);
                        }
                        else
                        {
                            ScaleAll(mouseOffset);
                        }

                        prevMousePos = mousePos;

                        break;
                    }

                case ETransformMode.SCALEDM:
                    {
                        Vector3 mouseOffset = mousePos - prevMousePos;
                        mouseOffset = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * mouseOffset;

                        if (!chained)
                        {
                            ScaleDown(mouseOffset.y);
                        }
                        else
                        {
                            ScaleAll(mouseOffset);
                        }

                        prevMousePos = mousePos;

                        break;
                    }

                case ETransformMode.SCALEDR:
                    {
                        Vector3 mouseOffset = mousePos - prevMousePos;
                        mouseOffset = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z) * mouseOffset;

                        if (!chained)
                        {
                            ScaleDown(mouseOffset.y);
                            ScaleRight(mouseOffset.x);
                        }
                        else
                        {
                            ScaleAll(mouseOffset);
                        }

                        prevMousePos = mousePos;

                        break;
                    }
            }

            if (go != null)
            {
                go.transform.position = transform.position + Vector3.forward*5;
                go.transform.rotation = transform.rotation;
                go.transform.localScale = new Vector3(sr.size.x, sr.size.y, go.transform.localScale.z);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // set position of the corner colliders
            bc[0].offset = new Vector2(-sr.size.x / 2f + 0.02f, sr.size.y / 2f - 0.02f);
            bc[2].offset = new Vector2(sr.size.x / 2f - 0.02f, sr.size.y / 2f - 0.02f);
            bc[6].offset = new Vector2(-sr.size.x / 2f + 0.02f, -sr.size.y / 2f + 0.02f);
            bc[8].offset = new Vector2(sr.size.x / 2f - 0.02f, -sr.size.y / 2f + 0.02f);

            // set position of the edges
            bc[1].offset = new Vector2(0f, sr.size.y / 2f - 0.02f);
            bc[1].size = new Vector2(sr.size.x - 0.08f, 0.04f);
            bc[3].offset = new Vector2(-sr.size.x / 2f + 0.02f, 0f);
            bc[3].size = new Vector2(0.04f, sr.size.y - 0.08f);
            bc[5].offset = new Vector2(sr.size.x / 2f - 0.02f, 0f);
            bc[5].size = new Vector2(0.04f, sr.size.y - 0.08f);
            bc[7].offset = new Vector2(0f, -sr.size.y / 2f + 0.02f);
            bc[7].size = new Vector2(sr.size.x - 0.08f, 0.04f);

            // set scale of middle collider
            bc[4].size = new Vector2(sr.size.x-0.08f, sr.size.y-0.08f);

            dragging = false;
        }
	}

    public void SetObject(GameObject gameObj, int index)
    {
        go = gameObj;
        if (go == null) return;
        transform.position = gameObj.transform.position - Vector3.forward*5;
        transform.rotation = gameObj.transform.rotation;
        sr.size = gameObj.transform.localScale;
        mode = ETransformMode.NONE;
        worldBlockIndex = index;
    }

    void ScaleUp(float dist)
    {
        sr.size = new Vector2(sr.size.x, sr.size.y + dist);
        transform.position += transform.up * (dist / 2f);
    }

    void ScaleLeft(float dist)
    {
        sr.size = new Vector2(sr.size.x - dist, sr.size.y);
        transform.position += -transform.right * (-dist / 2f);
    }

    void ScaleRight(float dist)
    {
        sr.size = new Vector2(sr.size.x + dist, sr.size.y);
        transform.position += transform.right * (dist / 2f);
    }

    void ScaleDown(float dist)
    {
        sr.size = new Vector2(sr.size.x, sr.size.y - dist);
        transform.position += -transform.up * (-dist / 2f);
    }

    void ScaleAll(Vector3 dist)
    {
        sr.size = new Vector2(sr.size.x + dist.x*2f, sr.size.y + dist.x*2f);
    }
}