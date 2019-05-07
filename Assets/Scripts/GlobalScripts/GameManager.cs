using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text InteractionText;
    public GameObject DialogueBox;
    public Text DialogueText;

    public Text questText;
    public Text tipsText;

    private string[] TIPS;
    private string[] QUESTS;

    private int QuestNumber = 0;

    private bool AllowToUseBoat = false;
    public GameObject BoatPaddles;

    private AudioSource ManagerAudioSource;
    public AudioClip[]  clips;

    // Start is called before the first frame update
    void Start()
    {
        TIPS = new string[10];
        QUESTS = new string[10];
        InitiateTextArrays();
        questText.text = QUESTS[0];        
        ManagerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

   

    private void InitiateTextArrays()
    {
        QUESTS[0] = "Go to the nearest town";
        QUESTS[1] = "Talk to the pirates for help";
        QUESTS[2] = "Find Boat paddles";
        QUESTS[3] = "Paddles found, use the boat";
        QUESTS[4] = "Find the Treasure to win";
        QUESTS[5] = "Congratulations";


        TIPS[0] = "Swimming for too long will attract sharks!";
        TIPS[1] = "Talk to the pirates in town";
        TIPS[3] = "Go to the big town using the boat";
        TIPS[4] = "";
    }

  
    public bool IsAllowToUseBoat()
    {
        return AllowToUseBoat;
    }

    public ref Text GetInteractionText()
    {
        return ref InteractionText;
    }

    public void ToggleDialogueBox(bool Toggle)
    {
        DialogueBox.SetActive(Toggle);
    }
     
    public void SetDialogueText(string dialogue)
    {
        DialogueText.text = dialogue;
    }

    public void SetQuestText(int QuestNumber)
    {
        this.QuestNumber = QuestNumber;
        ActivateQuestObjects(QuestNumber);
        questText.text = QUESTS[QuestNumber];
    }

    public void SetTipsText(int index)
    {
        tipsText.text = TIPS[index];
    }


    private void ActivateQuestObjects(int questNumber)
    {
        switch (questNumber)
        {
            case 1:
                SetTipsText(questNumber);
                break;
            case 2:
                BoatPaddles.SetActive(true);
                break;
            case 3:
                AllowToUseBoat = true;
                GameObject.Find("FirstCityBoat").GetComponent<Rigidbody>().isKinematic = false;
                SetTipsText(questNumber);
                break;
            case 4:
                SetTipsText(questNumber);
                break;
            case 5:
                //EndGame
                SceneManager.LoadScene(3);
                break;

        }
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
