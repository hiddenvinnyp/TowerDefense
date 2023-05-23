using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector3 m_Position;

    void Update()
    {
        m_Position = Input.mousePosition;
        m_Position.z = 1f;// Camera.main.transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(m_Position);
    }
}
