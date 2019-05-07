using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour {

    private GameObject waterPlane;
    private Rigidbody m_RigidBody;
    private BoxCollider m_boxCollider;
    private Vector3[] BoxVertices;
    private float waterLevel;
	public float  floatHeight = 1.13f;
	public Vector3 buoyancyCentreOffset = new Vector3(0,1,0);
	public float bounceDamp = 0.05f;

    private void Awake()
    {
        waterPlane = GameObject.FindGameObjectWithTag("Ocean");
        waterLevel = waterPlane.transform.position.y;
        m_RigidBody = GetComponent<Rigidbody>();
        m_boxCollider = GetComponent<BoxCollider>();
        BoxVertices = new Vector3[8];
    }


    void FixedUpdate () {

        GetColliderVertices();

        for (int i = 0; i < BoxVertices.Length; i++)
        {

            Vector3 actionPoint = BoxVertices[i] + transform.TransformDirection(buoyancyCentreOffset);
            float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

            if (forceFactor > 0f)
            {
                Vector3 uplift = -Physics.gravity * (forceFactor - m_RigidBody.velocity.y * bounceDamp);
                m_RigidBody.AddForceAtPosition(uplift * m_RigidBody.mass, actionPoint);
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
}
