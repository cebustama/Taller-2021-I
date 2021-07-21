using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class OnDestroyOrDeactivate : MonoBehaviour
{
    public GameObject boss;
    public int nextSceneIndex = 1;

    // Update is called once per frame
    void Update()
    {
        if (boss == null || !boss.activeSelf)
        {
            SceneManager.LoadScene(nextSceneIndex);
        } 
    }
}
