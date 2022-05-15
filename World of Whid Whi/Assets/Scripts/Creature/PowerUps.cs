using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps
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

    public PowerUpStat TrackedStat; // this is the stat that will calculated to determine if the creature gets the power up yet. Can be none if the creature starts with the power up.
    public int StatGoal; // this is the stat goal of the stat that must be reached to recieve the power up.
    public Reward Reward; 

    public PowerUps(PowerUpStat trackedStat, int statGoal, Reward reward)
    {
        TrackedStat = trackedStat;
        StatGoal = statGoal;
        Reward = reward;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public AttributeName AttributeName;
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

    public Reward(AttributeName attribute)
    {
        Type = RewardType.Attribute;
        AttributeName = attribute; // maybe should just add an attribute
    }

    public Reward(AbilityName ability)
    {
        Type = RewardType.Ability;
        AbilityName = ability; // maybe should just add an attribute
    }

}

public enum PowerUpStat
{
    None,
    XP
}

public enum RewardType
{
    Stat,
    Attribute,
    //Level, Probably don't need Lvl. Lvls are just XP based power ups. May want to define standard XP Goals though which would be like levels.
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