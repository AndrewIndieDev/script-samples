using UnityEngine;
using System.Collections;

public enum PlantType
{
    NONE,
    CARROT,
    POTATO,
    TREE
}

public class Plant : MonoBehaviour
{
    public PlantType plantType;

    public Plant(PlantType plantType = PlantType.NONE)
    {
        this.plantType = plantType;
    }

	
}
