using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerScriptableObject playerData;

    float currentHealth;
    float currentMoveSpeed;
    float currentRecovery;
    float currentStrength;
    float currentProjectileSpeed;
    float currentAttackSpeed;
    float currentDashRange;
    // Start is called before the first frame update
    private void Awake()
    {
        currentAttackSpeed = playerData.AttackSpeed;
        currentRecovery = playerData.Recovery;
        currentStrength = playerData.Strength;
        currentMoveSpeed = playerData.MoveSpeed;
        currentProjectileSpeed = playerData.ProjectileSpeed;
        currentDashRange = playerData.DashRange;
        currentHealth = playerData.MaxHealth;

    }
}
