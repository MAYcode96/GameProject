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
public class GameData
{
    public string lastScene;
    public List<ScenePosition> scenePositions = new List<ScenePosition>();
    public List<string> npcMet = new List<string>();
    public List<string> npcGone = new List<string>();

    // TAMBAHKAN INI: Untuk menyimpan ID objek yang sudah di-unlock
    public List<string> objectUnlocked = new List<string>();
}