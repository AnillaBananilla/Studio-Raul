using System;
using TMPro;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    public string questName;
    public string startText;
    public string completeText;
    public int questNum;
    public bool isAccepted;

    private DialogueManager DM;
    private QuestManager QM;

    void Start()
    {
        DM = FindFirstObjectByType<DialogueManager>();
        QM = FindFirstObjectByType<QuestManager>();
    }

    public void OfferQuest()
    {
        QM.ShowQuestText(startText);
        UIManager.Instance.ShowQuestOffer(startText, AcceptQuest, DeclineQuest);
    }

    private void AcceptQuest()
    {
        isAccepted = true;
        QM.AddActiveQuest(this);
        UIManager.Instance.ShowQuestNotification("New Quest: " + questName);
    }

    public void DeclineQuest()
    {
        QM.ShowQuestText("Maybe next time...");
    }
    public void EndQuest()
    {

        QM.ShowQuestText(completeText);

        //update the list of completed quests
        QM.CompleteQuest(this);
        QM.questCompleted[questNum] = true;
        UIManager.Instance.UpdateQuestList(QM.activeQuests);
        this.enabled = false;
    }
}
