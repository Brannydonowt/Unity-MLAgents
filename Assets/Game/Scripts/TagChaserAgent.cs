using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TagChaserAgent : Agent
{
    [SerializeField]TagRunnerAgent runnerAgent;
    [SerializeField]private Vector3 spawnpoint;
    [SerializeField]private bool randSpawn;
    [SerializeField]private Vector3 minSpawn;
    [SerializeField]private Vector3 maxSpawn;
    [SerializeField]private Classroom myClass;
    [SerializeField]private Transform targetTransform;
    [SerializeField]private float goalX;
    private float lDistFromTarget;

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
        sensor.AddObservation(targetTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        
        float moveSpeed = 5f;

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

        //AddReward(-1f / MaxStep);

        float newDistR = Vector3.Distance(transform.position, targetTransform.position);
        // We got closer to our goal
        if (newDistR < lDistFromTarget)
        {
            AddReward(10 / newDistR);
        } 
        else
        {
            AddReward(10 / newDistR * -1);
        }

        lDistFromTarget = newDistR;
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
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Runner"))
        {
            SetReward(20000);
            myClass.EndEpisodes();
        }
    }
}
