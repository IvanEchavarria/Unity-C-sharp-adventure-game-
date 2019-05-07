using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{

    private GameManager GameManagerScript;
    public int QuestIndex;
    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManagerScript.SetQuestText(QuestIndex);
            Destroy(this.gameObject, 0.5f);
        }
    }
}
