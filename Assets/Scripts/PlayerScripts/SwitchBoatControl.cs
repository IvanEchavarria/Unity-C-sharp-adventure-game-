using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchBoatControl : MonoBehaviour
{
    [SerializeField]
    GameObject Boat;

    public Text InteractionText;
    //Player Components
    public GameObject       m_PlayerCamera;
    public PlayerController m_PlayerControlScript; 

    //Boat information
    private Transform       m_BoatCamera;    
    private BoatController  m_BoatControlScript;
    private Transform       m_PlayerBoatPosition;

    private bool            m_OnBoat;

    //Manager Script
    private GameManager GameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (m_OnBoat && Boat)
        {
            transform.position = m_PlayerBoatPosition.position; // Move Player to boat position
            transform.forward = m_PlayerBoatPosition.forward;
        }           

        if (!m_OnBoat && Boat && Input.GetKeyDown(KeyCode.E))
        {            
            if(Boat)
            {
                m_OnBoat = true;
                m_PlayerBoatPosition = Boat.transform.Find("PlayerPositionToSpawn");
                m_BoatCamera = Boat.transform.Find("Camera");
                m_BoatControlScript = Boat.GetComponent<BoatController>();
                m_PlayerCamera.SetActive(false);                    // Deactivate Player Camera
                m_BoatCamera.gameObject.SetActive(true);           // Activate boat Camera

                m_PlayerControlScript.GetAnimationControlScript().AnimationStateChange(CustomTypes.AnimationStateController.DRIVING);
                m_PlayerControlScript.EnableDisableAudioSource(false); //Deactivate Audio Source
                m_PlayerControlScript.JumpOnBoat();
                m_PlayerControlScript.enabled = false;             // Deactivate player control
                m_BoatControlScript.enabled = true;               // Activate boat control    
                m_BoatControlScript.FreezeBoatRotation();        //  Freeze the boar Rigidbody Rotation
            }
        }
        else if(m_OnBoat && Input.GetKeyDown(KeyCode.E))
        {           
            m_PlayerCamera.SetActive(true);                    //activate Player Camera
            m_BoatCamera.gameObject.SetActive(false);
            m_PlayerControlScript.enabled = true;             // activate player control
            m_PlayerControlScript.EnableDisableAudioSource(true); //activate Audio Source
            m_PlayerControlScript.GetAnimationControlScript().AnimationStateChange(CustomTypes.AnimationStateController.WALK);
            m_BoatControlScript.UnfreezeBoatRotation();       // Unfreeze Rigidboady rotation before disabling script       
            m_BoatControlScript.enabled = false;
            m_OnBoat = false; 

        }       
    }

    

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ship")
        {
            if (!GameManagerScript.IsAllowToUseBoat())
            {
                InteractionText.text = "";
                return;
            }

            if (!m_OnBoat)
            {
                InteractionText.text = "";
                Boat = null;
                m_PlayerBoatPosition = null;
                m_BoatCamera = null;
                m_BoatControlScript = null;
            }            
        }
    }


    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Ship")
        {
            if (!GameManagerScript.IsAllowToUseBoat())
            {
                InteractionText.text = "No Paddles";
                return;
            }           

            if (!m_OnBoat)
            {
                InteractionText.text = "Drive (E)";
            }
            if (!Boat)
            {
                Boat = other.gameObject;               
            }          

            if (!m_OnBoat)
            {
                InteractionText.text = "Drive (E)";
            }
            else if (m_OnBoat)
            {
                InteractionText.text = "Release (E)";
            }
        }
    }
}
