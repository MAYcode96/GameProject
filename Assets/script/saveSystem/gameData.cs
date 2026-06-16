using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenePosition
{
    public string sceneName;
    public float x;
    public float y;
    public float z;

    // Constructor bawaan agar pembuatan data baru di GameManager lebih bersih
    public ScenePosition(string name, Vector3 pos)
    {
        sceneName = name;
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }
}

[System.Serializable]
public class NPCSaveData
{
    public string npcID;
    public string sceneName;
    public float x;
    public float y;
    public float z;

    public NPCSaveData(string id, string scene, Vector3 pos)
    {
        npcID = id;
        sceneName = scene;

        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
}

[System.Serializable]
public class NPCDialogueState
{
    public string npcID;
    public int dialogueIndex;

    public NPCDialogueState(string id, int index)
    {
        npcID = id;
        dialogueIndex = index;
    }
}

[System.Serializable]
public class GameData
{
    public string lastScene;
    public List<ScenePosition> scenePositions = new List<ScenePosition>();
    public List<NPCSaveData> npcPositions = new List<NPCSaveData>();
    public List<NPCDialogueState> npcDialogueStates = new List<NPCDialogueState>();
    public List<string> npcMet = new List<string>();
    public List<string> npcGone = new List<string>();

    // TAMBAHKAN INI: Untuk menyimpan ID objek yang sudah di-unlock
    public List<string> objectUnlocked = new List<string>();
<<<<<<< Updated upstream
=======
    public List<InventorySaveData> inventorySaveData = new List<InventorySaveData>();
// TAMBAHKAN BARIS INI: List untuk menyimpan ID unik barang di world yang sudah diambil
    public List<string> collectedItems = new List<string>();
>>>>>>> Stashed changes
}