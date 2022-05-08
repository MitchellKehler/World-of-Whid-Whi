using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute
{
    /// <summary>
    /// 
    /// </summary>
    
    public AttributeName Name;
    public string Description;
    public Reward AttributeReward;
    private AttributeName Parent;

    public Attribute(AttributeName name)
    {
        switch (name)
        {
            // Aspects

            case AttributeName.Tough:
                AttributeReward = new Reward(RewardStat.HP_Multiplier, 1.25f);
                Description = "This creature can take a beating.";
                break;
            case AttributeName.VeryTough:
                AttributeReward = new Reward(RewardStat.HP_Multiplier, 1.5f);
                Description = "This creature can take a beating.";
                break;
            case AttributeName.Frail:
                AttributeReward = new Reward(RewardStat.HP_Multiplier, 0.75f);
                Description = "This creature is hurt easily.";
                break;
            case AttributeName.Fast:
                Description = "This creature is quick to react and moves quickly.";
                break;
            case AttributeName.Slow:
                Description = "This creature is slow to react and moves slowly.";
                break;
            case AttributeName.Intellegent:
                Description = "This creature is smarter then your average bear.";
                break;
            case AttributeName.VeryIntellegent:
                Description = "This creature is smarter then your average dolphin.";
                break;
            case AttributeName.Brilliant:
                Description = "This creature is smarter then your average rocket scientist.";
                break;
            case AttributeName.Dumb:
                Description = "This creature is not the sharpest tool in the shed.";
                break;
            case AttributeName.VeryDumb:
                Description = "Ok, a hammer would be sharper then this thing's mind.";
                break;
            case AttributeName.DumbLikeMud:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.PoisonResistant:
                AttributeReward = new Reward(RewardStat.Poison_Resistance, 2f);
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.Pestilent:
                Description = "I question whether this creature has a brain at all.";
                break;

            // Anatomy
            case AttributeName.Claws:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.SharpClaws:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.Claws;
                break;
            case AttributeName.VerySharpClaws:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.SharpClaws;
                break;
            case AttributeName.RasorSharpClaws:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.VerySharpClaws;
                break;
            case AttributeName.StrongClaws:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.Claws;
                break;
            case AttributeName.VeryStrongClaws:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.StrongClaws;
                break;
            case AttributeName.PowerfulClaws:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.VeryStrongClaws;
                break;
            case AttributeName.Talons:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.SharpTeath:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.StrongJaws:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.Beak:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.FlightlessWings:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.PowerfulLegs:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.HardHead:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.Horned:
                Parent = AttributeName.HardHead;
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.SharpHorned:
                Parent = AttributeName.Horned;
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.BalancingTail:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.LongTongue:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.PoisonStinger:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.PowerfulStinger:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.PoisonStinger;
                break;
            case AttributeName.PotentStinger:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.PowerfulStinger;
                break;
            case AttributeName.DeadlyStinger:
                Description = "I question whether this creature has a brain at all.";
                Parent = AttributeName.PotentStinger;
                break;

            // Natural Armor
            case AttributeName.ThickSkin:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.SuperThickSkin:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.Feathers:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.Fur:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.Chitin:
                Description = "I question whether this creature has a brain at all.";
                break;
            case AttributeName.Scales:
                Description = "I question whether this creature has a brain at all.";
                break;

            default:
                break;
        }
        Name = name;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsOrHasParent(AttributeName attribute)
    {
        if (Name == attribute)
        {
            return true;
        }

        if (Parent != AttributeName.None && new Attribute(Parent).IsOrHasParent(attribute))
        {
            return true;
        }

        return false;
    }
}

//public enum AttributeType
//{
//    NaturalArmor,
//    Anatomy,
//    Aspect
//}

public enum AttributeName
{
    None,

    // Fighting Knolege
    SavageInstincts,
    PackTackticks,

    // Aspects
    Strong, // + STR
    Tough, // + HP
    VeryTough, // + HP
    Weak, // - STR
    Frail, // - HP
    Agile, // + AGI
    Fast, // + Inititive
    Slow, // - AGI
    VerySlow, // - AGI
    Sluggish, // - Inititive
    Precise, // + Acuracy
    Clumsy, // - Acuracy
    WillFull, // + Will (or maybe Determined)
    Focused, // + Will, - Dodge Chance
    Stuburn, // + Will (for Resisting Mind), - INT
    Emotional, // + Will, - Will for mental control effects
    Logical, // - Will, + Will for mental control effects
    Intellegent,
    VeryIntellegent,
    Brilliant,
    Dumb,
    VeryDumb,
    DumbLikeMud,

    // Anatomy

    Claws,
    SharpClaws,
    VerySharpClaws,
    RasorSharpClaws,
    StrongClaws,
    VeryStrongClaws,
    PowerfulClaws,
    Talons,
    StrongTalons,
    SavageTalons,
    RaserTalons,
    SharpTeath,
    VerySharpTeath,
    RazorTeath,
    StrongJaws,
    PowerfulJaws, // strong jaws that can cause impact damage even without teath
    CrushingJaws,
    IronJaws, // At this point you are getting into the terretory of things like insects and crocodiles
    Beak,
    FlightlessWings, // flightless but can still be used for better jumping
    ClumbsyWings, // Enable flight but not at a very high level, like bat wings
    Wings, // Enable full unhindered flight
    InsectoidWings, // Insect wings, very agile but very delacate
    PowerfulLegs, // these legs can produce sudden burst of strength like forg or grass hopper legs
    ExplosiveLegs,
    StrongLegs, // These legs are just strong and fast like a horse's legs. Most four legad creatures would have this
    VeryStrongLegs,
    ExtremelyStrongLegs,
    HardHead, // Allows creatures to do headbutts
    Horned, // Improves headbutt and may add other perks
    SharpHorned,
    RazorSharpHorned,
    PoisonStinger,
    PowerfulStinger,
    PotentStinger,
    DeadlyStinger,
    SpikedTail,
    SharpSpikedTail,
    RazorSpikedTail,
    ClubTail,
    HeavyClubTail,
    BalancingTail, // Chicken or Rat
    GoodBalancingTail, // regular cat or elegant bird
    AmazingBalancingTail, // linx
    AcidSaliva,
    AcidBlood,
    LongTongue,
    PoisonResistant,
    FireProof,
    WaterResistant,
    Grounded,
    Pestilent,

    // NaturalArmor
    Skin,
    ThickSkin,
    SuperThickSkin,
    Feathers,
    Fur,
    ThickFur,
    Chitin,
    ThickChitin,
    SuperThickChitin,
    Scales,
    HeavyScales,
    SuperHeavyScales,
    ArcaneScales,
    StrongArcaneScales
}