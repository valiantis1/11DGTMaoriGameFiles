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
        "Defeat 1 enemy.", //0
        "Talk to Rawiri.",
        "Defeat 1 enemy.",
        "Defeat enemies.\n0/3",
        "Defeat enemies.\n1/3",
        "Defeat enemies.\n2/3", //5
        "Continue on the path.",
        "Find the one who looks to the moana.",
        "Complete Tangaroa's puzzles.",
        "Go to the third island.",
        "Follow the path.", //10
        "Defeat enemies.\n0/2",
        "Defeat enemies.\n1/2",
        "Talk to Mikaere.",
        "Prepare to fight.",
        "Start the Fight.", //15
        "Defeat enemies.\n0/2", 
        "Defeat enemies.\n1/2",
        "Defeat enemies.\n0/4",
        "Defeat enemies.\n1/4",
        "Defeat enemies.\n2/4", //20
        "Defeat enemies.\n3/4", 
        "Defeat enemies.\n0/2",
        "Defeat enemies.\n1/2",
        "Defeat enemies.\n0/3",
        "Defeat enemies.\n1/3", //25
        "Defeat enemies.\n2/3", 
        "Defeat enemies.\n0/3",
        "Defeat enemies.\n1/3",
        "Defeat enemies.\n2/3", 
        "Done" //30
    };
    [SerializeField] private List<GameObject> gates = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float waitTime;
    public int _questOn = -1;
    private bool _gameStarted;
    private bool _newQuest;

    private int _enemysDefeated;
    private string _lastNpcTalkedTo;
    private string _lastTrigger;

    // Update is called once per frame
    void Update()
    {
        if(!_gameStarted) { return; }
        int oldQuest = _questOn;
        if (_questOn == -1) { _questOn++; }
        else if (_questOn == 0) { if(_enemysDefeated > 0) { _questOn++; } }

        else if (_questOn == 1) { if (_lastNpcTalkedTo == "Rawiri") { _questOn++; } }

        else if (_questOn == 2) { if (_enemysDefeated > 0) { _questOn++; } }

        else if (_questOn == 3) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 4) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 5) { if (_enemysDefeated > 0) { _questOn++; } }

        else if (_questOn == 6) { if (_lastNpcTalkedTo == "Tāne") { _questOn++; } }

        else if (_questOn == 7) { if (_lastNpcTalkedTo == "Wiremu") { _questOn++; } }

        else if (_questOn == 8) { if (_lastNpcTalkedTo == "Tangaroa") { _questOn++; } }

        else if (_questOn == 9) { if (_lastTrigger == "Island3") { _questOn++; } }

        else if (_questOn == 10) { if (_lastTrigger == "Island3Path") { _questOn++; } }

        else if (_questOn == 11) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 12) { if (_enemysDefeated > 0) { _questOn++; } }

        else if (_questOn == 13) { if (_lastNpcTalkedTo == "Mikaere") { _questOn++; } }

        else if (_questOn == 14) { if (_lastTrigger == "BossFight") { _questOn++; } }

        else if (_questOn == 15) { if (_lastTrigger == "BossFightStart") { _questOn++; } }

        else if (_questOn == 16) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 17) { if (_enemysDefeated > 0) { _questOn++; } }

        else if (_questOn == 18) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 19) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 20) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 21) { if (_enemysDefeated > 0) { _questOn++; } }

        else if (_questOn == 22) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 23) { if (_enemysDefeated > 0) { _questOn++; } }

        else if (_questOn == 24) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 25) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 26) { if (_enemysDefeated > 0) { _questOn++; } }

        else if (_questOn == 27) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 28) { if (_enemysDefeated > 0) { _questOn++; } }
        else if (_questOn == 29) { if (_enemysDefeated > 0) { _questOn++; } }


        if (oldQuest != _questOn)
        {
            _enemysDefeated = 0;
            _lastNpcTalkedTo = "";
            _newQuest = true;
            StartCoroutine(TypeQuest(quests[_questOn]));
            try
            {
                //gates[oldQuest].gameObject.SetActive(false);
                Transform[] Gates_ = gates[oldQuest].GetComponentsInChildren<Transform>();
                for (int i = 0; i < Gates_.Length; i++)
                {
                    if (Gates_[i].CompareTag("Gate")) //sees if it is actually a gate
                    {
                        Gates_[i].gameObject.SetActive(false);
                    }
                }
            }
            catch { }
        }
        if(_lastTrigger == "death")
        {
            _lastTrigger = null;
            _questOn = 14;
        }
    }

    private IEnumerator TypeQuest(string quest)
    {
        if(_newQuest)
            _newQuest = false;

        quest = "Quest:\n" + quest;

        for (int i = text.text.Length - 6; i >= 0; i--)
        {
            if(_newQuest) { yield break; } //if there is a new quest coming in it will stop.
            text.maxVisibleCharacters = i + 6;
            yield return new WaitForSeconds(waitTime);
        }

        text.maxVisibleCharacters = 0;
        text.text = quest;
        for (int i = 0; i <= quest.Length - 6; i++)
        {
            if (_newQuest) { yield break; } //if there is a new quest coming in it will stop.
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
    public void Trigger(string whatPoint) //a trigger will get set of then tell this script. Triggers can be the quest trigger script or just other scripts.
    {
        _lastTrigger = whatPoint;
    }
}
