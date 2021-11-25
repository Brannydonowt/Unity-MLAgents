using UnityEngine;

// The academy will be responsible for spawning classrooms in the correct positions
public class Academy : MonoBehaviour 
{
    public GameObject classToSpawn;
    
    public int numToSpawn;
    public int numRows;
    
    public float xSpacing;
    public float zSpacing;

    private void Start()
    {
        SetupAcademy();    
    }

    void SetupAcademy()
    {
        Classroom c = classToSpawn.GetComponent<Classroom>();
        Vector3 cSize = c.GetClassRoomSize();

        float xDist = 0;
        float zDist = 0;

        for (int i = 0; i < numToSpawn; i++)
        {
            xDist += (cSize.x + xSpacing);

            if (i % numRows == 0)
            {
                xDist = 0;
                zDist += (cSize.z + zSpacing);
            }

            Vector3 spawn = new Vector3(xDist, 0, zDist);
            GameObject newClass = Instantiate(classToSpawn, spawn, Quaternion.identity);
        }


    }
}