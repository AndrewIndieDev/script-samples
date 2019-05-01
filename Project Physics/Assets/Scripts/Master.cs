using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum EBuildType //Enum for all buildable objects
{
    MOVE,
    ROTATE,
    BALLCLOCKWISE,
    BALLCOUNTERCLOCKWISE,
    THRUSTER,
    PROPELLER,
    SPIKEBALLCLOCKWISE,
    SPIKEBALLCOUNTERCLOCKWISE,
    HOVERBALL,
    DYNAMITE,
    SOLIDLINE,
    LITELINE,
    GLUEBALL,
    GRAPPLINGHOOK,  

    CONNECTIONPOINT //ALWAYS LAST
}

public class Master : MonoBehaviour
{
    [System.Serializable]
    public class UserItem
    {
        public bool connectable;
        public GameObject buildTypePrefab;
        public Texture2D guiVisualPrefab;

        public UserItem()
        {
            connectable = false;
            buildTypePrefab = null;
            guiVisualPrefab = null;
        }
    }

    [System.Serializable]
    public class EditorItem
    {
        public string name;
        public string description;
        public GameObject gameObject;
        public Texture2D guiTexture;
        public bool chainedScaling;

        public EditorItem()
        {
            name = "Missing Name";
            description = "Missing Description";
            gameObject = null;
            guiTexture = null;
            chainedScaling = false;
        }
    }

    public string scene = "";
    public List<UserItem> userItems = new List<UserItem>();
    public List<EditorItem> editorItems = new List<EditorItem>();
    public static Master manager;

    private void Start()
    {
        manager = this;
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }
}