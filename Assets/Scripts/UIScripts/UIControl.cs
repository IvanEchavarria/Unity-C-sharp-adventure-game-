using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject InstructionsHolder;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                LoadScene(1);
            }
            else
            {
                LoadScene(0);
            }            
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            DeactivateInstructions();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitApplication();
        }
    }


    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void DeactivateInstructions()
    {
        if(InstructionsHolder)
        {
            if (InstructionsHolder.activeSelf)
            {
                InstructionsHolder.SetActive(false);
            }
            else
            {
                InstructionsHolder.SetActive(true);
            }
        }        
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
