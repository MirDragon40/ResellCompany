using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockyGridRoomControls))]
public class BlockGridRoomControlEditor : Editor
{

    #region

    SerializedProperty ObstacleAmount;
    SerializedProperty EnemiesAmount;
    SerializedProperty NewFloorBlock;
    SerializedProperty FloorBlockAmountToChange;
    SerializedProperty NewWallBlock;
    SerializedProperty NewDoorway;
    SerializedProperty NewObstacles;
    SerializedProperty newCornerObjects;
    SerializedProperty CornerObjectAmount;
    SerializedProperty NewMateraialForThisRoom;


    #endregion

    public bool RoomControlVaribles = false;
    public bool RoomObstacleControls = false;
    public bool RoomEnemyControls = false;
    public bool RoomFloorControls = false;
    public bool RoomDoorwayControls = false;
    public bool RoomWallControls = false;
    public bool RoomMaterialControls = false;
    public bool RoomConterObjectsControls = false;



    public void OnEnable()
    {
        ObstacleAmount = serializedObject.FindProperty("ObstacleAmount");
        EnemiesAmount = serializedObject.FindProperty("EnemiesAmount");
        NewFloorBlock = serializedObject.FindProperty("NewFloorBlock");
        FloorBlockAmountToChange = serializedObject.FindProperty("FloorBlockAmountToChange");
        NewWallBlock = serializedObject.FindProperty("NewWallBlock");
        NewDoorway = serializedObject.FindProperty("NewDoorway");
        NewObstacles = serializedObject.FindProperty("NewObstacles");
        newCornerObjects = serializedObject.FindProperty("newCornerObjects");
        CornerObjectAmount = serializedObject.FindProperty("CornerObjectAmount");
        NewMateraialForThisRoom = serializedObject.FindProperty("NewMateraialForThisRoom");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BlockyGridRoomControls blockScript = (BlockyGridRoomControls)target;

        serializedObject.Update();

        
        //Floors
        EditorGUILayout.LabelField("Floors:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(NewFloorBlock);
        EditorGUILayout.PropertyField(FloorBlockAmountToChange);
        EditorGUI.indentLevel++;
        RoomFloorControls = EditorGUILayout.Foldout(RoomFloorControls, "Room Floor Controls", true);
        EditorGUI.indentLevel--;
        if (RoomFloorControls)
        {
            EditorGUILayout.LabelField("Room Floor Controls", EditorStyles.boldLabel);
            //Button in Inpector for Spawning Enemies
            if (GUILayout.Button("Change Floors (Random)"))
            {
                blockScript.ChangeFloors_Random();
            }
            //Button in Inpector for Spawning Enemies
            if (GUILayout.Button("Change Floors (All)"))
            {
                blockScript.ChangeFloors_All();
            }
            //Button in Inpector for Spawning Enemies
            if (GUILayout.Button("Rearrange Floor"))
            {
                blockScript.ReorderFloorBlocks();
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10f);

        //Walls
        EditorGUILayout.LabelField("Walls:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(NewWallBlock);
        EditorGUI.indentLevel++;
        RoomWallControls = EditorGUILayout.Foldout(RoomWallControls, "Room Wall Controls", true);
        EditorGUI.indentLevel--;
        if (RoomWallControls)
        {
            if (GUILayout.Button("Change All Walls"))
            {
                blockScript.ChangeWalls_All();
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10f);

        //Doorways
        EditorGUILayout.LabelField("Doorways:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(NewDoorway);
        EditorGUI.indentLevel++;
        RoomDoorwayControls = EditorGUILayout.Foldout(RoomDoorwayControls, "Room Doorway Controls", true);
        EditorGUI.indentLevel--;
        if (RoomDoorwayControls)
        {
            //EditorGUILayout.LabelField("Doorway Controls", EditorStyles.boldLabel);
            if (GUILayout.Button("Change All Doorways"))
            {
                blockScript.ChangeDoorways_All();
            }
            //Button in Inpector for Spawning Enemies
            if (GUILayout.Button("Close/Open Doorways"))
            {
                blockScript.CloseOrOpenDoorways();
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10f);

        //Obstacles
        EditorGUILayout.LabelField("Obstacles:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(ObstacleAmount);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(NewObstacles);
        EditorGUI.indentLevel--;
        //EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.indentLevel++;
        RoomObstacleControls = EditorGUILayout.Foldout(RoomObstacleControls, "Room Obstacle Controls", true);
        EditorGUI.indentLevel--;
        if (RoomObstacleControls)
        {
            //Button in Inpector for Rearranging Obstacles
            if (GUILayout.Button("Rearrange Obstacles"))
            {

                blockScript.ReorderObstacles();

            }
            //Button in Inpector for Adding Obstacles
            if (GUILayout.Button("Add Obstacles"))
            {
                blockScript.AddObstacles();


            }
            //Button in Inpector for Removing Obstacles
            if (GUILayout.Button("Remove Obstacles"))
            {
                blockScript.RemoveObstacles();


            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10f);

        //Corner Objects
        EditorGUILayout.LabelField("Corner Objects:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(CornerObjectAmount);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(newCornerObjects);
        EditorGUI.indentLevel--;
        //EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUI.indentLevel++;
        RoomConterObjectsControls = EditorGUILayout.Foldout(RoomConterObjectsControls, "Room Corner Objects Controls", true);
        EditorGUI.indentLevel--;
        if (RoomConterObjectsControls)
        {
            //Button in Inpector for Changing Corner Spots and Obejcts
            if (GUILayout.Button("Change All Corner Objects"))
            {

                blockScript.ChangeCornerObjectsAll();

            }
            //Button in Inpector for Adding Corner Object
            if (GUILayout.Button("Add Corner Object"))
            {
                blockScript.CornerSpotAdd();


            }
            //Button in Inpector for Removing Corner Object
            if (GUILayout.Button("Remove Corner Object"))
            {
                blockScript.RemoveCornerObejcts();


            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10f);


        //Enemies
        EditorGUILayout.LabelField("Enemies:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(EnemiesAmount);
        EditorGUI.indentLevel++;
        RoomEnemyControls = EditorGUILayout.Foldout(RoomEnemyControls, "Room Enemy Controls", true);
        EditorGUI.indentLevel--;
        if (RoomEnemyControls)
        {
            //Button in Inpector for Spawning Enemies
            if (GUILayout.Button("Spawn Enemies"))
            {
                blockScript.SpawnEnemiesInRoom();
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10f);

        //Materials
        EditorGUILayout.LabelField("Materials:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.PropertyField(NewMateraialForThisRoom);
        EditorGUI.indentLevel++;
        RoomMaterialControls = EditorGUILayout.Foldout(RoomMaterialControls, "Room Material Controls", true);
        EditorGUI.indentLevel--;
        if (RoomMaterialControls)
        {
            EditorGUILayout.LabelField("Room Material Controls", EditorStyles.boldLabel);
            //Button in Inpector for Spawning Enemies
            if (GUILayout.Button("Change All Floor Materials (And Doorways)"))
            {
                blockScript.ChangeAllFloorBlockMaterial();
            }
            //Button in Inpector for Spawning Enemies
            if (GUILayout.Button("Change All Wall Block Materials)"))
            {
                blockScript.ChangeAllWallMaterials();
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10f);

        //Center Blocks
        EditorGUILayout.LabelField("Center Block Area:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("GroupBox");
        //Button in Inpector for Spawning Enemies
        if (GUILayout.Button("Remove Center Block Area"))
        {
            blockScript.RemoveCenterBlocks();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(10f);



        serializedObject.ApplyModifiedProperties();


    }
}
