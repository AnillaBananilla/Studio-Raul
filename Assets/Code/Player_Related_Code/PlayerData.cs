
[System.Serializable]

public class PlayerData
{
    public float[] position = new float[3];
    public int SceneIndex;  // I might not need this one for now
    public int Money;
    public int HP;
    public bool[] Progress = new bool[3]; //Hay 3 misiones
    public int[] ItemAmounts = new int[9];
    public bool[] Skills = new bool[1]; //Index is to be defined later

    /* WE NEED
     * HP: Found in the Player's Healt
     * Transform: Found in any of Player's scripts
     * Money: Should Be Found in the Game Manager
     * Progress: I need to find it, or it is yet to be implemented
     * Inventory Items: Found at the PlayerInventory class
     * Skills: Found in the Player Skills SO
     */
    public PlayerData(GameManager Game)
    {
        // Save the Player's position
        position[0] = Game.PlayerHP.gameObject.transform.position.x;
        position[1] = Game.PlayerHP.gameObject.transform.position.y;
        position[2] = -1.2563f;
        SceneIndex = 1;

        //Save the Player's Health and money
        HP = Game.PlayerHP.currentHealt;
        Money = Game.score;

        //Save the Player's skills
        Skills[0] = Game.SkillList.skills[0].isUnlocked;

        //Save the Item Amounts
        int i = 0;
        foreach(PlayerInventory.InventoryItem item in Game.Inventory.items)
        {
            ItemAmounts[i] = Game.Inventory.items[i].quantity;
            i++;
        }
        



        //, int money, bool[] progress, int[] items, bool[] skills

    }
}

