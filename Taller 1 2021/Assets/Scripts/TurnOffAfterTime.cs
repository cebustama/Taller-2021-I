using UnityEngine;
using UnityEngine.Events;

public class TurnOffAfterTime : MonoBehaviour
{
    public float turnOffTimeAmount = 1f;
    private float turnOffTimer;

    public UnityEvent onTurnOffEvent;

    private void OnEnable()
    {
        turnOffTimer = turnOffTimeAmount;
    }

    private void Update()
    {
        // Disminuir timer
        if (turnOffTimer > 0)
        {
            turnOffTimer -= Time.deltaTime;
        }

        // Desactivar objeto
        if (turnOffTimer <= 0)
        {
            onTurnOffEvent.Invoke();
            gameObject.SetActive(false);
        }
    }
}
