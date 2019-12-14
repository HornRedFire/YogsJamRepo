using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;     //the offset distance between the player and the camera


    // Start is called before the first frame update
    void Start()
    {
        //calculate the value by getting the distance between the player position and camera position
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
