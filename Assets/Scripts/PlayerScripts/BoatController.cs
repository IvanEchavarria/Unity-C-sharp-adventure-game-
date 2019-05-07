using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float m_turnSpeed = 25.0f;
    public float m_acceleration = 500.0f;

    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        //
    }

    // Update is called once per frame
    void FixedUpdate()
    {       
        float rotationSpeed = Input.GetAxis("Horizontal");
        float forwardSpeed = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * rotationSpeed * m_turnSpeed * Time.fixedDeltaTime);              

        m_rigidbody.AddForce(transform.forward * forwardSpeed * m_acceleration * Time.fixedDeltaTime);        
    }

    public void FreezeBoatRotation()
    {
        transform.eulerAngles = Vector3.zero;
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void UnfreezeBoatRotation()
    {
        m_rigidbody.constraints = RigidbodyConstraints.None;
    }
 
}
