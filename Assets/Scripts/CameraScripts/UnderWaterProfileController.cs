using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class UnderWaterProfileController : MonoBehaviour
{

    public PostProcessingProfile normalProfile, underWaterProfile;

    private PostProcessingBehaviour postProcessBehaviourScript;

    private bool m_IsUnderWater;

    // Start is called before the first frame update
    void Start()
    {
        postProcessBehaviourScript = FindObjectOfType<PostProcessingBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ocean")
        {
            postProcessBehaviourScript.profile = underWaterProfile;
            m_IsUnderWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ocean")
        {
            postProcessBehaviourScript.profile = normalProfile;
            m_IsUnderWater = false;
        }
    }

    public bool IsUnderWater()
    {
        return m_IsUnderWater;
    }

    public void JumpOnBoat()
    {
        postProcessBehaviourScript.profile = normalProfile;
        m_IsUnderWater = false;
    }

}
