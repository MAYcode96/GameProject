using System;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public string lastScene;

    // NPC state
    public List<string> npcMet = new List<string>();
    public List<string> npcGone = new List<string>();
}