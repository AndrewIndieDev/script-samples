using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum EManipulationMode
{
    NONE,
    PLACE
}

public class EditorControl : MonoBehaviour
{
    public class WorldBlocks
    {
        public GameObject go;
        public int index;

        public WorldBlocks(GameObject go, int index)
        {
            this.go = go;
            this.index = index;
        }
    }

    private GameObject worldObject;
    private int worldObjectIndex = -1;
    private GameObject selectedObject;
    private ObjectSelection os;
    public LayerMask selectionLayer;
    private Vector3 mousePosWhenClick;
    private Vector3 mousePos;
    private Vector3 pixelMousePos;
    private Vector3 prevPixelMousePos;
    private bool enableGrid;
    [SerializeField]
    private Texture2D gridButtonTexture;
    [SerializeField]
    private Texture2D gridButtonTextureInv;
    private int gridSize;
    private int columns = 1;
    public float buttonSize = 50f;
    private EManipulationMode eMode = EManipulationMode.NONE;
    private List<WorldBlocks> worldBlocks = new List<WorldBlocks>();
    private Scene scene;
    private Scene oldScene;

    private void Start()
    {
        os = FindObjectOfType<ObjectSelection>();
        os.gameObject.SetActive(false);

        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        //if (oldScene != scene) { ChangingScene(); }

        prevPixelMousePos = pixelMousePos;
        pixelMousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && MouseInEditor())
        {
            mousePosWhenClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mousePos, ~selectionLayer);

            if (hit != null && mousePosWhenClick == mousePos)
            {
                selectedObject = hit.transform.gameObject;
                os.gameObject.SetActive(true);
                worldObjectIndex = GetWorldBlockIndex(selectedObject);
                os.SetObject(hit.gameObject, worldObjectIndex);

            }
            else if (mousePosWhenClick == mousePos)
            {
                selectedObject = null;
                os.gameObject.SetActive(false);
                os.SetObject(null, -1);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedObject = null;
            os.gameObject.SetActive(false);
            os.SetObject(null, -1);
        }

        if (Input.GetMouseButtonDown(0) && MouseInEditor() && eMode == EManipulationMode.PLACE)
        {
            if (worldObject != null && !os.gameObject.activeInHierarchy)
            {
                selectedObject = Instantiate(worldObject, mousePos, Quaternion.identity);
                selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, 0);
                worldBlocks.Add(new WorldBlocks(selectedObject, worldObjectIndex));
                eMode = EManipulationMode.NONE;
            }
        }

        if (Input.GetMouseButton(1))
        {
            Camera.main.transform.position -= Camera.main.ScreenToWorldPoint(pixelMousePos) - Camera.main.ScreenToWorldPoint(prevPixelMousePos);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize-Input.mouseScrollDelta.y/5f, 0.5f, 20f);
        }
    }

    private bool MouseInEditor()
    {
        return Input.mousePosition.x > columns * (buttonSize + 4) + 4;
    }

    private int GetWorldBlockIndex(GameObject go)
    {
        foreach (WorldBlocks wb in worldBlocks)
        {
            if (wb.go == go)
            {
                return wb.index;
            }
        }
        return -1;
    }

    private void OnGUI()
    {
        int nextLineCount = 0;
        columns = 1;
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical("box");
        for (int i = 0; i < Master.manager.editorItems.Count; ++i)
        {
            Master.EditorItem index = Master.manager.editorItems[i];
            if (nextLineCount*(buttonSize+4)+buttonSize > Screen.height)
            {
                nextLineCount = 0;
                columns++;
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
            }
            nextLineCount++;
            if (GUILayout.Button(index.guiTexture, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
            {
                worldObject = index.gameObject;
                worldObjectIndex = i;
                eMode = EManipulationMode.PLACE;
                selectedObject = null;
                os.gameObject.SetActive(false);
                os.SetObject(null, -1);
            }
        }
        GUILayout.EndVertical();
        GUIContent c = new GUIContent(enableGrid ? gridButtonTexture : gridButtonTextureInv, "Enable/disable grid snapping");
        if (GUILayout.Button(c, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            enableGrid = !enableGrid;
        }
        
        GUILayout.EndHorizontal();
    }

    private void ChangingScene()
    {
        oldScene = scene;
        SaveStateManager.LoadState();
    }
}
