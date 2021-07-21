using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public PlayerController player;

    public float force;
    public float speedThreshold = 5f;
    public int staminaUse = 50;
    public float cooldown = 2f;
    private float cooldownTimer = 0f;
    public bool useAnimator = false;

    // TODO: Configurar el player state de Dash (hace daño, recibe daño, etc)

    void Update()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && cooldownTimer <= 0f)
        {
            if (player.UseStamina(staminaUse))
            {
                StartCoroutine(DashCo());
            }
        }
    }

    private IEnumerator DashCo()
    {
        // Empieza el dash
        player.currentState = PlayerState.stagger;
        player.rb.AddForce(player.change * force * 10, ForceMode2D.Impulse);
        if (useAnimator) player.animator.SetBool("dashing", true);
        cooldownTimer = cooldown;

        // Mientras la velocidad del jugador sea mayor que esto, se mantiene en el estado "dash"
        while (player.rb.velocity.sqrMagnitude > speedThreshold)
        {
            yield return null;
        }

        // Termina el dash
        if (useAnimator) player.animator.SetBool("dashing", false);
        player.currentState = PlayerState.idle;
    }
}
