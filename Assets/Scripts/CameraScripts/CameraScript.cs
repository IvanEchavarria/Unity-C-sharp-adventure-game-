using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject Player;
    private Vector3 DistancetoPlayer;
    private float RotationSpeed = 4.0f;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        DistancetoPlayer = Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Position camera on the set up distance created at the start of the game
        transform.position = Player.transform.position - DistancetoPlayer;

        transform.LookAt(new Vector3(Player.transform.position.x, Player.transform.position.y + 20.0f, Player.transform.position.z));

        Quaternion rotation = Quaternion.LookRotation((Player.transform.forward));
        transform.rotation = rotation;

    }
}