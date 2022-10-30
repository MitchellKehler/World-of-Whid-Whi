using MLAPI.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : INetworkSerializable
{
    public ulong ID;
    public int Account;
    public string Name;
    public int Lvl;
    public int XP;
    public string Location;
    public float Position_X;
    public float Position_Y;

    public List<InitializedCreatureData> OwnedCreatures;
    public List<InitializedCreatureData> CurrentCreatureTeam;
    public SpawnPoint spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NetworkSerialize(NetworkSerializer serializer)
    {

        serializer.Serialize(ref ID);
        serializer.Serialize(ref Account);
        serializer.Serialize(ref Name);
        serializer.Serialize(ref Lvl);
        serializer.Serialize(ref XP);
        serializer.Serialize(ref Location);
        serializer.Serialize(ref Position_X);
        serializer.Serialize(ref Position_Y);
    }

    public CharacterData() : base()
    {

    }

    public CharacterData(int account)
    {
        Account = account;
    }

    public CharacterData(ulong id, int account, string name, int lvl, int xp, string location, float x, float y)
    {
        ID = id;
        Account = account;
        Name = name;
        Lvl = lvl;
        XP = xp;
        Location = location;
        Position_X = x;
        Position_Y = y;
    }


}
