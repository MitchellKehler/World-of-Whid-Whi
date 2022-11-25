using MLAPI.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitializedCreatureData : INetworkSerializable
{
    public string Name;
    public string NickName;
    public int CurrentXP; // many not need it (just limited by it becoming too difficult to gan XP)
    //public PowerUpStat[] StatsToTrack;
    //public int[] CurrentAmounts;
    //List<Condition> Conditions; // I think I will need this here.
    public int CurrentHP;
    public ulong Owner;
    private int ID;
    public int battleCreatureID; // just used in battles and set at the start of each battle.
    public CreatureSize Size;
    public int Strength;
    public int Agility;
    public int Mind;
    public int Will;

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref Name);
        serializer.Serialize(ref NickName);
        serializer.Serialize(ref CurrentXP);
        serializer.Serialize(ref CurrentHP);
        serializer.Serialize(ref Owner);
        serializer.Serialize(ref ID);
        serializer.Serialize(ref battleCreatureID);
        serializer.Serialize(ref Size);
        serializer.Serialize(ref Strength);
        serializer.Serialize(ref Agility);
        serializer.Serialize(ref Mind);
        serializer.Serialize(ref Will);

        //int length_StatsToTrack = 0;
        //if (!serializer.IsReading)
        //{
        //    length_StatsToTrack = StatsToTrack.Length;
        //}

        //serializer.Serialize(ref length_StatsToTrack);

        //if (serializer.IsReading)
        //{
        //    StatsToTrack = new PowerUpStat[length_StatsToTrack];
        //    CurrentAmounts = new int[length_StatsToTrack];
        //}

        //for (int n = 0; n < length_StatsToTrack; ++n)
        //{
        //    serializer.Serialize(ref StatsToTrack[n]);
        //    serializer.Serialize(ref CurrentAmounts[n]);
        //}

        //// Length
        //int length_KnownAbilities = 0;
        //if (!serializer.IsReading)
        //{
        //    length_KnownAbilities = KnownAbilities.Length;
        //}

        //serializer.Serialize(ref length_KnownAbilities);

        //// Array
        //if (serializer.IsReading)
        //{
        //    KnownAbilities = new AbilityName[length_KnownAbilities];
        //}

        //for (int n = 0; n < length_KnownAbilities; ++n)
        //{
        //    serializer.Serialize(ref KnownAbilities[n]);
        //}

    }

    public InitializedCreatureData() : base()
    {
        //TrackedStat = new TrackedStat();
    }

    //ID, character, name, nick_name, current_hp, size, strength, agility, mind, will, xp
    public InitializedCreatureData(int ID, string name, string nick_name, int current_hp, string size, int strength, int agility, int mind, int will, int xp)
    {
        this.ID = ID;
        Name = name;
        NickName = nick_name; // Need to add this
        CurrentHP = current_hp;
        Enum.TryParse(size, out Size);
        Strength = strength;
        Agility = agility;
        Mind = mind;
        Will = will;
        CurrentXP = xp;
    }

    public InitializedCreatureData(BaseCreature creature)
    {
        Debug.Log("In InitializedCreatureData constructor 1");
        // should find a way to avoid applying power ups while creating the initialized creature here
        InitializedCreature initializedCreature = new InitializedCreature(creature);
        Debug.Log("created initializedCreature");
        SetVeriables(initializedCreature);
        Debug.Log("finished SetVeriables");
        ID = -1;
    }

    public InitializedCreatureData(InitializedCreature creature)
    {
        Debug.Log("In InitializedCreatureData constructor 2");
        SetVeriables(creature);
        Debug.Log("finished SetVeriables");
        ID = creature.GetID();
    }

    public InitializedCreatureData(InitializedCreature creature, int iD)
    {
        // should instead call it's own constructor that uses an initialized creature and then set the ID at the end.
        SetVeriables(creature);
        ID = iD;
        //StatToTrack = iterate through power ups to comparing against tracked stats to see what power ups should be added.
    }

    public void SetVeriables(InitializedCreature creature)
    {
        Name = creature.Name;
        NickName = creature.NickName;
        Size = creature.Size;
        //Armor = creature.Armor;
        //General_Resistance = creature.General_Resistance;
        //Fire_Resistance = creature.Fire_Resistance;
        //Water_Resistance = creature.Water_Resistance;
        //Poison_Resistance = creature.Poison_Resistance;
        //Electric_Resistance = creature.Electric_Resistance;
        //Death_Resistance = creature.Death_Resistance;

        //if (creature.TrackedStat.StatsToTrack.Length > 0)
        //    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "SetVeriables Befire TrackedStat.StatsToTrack[0]" + creature.TrackedStat.StatsToTrack[0]);
        //else
        //    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "SetVeriables Befire TrackedStat.StatsToTrack.Length == 0.");

        //StatsToTrack = (PowerUpStat[])creature.TrackedStats.Keys.ToArray();
        //CurrentAmounts = (int[])creature.TrackedStats.Values.ToArray();

        //if (TrackedStat.StatsToTrack.Length > 0)
        //    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "SetVeriables After TrackedStat.StatsToTrack[0]" + TrackedStat.StatsToTrack[0]);
        //else
        //    GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "SetVeriables After TrackedStat.StatsToTrack.Length == 0.");

        //KnownAbilities = new AbilityName[creature.KnownAbilities.Count]; //creature.KnownAbilities.ToArray();
        //for (int i = 0; i < creature.KnownAbilities.Count; i++)
        //{
        //    KnownAbilities[i] = creature.KnownAbilities[i].AbilityName;
        //}
        CurrentXP = creature.CurrentXP; // many not need it (just limited by it becoming too difficult to gan XP)

        Strength = creature.Strength;
        Agility = creature.Agility;
        Will = creature.Will;
        Mind = creature.Mind;
        CurrentHP = creature.CurrentHP;
        //HPMultiplier = creature.HPMultiplier;

        //Debug.Log("New Creature Data!");
        //Debug.Log("Level = " + CurrentLvl);

        //Debug.Log("New Creature!");
        //Debug.Log("Armor = " + Armor);
        //Debug.Log("General_Resistance = " + General_Resistance);
        //Debug.Log("Agility = " + Agility);
        //Debug.Log("Mind = " + Mind);
        //Debug.Log("Will = " + Will);
        //Debug.Log("Strength = " + Strength);
        //Debug.Log("Total Strength = " + creature.GetTotalStrength());
    }

    public int GetID()
    {
        return ID;
    }
}


public class TrackedStat : INetworkSerializable
{
    public PowerUpStat[] StatsToTrack;
    public int[] CurrentAmount;

    public TrackedStat() : base()
    {
        //StatsToTrack = new PowerUpStat[0];
        //CurrentAmount = new int[0];
    }

    //public TrackedStat(PowerUpStat[] powerUpStats)
    //{
    //    for(int i = 0; i < powerUpStats.Length; i++)
    //    {
    //        //StatsToTrack[i] = powerUpStats[i];
    //        //CurrentAmount[i] = 0;
    //    }
    //}

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Starting TrackedStat NetworkSerialize");

        int length_StatsToTrack = 0;
        if (!serializer.IsReading)
        {
            length_StatsToTrack = StatsToTrack.Length;
        }

        serializer.Serialize(ref length_StatsToTrack);

        if (serializer.IsReading)
        {
            StatsToTrack = new PowerUpStat[length_StatsToTrack];
            CurrentAmount = new int[length_StatsToTrack];
        }

        for (int n = 0; n < length_StatsToTrack; ++n)
        {
            serializer.Serialize(ref StatsToTrack[n]);
            serializer.Serialize(ref CurrentAmount[n]);
        }
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Ending TrackedStat NetworkSerialize");
    }

    public TrackedStat Clone()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Cloneing TrackedStat");

        TrackedStat stat = new TrackedStat();
        stat.StatsToTrack = (PowerUpStat[])this.StatsToTrack;
        stat.CurrentAmount = (int[])this.CurrentAmount;
        return stat;
    }

}