using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveLoader", menuName = "SaveLoader/Loader")]
public class SaveScriptableObject : ScriptableObject
{
    

    public void LoadData(PlayerData Data)
    {
        PlayerData LoadedData = SaveManager.LoadPlayerData();
        

    }
    
}
