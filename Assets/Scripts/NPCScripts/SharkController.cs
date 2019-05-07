using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : MonoBehaviour
{
    Vector3 StartPosition;
    Vector3 MoveToPosition;
   
    public float minDistance = 2.2f;
    public float movementSpeed = 13.0f;
    public float AttackTimeOut = 1.5f;

    private GameObject PlayerObj;
    private PlayerController playerControlScript;
    public GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        playerControlScript = PlayerObj.GetComponent<PlayerController>();
        StartPosition = transform.position;        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerControlScript.IsOnWater())
        {
            transform.LookAt(PlayerObj.transform);
            MoveToPosition = PlayerObj.transform.position;           
        }
        else
        {           
            transform.LookAt(StartPosition);
            MoveToPosition = StartPosition;
        }

         if (Vector3.Distance(transform.position, MoveToPosition) > minDistance)
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject blood =  Instantiate(bloodEffect, PlayerObj.transform);
            Destroy(blood, 1.5f);
            playerControlScript.SharkAttkDamage();
            movementSpeed *= -1;
            Invoke("SharkAttack", AttackTimeOut);
        }
    }


    void SharkAttack()
    {
        movementSpeed *= -1;
    }
    
}
