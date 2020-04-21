using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerDriverCamera : MonoBehaviour
{
    [SerializeField] private Transform m_CarTransform;

    private float camZ;
    
    // Start is called before the first frame update
    void Start()
    {
        camZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = m_CarTransform.position;
        pos.z = camZ;
        transform.position = pos;
    }
}
