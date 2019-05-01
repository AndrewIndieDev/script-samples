using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    #region Variables
    public static Control manager;

    [Range(0f, 0.5f)]
    public float cameraSpeed = 0.05f;
    public EBuildType buildType = EBuildType.ROTATE;
    public bool gameActive;
    public LayerMask connectableObjectLayer;
    public LayerMask nonConnectableObjectLayer;
    public LayerMask environmentBlockLayer;
    public LayerMask lineLayer;
    public LayerMask buildArea;
    public Texture2D wipTexture;
    public GameObject connectionPointPrefab;
    public List<Master.UserItem> userItems = new List<Master.UserItem>();
    public List<LayerMask> layersToDelete;

    private int buildTypeIndex = 0;
    private float orthoTo; //screen position (scrolling in and out) (Orthographic view)
    private GameObject currentLine; //the line currently being drawn
    private GameObject currentObject; //the object being placed
    private GameObject currentNonConnectableObject; //the object being placed that isn't a connectable
    private GameObject moveObject; //object being moved with the move tool
    private GameObject rotateObject; //object being rotated by the rotate tool
    private GameObject startNewPoint; //sets the starting game object when making a line
    private Vector3 startPos; //start position of the click
    private Vector3 cameraMousePos; //mouse position on the screen
    private Scene scene;
    private Scene oldScene;
    #endregion

    void Start()
    {
        orthoTo = Camera.main.orthographicSize;
        manager = this;
        userItems = Master.manager.userItems;
        environmentBlockLayer = LayerMask.NameToLayer("EnvironmentBlocks");
        scene = SceneManager.GetActiveScene();

        SaveStateManager.LoadState();
    }

    void Update()
    {
        //if (oldScene != scene) { ChangingScene(); }

        if (!gameActive)
        {
            if (buildType != EBuildType.MOVE && buildType != EBuildType.ROTATE)
            {
                #region Build
                if (buildType == EBuildType.SOLIDLINE || buildType == EBuildType.LITELINE)
                {
                    MakeLine(userItems[(int)buildType].buildTypePrefab);
                }

                else if (userItems[(int)buildType].connectable)
                //buildType == BuildType.BALLCLOCKWISE
                //    || buildType == BuildType.BALLCOUNTERCLOCKWISE
                //    || buildType == BuildType.THRUSTER
                //    || buildType == BuildType.PROPELLER
                //    || buildType == BuildType.HOVERBALL
                //    || buildType == BuildType.SPIKEBALLCLOCKWISE
                //    || buildType == BuildType.SPIKEBALLCOUNTERCLOCKWISE
                //    || buildType == BuildType.GLUEBALL
                //    || buildType == BuildType.GRAPPLINGHOOK
                {
                    MakeConnectableObject(userItems[(int)buildType].buildTypePrefab);
                }

                else if (buildType != EBuildType.CONNECTIONPOINT)
                {
                    MakeNonConnectableObject(userItems[(int)buildType].buildTypePrefab);
                }
#endregion
            }
            else if (buildType != EBuildType.MOVE)
            {
                #region Move
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                    if (hit.collider != null)
                    {
                        moveObject = hit.transform.gameObject;
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    if (moveObject != null)
                    {
                        if (Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, buildArea).collider != null)
                            moveObject.transform.position = mousePos;
                        foreach (GameObject t in moveObject.GetComponent<ConnectionPoint>().connections)
                        {
                            t.GetComponent<ConnectionLine>().UpdateLine();
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    moveObject = null;
                }
                #endregion
            }
            else if (buildType != EBuildType.ROTATE)
            {
                #region Rotate
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                    if (hit.collider != null)
                    {
                        rotateObject = hit.transform.gameObject;
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    if (rotateObject != null)
                    {
                        rotateObject.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(mousePos.y - rotateObject.transform.position.y, mousePos.x - rotateObject.transform.position.x) * Mathf.Rad2Deg, Vector3.forward);
                        rotateObject.transform.rotation *= Quaternion.Euler(0, 0, -90);
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (rotateObject != null)
                    {

                    }
                }
#endregion
            }

            #region Cycle Items
            if (Input.GetButtonDown("Next Item"))
            {
                buildTypeIndex = Mathf.Clamp(buildTypeIndex + 1, 0, Enum.GetNames(typeof(EBuildType)).Length - 2);
                buildType = (EBuildType)buildTypeIndex;
            }

            if (Input.GetButtonDown("Prev Item"))
            {
                buildTypeIndex = Mathf.Clamp(buildTypeIndex - 1, 0, Enum.GetNames(typeof(EBuildType)).Length - 2);
                buildType = (EBuildType)buildTypeIndex;
            }
            #endregion

            #region Delete
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                if (hit.collider != null)
                {
                    if (hit.transform.tag != "ConnectionPoint")
                    {
                        if (hit.transform.gameObject.GetComponent<ConnectionPoint>().connections.Count > 0)
                        {
                            GameObject tempConnectionPointGameObject = Instantiate(connectionPointPrefab, hit.transform.position, Quaternion.identity);
                            ConnectionPoint tempConnectionPoint = tempConnectionPointGameObject.GetComponent<ConnectionPoint>();
                            tempConnectionPoint.connections = hit.transform.gameObject.GetComponent<ConnectionPoint>().connections;
                            for (int i = 0; i < tempConnectionPoint.connections.Count; i++)
                            {
                                if (hit.transform.gameObject.GetComponent<ConnectionPoint>().connections[i].GetComponent<ConnectionLine>().connections[0] == hit.transform.gameObject.GetComponent<ConnectionPoint>())
                                {
                                    hit.transform.gameObject.GetComponent<ConnectionPoint>().connections[i].GetComponent<ConnectionLine>().connections[0] = tempConnectionPoint;
                                }
                                else if (hit.transform.gameObject.GetComponent<ConnectionPoint>().connections[i].GetComponent<ConnectionLine>().connections[1] == hit.transform.gameObject.GetComponent<ConnectionPoint>())
                                {
                                    hit.transform.gameObject.GetComponent<ConnectionPoint>().connections[i].GetComponent<ConnectionLine>().connections[1] = tempConnectionPoint;
                                }
                                tempConnectionPoint.gameObject.AddComponent<HingeJoint2D>().connectedBody = tempConnectionPoint.connections[i].GetComponent<Rigidbody2D>();
                            }
                        }
                        Destroy(hit.transform.gameObject);
                    }
                    else
                    {
                        for (int i = 0; i < hit.transform.gameObject.GetComponent<ConnectionPoint>().connections.Count; i = 0)
                        {
                            hit.transform.gameObject.GetComponent<ConnectionPoint>().connections[i].GetComponent<ConnectionLine>().DestroyLine();
                        }
                    }
                }
                else
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, lineLayer);
                    if (hit2.collider != null)
                    {
                        hit2.transform.gameObject.GetComponent<ConnectionLine>().DestroyLine();
                    }
                }

                RaycastHit2D hitNonCon = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, nonConnectableObjectLayer);
                if (hitNonCon.collider != null)
                {
                    Destroy(hitNonCon.collider.gameObject);
                }

            }
            #endregion

            #region Test Changing Level (Commented)
            //if (Input.GetKeyDown("1"))
            //{
            //    SceneManager.LoadScene("Menu");
            //}
            //if (Input.GetKeyDown("2"))
            //{
            //    SceneManager.LoadScene("Level1");
            //}
            //if (Input.GetKeyDown("3"))
            //{
            //    SceneManager.LoadScene("TestEnvironment");
            //}
            //if (Input.GetKeyDown("4"))
            //{
            //    SceneManager.LoadScene("LevelEditor");
            //}
            #endregion
        }
        else //if game is active
        {
            if (Input.GetButtonDown("ReloadLevel"))
            {
                Rigidbody2D[] rbs = FindObjectsOfType<Rigidbody2D>();
                foreach (Rigidbody2D t in rbs)
                {
                    Destroy(t.gameObject);
                }

                gameActive = false;
                SceneManager.LoadScene(scene.name);
            }
        }

        #region Camera Movement
        if (Input.GetMouseButtonDown(2))
        {
            cameraMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 delta = -(Input.mousePosition - cameraMousePos);
            Camera.main.transform.Translate(delta.x*0.02f*(Camera.main.orthographicSize/5), delta.y*0.02f*(Camera.main.orthographicSize / 5), 0);
            cameraMousePos = Input.mousePosition;
        }
        Camera.main.transform.Translate(Input.GetAxis("Horizontal") * cameraSpeed, Input.GetAxis("Vertical") * cameraSpeed, 0);
        orthoTo = Mathf.Clamp(orthoTo - Input.GetAxis("Mouse ScrollWheel")*2, 1, 10);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, orthoTo, Time.deltaTime * 7);
        #endregion

        #region Reload Level
        if (Input.GetKeyDown(KeyCode.L))
        {
            gameActive = false;
            foreach (Item item in FindObjectsOfType<Item>())
            {
                Destroy(item.gameObject);
            }
            foreach (ConnectionLine item in FindObjectsOfType<ConnectionLine>())
            {
                Destroy(item.gameObject);
            }
            SaveStateManager.LoadState();
        }
        #endregion

        if (CanStartGame() && !gameActive) //if left mouse button is not held down, and game is not already active
        {
            #region Hit Start Button
            if (Input.GetButtonDown("StartButton"))
            {
                SaveStateManager.SaveState();
                gameActive = true;

                Rigidbody2D[] rbs = FindObjectsOfType<Rigidbody2D>();
                foreach (Rigidbody2D t in rbs)
                {
                    t.isKinematic = false; //set kinematic to false on all those objects
                }

                Item[] item = FindObjectsOfType<Item>(); //cycle through all placeable objects
                foreach (Item t in item)
                {
                    t.enabled = true; //enable those objects to move

                    //get both thruster and propeller to have a min/max of 0, so no rotation with joints.
                    if (t.buildType == EBuildType.THRUSTER || t.buildType == EBuildType.PROPELLER || t.buildType == EBuildType.GRAPPLINGHOOK)
                    {
                        HingeJoint2D[] hingeArray = t.GetComponents<HingeJoint2D>();
                        for (int i = 0; i < hingeArray.Length; i++)
                        {
                            HingeJoint2D tempAngle = hingeArray[i];
                            tempAngle.useLimits = true;
                            tempAngle.limits = new JointAngleLimits2D() { min = 0, max = 0 };
                            hingeArray[i] = tempAngle;
                        }
                    }

                    if (t.buildType == EBuildType.CONNECTIONPOINT)
                    {
                        t.GetComponent<CircleCollider2D>().enabled = false;
                    }
                }

                SpinXYZ[] spinXYZ = FindObjectsOfType<SpinXYZ>();
                foreach (SpinXYZ spinObj in spinXYZ)
                {
                    spinObj.enabled = true;
                }
            }
            #endregion
        }
    }

    bool CanStartGame() //if left mouse button isn't held down
    {
        if (!Input.GetMouseButton(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateTransform(Vector3 targetPos) //transforming the line while placing/moving
    {
        currentLine.transform.position = Vector3.Lerp(startPos, targetPos, 0.5f);
        currentLine.transform.localScale = new Vector3(Vector3.Distance(startPos, targetPos), currentLine.transform.localScale.y, currentLine.transform.localScale.z);
        currentLine.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetPos.y - startPos.y, targetPos.x - startPos.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    void RemoveLine(ConnectionLine line)
    {
        line.DestroyLine();
    }

    void MakeLine(GameObject lineObject)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if (Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, buildArea))
        {
            if (Input.GetMouseButtonDown(0))
            {
                startNewPoint = null;
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                if (hit.collider != null)
                {
                    startPos = hit.transform.position;
                    currentLine = Instantiate(lineObject, startPos, Quaternion.identity);
                    hit.transform.gameObject.GetComponent<ConnectionPoint>().AddConnection(currentLine);
                    currentLine.GetComponent<ConnectionLine>().connections[0] = hit.transform.gameObject.GetComponent<ConnectionPoint>();
                    HingeJoint2D temp = hit.transform.gameObject.AddComponent<HingeJoint2D>();
                    currentLine.GetComponent<ConnectionLine>().hingeJoints[0] = temp;
                    temp.connectedBody = currentLine.GetComponent<Rigidbody2D>();
                    startNewPoint = hit.transform.gameObject;
                }
                else
                {
                    startPos = mousePos;
                    GameObject newPoint = Instantiate(connectionPointPrefab, startPos, Quaternion.identity);
                    currentLine = Instantiate(lineObject, startPos, Quaternion.identity);
                    newPoint.transform.gameObject.GetComponent<ConnectionPoint>().AddConnection(currentLine);
                    currentLine.GetComponent<ConnectionLine>().connections[0] = newPoint.transform.gameObject.GetComponent<ConnectionPoint>();
                    HingeJoint2D temp = newPoint.transform.gameObject.AddComponent<HingeJoint2D>();
                    currentLine.GetComponent<ConnectionLine>().hingeJoints[0] = temp;
                    temp.connectedBody = currentLine.GetComponent<Rigidbody2D>();
                    JointAngleLimits2D limits = temp.limits;
                    limits.min = Mathf.NegativeInfinity;
                    limits.max = Mathf.Infinity;
                    temp.limits = limits;
                    startNewPoint = newPoint;
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (currentLine != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                    var targetPos = hit.collider != null ? hit.transform.position : mousePos;
                    UpdateTransform(targetPos);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (currentLine != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                    if (hit.collider != null)
                    {
                        UpdateTransform(hit.transform.position);
                        hit.transform.gameObject.GetComponent<ConnectionPoint>().AddConnection(currentLine);
                        currentLine.GetComponent<ConnectionLine>().connections[1] = hit.transform.gameObject.GetComponent<ConnectionPoint>();
                        HingeJoint2D temp = hit.transform.gameObject.AddComponent<HingeJoint2D>();
                        currentLine.GetComponent<ConnectionLine>().hingeJoints[1] = temp;
                        temp.connectedBody = currentLine.GetComponent<Rigidbody2D>();
                        if (startNewPoint == hit.transform.gameObject)
                            RemoveLine(currentLine.GetComponent<ConnectionLine>());
                    }
                    else
                    {
                        UpdateTransform(mousePos);
                        GameObject newPoint = Instantiate(connectionPointPrefab, mousePos, Quaternion.identity);
                        newPoint.transform.gameObject.GetComponent<ConnectionPoint>().AddConnection(currentLine);
                        currentLine.GetComponent<ConnectionLine>().connections[1] = newPoint.transform.gameObject.GetComponent<ConnectionPoint>();
                        HingeJoint2D temp = newPoint.transform.gameObject.AddComponent<HingeJoint2D>();
                        currentLine.GetComponent<ConnectionLine>().hingeJoints[1] = temp;
                        temp.connectedBody = currentLine.GetComponent<Rigidbody2D>();
                        currentLine = null;
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (currentLine != null)
                {
                    UpdateTransform(mousePos);
                    GameObject newPoint = Instantiate(connectionPointPrefab, mousePos, Quaternion.identity);
                    newPoint.transform.gameObject.GetComponent<ConnectionPoint>().AddConnection(currentLine);
                    currentLine.GetComponent<ConnectionLine>().connections[1] = newPoint.transform.gameObject.GetComponent<ConnectionPoint>();
                    HingeJoint2D temp = newPoint.transform.gameObject.AddComponent<HingeJoint2D>();
                    currentLine.GetComponent<ConnectionLine>().hingeJoints[1] = temp;
                    temp.connectedBody = currentLine.GetComponent<Rigidbody2D>();
                    RemoveLine(currentLine.GetComponent<ConnectionLine>());
                    currentLine = null;
                }
            }
        }
    }

    void MakeConnectableObject(GameObject connectableObject)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if (Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, buildArea))
        {
            if (Input.GetMouseButtonDown(0))
            {
                startNewPoint = null;
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                if (hit.collider != null)
                {
                    if (hit.transform.tag == "ConnectionPoint")
                    {
                        startPos = hit.transform.position;
                        GameObject newObject = Instantiate(connectableObject, hit.transform.position, Quaternion.identity);
                        currentObject = newObject;
                    }
                    else
                    {
                        startPos = mousePos;
                        GameObject newObject = Instantiate(connectableObject, startPos, Quaternion.identity);
                        currentObject = newObject;
                    }
                }
                else
                {
                    startPos = mousePos;
                    GameObject newObject = Instantiate(connectableObject, startPos, Quaternion.identity);
                    currentObject = newObject;
                }
                currentObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }

            if (Input.GetMouseButton(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                if (currentObject != null)
                {
                    if (hit.transform != null && hit.transform.tag == "ConnectionPoint")
                    {
                        currentObject.transform.position = hit.transform.position;
                    }
                    else
                    {
                        currentObject.transform.position = mousePos;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (currentObject != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, connectableObjectLayer);
                    currentObject.layer = LayerMask.NameToLayer("ConnectableObject");
                    if (hit.transform != null)
                    {
                        if (hit.transform.tag == "ConnectionPoint")
                        {
                            currentObject.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, 0);
                            ConnectionPoint temp = hit.transform.GetComponent<ConnectionPoint>();
                            ConnectionPoint tempObj = currentObject.transform.GetComponent<ConnectionPoint>();
                            tempObj.connections = temp.connections;
                            foreach (GameObject t in tempObj.connections)
                            {
                                if (t.gameObject.GetComponent<ConnectionLine>().connections[0] == temp)
                                {
                                    t.gameObject.GetComponent<ConnectionLine>().connections[0] = tempObj;
                                }
                                else if (t.gameObject.GetComponent<ConnectionLine>().connections[1] == temp)
                                {
                                    t.gameObject.GetComponent<ConnectionLine>().connections[1] = tempObj;
                                }
                                currentObject.AddComponent<HingeJoint2D>().connectedBody = t.GetComponent<Rigidbody2D>();
                            }
                            Destroy(hit.transform.gameObject);
                        }
                        else
                        {
                            currentObject.transform.position = mousePos;
                        }
                    }
                    currentObject = null;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {

                currentObject = null;
            }
        }
    }

    void MakeNonConnectableObject(GameObject nonConnectableObject)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if (Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, buildArea))
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentNonConnectableObject = Instantiate(nonConnectableObject, mousePos, Quaternion.identity);
            }

            if (Input.GetMouseButton(0))
            {
                if (currentNonConnectableObject != null)
                {
                    currentNonConnectableObject.transform.position = mousePos;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                currentNonConnectableObject = null;
            }
        }
    }

    private void ChangingScene()
    {
        oldScene = scene;
        SaveStateManager.LoadState();
    }

    void OnGUI()
    {
        if (!gameActive)
        {
            int amount = Enum.GetNames(typeof(EBuildType)).Length-1;
            int index = -1;
            for (int i = 0; i < amount; i++)
            {
                var drawTexture = userItems[i].guiVisualPrefab ?? wipTexture;
                if (i == (int)buildType)
                {
                    index = i;
                }
                else
                {
                    GUI.Box(new Rect(i * (Screen.width / (float)amount), Screen.height - Screen.height / 10f, Screen.width / (float)amount, Screen.height / 10f), drawTexture);
                }
            }

            if (index != -1)
                GUI.Box(new Rect(index * (Screen.width / (float)amount) - 8, (Screen.height - Screen.height / 10) - 8, (Screen.width / (float)amount) + 16, (Screen.height / 10) + 16), userItems[index].guiVisualPrefab ? userItems[index].guiVisualPrefab : wipTexture);
        }
    } //draw the GUI for the buildType
}
