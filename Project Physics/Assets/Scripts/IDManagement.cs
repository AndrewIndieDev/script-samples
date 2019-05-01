using UnityEngine;

public class IDManagement : MonoBehaviour
{

    private static int uniqueID = -1;

    public static int GetUniqueID()
    {
        ++uniqueID;
        return uniqueID;
    }

    public static void ResetIDs()
    {
        uniqueID = -1;
    }

}
