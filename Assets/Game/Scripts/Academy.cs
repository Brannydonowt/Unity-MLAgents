using UnityEngine;
using System.Collections.Generic;

// The academy will be responsible for spawning classrooms in the correct positions
public class Academy : MonoBehaviour 
{
    public AcademyUI ui;
    public GameObject classToSpawn;
    
    public int numToSpawn;
    public int numColumns;
    
    public float xSpacing;
    public float zSpacing;

    public GameObject mainCam;
    public GameObject currentCam;

    public bool classCamMode;

    public List<Classroom> classes;
    public Classroom currentClass;
    public int currentClassId = 0;
    public int currentClassCam = 0;
    private void Start()
    {
        SetupAcademy();
    }

    void SetupAcademy()
    {
        Classroom c = classToSpawn.GetComponent<Classroom>();
        Vector3 cSize = c.GetClassRoomSize();
        ui.SetClassType(classToSpawn.name);

        float xDist = 0;
        float zDist = 0;

        for (int i = 0; i < numToSpawn; i++)
        {
            xDist += (cSize.x + xSpacing);

            if (i % numColumns == 0)
            {
                xDist = 0;
                zDist += (cSize.z + zSpacing);
            }

            Vector3 spawn = new Vector3(xDist, 0, zDist);
            GameObject newClass = Instantiate(classToSpawn, spawn, Quaternion.identity);
            classes.Add(newClass.GetComponent<Classroom>());
        }

        print(cSize.z);

        SetCamera(cSize);
    }

    void SetCamera(Vector3 cSize)
    {
        float academyX = numColumns * (cSize.x + xSpacing / 2);
        float academyY = (numToSpawn / numColumns) * (cSize.z + zSpacing);

        print (academyX);
        print(academyY);

        Camera.main.transform.position = new Vector3(academyX / 2, 15, academyY / 2);

        currentClass = classes[0];
        currentClassId = 0;
        currentCam = mainCam;

        ui.SetClassName("All classes");
        ui.SetCamName(currentCam.name);
    }

    private void Update()
    {
        // Enable Class Cam Mode
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            classCamMode = true;
            ChangeActiveCamera(currentClass.classCams[currentClassCam]);
            ui.SetClassName("Classroom " + (currentClassId + 1));
        }

        // Enable main cam mode
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            classCamMode = false;
            ChangeActiveCamera(mainCam);
            ui.SetClassName("All classes");
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
            ChangeCurrentClass(false);

        if (Input.GetKeyDown(KeyCode.RightBracket))
            ChangeCurrentClass(true);

        if (Input.GetKeyDown(KeyCode.Keypad4))
            ChangeActiveCamera(GetNextClassCamera(false));

        if (Input.GetKeyDown(KeyCode.Keypad6))
            ChangeActiveCamera(GetNextClassCamera(true));
        
    }

    void ChangeActiveCamera(GameObject cam)
    {
        currentCam.SetActive(false);
        currentCam = cam;
        currentCam.SetActive(true);
        ui.SetCamName(currentCam.name);
    }

    void ChangeCurrentClass(bool updown)
    {
        if (updown)
        {
            // Check if we're as high as we can be
            if (currentClassId == classes.Count - 1)
                return;

            currentClassId++;
            currentClass = classes[currentClassId];
        }
        else
        {
            // Check if we're already as low as we can be
            if (currentClassId == 0)
                return;

            currentClassId--;
            currentClass = classes[currentClassId];
        }

        if (classCamMode){
            ChangeActiveCamera(currentClass.classCams[currentClassCam]);
            ui.SetClassName("Classroom " + (currentClassId + 1));
        }
    }

    GameObject GetNextClassCamera(bool updown)
    {
        if (!classCamMode)
            return mainCam;

        if (updown)
        {
            if (currentClassCam == currentClass.classCams.Length - 1)
            {
                currentClassCam = 0;
                
            } 
            else
                currentClassCam++;
        } 
        else 
        {
            if (currentClassCam == 0)
                currentClassCam = currentClass.classCams.Length -1;
            else
                currentClassCam --;
        }

        ui.SetCamName(currentClass.classCams[currentClassCam].name);
        return currentClass.classCams[currentClassCam];
    }
}