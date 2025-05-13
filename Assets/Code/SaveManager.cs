using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

public static class SaveManager
{
    public static void SavePlayerData(GameManager Game)
    {
        PlayerData playerData = new PlayerData(Game);
        string dataPath = Application.persistentDataPath + "/player.save"; //Puede tener la terminacion que quiera, porque es binario
        FileStream fileStream = new FileStream(dataPath, FileMode.Create); //Crea el archivo
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, playerData); //Escribe los datos.
        fileStream.Close(); // Cierra el archivo.

    }

    public static PlayerData LoadPlayerData()
    {
        
        string dataPath = Application.persistentDataPath + "/player.save";

        if (File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return playerData;
        }
        else
        {
            Debug.LogError("No se encontro el archivo de guardado");
            return null;
        }
        
    }

    public static void NewGame()
    {
        PlayerData playerData = new PlayerData();
        string dataPath = Application.persistentDataPath + "/player.save"; //Puede tener la terminacion que quiera, porque es binario
        FileStream fileStream = new FileStream(dataPath, FileMode.Create); //Crea el archivo
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, playerData); //Escribe los datos.
        fileStream.Close(); // Cierra el archivo.
    }

    public static void OpenSavedScene() //Might Have to delete.
    {

    }

    private static IEnumerator Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            
        }
    }


}