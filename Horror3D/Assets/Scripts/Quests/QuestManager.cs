using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private List<Quest> quests = new List<Quest>();

    private int currentQuestIndex = 0;

    public TextMeshProUGUI tmpText;

    public float typingSpeed = 0.05f;

    private bool questRoutineStarted { get; set; } = false;

    public void StartQuestRoutine()
    {
        if (quests.Count > 0 && questRoutineStarted == false)
        {
            RegisterQuest(quests[0]);
            quests[0].Activate();
            StartCoroutine(TypeText(quests[0].text));
            questRoutineStarted = true;
        }
    }

    private void RegisterQuest(Quest quest)
    {
        quest.OnQuestCompleted += OnQuestCompleted;
    }

    public void RetypeQuestText()
    {
        StartCoroutine(TypeText(quests[currentQuestIndex].text));
    }

    private void OnQuestCompleted(Quest completedQuest)
    {
        currentQuestIndex++;

        if (currentQuestIndex < quests.Count)
        {
            Quest nextQuest = quests[currentQuestIndex];
            RegisterQuest(nextQuest);
            nextQuest.Activate();
            StartCoroutine(TypeText(nextQuest.text));
            Debug.Log($"Activated next quest: {nextQuest.gameObject.name}");
        }
        else
        {
            Debug.Log("All quests completed!");
        }
    }
    private IEnumerator TypeText(string fullText)
    {
        tmpText.text = "";
        foreach (char c in fullText)
        {
            tmpText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public bool GetStartedQuestRoutine()
    {
        return questRoutineStarted;
    }
}
