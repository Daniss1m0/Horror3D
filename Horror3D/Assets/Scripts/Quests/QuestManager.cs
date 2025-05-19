using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private List<Quest> quests = new List<Quest>();

    private int currentQuestIndex = 0;

    public TextMeshProUGUI tmpText;

    void Start()
    {
        if (quests.Count > 0)
        {
            RegisterQuest(quests[0]);
            quests[0].Activate();
            tmpText.text = $"{quests[0].text}";
        }
    }

    private void RegisterQuest(Quest quest)
    {
        quest.OnQuestCompleted += OnQuestCompleted;
    }

    private void OnQuestCompleted(Quest completedQuest)
    {
        currentQuestIndex++;

        if (currentQuestIndex < quests.Count)
        {
            Quest nextQuest = quests[currentQuestIndex];
            RegisterQuest(nextQuest);
            nextQuest.Activate();
            tmpText.text = $"{nextQuest.text}";
            Debug.Log($"Activated next quest: {nextQuest.gameObject.name}");
        }
        else
        {
            Debug.Log("All quests completed!");
        }
    }
}
