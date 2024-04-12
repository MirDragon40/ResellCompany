using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockyGridRoomControls : MonoBehaviour
{
    //It is easier, and save time/speed to simply deactivate/activate any of the blocks/children
    [HideInInspector]public PGBlockGridLevel BlockyGrid;

    //[Header("Obstacles:")]
    [HideInInspector] public int ObstacleAmount;

    //[Header("Enemies:")]
    [HideInInspector] public int EnemiesAmount;

    //[Header("Floor Block:")]
    [HideInInspector] public GameObject NewFloorBlock;
    [HideInInspector] public int FloorBlockAmountToChange;

    //[Header("Wall Block:")]
    [HideInInspector] public GameObject NewWallBlock;

    //[Header("Doorway Block:")]
    [HideInInspector] public GameObject NewDoorway;

    //[Header("New Obstacles")]
    [HideInInspector] public List<GameObject> NewObstacles = new List<GameObject>();


    //All the walls
    [HideInInspector]public List<GameObject> WallBlocks = new List<GameObject>();

    //All the floor
    
    [HideInInspector]public List<GameObject> FloorBlocks = new List<GameObject>();

    //All the doorways
    [HideInInspector]public List<GameObject> Doorways = new List<GameObject>();

    [HideInInspector]public List<GameObject> ObstacleBlocks = new List<GameObject>();

    [HideInInspector]public Material NewMateraialForThisRoom;

    [HideInInspector] public GameObject CenterBlock;
    //For if a room has center blocks
    [HideInInspector] public List<GameObject> CenterAreaBlocks = new List<GameObject>();

    //Corner Spots
    [HideInInspector]public List<GameObject> CornerSpots = new List<GameObject>();
    [HideInInspector]public List<GameObject> CornerSpotsUsed = new List<GameObject>();
    [HideInInspector]public List<GameObject> cornerObjects = new List<GameObject>();
    [HideInInspector] public List<GameObject> newCornerObjects = new List<GameObject>();
    [HideInInspector]public int CornerObjectAmount;


    /*//New wall block
    public GameObject newWallBlock;*/


    public void CloseOrOpenDoorways()
    {
        foreach(GameObject d in Doorways)
        {
            if(!d.transform.GetChild(0).gameObject.activeSelf)
            {
                d.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                d.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void HideOrShowWalls()
    {
        foreach (GameObject w in WallBlocks)
        {
            if (!w.transform.GetChild(0).gameObject.activeSelf)
            {
                w.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                w.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void ReorderObstacles()
    {
        
        for (int i = 0; i < ObstacleBlocks.Count; i++)
        {
            GameObject obstSelect = ObstacleBlocks[i];

            int rando = Random.Range(0, FloorBlocks.Count);
            GameObject floorSelect = FloorBlocks[rando];

            Vector3 floorPos = new Vector3(floorSelect.transform.position.x, floorSelect.transform.position.y, floorSelect.transform.position.z);

            obstSelect.transform.position = floorPos;
        }
    }

    public void AddObstacles()
    {
        for(int i = 0; i < ObstacleAmount; i ++)
        {
            if (FloorBlocks.Count > 0 && BlockyGrid.Obstacles.Count >= 1)
            {
                //Get Random floor block and position
                int rando = Random.Range(0, FloorBlocks.Count);
                GameObject floorSelect = FloorBlocks[rando];
                GameObject newObstacle = null;
                
                if (NewObstacles.Count < 1)
                {

                    //Get, instantiate, position, parent, rename, and add to list, new obstacle
                    int ranObs = Random.Range(0, BlockyGrid.Obstacles.Count);
                    newObstacle = BlockyGrid.Obstacles[ranObs];
                }
                else if(NewObstacles.Count >= 1)
                {
                    //Get, instantiate, position, parent, rename, and add to list, new obstacle
                    int ranObs = Random.Range(0, NewObstacles.Count);
                    newObstacle = NewObstacles[ranObs];
                }
                else
                {
                    newObstacle = NewObstacles[0];
                }

                if(newObstacle != null)
                {
                    newObstacle = Instantiate(newObstacle.gameObject, transform.position, transform.rotation);

                    //Get a Y position
                    float newPosY = newObjectYPosition(newObstacle);
                    Vector3 floorPos = new Vector3(floorSelect.transform.position.x, newPosY, floorSelect.transform.position.z);


                    newObstacle.transform.position = floorPos;
                    newObstacle.transform.parent = floorSelect.transform.parent.transform;
                    newObstacle.name = "Obstacle";
                    ObstacleBlocks.Add(newObstacle);
                }


            }

        }

    }
    public void RemoveObstacles()
    {
        List<GameObject> objectsToDelete = new List<GameObject>();

        for (int i = 0; i < ObstacleAmount; i++)
        {
            if(ObstacleBlocks.Count > 0)
            {
                //Get Random floor block and position
                int rando = Random.Range(0, ObstacleBlocks.Count);
                GameObject obstacleSelect = ObstacleBlocks[rando];


                objectsToDelete.Add(obstacleSelect);
                obstacleSelect.SetActive(false);

                ObstacleBlocks.Remove(obstacleSelect);
            }
            

        }

        for (int i = 0; i < objectsToDelete.Count; i++)
        {
            DestroyImmediate(objectsToDelete[i].gameObject);
        }

    }

    public void ChangeObstacles_NewObs()
    {
        List<GameObject> objectsToDelete = new List<GameObject>();
        List<GameObject> newObjectsTempList = new List<GameObject>();

        foreach(GameObject oldObst in ObstacleBlocks)
        {
            Vector3 obstaclePos = new Vector3(oldObst.transform.position.x, oldObst.transform.position.y, oldObst.transform.position.z);


            //Get, instantiate, position, parent, rename, and add to list, new obstacle

            GameObject newObst = null;

            if (NewObstacles.Count > 1)
            {
                int r = Random.Range(0, NewObstacles.Count);
                newObst = Instantiate(NewObstacles[r].gameObject, transform.position, transform.rotation);
            }
            else if (NewObstacles.Count == 1)
            {
                newObst = Instantiate(NewObstacles[0].gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.NewObstacles.Count > 1)
            {
                int r = Random.Range(0, BlockyGrid.NewObstacles.Count);
                newObst = Instantiate(BlockyGrid.NewObstacles[r].gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.NewObstacles.Count == 1)
            {
                newObst = Instantiate(BlockyGrid.NewObstacles[0].gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.Obstacles.Count > 1)
            {
                int r = Random.Range(0, BlockyGrid.Obstacles.Count);
                newObst = Instantiate(BlockyGrid.Obstacles[r].gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.Obstacles.Count == 1)
            {
                newObst = Instantiate(BlockyGrid.Obstacles[0].gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.newFloorBlock != null)
            {
                newObst = Instantiate(BlockyGrid.newFloorBlock.gameObject, transform.position, transform.rotation);
                Debug.LogWarning("No obstacles selected - Floor has been placed.");
            }
            else if (BlockyGrid.newFloorBlock != null)
            {
                newObst = Instantiate(BlockyGrid.FloorBlock.gameObject, transform.position, transform.rotation);
                Debug.LogWarning("No obstacles selected - Floor has been placed.");
            }
            else
            {
                Debug.LogWarning("Cannot Change Obstacles Blocks Until New Floor Block Is Assigned");
            }

            //Get a Y position
            float newPosY = newObjectYPosition(newObst); 

            newObst.transform.position = new Vector3(obstaclePos.x, newPosY, obstaclePos.z);
            newObst.name = "Obstacle";
            newObst.transform.parent = oldObst.transform.parent.transform;

            newObjectsTempList.Add(newObst);




            objectsToDelete.Add(oldObst);
            oldObst.SetActive(false);

            //ObstacleBlocks.Remove(oldObst); 

        }

        ObstacleBlocks.Clear();

        for(int i = 0; i < newObjectsTempList.Count; i++)
        {
            ObstacleBlocks.Add(newObjectsTempList[i]);
        }
        for (int i = 0; i < objectsToDelete.Count; i++)
        {
            DestroyImmediate(objectsToDelete[i].gameObject);
        }
    }

    public void SpawnEnemiesInRoom()
    {
        for(int i = 0; i < EnemiesAmount; i++)
        {
            //Random spot in room
            int randoSpot = Random.Range(0, FloorBlocks.Count);
            Transform sp = FloorBlocks[randoSpot].transform;

            //Random Enemy
            int randoEnem = Random.Range(0, BlockyGrid.Enemies.Count);
            GameObject enem = Instantiate(BlockyGrid.Enemies[randoEnem], transform.position, transform.rotation);
   
            float newPosY = 0;
            if (enem.GetComponent<Renderer>() != null)
            {
                newPosY = (enem.GetComponent<Renderer>().bounds.size.y * 0.5f) + (BlockyGrid.blockSizeY * 0.5f);
            }
            else if (enem.transform.GetChild(0) != null && enem.transform.GetChild(0).gameObject.GetComponent<Renderer>() != null)
            {
                newPosY = (enem.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f) + (BlockyGrid.blockSizeY * 0.5f);
            }
            else
            {
                newPosY = BlockyGrid.blockSizeY;

            }

            //Calculate and assign position
            Vector3 enemSpot = new Vector3(sp.position.x, newPosY, sp.position.z);
            enem.transform.position = enemSpot;
            enem.transform.parent = this.gameObject.transform.parent.transform;

        }
    }

    
    public void ChangeFloors_All()
    {
        List<GameObject> tempHoldFloor = new List<GameObject>();

        List<GameObject> tempHoldCorner = new List<GameObject>();

        List<GameObject> tempHoldCornerUsed = new List<GameObject>();

        List<GameObject> tempHoldCenterBlocks = new List<GameObject>();

        List<GameObject> objectsToDelete = new List<GameObject>();

        foreach(GameObject i in FloorBlocks)
        {
            //Get Old floor block and it's position
            GameObject oldFloor = i;
            Vector3 floorPos = oldFloor.transform.position;

            //Get the new game block
            GameObject newFloor = null;
            if(NewFloorBlock != null)
            {
                newFloor = Instantiate(NewFloorBlock.gameObject, transform.position, transform.rotation);
            }
            else if(BlockyGrid.newFloorBlock != null)
            {
                newFloor = Instantiate(BlockyGrid.newFloorBlock.gameObject, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogError("No new block selected");
                return;
            }

            //Assign Position, name, parent, and add to temp list
            newFloor.transform.position = floorPos;
            if(oldFloor.name == "CenterBlock")
            {
                newFloor.name = "CenterBlock";
                CenterBlock = newFloor.gameObject;
            }
            else
            {
                newFloor.name = "Floor";
            }
            //newFloor.name = "Floor";
            newFloor.transform.parent = oldFloor.transform.parent;
            tempHoldFloor.Add(newFloor);

            //Get rid of old floor
            oldFloor.SetActive(false);
            //FloorBlocks.Remove(oldFloor);
            objectsToDelete.Add(oldFloor);


        }

        foreach (GameObject i in CornerSpots)
        {
            //Get Old floor block and it's position
            GameObject oldFloor = i;
            Vector3 floorPos = oldFloor.transform.position;

            //Get the new game block
            GameObject newFloor = null;
            if (NewFloorBlock != null)
            {
                newFloor = Instantiate(NewFloorBlock.gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.newFloorBlock != null)
            {
                newFloor = Instantiate(BlockyGrid.newFloorBlock.gameObject, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogError("No new block selected");
                return;
            }

            //Assign Position, name, parent, and add to temp list
            newFloor.transform.position = floorPos;
            
            //Name the corner
            newFloor.name = "CornerBlock";

            //newFloor.name = "Floor";
            newFloor.transform.parent = oldFloor.transform.parent;

            tempHoldCorner.Add(newFloor);

            //Get rid of old floor
            oldFloor.SetActive(false);
            //FloorBlocks.Remove(oldFloor);
            objectsToDelete.Add(oldFloor);


        }

        foreach (GameObject i in CornerSpotsUsed)
        {
            //Get Old floor block and it's position
            GameObject oldFloor = i;
            Vector3 floorPos = oldFloor.transform.position;

            //Get the new game block
            GameObject newFloor = null;
            if (NewFloorBlock != null)
            {
                newFloor = Instantiate(NewFloorBlock.gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.newFloorBlock != null)
            {
                newFloor = Instantiate(BlockyGrid.newFloorBlock.gameObject, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogError("No new block selected");
                return;
            }

            //Assign Position, name, parent, and add to temp list
            newFloor.transform.position = floorPos;

            //Name the corner
            newFloor.name = "CornerBlock";

            //newFloor.name = "Floor";
            newFloor.transform.parent = oldFloor.transform.parent;

            tempHoldCornerUsed.Add(newFloor);

            //Get rid of old floor
            oldFloor.SetActive(false);
            //FloorBlocks.Remove(oldFloor);
            objectsToDelete.Add(oldFloor);


        }

        foreach(GameObject i in CenterAreaBlocks)
        {
            //Get Old floor block and it's position
            GameObject oldFloor = i;
            Vector3 floorPos = oldFloor.transform.position;

            //Get the new game block
            GameObject newFloor = null;
            if (NewFloorBlock != null)
            {
                newFloor = Instantiate(NewFloorBlock.gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.newFloorBlock != null)
            {
                newFloor = Instantiate(BlockyGrid.newFloorBlock.gameObject, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogError("No new block selected");
                return;
            }

            //Assign Position, name, parent, and add to temp list
            newFloor.transform.position = floorPos;

            //Name the corner
            newFloor.name = "CenterPiecesWhole";

            //newFloor.name = "Floor";
            newFloor.transform.parent = oldFloor.transform.parent;

            tempHoldCenterBlocks.Add(newFloor);

            //Get rid of old floor
            oldFloor.SetActive(false);
            //FloorBlocks.Remove(oldFloor);
            objectsToDelete.Add(oldFloor);


        }

        //Destroy old blocks
        for (int i = 0; i < objectsToDelete.Count; i++)
        {
            DestroyImmediate(objectsToDelete[i].gameObject);
        }

        FloorBlocks.Clear();
        CornerSpots.Clear();
        CornerSpotsUsed.Clear();
        CenterAreaBlocks.Clear();

        //Populate list with new blocks
        for (int i = 0; i < tempHoldFloor.Count; i++)
        {
            FloorBlocks.Add(tempHoldFloor[i]);
        }

        for(int i = 0; i < tempHoldCorner.Count; i++)
        {
            CornerSpots.Add(tempHoldCorner[i]);
        }

        for (int i = 0; i < tempHoldCornerUsed.Count; i++)
        {
            CornerSpotsUsed.Add(tempHoldCornerUsed[i]);
        }

        for(int i = 0; i < tempHoldCenterBlocks.Count; i++)
        {
            CenterAreaBlocks.Add(tempHoldCenterBlocks[i]);
        }

        UpdateBlockSizes();

    }

    public void UpdateBlockSizes()
    {
        if(FloorBlocks[0].GetComponent<Renderer>() != null)
        {
            float blockSizeX = FloorBlocks[0].GetComponent<Renderer>().bounds.size.x;
            float blockSizeZ = FloorBlocks[0].GetComponent<Renderer>().bounds.size.z;
            float blockSizeY = FloorBlocks[0].GetComponent<Renderer>().bounds.size.y;

            BlockyGrid.blockSizeX = blockSizeX;
            BlockyGrid.blockSizeZ = blockSizeZ;
            BlockyGrid.blockSizeY = blockSizeY;

        }
        

    }

    public void ChangeFloors_Random()
    {
        List<GameObject> objectsToDelete = new List<GameObject>();

        for (int i = 0; i < FloorBlockAmountToChange; i++)
        {
            int changefloorOrCorner = Random.Range(0, 5);
            if(changefloorOrCorner == 0)
            {
                int changeCornerUsedOrNot = Random.Range(0, 2);

                if(changeCornerUsedOrNot == 0 && CornerSpots.Count >= 1)
                {
                    //Get Old floor block and it's position
                    int rando = Random.Range(0, CornerSpots.Count);
                    GameObject oldFloor = CornerSpots[rando];
                    Vector3 floorPos = oldFloor.transform.position;

                    //Get the new game block
                    GameObject newFloor = null;
                    if (NewFloorBlock != null)
                    {
                        newFloor = Instantiate(NewFloorBlock.gameObject, transform.position, transform.rotation);
                    }
                    else if (BlockyGrid.newFloorBlock != null)
                    {
                        newFloor = Instantiate(BlockyGrid.newFloorBlock.gameObject, transform.position, transform.rotation);
                    }
                    else
                    {
                        Debug.LogError("No new block selected");
                        return;
                    }

                    //Assign Position, name, parent, and add to temp list
                    newFloor.transform.position = floorPos;

                    newFloor.name = "CornerBlock";

                    newFloor.transform.parent = oldFloor.transform.parent;
                    CornerSpots.Add(newFloor);

                    //Get rid of old floor
                    oldFloor.SetActive(false);
                    CornerSpots.Remove(oldFloor);
                    objectsToDelete.Add(oldFloor);
                }
                else if((changeCornerUsedOrNot == 1 && CornerSpotsUsed.Count >= 1))
                {
                    //Get Old floor block and it's position
                    int rando = Random.Range(0, CornerSpotsUsed.Count);
                    GameObject oldFloor = CornerSpotsUsed[rando];
                    Vector3 floorPos = oldFloor.transform.position;

                    //Get the new game block
                    GameObject newFloor = null;
                    if (NewFloorBlock != null)
                    {
                        newFloor = Instantiate(NewFloorBlock.gameObject, transform.position, transform.rotation);
                    }
                    else if (BlockyGrid.newFloorBlock != null)
                    {
                        newFloor = Instantiate(BlockyGrid.newFloorBlock.gameObject, transform.position, transform.rotation);
                    }
                    else
                    {
                        Debug.LogError("No new block selected");
                        return;
                    }

                    //Assign Position, name, parent, and add to temp list
                    newFloor.transform.position = floorPos;

                    newFloor.name = "CornerBlock";

                    newFloor.transform.parent = oldFloor.transform.parent;
                    CornerSpotsUsed.Add(newFloor);

                    //Get rid of old floor
                    oldFloor.SetActive(false);
                    CornerSpotsUsed.Remove(oldFloor);
                    objectsToDelete.Add(oldFloor);
                }
                else
                {
                    i--;
                }
                


            }
            else
            {
                //Get Old floor block and it's position
                int rando = Random.Range(0, FloorBlocks.Count);
                GameObject oldFloor = FloorBlocks[rando];
                Vector3 floorPos = oldFloor.transform.position;

                //Get the new game block
                GameObject newFloor = null;
                if (NewFloorBlock != null)
                {
                    newFloor = Instantiate(NewFloorBlock.gameObject, transform.position, transform.rotation);
                }
                else if (BlockyGrid.newFloorBlock != null)
                {
                    newFloor = Instantiate(BlockyGrid.newFloorBlock.gameObject, transform.position, transform.rotation);
                }
                else
                {
                    Debug.LogError("No new block selected");
                    return;
                }

                //Assign Position, name, parent, and add to temp list
                newFloor.transform.position = floorPos;

                if (oldFloor.name == "CenterBlock")
                {
                    newFloor.name = "CenterBlock";
                    CenterBlock = newFloor.gameObject;
                }
                else
                {
                    newFloor.name = "Floor";
                }

                newFloor.transform.parent = oldFloor.transform.parent;
                FloorBlocks.Add(newFloor);

                //Get rid of old floor
                oldFloor.SetActive(false);
                FloorBlocks.Remove(oldFloor);
                objectsToDelete.Add(oldFloor);
            }
            





        }



        //Destroy old blocks
        for (int i = 0; i < objectsToDelete.Count; i++)
        {
            DestroyImmediate(objectsToDelete[i].gameObject);
        }
    }

    public void ReorderFloorBlocks()
    {

        for (int i = 0; i < FloorBlocks.Count; i++)
        {
            GameObject floorSelect1 = FloorBlocks[i];
            string floorName = floorSelect1.gameObject.name;
            if(floorName != "CenterBlock")
            {
                int rando = Random.Range(0, FloorBlocks.Count);
                GameObject floorSelect2 = FloorBlocks[rando];

                Vector3 floorPos1 = new Vector3(floorSelect1.transform.position.x, floorSelect1.transform.position.y, floorSelect1.transform.position.z);
                Vector3 floorPos2 = new Vector3(floorSelect2.transform.position.x, floorSelect2.transform.position.y, floorSelect2.transform.position.z);

                floorSelect1.transform.position = floorPos2;
                floorSelect2.transform.position = floorPos1;
            }
            
        }

        for (int i = 0; i < CornerSpots.Count; i++)
        {
            GameObject floorSelect1 = CornerSpots[i];

            int rando = Random.Range(0, CornerSpots.Count);
            GameObject floorSelect2 = CornerSpots[rando];

            Vector3 floorPos1 = new Vector3(floorSelect1.transform.position.x, floorSelect1.transform.position.y, floorSelect1.transform.position.z);
            Vector3 floorPos2 = new Vector3(floorSelect2.transform.position.x, floorSelect2.transform.position.y, floorSelect2.transform.position.z);

            floorSelect1.transform.position = floorPos2;
            floorSelect2.transform.position = floorPos1;


        }

        for (int i = 0; i < CornerSpotsUsed.Count; i++)
        {
            GameObject floorSelect1 = CornerSpotsUsed[i];

            int rando = Random.Range(0, CornerSpotsUsed.Count);
            GameObject floorSelect2 = CornerSpotsUsed[rando];

            Vector3 floorPos1 = new Vector3(floorSelect1.transform.position.x, floorSelect1.transform.position.y, floorSelect1.transform.position.z);
            Vector3 floorPos2 = new Vector3(floorSelect2.transform.position.x, floorSelect2.transform.position.y, floorSelect2.transform.position.z);

            floorSelect1.transform.position = floorPos2;
            floorSelect2.transform.position = floorPos1;


        }

    }


    public void ChangeWalls_All()
    {
        List<GameObject> tempHold = new List<GameObject>();

        List<GameObject> objectsToDelete = new List<GameObject>();

        foreach (GameObject i in WallBlocks)
        {
            //Get Old floor block and it's position
            GameObject oldWall = i;
            Vector3 wallPos = oldWall.transform.position;

            //Get the new game block
            GameObject newWall = null;
            if (NewWallBlock != null)
            {
                newWall = Instantiate(NewWallBlock.gameObject, transform.position, transform.rotation);
            }
            else if(BlockyGrid.newWallBlock != null)
            {
                newWall = Instantiate(BlockyGrid.newWallBlock.gameObject, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogError("No new block selected");
                return;
            }

            //Assign Position, name, parent, and add to temp list
            newWall.transform.position = wallPos;
            newWall.name = "Wall";
            newWall.transform.parent = oldWall.transform.parent;
            tempHold.Add(newWall);

            //Get rid of old floor
            oldWall.SetActive(false);
            //FloorBlocks.Remove(oldFloor);
            objectsToDelete.Add(oldWall);





        }

        


        //Destroy old blocks
        for (int i = 0; i < objectsToDelete.Count; i++)
        {
            DestroyImmediate(objectsToDelete[i].gameObject);
        }

        WallBlocks.Clear();

        //Populate list with new blocks
        for (int i = 0; i < tempHold.Count; i++)
        {
            WallBlocks.Add(tempHold[i]);
        }
    }


    public void ChangeDoorways_All()
    {
        List<GameObject> tempHold = new List<GameObject>();

        List<GameObject> objectsToDelete = new List<GameObject>();

        foreach (GameObject i in Doorways)
        {
            //Get Old floor block and it's position
            GameObject oldDoor = i;
            Vector3 doorPos = oldDoor.transform.position;

            //Get the new game block
            GameObject newDoor = null;
            if (NewDoorway != null)
            {
                newDoor = Instantiate(NewDoorway.gameObject, transform.position, transform.rotation);
            }
            else if(BlockyGrid.newDoorObject != null)
            {
                NewDoorway = BlockyGrid.newDoorObject;
                newDoor = Instantiate(BlockyGrid.newDoorObject.gameObject, transform.position, transform.rotation);
            }
            else if (NewWallBlock != null)
            {
                newDoor = Instantiate(NewWallBlock.gameObject, transform.position, transform.rotation);
            }
            else if (BlockyGrid.newWallBlock != null)
            {
                newDoor = Instantiate(BlockyGrid.newWallBlock.gameObject, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogError("No new block selected");
                return;
            }

            //Assign Position, name, parent, and add to temp list
            newDoor.transform.position = doorPos;
            bool doWeRotate = CheckDoorwayWhenChanging_DoWeNeedToTurn_AfterBuilt(oldDoor);

            if(doWeRotate)
            {
                newDoor.transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else
            {
                newDoor.transform.eulerAngles = new Vector3(0, 0, 0);
            }

            


            newDoor.name = "Doorway";
            newDoor.transform.parent = oldDoor.transform.parent;
            tempHold.Add(newDoor);

            //Get rid of old floor
            oldDoor.SetActive(false);
            //FloorBlocks.Remove(oldFloor);
            objectsToDelete.Add(oldDoor);





        }




        //Destroy old blocks
        for (int i = 0; i < objectsToDelete.Count; i++)
        {
            DestroyImmediate(objectsToDelete[i].gameObject);
        }

        Doorways.Clear();

        //Populate list with new blocks
        for (int i = 0; i < tempHold.Count; i++)
        {
            Doorways.Add(tempHold[i]);
        }
    }

    public void ChangeAllFloorBlockMaterial()
    {
        foreach (GameObject f in FloorBlocks)
        {
            if (f.GetComponent<MeshRenderer>() != null)
            {
                f.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else if (f.transform.childCount >= 1 && f.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>() != null)
            {
                f.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else
            {
                Debug.LogError("If you are seeing this, than the material is not finding a mesh renderer on the floor gameObject or it's child. Double Click this Error to open the Script In Visual Studio. Edit either the code above this error to fit your renderer, or use the code commented below to select the child of the index you want.");
                //f.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;

            }

        }

        foreach (GameObject c in CornerSpots)
        {
            if (c.GetComponent<MeshRenderer>() != null)
            {
                c.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else if (c.transform.childCount >= 1 && c.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>() != null)
            {
                c.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else
            {
                Debug.LogError("If you are seeing this, than the material is not finding a mesh renderer on the floor gameObject or it's child. Double Click this Error to open the Script In Visual Studio. Edit either the code above this error to fit your renderer, or use the code commented below to select the child of the index you want.");
                //f.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;

            }

        }

        foreach (GameObject c in CornerSpotsUsed)
        {
            if (c.GetComponent<MeshRenderer>() != null)
            {
                c.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else if (c.transform.childCount >= 1 && c.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>() != null)
            {
                c.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else
            {
                Debug.LogError("If you are seeing this, than the material is not finding a mesh renderer on the floor gameObject or it's child. Double Click this Error to open the Script In Visual Studio. Edit either the code above this error to fit your renderer, or use the code commented below to select the child of the index you want.");
                //f.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;

            }

        }

        foreach (GameObject cb in CenterAreaBlocks)
        {
            if (cb.GetComponent<MeshRenderer>() != null)
            {
                cb.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else if (cb.transform.childCount >= 1 && cb.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>() != null)
            {
                cb.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else
            {
                Debug.LogError("If you are seeing this, than the material is not finding a mesh renderer on the floor gameObject or it's child. Double Click this Error to open the Script In Visual Studio. Edit either the code above this error to fit your renderer, or use the code commented below to select the child of the index you want.");
                //f.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;

            }
        }

        foreach (GameObject d in Doorways)
        {
            if (d.GetComponent<MeshRenderer>() != null)
            {
                d.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else
            {
                Debug.LogError("If you are seeing this, than the material is not finding a mesh renderer on the floor gameObject or it's child. Double Click this Error to open the Script In Visual Studio. Edit either the code above this error to fit your renderer, or use the code commented below to select the child of the index you want.");
                //d.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;

            }

        }

        foreach(GameObject w in WallBlocks)
        {
            if(w.transform.childCount >= 1)
            {
                if (w.GetComponent<MeshRenderer>() != null)
                {
                    w.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
                }
                else
                {
                    Debug.LogError("If you are seeing this, than the material is not finding a mesh renderer on the floor gameObject or it's child. Double Click this Error to open the Script In Visual Studio. Edit either the code above this error to fit your renderer, or use the code commented below to select the child of the index you want.");
                    //d.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;

                }
            }
        }
    }

    public void ChangeAllWallMaterials()
    {
       

        foreach (GameObject w in WallBlocks)
        {
            
            if (w.transform.childCount >= 1 && w.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>() != null)
            {
                w.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else if (w.GetComponent<MeshRenderer>() != null)
            {
                w.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;
            }
            else
            {
                Debug.LogError("If you are seeing this, than the material is not finding a mesh renderer on the floor gameObject or it's child. Double Click this Error to open the Script In Visual Studio. Edit either the code above this error to fit your renderer, or use the code commented below to select the child of the index you want.");

                //w.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = NewMateraialForThisRoom;

                


            }

        }

    }

    public void ChangeCornerObjectsAll()
    {

        List<GameObject> tempHoldList = new List<GameObject>();
        List<GameObject> objectsToDelete = new List<GameObject>();

        CornerObjectAmount = cornerObjects.Count;
        foreach(GameObject i in cornerObjects)
        {
            //Get Old floor block and it's position
            GameObject oldFloor = null;
            foreach(GameObject t in CornerSpotsUsed)
            {
                Vector3 objectXAndY = new Vector3(i.transform.position.x, 0, i.transform.position.z);

                if(t.transform.position.x == objectXAndY.x || t.transform.position.z == objectXAndY.z)
                {
                    oldFloor = t;
                    break;
                }


            }
            Vector3 floorPos = oldFloor.transform.position;

            GameObject oldCornerObject = i;
            //Vector3 cornerObjPos;

            //Get the new game block
            GameObject newCornerObj = null;
            if (newCornerObjects.Count < 1)
            {
                int r = Random.Range(0, BlockyGrid.CornerObjects.Count);
                newCornerObj = Instantiate(BlockyGrid.CornerObjects[r].gameObject, transform.position, transform.rotation);
            }
            else if (newCornerObjects.Count >= 1)
            {
                int r = Random.Range(0, newCornerObjects.Count);
                newCornerObj = Instantiate(newCornerObjects[r].gameObject, transform.position, transform.rotation);
            }
            else
            {
                Debug.LogError("No new block selected");
                return;
            }


            float cornerY = newObjectYPosition(newCornerObj);

            //Assign Position, name, parent, and add to temp list
             newCornerObj.transform.position = new Vector3(i.transform.position.x, cornerY, i.transform.position.z);

            //Name the corner
            newCornerObj.name = "CornerObject";

            //newFloor.name = "Floor";
            newCornerObj.transform.parent = oldCornerObject.transform.parent;

            tempHoldList.Add(newCornerObj);

            //Get rid of old object
            oldCornerObject.SetActive(false);

            //cornerObjects.Remove(oldCornerObject);
            objectsToDelete.Add(oldCornerObject);

        }

        for(int i = 0; i < objectsToDelete.Count; i++)
        {
            DestroyImmediate(objectsToDelete[i]);
        }

        cornerObjects.Clear();


        foreach(GameObject cO in tempHoldList)
        {
            cornerObjects.Add(cO);
        }


    }
    public void CornerSpotAdd() //Call this after the room controllers have been added and populated 
    {
        for(int i = 0; i < CornerObjectAmount; i++)
        {
            if (CornerSpots.Count > 0 && BlockyGrid.CornerObjects.Count >= 1)
            {
                int rando = Random.Range(0, CornerSpots.Count);
                GameObject floorSelect = CornerSpots[rando];
                GameObject newObject = null;

                if(newCornerObjects.Count < 1)
                {
                    //Get, instantiate, position, parent, rename, and add to list, new obstacle
                    int ranObs = Random.Range(0, BlockyGrid.CornerObjects.Count);
                    newObject = BlockyGrid.CornerObjects[ranObs];
                }
                else if (newCornerObjects.Count >= 1)
                {
                    //Get, instantiate, position, parent, rename, and add to list, new obstacle
                    int ranObs = Random.Range(0, cornerObjects.Count);
                    newObject = cornerObjects[ranObs];
                }
                else
                {
                    Debug.LogError("No corner objects found to place.");

                }

                if(newObject != null)
                {
                    //Get a Y position
                    float newPosY = newObjectYPosition(newObject);

                    newObject = Instantiate(newObject.gameObject, transform.position, transform.rotation);

                    Vector3 floorPos = new Vector3(floorSelect.transform.position.x, newPosY, floorSelect.transform.position.z);

                    newObject.transform.position = floorPos;
                    newObject.transform.parent = floorSelect.transform.parent.transform;
                    newObject.name = "CornerObject";
                    cornerObjects.Add(newObject);
                    CornerSpots.Remove(floorSelect);
                    CornerSpotsUsed.Add(floorSelect);
                }

                


            }
        }
    }
    public void RemoveCornerObejcts()
    {
        List<GameObject> objectsToDelete = new List<GameObject>();

        for (int i = 0; i < CornerObjectAmount; i++)
        {
            if (cornerObjects.Count > 0)
            {
                //Get Random floor block and position
                int rando = Random.Range(0, cornerObjects.Count);
                GameObject cornerObjSelect = cornerObjects[rando];


                objectsToDelete.Add(cornerObjSelect);
                cornerObjSelect.SetActive(false);

                cornerObjects.Remove(cornerObjSelect);

                GameObject floorCornerBlock = null;

                foreach(GameObject corUsed in CornerSpotsUsed)
                {
                    if(corUsed.transform.position.x == cornerObjSelect.transform.position.x && corUsed.transform.position.z == cornerObjSelect.transform.position.z)
                    {
                        floorCornerBlock = corUsed;

                    }
                }

                CornerSpotsUsed.Remove(floorCornerBlock);
                CornerSpots.Add(floorCornerBlock);

            }


        }

        for (int i = 0; i < objectsToDelete.Count; i++)
        {
            DestroyImmediate(objectsToDelete[i].gameObject);
        }


    }


    public void RemoveCenterBlocks()
    {
        for(int i = 0; i < CenterAreaBlocks.Count; i++)
        {
            CenterAreaBlocks[i].gameObject.name = "Floor";
            FloorBlocks.Add(CenterAreaBlocks[i].gameObject);
        }

        CenterAreaBlocks.Clear();
    }


    //Function for checking if the doorways need rotating when changing, after they have been built
    public bool CheckDoorwayWhenChanging_DoWeNeedToTurn_AfterBuilt(GameObject doorwayToChange)
    {
        Vector3 tPosXAdd = new Vector3(doorwayToChange.transform.position.x + BlockyGrid.blockSizeX, 0, doorwayToChange.transform.position.z);
        Vector3 tPosXMinus = new Vector3(doorwayToChange.transform.position.x - BlockyGrid.blockSizeX, 0, doorwayToChange.transform.position.z);

        Vector3 tPosXAdd2 = new Vector3(doorwayToChange.transform.position.x - (BlockyGrid.blockSizeX * 2), 0, doorwayToChange.transform.position.z);
        Vector3 tPosXMinus2 = new Vector3(doorwayToChange.transform.position.x + (BlockyGrid.blockSizeX * 2), 0, doorwayToChange.transform.position.z);

        bool tDoor = false;

        foreach (GameObject blocky in Doorways)
        {

            if (blocky.transform.position == tPosXAdd || blocky.transform.position == tPosXAdd2)
            {
                /*if (blocky.gameObject.name == "Def" || blocky.gameObject.name == "T" || blocky.gameObject.name == "B" || blocky.gameObject.name == "L" || blocky.gameObject.name == "R" || blocky.name == "Wall")
                {
                    tDoor = true;
                    return true;
                }*/
                tDoor = true;
                return true;
            }
            else if (blocky.transform.position == tPosXMinus || blocky.transform.position == tPosXMinus)
            {
                /*if (blocky.gameObject.name == "Def" || blocky.gameObject.name == "T" || blocky.gameObject.name == "B" || blocky.gameObject.name == "L" || blocky.gameObject.name == "R" || blocky.name == "Wall")
                {
                    tDoor = true;
                    return true;
                }*/

                tDoor = true;
                return true;
            }


            if (tDoor == true)
            {
                return true;
            }

        }

        if (tDoor)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    public float newObjectYPosition(GameObject newObject)
    {
        float posY = 0;
        if (BlockyGrid.basePivots)
        {
            posY = BlockyGrid.blockSizeY; ;
        }
        else
        {
            if (newObject.transform.GetComponent<Renderer>() != null)
            {
                posY = (BlockyGrid.blockSizeY * 0.5f) + (newObject.transform.GetComponent<Renderer>().bounds.size.y * 0.5f);
            }
            else
            {
                //Just run through all the child objects unitl we get a renderer
                for (int it = 0; it < newObject.transform.childCount; it++)
                {
                    if (newObject.transform.GetChild(it).GetComponent<Renderer>() != null)
                    {
                        posY = (newObject.transform.GetChild(it).GetComponent<Renderer>().bounds.size.y * 0.5f) + (BlockyGrid.blockSizeY * 0.5f);
                        break;
                    }
                }

                //Check to see if we haven't got a value
                if (posY == 0)
                {
                    posY = (newObject.transform.lossyScale.y * 0.5f) + (BlockyGrid.blockSizeY * 0.5f);
                }
            }
        }

        return posY;
    }

}
