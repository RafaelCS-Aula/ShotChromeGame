using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class Herd : MonoBehaviour
{
    [SerializeField] private GameObject agentPrefab;

    [SerializeField] List<NavMeshAgent> nmAgents = new List<NavMeshAgent>();
    List<EnemyAgentControl> ecAgents = new List<EnemyAgentControl>();
    List<Collider> cAgents = new List<Collider>();

    [SerializeField] private Transform target;

    [Range(1, 500)]
    [SerializeField] private int agentCount = 10;

    [Range(1f, 100f)]
    [SerializeField] private float agentChaseSpeed;

    public bool showDebugGizmos;


    void Start()
    {
        // Iterate through every agent that will be created
        for (int i = 0; i < agentCount; i++)
        {
            // Create an instance of said agent, and assign it as a child of this object
            GameObject newAgent = Instantiate(
                agentPrefab, transform.position, Quaternion.identity, transform);

            // Add the agents NavMeshAgent component (required) to the nmAgents list
            nmAgents.Add(newAgent.GetComponent<NavMeshAgent>());

            // Add the agents EnemyAgentControl component (required) to the ecAgents list
            ecAgents.Add(newAgent.GetComponent<EnemyAgentControl>());

            // Add the agents Collider component (required) to the cAgents list
            cAgents.Add(newAgent.GetComponent<Collider>());

            // Assign a proper name to the agent
            newAgent.name = $"{name} A{i + 1}";

            // Assign each agent to the Enemy layer
            newAgent.layer = LayerMask.NameToLayer("Enemy");
        }

        // Iterate through every agent created
        for (int i = 0; i < agentCount; i++)
        {
            // Assign the given target to each ecAgents target variable
            ecAgents[i].target = target;

            nmAgents[i].speed = agentChaseSpeed;

            nmAgents[i].stoppingDistance = 2;
        }
    }

    void Update()
    {
        RemoveDeadAgents();

        if (!CheckForAgents()) Destroy(transform.gameObject);
    }

    private void RemoveDeadAgents()
    {
        int removedCount = 0;
        int numAgents = nmAgents.Count; 
        //List<int> indexesToRemove = new List<int>();
        for (int i = numAgents-1; i > 0; i--)
        {
            if (nmAgents[i] == null)
            {
                //indexesToRemove.Add(i);
                nmAgents.RemoveAt(i);
                ecAgents.RemoveAt(i);
                cAgents.RemoveAt(i);
                removedCount++;

                
            }
        }
    }

    void OnDrawGizmosSelected()
    {

    }

    public Transform GetTarget() => target;

    private bool CheckForAgents()
    {
        if (nmAgents.Count <= 0) return false;
        else return true;
    }
}