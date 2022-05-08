using MLAPI.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityData : INetworkSerializable
{
    public AbilityName AbilityName;
    public string DisplayName;
    public bool LockedTargets;
    public int Speed;
    public int Rank;
    //ActionData[] Actions;

    public void NetworkSerialize(NetworkSerializer serializer)
    {

        serializer.Serialize(ref AbilityName);
        serializer.Serialize(ref DisplayName);
        serializer.Serialize(ref LockedTargets);
        serializer.Serialize(ref Speed);
        serializer.Serialize(ref Rank);


        //// Length
        //int length_Actions = 0;
        //if (!serializer.IsReading)
        //{
        //    length_Actions = Actions.Length;
        //}

        //serializer.Serialize(ref length_Actions);

        //// Array
        //if (serializer.IsReading)
        //{
        //    Actions = new ActionData[length_Actions];
        //}

        //for (int n = 0; n < length_Actions; ++n)
        //{
        //    Actions[n].NetworkSerialize(serializer);
        //}
    }

    public AbilityData() : base()
    {

    }

    public AbilityData(Ability ability)
    {
        AbilityName = ability.Name;
        DisplayName = ability.DisplayName;
        LockedTargets = ability.LockedTargets;
        Rank = ability.Rank;
        Speed = ability.Speed;

        //for (int i = 0; i < ability.Actions.Count; i++)
        //{
        //    Actions[i] = new ActionData(ability.Actions[i].NumberOfTargets, Actions[i].TargetType = ability.Actions[i].TargetType);
        //}
    }

    public AbilityData(Ability ability, int speed)
    {
        AbilityName = ability.Name;
        DisplayName = ability.DisplayName;
        LockedTargets = ability.LockedTargets;
        Rank = ability.Rank;
        Speed = speed;

        //for (int i = 0; i < ability.Actions.Count; i++)
        //{
        //    Actions[i] = new ActionData(ability.Actions[i].NumberOfTargets, Actions[i].TargetType = ability.Actions[i].TargetType);
        //}
    }
}

//public class ActionData : INetworkSerializable
//{
//    public int NumberOfTargets;
//    public TargetType TargetType;

//    public void NetworkSerialize(NetworkSerializer serializer)
//    {

//    }

//    public ActionData() : base()
//    {

//    }

//    public ActionData(int numberOfTargets, TargetType targetType)
//    {
//        NumberOfTargets = numberOfTargets;
//        TargetType = targetType;
//    }
//}

public class ActionTargetsData : INetworkSerializable
{
    public TargetType TargetType;
    public int[] ValidTargets;
    public int[] PickedTargets;
    public string AnimationName;
    // some kind of animation information

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref TargetType);
        serializer.Serialize(ref ValidTargets);
        serializer.Serialize(ref PickedTargets);
        serializer.Serialize(ref AnimationName);
    }

    public ActionTargetsData() : base()
    {

    }

    public ActionTargetsData(int[] pickedTargets, string animationName)
    {
        PickedTargets = pickedTargets;
        AnimationName = animationName;
    }

    public ActionTargetsData(TargetGroup group, TargetType type, int[] validTargets, int NumberOfTargets)
    {
        TargetType = type;
        ValidTargets = validTargets;
        PickedTargets = new int[NumberOfTargets];
        for (int i = 0; i < PickedTargets.Length; i++)
        {
            PickedTargets[i] = -1;
        }
    }

    public ActionTargetsData(TargetGroup group, TargetType type, int[] validTargets, int[] pickedTargets, int NumberOfTargets)
    {
        TargetType = type;
        ValidTargets = validTargets;
        PickedTargets = pickedTargets;
    }

}