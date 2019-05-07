using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : MonoBehaviour
{
    private Text InteractionText;
    private CustomTypes.AnimationStateController currentState;
    private AnimationController animController;
    private Quaternion StartingRotation;
    public float m_rotationSpeed = 2.0f;
    private bool m_talkingToPlayer = false;
    private bool m_SpokenToOnce = false;
    private GameManager GameManagerScript;
    private GameObject PlayerObj;

    public Transform DialogueCameraPosition;

    private int m_numberOfDialogues;
    private int m_dialogueCounter = 0;

    public int tipIndex = -1;
    public int initQuest = -1;
    public string[] NPCDialogue;
    
    

    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        InteractionText = GameManagerScript.GetInteractionText();
        animController = GetComponent<AnimationController>();
        currentState = CustomTypes.AnimationStateController.IDLE;
        StartingRotation = transform.rotation;
        m_numberOfDialogues = NPCDialogue.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerObj && Input.GetKeyDown(KeyCode.E) && !m_talkingToPlayer)
        {
            Debug.Log("E PRESSED");

            if(PlayerObj)
            {
                m_talkingToPlayer = true;

                PlayerObj.GetComponent<Rigidbody>().Sleep();
                Transform PlayerCamera = PlayerObj.GetComponent<PlayerController>().GetPlayerCameraTransform();
                PlayerCamera.position = DialogueCameraPosition.position;
                PlayerCamera.rotation = DialogueCameraPosition.rotation;
                PlayerObj.transform.LookAt(this.transform);
                GameManagerScript.ToggleDialogueBox(true);

                PlayerController playerScript = PlayerObj.GetComponent<PlayerController>();
                playerScript.UpdateAnimation(CustomTypes.AnimationStateController.IDLE);
                playerScript.EnableDisableAudioSource(false);
                playerScript.enabled = false;
               
                UpdateAnimation(CustomTypes.AnimationStateController.TALK);
                GameManagerScript.SetDialogueText(NPCDialogue[m_dialogueCounter]);
                m_dialogueCounter++;
            }          
        }
        else if (Input.GetKeyDown(KeyCode.E) && m_talkingToPlayer)
        {
            Debug.Log("INSIDE DIALOGUE COUNTER");

            //Continue talking to NPC while there is dialogues
            if (m_dialogueCounter < m_numberOfDialogues)
            {
                GameManagerScript.SetDialogueText(NPCDialogue[m_dialogueCounter]);
                m_dialogueCounter++;               
            }
            else //If no more dialogues then release the player
            {
                if(PlayerObj)
                {
                    PlayerObj.GetComponent<Rigidbody>().WakeUp();
                    GameManagerScript.SetDialogueText("");
                    GameManagerScript.ToggleDialogueBox(false);
                    m_dialogueCounter = 0;
                    m_talkingToPlayer = false;

                    PlayerController playerScript = PlayerObj.GetComponent<PlayerController>();
                    playerScript.enabled = true;                    
                    playerScript.EnableDisableAudioSource(true);
                    PlayerObj.GetComponent<PlayerController>().ReturnPlayerCameraToPosition();
                    
                    UpdateAnimation(CustomTypes.AnimationStateController.IDLE);

                    if(tipIndex != -1 && !m_SpokenToOnce)
                    {
                        m_SpokenToOnce = true;
                        GameManagerScript.SetTipsText(tipIndex);                       
                    }
                    else if(initQuest != -1 && !m_SpokenToOnce)
                    {
                        GameManagerScript.SetQuestText(initQuest);
                    }
                }                
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InteractionText.text = "";
            PlayerObj = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {           
            InteractionText.text = "Talk (E)";

            transform.LookAt(other.transform);
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f));
            PlayerObj = other.gameObject;
        }
    }


    private void UpdateAnimation(CustomTypes.AnimationStateController animationState)
    {
        currentState = animationState;            
        animController.AnimationStateChange(currentState);       
    }

}
