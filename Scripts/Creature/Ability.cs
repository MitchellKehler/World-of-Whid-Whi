using MLAPI.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ability
{
    /*
     * Some way to determine if self target is optional, mandatory, or not allowed
    * Friendly Targets List
    * Enemy Targets List
    * Energy Cost
    * Speed
    * Acuracy (or maybe this would just be an attribut if it was more of an uncommon thing)
    * Other attributes (Maybe?)
    *
    * Effect List 
    * could be damage (With a damage type) from an attack, buffs to stats, conditions (Need to add these) like poison. And so on.
    * Maybe have some way to indicate effect for primary target VS secondary. Maybe have a list of lists and the final list gets applied to 
    * all remaining?
    * Friendly Effect List (Effects will generally be buffs but could be negative in some secenarios)
    * Enemy Effect List
    * 
    *       Each ability has
                a Name
                A Description Text
                A List of Actions
                A predictability rating

            Each action has
                A number of targets
                An indicator of weatehr the targets are frienly or enemy
                A list of effects (simplest effect would be to apply damage)
                An Animation
    */

    public AbilityName Name;
    public int Rank;
    public AbilityName UpgradedAbility;
    public string DisplayName;
    public string Description;
    public Pradictability Pradictability;
    public List<Action> Actions;
    public bool LockedTargets; // Can targets be different from one action to the next.
    public bool Upgrades;
    public int Speed;

    public Ability(AbilityName name, int rank, string displayName, string description, int speed, Pradictability pradictability, List<Action> actions, bool lockedTargets)
    {
        Name = name;
        Rank = rank;
        DisplayName = displayName;
        Description = description;
        Pradictability = pradictability;
        Actions = actions;
        LockedTargets = lockedTargets;
        Speed = speed;
        Upgrades = false;
    }

    public Ability(AbilityName name, int rank, string displayName, string description, int speed, Pradictability pradictability, List<Action> actions, bool lockedTargets, AbilityName upgrade)
    {
        Name = name;
        Rank = rank;
        DisplayName = displayName;
        Description = description;
        Pradictability = pradictability;
        Actions = actions;
        LockedTargets = lockedTargets;
        Speed = speed;
        UpgradedAbility = upgrade;
        Upgrades = true;
    }

    public Ability Clone()
    {
        List<Action> actions = new List<Action>();
        foreach (Action action in this.Actions)
        {
            actions.Add(action.Clone());
        }
        return new Ability(this.Name, this.Rank, this.DisplayName, this.Description, this.Speed, this.Pradictability, actions, this.LockedTargets);
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

public class Action
{ 
    // this is where the animation is set
    public int[] Targets;
    public string UseDescription;
    public TargetGroup TargetGroup;
    public TargetType TargetType;
    public List<Effect> Effects;
    public AttackType AttackType;
    public bool IsBlockable; // May not need this
    public bool IsDodgeable; // May not need this
    public AnimationLength AnimationLength;
    public string AnimationName;

    public Action(int numberOfTargets, string use, TargetGroup targetGroup, TargetType targetType, List<Effect> effects, AttackType attackType, string animationName)
    {
        Targets = new int[numberOfTargets];
        for (int i = 0; i < Targets.Length; i++)
        {
            Targets[i] = -1;
        }
        UseDescription = use;
        TargetGroup = targetGroup;
        TargetType = targetType;
        Effects = effects;
        AttackType = attackType;
        IsBlockable = true;
        IsDodgeable = true;
        AnimationLength = AnimationLength.Normal;
        AnimationName = animationName;
    }

    public Action(int numberOfTargets, string use, TargetGroup targetGroup, TargetType targetType, List<Effect> effects, AttackType attackType, string animationName, bool isBlocable, bool isDodgeable)
    {
        Targets = new int[numberOfTargets];
        for (int i = 0; i < Targets.Length; i++)
        {
            Targets[i] = -1;
        }
        UseDescription = use;
        TargetGroup = targetGroup;
        TargetType = targetType;
        Effects = effects;
        AttackType = attackType;
        IsBlockable = isBlocable;
        IsDodgeable = isDodgeable;
        AnimationLength = AnimationLength.Normal;
        AnimationName = animationName;
    }

    public Action(int numberOfTargets, string use, TargetGroup targetGroup, TargetType targetType, List<Effect> effects, AttackType attackType, string animationName, bool isBlocable, bool isDodgeable, AnimationLength animationLength)
    {
        Targets = new int[numberOfTargets];
        for (int i = 0; i < Targets.Length; i++)
        {
            Targets[i] = -1;
        }
        UseDescription = use;
        TargetGroup = targetGroup;
        TargetType = targetType;
        Effects = effects;
        AttackType = attackType;
        IsBlockable = isBlocable;
        IsDodgeable = isDodgeable;
        AnimationLength = animationLength;
        AnimationName = animationName;
    }

    public Action Clone()
    {
        List<Effect> effects = new List<Effect>();
        //Debug.Log("Action " + this.UseDescription + " is being cloned");
        foreach (Effect effect in Effects)
        {
            effects.Add(effect.Clone());
        }
        return new Action(Targets.Length, this.UseDescription, this.TargetGroup, this.TargetType, effects, this.AttackType, this.AnimationName, this.IsBlockable, this.IsDodgeable, this.AnimationLength);
    }
}

public enum Pradictability
{
    Pradictable, // can tell exactly what ability is being used and when it will execute.
    Unpradictable, // can tell what ability is being used but not when it will execute.
    Unreadable, // can not tell what ability is being used but can tell when it will be executed
    Impossible // can not tell the ability being used or when it will be executed.
}

public enum TargetGroup
{
    Self,
    Friendly,
    Friendly_Other,
    Enemy,
    All
}
public enum TargetType
{
    Positive,
    Negative
}

public class Effect
{
    public EffectType EffectType;
    //public ConditionType ConditionType; // Probably need a class for conditions that this corisponds to. That is where things like the condition indicator image and description can be stored.
    public AmountType AmountType;
    public float Amount; // May need a string version for more advanced types. We will see.
    public float Amount2;

    // Constructor 1
    public Effect(EffectType effectType, AmountType amountType, float amount)
    {
        EffectType = effectType;
        AmountType = amountType;
        Amount = amount;
    }

    // Constructor 2
    public Effect(EffectType effectType, AmountType amountType, float amount, float amount2)
    {
        EffectType = effectType;
        AmountType = amountType;
        Amount = amount;
        Amount2 = amount2;
    }

    public Effect Clone()
    {
        return new Effect(this.EffectType, this.AmountType, this.Amount);
    }
}

public enum AmountType
{
    Constant, // Constructor 1
    STR_Multiplier, // Constructor 1
    AGI_Multiplier, // Constructor 1
    INT_Multiplier, // Constructor 1
    WILL_Multiplier, // Constructor 1
    SizeDiff_Multiplier, // Constructor 1
    SizeDiff_Multiplier_Plus // Constructor 2
}

public enum AttackType
{ // Melee and Ranged are dodged and blocked based on AGI, Mind and Touch are dodged and blocked based on Will.
    Melee,
    Range,
    Mind, // Mind Ranged
    Touch // Mind Touch
}

public enum EffectType
{
    PhisicalDamage,
    ImpactDamage,
    TrueDamage, // Ignore Armour and Resistance, Generally Impact
    ElectricDamage, // Air
    FireDamage, // Fire
    WaterDamage, // Water, Often Ice
    PoisonDamage, // Earth
    DeathDamage,
    ArcaneDamage,
    LifeHealing,
    Penetration,
    Slow

}

public enum ConditionType
{
    Poisoned,
    Stun,
    Slow,
    SpeedUp,
    IncreaseSTR,
    IncreaseAGI,
    IncreaseINT,
    IncreaseWILL,
    DecreaseSTR,
    DecreaseAGI,
    DecreaseINT,
    DecreaseWILL,
    Confuse,
    IncreaseArmour,
    IncreaseResistance,
    Sheild, // Blocks a small amount of each incoming attack
    Barier // Adds a temporary increase of hitpoints
}

public enum AnimationLength
{
    Short,
    Normal,
    Long
}