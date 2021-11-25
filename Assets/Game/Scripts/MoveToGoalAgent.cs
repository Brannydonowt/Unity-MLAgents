using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField]private Classroom myClass;
    [SerializeField]private Transform goalTransform;
    [SerializeField]private Material winMat, lostMat;
    [SerializeField]private MeshRenderer floorRenderer;
    [SerializeField]private bool randomSpawn;

    private void Awake() 
    {
        myClass = GetComponentInParent<Classroom>();   
    }

    public override void OnEpisodeBegin()
    {
        myClass.ResetClassroom();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(goalTransform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        
        float moveSpeed = 5f;

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

        AddReward(-1f / MaxStep);
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
            SetReward(-1f);
            floorRenderer.material = lostMat;
        }

        else if (other.CompareTag("Goal"))
        {
            // positive reward
            SetReward(1f);
            floorRenderer.material = winMat;
        }
        
        EndEpisode();
    }
}
