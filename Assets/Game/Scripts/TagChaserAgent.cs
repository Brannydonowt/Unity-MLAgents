using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TagChaserAgent : Agent
{
    [SerializeField]private Vector3 spawnpoint;
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
        transform.position = spawnpoint;
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
            AddReward(0.1f);
        } 
        else
        {
            AddReward(-0.1f);
        }

        if (targetTransform.localPosition.x >= goalX)
        {
            SetReward(-100f);
            EndEpisode();
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

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Runner"))
        {
            SetReward(100f);
        }

        EndEpisode();
    }
}
