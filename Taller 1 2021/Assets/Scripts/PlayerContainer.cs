using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public Transform player;
    Vector3 originalOffset;

    private void Start()
    {
        originalOffset = player.localPosition;
    }

    private void Update()
    {
        
    }
}
