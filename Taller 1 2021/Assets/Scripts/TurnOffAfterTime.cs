using UnityEngine;

public class TurnOffAfterTime : MonoBehaviour
{
    public float turnOffTimeAmount = 1f;
    private float turnOffTimer;

    private void OnEnable()
    {
        turnOffTimer = turnOffTimeAmount;
    }

    private void Update()
    {
        if (turnOffTimer > 0)
        {
            turnOffTimer -= Time.deltaTime;
        }

        if (turnOffTimer <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
