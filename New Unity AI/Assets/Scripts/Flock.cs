using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    public FlockAgent agentPrefab;
    private List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    public int startingCount = 25;
    const float AgentDensity = 0.08f;

    [Range(1f, 1000f)]
    public float driveFactor = 10f;
    [Range(1f, 1000f)]
    public float maxSpeed = 5f;
    [Range(1f, 100f)]
    public float neighborRadius = 3f;
    [Range(0f, 10f)]
    public float avoidanceRadiusMultiplier = 3f;

    private float squareMaxSpeed;
    private float squareNeighborRadius;
    private float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return SquareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            agent.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            //Vector3 move = behaviour.CalculateMove(agent, context, this);
            //move *= driveFactor;
            //if (move.sqrMagnitude>squareMaxSpeed)
            //{
            //    move = move.normalized * maxSpeed;
            //}
            //agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c!= agent.AgentCollider)
            {
                context.Add(c.transform);
            }

        }
        return context;
    }
}
