using System.Collections.Generic;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public QuestObject[] quests;
    public bool[] questCompleted;

    public DialogueManager DM;

    public List<QuestObject> activeQuests = new List<QuestObject>();


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        questCompleted = new bool[quests.Length];
    }

    public void AddActiveQuest(QuestObject quest)
    {
        if (!activeQuests.Contains(quest))
        {
            activeQuests.Add(quest);
            UIManager.Instance.UpdateQuestList(activeQuests);
        }
    }

    public void CompleteQuest(QuestObject quest)
    {
        activeQuests.Remove(quest);
        UIManager.Instance.UpdateQuestList(activeQuests);
        questCompleted[quest.questNum] = true;
    }

    public void ShowQuestText(string questText)
    {
        DM.dialogLines = new string[1] { questText };
        DM.currentLine = 0;
        DM.ShowDialogue();
    }
}
