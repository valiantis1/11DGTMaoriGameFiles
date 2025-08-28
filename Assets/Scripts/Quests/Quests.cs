using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quests : MonoBehaviour
{
    // this works by each quest might have a gate to open
    // eg. after doing quest[0], gate[0] might open.
    private List<string> quests = new List<string> 
    { 
        "Defeat 1 enemy.",
        "Talk to Rawiri.",
        "Defeat 1 enemy.",
        "Defeat enemies.\n0/3",
        "Defeat enemies.\n1/3",
        "Defeat enemies.\n2/3",
        "Continue on the path ",
        "Find the one who looks to the moana.",
        "Complete Tangaroa's puzzles.",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        ""
    };
    [SerializeField] private List<GameObject> gates = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float waitTime;
    private int _questOn = -1;
    private bool _gameStarted;

    private int _enemysDefeated;
    private string _lastNpcTalkedTo;

    // Update is called once per frame
    void Update()
    {
        if(!_gameStarted) { return; }
        int oldQuest = _questOn;
        if (_questOn == -1) { _questOn++; }
        else if (_questOn == 0) { if(_enemysDefeated > 0) { _questOn++; _enemysDefeated = 0; } }

        else if (_questOn == 1) { if (_lastNpcTalkedTo == "Rawiri") { _questOn++; } }

        else if (_questOn == 2) { if (_enemysDefeated > 0) { _questOn++; _enemysDefeated = 0; } }

        else if (_questOn == 3) { if (_enemysDefeated > 0) { _questOn++; _enemysDefeated = 0; } }
        else if (_questOn == 4) { if (_enemysDefeated > 0) { _questOn++; _enemysDefeated = 0; } }
        else if (_questOn == 5) { if (_enemysDefeated > 0) { _questOn++; _enemysDefeated = 0; } }

        else if (_questOn == 6) { if (_lastNpcTalkedTo == "Tāne") { _questOn++; } }

        else if (_questOn == 7) { if (_lastNpcTalkedTo == "Wiremu") { _questOn++; } }

        //else if (_questOn == 6) { if (_lastNpcTalkedTo == "Wiremu") { _questOn++; } }

        if (oldQuest != _questOn)
        {
            StartCoroutine(TypeQuest(quests[_questOn]));
        }
    }

    private IEnumerator TypeQuest(string quest)
    {
        quest = "Quest:\n" + quest;

        for (int i = text.text.Length - 6; i >= 0; i--)
        {
            text.maxVisibleCharacters = i + 6;
            yield return new WaitForSeconds(waitTime);
        }

        text.maxVisibleCharacters = 0;
        text.text = quest;
        for (int i = 0; i <= quest.Length - 6; i++)
        {
            text.maxVisibleCharacters = i + 6;
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void EnemysDefeated()
    {
        _enemysDefeated++;
    }
    public void TalkedWithNpc(string npcName)
    {
        _gameStarted = true;
        _lastNpcTalkedTo = npcName;
    }
}
