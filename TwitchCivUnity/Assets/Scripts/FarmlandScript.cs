using UnityEngine;
using System.Collections;

public class FarmlandScript : MonoBehaviour {

    Plant[,] crops = new Plant[3,3];
    public GameObject tree;

    void Start()
    {
        for (int x = 0; x < crops.GetLength(0); x++)
        {
            for (int y = 0; y < crops.GetLength(1); y++)
            {
                crops[x, y] = null;
            }
        }

        for (int i = 0; i < 9; i++)
        {
            AddPlant(Instantiate(tree).GetComponent<Plant>());
        }
    }

    public bool AddPlant(Plant plant)
    {
        for (int x = 0; x < crops.GetLength(0); x++)
        {
            for (int y = 0; y < crops.GetLength(1); y++)
            {
                if (crops[x, y] == null)
                {
                    crops[x, y] = plant;
                    plant.transform.parent = this.transform;
                    plant.transform.localPosition = new Vector3(-2 + x*2, 0, -2 + y*2);
                    plant.transform.rotation = Quaternion.identity * Quaternion.Euler(0, Random.value * 360, 0);
                    return true;
                }
            }
        }
        return false;
    }

}
