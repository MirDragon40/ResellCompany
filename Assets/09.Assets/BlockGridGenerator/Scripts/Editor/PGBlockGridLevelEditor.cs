using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PGBlockGridLevel))]
public class PGBlockGridLevelEditor : Editor
{
    //protected static bool ShowOffsetSettings = true; 
/*
    SerializedProperty RoomPresetsVariables;*/

    //Room Size Controls
    #region
    
    SerializedProperty HowManyRooms;
    SerializedProperty MinWidthCorridor;
    SerializedProperty MaxWidthCorridor;
    SerializedProperty MinLengthCorridor;
    SerializedProperty MaxLengthCorridor;
    SerializedProperty MinWidthRoom;
    SerializedProperty MaxWidthRoom;
    SerializedProperty MinLenghtRoom;
    SerializedProperty MaxLengthRoom;
    SerializedProperty spacing;

    #endregion

    //Blocks
    #region

    SerializedProperty LevelContainer;
    SerializedProperty FloorBlock;
    SerializedProperty WallBlock;
    SerializedProperty DoorwayBlock;
    

    #endregion

    //Additional Objects
    #region

    SerializedProperty Player;
    SerializedProperty Enemies;
    SerializedProperty minEnemyPerRoom;
    SerializedProperty maxEnemyPerRoom;
    SerializedProperty BigBossEnemy;
    SerializedProperty bigBossRound;
    SerializedProperty Items;
    SerializedProperty minItemPerRoom;
    SerializedProperty maxItemPerRoom;
    SerializedProperty RareItems;
    SerializedProperty rareChance;
    SerializedProperty obstacles;
    SerializedProperty obstaclePerRoom;
    SerializedProperty cornerFillAmountMin;
    SerializedProperty cornerFillAmountMax;
    SerializedProperty CornerObjects;
    SerializedProperty CenterPieces;
    SerializedProperty ChanceOfPlacingCenterPiece;
    SerializedProperty CenterPieceObjects;
    SerializedProperty lastObject;
    SerializedProperty loadingScreen;


    #endregion

    //Additional Settings
    #region

    SerializedProperty AddRoomControls;
    SerializedProperty StartRoomEmpty;
    SerializedProperty NoCorridorWalls;
    SerializedProperty OverlappingBlocks;
    SerializedProperty ScatteredWalls;
    SerializedProperty SeperateCorridorParents;
    SerializedProperty basePivots;
    #endregion

    //Variables for already built rooms
    #region
    SerializedProperty newFloorBlock;
    SerializedProperty newWallBlock;
    SerializedProperty newObstaclesAmount;
    SerializedProperty newDoorObject;

    SerializedProperty NewObstacles;

    SerializedProperty NewMaterial;

    #endregion


    bool RoomSizeControls = true;
    bool Blocks = false;
    bool AdditionalObjects = false;
    bool AdditionalSettings = false;
    bool BuiltRoomsChanges = false;
    //bool obstacleList = false;
    //bool PresetVariables = false;

    void OnEnable()
    {
        PGBlockGridLevel blockScript = (PGBlockGridLevel)target;
        // Fetch the objects from the GameObject script to display in the inspector
        //Room Size Controls

        //RoomPresetsVariables = serializedObject.FindProperty("RoomBasicPresetVariables");

        HowManyRooms = serializedObject.FindProperty("HowManyRooms");
        MinWidthCorridor = serializedObject.FindProperty("MinWidthCorridor");
        MaxWidthCorridor = serializedObject.FindProperty("MaxWidthCorridor");
        MinLengthCorridor = serializedObject.FindProperty("MinLengthCorridor");
        MaxLengthCorridor = serializedObject.FindProperty("MaxLengthCorridor");
        MinWidthRoom = serializedObject.FindProperty("MinWidthRoom");
        MaxWidthRoom = serializedObject.FindProperty("MaxWidthRoom");
        MinLenghtRoom = serializedObject.FindProperty("MinLenghtRoom");
        MaxLengthRoom = serializedObject.FindProperty("MaxLengthRoom");
        spacing = serializedObject.FindProperty("spacing");

        //Blocks
        LevelContainer = serializedObject.FindProperty("LevelContainer");
        FloorBlock = serializedObject.FindProperty("FloorBlock");
        WallBlock = serializedObject.FindProperty("WallBlock");
        DoorwayBlock = serializedObject.FindProperty("DoorwayBlock");



        //ADDITIONAL OBJECTS
        Player = serializedObject.FindProperty("Player");
        Enemies = serializedObject.FindProperty("Enemies");
        minEnemyPerRoom = serializedObject.FindProperty("minEnemyPerRoom");
        maxEnemyPerRoom = serializedObject.FindProperty("maxEnemyPerRoom");
        BigBossEnemy = serializedObject.FindProperty("BigBossEnemy");
        bigBossRound = serializedObject.FindProperty("bigBossRound");
        Items = serializedObject.FindProperty("Items");
        minItemPerRoom = serializedObject.FindProperty("minItemPerRoom");
        maxItemPerRoom = serializedObject.FindProperty("maxItemPerRoom");
        RareItems = serializedObject.FindProperty("RareItems");
        rareChance = serializedObject.FindProperty("rareChance");
        obstacles = serializedObject.FindProperty("Obstacles");
        obstaclePerRoom = serializedObject.FindProperty("obstaclePerRoom");
        cornerFillAmountMin = serializedObject.FindProperty("cornerFillAmountMin");
        cornerFillAmountMax = serializedObject.FindProperty("cornerFillAmountMax");
        CornerObjects = serializedObject.FindProperty("CornerObjects");
        CenterPieces = serializedObject.FindProperty("CenterPieces");
        ChanceOfPlacingCenterPiece = serializedObject.FindProperty("ChanceOfPlacingCenterPiece");
        CenterPieceObjects = serializedObject.FindProperty("CenterPieceObjects");
        lastObject = serializedObject.FindProperty("lastObject");
        loadingScreen = serializedObject.FindProperty("loadingScreen");

        //Additional Settings
        AddRoomControls = serializedObject.FindProperty("AddRoomControls");
        StartRoomEmpty = serializedObject.FindProperty("StartRoomEmpty");
        NoCorridorWalls = serializedObject.FindProperty("NoCorridorWalls");
        OverlappingBlocks = serializedObject.FindProperty("OverlappingBlocks");
        ScatteredWalls = serializedObject.FindProperty("ScatteredWalls");
        SeperateCorridorParents = serializedObject.FindProperty("SeperateCorridorParents");
        basePivots = serializedObject.FindProperty("basePivots");

        //Variables for Already Built Rooms 
        newFloorBlock = serializedObject.FindProperty("newFloorBlock");
        newWallBlock = serializedObject.FindProperty("newWallBlock");
        newObstaclesAmount = serializedObject.FindProperty("newObstaclesAmount");
        newDoorObject = serializedObject.FindProperty("newDoorObject");
        NewObstacles = serializedObject.FindProperty("NewObstacles");
        NewMaterial = serializedObject.FindProperty("NewMaterial");



    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PGBlockGridLevel blockScript = (PGBlockGridLevel)target;

        serializedObject.Update();
/*
        PresetVariables = EditorGUILayout.Foldout(PresetVariables, "Preset Variables", true);
        if(PresetVariables)
        {
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(RoomPresetsVariables);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }*/

        RoomSizeControls = EditorGUILayout.Foldout(RoomSizeControls, "Room Size Values", true);
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        if (RoomSizeControls)
        {
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.PropertyField(HowManyRooms);
            EditorGUILayout.PropertyField(MinWidthCorridor);
            EditorGUILayout.PropertyField(MaxWidthCorridor);
            EditorGUILayout.PropertyField(MinLengthCorridor);
            EditorGUILayout.PropertyField(MaxLengthCorridor);
            EditorGUILayout.PropertyField(MinWidthRoom);
            EditorGUILayout.PropertyField(MaxWidthRoom);
            EditorGUILayout.PropertyField(MinLenghtRoom);
            EditorGUILayout.PropertyField(MaxLengthRoom);
            EditorGUILayout.PropertyField(spacing);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }
        //EditorGUILayout.EndFoldoutHeaderGroup();


        Blocks = EditorGUILayout.Foldout(Blocks, "Blocks", true);
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        if (Blocks)
        {
            //SerializedProperty obt = serializedObject.FindProperty(obstacles[i]);
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.PropertyField(LevelContainer);
            EditorGUILayout.PropertyField(FloorBlock);
            EditorGUILayout.PropertyField(WallBlock);
            EditorGUILayout.PropertyField(DoorwayBlock);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

        }
        

        AdditionalObjects = EditorGUILayout.Foldout(AdditionalObjects, "Additional Objects", true);
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        if (AdditionalObjects)
        {
            EditorGUILayout.BeginVertical("GroupBox");

            EditorGUILayout.PropertyField(Player);

            EditorGUILayout.Space(10f);

            

            
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("Enemies:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(minEnemyPerRoom);
            EditorGUILayout.PropertyField(maxEnemyPerRoom);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(Enemies);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();


            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("Big Boss Enemy:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(bigBossRound);
            EditorGUILayout.PropertyField(BigBossEnemy);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("Items:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(minItemPerRoom);
            EditorGUILayout.PropertyField(maxItemPerRoom);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(Items);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("Rare Items:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(rareChance);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(RareItems);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("Obstacles:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(obstaclePerRoom);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(obstacles);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("Corner Objects:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(cornerFillAmountMin);
            EditorGUILayout.PropertyField(cornerFillAmountMax);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(CornerObjects);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField("Center Pieces:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(CenterPieces);
            EditorGUILayout.PropertyField(ChanceOfPlacingCenterPiece);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(CenterPieceObjects);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.PropertyField(lastObject);
            EditorGUILayout.Space(10f);
            EditorGUILayout.PropertyField(loadingScreen);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

        }
        //EditorGUILayout.EndFoldoutHeaderGroup();

        AdditionalSettings = EditorGUILayout.Foldout(AdditionalSettings, "Additional Settings", true);
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        if (AdditionalSettings)
        {
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.PropertyField(AddRoomControls);
            EditorGUILayout.PropertyField(StartRoomEmpty);
            EditorGUILayout.PropertyField(NoCorridorWalls);
            EditorGUILayout.PropertyField(OverlappingBlocks);
            EditorGUILayout.PropertyField(ScatteredWalls);
            EditorGUILayout.PropertyField(SeperateCorridorParents);
            EditorGUILayout.PropertyField(basePivots);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space(10f);

        EditorGUILayout.LabelField("Build Controls:", EditorStyles.boldLabel);
        
        //Button in Inpector to build Level
        if (GUILayout.Button("Build Level"))
        {

            blockScript.StartBullding();

        }
        //Button in Inpector for removing all physical boxes
        if (GUILayout.Button("Clear All"))
        {
            blockScript.ClearAll();
        }

        EditorGUILayout.Space(10f);

        BuiltRoomsChanges = EditorGUILayout.Foldout(BuiltRoomsChanges, "Built Room Changes", true);
        //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
        if (BuiltRoomsChanges)
        {
            EditorGUILayout.BeginVertical("GroupBox");

            //Variables And Controls For Already Built Rooms

            EditorGUILayout.LabelField("Floor Controls:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(newFloorBlock);
            EditorGUILayout.Space();
            //Button in Inpector to build Level
            if (GUILayout.Button("Change: Floors"))
            {

                blockScript.ChangeAllFloors();

            }
            EditorGUILayout.Space(5f);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.LabelField("Wall Controls:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(newWallBlock);
            EditorGUILayout.Space();
            //Button in Inpector for Changing Walls
            if (GUILayout.Button("Change: Walls"))
            {
                blockScript.ChangeAllWalls();


            }
            EditorGUILayout.Space(5f);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.LabelField("Obstacle Controls:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(newObstaclesAmount);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("New Obstacles:", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(NewObstacles);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            if (GUILayout.Button("Change: Obstacles"))
            {
                blockScript.ChangeAllObstacles();


            }
            if (GUILayout.Button("Add Obstacles"))
            {
                blockScript.AddObstaclesToAll();


            }
            if (GUILayout.Button("Remove Obstacles"))
            {
                blockScript.RemoveObstaclesFromAll();


            }
            EditorGUILayout.Space(5f);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10f);

            EditorGUILayout.LabelField("Doorway Controls:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(newDoorObject);
            EditorGUILayout.Space();
            if (GUILayout.Button("Change: Doorways"))
            {
                blockScript.ChangeAllDoorWays();


            }
            if (GUILayout.Button("Open/Close All Doors"))
            {

                blockScript.OpenOrCloseAllDoors();

            }
            EditorGUILayout.Space(5f);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10f);

            EditorGUILayout.LabelField("Material Controls:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.Space(5f);
            EditorGUILayout.PropertyField(NewMaterial);
            EditorGUILayout.Space();
            if (GUILayout.Button("Change All Floor Materials"))
            {

                blockScript.ChangeAllFloorMaterials();

            }
            if (GUILayout.Button("Change All Wall Materials"))
            {

                blockScript.ChangeAllWallMaterials();

            }
            EditorGUILayout.Space(5f);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            

            EditorGUILayout.EndVertical();


        }
        //EditorGUILayout.EndFoldoutHeaderGroup();



        serializedObject.ApplyModifiedProperties();
        
    }





}


