using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlocks : MonoBehaviour {

    public enum EBuildingBlockCategory
    {
        NONE,
        Blocks,
        Flat,
        Logic,
        Weapons
    }

    [System.Serializable]
    public class Block
    {
        public GameObject gameObject;
        public EBuildingBlockCategory category;
        public string name;
        public string description;

        public Block(GameObject gameObject = null, EBuildingBlockCategory category = EBuildingBlockCategory.NONE, string name = "NO NAME", string description = "NO DESCRIPTION")
        {
            this.gameObject = gameObject;
            this.category = category;
            this.name = name;
            this.description = description;
        }
    }

    public List<Block> blocks = new List<Block>();
    public List<Material> presetMaterials = new List<Material>();

    public static BuildingBlocks manager;

    void Start()
    {
        manager = this;
    }

}
