using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float minXClamp = -1.77f;
    public float maxXClamp = 150.0f;

    private void LateUpdate()
    {
        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(player.transform.position.x, minXClamp, maxXClamp);

        transform.position = cameraPos;
    }
}
