[System.Serializable]

public class PlayerData
{
    public float[] position = new float[3];

    public PlayerData(PlayerEntity Player)
    {
        position[0] = Player.transform.position.x;
        position[1] = Player.transform.position.y;
        position[2] = 0;
    }
}

