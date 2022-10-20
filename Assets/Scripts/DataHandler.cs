using UnityEngine;

public static class DataHandler
{
    public static void LoadGameData()
    {
        Debug.Log("Loading Buildings");
        Globals.BUILDING_DATA = Resources.LoadAll<BuildingData>("ScriptableObjects/Buildings") as BuildingData[];
        Debug.Log(Globals.BUILDING_DATA.Length);
    }
}