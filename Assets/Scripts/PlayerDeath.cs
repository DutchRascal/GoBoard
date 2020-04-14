using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Animator playerAnimatorController;
    public string playerDeathTrigger = "IsDead";

    public void Die()
    {
        if (playerAnimatorController)
        {
            playerAnimatorController.SetTrigger(playerDeathTrigger);
        }
    }
}
