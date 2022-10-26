using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use similar setup as for creating creatures from base creature
/// Each PowerUpGroup should have a description and a list of powerups (I think that's it)
/// Power ups will have conditions and a reward
/// 
/// </summary>
public class PowerUpGroup
{
    public string description;
    public List<PowerUp> powerups;
    public PowerUpGroupType type;

    public PowerUpGroup(string description, List<PowerUp> powerups, PowerUpGroupType type)
    {
        this.description = description;
        this.powerups = powerups;
        this.type = type;
    }

    public PowerUpGroup Clone()
    {
        List<PowerUp> powerupsClone = new List<PowerUp>();
        foreach (PowerUp powerup in powerups)
        {
            powerupsClone.Add(powerup.Clone());
        }
        return new PowerUpGroup(description, powerupsClone, type);
    }
}


public class PowerUp
{
    /// <summary>
    /// Power Ups
    /// 
    /// In WWW each creature has a number of powerups where if they have achived a certain goal (most common example would be a level) they get certain bonuses.
    /// Each power up has a tracked stat and required amount in that stat. 
    /// </summary>
    /// 

    // Needs a list of conditions!!!!!!!!!!!!!!!!!!!!!! condition types, powerUpStat, baseStat, Attribute

    public const int NUMBER_OF_POWERUP_STATS = 2;

    public List<PowerUpCondition> conditions;
    public Reward Reward;
    public bool recieved;

    public PowerUp(List<PowerUpCondition> myConditions, Reward reward)
    {
        conditions = myConditions;
        Reward = reward;
        recieved = false;
    }

    public PowerUp Clone()
    {
        return new PowerUp(conditions, Reward);
    }
}

public class PowerUpCondition
{
    public PowerUpStat TrackedStat; // this is the stat that will calculated to determine if the creature gets the power up yet. Can be none if the creature starts with the power up.
    public int StatGoal; // this is the stat goal of the stat that must be reached to recieve the power up.

    public PowerUpCondition (PowerUpStat trackedStat, int statGoal)
    {
        TrackedStat = trackedStat;
        StatGoal = statGoal;
    }
}

// I may want to move this to it's own file.
public class Reward
{
    // Maybe should just have the attribute name here if it is an attribute and not bother with a separate attribute class.
    public RewardType Type; // The type of reward. e.g. basic stat, ability, level up, attribute, evolution...
    public float Amount; // The quantity of the reward that will be recieved for simple reward types
    public string Name; // The name of the reward if it is a complex type like an ability, attribute, or evelution
    public RewardStat RewardStat; // The stat that will be incarease for simple reward types.
    public AbilityName AbilityName;

    public Reward(RewardType type, string name)
    {
        Type = type;
        Name = name;
    }

    public Reward(RewardStat stat, float amount)
    {
        Type = RewardType.Stat;
        RewardStat = stat;
        Amount = amount;
    }

    public Reward(AbilityName ability)
    {
        Type = RewardType.Ability;
        AbilityName = ability;
    }

}

public enum PowerUpStat
{
    None,
    XP,
    STR,
    AGI,
    WILL,
    MIND,
    SIZE
}

public enum RewardType
{
    Stat,
    Attribute,
    Ability,
    Evolution,
    Lvl
}

public enum RewardStat
{
    HP,
    HP_Multiplier, // If we want to allow multipliers. I don't know yet.
    Strength,
    Agility,
    Mind,
    Will,
    Size,
    Armour,
    General_Resistance,
    Fire_Resistance,
    Water_Resistance,
    Poison_Resistance,
    Electric_Resistance,
    Death_Resistance
}