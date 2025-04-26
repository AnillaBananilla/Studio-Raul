using System.Collections.ObjectModel;
using UnityEngine;

public class CoinsQuest : MonoBehaviour
{
    private QuestManager QM;
    QuestObject quest;
    public GameManager gameManagerX;

    public int questNum;

    void Start()
    {
        QM = FindFirstObjectByType<QuestManager>();
       
        quest = QM.quests[questNum];
    }

    void Update()
    {
        CollectedCoins();
    }

    void CollectedCoins()
    {
        Debug.Log("Coins: " + gameManagerX.score);
        if (gameManagerX.score >= 2 && quest.isAccepted && !QM.questCompleted[questNum])
        {
 
            quest.EndQuest();
        }
    }
}
