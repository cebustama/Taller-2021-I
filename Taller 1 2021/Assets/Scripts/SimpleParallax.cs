using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    public Transform player;
    public float parallaxStrength = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(player.localPosition.x * parallaxStrength, 0, 0);
    }
}
