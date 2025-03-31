[System.Serializable]

public class PlayerData
{
    public float[] position = new float[3];
    public int SceneIndex;
    public int Money;

    public PlayerData(PlayerEntity Player)
    {
        position[0] = Player.transform.position.x;
        position[1] = Player.transform.position.y;
        position[2] = 0;
        SceneIndex = Player.GetScene();
        Money = Player.Money;
    }
}

