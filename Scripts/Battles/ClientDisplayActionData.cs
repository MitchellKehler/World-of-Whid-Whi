using MLAPI.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientDisplayActionData : INetworkSerializable
{
    int CurrentCreature; // this is the index of the creature performing the action.
    ulong CreatureOwner;
    AnimationLength AnimationLength;
    int[] Targets;
    // Always move the creature to the correct location based on the number of targets before showing the action annimations.
    // If this is the last action then we also need to move it back to its original location after sowing the action animations.
    bool Last;

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref CurrentCreature);
        serializer.Serialize(ref CreatureOwner);
        serializer.Serialize(ref AnimationLength);
        serializer.Serialize(ref Targets);
        serializer.Serialize(ref Last);
    }

    public ClientDisplayActionData() : base()
    {

    }

    public ClientDisplayActionData(int currentCreature, ulong creatureOwner, int[] targets, bool isLast)
    {
        CurrentCreature = currentCreature;
        CreatureOwner = creatureOwner;
        Targets = targets;
        AnimationLength = AnimationLength.Normal;
        Last = isLast;
    }

    public ClientDisplayActionData(int currentCreature, ulong creatureOwner, int[] targets, bool isLast, AnimationLength animationLength)
    {
        CurrentCreature = currentCreature;
        CreatureOwner = creatureOwner;
        Targets = targets;
        AnimationLength = animationLength;
        Last = isLast;
    }

    public int GetCurrentCreature()
    {
        return CurrentCreature;
    }

    public int[] GetTargets()
    {
        return Targets;
    }

    public bool IsLast()
    {
        return Last;
    }
    public ulong GetCreatureOwner()
    {
        return CreatureOwner;
    }
    public AnimationLength GetAnimationLength()
    {
        return AnimationLength;
    }
}
