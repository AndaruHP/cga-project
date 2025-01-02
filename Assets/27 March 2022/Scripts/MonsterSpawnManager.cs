using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance { get; private set; }
    public float respawnTime = 60f; // 1 minute respawn time
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartRespawnTimer(GameObject monster)
    {
        StartCoroutine(RespawnMonster(monster));
    }

    private IEnumerator RespawnMonster(GameObject monster)
    {
        yield return new WaitForSeconds(respawnTime);

        // Reset position to original spawn point
        monster.transform.position = monster.GetComponent<Antagonis>().Patrol[0].position;
        
        // Reset the NavMeshAgent
        UnityEngine.AI.NavMeshAgent agent = monster.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = false;
            agent.ResetPath();
        }

        // Reactivate the monster
        monster.SetActive(true);

        // Reset animator parameters
        Animator animator = monster.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isDeath", false);
            animator.SetFloat("Walk", 0f);
            animator.SetBool("Looking Around", false);
        }

        Debug.Log($"Monster respawned after {respawnTime} seconds");
    }
}