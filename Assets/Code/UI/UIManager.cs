using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private QuestManager QM;

    [Header("UI Elements for Quests")]
    public GameObject questOfferUI;
    public TMPro.TextMeshProUGUI questOfferText;
    public Button acceptButton;
    public Button declineButton;

    [Header("Quest List UI")]
    public GameObject questListUI;
    public GameObject questItemPrefab;

    [Header("Notifications")]
    public GameObject questNotification;
    public TMPro.TextMeshProUGUI questNotificationText;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        QM = FindFirstObjectByType<QuestManager>();
        questOfferUI.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = questListUI.activeSelf;
            questListUI.SetActive(isActive);

            if (isActive)
            {
                UpdateQuestList(QM.activeQuests);
            }
        }
    }

    public void ShowQuestOffer(string text, System.Action onAccept, System.Action onDecline)
    {
        questOfferUI.SetActive(true);
        questOfferText.text = text;

        acceptButton.gameObject.SetActive(true);
        declineButton.gameObject.SetActive(true);



        acceptButton.onClick.AddListener(() => {

            questOfferUI.SetActive(false);
            acceptButton.gameObject.SetActive(false);
            declineButton.gameObject.SetActive(false);
            onAccept.Invoke();
        });

        declineButton.onClick.AddListener(() => {
            questOfferUI.SetActive(false);
            acceptButton.gameObject.SetActive(false);
            declineButton.gameObject.SetActive(false);
            onDecline.Invoke();
        });
    }

    public void UpdateQuestList(List<QuestObject> activeQuests)
    {
        foreach (Transform child in questListUI.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (QuestObject quest in activeQuests)
        {
            GameObject questItem = Instantiate(questItemPrefab, questListUI.transform);
            questItem.GetComponent<Text>().text = quest.questName;
        }
    }

    public void ShowQuestNotification(string text)
    {
        questNotification.SetActive(true);
        questNotificationText.text = text;
        StartCoroutine(HideNotification());
    }

    private IEnumerator HideNotification()
    {
        yield return new WaitForSeconds(3f);
        questNotification.SetActive(false);
    }


}
