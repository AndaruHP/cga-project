using UnityEngine;

public class AIStats : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float chaseSpeed = 5f;
    public float chaseTime = 7f;
    
    [Header("Detection Settings")]
    public float radius = 8f;
    public float timeToWait = 15f;
    public float angle = 150f;
    public float hearValue = 0f;

    [Header("Combat Settings")]
    public float damage = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
} 