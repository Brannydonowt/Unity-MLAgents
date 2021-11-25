using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will be responsible for 
public class Classroom : MonoBehaviour
{
    public bool randomAgentSpawn;
    public bool randomGoalSpawn;

    public Vector3 setAgentSpawn;
    public Vector3 setGoalSpawn;

    public GameObject platform;
    public Vector3 classSize;

    public GameObject agent;
    public GameObject goal;

    float agentMinSpawnX, agentMaxSpawnX;
    float agentMinSpawnZ, agentMaxSpawnZ;
    float goalMinSpawnX, goalMaxSpawnX;
    float goalMinSpawnZ, goalMaxSpawnZ;

    public void ResetClassroom()
    {
        if (randomAgentSpawn)
            agent.transform.localPosition = GetRandAgentSpawn(classSize.y);
        else
            agent.transform.localPosition = setAgentSpawn;

        if (randomGoalSpawn)
            goal.transform.localPosition = GetRandGoalSpawn(classSize.y);
        else
            goal.transform.localPosition = setGoalSpawn;
    }

    public Vector3 GetRandAgentSpawn(float yAxis)
    {
        return new Vector3(Random.Range(agentMaxSpawnX, agentMaxSpawnX), yAxis, Random.Range(agentMinSpawnZ, agentMaxSpawnZ));
    }

    public Vector3 GetRandGoalSpawn(float yAxis)
    {
        return new Vector3(Random.Range(goalMinSpawnX, goalMaxSpawnX), yAxis, Random.Range(goalMinSpawnZ, goalMaxSpawnZ));
    }

    public Vector2 GetClassRoomSize()
    {        
        classSize = Vector3.Scale(transform.localScale, platform.GetComponent<MeshRenderer>().bounds.size);

        return classSize;
    }
}
