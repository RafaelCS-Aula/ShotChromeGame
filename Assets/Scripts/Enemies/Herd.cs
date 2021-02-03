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
    //List<EnemyHealth> ehAgents = new List<EnemyHealth>();

    [SerializeField] private Transform target;

    [Range(1, 500)]
    [SerializeField] private int agentCount = 10;

    [Range(1f, 100f)]
    [SerializeField] private float agentChaseSpeed;
    [SerializeField] private float agentChaseDist;

    [HideInInspector] public bool isChasing;
    [HideInInspector] public bool isWandering;

    [SerializeField] private LayerMask wallLayer;


    [Range(1f, 100f)]
    [SerializeField] private float agentWanderSpeed;

    [SerializeField] private Vector3 wanderLimits = Vector3.zero;

    [SerializeField] private Color boundingBoxColor;

    [SerializeField] private Transform wanderBounds;

    [HideInInspector] public Vector3 goalPos;

    [SerializeField] private float timeUntilNextGoalChange;

    private float goalChangeTimer;

    public bool showDebugGizmos;


    void Start()
    {
        goalPos = transform.position;
        // Iterate through every agent that will be created
        for (int i = 0; i < agentCount; i++)
        {
            // Create an instance of said agent, and assign it as a child of this object
            GameObject newAgent = Instantiate(
                agentPrefab, transform);

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

            // Assign the given chase distance to each ecAgents chaseDist variable
            ecAgents[i].chaseDist = agentChaseDist;

            // Set every ecAgents' wallLayer to variable to the Herd's wallLayer
            ecAgents[i].wallLayer = wallLayer;

            // Set every ecAgents' wallLayer to variable to the Herd's wallLayer
            ecAgents[i].herdWanderBounds = wanderBounds;

            nmAgents[i].speed = agentWanderSpeed;
        }

        wanderBounds.transform.localScale = wanderLimits;
    }

    void Update()
    {
        RemoveDeadAgents();
        UpdateMovementStatus();

        UpdateBounds();

        // Iterate through every agent created
        for (int i = 0; i < nmAgents.Count; i++)
        {
            // Change the isHerdChasing variable for every agent in the ecAgents list based on the return of the AnyAgentInChaseDist method call
            ecAgents[i].isHerdChasing = isChasing;
            ecAgents[i].isHerdWandering = isWandering;
        }

        if (isWandering) UpdateGoalPosition();

        if (!CheckForAgents()) Destroy(transform.gameObject);
    }

    /// <summary>
    /// Check if any agent on the ecAgents list is in Chase distance of the mutual target
    /// </summary>
    /// <returns></returns>
    private void UpdateMovementStatus()
    {
        int count = nmAgents.Count;
        bool hasAgentInChaseDist = false;
        // Iterate through every agent created
        for (int i = 0; i < count; i++)
        {
            // Return true if one of the agents is in Chase distance and if it has a path to the target
            if (ecAgents[i].inChaseDist /*&& ecAgents[i].hasPath*/)
            {
                isChasing = true;
                isWandering = false;

                hasAgentInChaseDist = true;
            }

            // Check if the loop is on it's last iteraction
            if (i == count - 1)
            {
                if (!hasAgentInChaseDist)
                {
                    // Update the bounding box before setting the herd to Wandering
                    //if (!isWandering) UpdateBoundingBox(); //This complicates things, cause if the box is changed, the wanderers goal position may be outside the navMesh or inside a wall -
                    //If needed, make verifications to be able to use this

                    isChasing = false;
                    isWandering = true;
                }
            }
        }

        for (int i = 0; i < nmAgents.Count; i++)
        {
            if (isChasing)
            {
                if (nmAgents[i])
                {
                    nmAgents[i].speed = agentChaseSpeed;
                    nmAgents[i].stoppingDistance = 2;
                }
            }
            else
            {
                if (nmAgents[i])
                {
                    nmAgents[i].speed = agentWanderSpeed;
                    nmAgents[i].stoppingDistance = 0;
                }
            }
        }
    }

    private void UpdateBoundingBox()
    {
        float totalX = 0f;
        float totalZ = 0f;

        for (int i = 0; i < nmAgents.Count; i++)
        {
            totalX += nmAgents[i].transform.position.x;
            totalZ += nmAgents[i].transform.position.z;
        }

        float centerX = totalX / nmAgents.Count;
        float centerZ = totalZ / nmAgents.Count;

        wanderBounds.position = new Vector3(centerX, wanderBounds.position.y, centerZ);
    }

    private void UpdateGoalPosition()
    {
        goalChangeTimer += Time.deltaTime;
        //print(goalChangeTimer);
        if (nmAgents.Count <= 0) return;

        if (!wanderBounds.GetComponent<Collider>().bounds.Contains(goalPos) ||
            !ecAgents[0].CheckForPath(goalPos) || goalChangeTimer >= timeUntilNextGoalChange)
        {
            print("CHANGED");
            goalPos = wanderBounds.position +
                    new Vector3(Random.Range(-wanderLimits.x / 2, wanderLimits.x / 2),
                    wanderBounds.localPosition.y,
                    Random.Range(-wanderLimits.z / 2, wanderLimits.z / 2));

            goalChangeTimer = 0;
            return;
        }

        /*GOAL POSITION CHANGED WHEN ONE AGENTS REACHES IT*/
        for (int i = 0; i < cAgents.Count; i++)
        {
            if (cAgents[i] && cAgents[i].bounds.Contains(goalPos))
            {
                //print("GOAL REAHCED");
                goalPos = wanderBounds.position +
                    new Vector3(Random.Range(-wanderLimits.x / 2, wanderLimits.x / 2),
                    wanderBounds.localPosition.y,
                    Random.Range(-wanderLimits.z / 2, wanderLimits.z / 2));
                
                goalChangeTimer = 0;
                return;
            }
        }
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

    private void UpdateBounds()
    {
        wanderBounds.localScale = wanderLimits;
    }

    void OnDrawGizmosSelected()
    {
        if (showDebugGizmos)
        {
            Gizmos.color = boundingBoxColor;
            Gizmos.DrawCube(wanderBounds.position, wanderBounds.localScale);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(goalPos, .5f);
        }
    }

    public Transform GetTarget() => target;

    private bool CheckForAgents()
    {
        if (nmAgents.Count <= 0) return false;
        else return true;
    }
}