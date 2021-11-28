using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TagRunnerAgent : Agent
{
    [SerializeField]private TagChaserAgent chaserAgent;

    [SerializeField]private Vector3 spawnpoint;
    [SerializeField]private bool randSpawn;
    [SerializeField]private Vector3 minSpawn;
    [SerializeField]private Vector3 maxSpawn;
    [SerializeField]private Classroom myClass;

    [SerializeField]private Transform chaserTransform;
    [SerializeField]private float goalX;

    private float lDistFromGoal;
    private float lDistFromChaser;

    private void Awake() 
    {
        myClass = GetComponentInParent<Classroom>();
    }

    public override void OnEpisodeBegin()
    {
        if (randSpawn)
            transform.localPosition = new Vector3(Random.Range(minSpawn.x, maxSpawn.x), spawnpoint.y, Random.Range(minSpawn.z, maxSpawn.z));
        else
            transform.localPosition = spawnpoint;
        SetReward(0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(chaserTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        
        float moveSpeed = 5f;

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

        float newDistG = Vector3.Distance(transform.position, new Vector3(15, transform.position.y, transform.position.z));
        float newDistC = Vector3.Distance(transform.position, chaserTransform.position);
        // We got closer to our goal
        if (newDistG < lDistFromGoal)
        {
            AddReward(20 / newDistG);
        } 
        else
        {
            AddReward(20 / newDistG * -1);
        }

        lDistFromGoal = newDistG;

        if (newDistC > lDistFromChaser)
            AddReward(10 / newDistC);
        else
            AddReward(10 / newDistC * -1);

        lDistFromChaser = newDistC;        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contrinousActions = actionsOut.ContinuousActions;
        contrinousActions[0] = Input.GetAxisRaw("Horizontal");
        contrinousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other) 
    {
        
        if (other.CompareTag("Wall"))
        {
            // negative reward
            SetReward(-20000);
            myClass.EndEpisodes();        
        }

        if (other.CompareTag("Goal"))
        {
            // positive reward
            SetReward(20000f);
            myClass.EndEpisodes();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Chaser"))
        {
            SetReward(-20000f);
            myClass.EndEpisodes();
        }
    }

    
}
