using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using System;
using System.Linq;

public class InitializedCreature
{
    private GameManager GM;

    public string Name;
    public string Path;
    public string ShortName; // used for displays like the battle icons where space is limited.
    public string NickName;
    public string Description;
    public List<CreatureType> Types;
    public CreatureSize Size;
    public int Armor;
    public int General_Resistance;
    public int Fire_Resistance;
    public int Water_Resistance;
    public int Poison_Resistance;
    public int Electric_Resistance;
    public int Death_Resistance;
    public List<AbilityData> KnownAbilities;
    public List<AttributeName> CurrentAttributes;
    public Dictionary<PowerUpStat, int> TrackedStats;
    public int CurrentLvl; // many not need it (just limited by it becoming too difficult to gan XP)
    public Rating Rating;

    public int Strength;
    public int Agility;
    public int Will;
    public int Mind;
    public int CurrentHP;
    public float HPMultiplier;
    List<Condition> Conditions;

    //Not set up properly yet
    public InitializedCreature(string MyName, string MyShortName, List<CreatureType> MyTypes, CreatureSize MySize,
    CreatureIntelligence MyIntelligence, List<Ability> MyStartingAbilities, List<PowerUps> MyPowerUps, int MyMaxLvl,
    Rating MyRating, int MyPowerLevel)
    {

    }

    public InitializedCreature(BaseCreature NewBaseCreature)
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //GM.LogToServerRpc(0, "In InitializedCreature Constructor");

        CurrentAttributes = new List<AttributeName>();
        KnownAbilities = new List<AbilityData>();
        TrackedStats = new Dictionary<PowerUpStat, int>();
        TrackedStats.Add(PowerUpStat.XP, 10);

        HPMultiplier = 1;

        Name = NewBaseCreature.Name;
        Path = NewBaseCreature.Path;
        ShortName = NewBaseCreature.ShortName;
        Description = NewBaseCreature.Description;
//        Types = MyTypes;
        Size = NewBaseCreature.Size;

        // these will increase with powerups
        CurrentLvl = 1;
        Rating = NewBaseCreature.Rating;
        Armor = 0;
        General_Resistance = 0;
        Fire_Resistance = 0;
        Water_Resistance = 0;
        Poison_Resistance = 0;
        Electric_Resistance = 0;
        Death_Resistance = 0;
        Agility = 0;
        Mind = 0;
        Will = 0;
        Strength = 0;

        AddBaseStats(NewBaseCreature.Rating, NewBaseCreature.CreatureTypePercents, 10);

        // I don't like this. I think Agiliy should be effected little if at all by size. Size should effect dodge chance more. Maybe need a new category along with size and intelegence of athletecisim? also need to add mind to things that impact initiative. 

        GetInitialPowerups();
        ApplyArmor();
        ApplyPowerups();

        CurrentHP = GetMaxHp();
        Conditions = new List<Condition>();


        //Debug.Log("New Creature!");
        //Debug.Log("Level = " + CurrentLvl);
        //Debug.Log("Armor = " + Armor);
        //Debug.Log("General_Resistance = " + General_Resistance);
        //Debug.Log("Agility = " + Agility);
        //Debug.Log("Mind = " + Mind);
        //Debug.Log("Will = " + Will);
        //Debug.Log("Strength = " + Strength);
        //Debug.Log("Total Strength = " + GetTotalStrength());
    }

    public InitializedCreature(InitializedCreatureData NewCreatureData)
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        BaseCreature NewBaseCreature = GM.AllCreatures.Find(creature => creature.Name.Equals(NewCreatureData.Name));

        //GM.LogToServerRpc(0, "In InitializedCreature Constructor");

        HPMultiplier = NewCreatureData.HPMultiplier;

        Name = NewCreatureData.Name;
        Path = NewBaseCreature.Path;
        NickName = NewCreatureData.NickName;
        ShortName = NewBaseCreature.ShortName;
        Description = NewBaseCreature.Description;
        //        Types = MyTypes;
        Size = NewCreatureData.Size;

        // these will increase with powerups
        Armor = NewCreatureData.Armor;
        General_Resistance = NewCreatureData.General_Resistance;
        Fire_Resistance = NewCreatureData.Fire_Resistance;
        Water_Resistance = NewCreatureData.Water_Resistance;
        Poison_Resistance = NewCreatureData.Poison_Resistance;
        Electric_Resistance = NewCreatureData.Electric_Resistance;
        Death_Resistance = NewCreatureData.Death_Resistance;
        Strength = NewCreatureData.Strength;
        Agility = NewCreatureData.Agility;
        Mind = NewCreatureData.Mind;
        Will = NewCreatureData.Will;

        CurrentAttributes = (List<AttributeName>)NewCreatureData.CurrentAttributes.ToList(); // should look like the known abilities, need to make an AttributeData first.

        KnownAbilities = new List<AbilityData>(); // NewCreatureData.KnownAbilities.ToList();
        for (int i = 0; i < NewCreatureData.KnownAbilities.Length; i++)
        {
            KnownAbilities.Add(new AbilityData(AllAbilities.CloneAbility(NewCreatureData.KnownAbilities[i])));
        }

        //GM.LogToServerRpc(0, "Before " + NewCreatureData.TrackedStat.Clone());
        TrackedStats = new Dictionary<PowerUpStat, int>();
        for (int i = 0; i < NewCreatureData.StatsToTrack.Length; i++)
        {
            TrackedStats.Add(NewCreatureData.StatsToTrack[i], NewCreatureData.CurrentAmounts[i]);
        }

        // TODO Add to the power up stats based on the stats passed in InitializedCreatureData
        //        KnownAbilities = MyStartingAbilities;
        //        RecievedPowerUps = MyPowerUps; // May add to abilities, attributes, and regular stats.
        CurrentLvl = NewCreatureData.CurrentLvl;
        Rating = NewBaseCreature.Rating;

        CurrentHP = NewCreatureData.CurrentHP;

        Conditions = new List<Condition>();

        //Debug.Log("Data Creature Initialized");
        //Debug.Log("Level = " + CurrentLvl);
    }

    // number to add is multiplied based on rating
    public void AddBaseStats(Rating rating, CreatureTypePercents creatureTypePercents, int numberToAdd)
    {
        int TotalStatsToAdd = 0;
        switch (rating)
        {
            case Rating.Pathetic:
                TotalStatsToAdd = 2;
                break;
            case Rating.Weak:
                TotalStatsToAdd = 4;
                break;
            case Rating.Average:
                TotalStatsToAdd = 5;
                break;
            case Rating.AboveAverage:
                TotalStatsToAdd = 6;
                break;
            case Rating.Powerful:
                TotalStatsToAdd = 7;
                break;
            case Rating.VeryPowerfull:
                TotalStatsToAdd = 8;
                break;
            case Rating.Epic:
                TotalStatsToAdd = 10;
                break;
            case Rating.Legendary:
                TotalStatsToAdd = 12;
                break;
            case Rating.Mythic:
                TotalStatsToAdd = 15;
                break;
        }

        //Debug.Log("rating = " + rating);
        TotalStatsToAdd = TotalStatsToAdd * numberToAdd;
        int RandomStatsToAdd = (int)Math.Ceiling((double)(TotalStatsToAdd * .25f));
        //Debug.Log("RandomStatsToAdd = " + RandomStatsToAdd);
        int TypeStatsToAdd = (int)Math.Floor((double)(TotalStatsToAdd * .75f));
        //Debug.Log("TypeStatsToAdd = " + TypeStatsToAdd);

        //for (int i = 0; i < RandomStatsToAdd; i++)
        //{
        //    int randomPercent = UnityEngine.Random.Range(0, 100);
        //    if (randomPercent < 25)
        //    {
        //        Strength++;
        //    }
        //    else if (randomPercent < 50)
        //    {
        //        Agility++;
        //    }
        //    else if (randomPercent < 75)
        //    {
        //        Mind++;
        //    }
        //    else
        //    {
        //        Will++;
        //    }
        //}

        int BloodStatsToAdd = (int)(TypeStatsToAdd * (((float)creatureTypePercents.Blood) / 100f));
        TypeStatsToAdd -= BloodStatsToAdd;

        //Debug.Log("TotalStatsToAdd = " + TotalStatsToAdd);
        //Debug.Log("TypeStatsToAdd = " + TypeStatsToAdd);
        //Debug.Log("RandomStatsToAdd = " + RandomStatsToAdd);
        //Debug.Log("BloodStatsToAdd = " + BloodStatsToAdd);
        //Debug.Log("creatureTypePercents.Blood = " + creatureTypePercents.Blood);

        for (int i = 0; i < BloodStatsToAdd + RandomStatsToAdd; i++)
        {
            // Blood - same amount of each
            int totalStats = Strength + Agility + Mind + Will;
            if (totalStats % 4 == 0)
            {
                Strength++;
            }
            else if (totalStats % 3 == 0)
            {
                Agility++;
            }
            else if (totalStats % 2 == 0)
            {
                Mind++;
            }
            else
            {
                Will++;
            }
        }

        for (int i = 0; i < TypeStatsToAdd; i++)
        {
            int randomPercent = creatureTypePercents.Blood + UnityEngine.Random.Range(0, 100 - creatureTypePercents.Blood);
            if (creatureTypePercents.Fire >= randomPercent)
            {
                // Fire - Will
                Will++;
            }
            else if (creatureTypePercents.Fire + creatureTypePercents.Water >= randomPercent)
            {
                // Water - Mind
                Mind++;
            }
            else if (creatureTypePercents.Fire + creatureTypePercents.Water + creatureTypePercents.Earth >= randomPercent)
            {
                // Earth - Strength
                Strength++;
            }
            else if (creatureTypePercents.Fire + creatureTypePercents.Water + creatureTypePercents.Earth + creatureTypePercents.Air >= randomPercent)
            {
                // Air - Agility
                Agility++;
            }
            else if (creatureTypePercents.Fire + creatureTypePercents.Water + creatureTypePercents.Earth + creatureTypePercents.Air
                + creatureTypePercents.Life >= randomPercent)
            {
                // Life - Strength or Will
                randomPercent = UnityEngine.Random.Range(0, 100);
                if (randomPercent < 50)
                {
                    Strength++;
                } else
                {
                    Will++;
                }
            }
            else if (creatureTypePercents.Fire + creatureTypePercents.Water + creatureTypePercents.Earth + creatureTypePercents.Air
                + creatureTypePercents.Life + creatureTypePercents.Death >= randomPercent)
            {
                // Death - Mind or Agility
                randomPercent = UnityEngine.Random.Range(0, 100);
                if (randomPercent < 50)
                {
                    Mind++;
                }
                else
                {
                    Agility++;
                }
            }
            else
            {
                // Arcane - All random
                randomPercent = UnityEngine.Random.Range(0, 100);
                if (randomPercent < 25)
                {
                    Strength++;
                }
                else if (randomPercent < 50)
                {
                    Agility++;
                }
                else if (randomPercent < 75)
                {
                    Mind++;
                }
                else
                {
                    Will++;
                }
            }
        }
    }


    public int GetMaxHp()
    {
        return (int)(((GetTotalStrength() * 5) + 100 * GetStrengthMultiplier()) * HPMultiplier);
    }

    public int GetTotalWill()
    {
        return (int)(Will * GetStrengthMultiplier());
    }

    public int GetTotalStrength()
    {
        return (int)(Strength * GetStrengthMultiplier());
    }

    public int GetTotalAgility()
    {
        return (int)(Agility * GetAgilityMultiplier());
    }

    public float GetStrengthMultiplier()
    {
        switch (Size)
        {
            case CreatureSize.Collosal:
                return 2.5f;
            case CreatureSize.VeryMassive:
                return 2.2f;
            case CreatureSize.Massive:
                return 2f;
            case CreatureSize.Giant:
                return 1.7f;
            case CreatureSize.Huge:
                return 1.6f;
            case CreatureSize.ExtremelyLarge:
                return 1.3f;
            case CreatureSize.VeryLarge:
                return 1.2f;
            case CreatureSize.Large:
                return 1.1f;
            case CreatureSize.Medium:
                return 1f;
            case CreatureSize.Small:
                return .7f;
            case CreatureSize.VerySmall:
                return .6f;
            case CreatureSize.SuperSmall:
                return .3f;
            case CreatureSize.Tiny:
                return .2f;
            default:
                return .1f;
        }
    }

    public float GetAgilityMultiplier()
    {
        switch (Size)
        {
            case CreatureSize.Collosal:
                return .2f;
            case CreatureSize.VeryMassive:
                return .3f;
            case CreatureSize.Massive:
                return .4f;
            case CreatureSize.Giant:
                return .5f;
            case CreatureSize.Huge:
                return .6f;
            case CreatureSize.ExtremelyLarge:
                return .7f;
            case CreatureSize.VeryLarge:
                return .8f;
            case CreatureSize.Large:
                return .9f;
            case CreatureSize.Medium:
                return 1f;
            case CreatureSize.Small:
                return 1.1f;
            case CreatureSize.VerySmall:
                return 1.2f;
            case CreatureSize.SuperSmall:
                return 1.3f;
            case CreatureSize.Tiny:
                return 1.4f;
            default:
                return 1.5f;
        }
    }

    public static float SizeToNumber(CreatureSize Size)
    {
        switch (Size)
        {
            case CreatureSize.Collosal:
                return 12f;
            case CreatureSize.VeryMassive:
                return 11f;
            case CreatureSize.Massive:
                return 10f;
            case CreatureSize.Giant:
                return 9f;
            case CreatureSize.Huge:
                return 8f;
            case CreatureSize.ExtremelyLarge:
                return 7f;
            case CreatureSize.VeryLarge:
                return 6f;
            case CreatureSize.Large:
                return 5f;
            case CreatureSize.Medium:
                return 4f;
            case CreatureSize.Small:
                return 3f;
            case CreatureSize.VerySmall:
                return 2f;
            case CreatureSize.SuperSmall:
                return 1f;
            case CreatureSize.Tiny:
                return 0f;
            default:
                return -1f;
        }
    }

    //public int SizeToNumber(CreatureSize Size)
    //{
    //    switch (Size)
    //    {
    //        case CreatureSize.Collosal:
    //            return 8;
    //        case CreatureSize.VeryMassive:
    //            return 7;
    //        case CreatureSize.Massive:
    //            return 6;
    //        case CreatureSize.Giant:
    //            return 5;
    //        case CreatureSize.Huge:
    //            return 4;
    //        case CreatureSize.ExtremelyLarge:
    //            return 3;
    //        case CreatureSize.VeryLarge:
    //            return 2;
    //        case CreatureSize.Large:
    //            return 1;
    //        case CreatureSize.Medium:
    //            return 0;
    //        case CreatureSize.Small:
    //            return -1;
    //        case CreatureSize.VerySmall:
    //            return -2;
    //        case CreatureSize.SuperSmall:
    //            return -3;
    //        case CreatureSize.Tiny:
    //            return -4;
    //        default:
    //            return -5;
    //    }
    //}

    public static int IntelligenceToNumber(CreatureIntelligence intelligence)
    {
        switch (intelligence)
        {
            case CreatureIntelligence.Genious:
                return 5;
            case CreatureIntelligence.Brilliant:
                return 4;
            case CreatureIntelligence.VerySmart:
                return 3;
            case CreatureIntelligence.Smart:
                return 2;
            case CreatureIntelligence.AboveAverage:
                return 1;
            case CreatureIntelligence.Average:
                return 0;
            case CreatureIntelligence.BellowAverage:
                return -1;
            case CreatureIntelligence.Dumb:
                return -2;
            case CreatureIntelligence.VeryDumb:
                return -3;
            default:
                return -4;
        }
    }

    /// <summary>
    /// Later I may need to remove armor in order to upgrade to a different armor type.
    /// </summary>
    /// <param name="creature"></param>
    public void RemoveArmor()
    {

    }

    public void ApplyArmor()
    {
        BaseCreature creature = GM.AllCreatures.Find(creature => creature.Name.Equals(Name));

        int armor = 0;
        int resistance = 0;
        int agility = 0;
        bool gotNaturalArmor = false;

        foreach (AttributeName attribute in CurrentAttributes)
        {
            if (attribute == AttributeName.Skin && !gotNaturalArmor)
            {
                //Debug.Log("Armor Type = " + attribute);
                armor = (int)((SizeToNumber(creature.Size) / 4) - 1);
                resistance = (int)((SizeToNumber(creature.Size) / 4) - 1);
                gotNaturalArmor = true;
            }
            if (attribute == AttributeName.ThickSkin && !gotNaturalArmor)
            {
                //Debug.Log("Armor Type = " + attribute);
                armor = (int)(SizeToNumber(creature.Size) * 1);
                resistance = (int)(SizeToNumber(creature.Size) / 4);
                agility = (int)(-SizeToNumber(creature.Size));
                gotNaturalArmor = true;
            }
            else if (attribute == AttributeName.SuperThickSkin && !gotNaturalArmor)
            {
                //Debug.Log("Armor Type = " + attribute);
                armor = (int)(SizeToNumber(creature.Size) * 1.5);
                resistance = (int)(SizeToNumber(creature.Size) / 3);
                agility = (int)(-SizeToNumber(creature.Size) * 1.5f);
            }
            else if (attribute == AttributeName.Feathers && !gotNaturalArmor)
            {
                //Debug.Log("Armor Type = " + attribute);
                armor = (int)(SizeToNumber(creature.Size) / 3f);
                resistance = (int)(SizeToNumber(creature.Size) * .75f);
                agility = 3;
            }
            else if (attribute == AttributeName.Fur && !gotNaturalArmor)
            {
                //Debug.Log("Armor Type = " + attribute);
                armor = (int)(SizeToNumber(creature.Size) * 1);
                resistance = (int)(SizeToNumber(creature.Size) * 1.5f);
            }
            else if (attribute == AttributeName.Scales && !gotNaturalArmor)
            {
                //Debug.Log("Armor Type = " + attribute);
                armor = (int)(SizeToNumber(creature.Size) * 1.5);
                resistance = (int)(SizeToNumber(creature.Size) / 3);
                agility = (int)(-SizeToNumber(creature.Size) * 1.5f);
            }
            else if (attribute == AttributeName.Chitin && !gotNaturalArmor)
            {
                //Debug.Log("Armor Type = " + attribute);
                armor = (int)(SizeToNumber(creature.Size) * 1.5);
                resistance = (int)(SizeToNumber(creature.Size) / 3);
                agility = (int)(-SizeToNumber(creature.Size) * 1.5f);
            }
        }

        //Debug.Log("armor = " + armor);
        //Debug.Log("resistance = " + resistance);
        //Debug.Log("agility = " + agility);

        if (armor > 0)
        {
            Armor += armor;
        }
        else
        {
            Armor += 0;
        }
        //Debug.Log("Armor = " + Armor);

        if (resistance > 0)
        {
            General_Resistance += resistance;
        }
        else
        {
            General_Resistance += 0;
        }
        //Debug.Log("General_Resistance = " + General_Resistance);

        if (agility > 0)
        {
            Agility += agility;
        }
        else
        {
            Agility += 0;
        }
        //Debug.Log("Agility = " + Agility);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetInitialPowerups()
    {
        BaseCreature ThisCreature = GM.AllCreatures.Find(creature => creature.Name.Equals(this.Name));
        foreach (PowerUps powerUp in ThisCreature.PowerUps)
        {
            if (powerUp.TrackedStat == PowerUpStat.None)
            {
                ApplyReward(powerUp.Reward);
            }
        }
    }

    //public void GetPowerUps() // Get BaseCreature from GameController and check if any unrecieved stat goals are now met.
    //{
    //    BaseCreature ThisCreature = GM.AllCreatures.Find(creature => creature.Name.Equals(this.Name));
    //    foreach(PowerUps powerUp in ThisCreature.PowerUps)
    //    {
    //        TrackedStat statToTrack = StatsToTrack.Find(stat => stat.StatToTrack.Equals(powerUp.TrackedStat));
    //        if (statToTrack != null && (powerUp.TrackedStat.Equals(PowerUpStat.None) || (statToTrack.CurrentAmount >= powerUp.StatGoal))) // Will eventually need a way to stop the creature recieving a power up twice.
    //        {
    //            ApplyReward(powerUp.Reward);
    //            RecievedPowerUps.Add(powerUp);
    //        }
    //    }
    //}

    public void ApplyReward(Reward reward)
    {
        switch (reward.Type)
        {
            case RewardType.Stat:
                switch (reward.RewardStat)
                {
                    case RewardStat.Strength:
                        Strength += (int)reward.Amount;
                        break;
                    case RewardStat.Agility:
                        Agility += (int)reward.Amount;
                        break;
                    case RewardStat.Armour:
                        Armor += (int)reward.Amount;
                        break;
                    case RewardStat.General_Resistance:
                        General_Resistance += (int)reward.Amount;
                        break;
                    case RewardStat.Fire_Resistance:
                        Fire_Resistance += (int)reward.Amount;
                        break;
                    case RewardStat.Water_Resistance:
                        Water_Resistance += (int)reward.Amount;
                        break;
                    case RewardStat.Poison_Resistance:
                        Poison_Resistance += (int)reward.Amount;
                        break;
                    case RewardStat.Electric_Resistance:
                        Electric_Resistance += (int)reward.Amount;
                        break;
                    case RewardStat.Death_Resistance:
                        Death_Resistance += (int)reward.Amount;
                        break;
                    case RewardStat.HP_Multiplier: // HP_Multiplier
                        HPMultiplier = this.HPMultiplier * reward.Amount;
                        break;
                    default:
                        break;
                }
                break;
            case RewardType.Attribute:
                Attribute TempAttribute = new Attribute(reward.AttributeName);
                if (!CurrentAttributes.Contains(reward.AttributeName)) 
                {
                    if (TempAttribute.AttributeReward != null)
                    {
                        ApplyReward(TempAttribute.AttributeReward);
                    }
                    CurrentAttributes.Add(TempAttribute.Name);
                }
                break;
            case RewardType.Ability:
                Ability ability = AllAbilities.GetAbility(reward.AbilityName);
                if (ability.Upgrades)
                {
                    KnownAbilities.Remove(KnownAbilities.Find(knownAbility => knownAbility.AbilityName == ability.UpgradedAbility));
                }
                KnownAbilities.Add(new AbilityData(ability, AllAbilities.GetAbility(reward.AbilityName).Speed));
                break;
            case RewardType.Lvl:
                CurrentLvl++;
                // Check attributes for additional increases or decreases. E.G Intellegent, Dumb, Fast, Slow...
                AddBaseStats(Rating, GM.AllCreatures.Find(creature => creature.Name.Equals(Name)).CreatureTypePercents, 1);
                break;
            case RewardType.Evolution:

                break;
            default:
                break;
        }
    }

    public void UpdatePowerupStats(Dictionary<PowerUpStat, int> stats)
    {
        for (int i = 0; i < stats.Count; i++)
        {
            PowerUpStat key = stats.Keys.ElementAt(i);
            if (TrackedStats.ContainsKey(key))
            {
                TrackedStats[key] += stats[key];
                //Debug.Log("Increasing Tracked Stat By: " + key + " gets " + stats[key]);
            }
            else
            {
                TrackedStats.Add(key, stats[key]);
                //Debug.Log("Adding New Stat To Track: " + key + " gets " + stats[key]);
            }
        }
    }

    public void ApplyPowerups()
    {
        BaseCreature baseCreature = GM.AllCreatures.Find(creature => creature.Name.Equals(Name));

        //Debug.Log("ApplyPowerups");
        //Debug.Log("TrackedStats[PowerUpStat.XP] = " + TrackedStats[PowerUpStat.XP]);
        //Debug.Log("InitializeCreatures.XpToLevel(TrackedStats[PowerUpStat.XP]) = " + InitializeCreatures.XpToLevel(TrackedStats[PowerUpStat.XP]));
        //Debug.Log("CurrentLvl = " + CurrentLvl);

        // Check for Level Ups
        int levelDiff = InitializeCreatures.XpToLevel(TrackedStats[PowerUpStat.XP]) - CurrentLvl;
        //Debug.Log("levelDiff = " + levelDiff);
        if (levelDiff > 0)
        {
            int previousMaxHp = GetMaxHp();
            for (int i = 0; i < levelDiff; i++)
            {
                //Debug.Log(Name + " gets a level up!");
                ApplyReward(new Reward(RewardType.Lvl, "Level Up"));
                //Debug.Log(Name + "'s new level is " + CurrentLvl);
            }
            CurrentHP += GetMaxHp() - previousMaxHp;
        }

        // Add creature specific powerups
        foreach (PowerUps powerUp in baseCreature.PowerUps)
        {
            if (powerUp.Reward != null && powerUp.TrackedStat != PowerUpStat.None && powerUp.Reward.Type != RewardType.Stat && powerUp.Reward.Type != RewardType.Lvl)
            {
                if (TrackedStats[powerUp.TrackedStat] >= powerUp.StatGoal)
                    ApplyReward(powerUp.Reward);
            }
        }


        // Add attribute based rewards

        // I think later I will just have this list stored on the initialized creature itself instead of a list of ability datas.
        List<AbilityName> abilityNames = new List<AbilityName>();
        foreach(AbilityData ability in KnownAbilities)
        {
            abilityNames.Add(ability.AbilityName);
        }

        // Add Attribute Based Bonuses - This may need to be made more efficient later
        if (HasAttribute(AttributeName.Claws))
        {
            if (!abilityNames.Contains(AbilityName.Scratch_1))
                ApplyReward(new Reward(AbilityName.Scratch_1));

            if (HasAttribute(AttributeName.SharpClaws))
            {
                if (!abilityNames.Contains(AbilityName.Scratch_2))
                    ApplyReward(new Reward(AbilityName.Scratch_2));
            }

            if (Mind > 20)
            {
                if (!abilityNames.Contains(AbilityName.Swipe_2))
                    ApplyReward(new Reward(AbilityName.Swipe_2));
            }
        }

        if (HasAttribute(AttributeName.Talons))
        {
            if (!abilityNames.Contains(AbilityName.Scratch_1))
                ApplyReward(new Reward(AbilityName.Scratch_1));
        }

        if (HasAttribute(AttributeName.SharpTeath))
        {
            if (!abilityNames.Contains(AbilityName.Bite_1))
                ApplyReward(new Reward(AbilityName.Bite_1));

            if (HasAttribute(AttributeName.Pestilent) && Mind > 15)
            {
                if (!abilityNames.Contains(AbilityName.VileBite_2))
                    ApplyReward(new Reward(AbilityName.VileBite_2));
            }
        }

        if (HasAttribute(AttributeName.Beak))
        {
            if (!abilityNames.Contains(AbilityName.Peck_1))
                ApplyReward(new Reward(AbilityName.Peck_1));
        }

        if (HasAttribute(AttributeName.StrongJaws))
        {
            if (!abilityNames.Contains(AbilityName.Crunch_1))
                ApplyReward(new Reward(AbilityName.Crunch_1));
        }

        if (HasAttribute(AttributeName.PowerfulJaws))
        {
            if (Strength > 20)
                if (!abilityNames.Contains(AbilityName.Crunch_2))
                    ApplyReward(new Reward(AbilityName.Crunch_2));
        }

        if (HasAttribute(AttributeName.LongTongue))
        {
            if (!abilityNames.Contains(AbilityName.TongueSmack_1))
                ApplyReward(new Reward(AbilityName.TongueSmack_1));

            if (Strength > 20)
                if (!abilityNames.Contains(AbilityName.TongueBash_2))
                    ApplyReward(new Reward(AbilityName.TongueBash_2));

            if (Mind > 15)
            {
                if (!abilityNames.Contains(AbilityName.TongueHarpoon_2))
                    ApplyReward(new Reward(AbilityName.TongueHarpoon_2));

                if (Strength > 15 && Agility > 15)
                {
                    if (!abilityNames.Contains(AbilityName.TongueLash_2))
                        ApplyReward(new Reward(AbilityName.TongueLash_2));
                }
            }
        }

        if (HasAttribute(AttributeName.HardHead) && HasAttribute(AttributeName.PowerfulLegs))
        {
            if (!abilityNames.Contains(AbilityName.HeadButt_1))
                ApplyReward(new Reward(AbilityName.HeadButt_1));
            if (HasAttribute(AttributeName.SharpHorned))
            {
                if (!abilityNames.Contains(AbilityName.HornedHeadButt_2))
                    ApplyReward(new Reward(AbilityName.HornedHeadButt_2));
            }
        }

            if (HasAttribute(AttributeName.PoisonStinger))
        {
            if (!abilityNames.Contains(AbilityName.Sting_1))
                ApplyReward(new Reward(AbilityName.Sting_1));

            if (HasAttribute(AttributeName.PowerfulStinger))
            {
                if (!abilityNames.Contains(AbilityName.Sting_2))
                    ApplyReward(new Reward(AbilityName.Sting_2));
            }
        }
    }

    public bool HasAttribute(AttributeName attribute)
    {
        foreach(AttributeName attributeName in CurrentAttributes)
        {
            if (new Attribute(attributeName).IsOrHasParent(attribute))
            {
                return true;
            }
        }
        return false;
    }

}
