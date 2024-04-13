using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


public class PGBlockGridLevel : MonoBehaviour
{
    //For anybody who has decided to read and/or adjust this script, I apologise in advance for all the repetive/terrible code :)



    // This region contains all the variables.
    // Why is it in a region?
    // Simply so I could collapse all the variables. Enjoy.
    #region

    //Bool for if we are using the ediotr to build rooms or not
    [Header("Editor Mode")]
    public bool EditorMode = true;

    //Bool for ending the stopwatch
    private bool stopWatch;

    //Name for newPreset
    //public string PresetName;

    //This is for level
    [Header("How Many Rooms?")]
    [HideInInspector] public int HowManyRooms = 15;
    private int placedRooms;
    [HideInInspector]public float spinRoom;
    [HideInInspector] public bool Hori, corridorOrNot;
    [HideInInspector] public bool mUp, mDown, mLeft, mRight, alreadyBuiltOne;
    [HideInInspector] public float blockSizeX;
    [HideInInspector] public float blockSizeZ;
    [HideInInspector] public float blockSizeY;

    //private int enemyBlockCounter;

    [HideInInspector] public List<GameObject> AllBlocksInUse = new List<GameObject>();

    [Header("Doorway")]
    private List<GameObject> Doorways = new List<GameObject>();
    private List<GameObject> DoorwaysL = new List<GameObject>();
    private List<GameObject> DoorwaysR = new List<GameObject>();
    private List<GameObject> DoorwaysT = new List<GameObject>();
    private List<GameObject> DoorwaysB = new List<GameObject>();

    //TempDoorways and TempDoorways spins are used in deciding doorways and room placements
    private List<GameObject> TempDoorways = new List<GameObject>();
    private List<float> TempDoorwaySpins = new List<float>();

    //Check Entrances is used to check doorways to ensure they are not blocked by walls
    [HideInInspector] public List<GameObject> checkEntrances = new List<GameObject>();
    //Deactivated holds all the blocks that overlap (the last function deletes all these objects)
    [HideInInspector] public List<GameObject> Deactivated = new List<GameObject>();
    //All Blocks Contains all the block in the game
    [HideInInspector] public List<GameObject> AllBlocks = new List<GameObject>();
    //List of the rooms being created
    private List<GameObject> Rooms = new List<GameObject>();


    [Header("Block Count For Corridors")]
    [Tooltip("Remember, with the walled corridors, 2 block will be added either side of the width amount. Also, try to keep the Max width to half of the Min Room Width/Size to stop any corridor misshaps.")]
    [HideInInspector] public int MinWidthCorridor = 2;
    [HideInInspector] public int MaxWidthCorridor = 4;
    private int CorWCount;
    [HideInInspector] public int MinLengthCorridor = 9;
    [HideInInspector] public int MaxLengthCorridor = 15;
    private int CorLCount;


    [HideInInspector] public GameObject CorridorCreated;
    [HideInInspector] public float blockLengthX;
    [HideInInspector] public float blockLengthZ;

    //This is all for single rooms
    [Header("Block Count For Single Room")]
    [HideInInspector] public int MinWidthRoom = 12;
    [HideInInspector] public int MaxWidthRoom = 15;
    private int RoomWCount;
    [HideInInspector] public int MinLenghtRoom = 12;
    [HideInInspector] public int MaxLengthRoom = 15;
    private int RoomLCount;

    //How many blocks in total
    [HideInInspector] public int combinedBlockCount;

    [Header("Space between blocks.")]
    [HideInInspector] public float spacing = 0;



    //Corner Spots Variables
    [HideInInspector] public List<GameObject> CornerObjects = new List<GameObject>();
    [HideInInspector] public int cornerFillAmountMin;
    [HideInInspector] public int cornerFillAmountMax;

    


    [Header("Parent object for all objects:")]
    [HideInInspector] public GameObject LevelContainer;


    [Header("Floor Block:")]
    [HideInInspector] public GameObject FloorBlock;

    [Header("Wall Block:")]
    [HideInInspector] public GameObject WallBlock;

    [Header("Doorway Block")]
    [HideInInspector] public GameObject DoorwayBlock;
    private bool turnDoor;

    [Header("Loading Screen:")]
    [HideInInspector] public GameObject loadingScreen;

    [Header("Player:")]
    [HideInInspector] public GameObject Player;
    [HideInInspector] public GameObject StartBlock;
    private bool playerIn;

    [Header("Big Boss Enemy:")]
    [HideInInspector] public GameObject BigBossEnemy;
    [HideInInspector] public bool bigBossRound;

    [Header("Finish Line:")]
    [HideInInspector] public GameObject lastObject;

    //[Header("Empty object to make into room:")]
    [HideInInspector] public GameObject ParentObj;

    //[Space(30)]

    [Header("Easy to access script added to rooms")]
    [HideInInspector] public bool AddRoomControls = true;
    private bool roomControlsAdded;

    [Header("Do you want NO obstacles in the Start Room?")]
    [HideInInspector] public bool StartRoomEmpty = false;

    [Header("Do you want no Corridor Walls")]
    [HideInInspector] public bool NoCorridorWalls = false;

    [Header("Overlapping Walls")]
    [HideInInspector] public bool OverlappingBlocks = false;

    [Header("Overlapping Walls Scattered")]
    [HideInInspector] public bool ScatteredWalls = false;

    [Header("Corridors Have Seperate Parents")]
    [HideInInspector] public bool SeperateCorridorParents = true;
    private int corridorParentCounter;

    [Header("Base Pivots")]
    [HideInInspector]public bool basePivots = true;

    //Center Pieces
    [HideInInspector]public bool CenterPieces;
    
    

    //[Space(30)]


    //[Header("Enemies:")]
    [HideInInspector] public List<GameObject> Enemies = new List<GameObject>();
    [HideInInspector] public int minEnemyPerRoom, maxEnemyPerRoom;
    private List<GameObject> EnemiesAddedIn = new List<GameObject>();


    //[Header("Items:")]
    [HideInInspector] public List<GameObject> Items = new List<GameObject>();
    private List<GameObject> ItemsAddedIn = new List<GameObject>();
    [HideInInspector] public int minItemPerRoom, maxItemPerRoom;


    //[Header("Rare Items:")]
    [HideInInspector] public List<GameObject> RareItems = new List<GameObject>();


    [Header("Chance % of item being a rare one")]
    [HideInInspector] public int rareChance;

    //Center Pieces 
    [HideInInspector]public List<GameObject> CenterPieceObjects = new List<GameObject>();
    [HideInInspector]public int ChanceOfPlacingCenterPiece;


    //[Header("Obstacle Blocks:")]
    [HideInInspector] public List<GameObject> Obstacles = new List<GameObject>();
    [HideInInspector] public int obstaclePerRoom;

    //[Header("")]

    //[Header("Buildings")]
    /*[HideInInspector] public List<GameObject> Buildings = new List<GameObject>();*/

    //Bool for last checks
    private bool lastEntrancesCheked;
    private bool lastHolesAndExtraBlocksChecked;
    private bool lastCheckChecked;
    private bool CorridorWallsChecked;



    // FOR ROOM/CORRIDOR ALIGMENT //

    //Positions of blocks
    private float oldPosX;
    private float oldPosZ;

    //Position of last placed corridor
    //private Vector3 oldCorPos;

    //How many blocks have been placed
    private int tempCountZ;
    private int tempCountX;

    //Last Room and Doorway
    [HideInInspector] public GameObject lastRoom;
    [HideInInspector] public GameObject lastDoorway;

    //Other Doorway
    [HideInInspector] public GameObject doorwayOne;

    //Last block placed
    private GameObject lastBlock;

    //To minus from the list of rooms count when selecting the next room to grow corridors from
    private int chooseRoom;

    #endregion


    //[Space(30)]

    //[Header("Variables For Already Built Rooms Changes")]

    //Variable for already built room Controls
    //[Header("New Floor Block")]
    [HideInInspector] public GameObject newFloorBlock;
    //[Header("New Wall Block")]
    [HideInInspector] public GameObject newWallBlock;
    [Header("New Obstacles Amount")]
    [HideInInspector] public int newObstaclesAmount;
    [Header("New Door Block")]
    [HideInInspector] public GameObject newDoorObject;
    //[Header("New Obstacles List")]
    [HideInInspector] public List<GameObject> NewObstacles = new List<GameObject>();
    //CornerObjects
    [HideInInspector] public List<GameObject> NewCornerObjects = new List<GameObject>();
    [HideInInspector] public int CornerObjectChangeAmount;
    //Materials
    //[Header("New Material For Channing Room and Floors")]
    [HideInInspector] public Material NewMaterial;


    //Start function to be used if editor mode is off and no rooms have been built yet.
    public void Start()
    {
        if (!EditorMode)
        {
            StartCoroutine(LevelBuilder());
        }

        StartBullding();
    }

    //Function called when Add Block is clicked in Editor Mode.
    public void StartBullding()
    {

        StartCoroutine(LevelBuilder());



    }

    //Coroutine that runs until all is complete.
    IEnumerator LevelBuilder()
    {

        if(HowManyRooms <= 0)
        {
            HowManyRooms = 5;
        }

        if(LevelContainer != null)
        {
            ClearAll();
        }
        else
        {
            LevelContainer = new GameObject();
            LevelContainer.gameObject.name = "Level";
        }

        stopWatch = true;
        StartCoroutine(TimeCheck());

        if (!EditorMode && loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        //Turn off rooms controls and Seperate corridor parents - they work togheter, but don't create any desirable outcome
        if (OverlappingBlocks == true)
        {
            AddRoomControls = false;
            SeperateCorridorParents = false;
        }

        LevelContainer.transform.position = new Vector3(0, 0, 0);

        AllBlocksInUse.Clear();

        ParentObj = new GameObject();
        ParentObj.transform.parent = LevelContainer.transform;
        Deactivated.Add(ParentObj);


        Debug.Log("Coroutine Started.");
        lastCheckChecked = false;
        lastEntrancesCheked = false;
        lastHolesAndExtraBlocksChecked = false;
        CorridorWallsChecked = false;

        placedRooms = 0;
        corridorParentCounter = 0;


        blockSizeX = FloorBlock.GetComponent<Renderer>().bounds.size.x + spacing;

        blockSizeZ = FloorBlock.GetComponent<Renderer>().bounds.size.z + spacing;
        blockSizeY = FloorBlock.GetComponent<Renderer>().bounds.size.y;

        //Place everyhting
        while (placedRooms < HowManyRooms)
        {
            BuildRoom();
            if (!EditorMode)
            {
                yield return new WaitForSeconds(0.05f);
            }
        }

        //Debug.Log("Building Finished");

        //Debug.Log("Last Checks Started");

        if (!EditorMode)
        {
            yield return new WaitForSeconds(0.05f);
        }

        foreach (Transform child in LevelContainer.transform)
        {
            foreach (Transform blocky in child.transform)
            {
                if (blocky.gameObject.activeSelf)
                {
                    AllBlocksInUse.Add(blocky.gameObject);
                }
            }
        }

        if (!EditorMode)
        {
            yield return new WaitForSeconds(0.05f);
        }

        while (lastEntrancesCheked == false)
        {
            ClearEntrancesIfObstructed();
            if (!EditorMode)
            {
                yield return new WaitForSeconds(1f);
            }
        }

        if (!EditorMode)
        {
            yield return new WaitForSeconds(0.05f);
        }

        //Debug.Log("Entrances Completed");

        //Do big final check to clean up mess
        if (OverlappingBlocks == true)
        {
            AllBlocksInUse.Clear();
            foreach (Transform child in LevelContainer.transform)
            {
                foreach (Transform blocky in child.transform)
                {
                    if (blocky.gameObject.activeSelf)
                    {
                        AllBlocksInUse.Add(blocky.gameObject);
                    }
                }
            }

            while (lastHolesAndExtraBlocksChecked == false)
            {
                CheckNoHolesInWallsAndNoExtraWalls();
                if (!EditorMode)
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }

            AllBlocksInUse.Clear();
            foreach (Transform child in LevelContainer.transform)
            {
                foreach (Transform blocky in child.transform)
                {
                    if (blocky.gameObject.activeSelf)
                    {
                        AllBlocksInUse.Add(blocky.gameObject);
                    }
                }
            }

            while (CorridorWallsChecked == false)
            {
                CheckCorridorWalls();
                if (!EditorMode)
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }

            if (!EditorMode)
            {
                yield return new WaitForSeconds(0.05f);
            }

        }
        else if (OverlappingBlocks == false && NoCorridorWalls == false)
        {
            /*while (CorridorWallsChecked == false)
            {
                CheckForHoles();
                if (!EditorMode)
                {
                    yield return new WaitForSeconds(0.1f);
                }

            }*/
        }




        //PlaceBuilding(Buildings[0]);
        //yield return new WaitForSeconds(0.05f);
        //Debug.Log("Holes and Extra Walls Completed");

        //Run the last checks (parenting, deleting, end room objs, e.t.c)



        if (AddRoomControls == true)
        {
            AddRoomControlScriptAndPopulate();
            if (!EditorMode)
            {
                yield return new WaitForSeconds(0.25f);
            }
        }




        while (lastCheckChecked == false)
        {
            LastChecks();
            if (!EditorMode)
            {
                yield return new WaitForSeconds(0.1f);
            }
            //yield return new WaitForSeconds(0.5f);
        }



        //Debug.Log("Last Checks Completed");

        yield return new WaitForSeconds(0.05f);


        if (!EditorMode && loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }


        stopWatch = false;

        StopCoroutine(LevelBuilder());

        //Debug.Log("Coroutine Finished.");

        ClearLog();
        stopWatch = false;

    }

    IEnumerator TimeCheck()
    {
        float counter = 0;
        while (stopWatch == true)
        {
            yield return new WaitForSeconds(0.1f);
            counter += 0.1f;
        }
        string rounded = counter.ToString("F2");
        int s1 = MaxWidthRoom - MinWidthRoom;
        int s2 = MinWidthRoom + s1;
        int s3 = MaxLengthRoom - MinLenghtRoom;
        int s4 = MinLenghtRoom + s3;
        int averageRoomSize = s2 * s4;
        

        Debug.Log(HowManyRooms + " Rooms || Average Room Size:" + averageRoomSize + " Blocks || Build & Clean: " + rounded + "s");
        
        yield return new WaitForSeconds(1f);

        StopCoroutine(TimeCheck());

    }

    //

    //Codes for building a room.
    public void BuildRoom()
    {

        float posX = 0;
        float posZ = 0;

        tempCountX = 0;
        tempCountZ = 0;

        List<GameObject> blocks = new List<GameObject>();

        GameObject tParent;

        if (TempDoorways.Count >= 1)
        {
            Vector3 RoomPositon = new Vector3(TempDoorways[0].transform.position.x, 0, TempDoorways[0].transform.position.z);
            tParent = Instantiate(ParentObj, RoomPositon, transform.rotation);
        }
        else
        {
            tParent = Instantiate(ParentObj, LevelContainer.transform.position, transform.rotation);
        }

        //Center Pieces Chance
        bool placeCenterPiece = false;
        int chanceOfCenterPiece = Random.Range(1, 11);
        if(chanceOfCenterPiece <= ChanceOfPlacingCenterPiece && chanceOfCenterPiece > 0 && CenterPieces == true)
        {
            placeCenterPiece = true;
        }
        else
        {
            placeCenterPiece = false;
        }

        tParent.name = "Room " + (placedRooms + 1);


        RoomWCount = Random.Range(MinWidthRoom, MaxLengthRoom);
        RoomLCount = Random.Range(MinLenghtRoom, MaxLengthRoom);

        int tempLengthZ = (int)(RoomLCount * 0.5f); //2 because of the two corner blocks
        int tempLengthX = (int)(RoomWCount * 0.5f);

        combinedBlockCount = RoomWCount * RoomLCount;

        List<GameObject> tempObstacleList = new List<GameObject>();

        List<GameObject> CornerSpots = new List<GameObject>();

        if (lastBlock != null)
        {
            lastBlock = lastRoom.transform.GetChild(lastRoom.transform.childCount - 1).gameObject;
        }

        //Build Start Room Code
        if (placedRooms == 0)
        {
            //int startPointTemp = Mathf.RoundToInt(combinedBlockCount / (combinedBlockCount / 2));
            Vector3 playerStartPointHold = new Vector3(0, 0, 0);

            int obstacletempCount = 0;
            int sinceLastObstacle = 0;

            while (blocks.Count <= combinedBlockCount - 1)
            {

                if (blocks.Count == 0)
                {
                    corridorOrNot = false;

                    Vector3 thePosition = new Vector3(0, 0, 0);

                    //CheckTheEntrances(thePosition);

                    GameObject newB = Instantiate(WallBlock, transform.position, transform.rotation);
                    newB.name = "Def";

                    blocks.Add(newB);


                    newB.transform.position = thePosition;



                    Doorways.Add(newB);


                    oldPosZ = newB.transform.position.z;
                    oldPosX = 0;
                    tempCountZ += 1;

                }
                else if (tempCountZ < RoomLCount)
                {
                    corridorOrNot = false;

                    int placeObst = Random.Range(0, (int)(Mathf.Sqrt((float)combinedBlockCount)));

                    int randoPlayerPlacement = Random.Range(0, 7);

                    float roundedZ = Mathf.Round(oldPosZ + blockSizeZ);

                    //Change the blocks position to the old position we registered when the first/last block was placed
                    Vector3 thePosition = new Vector3(oldPosX, 0, roundedZ);

                    //CheckTheEntrances(thePosition);

                    //Add blocks to the Z axis
                    GameObject newB;
                    //For getting bottom boxes
                    if (/*!corridorOrNot &&*/ tempCountX <= 0)
                    {
                        newB = Instantiate(WallBlock, transform.position, transform.rotation);
                        DoorwaysL.Add(newB);
                        newB.name = "L";
                    }
                    //Getting top boxes
                    else if (/*!corridorOrNot &&*/ tempCountX == RoomWCount - 1)
                    {
                        newB = Instantiate(WallBlock, transform.position, transform.rotation);
                        DoorwaysR.Add(newB);
                        newB.name = "R";
                    }
                    //Getting left side boxes
                    else if (/*!corridorOrNot &&*/ tempCountZ == 0)
                    {
                        newB = Instantiate(WallBlock, transform.position, transform.rotation);
                        DoorwaysB.Add(newB);
                        newB.name = "B";
                    }
                    //Getting right side boxes
                    else if (/*!corridorOrNot &&*/ tempCountZ == RoomLCount - 1)
                    {
                        newB = Instantiate(WallBlock, transform.position, transform.rotation);
                        DoorwaysT.Add(newB);
                        newB.name = "T";
                    }
                    else if (/*!corridorOrNot &&*/ playerIn == false)
                    {
                        playerStartPointHold = thePosition;

                        if (Player != null && randoPlayerPlacement == 4)
                        {
                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                            newB.transform.position = thePosition;
                            float playerY = Player.GetComponent<Renderer>().bounds.size.y;
                            Vector3 playerPos = new Vector3(thePosition.x, playerY, thePosition.z);

                            GameObject playerAdd = Instantiate(Player, playerPos, transform.rotation);

                            ItemsAddedIn.Add(playerAdd);

                            //newB.transform.GetChild(0).transform.parent = null;

                            newB.name = "Floor";
                            playerIn = true;
                        }
                        else if (Player != null && randoPlayerPlacement != 4)
                        {
                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                            newB.name = "Floor";
                        }
                        else
                        {
                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                            newB.name = "Floor";
                            playerIn = true;
                        }

                    }
                    else if (StartRoomEmpty == false)
                    {
                        //For enemy creation and placement / Item creation and placement
                        if (obstaclePerRoom > 0)
                        {
                            int tempSpacing = combinedBlockCount / obstaclePerRoom;

                            if (obstacletempCount < obstaclePerRoom && Obstacles.Count >= 1 && placeObst <= 5 && sinceLastObstacle >= tempSpacing/*(blocks.Count >= tempSpacing && blocks.Count <= (combinedBlockCount - tempSpacing))*/)
                            {


                                if (Obstacles.Count > 1)
                                {
                                    int rand = Random.Range(0, Obstacles.Count);
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "Floor";
                                    GameObject newObst = Instantiate(Obstacles[rand], transform.position, transform.rotation);
                                    float newObstY = 0;
                                    if(newObst.GetComponent<Renderer>() != null)
                                    {
                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                    }
                                    else
                                    {
                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                    }


                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                    tempObstacleList.Add(newObst);
                                    newObst.name = "Obstacle";
                                    obstacletempCount += 1;
                                }
                                else
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "Floor";
                                    GameObject newObst = Instantiate(Obstacles[0], transform.position, transform.rotation);

                                    float newObstY = 0;
                                    if (newObst.GetComponent<Renderer>() != null)
                                    {
                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                    }
                                    else
                                    {
                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                    }
                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);

                                    tempObstacleList.Add(newObst);
                                    newObst.name = "Obstacle";
                                    obstacletempCount += 1;
                                }

                                sinceLastObstacle = 0;
                            }
                            else
                            {
                                newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                newB.name = "Floor";
                                sinceLastObstacle += 1;
                            }
                        }
                        else
                        {
                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                            newB.name = "Floor";
                            sinceLastObstacle += 1;
                        }
                    }
                    else
                    {
                        newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                        newB.name = "Floor";
                        sinceLastObstacle += 1;
                    }




                    blocks.Add(newB);

                    //This new block is now the last block we have created
                    lastBlock = newB;

                    //Change the blocks position to the old position we registered when the first/last block was placed
                    newB.transform.position = thePosition;


                    //Getting each corner
                    if (tempCountX == 0 && tempCountZ == RoomLCount - 1 || tempCountX == RoomWCount - 1 && tempCountZ == 0)
                    {
                        if (DoorwaysB.Contains(newB))
                        {
                            DoorwaysB.Remove(newB);
                            newB.name = "Def";
                        }
                        else if (DoorwaysT.Contains(newB))
                        {
                            DoorwaysT.Remove(newB);
                            newB.name = "Def";
                        }
                        else if (DoorwaysR.Contains(newB))
                        {
                            DoorwaysR.Remove(newB);
                            newB.name = "Def";
                        }
                        else if (DoorwaysL.Contains(newB))
                        {
                            DoorwaysL.Remove(newB);
                            newB.name = "Def";
                        }
                    }
                    else if (tempCountX == RoomWCount - 1 && tempCountZ == RoomLCount - 1)
                    {
                        DoorwaysR.Remove(newB);
                        newB.name = "Def";
                    }






                    //Assign the new blocks positon on the z-axis for the next block to use
                    oldPosZ = newB.transform.position.z;
                    //Add one to our z counter
                    tempCountZ += 1;

                }
                else if (tempCountZ >= RoomLCount)
                {

                    tempCountZ = 0;
                    //Add one to our x counter to move up when level
                    tempCountX += 1;
                    //Reset our x positon back to 0, with the block's size taken into count
                    oldPosZ = 0 - blockSizeZ;
                    //Get our boxes last x axis (like its height) and add the size of our block to that variable to get our next level of blocks
                    oldPosX = Mathf.Round(lastBlock.transform.position.x + blockSizeX);

                }


            }
            if (blocks.Count > combinedBlockCount - 1) //Room finished building
            {
                if (Player != null && playerIn == false)
                {
                    float playerY = Player.GetComponent<Renderer>().bounds.size.y;
                    Vector3 playerPos = new Vector3(playerStartPointHold.x, 2, playerStartPointHold.z);

                    GameObject playerAdd = Instantiate(Player, playerPos, transform.rotation);

                    ItemsAddedIn.Add(playerAdd);
                }

                if (tParent != null)
                {
                    foreach (GameObject blok in blocks)
                    {
                        blok.transform.parent = tParent.transform;

                    }
                }

                foreach (GameObject obs in tempObstacleList)
                {
                    obs.transform.parent = tParent.transform;
                }

                Rooms.Add(tParent);
                tParent.transform.parent = LevelContainer.transform;

                //tParent.transform.position = new Vector3(0, 0, 0);

                lastRoom = tParent;

                alreadyBuiltOne = false;
                CorridorSelection(tParent);


            }

            placedRooms += 1;



        }
        else if (placedRooms <= HowManyRooms - 1)
        {
            //Debug.Log(TempDoorways.Count);   
            if (TempDoorways.Count >= 1 && TempDoorwaySpins.Count >= 1)
            {
                lastBlock = TempDoorways[0];
                spinRoom = TempDoorwaySpins[0];
                GameObject lastParent = lastBlock.transform.parent.gameObject;



                chooseRoom = 1;

                int obstacletempCount = 0;
                int sinceLastObstacle = 0;

                int enemiesToPlace = Random.Range(minEnemyPerRoom, maxEnemyPerRoom + 1);


                switch (spinRoom)
                {
                    case 0:
                        while (blocks.Count <= combinedBlockCount - 1)
                        {
                            if (blocks.Count == 0)
                            {
                                corridorOrNot = false;
                                //float startPosTemp = Mathf.Sqrt(blockLengthZ);
                                float rn = Random.Range(0.4f, 0.6f);
                                float startPosTemp = RoomWCount * rn;
                                float startPosRound = Mathf.Round(startPosTemp);
                                float startPosZ = startPosRound * blockSizeX;

                                Vector3 thePosition = new Vector3(lastBlock.transform.position.x - startPosZ, 0, lastBlock.transform.position.z + blockSizeZ);


                                //CheckTheEntrances(thePosition);



                                GameObject newB;

                                newB = Instantiate(WallBlock, transform.position, transform.rotation);

                                /*if (!corridorOrNot)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                }
                                else
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                }
                                */
                                newB.name = "Wall";




                                blocks.Add(newB);
                                Doorways.Add(newB);

                                newB.transform.position = thePosition;




                                posX = newB.transform.position.x;
                                posZ = newB.transform.position.z;


                                oldPosX = posX;
                                oldPosZ = posZ;
                                tempCountZ += 1;

                            }


                            else if (tempCountZ < RoomLCount)
                            {
                                corridorOrNot = false;

                                int placeObst = Random.Range(0, (int)(Mathf.Sqrt((float)combinedBlockCount)));



                                float roundedX = Mathf.Round(posZ + blockSizeZ);

                                //Change the blocks position to the old position we registered when the first/last block was placed
                                Vector3 thePosition = new Vector3(posX, 0, roundedX);



                                GameObject newB;

                                //For getting bottom boxes
                                if (tempCountZ <= 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    newB.name = "Def";
                                }
                                //Getting top boxes
                                else if (tempCountZ == RoomLCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysT.Add(newB);
                                    newB.name = "T";

                                }
                                //Getting left side boxes
                                else if (tempCountX == 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysL.Add(newB);
                                    newB.name = "L";
                                }
                                //Getting right side boxes
                                else if (tempCountX == RoomWCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysR.Add(newB);
                                    newB.name = "R";
                                }
                                else if (tempCountX == tempLengthX && tempCountZ == tempLengthZ && OverlappingBlocks == false) //Center Block
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterBlock";
                                }
                                else if((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Bottom Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Top Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Bottom Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Top Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if((tempCountZ >= RoomLCount * 0.3f && tempCountZ <= RoomLCount - (RoomLCount * 0.3f)) && (tempCountX >= RoomWCount * 0.3f && tempCountX <= RoomWCount - (RoomWCount * 0.3f)) && placeCenterPiece == true)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterPiecesWhole";
                                }
                                else
                                {
                                    //Ensure we are not against a wall
                                    if (tempCountZ != RoomLCount - 2 && tempCountX != RoomWCount - 2 && tempCountX > 1 && tempCountZ > 1)
                                    {
                                        //For obstacle placement
                                        if (obstaclePerRoom > 0)
                                        {
                                            int tempSpacing = combinedBlockCount / obstaclePerRoom;

                                            if (obstacletempCount < obstaclePerRoom && Obstacles.Count >= 1 && placeObst <= 5 && sinceLastObstacle >= tempSpacing/*(blocks.Count >= tempSpacing && blocks.Count <= (combinedBlockCount - tempSpacing))*/)
                                            {


                                                if (Obstacles.Count > 1)
                                                {
                                                    int rand = Random.Range(0, Obstacles.Count);
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[rand], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, (newObstY), thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }
                                                else
                                                {
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[0], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }

                                                sinceLastObstacle = 0;
                                            }
                                            else
                                            {
                                                newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                newB.name = "Floor";
                                                sinceLastObstacle += 1;
                                            }
                                        }
                                        else
                                        {
                                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                            newB.name = "Floor";
                                            sinceLastObstacle += 1;
                                        }
                                    }
                                    else
                                    {
                                        newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                        newB.name = "Floor";
                                        sinceLastObstacle += 1;
                                    }

                                }






                                blocks.Add(newB);
                                //This new block is now the last block we have created
                                lastBlock = newB;

                                //Change the blocks position to the one checked above;
                                newB.transform.position = thePosition;


                                //Get the four corner blocks and remove them form the doorways lists

                                //Getting each corner
                                if (tempCountX == 0 && tempCountZ == RoomLCount - 1 || tempCountX == RoomWCount - 1 && tempCountZ == 0)
                                {
                                    if (DoorwaysB.Contains(newB))
                                    {
                                        DoorwaysB.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysT.Contains(newB))
                                    {
                                        DoorwaysT.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysR.Contains(newB))
                                    {
                                        DoorwaysR.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysL.Contains(newB))
                                    {
                                        DoorwaysL.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                }
                                else if (tempCountX == RoomWCount - 1 && tempCountZ == RoomLCount - 1)
                                {
                                    DoorwaysR.Remove(newB);
                                    newB.name = "Wall";
                                }
                                else if(tempCountX == 1 && tempCountZ == 1 || tempCountX == 1 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Left - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if(newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }
                                else if(tempCountX == RoomWCount - 2 && tempCountZ == 1 || tempCountX == RoomWCount - 2 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Right - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }
                                
                                //Assign the new blocks positon on the z-axis for the next block to use
                                posZ = newB.transform.position.z;


                                //Check we are not placing on top of other blocks
                                //CheckOverlap(newB);




                                //Add one to the Z count to move along a step
                                tempCountZ += 1;


                            }
                            else if (tempCountZ >= RoomLCount)
                            {

                                tempCountZ = 0;
                                //Add one to our x counter to move up when level
                                tempCountX += 1;
                                //Reset our x positon back to 0, with the block's size taken into count
                                posZ = Mathf.Round(oldPosZ - blockSizeZ);
                                //Get our boxes last x axis (like its height) and add the size of our block to that variable to get our next level of blocks
                                posX = Mathf.Round(posX + blockSizeX);

                            }
                        }
                        if (blocks.Count > combinedBlockCount - 1)
                        {
                            if (tParent != null)
                            {

                                foreach (GameObject blok in blocks)
                                {
                                    blok.transform.parent = tParent.transform;

                                }


                            }


                            if (OverlappingBlocks == true)
                            {
                                foreach (GameObject blok in blocks)
                                {
                                    CheckOverlap(blok);
                                }
                            }



                            PlaceItems(blocks);

                            PlaceEnemies(blocks);

                            Rooms.Add(tParent);

                            tParent.transform.parent = LevelContainer.transform;

                            lastRoom = tParent;



                            //BuildCorridor(tParent);
                        }
                        // Debug.Log(tParent.name + spinRoom);
                        placedRooms += 1;
                        break;

                    case 90:
                        while (blocks.Count <= combinedBlockCount - 1)
                        {
                            if (blocks.Count == 0)
                            {
                                corridorOrNot = false;
                                float rn = Random.Range(0.4f, 0.6f);
                                float startPosTemp = RoomLCount * rn;
                                float startPosRound = Mathf.Round(startPosTemp);
                                float startPosZ = startPosRound * blockSizeZ;

                                Vector3 thePosition = new Vector3(lastBlock.transform.position.x + blockSizeX, 0, lastBlock.transform.position.z - startPosZ);

                                //CheckTheEntrances(thePosition);

                                GameObject newB;
                                newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                newB.name = "Wall";

                                /*//Place the first block
                                if (!corridorOrNot)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    newB.name = "Wall";
                                }
                                else
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "Used Corridor";
                                }*/

                                blocks.Add(newB);

                                newB.transform.position = thePosition;

                                Doorways.Add(newB);


                                posX = newB.transform.position.x;
                                posZ = newB.transform.position.z;

                                oldPosX = posX;
                                oldPosZ = posZ;
                                tempCountZ += 1;

                            }


                            else if (tempCountZ < RoomLCount)
                            {
                                corridorOrNot = false;

                                int placeObst = Random.Range(0, (int)(Mathf.Sqrt((float)combinedBlockCount)));


                                float roundedZ = Mathf.Round(posZ + blockSizeZ);

                                //Change the blocks position to the old position we registered when the first/last block was placed
                                Vector3 thePosition = new Vector3(posX, 0, roundedZ);



                                GameObject newB;

                                //For getting bottom boxes
                                if (tempCountX == 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    newB.name = "Def";

                                }
                                //Getting top boxes
                                else if (tempCountX == RoomWCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysR.Add(newB);
                                    newB.name = "R";
                                }
                                //Getting left side boxes
                                else if (tempCountZ == 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysB.Add(newB);
                                    newB.name = "B";
                                }
                                //Getting right side boxes
                                else if (tempCountZ == RoomLCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysT.Add(newB);
                                    newB.name = "T";
                                }
                                else if (tempCountX == tempLengthX && tempCountZ == tempLengthZ)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterBlock";
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Bottom Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Top Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Bottom Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Top Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount * 0.3f && tempCountZ <= RoomLCount - (RoomLCount * 0.3f)) && (tempCountX >= RoomWCount * 0.3f && tempCountX <= RoomWCount - (RoomWCount * 0.3f)) && placeCenterPiece == true)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterPiecesWhole";
                                }
                                else
                                {
                                    //Ensure we are not against a wall
                                    if (tempCountZ != RoomLCount - 2 && tempCountX != RoomWCount - 2 && tempCountX > 1 && tempCountZ > 1)
                                    {
                                        //For obstacle placement
                                        if (obstaclePerRoom > 0)
                                        {
                                            int tempSpacing = combinedBlockCount / obstaclePerRoom;

                                            if (obstacletempCount < obstaclePerRoom && Obstacles.Count >= 1 && placeObst <= 5 && sinceLastObstacle >= tempSpacing/*(blocks.Count >= tempSpacing && blocks.Count <= (combinedBlockCount - tempSpacing))*/)
                                            {


                                                if (Obstacles.Count > 1)
                                                {
                                                    int rand = Random.Range(0, Obstacles.Count);
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[rand], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }


                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }
                                                else
                                                {
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[0], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }

                                                sinceLastObstacle = 0;
                                            }
                                            else
                                            {
                                                newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                newB.name = "Floor";
                                                sinceLastObstacle += 1;
                                            }
                                        }
                                        else
                                        {
                                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                            newB.name = "Floor";
                                            sinceLastObstacle += 1;
                                        }
                                    }
                                    else
                                    {
                                        newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                        newB.name = "Floor";
                                        sinceLastObstacle += 1;
                                    }

                                }


                                blocks.Add(newB);
                                //This new block is now the last block we have created
                                lastBlock = newB;

                                //Change the blocks position to the old position we registered when the first/last block was placed
                                newB.transform.position = thePosition;

                                //Assign the new blocks positon on the z-axis for the next block to use
                                posZ = newB.transform.position.z;

                                //Getting each corner
                                if (tempCountX == 0 && tempCountZ == RoomLCount - 1 || tempCountX == RoomWCount - 1 && tempCountZ == 0)
                                {
                                    if (DoorwaysB.Contains(newB))
                                    {
                                        DoorwaysB.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysT.Contains(newB))
                                    {
                                        DoorwaysT.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysR.Contains(newB))
                                    {
                                        DoorwaysR.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysL.Contains(newB))
                                    {
                                        DoorwaysL.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                }
                                else if (tempCountX == RoomWCount - 1 && tempCountZ == RoomLCount - 1)
                                {
                                    DoorwaysR.Remove(newB);
                                    newB.name = "Wall";
                                }
                                else if (tempCountX == 1 && tempCountZ == 1 || tempCountX == 1 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Left - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }
                                else if (tempCountX == RoomWCount - 2 && tempCountZ == 1 || tempCountX == RoomWCount - 2 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Right - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }



                                //Add one to the Z count to move along a step
                                tempCountZ += 1;

                            }
                            else if (tempCountZ >= RoomLCount)
                            {

                                tempCountZ = 0;
                                //Add one to our x counter to move up when level
                                tempCountX += 1;
                                //Reset our x positon back to 0, with the block's size taken into count
                                posZ = Mathf.Round(oldPosZ - blockSizeZ);
                                //Get our boxes last x axis (like its height) and add the size of our block to that variable to get our next level of blocks
                                posX = Mathf.Round(posX + blockSizeX);

                            }
                        }
                        if (blocks.Count > combinedBlockCount - 1)
                        {
                            if (tParent != null)
                            {

                                foreach (GameObject blok in blocks)
                                {
                                    blok.transform.parent = tParent.transform;

                                }


                            }


                            if (OverlappingBlocks == true)
                            {
                                foreach (GameObject blok in blocks)
                                {
                                    CheckOverlap(blok);
                                }
                            }

                            PlaceItems(blocks);

                            PlaceEnemies(blocks);

                            Rooms.Add(tParent);

                            tParent.transform.parent = LevelContainer.transform;


                            lastRoom = tParent;

                        }
                        //Debug.Log(tParent.name + spinRoom);
                        placedRooms += 1;
                        break;

                    case -90:
                        while (blocks.Count <= combinedBlockCount - 1)
                        {
                            if (blocks.Count == 0)
                            {
                                corridorOrNot = false;
                                float rn = Random.Range(0.4f, 0.6f);
                                float startPosTemp = RoomLCount * rn;
                                float startPosRound = Mathf.Round(startPosTemp);
                                float startPosZ = startPosRound * blockSizeZ;

                                Vector3 thePosition = new Vector3(lastBlock.transform.position.x - blockSizeX, 0, lastBlock.transform.position.z - startPosZ);

                                //CheckTheEntrances(thePosition);
                                GameObject newB;

                                newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                newB.name = "Def";

                                /* //Place the first block
                                 if (!corridorOrNot)
                                 {
                                     newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                     newB.name = "Def";
                                 }
                                 else
                                 {
                                     newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                     newB.name = "UsedC";
                                 }*/

                                blocks.Add(newB);


                                newB.transform.position = thePosition;



                                Doorways.Add(newB);


                                posX = newB.transform.position.x;
                                posZ = newB.transform.position.z;

                                oldPosX = posX;
                                oldPosZ = posZ;
                                tempCountZ += 1;

                            }


                            else if (tempCountZ < RoomLCount)
                            {
                                corridorOrNot = false;

                                int placeObst = Random.Range(0, (int)(Mathf.Sqrt((float)combinedBlockCount)));


                                float roundedZ = Mathf.Round(posZ + blockSizeZ);

                                //Change the blocks position to the old position we registered when the first/last block was placed
                                Vector3 thePosition = new Vector3(posX, 0, roundedZ);



                                //Add blocks to the Z axis
                                GameObject newB;

                                //For getting bottom boxes
                                if (tempCountX == 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    newB.name = "Def";
                                }
                                //Getting top boxes
                                else if (tempCountX == RoomWCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysL.Add(newB);
                                    newB.name = "L";
                                }
                                //Getting left side boxes
                                else if (tempCountZ == 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysB.Add(newB);
                                    newB.name = "B";
                                }
                                //Getting right side boxes
                                else if (tempCountZ == RoomLCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysT.Add(newB);
                                    newB.name = "T";
                                }
                                else if (tempCountX == tempLengthX && tempCountZ == tempLengthZ && OverlappingBlocks == false)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterBlock";
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Bottom Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Top Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Bottom Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Top Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount * 0.3f && tempCountZ <= RoomLCount - (RoomLCount * 0.3f)) && (tempCountX >= RoomWCount * 0.3f && tempCountX <= RoomWCount - (RoomWCount * 0.3f)) && placeCenterPiece == true)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterPiecesWhole";
                                }
                                else
                                {
                                    //Ensure we are not against a wall
                                    if (tempCountZ != RoomLCount - 2 && tempCountX != RoomWCount - 2 && tempCountX > 1 && tempCountZ > 1)
                                    {
                                        //For obstacle placement
                                        if (obstaclePerRoom > 0)
                                        {
                                            int tempSpacing = combinedBlockCount / obstaclePerRoom;

                                            if (obstacletempCount < obstaclePerRoom && Obstacles.Count >= 1 && placeObst <= 5 && sinceLastObstacle >= tempSpacing/*(blocks.Count >= tempSpacing && blocks.Count <= (combinedBlockCount - tempSpacing))*/)
                                            {


                                                if (Obstacles.Count > 1)
                                                {
                                                    int rand = Random.Range(0, Obstacles.Count);
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[rand], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }
                                                else
                                                {
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[0], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }

                                                sinceLastObstacle = 0;
                                            }
                                            else
                                            {
                                                newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                newB.name = "Floor";
                                                sinceLastObstacle += 1;
                                            }
                                        }
                                        else
                                        {
                                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                            newB.name = "Floor";
                                            sinceLastObstacle += 1;
                                        }
                                    }
                                    else
                                    {
                                        newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                        newB.name = "Floor";
                                        sinceLastObstacle += 1;
                                    }

                                }



                                blocks.Add(newB);
                                //This new block is now the last block we have created
                                lastBlock = newB;

                                newB.transform.position = thePosition;



                                //Getting each corner
                                if (tempCountX == 0 && tempCountZ == RoomLCount - 1 || tempCountX == RoomWCount - 1 && tempCountZ == 0)
                                {
                                    if (DoorwaysB.Contains(newB))
                                    {
                                        DoorwaysB.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysT.Contains(newB))
                                    {
                                        DoorwaysT.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysR.Contains(newB))
                                    {
                                        DoorwaysR.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysL.Contains(newB))
                                    {
                                        DoorwaysL.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                }
                                else if (tempCountX == RoomWCount - 1 && tempCountZ == RoomLCount - 1)
                                {
                                    DoorwaysR.Remove(newB);
                                    newB.name = "Wall";
                                }
                                else if (tempCountX == 1 && tempCountZ == 1 || tempCountX == 1 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Left - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }
                                else if (tempCountX == RoomWCount - 2 && tempCountZ == 1 || tempCountX == RoomWCount - 2 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Right - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }




                                //Assign the new blocks positon on the z-axis for the next block to use
                                posX = newB.transform.position.x;
                                posZ = newB.transform.position.z;

                                //Check we are not placing on top of other blocks
                                //CheckOverlap(newB);


                                //Add one to the Z count to move along a step
                                tempCountZ += 1;



                            }
                            else if (tempCountZ >= RoomLCount)
                            {

                                tempCountZ = 0;
                                //Add one to our x counter to move up when level
                                tempCountX += 1;
                                //Reset our x positon back to 0, with the block's size taken into count
                                posZ = Mathf.Round(oldPosZ - blockSizeZ);
                                //Get our boxes last x axis (like its height) and add the size of our block to that variable to get our next level of blocks
                                posX = Mathf.Round(posX - blockSizeX);

                            }
                        }
                        if (blocks.Count > combinedBlockCount - 1)
                        {

                            foreach (GameObject blok in blocks)
                            {
                                blok.transform.parent = tParent.transform;

                            }

                        }

                        if (tParent != null)
                        {
                            if (OverlappingBlocks == true)
                            {
                                foreach (GameObject blok in blocks)
                                {
                                    CheckOverlap(blok);
                                }
                            }

                            PlaceItems(blocks);

                            PlaceEnemies(blocks);


                            Rooms.Add(tParent);

                            tParent.transform.parent = LevelContainer.transform;

                            lastRoom = tParent;


                            //BuildCorridor(tParent);
                        }
                        //Debug.Log(tParent.name + spinRoom);
                        placedRooms += 1;
                        break;

                    case 180:
                        while (blocks.Count <= combinedBlockCount - 1)
                        {
                            if (blocks.Count == 0)
                            {
                                corridorOrNot = false;
                                float rn = Random.Range(0.45f, 0.55f);
                                float startPosTemp = RoomWCount * rn;
                                float startPosRound = Mathf.Round(startPosTemp);
                                float startPosZ = startPosRound * blockSizeX;


                                Vector3 thePosition = new Vector3(lastBlock.transform.position.x - startPosZ, 0, lastBlock.transform.position.z - blockSizeZ);

                                //CheckTheEntrances(thePosition);

                                GameObject newB;

                                newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                newB.name = "Wall";
                                newB.transform.position = new Vector3(lastBlock.transform.position.x - startPosZ, 0, lastBlock.transform.position.z - blockSizeZ);


                                blocks.Add(newB);
                                Doorways.Add(newB);


                                posX = newB.transform.position.x;
                                posZ = newB.transform.position.z;

                                oldPosX = posX;
                                oldPosZ = posZ;
                                tempCountZ += 1;

                            }


                            else if (tempCountZ < RoomLCount)
                            {
                                corridorOrNot = false;

                                int placeObst = Random.Range(0, (int)(Mathf.Sqrt((float)combinedBlockCount)));

                                float roundedX = Mathf.Round(posZ - blockSizeZ);

                                //Change the blocks position to the old position we registered when the first/last block was placed
                                Vector3 thePosition = new Vector3(posX, 0, roundedX);



                                //Add blocks to the Z axis
                                GameObject newB;

                                //For getting Top boxes
                                if (tempCountZ <= 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    newB.name = "Def";
                                }
                                //Getting Bottom boxes
                                else if (tempCountZ == RoomLCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysB.Add(newB);
                                    newB.name = "B";
                                }
                                //Getting left side boxes
                                else if (tempCountX == 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysL.Add(newB);
                                    newB.name = "L";
                                }
                                //Getting right side boxes
                                else if (tempCountX == RoomWCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysR.Add(newB);
                                    newB.name = "R";
                                }
                                else if (tempCountX == tempLengthX && tempCountZ == tempLengthZ && OverlappingBlocks == false)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterBlock";
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Bottom Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Top Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Bottom Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Top Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount * 0.3f && tempCountZ <= RoomLCount - (RoomLCount * 0.3f)) && (tempCountX >= RoomWCount * 0.3f && tempCountX <= RoomWCount - (RoomWCount * 0.3f)) && placeCenterPiece == true)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterPiecesWhole";
                                }
                                else
                                {
                                    //Ensure we are not against a wall
                                    if (tempCountZ != RoomLCount - 2 && tempCountX != RoomWCount - 2 && tempCountX > 1 && tempCountZ > 1)
                                    {
                                        //For obstacle placement
                                        if (obstaclePerRoom > 0)
                                        {
                                            int tempSpacing = combinedBlockCount / obstaclePerRoom;

                                            if (obstacletempCount < obstaclePerRoom && Obstacles.Count >= 1 && placeObst <= 5 && sinceLastObstacle >= tempSpacing/*(blocks.Count >= tempSpacing && blocks.Count <= (combinedBlockCount - tempSpacing))*/)
                                            {


                                                if (Obstacles.Count > 1)
                                                {
                                                    int rand = Random.Range(0, Obstacles.Count);
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[rand], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }
                                                else
                                                {
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[0], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }

                                                sinceLastObstacle = 0;
                                            }
                                            else
                                            {
                                                newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                newB.name = "Floor";
                                                sinceLastObstacle += 1;
                                            }
                                        }
                                        else
                                        {
                                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                            newB.name = "Floor";
                                            sinceLastObstacle += 1;
                                        }
                                    }
                                    else
                                    {
                                        newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                        newB.name = "Floor";
                                        sinceLastObstacle += 1;
                                    }

                                }

                                blocks.Add(newB);
                                //This new block is now the last block we have created
                                lastBlock = newB;

                                //Change the blocks position to the old position we registered when the first/last block was placed
                                newB.transform.position = thePosition;

                                //Getting each corner
                                if (tempCountX == 0 && tempCountZ == RoomLCount - 1 || tempCountX == RoomWCount - 1 && tempCountZ == 0)
                                {
                                    if (DoorwaysB.Contains(newB))
                                    {
                                        DoorwaysB.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysT.Contains(newB))
                                    {
                                        DoorwaysT.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysR.Contains(newB))
                                    {
                                        DoorwaysR.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysL.Contains(newB))
                                    {
                                        DoorwaysL.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                }
                                else if (tempCountX == RoomWCount - 1 && tempCountZ == RoomLCount - 1)
                                {
                                    DoorwaysR.Remove(newB);
                                    newB.name = "Wall";
                                }
                                else if (tempCountX == 1 && tempCountZ == 1 || tempCountX == 1 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Left - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }
                                else if (tempCountX == RoomWCount - 2 && tempCountZ == 1 || tempCountX == RoomWCount - 2 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Right - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }



                                //Assign the new blocks positon on the z-axis for the next block to use
                                posZ = newB.transform.position.z;



                                //Add one to the Z count to move along a step
                                tempCountZ += 1;



                            }
                            else if (tempCountZ >= RoomLCount)
                            {

                                tempCountZ = 0;
                                //Add one to our x counter to move up when level
                                tempCountX += 1;
                                //Reset our x positon back to 0, with the block's size taken into count
                                posZ = Mathf.Round(oldPosZ + blockSizeZ);
                                //Get our boxes last x axis (like its height) and add the size of our block to that variable to get our next level of blocks
                                posX = Mathf.Round(posX + blockSizeX);

                            }
                        }
                        if (blocks.Count > combinedBlockCount - 1)
                        {

                            if (tParent != null)
                            {

                                foreach (GameObject blok in blocks)
                                {
                                    blok.transform.parent = tParent.transform;

                                }

                            }


                            if (OverlappingBlocks == true)
                            {
                                foreach (GameObject blok in blocks)
                                {
                                    CheckOverlap(blok);
                                }
                            }

                            PlaceItems(blocks);

                            PlaceEnemies(blocks);



                            Rooms.Add(tParent);

                            tParent.transform.parent = LevelContainer.transform;

                            lastRoom = tParent;


                            //BuildCorridor(tParent);
                        }
                        // Debug.Log(tParent.name + spinRoom);
                        placedRooms += 1;
                        break;

                    case -180:
                        while (blocks.Count <= combinedBlockCount - 1)
                        {
                            if (blocks.Count == 0)
                            {
                                corridorOrNot = false;
                                float rn = Random.Range(0.45f, 0.55f);
                                float startPosTemp = RoomWCount * rn;
                                float startPosRound = Mathf.Round(startPosTemp);
                                float startPosZ = startPosRound * blockSizeX;


                                Vector3 thePosition = new Vector3(lastBlock.transform.position.x - startPosZ, 0, lastBlock.transform.position.z - blockSizeZ);

                                //CheckTheEntrances(thePosition);

                                GameObject newB;

                                if (!corridorOrNot)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    newB.name = "Wall";
                                }
                                else
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "Floor";
                                }

                                newB.transform.position = new Vector3(lastBlock.transform.position.x - startPosZ, 0, lastBlock.transform.position.z - blockSizeZ);


                                blocks.Add(newB);




                                Doorways.Add(newB);


                                posX = newB.transform.position.x;
                                posZ = newB.transform.position.z;

                                oldPosX = posX;
                                oldPosZ = posZ;
                                tempCountZ += 1;

                            }


                            else if (tempCountZ < RoomLCount)
                            {
                                corridorOrNot = false;

                                int placeObst = Random.Range(0, (int)(Mathf.Sqrt((float)combinedBlockCount)));

                                float roundedX = Mathf.Round(posZ - blockSizeZ);

                                //Change the blocks position to the old position we registered when the first/last block was placed
                                Vector3 thePosition = new Vector3(posX, 0, roundedX);



                                //Add blocks to the Z axis
                                GameObject newB;

                                //For getting Top boxes
                                if (tempCountZ <= 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    newB.name = "Def";
                                }
                                //Getting Bottom boxes
                                else if (tempCountZ == RoomLCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysB.Add(newB);
                                    newB.name = "B";
                                }
                                //Getting left side boxes
                                else if (tempCountX == 0)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysL.Add(newB);
                                    newB.name = "L";
                                }
                                //Getting Right side boxes
                                else if (tempCountX == RoomLCount - 1)
                                {
                                    newB = Instantiate(WallBlock, transform.position, transform.rotation);
                                    DoorwaysR.Add(newB);
                                    newB.name = "R";
                                }
                                else if (tempCountX == tempLengthX && tempCountZ == tempLengthZ)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Bottom Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX > 0 && tempCountX <= RoomWCount * 0.25f) && OverlappingBlocks == false) //Top Left Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ > 0 && tempCountZ <= RoomLCount * 0.25f) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Bottom Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount - (RoomLCount * 0.25f) && tempCountZ < RoomLCount - 1) && (tempCountX >= RoomWCount - (RoomWCount * 0.25f) && tempCountX < RoomWCount - 1) && OverlappingBlocks == false) //Top Right Corner Corner Blocks
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CornerBlock";
                                    CornerSpots.Add(newB.gameObject);
                                }
                                else if ((tempCountZ >= RoomLCount * 0.3f && tempCountZ <= RoomLCount - (RoomLCount * 0.3f)) && (tempCountX >= RoomWCount * 0.3f && tempCountX <= RoomWCount - (RoomWCount * 0.3f)) && placeCenterPiece == true)
                                {
                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    newB.name = "CenterPiecesWhole";
                                }
                                else
                                {
                                    //Ensure we are not against a wall
                                    if (tempCountZ != RoomLCount - 2 && tempCountX != RoomWCount - 2 && tempCountX > 1 && tempCountZ > 1)
                                    {
                                        //For obstacle placement
                                        if (obstaclePerRoom > 0)
                                        {
                                            int tempSpacing = combinedBlockCount / obstaclePerRoom;

                                            if (obstacletempCount < obstaclePerRoom && Obstacles.Count >= 1 && placeObst <= 5 && sinceLastObstacle >= tempSpacing/*(blocks.Count >= tempSpacing && blocks.Count <= (combinedBlockCount - tempSpacing))*/)
                                            {
                                                if (Obstacles.Count > 1)
                                                {
                                                    int rand = Random.Range(0, Obstacles.Count);
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[rand], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }
                                                else
                                                {
                                                    newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                    newB.name = "Floor";
                                                    GameObject newObst = Instantiate(Obstacles[0], transform.position, transform.rotation);

                                                    float newObstY = 0;
                                                    if (newObst.GetComponent<Renderer>() != null)
                                                    {
                                                        newObstY = (newObst.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }
                                                    else
                                                    {
                                                        newObstY = (newObst.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                                                    }

                                                    newObst.transform.position = new Vector3(thePosition.x, newObstY, thePosition.z);
                                                    tempObstacleList.Add(newObst);
                                                    newObst.name = "Obstacle";
                                                    obstacletempCount += 1;
                                                }

                                                sinceLastObstacle = 0;
                                            }
                                            else
                                            {
                                                newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                                newB.name = "Floor";
                                                sinceLastObstacle += 1;
                                            }
                                        }
                                        else
                                        {
                                            newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                            newB.name = "Floor";
                                            sinceLastObstacle += 1;
                                        }
                                    }
                                    else
                                    {
                                        newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                        newB.name = "Floor";
                                        sinceLastObstacle += 1;
                                    }

                                }

                                blocks.Add(newB);
                                //This new block is now the last block we have created
                                lastBlock = newB;

                                //Change the blocks position to the old position we registered when the first/last block was placed
                                newB.transform.position = thePosition;

                                //Getting each corner
                                if (tempCountX == 0 && tempCountZ == RoomLCount - 1 || tempCountX == RoomWCount - 1 && tempCountZ == 0)
                                {
                                    if (DoorwaysB.Contains(newB))
                                    {
                                        DoorwaysB.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysT.Contains(newB))
                                    {
                                        DoorwaysT.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysR.Contains(newB))
                                    {
                                        DoorwaysR.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                    else if (DoorwaysL.Contains(newB))
                                    {
                                        DoorwaysL.Remove(newB);
                                        newB.name = "Wall";
                                    }
                                }
                                else if (tempCountX == RoomWCount - 1 && tempCountZ == RoomLCount - 1)
                                {
                                    DoorwaysR.Remove(newB);
                                    newB.name = "Wall";
                                }
                                else if (tempCountX == 1 && tempCountZ == 1 || tempCountX == 1 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Left - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }
                                else if (tempCountX == RoomWCount - 2 && tempCountZ == 1 || tempCountX == RoomWCount - 2 && tempCountZ == RoomLCount - 2) //Second Corner - For Corner Blocks - Right - Bottom or Top - remove from corners block - only during intial build, after, if objects are changed, it will be used
                                {
                                    if (newB.name == "CornerBlock")
                                    {
                                        CornerSpots.Remove(newB);
                                    }
                                }



                                //Assign the new blocks positon on the z-axis for the next block to use
                                posX = newB.transform.position.x;



                                //Add one to the Z count to move along a step
                                tempCountZ += 1;



                            }
                            else if (tempCountZ >= RoomLCount)
                            {

                                tempCountZ = 0;
                                //Add one to our x counter to move up when level
                                tempCountX += 1;
                                //Reset our x positon back to 0, with the block's size taken into count
                                posZ = Mathf.Round(oldPosZ + blockSizeZ);
                                //Get our boxes last x axis (like its height) and add the size of our block to that variable to get our next level of blocks
                                posX = Mathf.Round(posX + blockSizeX);

                            }
                        }
                        if (blocks.Count > combinedBlockCount - 1)
                        {
                            if (tParent != null)
                            {

                                foreach (GameObject blok in blocks)
                                {
                                    blok.transform.parent = tParent.transform;

                                }


                            }


                            if (OverlappingBlocks == true)
                            {
                                foreach (GameObject blok in blocks)
                                {
                                    CheckOverlap(blok);
                                }
                            }

                            PlaceItems(blocks);

                            PlaceEnemies(blocks);

                            Rooms.Add(tParent);

                            tParent.transform.parent = LevelContainer.transform;

                            lastRoom = tParent;

                        }
                        // Debug.Log(tParent.name + spinRoom);
                        placedRooms += 1;
                        break;

                }




                foreach (GameObject obs in tempObstacleList)
                {
                    obs.transform.parent = tParent.transform;
                }

                //Can Just call the CornerFill Function, as it checks the amount to fill inside - if 0, it will simply return straight away
                if (!OverlappingBlocks)
                {
                    FillCorners(CornerSpots);
                }

                if(placeCenterPiece == true)
                {
                    foreach (GameObject b in blocks)
                    {
                        if (b.name == "CenterBlock")
                        {
                           if(CenterPieceObjects.Count > 0)
                            {
                                int rPiece = Random.Range(0, CenterPieceObjects.Count);
                                GameObject newPiece = Instantiate(CenterPieceObjects[rPiece].gameObject, transform.position, transform.rotation);
                                Vector3 newPos = new Vector3(b.transform.position.x, blockSizeY, b.transform.position.z);

                                newPiece.name = "CenterPiece";
                                newPiece.transform.position = newPos;
                                newPiece.transform.parent = b.transform.parent.transform;


                            }
                        }
                
                    }

                }
                

                TempDoorways.Remove(TempDoorways[0]);
                TempDoorwaySpins.Remove(TempDoorwaySpins[0]);




            }
            else
            {
                TempDoorways.Clear();
                TempDoorwaySpins.Clear();
                chooseRoom += 1;
                tempObstacleList.Clear();

                ChooseARandomRoom();

                //placedRooms += 1;



                DestroyImmediate(tParent, true);

            }


        }



    }

    //

    //Corridor Code

    //Code for chosing how many corridors, which then calls the next function
    public void CorridorSelection(GameObject parentRoom)
    {
        if (mUp && mRight && mDown || mDown && mRight && mUp)
        {
            DoorwaysL.Clear();
        }
        else if (mUp && mLeft && mDown || mDown && mLeft && mUp)
        {
            DoorwaysR.Clear();
        }
        else if (mLeft && mDown & mRight || mRight && mDown && mLeft)
        {
            DoorwaysT.Clear();
        }
        else if (mLeft && mUp & mRight || mRight && mUp && mLeft)
        {
            DoorwaysB.Clear();
        }

        int r;

        if (alreadyBuiltOne == false)
        {
            r = Random.Range(1, 5);
        }
        else
        {
            r = 1;
        }
        //Debug.Log(r);

        if (Rooms.Count <= HowManyRooms - 4)
        {
            BuildCorridors(parentRoom, r);
        }
        else if (Rooms.Count == HowManyRooms - 3)
        {
            r = Random.Range(1, 3);
            BuildCorridors(parentRoom, r);
        }
        else if (Rooms.Count == HowManyRooms - 2)
        {
            if (!alreadyBuiltOne)
            {
                r = 1;
                BuildCorridors(parentRoom, r);
            }
            else
            {
                //Do Nothing
            }
        }
        else if (Rooms.Count == HowManyRooms - 1)
        {
            //Debug.Log(Rooms.Count);
            /*r = 1;
            ChooseARandomDoor(parentRoom, r);*/
        }
        else
        {
            //Do nothing
        }


    }
    //Code for choosing a doorway and building off from it.
    public void BuildCorridors(GameObject parentRoom, int random)
    {
        if (DoorwaysL.Count == 0 && DoorwaysR.Count == 0 && DoorwaysT.Count == 0 && DoorwaysB.Count == 0)
        {
            ChooseARandomRoom();
        }

        int tCounter = 0;

        for (int i = 0; i < random; i++)
        {



            List<GameObject> blocks = new List<GameObject>();

            List<GameObject> corridorBlocks = new List<GameObject>();


            tempCountZ = 0;
            tempCountX = 0;

            float posX = 0;
            float posZ = 0;

            CorWCount = Random.Range(MinWidthCorridor, MaxWidthCorridor);
            CorLCount = Random.Range(MinLengthCorridor, MaxLengthCorridor);
            //Debug.Log(BlockXCorCount + " = Z and " + BlockZCorCount + " = X.");

            combinedBlockCount = CorWCount * CorLCount;

            blockLengthZ = CorLCount;
            blockLengthX = CorWCount;

            spinRoom = 0;

            int chooseRandomDoor = Random.Range(0, 4);

            //For if we are creating seperate corridor parents
            GameObject corridorParent = null;

            corridorBlocks.Clear();


            if (OverlappingBlocks == false)
            {



                switch (chooseRandomDoor)
                {
                    //Top Doorways
                    case 0:
                        int d1 = DoorwaysT.Count - 1;
                        if (d1 >= 1)
                        {
                            int randoD1 = 0;
                            //int randoD1 = Random.Range(0, d1);
                            bool hasNotOverlapped = false;
                            for (int dway = 0; dway < DoorwaysT.Count; dway++)
                            {
                                if (dway == 0)
                                {
                                    randoD1 = Mathf.RoundToInt(DoorwaysT.Count / 2);
                                }
                                else
                                {
                                    randoD1 = dway;
                                }


                                hasNotOverlapped = CheckNoOverlapWillOccur(blockLengthX, blockLengthZ, DoorwaysT[randoD1].transform.position, "Up");
                                //Debug.Log(hasNotOverlapped);

                                if (hasNotOverlapped == false)
                                {
                                    Doorways.Remove(DoorwaysT[randoD1]);
                                    DoorwaysT[randoD1].name = "Def";
                                    DoorwaysT.Remove(DoorwaysT[randoD1]);
                                }
                                else
                                {
                                    break;
                                }

                            }

                            if (hasNotOverlapped == false)
                            {
                                break;
                            }

                            doorwayOne = DoorwaysT[randoD1];
                            spinRoom = 0;
                            TempDoorwaySpins.Add(spinRoom);
                            mUp = true;
                            doorwayOne.name = "UsedCorridor";

                            //Debug.Log(parentRoom.name + " = Case: 0");

                            //For blocks to be added to their own parent
                            if (SeperateCorridorParents == true)
                            {

                                corridorParent = Instantiate(ParentObj, transform.position, transform.rotation);
                                //Maybe have to add in a position later
                                corridorParent.name = ("Corridor " + (corridorParentCounter + 1));
                                corridorParent.transform.parent = LevelContainer.transform;
                            }
                            corridorParentCounter += 1;


                            while (blocks.Count <= combinedBlockCount - 1)
                            {
                                if (blocks.Count == 0)
                                {
                                    //Place the first block


                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);

                                    Vector3 center = new Vector3(0, 0, 0);

                                    //Width check for wheter to plus or minus the size
                                    for (int ind = 0; ind < doorwayOne.transform.parent.transform.childCount; ind++)
                                    {
                                        if (doorwayOne.transform.parent.transform.GetChild(ind).gameObject.name == "CenterBlock")
                                        {
                                            center = doorwayOne.transform.parent.transform.GetChild(ind).transform.position;
                                            break;
                                        }
                                    }

                                    if (center.x == 0 && center.y == 0 && center.z == 0)
                                    {
                                        center = doorwayOne.transform.position;
                                    }
                                    Vector3 newPos = doorwayOne.transform.position;

                                    float rounded = Mathf.Round(newPos.z + blockSizeZ);

                                    if (newPos.x > center.x)
                                    {
                                        if(CorWCount + 2 < (RoomWCount * 0.5f) - 2f)
                                        {
                                            newB.transform.position = new Vector3(center.x - (blockSizeX * 2), 0, rounded);
                                        }
                                        else if(CorWCount + 2 < (RoomWCount * 0.5f))
                                        {
                                            newB.transform.position = new Vector3(center.x - blockSizeX, 0, rounded);

                                        }
                                        else
                                        {
                                            newB.transform.position = new Vector3(center.x, 0, rounded);
                                        }

                                    }
                                    else if (newPos.x < center.x)
                                    {
                                        if (CorWCount + 2 < (RoomWCount * 0.5f) - 2f)
                                        {
                                            newB.transform.position = new Vector3(center.x + (blockSizeX * 2), 0, rounded);
                                        }
                                        else if (CorWCount + 2 < (RoomWCount * 0.5f))
                                        {
                                            newB.transform.position = new Vector3((center.x + blockSizeX), 0, rounded);

                                        }
                                        else
                                        {
                                            newB.transform.position = new Vector3(center.x, 0, rounded);
                                        }
                                    }
                                    else
                                    {
                                        newB.transform.position = new Vector3(center.x, 0, rounded);
                                    }





                                    //newB.transform.position = new Vector3(newPos.x, 0, newPos.z + blockSizeZ);
                                    newB.name = "Entrance";
                                    checkEntrances.Add(newB);


                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;
                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);

                                            }


                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1 - add to one side
                                        else
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }



                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }




                                    posX = newB.transform.position.x;
                                    posZ = newB.transform.position.z;

                                    oldPosX = posX;
                                    oldPosZ = posZ;

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ < CorLCount)
                                {
                                    //Add blocks to the Z axis
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    lastBlock = newB;

                                    CorridorCreated = newB;


                                    float rounded = Mathf.Round(posZ + blockSizeZ);

                                    newB.transform.position = new Vector3(posX, 0, rounded);
                                    //newB.isStatic = true;


                                    if (tempCountZ == CorLCount - 1)
                                    {

                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);
                                    }
                                    else if (tempCountZ == 0)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);
                                        Vector3 below = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);
                                        Vector3 below2 = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - (blockSizeZ * 2));
                                        foreach (Transform childB in parentRoom.transform)
                                        {
                                            if ((childB.name == "T" || childB.name == "B" || childB.name == "R" || childB.name == "L" || childB.name == "Def" || childB.name == "Wall"))
                                            {
                                                if (childB.transform.position == below)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                }
                                                else if (childB.transform.position == below2)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        newB.name = "Floor";
                                    }

                                    posZ = newB.transform.position.z;



                                    if (NoCorridorWalls == false)
                                    {


                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {

                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1, and we are on the first row- add to one side
                                        else if (blockLengthX > 1 && tempCountX == 0)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                        //If Corridor is Greater than 1, but we are in the center (tempXCount - 1 == the end of corW - place nothing
                                        else if (blockLengthX > 1 && tempCountX > 0 && tempCountX < CorWCount - 1)
                                        {
                                            //Do nothing
                                        }
                                        //If corridor is Greater than 1, and we are on the last row - add to one side
                                        else if (blockLengthX > 1 && tempCountX == CorWCount - 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;


                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }




                                    tempCountZ += 1;

                                }
                                else if (tempCountZ >= CorLCount)
                                {
                                    tempCountZ = 0;
                                    tempCountX += 1;

                                    posX = Mathf.Round(posX + blockSizeX);
                                    //posX = posX - blockSizeX;
                                    posZ = Mathf.Round(oldPosZ - blockSizeZ);
                                }


                            }
                            if (blocks.Count > combinedBlockCount - 1)
                            {
                                if (parentRoom != null)
                                {
                                    foreach (GameObject blok in blocks)
                                    {

                                        if (SeperateCorridorParents == false)
                                        {
                                            //To add the corridor blocks to the current room as a parent
                                            blok.transform.parent = parentRoom.transform;
                                        }
                                        else
                                        {
                                            blok.transform.parent = corridorParent.transform;
                                        }




                                        if (blocks.IndexOf(blok) == blocks.Count - 1)
                                        {
                                            blok.transform.rotation = Quaternion.Euler(0, 0, 0);
                                            Doorways.Add(blok);
                                            TempDoorways.Add(blok);

                                        }

                                        if (OverlappingBlocks == true)
                                        {
                                            CheckOverlap(blok);
                                        }

                                        if (blok.name != "Entrance")
                                        {
                                            blok.name = "Corridor";
                                        }
                                    }
                                }

                                CorridorCreated = parentRoom;

                                foreach (GameObject dt in DoorwaysT)
                                {
                                    dt.name = "Def";
                                }


                                DoorwaysT.Clear();
                                /*DoorwaysT.Clear();
                                DoorwaysB.Clear();
                                DoorwaysL.Clear();
                                DoorwaysR.Clear();*/
                                alreadyBuiltOne = true;

                                lastRoom = CorridorCreated;
                                tCounter += 1;

                            }
                        }
                        else
                        {
                            break;
                            //BuildCorridor(parentRoom);
                        }

                        break;

                    //Bottom Doorways
                    case 1:
                        int d2 = DoorwaysB.Count - 1;
                        if (d2 >= 1)
                        {
                            int randoD2 = 0;
                            //int randoD2 = Random.Range(0, d2);

                            bool hasNotOverlapped = false;
                            for (int dway = 0; dway < DoorwaysB.Count; dway++)
                            {
                                if (dway == 0)
                                {
                                    randoD2 = Mathf.RoundToInt(DoorwaysB.Count / 2);
                                }
                                else
                                {
                                    randoD2 = dway;
                                }


                                hasNotOverlapped = CheckNoOverlapWillOccur(blockLengthX, blockLengthZ, DoorwaysB[randoD2].transform.position, "Down");

                                if (hasNotOverlapped == false)
                                {
                                    Doorways.Remove(DoorwaysB[randoD2]);
                                    DoorwaysB[randoD2].name = "Def";
                                    DoorwaysB.Remove(DoorwaysB[randoD2]);
                                }
                                else
                                {
                                    break;
                                }

                            }

                            if (hasNotOverlapped == false)
                            {
                                break;
                            }

                            doorwayOne = DoorwaysB[randoD2];
                            spinRoom = 180;
                            TempDoorwaySpins.Add(spinRoom);
                            mDown = true;
                            doorwayOne.name = "UsedCorridor";

                            //For blocks to be added to their own parent
                            if (SeperateCorridorParents == true)
                            {

                                corridorParent = Instantiate(ParentObj, transform.position, transform.rotation);
                                //Maybe have to add in a position later
                                corridorParent.name = ("Corridor " + (corridorParentCounter + 1));
                                corridorParent.transform.parent = LevelContainer.transform;
                            }
                            corridorParentCounter += 1;


                            while (blocks.Count <= combinedBlockCount - 1)
                            {
                                if (blocks.Count == 0)
                                {
                                    //Place the first block
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    CorridorCreated = newB;


                                    Vector3 center = new Vector3(0, 0, 0);

                                    //Width check for wheter to plus or minus the size
                                    for (int ind = 0; ind < doorwayOne.transform.parent.transform.childCount; ind++)
                                    {
                                        if (doorwayOne.transform.parent.transform.GetChild(ind).gameObject.name == "CenterBlock")
                                        {
                                            center = doorwayOne.transform.parent.transform.GetChild(ind).transform.position;
                                            break;
                                        }
                                    }

                                    if (center.x == 0 && center.y == 0 && center.z == 0)
                                    {
                                        center = doorwayOne.transform.position;
                                    }
                                    Vector3 newPos = doorwayOne.transform.position;

                                    float rounded = Mathf.Round(newPos.z - blockSizeZ);

                                    if (newPos.x > center.x)
                                    {
                                        if (CorWCount + 2 < (RoomWCount * 0.5f) - 2f)
                                        {
                                            newB.transform.position = new Vector3(center.x + (blockSizeX * 2), 0, rounded);
                                        }
                                        else if (CorWCount + 2 < (RoomWCount * 0.5f))
                                        {
                                            newB.transform.position = new Vector3(center.x + blockSizeX, 0, rounded);
                                        }
                                        else
                                        {
                                            newB.transform.position = new Vector3(center.x, 0, rounded);
                                        }

                                    }
                                    else if (newPos.x < center.x)
                                    {
                                        if (CorWCount + 2 < (RoomWCount * 0.5f) - 2f)
                                        {
                                            newB.transform.position = new Vector3(center.x - (blockSizeX * 2), 0, rounded);
                                        }
                                        else if (CorWCount + 2 < (RoomWCount * 0.5f))
                                        {
                                            newB.transform.position = new Vector3(center.x - blockSizeX, 0, rounded);
                                        }
                                        else
                                        {
                                            newB.transform.position = new Vector3(center.x, 0, rounded);
                                        }
                                    }
                                    else
                                    {
                                        newB.transform.position = new Vector3(newPos.x, 0, rounded);
                                    }






                                    //newB.transform.position = new Vector3(newPos.x, 0, rounded);


                                    newB.name = "Entrance";
                                    checkEntrances.Add(newB);




                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1 - add to one side
                                        else
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }



                                    posX = newB.transform.position.x;
                                    posZ = newB.transform.position.z;

                                    oldPosX = posX;
                                    oldPosZ = posZ;

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ < CorLCount)
                                {
                                    //Add blocks to the Z axis
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    lastBlock = newB;

                                    CorridorCreated = newB;

                                    float rounded = Mathf.Round(posZ - blockSizeZ);

                                    newB.transform.position = new Vector3(posX, 0, rounded);



                                    if (tempCountZ == CorLCount - 1)
                                    {

                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);
                                    }
                                    else if (tempCountZ == 0)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);

                                    }
                                    else
                                    {
                                        newB.name = "Floor";
                                    }

                                    posZ = newB.transform.position.z;
                                    //posX = newB.transform.position.x;



                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1, and we are on the first row- add to one side
                                        else if (blockLengthX > 1 && tempCountX == 0)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";


                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                        }
                                        //If Corridor is Greater than 1, but we are in the center (tempXCount - 1 == the end of corW - place nothing
                                        else if (blockLengthX > 1 && tempCountX > 0 && tempCountX < CorWCount - 1)
                                        {
                                            //Do nothing
                                        }
                                        //If corridor is Greater than 1, and we are on the last row - add to one side
                                        else if (blockLengthX > 1 && tempCountX == CorWCount - 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }



                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }




                                    /*foreach (Transform child in LevelContainer.transform)
                                    {
                                        foreach (Transform blocky in child.transform)
                                        {
                                            if (newB.transform.position == blocky.position)
                                            {
                                                blocky.gameObject.SetActive(false);
                                                blocky.gameObject.name = "DELETE ME!";
                                                Deactivated.Add(blocky.gameObject);
                                            }
                                        }
                                    }*/

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ >= CorLCount)
                                {
                                    tempCountZ = 0;
                                    tempCountX += 1;
                                    //oldPosZ = 0 - blockSizeZ;
                                    //oldPosX = lastBlock.transform.position.x + blockSizeX;
                                    posX = Mathf.Round(posX + blockSizeX);
                                    //posX = posX - blockSizeX;
                                    posZ = Mathf.Round(oldPosZ + blockSizeZ);
                                }


                            }
                            if (blocks.Count > combinedBlockCount - 1)
                            {
                                if (parentRoom != null)
                                {
                                    foreach (GameObject blok in blocks)
                                    {
                                        if (SeperateCorridorParents == false)
                                        {
                                            //To add the corridor blocks to the current room as a parent
                                            blok.transform.parent = parentRoom.transform;
                                        }
                                        else
                                        {
                                            blok.transform.parent = corridorParent.transform;
                                        }


                                        if (blocks.IndexOf(blok) == blocks.Count - 1)
                                        {
                                            blok.transform.rotation = Quaternion.Euler(0, 180, 0);
                                            Doorways.Add(blok);
                                            TempDoorways.Add(blok);
                                        }

                                        if (OverlappingBlocks == true)
                                        {
                                            CheckOverlap(blok);
                                        }

                                        if (blok.name != "Entrance")
                                        {
                                            blok.name = "Corridor";
                                        }

                                    }

                                }



                                CorridorCreated = parentRoom;

                                foreach (GameObject db in DoorwaysB)
                                {
                                    db.name = "Def";
                                }

                                DoorwaysB.Clear();



                                alreadyBuiltOne = true;
                                lastRoom = CorridorCreated;
                                tCounter += 1;
                            }
                        }
                        else
                        {
                            break;
                        }
                        break;

                    //Left Doorways
                    case 2:
                        int d3 = DoorwaysL.Count - 1;
                        if (d3 >= 1)
                        {
                            //int randoD3 = Random.Range(0, d3);
                            int randoD3 = 0;

                            bool hasNotOverlapped = false;
                            for (int dway = 0; dway < DoorwaysL.Count; dway++)
                            {
                                if (dway == 0)
                                {
                                    randoD3 = Mathf.RoundToInt(DoorwaysL.Count / 2);
                                }
                                else
                                {
                                    randoD3 = dway;
                                }


                                hasNotOverlapped = CheckNoOverlapWillOccur(blockLengthX, blockLengthZ, DoorwaysL[randoD3].transform.position, "Left");

                                if (hasNotOverlapped == false)
                                {
                                    Doorways.Remove(DoorwaysL[randoD3]);
                                    DoorwaysL[randoD3].name = "Def";
                                    DoorwaysL.Remove(DoorwaysL[randoD3]);
                                }
                                else
                                {
                                    Debug.Log("Has overlapperd");
                                    break;
                                }

                            }

                            if (hasNotOverlapped == false)
                            {
                                break;
                            }


                            doorwayOne = DoorwaysL[randoD3];
                            spinRoom = -90;
                            TempDoorwaySpins.Add(spinRoom);
                            mLeft = true;
                            doorwayOne.name = "UsedCorridor";

                            //For blocks to be added to their own parent
                            if (SeperateCorridorParents == true)
                            {

                                corridorParent = Instantiate(ParentObj, transform.position, transform.rotation);
                                //Maybe have to add in a position later
                                corridorParent.name = ("Corridor " + (corridorParentCounter + 1));
                                corridorParent.transform.parent = LevelContainer.transform;
                            }
                            corridorParentCounter += 1;

                            while (blocks.Count <= combinedBlockCount - 1)
                            {
                                if (blocks.Count == 0)
                                {
                                    //Place the first block
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    CorridorCreated = newB;



                                    Vector3 center = new Vector3(0, 0, 0);

                                    //Width check for wheter to plus or minus the size
                                    for (int ind = 0; ind < doorwayOne.transform.parent.transform.childCount; ind++)
                                    {
                                        if (doorwayOne.transform.parent.transform.GetChild(ind).gameObject.name == "CenterBlock")
                                        {
                                            center = doorwayOne.transform.parent.transform.GetChild(ind).transform.position;
                                            break;
                                        }
                                    }

                                    if (center.x == 0 && center.y == 0 && center.z == 0)
                                    {
                                        center = doorwayOne.transform.position;
                                    }

                                    Vector3 newPos = doorwayOne.transform.position;

                                    float rounded = Mathf.Round(newPos.x - blockSizeX);

                                    if (newPos.z > center.z)
                                    {

                                        if (CorWCount + 2 < (RoomLCount * 0.5f) - 2f)
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z - (blockSizeZ * 2));
                                        }
                                        else if (CorWCount + 2 < (RoomLCount * 0.5f))
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z - blockSizeZ);
                                        }
                                        else
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z);
                                        }
                                    }
                                    else if (newPos.z < center.z)
                                    {
                                        if (CorWCount + 2 < (RoomLCount * 0.5f) - 2f)
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z + (blockSizeZ * 2));
                                        }
                                        else if (CorWCount + 2 < (RoomLCount * 0.5f))
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z + blockSizeZ);
                                        }
                                        else
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z);
                                        }
                                    }
                                    else
                                    {
                                        newB.transform.position = new Vector3(rounded, 0, center.z);
                                    }
                                    //newB.transform.position = new Vector3(rounded, 0, center.z);

                                    //newB.transform.position = new Vector3(rounded, 0, newPos.z);


                                    newB.name = "Entrance";
                                    checkEntrances.Add(newB);


                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1 - add to one side
                                        else
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }


                                    posX = newB.transform.position.x;
                                    posZ = newB.transform.position.z;

                                    oldPosX = posX;
                                    oldPosZ = posZ;


                                    tempCountZ += 1;

                                }
                                else if (tempCountZ < CorLCount)
                                {
                                    //Add blocks to the Z axis
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    lastBlock = newB;

                                    CorridorCreated = newB;

                                    float rounded = Mathf.Round(posX - blockSizeX);

                                    newB.transform.position = new Vector3(rounded, 0, posZ);





                                    if (tempCountZ == CorLCount - 1 && CorLCount > 1)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);

                                    }
                                    else if (tempCountZ == 0)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);

                                    }
                                    else
                                    {
                                        newB.name = "Floor";
                                    }

                                    posX = newB.transform.position.x;





                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1, and we are on the first row- add to one side
                                        else if (blockLengthX > 1 && tempCountX == 0)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                        //If Corridor is Greater than 1, but we are in the center (tempXCount - 1 == the end of corW - place nothing
                                        else if (blockLengthX > 1 && tempCountX > 0 && tempCountX < CorWCount - 1)
                                        {
                                            //Do nothing
                                        }
                                        //If corridor is Greater than 1, and we are on the last row - add to one side
                                        else if (blockLengthX > 1 && tempCountX == CorWCount - 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;


                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }



                                    /*foreach (Transform child in LevelContainer.transform)
                                    {
                                        foreach (Transform blocky in child.transform)
                                        {
                                            if (newB.transform.position == blocky.position)
                                            {
                                                blocky.gameObject.SetActive(false);
                                                blocky.gameObject.name = "DELETE ME!";
                                                Deactivated.Add(blocky.gameObject);
                                            }
                                        }
                                    }*/

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ >= CorLCount)
                                {
                                    tempCountZ = 0;
                                    tempCountX += 1;

                                    posZ = Mathf.Round(posZ + blockSizeZ);
                                    posX = Mathf.Round(oldPosX + blockSizeX);


                                }


                            }
                            if (blocks.Count > combinedBlockCount - 1)
                            {
                                if (parentRoom != null)
                                {
                                    foreach (GameObject blok in blocks)
                                    {
                                        if (SeperateCorridorParents == false)
                                        {
                                            //To add the corridor blocks to the current room as a parent
                                            blok.transform.parent = parentRoom.transform;
                                        }
                                        else
                                        {
                                            blok.transform.parent = corridorParent.transform;
                                        }


                                        if (blocks.IndexOf(blok) == blocks.Count - 1)
                                        {
                                            blok.transform.rotation = Quaternion.Euler(0, -90, 0);
                                            Doorways.Add(blok);
                                            TempDoorways.Add(blok);
                                        }

                                        if (OverlappingBlocks == true)
                                        {
                                            CheckOverlap(blok);
                                        }

                                        if (blok.name != "Entrance")
                                        {
                                            blok.name = "Corridor";
                                        }
                                    }

                                }

                                /*if (WalledCorridors == true)
                                {
                                    foreach (GameObject coriWall in corridorBlocks)
                                    {
                                        if (coriWall.transform.childCount > 0 && coriWall.transform.GetChild(0).gameObject.name == "CorridorWall")
                                        {
                                            coriWall.transform.GetChild(0).gameObject.SetActive(true);
                                        }

                                        if (parentRoom != null)
                                        {
                                            coriWall.transform.parent = parentRoom.transform;
                                        }
                                    }

                                }*/


                                foreach (GameObject db in DoorwaysL)
                                {
                                    db.name = "Def";
                                }

                                CorridorCreated = parentRoom;

                                DoorwaysL.Clear();

                                alreadyBuiltOne = true;

                                lastRoom = CorridorCreated;
                                tCounter += 1;
                            }
                        }
                        else
                        {
                            break;
                        }

                        break;

                    //Right Doorways
                    case 3:
                        int d4 = DoorwaysR.Count - 1;
                        if (d4 >= 1)
                        {
                            //int randoD4 = Random.Range(0, d4);
                            int randoD4 = 0;

                            bool hasNotOverlapped = false;
                            for (int dway = 0; dway < DoorwaysR.Count; dway++)
                            {
                                if (dway == 0)
                                {
                                    randoD4 = Mathf.RoundToInt(DoorwaysR.Count / 2);
                                }
                                else
                                {
                                    randoD4 = dway;
                                }


                                hasNotOverlapped = CheckNoOverlapWillOccur(blockLengthX, blockLengthZ, DoorwaysR[randoD4].transform.position, "Right");

                                if (hasNotOverlapped == false)
                                {
                                    Doorways.Remove(DoorwaysR[randoD4]);

                                    DoorwaysR[randoD4].name = "Def";

                                    DoorwaysR.Remove(DoorwaysR[randoD4]);

                                }
                                else
                                {
                                    break;
                                }

                            }

                            if (hasNotOverlapped == false)
                            {
                                break;
                            }


                            doorwayOne = DoorwaysR[randoD4];
                            spinRoom = 90;
                            TempDoorwaySpins.Add(spinRoom);
                            mRight = true;
                            doorwayOne.name = "UsedCorridor";

                            //For blocks to be added to their own parent
                            if (SeperateCorridorParents == true)
                            {

                                corridorParent = Instantiate(ParentObj, transform.position, transform.rotation);
                                //Maybe have to add in a position later
                                corridorParent.name = ("Corridor " + (corridorParentCounter + 1));
                                corridorParent.transform.parent = LevelContainer.transform;
                            }
                            corridorParentCounter += 1;

                            while (blocks.Count <= combinedBlockCount - 1)
                            {
                                if (blocks.Count == 0)
                                {
                                    //Place the first block
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    CorridorCreated = newB;


                                    Vector3 center = new Vector3(0, 0, 0);

                                    //Width check for wheter to plus or minus the size
                                    for (int ind = 0; ind < doorwayOne.transform.parent.transform.childCount; ind++)
                                    {
                                        if (doorwayOne.transform.transform.parent.GetChild(ind).gameObject.name == "CenterBlock")
                                        {
                                            center = doorwayOne.transform.parent.transform.GetChild(ind).transform.position;
                                            break;
                                        }
                                    }

                                    if (center.x == 0 && center.y == 0 && center.z == 0)
                                    {
                                        center = doorwayOne.transform.position;
                                    } 

                                    Vector3 newPos = doorwayOne.transform.position;

                                    float rounded = Mathf.Round(newPos.x + blockSizeX);

                                    if (newPos.z > center.z)
                                    {
                                        if (CorWCount + 2 < (RoomLCount * 0.5f) - 2f)
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z - (blockSizeZ + blockSizeZ));
                                        }
                                        else if (CorWCount + 2 < (RoomLCount * 0.5f))
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, (center.z - blockSizeZ));
                                        }
                                        else
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z);
                                        }
                                    }
                                    else if (newPos.z < center.z)
                                    {
                                        if (CorWCount + 2 < (RoomLCount * 0.5f) - 2f)
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z + (blockSizeZ + blockSizeZ));
                                        }
                                        else if (CorWCount + 2 < (RoomLCount * 0.5f))
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, (center.z + blockSizeZ));
                                        }
                                        else
                                        {
                                            newB.transform.position = new Vector3(rounded, 0, center.z);
                                        }
                                    }
                                    else
                                    {
                                        newB.transform.position = new Vector3(rounded, 0, center.z);
                                    }




                                    newB.name = "Entrance";
                                    checkEntrances.Add(newB);


                                    /*Vector3 left = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);
                                    Vector3 left2 = new Vector3(newB.transform.position.x - (blockSizeX * 2), 0, newB.transform.position.z);*/
                                    /*foreach (Transform childB in parentRoom.transform)
                                    {
                                        if (childB.transform.position == left)
                                        {
                                            if (childB.gameObject.transform.childCount >= 1)
                                            {
                                                childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                            }

                                            childB.name = "Used Corridor";
                                        }
                                        else if (childB.transform.position == left2)
                                        {
                                            if (childB.gameObject.transform.childCount >= 1)
                                            {
                                                if (childB.transform.childCount >= 1)
                                                {
                                                    childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                }
                                                childB.name = "Used Corridor";
                                            }


                                        }
                                        else
                                        {
                                            //Do Nothing
                                        }
                                    }*/

                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1 - add to one side
                                        else
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }


                                    posX = newB.transform.position.x;
                                    posZ = newB.transform.position.z;

                                    oldPosX = posX;
                                    oldPosZ = posZ;

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ < CorLCount)
                                {
                                    //Add blocks to the Z axis
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    lastBlock = newB;

                                    CorridorCreated = newB;

                                    float rounded = Mathf.Round(posX + blockSizeX);

                                    newB.transform.position = new Vector3(rounded, 0, posZ);




                                    if (tempCountZ == CorLCount - 1 && CorLCount > 1)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);

                                        Vector3 right = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                        Vector3 right2 = new Vector3(newB.transform.position.x + (blockSizeX * 2), 0, newB.transform.position.z);
                                        /*foreach (Transform childB in parentRoom.transform)
                                        {
                                            if((childB.name == "T" || childB.name == "B" || childB.name == "R" || childB.name == "L" || childB.name == "Def" || childB.name == "Wall"))
                                            {
                                                if (childB.transform.position == right)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                    childB.name = "Used Corridor";
                                                }
                                                else if (childB.transform.position == right2)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }
                                                    childB.name = "Used Corridor";
                                                }
                                            }
                                        }*/

                                    }
                                    else if (tempCountZ == 0)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);

                                        Vector3 left = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);
                                        Vector3 left2 = new Vector3(newB.transform.position.x - (blockSizeX * 2), 0, newB.transform.position.z);
                                        foreach (Transform childB in parentRoom.transform)
                                        {

                                            if (childB.transform.position == left)
                                            {
                                                if (childB.transform.childCount >= 1)
                                                {
                                                    childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                }

                                            }
                                            else if (childB.transform.position == left2)
                                            {
                                                if (childB.transform.childCount >= 1)
                                                {
                                                    childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                }

                                            }


                                        }
                                    }
                                    else
                                    {
                                        newB.name = "Floor";
                                    }

                                    posX = newB.transform.position.x;



                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1, and we are on the first row- add to one side
                                        else if (blockLengthX > 1 && tempCountX == 0)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                        //If Corridor is Greater than 1, but we are in the center (tempXCount - 1 == the end of corW - place nothing
                                        else if (blockLengthX > 1 && tempCountX > 0 && tempCountX < CorWCount - 1)
                                        {
                                            //Do nothing
                                        }
                                        //If corridor is Greater than 1, and we are on the last row - add to one side
                                        else if (blockLengthX > 1 && tempCountX == CorWCount - 1)
                                        {

                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }



                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }



                                    /*foreach (Transform child in LevelContainer.transform)
                                    {
                                        foreach (Transform blocky in child.transform)
                                        {
                                            if (newB.transform.position == blocky.position)
                                            {
                                                blocky.gameObject.SetActive(false);
                                                blocky.gameObject.name = "DELETE ME!";
                                                Deactivated.Add(blocky.gameObject);
                                            }
                                        }
                                    }*/



                                    tempCountZ += 1;

                                }
                                else if (tempCountZ >= CorLCount)
                                {
                                    tempCountZ = 0;
                                    tempCountX += 1;

                                    posZ = Mathf.Round(posZ + blockSizeZ);
                                    posX = Mathf.Round(oldPosX - blockSizeX);


                                }


                            }
                            if (blocks.Count > combinedBlockCount - 1)
                            {
                                if (parentRoom != null)
                                {
                                    foreach (GameObject blok in blocks)
                                    {
                                        if (SeperateCorridorParents == false)
                                        {
                                            //To add the corridor blocks to the current room as a parent
                                            blok.transform.parent = parentRoom.transform;
                                        }
                                        else
                                        {
                                            blok.transform.parent = corridorParent.transform;
                                        }

                                        if (blocks.IndexOf(blok) == blocks.Count - 1)
                                        {
                                            blok.transform.rotation = Quaternion.Euler(0, 0, 0);
                                            if (blocks.IndexOf(blok) == blocks.Count - 1)
                                            {
                                                blok.transform.rotation = Quaternion.Euler(0, 90, 0);
                                                Doorways.Add(blok);
                                                TempDoorways.Add(blok);
                                            }
                                        }

                                        if (OverlappingBlocks == true)
                                        {
                                            CheckOverlap(blok);
                                        }

                                        if (blok.name != "Entrance")
                                        {
                                            blok.name = "Corridor";
                                        }
                                    }

                                }

                                /*if (WalledCorridors == true)
                                {
                                    foreach (GameObject coriWall in corridorBlocks)
                                    {
                                        if (coriWall.transform.childCount > 0 && coriWall.transform.GetChild(0).gameObject.name == "CorridorWall")
                                        {
                                            coriWall.transform.GetChild(0).gameObject.SetActive(true);
                                        }

                                        if (parentRoom != null)
                                        {
                                            coriWall.transform.parent = parentRoom.transform;
                                        }
                                    }

                                }*/


                                foreach (GameObject db in DoorwaysR)
                                {
                                    db.name = "Def";
                                }

                                CorridorCreated = parentRoom;

                                DoorwaysR.Clear();

                                alreadyBuiltOne = true;

                                lastRoom = CorridorCreated;
                                tCounter += 1;

                            }
                        }
                        else
                        {
                            break;
                        }

                        break;
                }


                //For adding corridor walls to parent room - done here to stop conflictions with certain checks
                foreach (GameObject cori in corridorBlocks)
                {
                    if (SeperateCorridorParents == false)
                    {
                        cori.transform.parent = parentRoom.transform;
                    }
                    else
                    {
                        cori.transform.parent = corridorParent.transform;
                    }

                }

                /*if (WalledCorridors == true)
                {
                    foreach (GameObject cori in corridorBlocks)
                    {
                        CheckOverlap(cori);
                    }
                }*/




            }
            else
            {
                switch (chooseRandomDoor)
                {
                    //Top Doorways
                    case 0:
                        int d1 = DoorwaysT.Count - 1;
                        if (d1 >= 1)
                        {
                            //int randoD1 = Random.Range(0, d1);
                            int randoD1 = Mathf.RoundToInt(DoorwaysT.Count / 2);
                            doorwayOne = DoorwaysT[randoD1];
                            spinRoom = 0;
                            TempDoorwaySpins.Add(spinRoom);
                            mUp = true;
                            doorwayOne.name = "UsedCorridor";

                            //Debug.Log(parentRoom.name + " = Case: 0");

                            //For blocks to be added to their own parent
                            if (SeperateCorridorParents == true)
                            {

                                corridorParent = Instantiate(ParentObj, transform.position, transform.rotation);
                                //Maybe have to add in a position later
                                corridorParent.name = ("Corridor " + (corridorParentCounter + 1));
                                corridorParent.transform.parent = LevelContainer.transform;
                            }
                            corridorParentCounter += 1;


                            while (blocks.Count <= combinedBlockCount - 1)
                            {
                                if (blocks.Count == 0)
                                {
                                    //Place the first block


                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);

                                    Vector3 newPos = doorwayOne.transform.position;

                                    newB.transform.position = new Vector3(newPos.x, 0, newPos.z + blockSizeZ);
                                    newB.name = "Entrance";
                                    checkEntrances.Add(newB);


                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1 - add to one side
                                        else
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }




                                    posX = newB.transform.position.x;
                                    posZ = newB.transform.position.z;

                                    oldPosX = posX;
                                    oldPosZ = posZ;

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ < CorLCount)
                                {
                                    //Add blocks to the Z axis
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    lastBlock = newB;

                                    CorridorCreated = newB;


                                    float rounded = Mathf.Round(posZ + blockSizeZ);

                                    newB.transform.position = new Vector3(posX, 0, rounded);
                                    //newB.isStatic = true;


                                    if (tempCountZ == CorLCount - 1)
                                    {

                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);

                                        Vector3 above = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                        Vector3 above2 = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + (blockSizeZ * 2));
                                        /*foreach (Transform childB in parentRoom.transform)
                                        {
                                            if((childB.name == "T" || childB.name == "B" || childB.name == "R" || childB.name == "L" || childB.name == "Def" || childB.name == "Wall"))
                                            {
                                                if (childB.transform.position == above)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                    childB.name = "Used Corridor";
                                                }
                                                else if (childB.transform.position == above2)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                    childB.name = "Used Corridor";
                                                }
                                            }


                                        }*/
                                    }
                                    else if (tempCountZ == 0)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);
                                        Vector3 below = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);
                                        Vector3 below2 = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - (blockSizeZ * 2));
                                        foreach (Transform childB in parentRoom.transform)
                                        {
                                            if ((childB.name == "T" || childB.name == "B" || childB.name == "R" || childB.name == "L" || childB.name == "Def" || childB.name == "Wall"))
                                            {
                                                if (childB.transform.position == below)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                }
                                                else if (childB.transform.position == below2)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        newB.name = "Floor";
                                    }

                                    posZ = newB.transform.position.z;



                                    if (NoCorridorWalls == false)
                                    {


                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {

                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1, and we are on the first row- add to one side
                                        else if (blockLengthX > 1 && tempCountX == 0)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                        //If Corridor is Greater than 1, but we are in the center (tempXCount - 1 == the end of corW - place nothing
                                        else if (blockLengthX > 1 && tempCountX > 0 && tempCountX < CorWCount - 1)
                                        {
                                            //Do nothing
                                        }
                                        //If corridor is Greater than 1, and we are on the last row - add to one side
                                        else if (blockLengthX > 1 && tempCountX == CorWCount - 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;


                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }






                                    /*foreach (Transform child in LevelContainer.transform)
                                    {
                                        foreach (Transform blocky in child.transform)
                                        {
                                            if (newB.transform.position == blocky.position)
                                            {
                                                blocky.gameObject.SetActive(false);
                                                blocky.gameObject.name = "DELETE ME!";
                                                Deactivated.Add(blocky.gameObject);
                                            }
                                        }
                                    }*/

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ >= CorLCount)
                                {
                                    tempCountZ = 0;
                                    tempCountX += 1;

                                    posX = Mathf.Round(posX + blockSizeX);
                                    //posX = posX - blockSizeX;
                                    posZ = Mathf.Round(oldPosZ - blockSizeZ);
                                }


                            }
                            if (blocks.Count > combinedBlockCount - 1)
                            {
                                if (parentRoom != null)
                                {
                                    foreach (GameObject blok in blocks)
                                    {
                                        if (SeperateCorridorParents == false)
                                        {
                                            //To add the corridor blocks to the current room as a parent
                                            blok.transform.parent = parentRoom.transform;
                                        }
                                        else
                                        {
                                            blok.transform.parent = corridorParent.transform;
                                        }


                                        if (blocks.IndexOf(blok) == blocks.Count - 1)
                                        {
                                            blok.transform.rotation = Quaternion.Euler(0, 0, 0);
                                            Doorways.Add(blok);
                                            TempDoorways.Add(blok);

                                        }

                                        if (OverlappingBlocks == true)
                                        {
                                            CheckOverlap(blok);
                                        }

                                        if (blok.name != "Entrance")
                                        {
                                            blok.name = "Corridor";
                                        }
                                    }
                                }

                                /*foreach (Transform childB in parentRoom.transform)
                                {
                                    corridorOrNot = false;

                                    Vector3 thePosition = childB.transform.position;

                                    CheckTheEntrances(thePosition);

                                    if (corridorOrNot)
                                    {
                                        ChangeBlock(childB.gameObject);
                                    }


                                }*/

                                /*if (WalledCorridors == true)
                                {
                                    foreach (GameObject coriWall in corridorBlocks)
                                    {
                                        if (coriWall.transform.childCount > 0 && coriWall.transform.GetChild(0).gameObject.name == "CorridorWall")
                                        {
                                            coriWall.transform.GetChild(0).gameObject.SetActive(true);
                                        }

                                        if (parentRoom != null)
                                        {
                                            coriWall.transform.parent = parentRoom.transform;
                                        }
                                    }


                                }*/



                                CorridorCreated = parentRoom;

                                foreach (GameObject dt in DoorwaysT)
                                {
                                    dt.name = "Def";
                                }


                                DoorwaysT.Clear();
                                /*DoorwaysT.Clear();
                                DoorwaysB.Clear();
                                DoorwaysL.Clear();
                                DoorwaysR.Clear();*/
                                alreadyBuiltOne = true;

                                lastRoom = CorridorCreated;
                                tCounter += 1;

                            }
                        }
                        else
                        {
                            break;
                            //BuildCorridor(parentRoom);
                        }

                        break;

                    //Bottom Doorways
                    case 1:
                        int d2 = DoorwaysB.Count - 1;
                        if (d2 >= 1)
                        {
                            //int randoD2 = Random.Range(0, d2);
                            int randoD2 = Mathf.RoundToInt(DoorwaysB.Count / 2);
                            doorwayOne = DoorwaysB[randoD2];
                            spinRoom = 180;
                            TempDoorwaySpins.Add(spinRoom);
                            mDown = true;
                            doorwayOne.name = "UsedCorridor";

                            //For blocks to be added to their own parent
                            if (SeperateCorridorParents == true)
                            {

                                corridorParent = Instantiate(ParentObj, transform.position, transform.rotation);
                                //Maybe have to add in a position later
                                corridorParent.name = ("Corridor " + (corridorParentCounter + 1));
                                corridorParent.transform.parent = LevelContainer.transform;
                            }
                            corridorParentCounter += 1;


                            while (blocks.Count <= combinedBlockCount - 1)
                            {
                                if (blocks.Count == 0)
                                {
                                    //Place the first block
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    CorridorCreated = newB;


                                    Vector3 newPos = doorwayOne.transform.position;

                                    float rounded = Mathf.Round(newPos.z - blockSizeZ);

                                    newB.transform.position = new Vector3(newPos.x, 0, rounded);
                                    newB.name = "Entrance";
                                    checkEntrances.Add(newB);



                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1 - add to one side
                                        else
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }



                                    posX = newB.transform.position.x;
                                    posZ = newB.transform.position.z;

                                    oldPosX = posX;
                                    oldPosZ = posZ;

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ < CorLCount)
                                {
                                    //Add blocks to the Z axis
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    lastBlock = newB;

                                    CorridorCreated = newB;

                                    float rounded = Mathf.Round(posZ - blockSizeZ);

                                    newB.transform.position = new Vector3(posX, 0, rounded);



                                    if (tempCountZ == CorLCount - 1)
                                    {

                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);
                                    }
                                    else if (tempCountZ == 0)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);
                                        /*Vector3 above = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                        Vector3 above2 = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + (blockSizeZ * 2));
                                        foreach (Transform childB in parentRoom.transform)
                                        {
                                            if ((childB.name == "T" || childB.name == "B" || childB.name == "R" || childB.name == "L" || childB.name == "Def" || childB.name == "Wall"))
                                            {
                                                if (childB.transform.position == above)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                }
                                                else if (childB.transform.position == above2)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                }
                                            }

                                        }*/
                                    }
                                    else
                                    {
                                        newB.name = "Floor";
                                    }

                                    posZ = newB.transform.position.z;
                                    //posX = newB.transform.position.x;



                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1, and we are on the first row- add to one side
                                        else if (blockLengthX > 1 && tempCountX == 0)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";


                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                        }
                                        //If Corridor is Greater than 1, but we are in the center (tempXCount - 1 == the end of corW - place nothing
                                        else if (blockLengthX > 1 && tempCountX > 0 && tempCountX < CorWCount - 1)
                                        {
                                            //Do nothing
                                        }
                                        //If corridor is Greater than 1, and we are on the last row - add to one side
                                        else if (blockLengthX > 1 && tempCountX == CorWCount - 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }



                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }




                                    /*foreach (Transform child in LevelContainer.transform)
                                    {
                                        foreach (Transform blocky in child.transform)
                                        {
                                            if (newB.transform.position == blocky.position)
                                            {
                                                blocky.gameObject.SetActive(false);
                                                blocky.gameObject.name = "DELETE ME!";
                                                Deactivated.Add(blocky.gameObject);
                                            }
                                        }
                                    }*/

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ >= CorLCount)
                                {
                                    tempCountZ = 0;
                                    tempCountX += 1;
                                    //oldPosZ = 0 - blockSizeZ;
                                    //oldPosX = lastBlock.transform.position.x + blockSizeX;
                                    posX = Mathf.Round(posX - blockSizeX);
                                    //posX = posX - blockSizeX;
                                    posZ = Mathf.Round(oldPosZ + blockSizeZ);
                                }


                            }
                            if (blocks.Count > combinedBlockCount - 1)
                            {
                                if (parentRoom != null)
                                {
                                    foreach (GameObject blok in blocks)
                                    {
                                        if (SeperateCorridorParents == false)
                                        {
                                            //To add the corridor blocks to the current room as a parent
                                            blok.transform.parent = parentRoom.transform;
                                        }
                                        else
                                        {
                                            blok.transform.parent = corridorParent.transform;
                                        }


                                        if (blocks.IndexOf(blok) == blocks.Count - 1)
                                        {
                                            blok.transform.rotation = Quaternion.Euler(0, 180, 0);
                                            Doorways.Add(blok);
                                            TempDoorways.Add(blok);
                                        }

                                        if (OverlappingBlocks == true)
                                        {
                                            CheckOverlap(blok);
                                        }

                                        if (blok.name != "Entrance")
                                        {
                                            blok.name = "Corridor";
                                        }

                                    }

                                }

                                /*if (WalledCorridors == true)
                                {
                                    foreach (GameObject coriWall in corridorBlocks)
                                    {
                                        if (coriWall.transform.childCount > 0 && coriWall.transform.GetChild(0).gameObject.name == "CorridorWall")
                                        {
                                            coriWall.transform.GetChild(0).gameObject.SetActive(true);
                                        }

                                        if (parentRoom != null)
                                        {
                                            coriWall.transform.parent = parentRoom.transform;
                                        }
                                    }

                                }*/


                                CorridorCreated = parentRoom;

                                foreach (GameObject db in DoorwaysB)
                                {
                                    db.name = "Def";
                                }

                                DoorwaysB.Clear();



                                alreadyBuiltOne = true;
                                lastRoom = CorridorCreated;
                                tCounter += 1;
                            }
                        }
                        else
                        {
                            break;
                        }
                        break;

                    //Left Doorways
                    case 2:
                        int d3 = DoorwaysL.Count - 1;
                        if (d3 >= 1)
                        {
                            //int randoD3 = Random.Range(0, d3);
                            int randoD3 = Mathf.RoundToInt(DoorwaysL.Count / 2);
                            doorwayOne = DoorwaysL[randoD3];
                            spinRoom = -90;
                            TempDoorwaySpins.Add(spinRoom);
                            mLeft = true;
                            doorwayOne.name = "UsedCorridor";

                            //For blocks to be added to their own parent
                            if (SeperateCorridorParents == true)
                            {

                                corridorParent = Instantiate(ParentObj, transform.position, transform.rotation);
                                //Maybe have to add in a position later
                                corridorParent.name = ("Corridor " + (corridorParentCounter + 1));
                                corridorParent.transform.parent = LevelContainer.transform;
                            }
                            corridorParentCounter += 1;

                            while (blocks.Count <= combinedBlockCount - 1)
                            {
                                if (blocks.Count == 0)
                                {
                                    //Place the first block
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    CorridorCreated = newB;

                                    Vector3 newPos = doorwayOne.transform.position;

                                    //Debug.Log(doorwayOne.transform.position);

                                    float rounded = Mathf.Round(newPos.x - blockSizeX);

                                    newB.transform.position = new Vector3(rounded, 0, newPos.z);


                                    newB.name = "Entrance";
                                    checkEntrances.Add(newB);


                                    Vector3 right = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                    Vector3 right2 = new Vector3(newB.transform.position.x + (blockSizeX * 2), 0, newB.transform.position.z);
                                    /*foreach (Transform childB in parentRoom.transform)
                                    {
                                        if (childB.transform.position == right)
                                        {
                                            if (childB.gameObject.transform.childCount >= 1)
                                            {
                                                childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                            }


                                            childB.name = "Used Corridor";
                                        }
                                        else if(childB.transform.position == right2)
                                        {
                                            if (childB.gameObject.transform.childCount >= 1)
                                            {
                                                childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                            }


                                            childB.name = "Used Corridor";
                                        }
                                        else
                                        {
                                            //Do Nothing
                                        }



                                    }*/

                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1 - add to one side
                                        else
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }


                                    posX = newB.transform.position.x;
                                    posZ = newB.transform.position.z;

                                    oldPosX = posX;
                                    oldPosZ = posZ;


                                    tempCountZ += 1;

                                }
                                else if (tempCountZ < CorLCount)
                                {
                                    //Add blocks to the Z axis
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    lastBlock = newB;

                                    CorridorCreated = newB;

                                    float rounded = Mathf.Round(posX - blockSizeX);

                                    newB.transform.position = new Vector3(rounded, 0, posZ);





                                    if (tempCountZ == CorLCount - 1 && CorLCount > 1)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);
                                        Vector3 left = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);
                                        Vector3 left2 = new Vector3(newB.transform.position.x - (blockSizeX * 2), 0, newB.transform.position.z);
                                        /*foreach (Transform childB in parentRoom.transform)
                                        {
                                            if((childB.name == "T" || childB.name == "B" || childB.name == "R" || childB.name == "L" || childB.name == "Def" || childB.name == "Wall"))
                                            {
                                                if (childB.transform.position == left)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }
                                                    childB.name = "Used Corridor";
                                                }
                                                else if (childB.transform.position == left2)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }
                                                    childB.name = "Used Corridor";
                                                }
                                            }

                                        }*/
                                    }
                                    else if (tempCountZ == 0)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);
                                        /*Vector3 right = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                        Vector3 right2 = new Vector3(newB.transform.position.x + (blockSizeX * 2), 0, newB.transform.position.z);
                                        foreach (Transform childB in parentRoom.transform)
                                        {

                                            if (childB.transform.position == right)
                                            {
                                                if (childB.transform.childCount >= 1)
                                                {
                                                    childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                }
                                            }
                                            else if (childB.transform.position == right2)
                                            {
                                                if (childB.transform.childCount >= 1)
                                                {
                                                    childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                }
                                            }


                                        }*/
                                    }
                                    else
                                    {
                                        newB.name = "Floor";
                                    }

                                    posX = newB.transform.position.x;





                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1, and we are on the first row- add to one side
                                        else if (blockLengthX > 1 && tempCountX == 0)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                        //If Corridor is Greater than 1, but we are in the center (tempXCount - 1 == the end of corW - place nothing
                                        else if (blockLengthX > 1 && tempCountX > 0 && tempCountX < CorWCount - 1)
                                        {
                                            //Do nothing
                                        }
                                        //If corridor is Greater than 1, and we are on the last row - add to one side
                                        else if (blockLengthX > 1 && tempCountX == CorWCount - 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);


                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;


                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }



                                    /*foreach (Transform child in LevelContainer.transform)
                                    {
                                        foreach (Transform blocky in child.transform)
                                        {
                                            if (newB.transform.position == blocky.position)
                                            {
                                                blocky.gameObject.SetActive(false);
                                                blocky.gameObject.name = "DELETE ME!";
                                                Deactivated.Add(blocky.gameObject);
                                            }
                                        }
                                    }*/

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ >= CorLCount)
                                {
                                    tempCountZ = 0;
                                    tempCountX += 1;

                                    posZ = Mathf.Round(posZ + blockSizeZ);
                                    posX = Mathf.Round(oldPosX + blockSizeX);


                                }


                            }
                            if (blocks.Count > combinedBlockCount - 1)
                            {
                                if (parentRoom != null)
                                {
                                    foreach (GameObject blok in blocks)
                                    {
                                        if (SeperateCorridorParents == false)
                                        {
                                            //To add the corridor blocks to the current room as a parent
                                            blok.transform.parent = parentRoom.transform;
                                        }
                                        else
                                        {
                                            blok.transform.parent = corridorParent.transform;
                                        }


                                        if (blocks.IndexOf(blok) == blocks.Count - 1)
                                        {
                                            blok.transform.rotation = Quaternion.Euler(0, -90, 0);
                                            Doorways.Add(blok);
                                            TempDoorways.Add(blok);
                                        }

                                        if (OverlappingBlocks == true)
                                        {
                                            CheckOverlap(blok);
                                        }

                                        if (blok.name != "Entrance")
                                        {
                                            blok.name = "Corridor";
                                        }
                                    }

                                }

                                /*if (WalledCorridors == true)
                                {
                                    foreach (GameObject coriWall in corridorBlocks)
                                    {
                                        if (coriWall.transform.childCount > 0 && coriWall.transform.GetChild(0).gameObject.name == "CorridorWall")
                                        {
                                            coriWall.transform.GetChild(0).gameObject.SetActive(true);
                                        }

                                        if (parentRoom != null)
                                        {
                                            coriWall.transform.parent = parentRoom.transform;
                                        }
                                    }

                                }*/


                                foreach (GameObject db in DoorwaysL)
                                {
                                    db.name = "Def";
                                }

                                CorridorCreated = parentRoom;

                                DoorwaysL.Clear();

                                alreadyBuiltOne = true;

                                lastRoom = CorridorCreated;
                                tCounter += 1;
                            }
                        }
                        else
                        {
                            break;
                        }

                        break;

                    //Right Doorways
                    case 3:
                        int d4 = DoorwaysR.Count - 1;
                        if (d4 >= 1)
                        {
                            //int randoD4 = Random.Range(0, d4);
                            int randoD4 = Mathf.RoundToInt(DoorwaysR.Count / 2);
                            doorwayOne = DoorwaysR[randoD4];
                            spinRoom = 90;
                            TempDoorwaySpins.Add(spinRoom);
                            mRight = true;
                            doorwayOne.name = "UsedCorridor";

                            //For blocks to be added to their own parent
                            if (SeperateCorridorParents == true)
                            {

                                corridorParent = Instantiate(ParentObj, transform.position, transform.rotation);
                                //Maybe have to add in a position later
                                corridorParent.name = ("Corridor " + (corridorParentCounter + 1));
                                corridorParent.transform.parent = LevelContainer.transform;
                            }
                            corridorParentCounter += 1;

                            while (blocks.Count <= combinedBlockCount - 1)
                            {
                                if (blocks.Count == 0)
                                {
                                    //Place the first block
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    CorridorCreated = newB;

                                    Vector3 newPos = doorwayOne.transform.position;


                                    float rounded = Mathf.Round(newPos.x + blockSizeX);

                                    newB.transform.position = new Vector3(rounded, 0, newPos.z);



                                    newB.name = "Entrance";
                                    checkEntrances.Add(newB);


                                    Vector3 left = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);
                                    Vector3 left2 = new Vector3(newB.transform.position.x - (blockSizeX * 2), 0, newB.transform.position.z);
                                    /*foreach (Transform childB in parentRoom.transform)
                                    {
                                        if (childB.transform.position == left)
                                        {
                                            if (childB.gameObject.transform.childCount >= 1)
                                            {
                                                childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                            }

                                            childB.name = "Used Corridor";
                                        }
                                        else if (childB.transform.position == left2)
                                        {
                                            if (childB.gameObject.transform.childCount >= 1)
                                            {
                                                if (childB.transform.childCount >= 1)
                                                {
                                                    childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                }
                                                childB.name = "Used Corridor";
                                            }


                                        }
                                        else
                                        {
                                            //Do Nothing
                                        }
                                    }*/

                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1 - add to one side
                                        else
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }


                                    posX = newB.transform.position.x;
                                    posZ = newB.transform.position.z;

                                    oldPosX = posX;
                                    oldPosZ = posZ;

                                    tempCountZ += 1;

                                }
                                else if (tempCountZ < CorLCount)
                                {
                                    //Add blocks to the Z axis
                                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);
                                    blocks.Add(newB);
                                    lastBlock = newB;

                                    CorridorCreated = newB;

                                    float rounded = Mathf.Round(posX + blockSizeX);

                                    newB.transform.position = new Vector3(rounded, 0, posZ);




                                    if (tempCountZ == CorLCount - 1 && CorLCount > 1)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);

                                        Vector3 right = new Vector3(newB.transform.position.x + blockSizeX, 0, newB.transform.position.z);
                                        Vector3 right2 = new Vector3(newB.transform.position.x + (blockSizeX * 2), 0, newB.transform.position.z);
                                        /*foreach (Transform childB in parentRoom.transform)
                                        {
                                            if((childB.name == "T" || childB.name == "B" || childB.name == "R" || childB.name == "L" || childB.name == "Def" || childB.name == "Wall"))
                                            {
                                                if (childB.transform.position == right)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }

                                                    childB.name = "Used Corridor";
                                                }
                                                else if (childB.transform.position == right2)
                                                {
                                                    if (childB.transform.childCount >= 1)
                                                    {
                                                        childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                    }
                                                    childB.name = "Used Corridor";
                                                }
                                            }
                                        }*/

                                    }
                                    else if (tempCountZ == 0)
                                    {
                                        newB.name = "Entrance";
                                        checkEntrances.Add(newB);

                                        Vector3 left = new Vector3(newB.transform.position.x - blockSizeX, 0, newB.transform.position.z);
                                        Vector3 left2 = new Vector3(newB.transform.position.x - (blockSizeX * 2), 0, newB.transform.position.z);
                                        foreach (Transform childB in parentRoom.transform)
                                        {

                                            if (childB.transform.position == left)
                                            {
                                                if (childB.transform.childCount >= 1)
                                                {
                                                    childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                }

                                            }
                                            else if (childB.transform.position == left2)
                                            {
                                                if (childB.transform.childCount >= 1)
                                                {
                                                    childB.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                                }

                                            }


                                        }
                                    }
                                    else
                                    {
                                        newB.name = "Floor";
                                    }

                                    posX = newB.transform.position.x;



                                    if (NoCorridorWalls == false)
                                    {
                                        //If Corridor width is 1 - add to both sides
                                        if (blockLengthX <= 1)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);
                                            GameObject corridorWall2 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);
                                            corridorWall2.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";
                                            corridorWall2.name = "CorridorWall";
                                            corridorWall2.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;
                                            //corridorWall2.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                                CheckOverlap(corridorWall2);
                                            }

                                            corridorBlocks.Add(corridorWall1);
                                            corridorBlocks.Add(corridorWall2);
                                        }
                                        //If Corridor width is Greater than 1, and we are on the first row- add to one side
                                        else if (blockLengthX > 1 && tempCountX == 0)
                                        {
                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z - blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }


                                            corridorBlocks.Add(corridorWall1);
                                        }
                                        //If Corridor is Greater than 1, but we are in the center (tempXCount - 1 == the end of corW - place nothing
                                        else if (blockLengthX > 1 && tempCountX > 0 && tempCountX < CorWCount - 1)
                                        {
                                            //Do nothing
                                        }
                                        //If corridor is Greater than 1, and we are on the last row - add to one side
                                        else if (blockLengthX > 1 && tempCountX == CorWCount - 1)
                                        {

                                            //Placing Corridor blocks on the Z axis, so will add walls to the X axis
                                            GameObject corridorWall1 = Instantiate(WallBlock, transform.position, transform.rotation);

                                            //Put it in the same Z position as the new corridor block above, and add the first to it's x, and minus the second from it's x
                                            corridorWall1.transform.position = new Vector3(newB.transform.position.x, 0, newB.transform.position.z + blockSizeZ);

                                            //Change names for later checks
                                            corridorWall1.name = "CorridorWall";
                                            corridorWall1.transform.GetChild(0).gameObject.name = "CorridorWall";

                                            //corridorWall1.transform.parent = parentRoom.transform;

                                            if (OverlappingBlocks == true)
                                            {
                                                CheckOverlap(corridorWall1);
                                            }



                                            corridorBlocks.Add(corridorWall1);
                                        }
                                    }



                                    /*foreach (Transform child in LevelContainer.transform)
                                    {
                                        foreach (Transform blocky in child.transform)
                                        {
                                            if (newB.transform.position == blocky.position)
                                            {
                                                blocky.gameObject.SetActive(false);
                                                blocky.gameObject.name = "DELETE ME!";
                                                Deactivated.Add(blocky.gameObject);
                                            }
                                        }
                                    }*/



                                    tempCountZ += 1;

                                }
                                else if (tempCountZ >= CorLCount)
                                {
                                    tempCountZ = 0;
                                    tempCountX += 1;

                                    posZ = Mathf.Round(posZ + blockSizeZ);
                                    posX = Mathf.Round(oldPosX - blockSizeX);


                                }


                            }
                            if (blocks.Count > combinedBlockCount - 1)
                            {
                                if (parentRoom != null)
                                {
                                    foreach (GameObject blok in blocks)
                                    {

                                        if (SeperateCorridorParents == false)
                                        {
                                            //To add the corridor blocks to the current room as a parent
                                            blok.transform.parent = parentRoom.transform;
                                        }
                                        else
                                        {
                                            blok.transform.parent = corridorParent.transform;
                                        }


                                        if (blocks.IndexOf(blok) == blocks.Count - 1)
                                        {
                                            blok.transform.rotation = Quaternion.Euler(0, 0, 0);
                                            if (blocks.IndexOf(blok) == blocks.Count - 1)
                                            {
                                                blok.transform.rotation = Quaternion.Euler(0, 90, 0);
                                                Doorways.Add(blok);
                                                TempDoorways.Add(blok);
                                            }
                                        }

                                        if (OverlappingBlocks == true)
                                        {
                                            CheckOverlap(blok);
                                        }

                                        if (blok.name != "Entrance")
                                        {
                                            blok.name = "Corridor";
                                        }
                                    }

                                }

                                /*if (WalledCorridors == true)
                                {
                                    foreach (GameObject coriWall in corridorBlocks)
                                    {
                                        if (coriWall.transform.childCount > 0 && coriWall.transform.GetChild(0).gameObject.name == "CorridorWall")
                                        {
                                            coriWall.transform.GetChild(0).gameObject.SetActive(true);
                                        }

                                        if (parentRoom != null)
                                        {
                                            coriWall.transform.parent = parentRoom.transform;
                                        }
                                    }

                                }*/


                                foreach (GameObject db in DoorwaysR)
                                {
                                    db.name = "Def";
                                }

                                CorridorCreated = parentRoom;

                                DoorwaysR.Clear();

                                alreadyBuiltOne = true;

                                lastRoom = CorridorCreated;
                                tCounter += 1;

                            }
                        }
                        else
                        {
                            break;
                        }

                        break;
                }

                //For adding corridor walls to parent room and checking after - done here to stop conflictions with certain checks
                foreach (GameObject cori in corridorBlocks)
                {
                    if (SeperateCorridorParents == false)
                    {
                        cori.transform.parent = parentRoom.transform;
                    }
                    else
                    {
                        cori.transform.parent = corridorParent.transform;
                    }

                }
                if (NoCorridorWalls == false && OverlappingBlocks == true)
                {
                    foreach (GameObject cori in corridorBlocks)
                    {
                        CheckOverlap(cori);
                    }
                }




            }



        }

    }
    //Code for choosing a different room if current room has no available doorways.
    public void ChooseARandomRoom()
    {


        int random = LevelContainer.transform.childCount - chooseRoom;

        if (chooseRoom >= placedRooms)
        {
            chooseRoom = 1;
            random = LevelContainer.transform.childCount - chooseRoom;
        }

        DoorwaysT.Clear();
        DoorwaysB.Clear();
        DoorwaysL.Clear();
        DoorwaysR.Clear();

        GameObject ChoosenRoom = LevelContainer.transform.GetChild(random).gameObject;



        foreach (Transform child in ChoosenRoom.transform)
        {
            if (child.gameObject.name == "B")
            {
                DoorwaysB.Add(child.gameObject);
            }
            else if (child.gameObject.name == "T")
            {
                DoorwaysT.Add(child.gameObject);
            }
            else if (child.gameObject.name == "L")
            {
                DoorwaysL.Add(child.gameObject);
            }
            else if (child.gameObject.name == "R")
            {
                DoorwaysR.Add(child.gameObject);
            }

        }

        if (DoorwaysB.Count <= 0 && DoorwaysT.Count <= 0 && DoorwaysL.Count <= 0 && DoorwaysR.Count <= 0)
        {
            chooseRoom += 1;
            ChooseARandomRoom();


        }
        else
        {

            alreadyBuiltOne = false;

            int r = Random.Range(1, 4);


            if (placedRooms <= HowManyRooms - 4)
            {
                BuildCorridors(ChoosenRoom, r);
            }
            else if (placedRooms == HowManyRooms - 3)
            {
                r = Random.Range(1, 3);
                BuildCorridors(ChoosenRoom, r);
            }
            else if (placedRooms == HowManyRooms - 2)
            {
                r = 1;
                BuildCorridors(ChoosenRoom, r);
            }
            else if (placedRooms == HowManyRooms - 1)
            {
                //Debug.Log(Rooms.Count);
                r = 1;
                BuildCorridors(ChoosenRoom, r);
            }
            else
            {
                //Do nothing
                r = 1;
                BuildCorridors(ChoosenRoom, r);
            }

        }


    }

    //

    //Doorway Code

    //Check our entrances and call the functions below to create doorways
    public void ClearEntrancesIfObstructed()
    {
        foreach (GameObject ent in checkEntrances)
        {
            Vector3 tPosXAdd = new Vector3(ent.transform.position.x + blockSizeX, 0, ent.transform.position.z);
            Vector3 tPosZAdd = new Vector3(ent.transform.position.x, 0, ent.transform.position.z + blockSizeZ);

            Vector3 tPosXMinus = new Vector3(ent.transform.position.x - blockSizeX, 0, ent.transform.position.z);
            Vector3 tPosZMinus = new Vector3(ent.transform.position.x, 0, ent.transform.position.z - blockSizeZ);

            Vector3 tPosXAddExtra = new Vector3(ent.transform.position.x + blockSizeX, 0, ent.transform.position.z - blockSizeZ);
            Vector3 tPosZAddExtra = new Vector3(ent.transform.position.x - blockSizeX, 0, ent.transform.position.z + blockSizeZ);

            Vector3 tPosXMinusExtra = new Vector3(ent.transform.position.x - blockSizeX, 0, ent.transform.position.z - blockSizeZ);
            Vector3 tPosZMinusExtra = new Vector3(ent.transform.position.x + blockSizeX, 0, ent.transform.position.z + blockSizeZ);


            for (int i = 0; i < AllBlocksInUse.Count; i++)
            {
                GameObject blocky = AllBlocksInUse[i];


                if (blocky.gameObject.name == "Def" || blocky.gameObject.name == "T" || blocky.gameObject.name == "B" || blocky.gameObject.name == "L" || blocky.gameObject.name == "R" || blocky.name == "Wall")
                {


                    if (blocky.transform.position == tPosXAdd || blocky.transform.position == tPosZAdd || blocky.transform.position == tPosXMinus || blocky.transform.position == tPosZMinus)
                    {
                        ChangeBlock(blocky.gameObject);
                    }
                }
            }

            /*//First check Whether we should change around entrance or the entrance itself 
            bool AreWeSurrounded = CheckAroundBeforeChangeBlock(ent);

            if(AreWeSurrounded == false)
            {
                GameObject newb = Instantiate(WallBlock.gameObject, transform.position, transform.rotation);
                newb.transform.position = ent.transform.position;
                newb.name = "Wall1";
                newb.transform.parent = ent.transform.parent;
                ent.name = "DELETE ME!";
                Deactivated.Add(ent);
                ent.SetActive(false);
                
            }
            else
            {
                Vector3 tPosXAdd = new Vector3(ent.transform.position.x + blockSizeX, 0, ent.transform.position.z);
                Vector3 tPosZAdd = new Vector3(ent.transform.position.x, 0, ent.transform.position.z + blockSizeZ);

                Vector3 tPosXMinus = new Vector3(ent.transform.position.x - blockSizeX, 0, ent.transform.position.z);
                Vector3 tPosZMinus = new Vector3(ent.transform.position.x, 0, ent.transform.position.z - blockSizeZ);

                Vector3 tPosXAddExtra = new Vector3(ent.transform.position.x + blockSizeX, 0, ent.transform.position.z - blockSizeZ);
                Vector3 tPosZAddExtra = new Vector3(ent.transform.position.x - blockSizeX, 0, ent.transform.position.z + blockSizeZ);

                Vector3 tPosXMinusExtra = new Vector3(ent.transform.position.x - blockSizeX, 0, ent.transform.position.z - blockSizeZ);
                Vector3 tPosZMinusExtra = new Vector3(ent.transform.position.x + blockSizeX, 0, ent.transform.position.z + blockSizeZ);


                for (int i = 0; i < AllBlocksInUse.Count; i++)
                {
                    GameObject blocky = AllBlocksInUse[i];


                    if (blocky.gameObject.name == "Def" || blocky.gameObject.name == "T" || blocky.gameObject.name == "B" || blocky.gameObject.name == "L" || blocky.gameObject.name == "R" || blocky.name == "Wall")
                    {


                        if (blocky.transform.position == tPosXAdd || blocky.transform.position == tPosZAdd || blocky.transform.position == tPosXMinus || blocky.transform.position == tPosZMinus)
                        {
                            ChangeBlock(blocky.gameObject);
                        }
                    }
                }


            }*/

        }



        lastEntrancesCheked = true;

    }
    //Check we can change the block we are trying to change 
    public bool CheckAroundBeforeChangeBlock(GameObject blockToChange)
    {
        Vector3 tPosXAdd = new Vector3(blockToChange.transform.position.x + blockSizeX, 0, blockToChange.transform.position.z);
        Vector3 tPosZAdd = new Vector3(blockToChange.transform.position.x, 0, blockToChange.transform.position.z + blockSizeZ);

        Vector3 tPosXMinus = new Vector3(blockToChange.transform.position.x - blockSizeX, 0, blockToChange.transform.position.z);
        Vector3 tPosZMinus = new Vector3(blockToChange.transform.position.x, 0, blockToChange.transform.position.z - blockSizeZ);


        bool top = false;
        bool left = false;
        bool bottom = false;
        bool right = false;

        turnDoor = false;

        foreach (Transform child in LevelContainer.transform)
        {
            foreach (Transform blocky in child.transform)
            {

                if (blocky.position == tPosXAdd)
                {
                    left = true;

                    if (blocky.gameObject.name == "Def" || blocky.gameObject.name == "T" || blocky.gameObject.name == "B" || blocky.gameObject.name == "L" || blocky.gameObject.name == "R" || blocky.name == "Wall")
                    {
                        turnDoor = true;
                    }
                }
                else if (blocky.position == tPosZAdd)
                {
                    bottom = true;
                }
                else if (blocky.position == tPosXMinus)
                {
                    right = true;
                    if (blocky.gameObject.name == "Def" || blocky.gameObject.name == "T" || blocky.gameObject.name == "B" || blocky.gameObject.name == "L" || blocky.gameObject.name == "R" || blocky.name == "Wall")
                    {
                        turnDoor = true;
                    }
                }
                else if (blocky.position == tPosZMinus)
                {
                    top = true;
                }


                if (top && bottom && left && right)
                {
                    break;
                }

            }

            if (top && bottom && left && right)
            {
                break;
            }
        }


        if (left && right && top && bottom)
        {
            return true;
        }
        else
        {
            return false;
        }




    }
    //Simple code to change the name of object (used very rarely).
    public void ChangeBlock(GameObject blockToChange)
    {
        bool canWeChange = CheckAroundBeforeChangeBlock(blockToChange);

        if (canWeChange == true && NoCorridorWalls == false && OverlappingBlocks == false)
        {
            if (DoorwayBlock == null)
            {
                blockToChange.name = "Doorway";
                blockToChange.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                //Get the doorways position
                Vector3 bPos = blockToChange.transform.position;

                //Instantiate the new gameObject
                GameObject newB = Instantiate(DoorwayBlock.gameObject, transform.position, transform.rotation);

                //Change name, parent, and position
                newB.gameObject.name = "Doorway";
                newB.transform.parent = blockToChange.transform.parent;
                newB.transform.position = bPos;

                //Change the rotation if needed - important for doorways slimmer than the doorway block
                //Don't want to rotate rectangular blocks
                bool rectangular = false;
                if(blockSizeX != blockSizeZ)
                {
                    rectangular = true;
                }
                else
                {
                    rectangular = false;
                }


                if (turnDoor == true && rectangular == false)
                {
                    newB.transform.eulerAngles = new Vector3(0f, 90f, 0f);
                }

                blockToChange.name = "DELETE ME!";
                blockToChange.SetActive(false);
                Deactivated.Add(blockToChange);
                AllBlocksInUse.Remove(blockToChange);

            }

        }
        else if(canWeChange == true )
        {
            //Get the doorways position
            Vector3 bPos = blockToChange.transform.position;

            //Instantiate the new gameObject
            GameObject newB = Instantiate(FloorBlock.gameObject, transform.position, transform.rotation);

            //Change name, parent, and position
            newB.gameObject.name = "Floor";
            newB.transform.parent = blockToChange.transform.parent;
            newB.transform.position = bPos;



            blockToChange.name = "DELETE ME!";
            blockToChange.SetActive(false);
            Deactivated.Add(blockToChange);
            AllBlocksInUse.Remove(blockToChange);
        }
        else
        {
            //Nothing
        }
    }
   


    /*public void CheckTheEntrances(Vector3 newB)
    {
        foreach (GameObject ent in checkEntrances)
        {

            Vector3 tPosXAdd = new Vector3(ent.transform.position.x + blockSizeX, 0, ent.transform.position.z);
            //Vector3 tPosXAddExtra = new Vector3(ent.transform.position.x + (blockSizeX * 2), 0, ent.transform.position.z);
            Vector3 tPosZAdd = new Vector3(ent.transform.position.x, 0, ent.transform.position.z + blockSizeZ);
            //Vector3 tPosZAddExtra = new Vector3(ent.transform.position.x, 0, ent.transform.position.z + (blockSizeZ * 2));

            Vector3 tPosXMinus = new Vector3(ent.transform.position.x - blockSizeX, 0, ent.transform.position.z);
            //Vector3 tPosXMinusExtra = new Vector3(ent.transform.position.x - (blockSizeX * 2), 0, ent.transform.position.z);
            Vector3 tPosZMinus = new Vector3(ent.transform.position.x, 0, ent.transform.position.z - blockSizeZ);
            //Vector3 tPosZMinusExtra = new Vector3(ent.transform.position.x, 0, ent.transform.position.z - (blockSizeZ * 2));



            if (ent.name != "Corridor")
            {
                if (newB == tPosXMinus)
                {
                    corridorOrNot = true;
                }

                

                if (newB == tPosZMinus)
                {
                    corridorOrNot = true; ;
                }

                if (newB == tPosXAdd)
                {
                    corridorOrNot = true;
                }


                if (newB == tPosZAdd)
                {
                    corridorOrNot = true;
                }

            }




        }
    }
*/
    /*public void CheckForHoles()
    {
        foreach (Transform Lv in LevelContainer.transform)
        {
            foreach (Transform lvChild in Lv.transform)
            {
                GameObject b = lvChild.gameObject;
                if (b.gameObject.name == "Doorway" || b.gameObject.name == "Entrance" || b.gameObject.name == "Wall" || (b.gameObject.name == "Floor" && b.transform.childCount >= 1))
                {
                    GameObject blockToChange = b;
                    Vector3 tPosXAdd = new Vector3(blockToChange.transform.position.x + blockSizeX, 0, blockToChange.transform.position.z);
                    Vector3 tPosZAdd = new Vector3(blockToChange.transform.position.x, 0, blockToChange.transform.position.z + blockSizeZ);

                    Vector3 tPosXMinus = new Vector3(blockToChange.transform.position.x - blockSizeX, 0, blockToChange.transform.position.z);
                    Vector3 tPosZMinus = new Vector3(blockToChange.transform.position.x, 0, blockToChange.transform.position.z - blockSizeZ);


                    bool top = false;
                    bool left = false;
                    bool bottom = false;
                    bool right = false;

                    foreach (Transform child in LevelContainer.transform)
                    {
                        foreach (Transform blocky in child.transform)
                        {

                            if (blocky.position == tPosXAdd)
                            {
                                left = true;
                            }
                            else if (blocky.position == tPosZAdd)
                            {
                                bottom = true;
                            }
                            else if (blocky.position == tPosXMinus)
                            {
                                right = true;
                            }
                            else if (blocky.position == tPosZMinus)
                            {
                                top = true;
                            }

                            if (left && right && top && bottom)
                            {
                                break;
                            }

                        }

                        if (left && right && top && bottom)
                        {
                            break;
                        }
                    }

                    if (!left && !right && !top && !bottom)
                    {
                        if (blockToChange.name == "Doorway")
                        {
                            blockToChange.name = "Wall";
                            blockToChange.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else
                        {
                            GameObject newb = Instantiate(WallBlock, transform.position, transform.rotation);
                            Vector3 newPos = new Vector3(blockToChange.transform.position.x, 0, blockToChange.transform.position.z);

                            newb.transform.position = newPos;
                            newb.transform.parent = blockToChange.transform.parent.transform;

                            newb.gameObject.name = "Wall";

                            AllBlocksInUse.Add(newb);

                            blockToChange.name = "DELETE ME!";
                            Deactivated.Add(blockToChange);
                            blockToChange.SetActive(false);


                        }
                    }


                }
            }
        }

        CorridorWallsChecked = true;
    }*/

    //

    //Checks - Mostly for Overlapping Blocks
    public void CheckNoHolesInWallsAndNoExtraWalls()
    {
        //List<GameObject> tempAllBlocks = new List<GameObject>();

        List<GameObject> tempWallBlocks = new List<GameObject>();

        for (int i = 0; i < AllBlocksInUse.Count; i++)
        {
            GameObject b = AllBlocksInUse[i].gameObject;
            if (b.gameObject.name == "Def" || b.gameObject.name == "Wall" || b.gameObject.name == "CorridorWall" || b.gameObject.name == "T" || b.gameObject.name == "R" || b.gameObject.name == "L" || b.gameObject.name == "B" || b.name == "Entrance" || b.name == "Doorway" /*|| b.name == "Corridor"*/)//Using a name to try and lessen the check load
            {



                //tempAllBlocks.Add(b.gameObject);
                Vector3 tPosXAdd = new Vector3(b.transform.position.x + blockSizeX, 0, b.transform.position.z);
                Vector3 tPosXAddExtra = new Vector3(b.transform.position.x + blockSizeX, 0, b.transform.position.z - blockSizeZ);
                Vector3 tPosZAdd = new Vector3(b.transform.position.x, 0, b.transform.position.z + blockSizeZ);
                Vector3 tPosZAddExtra = new Vector3(b.transform.position.x - blockSizeX, 0, b.transform.position.z + blockSizeZ);

                Vector3 tPosXMinus = new Vector3(b.transform.position.x - blockSizeX, 0, b.transform.position.z);
                Vector3 tPosXMinusExtra = new Vector3(b.transform.position.x - blockSizeX, 0, b.transform.position.z - blockSizeZ);
                Vector3 tPosZMinus = new Vector3(b.transform.position.x, 0, b.transform.position.z - blockSizeZ);
                Vector3 tPosZMinusExtra = new Vector3(b.transform.position.x + blockSizeX, 0, b.transform.position.z + blockSizeZ);



                bool top = false;
                bool left = false;
                bool bottom = false;
                bool right = false;

                bool top2 = false;
                bool left2 = false;
                bool bottom2 = false;
                bool right2 = false;

                //Check all the areas around our block
                for (int ii = 0; ii < AllBlocksInUse.Count; ii++)
                {
                    GameObject blocky = AllBlocksInUse[ii].gameObject;


                    if (blocky.transform.position == tPosXAdd)
                    {
                        left = true;
                    }
                    else if (blocky.transform.position == tPosZAdd)
                    {
                        bottom = true;
                    }
                    else if (blocky.transform.position == tPosXMinus)
                    {
                        right = true;
                    }
                    else if (blocky.transform.position == tPosZMinus)
                    {
                        top = true;
                    }
                    else if (blocky.transform.position == tPosXAddExtra)
                    {
                        left2 = true;
                    }
                    else if (blocky.transform.position == tPosZAddExtra)
                    {
                        bottom2 = true;
                    }
                    else if (blocky.transform.position == tPosXMinusExtra)
                    {
                        right2 = true;
                    }
                    else if (blocky.transform.position == tPosZMinusExtra)
                    {
                        top2 = true;
                    }
                    else
                    {
                        //Nothing
                    }


                    if (ScatteredWalls)
                    {
                        if (left == true && right == true && top == true && bottom == true)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (left == true && right == true && top == true && bottom == true && left2 == true && right2 == true && top2 == true && bottom2 == true)
                        {
                            break;
                        }
                    }




                    /*if ((blocky.transform.position.x == tPosXAdd.x || blocky.transform.position.x == -tPosXAdd.x) && (blocky.transform.position.z == tPosXAdd.z || blocky.transform.position.z == -tPosXAdd.z))
                    {
                        left = true;
                    }
                    else if ((blocky.transform.position.x == tPosZAdd.x || blocky.transform.position.x == -tPosZAdd.x) && (blocky.transform.position.z == tPosZAdd.z || blocky.transform.position.z == -tPosZAdd.z))
                    {
                        bottom = true;
                    }
                    else if ((blocky.transform.position.x == tPosXMinus.x || blocky.transform.position.x == -tPosXMinus.x) && (blocky.transform.position.z == tPosXMinus.z || blocky.transform.position.z == -tPosXMinus.z))
                    {
                        right = true;
                    }
                    else if ((blocky.transform.position.x == tPosZMinus.x || blocky.transform.position.x == -tPosZMinus.x) && (blocky.transform.position.z == tPosZMinus.z || blocky.transform.position.z == -tPosZMinus.z))
                    {
                        top = true;
                    }
                    else
                    {
                        //Nothing
                    }*/

                    /* else if ((blocky.transform.position.x == tPosXAddExtra.x || blocky.transform.position.x == -tPosXAddExtra.x) && (blocky.transform.position.z == tPosXAddExtra.z || blocky.transform.position.z == -tPosXAddExtra.z) && blocky.name == "Entrance")
                     {
                         left2 = true;
                     }
                     else if ((blocky.transform.position.x == tPosZAddExtra.x || blocky.transform.position.x == -tPosZAddExtra.x) && (blocky.transform.position.z == tPosZAddExtra.z || blocky.transform.position.z == -tPosZAddExtra.z) && blocky.name == "Entrance")
                     {
                         bottom2 = true;
                     }
                     else if ((blocky.transform.position.x == tPosXMinusExtra.x || blocky.transform.position.x == -tPosXMinusExtra.x) && (blocky.transform.position.z == tPosXMinusExtra.z || blocky.transform.position.z == -tPosXMinusExtra.z) && blocky.name == "Entrance")
                     {
                         right2 = true;
                     }
                     else if ((blocky.transform.position.x == tPosZMinusExtra.x || blocky.transform.position.x == -tPosZMinusExtra.x) && (blocky.transform.position.z == tPosZMinusExtra.z || blocky.transform.position.z == -tPosZMinusExtra.z) && blocky.name == "Entrance")
                     {
                         top2 = true;
                     }
                     else
                     {
                         //Nothing
                     }*/


                }

                if (ScatteredWalls == true)
                {
                    //If there are objects all around us
                    if (left == true && right == true && top == true && bottom == true)
                    {
                        /*Debug.Log("We Have Somebody" + " " + b.name);*/

                        //If we are a wall
                        if (b.transform.childCount >= 1)
                        {
                            GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);

                            Vector3 newPos = b.transform.position;

                            newB.transform.position = newPos;

                            newB.name = "Floor";

                            newB.transform.parent = b.transform.parent;

                            AllBlocksInUse.Add(newB);

                            Deactivated.Add(b.gameObject);
                            b.gameObject.SetActive(false);
                            b.gameObject.name = "DELETE ME!";
                            /*AllBlocksInUse.Remove(b);*/
                        }
                        else
                        {
                            //Do nothing
                        }




                    }
                    else //If there are not objects all around us
                    {
                        //If we are a wall type
                        if (b.transform.childCount >= 1 && b.gameObject.name != "CorridorWall")
                        {
                            if (b.transform.GetChild(0).gameObject.activeSelf == false)
                            {
                                b.transform.GetChild(0).gameObject.SetActive(true);
                            }
                        }
                        //else if we are not surrounded, but aren't a wall, become one
                        else
                        {

                            GameObject newB = Instantiate(WallBlock, transform.position, transform.rotation);

                            Vector3 newPos = b.transform.position;

                            newB.transform.position = newPos;

                            newB.name = "Wall";

                            newB.transform.parent = b.transform.parent;

                            AllBlocksInUse.Add(newB);

                            Deactivated.Add(b.gameObject);
                            b.gameObject.SetActive(false);
                            b.gameObject.name = "DELETE ME!";
                            AllBlocksInUse.Remove(b);

                        }
                    }
                }
                else
                {

                    //If there are objects all around us
                    if (left == true && right == true && top == true && bottom == true && left2 == true && right2 == true && top2 == true && bottom2 == true)
                    {
                        //If we are a wall
                        if (b.transform.childCount >= 1)
                        {
                            GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);

                            Vector3 newPos = b.transform.position;

                            newB.transform.position = newPos;

                            newB.name = "Floor";

                            newB.transform.parent = b.transform.parent;

                            AllBlocksInUse.Add(newB);

                            Deactivated.Add(b.gameObject);
                            b.gameObject.SetActive(false);
                            b.gameObject.name = "DELETE ME!";
                            /*AllBlocksInUse.Remove(b);*/
                        }
                        else
                        {
                            //Do nothing
                        }

                    }
                    else if (left == true && right == true && top == true && bottom == true && b.name == "CorridorWall")
                    {
                        GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);

                        Vector3 newPos = b.transform.position;

                        newB.transform.position = newPos;

                        newB.name = "Floor";

                        newB.transform.parent = b.transform.parent;

                        AllBlocksInUse.Add(newB);

                        Deactivated.Add(b);
                        b.SetActive(false);
                        b.name = "DELETE ME!";
                        /*AllBlocksInUse.Remove(b);*/
                    }
                    else //If there are not objects all around us
                    {
                        if (NoCorridorWalls == false)
                        {
                            //If we are a wall type
                            if (b.transform.childCount >= 1 && b.gameObject.name != "CorridorWall" && b.gameObject.name != "Obstacle")
                            {
                                if (b.transform.GetChild(0).gameObject.activeSelf == false)
                                {
                                    b.transform.GetChild(0).gameObject.SetActive(true);
                                }
                            }
                            //else if we are not surrounded, but aren't a wall, become one
                            else
                            {

                                GameObject newB = Instantiate(WallBlock, transform.position, transform.rotation);

                                Vector3 newPos = b.transform.position;

                                newB.transform.position = newPos;

                                newB.name = "Wall";

                                newB.transform.parent = b.transform.parent;

                                AllBlocksInUse.Add(newB);

                                Deactivated.Add(b.gameObject);
                                b.gameObject.SetActive(false);
                                b.gameObject.name = "DELETE ME!";
                                AllBlocksInUse.Remove(b);

                            }
                        }

                    }
                }

            }
        }

        lastHolesAndExtraBlocksChecked = true;

    }
    public void CheckCorridorWalls()
    {

        for (int i = 0; i < AllBlocksInUse.Count; i++)
        {
            GameObject b = AllBlocksInUse[i].gameObject;
            if (b.gameObject.name == "CorridorWall" /*|| b.gameObject.name == "Def"*/)//Using a name to try and lessen the check load
            {



                //tempAllBlocks.Add(b.gameObject);
                Vector3 tPosXAdd = new Vector3(b.transform.position.x + blockSizeX, 0, b.transform.position.z);
                Vector3 tPosXAddExtra = new Vector3(b.transform.position.x + blockSizeX, 0, b.transform.position.z - blockSizeZ);
                Vector3 tPosZAdd = new Vector3(b.transform.position.x, 0, b.transform.position.z + blockSizeZ);
                Vector3 tPosZAddExtra = new Vector3(b.transform.position.x - blockSizeX, 0, b.transform.position.z + blockSizeZ);

                Vector3 tPosXMinus = new Vector3(b.transform.position.x - blockSizeX, 0, b.transform.position.z);
                Vector3 tPosXMinusExtra = new Vector3(b.transform.position.x - blockSizeX, 0, b.transform.position.z - blockSizeZ);
                Vector3 tPosZMinus = new Vector3(b.transform.position.x, 0, b.transform.position.z - blockSizeZ);
                Vector3 tPosZMinusExtra = new Vector3(b.transform.position.x + blockSizeX, 0, b.transform.position.z + blockSizeZ);



                bool top = false;
                bool left = false;
                bool bottom = false;
                bool right = false;

                bool top2 = false;
                bool left2 = false;
                bool bottom2 = false;
                bool right2 = false;

                //Check all the areas around our block
                for (int ii = 0; ii < AllBlocksInUse.Count; ii++)
                {
                    GameObject blocky = AllBlocksInUse[ii].gameObject;


                    if (blocky.transform.position == tPosXAdd)
                    {
                        left = true;
                    }
                    else if (blocky.transform.position == tPosZAdd)
                    {
                        bottom = true;
                    }
                    else if (blocky.transform.position == tPosXMinus)
                    {
                        right = true;
                    }
                    else if (blocky.transform.position == tPosZMinus)
                    {
                        top = true;
                    }
                    else if (blocky.transform.position == tPosXAddExtra)
                    {
                        left2 = true;
                    }
                    else if (blocky.transform.position == tPosZAddExtra)
                    {
                        bottom2 = true;
                    }
                    else if (blocky.transform.position == tPosXMinusExtra)
                    {
                        right2 = true;
                    }
                    else if (blocky.transform.position == tPosZMinusExtra)
                    {
                        top2 = true;
                    }
                    else
                    {
                        //Nothing
                    }


                    if (left == true && right == true && top == true && bottom == true && left2 == true && right2 == true && top2 == true && bottom2 == true)
                    {
                        break;
                    }

                }

                //If there are objects all around us
                if (left == true && right == true && top == true && bottom == true && left2 == true && right2 == true && top2 == true && bottom2 == true)
                {
                    //If we are a wall
                    if (b.transform.childCount >= 1)
                    {
                        GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);

                        Vector3 newPos = b.transform.position;

                        newB.transform.position = newPos;

                        newB.name = "Floor";

                        newB.transform.parent = b.transform.parent;

                        AllBlocksInUse.Add(newB);

                        Deactivated.Add(b.gameObject);
                        b.gameObject.SetActive(false);
                        b.gameObject.name = "DELETE ME!";
                        /*AllBlocksInUse.Remove(b);*/
                    }
                    else
                    {
                        //Do nothing
                    }

                }
                else if (left == true && right == true && top == true && bottom == true && b.name == "CorridorWall")
                {
                    GameObject newB = Instantiate(FloorBlock, transform.position, transform.rotation);

                    Vector3 newPos = b.transform.position;

                    newB.transform.position = newPos;

                    newB.name = "Floor";

                    newB.transform.parent = b.transform.parent;

                    AllBlocksInUse.Add(newB);

                    Deactivated.Add(b);
                    b.SetActive(false);
                    b.name = "DELETE ME!";
                    /*AllBlocksInUse.Remove(b);*/
                }
                else //If there are not objects all around us
                {
                    //Nothing
                }
            }


        }

        CorridorWallsChecked = true;

    }
    //Code that checks for overlaps and gives specific orders of which block to keep (E.g. Floor atop wall = keep floor, remove wall. OR. Wall atop wall = keep 1 wall, delete other wall).
    public void CheckOverlap(GameObject newB)
    {
        for (int i = 0; i < LevelContainer.transform.childCount; i++)
        {
            GameObject child = LevelContainer.transform.GetChild(i).gameObject;
            for (int ii = 0; ii < child.transform.childCount; ii++)
            {
                GameObject blocky = child.transform.GetChild(ii).gameObject;

                if (newB.transform.position == blocky.transform.position && newB != blocky)
                {
                    //If a wall lands on top of a wall
                    if ((newB.name == "T" || newB.name == "B" || newB.name == "R" || newB.name == "L" || newB.name == "Def" || newB.name == "Wall" || newB.name == "CorridorWall") && (blocky.gameObject.name == "T" || blocky.gameObject.name == "B" || blocky.gameObject.name == "R" || blocky.gameObject.name == "L" || blocky.gameObject.name == "Def" || blocky.gameObject.name == "Wall" || blocky.gameObject.name == "CorridorWall"))
                    {
                        if (newB.transform.childCount >= 1 && blocky.activeSelf == true)
                        {
                            blocky.gameObject.SetActive(false);
                            blocky.name = "DELETE ME!";
                            Deactivated.Add(blocky.gameObject);
                            newB.SetActive(true);
                            newB.transform.GetChild(0).gameObject.SetActive(true);
                            newB.name = "CorridorWall";
                            if (Deactivated.Contains(newB))
                            {
                                Deactivated.Remove(newB);
                            }
                        }
                        else
                        {
                            newB.gameObject.SetActive(false);
                            newB.name = "DELETE ME!";
                            Deactivated.Add(newB.gameObject);
                            blocky.gameObject.SetActive(true);
                        }

                    }
                    //If a wall lands on a corridor, used corridor, entrance or floor 2
                    else if ((newB.name == "T" || newB.name == "B" || newB.name == "R" || newB.name == "L" || newB.name == "Def" || newB.name == "Wall" || newB.name == "CorridorWall") && (blocky.gameObject.name == "Floor" || blocky.gameObject.name == "Used Corridor" || blocky.gameObject.name == "Entrance" || blocky.gameObject.name == "Corridor" || blocky.gameObject.name == "Obstacle"))
                    {

                        newB.gameObject.SetActive(false);
                        newB.name = "DELETE ME!";
                        Deactivated.Add(newB.gameObject);
                        blocky.gameObject.SetActive(true);

                    }
                    //If floor lands on walls
                    else if ((newB.name == "Floor" || newB.name == "Corridor" || newB.name == "Entrance" || newB.name == "Used Corridor" || newB.name == "Obstacle") && (blocky.gameObject.name == "T" || blocky.gameObject.name == "B" || blocky.gameObject.name == "R" || blocky.gameObject.name == "L" || blocky.gameObject.name == "Def" || blocky.gameObject.name == "Wall" || blocky.gameObject.name == "CorridorWall"))
                    {
                        blocky.gameObject.SetActive(false);
                        blocky.name = "DELETE ME!";
                        Deactivated.Add(blocky.gameObject);
                        newB.SetActive(true);
                    }
                    else if ((newB.name == "Floor" || newB.name == "Corridor" || newB.name == "Entrance" || newB.name == "Used Corridor" || newB.name == "Obstacle") && (blocky.gameObject.name == "Floor" || blocky.gameObject.name == "Used Corridor" || blocky.gameObject.name == "Entrance" || blocky.gameObject.name == "Corridor"))
                    {
                        newB.gameObject.SetActive(false);
                        newB.name = "DELETE ME!";
                        Deactivated.Add(newB.gameObject);
                        blocky.gameObject.SetActive(true);
                    }
                    else
                    {
                        /*newB.gameObject.SetActive(false);
                        newB.name = "DELETE ME!";
                        Deactivated.Add(newB.gameObject);
                        blocky.gameObject.SetActive(true);*/
                    }

                }





            }


        }
    }

    //Code that checks if we will overlap any other blocks when building corridors (with room sizes in mind)
    public bool CheckNoOverlapWillOccur(float corriWidth, float corriLength, Vector3 currentPos, string WhatWayOut)
    {
        float roomWidth = MaxWidthRoom * blockSizeX;
        float roomLength = MaxLengthRoom * blockSizeZ;
        float corridorWidth = corriWidth * blockSizeX;
        float corridorLength = corriLength * blockSizeZ;

        float bSizeX = blockSizeX * 10;
        float bSizeZ = blockSizeZ * 10;

        /*Debug.Log(corriWidth);
        Debug.Log(corriLength);
        Debug.Log(currentPos);
        Debug.Log(WhatWayOut);
*/

        for (int i = 0; i < LevelContainer.transform.childCount; i++)
        {
            GameObject child = LevelContainer.transform.GetChild(i).gameObject;
            for (int ii = 0; ii < child.transform.childCount; ii++)
            {
                Vector3 blokPos = child.transform.GetChild(ii).transform.position;

                //If we are opening an entrance on the +Z axis
                if (WhatWayOut == "Up")
                {
                    //Current Doorway position - calculating where the first entrance block will start
                    Vector3 doorPos = new Vector3(currentPos.x, 0, currentPos.z + blockSizeZ);
                    //Current Doorway position - calculating where the last entrance block will start
                    Vector3 roomPos = new Vector3(currentPos.x, 0, currentPos.z + corridorLength);

                    //If the corridor width is greater tham door position, but less than the door positions greatest length minus the block size
                    if (((blokPos.x >= doorPos.x && blokPos.x <= (doorPos.x + corridorWidth + bSizeX)) || (blokPos.x <= doorPos.x && blokPos.x >= (doorPos.x - (corridorWidth + bSizeX)))) && ((blokPos.z >= doorPos.z && blokPos.z <= (doorPos.z + corridorLength + bSizeZ))/* || (blokPos.z <= (doorPos.z - corridorLength) && blokPos.z >= (doorPos.z - (corridorLength - blockSizeZ)))*/))
                    {
                        return false;
                    }
                    //If the largest room width is greater tham door position, but less than the door positions greatest length minus the block size (and same with the width)
                    else if (((blokPos.x >= roomPos.x && blokPos.x <= (roomPos.x + roomWidth + bSizeX)) || (blokPos.x <= roomPos.x && blokPos.x >= (roomPos.x - (roomWidth + bSizeX)))) && ((blokPos.z >= roomPos.z && blokPos.z <= (roomPos.z + roomLength + bSizeZ))/* || (blokPos.z <= (roomPos.z - roomLength) && blokPos.z >= (roomPos.z - (roomLength - blockSizeZ)))*/))
                    {
                        return false;
                    }
                    else
                    {
                        //Nothing Found - Hurray!!!
                    }
                }
                //If we are opening an entrance on the -Z axis
                if (WhatWayOut == "Down")
                {
                    //Current Doorway position - calculating where the first entrance block will start
                    Vector3 doorPos = new Vector3(currentPos.x, 0, currentPos.z - blockSizeZ);
                    //Current Doorway position - calculating where the last entrance block will start
                    Vector3 roomPos = new Vector3(currentPos.x, 0, currentPos.z - corridorLength);

                    //If the corridor width is greater tham door position, but less than the door positions greatest length minus the block size
                    if (((blokPos.x >= doorPos.x && blokPos.x <= (doorPos.x + corridorWidth + bSizeX)) || (blokPos.x <= doorPos.x && blokPos.x >= (doorPos.x - (corridorWidth + bSizeX)))) && (/*(blokPos.z >= (doorPos.z + corridorLength) && blokPos.z <= (doorPos.z + corridorLength + blockSizeZ)) ||*/ (blokPos.z <= doorPos.z && blokPos.z >= (doorPos.z - (corridorLength + bSizeZ)))))
                    {
                        return false;
                    }
                    //If the largest room width is greater tham door position, but less than the door positions greatest length minus the block size (and same with the width)
                    else if (((blokPos.x >= roomPos.x && blokPos.x <= (roomPos.x + roomWidth + bSizeX)) || (blokPos.x <= roomPos.x && blokPos.x >= (roomPos.x - (roomWidth + bSizeX)))) && (/*(blokPos.z >= (roomPos.z + roomLength) && blokPos.z <= (roomPos.z + roomLength + blockSizeZ)) ||*/ (blokPos.z <= roomPos.z && blokPos.z >= (roomPos.z - (roomLength + bSizeZ)))))
                    {
                        return false;
                    }
                    else
                    {
                        //Nothing Found - Hurray!!!
                    }
                }
                //If we are opening an entrance on the -X axis
                if (WhatWayOut == "Left")
                {
                    //Current Doorway position - calculating where the first entrance block will start
                    Vector3 doorPos = new Vector3(currentPos.x - blockSizeX, 0, currentPos.z);
                    //Current Doorway position - calculating where the last entrance block will start
                    Vector3 roomPos = new Vector3(currentPos.x - corridorWidth, 0, currentPos.z);

                    //If the corridor width is greater tham door position, but less than the door positions greatest length minus the block size
                    if ((/*(blokPos.x >= (doorPos.x + corridorWidth) && blokPos.x <= (doorPos.x + corridorWidth + blockSizeX)) ||*/ (blokPos.x <= doorPos.x && blokPos.x >= (doorPos.x - (corridorLength + bSizeX)))) && ((blokPos.z >= doorPos.z && blokPos.z <= (doorPos.z + corridorLength + bSizeZ)) || (blokPos.z <= doorPos.z && blokPos.z >= (doorPos.z - (corridorLength + bSizeZ)))))
                    {
                        Debug.Log("Found 1");
                        return false;
                    }
                    //If the largest room width is greater tham door position, but less than the door positions greatest length minus the block size (and same with the width)
                    else if ((/*(blokPos.x >= (roomPos.x + roomWidth) && blokPos.x <= (roomPos.x + roomWidth + blockSizeX)) ||*/ (blokPos.x <= roomPos.x && blokPos.x >= (roomPos.x - (roomLength + bSizeX)))) && ((blokPos.z >= roomPos.z && blokPos.z <= (roomPos.z + (roomLength + bSizeZ))) || (blokPos.z <= roomPos.z && blokPos.z >= (roomPos.z - (roomLength + bSizeZ)))))
                    {
                        Debug.Log("Found 2");
                        return false;
                    }
                    else
                    {
                        //Nothing Found - Hurray!!!
                    }
                }
                //If we are opening an entrance on the +X axis
                if (WhatWayOut == "Right")
                {
                    //Current Doorway position - calculating where the first entrance block will start
                    Vector3 doorPos = new Vector3(currentPos.x + blockSizeX, 0, currentPos.z);
                    //Current Doorway position - calculating where the last entrance block will start
                    Vector3 roomPos = new Vector3(currentPos.x + corridorWidth, 0, currentPos.z);

                    //If the corridor width is greater tham door position, but less than the door positions greatest length minus the block size
                    if (((blokPos.x >= doorPos.x && blokPos.x <= (doorPos.x + corridorLength + bSizeX)) /*|| (blokPos.x <= (doorPos.x - corridorWidth) && blokPos.x >= (doorPos.x - (corridorWidth + blockSizeX)))*/) && ((blokPos.z >= doorPos.z && blokPos.z <= (doorPos.z + corridorLength + bSizeZ)) || (blokPos.z <= doorPos.z && blokPos.z >= (doorPos.z - (corridorLength + bSizeZ)))))
                    {
                        return false;
                    }
                    //If the largest room width is greater tham door position, but less than the door positions greatest length minus the block size (and same with the width)
                    else if (((blokPos.x >= roomPos.x && blokPos.x <= (roomPos.x + roomLength + bSizeX)) /*|| (blokPos.x <= (roomPos.x - roomWidth) && blokPos.x >= (roomPos.x - (roomWidth + blockSizeX)))*/) && ((blokPos.z >= roomPos.z && blokPos.z <= (roomPos.z + roomLength + bSizeZ)) || (blokPos.z <= roomPos.z && blokPos.z >= (roomPos.z - (roomLength + bSizeZ)))))
                    {
                        return false;
                    }
                    else
                    {
                        //Nothing Found - Hurray!!!
                    }
                }


            }
        }


        return true;
    }






    //

    //Functions for placeing items, objecst, filling corners, and enemies

    //Code for placing items in rooms
    public void FillCorners(List<GameObject> corners)
    {
        int FillCornersAmount = Random.Range(cornerFillAmountMin, cornerFillAmountMax);
        for (int i = 0; i < FillCornersAmount; i++)
        {
            int rando = Random.Range(0, CornerObjects.Count);
            GameObject cornerObj = CornerObjects[rando];


            float newPosY = 0;
            /*if (cornerObj.GetComponent<Renderer>() != null)
            {
                newPosY = (cornerObj.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
            }
            else
            {
                newPosY = blockSizeY;
            }*/
            if(basePivots)
            {
                newPosY = blockSizeY;
            }
            else
            {
                if (cornerObj.transform.GetComponent<Renderer>() != null)
                {
                    newPosY = (blockSizeY * 0.5f) + (cornerObj.transform.GetComponent<Renderer>().bounds.size.y * 0.5f);
                }
                else
                {
                    //Just run through all the child objects unitl we get a renderer
                    for (int it = 0; it < cornerObj.transform.childCount; it++)
                    {
                        if (cornerObj.transform.GetChild(it).GetComponent<Renderer>() != null)
                        {
                            newPosY = (cornerObj.transform.GetChild(it).GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                            break;
                        }
                    }

                    //Check to see if we haven't got a value
                    if (newPosY == 0)
                    {
                        newPosY = (cornerObj.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                    }
                }
            }
            

            int randomCorner = Random.Range(0, corners.Count);
            GameObject cornerBlock = corners[randomCorner].gameObject;

            if (cornerBlock.name == "CornerBlock")
            {
                cornerObj = Instantiate(cornerObj.gameObject, transform.position, transform.rotation);
                Vector3 newPos = new Vector3(cornerBlock.transform.position.x, newPosY, cornerBlock.transform.position.z);
                cornerObj.transform.position = newPos;
                cornerObj.name = "CornerObject";
                cornerObj.transform.parent = cornerBlock.transform.parent;

                //Random rotation
                float randomRotation = Random.Range(-45f, 45f);
                cornerObj.transform.eulerAngles = new Vector3(0, randomRotation, 0);

                //Change CornerBlock name to CornerBlockUsed (will change back during the room scripts population
                cornerBlock.name = "CornerBlockUsed";

            }
            else
            {
                //Do nothing

            }

        }
    }
    //Code for placing items in rooms
    public void PlaceItems(List<GameObject> blocks)
    {


        int itemTempCount = 0;

        int itemsToPlace = Random.Range(minItemPerRoom, maxItemPerRoom + 1);

        while (itemTempCount < itemsToPlace)
        {
            int i = Random.Range(0, blocks.Count - 1);
            int rareChanceRoll = Random.Range(1, 101);
            if (blocks[i].name != "Obstacle" && blocks[i].name != "T" && blocks[i].name != "B" && blocks[i].name != "R" && blocks[i].name != "L" && blocks[i].name != "Def" && blocks[i].name != "Wall" && blocks[i].name != "Entrance" && blocks[i].name != "Used Corridor" && blocks[i].name != "T" && blocks[i].name != "CornerBlock" && blocks[i].name != "CornerBlockUsed" && blocks[i].name != "CenterPiecesWhole")
            {
                if (Items.Count >= 1)
                {
                    //Get a rare or normal item
                    GameObject ItemToPlace;
                    if (RareItems.Count >= 1 && rareChanceRoll <= rareChance)
                    {
                        if (RareItems.Count > 1)
                        {
                            int randoItem = Random.Range(0, RareItems.Count);
                            ItemToPlace = RareItems[randoItem];
                        }
                        else
                        {
                            ItemToPlace = RareItems[0];

                        }

                    }
                    else
                    {
                        if (Items.Count > 1)
                        {
                            int randoItem = Random.Range(0, Items.Count);
                            ItemToPlace = Items[randoItem];
                        }
                        else
                        {
                            ItemToPlace = Items[0];
                        }
                    }

                    //Instantiate the new item
                    GameObject newItem = Instantiate(ItemToPlace, transform.position, transform.rotation);


                    float newPosY = 0;
                    
                    if (basePivots)
                    {
                        newPosY = blockSizeY;
                    }
                    else
                    {
                        if (newItem.transform.GetComponent<Renderer>() != null)
                        {
                            newPosY = (blockSizeY * 0.5f) + (newItem.transform.GetComponent<Renderer>().bounds.size.y * 0.5f);
                        }
                        else
                        {
                            //Just run through all the child objects unitl we get a renderer
                            for (int it = 0; it < newItem.transform.childCount; it++)
                            {
                                if (newItem.transform.GetChild(it).GetComponent<Renderer>() != null)
                                {
                                    newPosY = (newItem.transform.GetChild(it).GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                    break;
                                }
                            }

                            //Check to see if we haven't got a value
                            if (newPosY == 0)
                            {
                                newPosY = (newItem.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                            }
                        }
                    }
                    //Assign position
                    Vector3 newPos = new Vector3(blocks[i].transform.position.x, newPosY, blocks[i].transform.position.z);
                    newItem.transform.position = newPos;

                    //Add to list
                    ItemsAddedIn.Add(newItem);

                    //Increase count
                    itemTempCount += 1;


                }
            }
            else
            {
                itemTempCount += 1;
            }


        }
    }
    //Code for placing enemies
    public void PlaceEnemies(List<GameObject> blocks)
    {

        int enemiesTempCount = 0;

        int enemiesToPlace = Random.Range(minEnemyPerRoom, maxEnemyPerRoom + 1);

        while (enemiesTempCount < enemiesToPlace)
        {
            int i = Random.Range(0, blocks.Count - 1);

            if (blocks[i].name != "Obstacle" && blocks[i].name != "T" && blocks[i].name != "B" && blocks[i].name != "R" && blocks[i].name != "L" && blocks[i].name != "Def" && blocks[i].name != "Wall" && blocks[i].name != "Entrance" && blocks[i].name != "Used Corridor" && blocks[i].name != "T" && blocks[i].name != "CornerBlock" && blocks[i].name != "CornerBlockUsed" && blocks[i].name != "CenterPiecesWhole")
            {
                if (Enemies.Count >= 1)
                {

                    int whichEnemy = Random.Range(0, Enemies.Count);
                    GameObject enem = Instantiate(Enemies[whichEnemy], transform.position, transform.rotation);

                    //Get a Y position
                    float newPosY = 0;
                    /*if (enem.GetComponent<Renderer>() != null)
                    {
                        newPosY = (enem.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                    }
                    else if (enem.transform.GetChild(0) != null && enem.transform.GetChild(0).gameObject.GetComponent<Renderer>() != null)
                    {
                        newPosY = (enem.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                    }
                    else
                    {
                        newPosY = blockSizeY;
                    }*/
                    if (basePivots)
                    {
                        newPosY = blockSizeY;
                    }
                    else
                    {
                        if(enem.transform.GetComponent<Renderer>() != null)
                        {
                            newPosY = (blockSizeY * 0.5f) + (enem.transform.GetComponent<Renderer>().bounds.size.y * 0.5f);
                        }
                        else
                        {
                            //Just run through all the child objects unitl we get a renderer
                            for(int it = 0; it < enem.transform.childCount; it++)
                            {
                                if(enem.transform.GetChild(it).GetComponent<Renderer>() != null)
                                {
                                    newPosY = (enem.transform.GetChild(it).GetComponent<Renderer>().bounds.size.y * 0.5f) + (blockSizeY * 0.5f);
                                    break;
                                }
                            }

                            //Check to see if we haven't got a value
                            if(newPosY == 0)
                            {
                                newPosY = (enem.transform.lossyScale.y * 0.5f) + (blockSizeY * 0.5f);
                            }
                        }
                    }

                    Vector3 newPos = new Vector3(blocks[i].transform.position.x, newPosY, blocks[i].transform.position.z);
                    enem.transform.position = newPos;

                    enem.transform.parent = null;

                    EnemiesAddedIn.Add(enem);

                    enemiesTempCount += 1;
                }
            }
            else
            {
                enemiesTempCount += 1;
            }
        }
    }
    //Code that ensure last object doesnt get placed inside a wall or obstacle.
    public void PlaceEndObject(GameObject lastR)
    {
        int spot = Random.Range(0, lastR.transform.childCount);
        GameObject theSpot = lastR.transform.GetChild(spot).gameObject;

        if (theSpot.name == "Floor")
        {
            Vector3 objPos = new Vector3(theSpot.transform.position.x, blockSizeY, theSpot.transform.position.z);
            GameObject lastOb = Instantiate(lastObject, objPos, theSpot.transform.rotation);
            lastOb.transform.parent = null;
            ItemsAddedIn.Add(lastOb);
        }
        else
        {
            PlaceEndObject(lastR);
        }
    }

    //

    //Function for adding room controls to our rooms and populating there lists with the correct current blocks
    public void AddRoomControlScriptAndPopulate()
    {
        foreach (Transform room in LevelContainer.transform)
        {

            room.gameObject.AddComponent<BlockyGridRoomControls>();
            BlockyGridRoomControls BGRC = room.gameObject.GetComponent<BlockyGridRoomControls>();

            BGRC.BlockyGrid = this;

            foreach (Transform blocky in room.transform)
            {
                string blockyname = blocky.gameObject.name;

                if (blockyname == "Floor" || blockyname == "Corridor" || blockyname == "Entrance" || blockyname == "CenterBlock")
                {
                    BGRC.FloorBlocks.Add(blocky.gameObject);
                }
                else if (blockyname == "Wall" || blockyname == "Def" || blockyname == "L" || blockyname == "R" || blockyname == "T" || blockyname == "B" || blockyname == "CorridorWall")
                {
                    if (blocky.GetChild(0).gameObject.activeSelf == false)
                    {
                        blocky.gameObject.name = "Floor";
                        BGRC.FloorBlocks.Add(blocky.gameObject);
                    }
                    else
                    {
                        BGRC.WallBlocks.Add(blocky.gameObject);
                        blocky.gameObject.name = "Wall";
                    }
                }
                else if (blockyname == "Doorway")
                {
                    BGRC.Doorways.Add(blocky.gameObject);
                }
                else if (blockyname == "Obstacle")
                {
                    BGRC.ObstacleBlocks.Add(blocky.gameObject);
                }
                else if (blockyname == "CornerBlock")
                {
                    BGRC.CornerSpots.Add(blocky.gameObject);
                }
                else if (blockyname == "CornerBlockUsed")
                {
                    blockyname = "CornerBlock";
                    BGRC.CornerSpotsUsed.Add(blocky.gameObject);
                }
                else if (blockyname == "CenterBlock")
                {
                    BGRC.CenterBlock = blocky.gameObject;
                }
                else if (blockyname == "CornerObject")
                {
                    BGRC.cornerObjects.Add(blocky.gameObject);
                }
                else if (blockyname == "CenterPiecesWhole")
                {

                    BGRC.CenterAreaBlocks.Add(blocky.gameObject);

                }
                else
                {
                    //Nothing
                }

            }



            //OnlyRooms Controlled
            /*if (room.gameObject.name.Contains("Room "))
            {
                if(room.GetComponent<BlockyGridRoomControls>() == false)
                {
                    room.gameObject.AddComponent<BlockyGridRoomControls>();
                    BlockyGridRoomControls BGRC = room.gameObject.GetComponent<BlockyGridRoomControls>();

                    foreach(Transform blocky in room.transform)
                    {
                        string blockyname = blocky.gameObject.name;

                        if(blockyname == "Floor")
                        {
                            BGRC.FloorBlocks.Add(blocky.gameObject);
                        }
                        else if(blockyname == "Wall" || blockyname == "Def" || blockyname == "L" || blockyname == "R" || blockyname == "T" || blockyname == "B")
                        {
                            if (blocky.GetChild(0).gameObject.activeSelf == false)
                            {
                                blocky.name = "Floor";
                                BGRC.FloorBlocks.Add(blocky.gameObject);
                            }
                            else
                            {
                                BGRC.WallBlocks.Add(blocky.gameObject);
                            }
                        }
                        else if(blockyname == "Doorway")
                        {
                            BGRC.Doorways.Add(blocky.gameObject);
                        }
                        else if(blockyname == "Obstacle")
                        {

                        }
                        else
                        {
                            //Nothing
                        }

                    }
                }



            }*/

        }
    }
    //Last Checks of the builder
    public void LastChecks()
    {
        GameObject tempObj = Instantiate(ParentObj, transform.position, transform.rotation);
        GameObject DestroyParent = Instantiate(ParentObj, transform.position, transform.rotation);


        /*foreach (Transform child in LevelContainer.transform)
        {
            foreach (Transform blocky in child.transform)
            {
                if (blocky.gameObject.activeSelf)
                {
                    AllBlocksInUse.Add(blocky.gameObject);
                }
            }
        }*/

        /*Debug.Log(AllBlocksInUse.Count);

        ClearEntrancesIfObstructed();

        Debug.Log("Stage 2 Completed");


        CheckNoHolesInWallsAndNoExtraWalls();

        Debug.Log("Stage 3 Completed");*/



        if (bigBossRound == true && BigBossEnemy != null)
        {
            int spot = Mathf.RoundToInt((lastRoom.transform.childCount / 2) / 2);
            GameObject theSpot = lastRoom.transform.GetChild(spot).gameObject;

            GameObject BigBoss = Instantiate(BigBossEnemy, theSpot.transform.position, theSpot.transform.rotation);

            BigBoss.transform.parent = null;
        }



        if (lastObject != null)
        {
            PlaceEndObject(lastRoom);


        }


        //Get rid of Deactivated blocks
        foreach (GameObject deactivated in Deactivated)
        {
            deactivated.transform.parent = DestroyParent.transform;
        }
        Deactivated.Clear();
        DestroyImmediate(DestroyParent, true);
        DestroyImmediate(tempObj, true);

        foreach (GameObject it in ItemsAddedIn)
        {
            it.transform.parent = LevelContainer.transform;
        }

        foreach (GameObject en in EnemiesAddedIn)
        {
            en.transform.parent = LevelContainer.transform;
        }


        lastCheckChecked = true;
    }
    
    //

    //Clear Functions

    //Code to clear all variables and delete everything (like a reset button).
    public void ClearAll()
    {
        //Clear all our counters
        tempCountX = 0;
        tempCountZ = 0;
        placedRooms = 0;
        spinRoom = 0;

        //Reset all our bools
        lastBlock = null;
        mUp = false;
        mDown = false;
        mLeft = false;
        mRight = false;
        alreadyBuiltOne = false;
        playerIn = false;

        if(LevelContainer != null)
        {
            //Destroy all objects
            while (LevelContainer.transform.childCount >= 1)
            {
                foreach (Transform child in LevelContainer.transform)
                {
                    DestroyImmediate(child.gameObject, true);
                }
            }
        }
        

        //Clear all Lists
        Rooms.Clear();
        Doorways.Clear();
        DoorwaysT.Clear();
        DoorwaysB.Clear();
        DoorwaysL.Clear();
        DoorwaysR.Clear();
        TempDoorways.Clear();
        TempDoorwaySpins.Clear();
        AllBlocks.Clear();
        Deactivated.Clear();
        ItemsAddedIn.Clear();
        EnemiesAddedIn.Clear();
        checkEntrances.Clear();



        //Clear the log
        ClearLog();


        //Stop Coroutine
        StopCoroutine(LevelBuilder());

    }
    //Code to clear the console log (get activated when ClearAll function is called.
    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    //

    //Functions For Already Built Rooms

    public void ChangeAllFloors()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                if (newFloorBlock != null)
                {
                    brc.NewFloorBlock = newFloorBlock;
                    brc.NewWallBlock = newWallBlock;
                    brc.ChangeFloors_All();
                }
                else
                {
                    Debug.LogError("No New Floor Assigned in Inspector.");
                    return;
                }

            }
        }
    }

    public void ChangeAllWalls()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                if (newWallBlock != null)
                {
                    brc.NewWallBlock = newWallBlock;
                    brc.ChangeWalls_All();
                }
                else
                {
                    Debug.LogError("No New Wall Assigned in Inspector.");
                    return;
                }

            }
        }
    }

    public void AddObstaclesToAll()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null && t.gameObject.name.Contains("Room"))
            {
                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                if (brc.NewObstacles.Count >= 1)
                {
                    brc.NewObstacles.Clear();

                    foreach (GameObject nb in NewObstacles)
                    {
                        brc.NewObstacles.Add(nb);
                    }
                }

                brc.ObstacleAmount = newObstaclesAmount;

                brc.AddObstacles();

            }
        }
    }

    public void RemoveObstaclesFromAll()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                if (brc.NewObstacles.Count >= 1)
                {
                    brc.NewObstacles.Clear();

                    foreach (GameObject nb in NewObstacles)
                    {
                        brc.NewObstacles.Add(nb);
                    }
                }

                brc.ObstacleAmount = newObstaclesAmount;

                brc.RemoveObstacles();

            }
        }
    }
    public void ChangeAllObstacles()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                if (brc.NewObstacles.Count >= 1)
                {
                    brc.NewObstacles.Clear();

                    foreach (GameObject nb in NewObstacles)
                    {
                        brc.NewObstacles.Add(nb);
                    }
                }
                brc.ChangeObstacles_NewObs();

            }
        }
    }

    public void RearrangeAllObstacles()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                brc.ReorderObstacles();

            }
        }
    }

    public void ChangeAllCornerObjects()
    {
        foreach (Transform r in LevelContainer.transform)
        {
            if (r.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = r.GetComponent<BlockyGridRoomControls>();

                if (brc.cornerObjects.Count >= 1)
                {
                    brc.cornerObjects.Clear();

                    foreach (GameObject nC in NewCornerObjects)
                    {
                        brc.cornerObjects.Add(nC);
                    }
                }

                int roomCornerObjectsCurrent = brc.cornerObjects.Count;
                brc.CornerObjectAmount = roomCornerObjectsCurrent;

                brc.RemoveCornerObejcts();

                brc.CornerSpotAdd();


            }
        }
    }
    public void AddCornerObjects()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null)
            {

                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                if(brc.CornerSpots.Count >= 1)
                {
                    if (brc.NewObstacles.Count >= 1)
                    {
                        brc.NewObstacles.Clear();

                        foreach (GameObject nb in NewObstacles)
                        {
                            brc.NewObstacles.Add(nb);
                        }
                    }
                    brc.CornerObjectAmount = CornerObjectChangeAmount;
                    brc.CornerSpotAdd();
                }

                

            }
        }
    }

    public void RemoveAllCornerObjects()
    {
        foreach (Transform r in LevelContainer.transform)
        {
            if (r.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = r.GetComponent<BlockyGridRoomControls>();

                if (brc.CornerSpots.Count >= 1)
                {
                    if (brc.NewObstacles.Count >= 1)
                    {
                        brc.NewObstacles.Clear();

                        foreach (GameObject nb in NewObstacles)
                        {
                            brc.NewObstacles.Add(nb);
                        }
                    }

                    int roomCornerObjectsCurrent = brc.cornerObjects.Count;
                    brc.CornerObjectAmount = roomCornerObjectsCurrent;

                    brc.RemoveCornerObejcts();
                }



            }
        }
    }

    public void ChangeAllDoorWays()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                if (newDoorObject != null)
                {
                    brc.NewDoorway = newDoorObject;
                    brc.ChangeDoorways_All();
                }
                else if (newWallBlock != null)
                {
                    brc.NewDoorway = newWallBlock;
                    brc.ChangeDoorways_All();
                }
                else
                {
                    Debug.LogError("No New Wall Assigned in Inspector.");
                    return;
                }
            }
        }
    }

    public void OpenOrCloseAllDoors()
    {
        foreach (Transform t in LevelContainer.transform)
        {
            if (t.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = t.GetComponent<BlockyGridRoomControls>();

                brc.CloseOrOpenDoorways();
            }
        }
    }

    public void ChangeAllFloorMaterials()
    {
        foreach (Transform r in LevelContainer.transform)
        {
            if (r.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = r.GetComponent<BlockyGridRoomControls>();

                brc.NewMateraialForThisRoom = NewMaterial;


                brc.ChangeAllFloorBlockMaterial();
            }
        }
    }

    public void ChangeAllWallMaterials()
    {
        foreach (Transform r in LevelContainer.transform)
        {
            if (r.GetComponent<BlockyGridRoomControls>() != null)
            {
                BlockyGridRoomControls brc = r.GetComponent<BlockyGridRoomControls>();

                brc.NewMateraialForThisRoom = NewMaterial;


                brc.ChangeAllWallMaterials();
            }
        }
    }


    

    /*    [System.Serializable]
        public class PresetVariables
        {
            public string roomType;

            public int RoomCount;

            public int CorridorWidthMin;
            public int CorridorWidthMax;
            public int CorridorLengthMin;
            public int CorridorLengthMax;

            public int RoomWidthMin;
            public int RoomWidthMax;
            public int RoomLengthMin;
            public int RoomLengthMax;

            public bool EditMode;

            public bool AddRControls;
            public bool Overlapping;
            public bool ScatteredWal;
            public bool seperateCorridors;


        }

        // 0 = Small Rooms
        // 1 = Medium Rooms
        // 2 = Large Rooms
        // 3 = A Lot of Rooms

        public Dictionary<int, PresetVariables> RoomPresets = new Dictionary<int, PresetVariables>()
        {
            { 0, new PresetVariables{ roomType = "Small Rooms", RoomCount = 10, CorridorWidthMin = 2, CorridorWidthMax = 4, CorridorLengthMin = 5, CorridorLengthMax = 9, RoomWidthMin = 7, RoomWidthMax = 10, RoomLengthMin = 7, RoomLengthMax = 10, AddRControls = true, Overlapping = false, ScatteredWal = false, seperateCorridors = true}},
            { 1, new PresetVariables{ roomType = "Medium Rooms", RoomCount = 20, CorridorWidthMin = 2, CorridorWidthMax = 5, CorridorLengthMin = 12, CorridorLengthMax = 15, RoomWidthMin = 9, RoomWidthMax = 15, RoomLengthMin = 9, RoomLengthMax = 14, AddRControls = true, Overlapping = false, ScatteredWal = false, seperateCorridors = true}},
            { 2, new PresetVariables{ roomType = "Large Rooms", RoomCount = 35, CorridorWidthMin = 2, CorridorWidthMax = 5, CorridorLengthMin = 12, CorridorLengthMax = 15, RoomWidthMin = 12, RoomWidthMax = 15, RoomLengthMin = 12, RoomLengthMax = 15, AddRControls = true, Overlapping = false, ScatteredWal = false, seperateCorridors = true}}

        };


        [Header("Preset Variables")]
        [HideInInspector]
        public List<PresetVariables> RoomBasicPresetVariables = new List<PresetVariables>()
        {
            { new PresetVariables{ roomType = "Small Rooms (Standard)", RoomCount = 10, CorridorWidthMin = 2, CorridorWidthMax = 4, CorridorLengthMin = 5, CorridorLengthMax = 9, RoomWidthMin = 7, RoomWidthMax = 10, RoomLengthMin = 7, RoomLengthMax = 10, AddRControls = true, Overlapping = false, ScatteredWal = false, seperateCorridors = true}},
            { new PresetVariables{ roomType = "Medium Rooms (Standard)", RoomCount = 20, CorridorWidthMin = 2, CorridorWidthMax = 5, CorridorLengthMin = 12, CorridorLengthMax = 15, RoomWidthMin = 9, RoomWidthMax = 15, RoomLengthMin = 9, RoomLengthMax = 14, AddRControls = true, Overlapping = false, ScatteredWal = false, seperateCorridors = true}},
            { new PresetVariables{ roomType = "Large Rooms (Standard)", RoomCount = 35, CorridorWidthMin = 2, CorridorWidthMax = 5, CorridorLengthMin = 12, CorridorLengthMax = 15, RoomWidthMin = 12, RoomWidthMax = 15, RoomLengthMin = 12, RoomLengthMax = 15, AddRControls = true, Overlapping = false, ScatteredWal = false, seperateCorridors = true}}
        };

        public void AddCurrentAsPreset()
        {
            RoomBasicPresetVariables.Add(new PresetVariables { roomType = PresetName, RoomCount = HowManyRooms, CorridorWidthMin = MinWidthCorridor, CorridorWidthMax = MaxWidthCorridor, 
                CorridorLengthMin = MinLengthCorridor, CorridorLengthMax = MaxLengthCorridor, RoomWidthMin = MinWidthRoom, RoomWidthMax = MaxWidthRoom, RoomLengthMin = MinLenghtRoom, 
                RoomLengthMax = MaxLengthRoom, AddRControls = AddRoomControls, Overlapping = OverlappingBlocks, ScatteredWal = ScatteredWalls, seperateCorridors = SeperateCorridorParents });
        }

        public void ChangePresets()
        {

        }
    */

    /*[Header("")]
    [Header("Doorway Testing - For Testing Purposes")]

    //ADDITIONAL ROOM CONTROLS

    public GameObject roomChoice;

    [HideInInspector]public List<GameObject> roomChoiceEntrances = new List<GameObject>();

    public void CloseDoorWays()
    {

        GameObject roomToClose = null;

        //Assign the room to the GamObject above
        if(roomChoice != null)
        {
            roomToClose = roomChoice;

            //Find the entrances and block them
            foreach (Transform child in roomToClose.transform)
            {
                if (child.gameObject.transform.name == "Doorway")
                {
                    GameObject doorBlock = child.transform.gameObject;

                    if(doorBlock.transform.GetChild(0).gameObject.activeSelf == false)
                    {
                        doorBlock.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        doorBlock.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    
                }
            }

        }
        else
        {
            Debug.Log("No Room Selected");
        }

    }
*/
}
