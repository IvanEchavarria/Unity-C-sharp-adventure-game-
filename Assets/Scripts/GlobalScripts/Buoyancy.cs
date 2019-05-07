using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    private GameObject waterPlane;
    private Rigidbody  m_RigidBody;
    private BoxCollider m_boxCollider;
    private float waterLevel;
    private Vector3[] BoxVertices;
    private SetRenderQueue DrawingQueueScript;
    private bool playerOnBoat;

    public  Vector3 ForceUp = new Vector3(0, 3.5f, 0);
    public float underWaterOffset = 0.2f; // How much can we go underwater before being pushed up
    public float waterDensity = 0.125f;
    public float GizmoRadius = 0.1f;

    // Start is called before the first frame update
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_boxCollider = GetComponent<BoxCollider>();
        BoxVertices = new Vector3[8];       
        waterPlane = GameObject.FindGameObjectWithTag("Ocean");
        waterLevel = waterPlane.transform.position.y;

        if(GetComponent<SetRenderQueue>())
        {
            DrawingQueueScript = GetComponent<SetRenderQueue>();
        }
    }   

    // Update is called once per frame
    void FixedUpdate()
    {
        GetColliderVertices();

        for( int i = 0; i < BoxVertices.Length; i++) 
        {        
            if (BoxVertices[i].y < waterLevel - underWaterOffset)
            {
                m_RigidBody.drag = 5.0f;
                m_RigidBody.angularDrag = 5.0f;

                m_RigidBody.AddForceAtPosition(ForceUp * m_RigidBody.mass, BoxVertices[i]);
                m_RigidBody.AddForceAtPosition(m_RigidBody.velocity * -1 * waterDensity, BoxVertices[i]);                
            }
            else
            {
                m_RigidBody.drag = 0.05f;
                m_RigidBody.angularDrag = 0.05f;
            }
        }
        
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (DrawingQueueScript)
            {
                DrawingQueueScript.ChangeMaterialQueue(3020);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //ForceUp = new Vector3(0, 3.5f, 0);

            if (DrawingQueueScript)
            {
                DrawingQueueScript.ChangeMaterialQueue(2090);
            }
        }
    }


    void GetColliderVertices()
    {
        //Box Collider Corners transformed into world coordinates
        BoxVertices[0] = transform.TransformPoint(m_boxCollider.center + new Vector3(m_boxCollider.size.x, -m_boxCollider.size.y, m_boxCollider.size.z) * 0.5f);
        BoxVertices[1] = transform.TransformPoint(m_boxCollider.center + new Vector3(-m_boxCollider.size.x, -m_boxCollider.size.y, m_boxCollider.size.z) * 0.5f);
        BoxVertices[2] = transform.TransformPoint(m_boxCollider.center + new Vector3(-m_boxCollider.size.x, -m_boxCollider.size.y, -m_boxCollider.size.z) * 0.5f);
        BoxVertices[3] = transform.TransformPoint(m_boxCollider.center + new Vector3(m_boxCollider.size.x, -m_boxCollider.size.y, -m_boxCollider.size.z) * 0.5f);
        BoxVertices[4] = transform.TransformPoint(m_boxCollider.center + new Vector3(m_boxCollider.size.x, m_boxCollider.size.y, m_boxCollider.size.z) * 0.5f);
        BoxVertices[5] = transform.TransformPoint(m_boxCollider.center + new Vector3(-m_boxCollider.size.x, m_boxCollider.size.y, m_boxCollider.size.z) * 0.5f);
        BoxVertices[6] = transform.TransformPoint(m_boxCollider.center + new Vector3(-m_boxCollider.size.x, m_boxCollider.size.y, -m_boxCollider.size.z) * 0.5f);
        BoxVertices[7] = transform.TransformPoint(m_boxCollider.center + new Vector3(m_boxCollider.size.x, m_boxCollider.size.y, -m_boxCollider.size.z) * 0.5f);
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;

    //    m_boxCollider = GetComponent<BoxCollider>();

    //    Gizmos.DrawSphere(transform.TransformPoint(m_boxCollider.center + new Vector3(m_boxCollider.size.x, -m_boxCollider.size.y, m_boxCollider.size.z) * 0.5f), GizmoRadius);
    //    Gizmos.DrawSphere(transform.TransformPoint(m_boxCollider.center + new Vector3(-m_boxCollider.size.x, -m_boxCollider.size.y, m_boxCollider.size.z) * 0.5f), GizmoRadius);
    //    Gizmos.DrawSphere(transform.TransformPoint(m_boxCollider.center + new Vector3(-m_boxCollider.size.x, -m_boxCollider.size.y, -m_boxCollider.size.z) * 0.5f), GizmoRadius);
    //    Gizmos.DrawSphere(transform.TransformPoint(m_boxCollider.center + new Vector3(m_boxCollider.size.x, -m_boxCollider.size.y, -m_boxCollider.size.z) * 0.5f), GizmoRadius);

    //    Gizmos.DrawSphere(transform.TransformPoint(m_boxCollider.center + new Vector3(m_boxCollider.size.x, m_boxCollider.size.y, m_boxCollider.size.z) * 0.5f), GizmoRadius);
    //    Gizmos.DrawSphere(transform.TransformPoint(m_boxCollider.center + new Vector3(-m_boxCollider.size.x, m_boxCollider.size.y, m_boxCollider.size.z) * 0.5f), GizmoRadius);
    //    Gizmos.DrawSphere(transform.TransformPoint(m_boxCollider.center + new Vector3(-m_boxCollider.size.x, m_boxCollider.size.y, -m_boxCollider.size.z) * 0.5f), GizmoRadius);
    //    Gizmos.DrawSphere(transform.TransformPoint(m_boxCollider.center + new Vector3(m_boxCollider.size.x, m_boxCollider.size.y, -m_boxCollider.size.z) * 0.5f), GizmoRadius);
    //}
}
