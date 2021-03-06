using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Ideas
 * 
 * Maybe have power level be determined by all the other aspects like size, attributes, abilities, and stats rather then picking it manually
 * Hit points should be 10 per size, 5 per striength.
 * 
 * Stats and their bonuses
 * Strength - Increases HP and Phisical damage, increases blocking power, improves chance to break free from phisical control effects
 * Agility - Increases dodge, block and critical hit chances, increases initiative, often reduces cool downs?
 * Will - Increases elemental damage, resistance to life and death damage, chance to break free of all control effects
 * Mind - Increases focus, choose better targets or the correct target when player controlled, needed for learning many abilities and attributes that can provide other bonuses, increases initiaitive, increases resistance to mind attacks and power of mind attacks
 * Size - Increases HP, chance to break free of phisical control effects, impact damage, reduces dodge chance
 * 
 */

/// <summary>
/// IMPORTANT!!!! Only add attribute and ability powerups to base creatures.
/// </summary>
public static class InitializeCreatures
{
    // In the future much of this may be handled by SQL

    public static List<BaseCreature> AllCreatureList;

    public static List<BaseCreature> GetInitializedCreatures()
    {
        //public BaseCreature(string MyName, List<CreatureType> MyTypes, CreatureSize MySize, CreatureIntelligence MyIntelligence, List<Abilities> MyStartingAbilities,
        //    List<PowerUps> MyPowerUps, int MyMaxLvl, Rating MyRating, int MyPowerLevel)

        //List<PowerUps> GlobalPowerUps = new List<PowerUps>();
        //for (int i = 2; i < 101; i++)
        //{
        //    //Debug.Log("i = " + i);
        //    //Debug.Log("i = " + XpToLevelRequired(LevelToXpRequired(i)));
        //    GlobalPowerUps.Add(new PowerUps(PowerUpStat.XP, LevelToXpRequired(i), new Reward(RewardType.Lvl, "Level Up")));
        //    //Debug.Log("XP Required for Level " + (i - 1 + " -> " + i + " = " + (int)Mathf.Pow((float)i, 2f) * 10) + "; Difference is " + ((int)(Mathf.Pow((float)i, 2f) * 10) - (int)Mathf.Pow((float)i - 1, 2f) * 10));
        //}
        //// XP required for each level
        //// 1 -> 2 = 40;
        //// 2 -> 3 = 90; Difference is 50
        //// 0 -> 1 = 160; Difference is 70
        //// 0 -> 1 = 250; Difference is 90
        //// 0 -> 1 = 360; Difference is 110
        //// 0 -> 1 = 490; Difference is 130
        //// 0 -> 1 = 640; Difference is 150
        //// 0 -> 1 = 810; Difference is 170
        //// 0 -> 1 = 1000; Difference is 190
        //// 0 -> 1 = 1210; Difference is 210
        //// 0 -> 1 = 1440; Difference is 230

        AllCreatureList = new List<BaseCreature>();
        List<CreatureType> MyTypes = new List<CreatureType>();
        List<PowerUps> MyPowerUps = new List<PowerUps>();
        string CreatureDescription = "";
        CreatureTypePercents creatureTypePercents;

        // Create Rat
        CreatureDescription = "Narsty little roadents, though little is relative in this case. They are as large as a dog and much harder to kill. Prone to spreading deseases these creatures are generally hated although some races like Rat Men and goblins favor them as pets.";
        MyTypes = new List<CreatureType>();
        MyPowerUps = new List<PowerUps>();
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Fur)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Claws)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.SharpTeath)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.BalancingTail)));

        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PoisonResistant)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.VeryTough)));

        MyPowerUps.Add(new PowerUps(PowerUpStat.XP, 40, new Reward(AttributeName.Pestilent))); // Later I want this to be based off of poison damage taken I think

        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Intellegent)));
        // Types: fire, water, earth, air, life, death, blood, arcane
        creatureTypePercents = new CreatureTypePercents(0, 0, 15, 0, 25, 0, 60, 0);
        AllCreatureList.Add(new BaseCreature("GiantRat", "Rats\\GiantRat\\GiantRat", "Giant Rat", CreatureDescription, MyTypes, CreatureSize.SuperSmall, MyPowerUps, 10, Rating.Average, creatureTypePercents));

        // Pestarat
        CreatureDescription = "Narsty little roadents, though little is relative in this case. They are as large as a dog and much harder to kill. Prone to spreading deseases these creatures are generally hated although some races like Rat Men and goblins favor them as pets.";
        MyTypes = new List<CreatureType>();
        MyPowerUps = new List<PowerUps>();
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Fur)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Claws)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.SharpTeath)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.BalancingTail)));

        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PoisonResistant)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.VeryTough)));

        MyPowerUps.Add(new PowerUps(PowerUpStat.XP, 40, new Reward(AttributeName.Pestilent))); // Later I want this to be based off of poison damage taken I think

        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Intellegent)));
        // Types: fire, water, earth, air, life, death, blood, arcane
        creatureTypePercents = new CreatureTypePercents(0, 0, 15, 0, 25, 0, 60, 0);
        AllCreatureList.Add(new BaseCreature("Pestarat", "Rats\\Pestarat\\Pestarat", "Pestarat", CreatureDescription, MyTypes, CreatureSize.SuperSmall, MyPowerUps, 10, Rating.AboveAverage, creatureTypePercents));

        // Create Grey Wolf
        CreatureDescription = "These are among the smallest and least threatening of the wolves in Whid Whi but in large groups they are still very dangerous.";
        MyTypes = new List<CreatureType>();
        MyPowerUps = new List<PowerUps>();
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Fur)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Claws)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.SharpTeath)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.BalancingTail)));

        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Fast)));

        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Intellegent)));
        // Types: fire, water, earth, air, life, death, blood, arcane
        creatureTypePercents = new CreatureTypePercents(0, 0, 15, 0, 25, 0, 60, 0);
        AllCreatureList.Add(new BaseCreature("GreyWolf", "GreyWolf\\GreyWolf", "Grey Wolf", CreatureDescription, MyTypes, CreatureSize.VerySmall, MyPowerUps, 10, Rating.Average, creatureTypePercents));

        // Create White Wolf
        CreatureDescription = "Much larger then their grey wolf companions white wolves are vicious and deadly.";
        MyTypes = new List<CreatureType>();
        MyPowerUps = new List<PowerUps>();
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Fur)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.SharpClaws)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.SharpTeath)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.BalancingTail)));

        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Fast)));

        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Intellegent)));
        // Types: fire, water, earth, air, life, death, blood, arcane
        creatureTypePercents = new CreatureTypePercents(0, 0, 15, 0, 25, 0, 60, 0);
        AllCreatureList.Add(new BaseCreature("WhiteWolf", "WhiteWolf\\WhiteWolf", "White Wolf", CreatureDescription, MyTypes, CreatureSize.Small, MyPowerUps, 10, Rating.AboveAverage, creatureTypePercents));

        // Create Wasp
        CreatureDescription = "Definately the worst sort of insect. About the size of a rat these huge wasps sting will certainly ruin your day.";
        MyTypes = new List<CreatureType>();
        MyPowerUps = new List<PowerUps>();
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Chitin)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PowerfulStinger)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.InsectoidWings)));

        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Intellegent)));
        // Types: fire, water, earth, air, life, death, blood, arcane
        creatureTypePercents = new CreatureTypePercents(0, 0, 20, 30, 15, 0, 35, 0);
        AllCreatureList.Add(new BaseCreature("Wasp", "Wasp\\Wasp", "Wasp", CreatureDescription, MyTypes, CreatureSize.VeryTiny, MyPowerUps, 10, Rating.Average, creatureTypePercents));

        // Create Bear
        CreatureDescription = "This large fury beast may look rolly polly and haggable but it will rip your face off pretty quick if you make it angry.";
        MyTypes = new List<CreatureType>();
        MyPowerUps = new List<PowerUps>();
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Fur)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.StrongClaws)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.SharpTeath)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.StrongJaws)));

        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Intellegent)));
        // Types: fire, water, earth, air, life, death, blood, arcane
        creatureTypePercents = new CreatureTypePercents(0, 0, 0, 0, 25, 0, 75, 0);
        AllCreatureList.Add(new BaseCreature("Bear", "\\Bear", "Bear", CreatureDescription, MyTypes, CreatureSize.VeryLarge, MyPowerUps, 10, Rating.Average, creatureTypePercents));

        // Create Moo Beast
        CreatureDescription = "This slow and simple beast is content to spend all of it's days eating grass in a field.";
        MyTypes = new List<CreatureType>();
        MyPowerUps = new List<PowerUps>();
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.ThickSkin)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PowerfulLegs)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.HardHead)));

        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Tough)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Slow)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Dumb)));

        // Types: fire, water, earth, air, life, death, blood, arcane
        creatureTypePercents = new CreatureTypePercents(0, 0, 0, 0, 20, 0, 80, 0);
        AllCreatureList.Add(new BaseCreature("MooBeast", "MooBeast\\MooBeast", "Moo Beast", CreatureDescription, MyTypes, CreatureSize.VeryLarge, MyPowerUps, 10, Rating.Average, creatureTypePercents));

        //// Create Eatern Moo Beast (Yack
        //CreatureDescription = "A silly but loveable flightless bird.";
        //MyTypes = new List<CreatureType>();
        //MyPowerUps = new List<PowerUps>();
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Fur)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.StrongLegs)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PowerfulLegs)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.HardHead)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Horned)));

        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Tough)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Slow)));

        //// Types: fire, water, earth, air, life, death, blood, arcane
        //creatureTypePercents = new CreatureTypePercents(0, 0, 0, 0, 20, 0, 80, 0);
        //AllCreatureList.Add(new BaseCreature("EaternMooBeast", "E M Beast", CreatureDescription, MyTypes, CreatureSize.VeryLarge, MyPowerUps, 10, Rating.Average, creatureTypePercents));

        // Create Greater Moo Beast
        CreatureDescription = "It's bigger, smarter, faster and much angrier looking then the Moo Beast.";
        MyTypes = new List<CreatureType>();
        MyPowerUps = new List<PowerUps>();
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.ThickSkin)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.StrongLegs)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PowerfulLegs)));
        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.SharpHorned)));

        MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Tough)));

        // Types: fire, water, earth, air, life, death, blood, arcane
        creatureTypePercents = new CreatureTypePercents(0, 0, 0, 0, 20, 0, 80, 0);
        AllCreatureList.Add(new BaseCreature("GreaterMooBeast", "GreaterMooBeast\\GreaterMooBeast", "G Moo Beast", CreatureDescription, MyTypes, CreatureSize.ExtremelyLarge, MyPowerUps, 10, Rating.AboveAverage, creatureTypePercents));

        //// Create Rooster
        //CreatureDescription = "A silly but loveable flightless bird.";
        //MyTypes = new List<CreatureType>();
        //MyPowerUps = new List<PowerUps>();
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Feathers)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Talons)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.Beak)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.BalancingTail)));

        //// Types: fire, water, earth, air, life, death, blood, arcane
        //creatureTypePercents = new CreatureTypePercents(0, 0, 0, 20, 20, 0, 60, 0);
        //AllCreatureList.Add(new BaseCreature("Cocka Doodle Devil", "Cocka Devil", CreatureDescription, MyTypes, CreatureSize.Medium, MyPowerUps, 10, Rating.Average, creatureTypePercents));

        //// Create Frog
        //CreatureDescription = "A bug-eyed amphibion with a rediculously large mouth. It's funny looking for sure.";
        //MyTypes = new List<CreatureType>();
        //MyPowerUps = new List<PowerUps>();
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.ThickSkin)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.StrongJaws)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.HardHead)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PowerfulLegs)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.LongTongue)));

        //// Types: fire, water, earth, air, life, death, blood, arcane
        //creatureTypePercents = new CreatureTypePercents(0, 20, 20, 0, 15, 0, 45, 0);
        //AllCreatureList.Add(new BaseCreature("Giant Frog", "Giant Frog", CreatureDescription, MyTypes, CreatureSize.Medium, MyPowerUps, 10, Rating.Average, creatureTypePercents));

        //// Create KingToad
        //CreatureDescription = "A true monstor of a toad and a king of tiny amphibions and other small vermin.";
        //MyTypes = new List<CreatureType>();
        //MyPowerUps = new List<PowerUps>();
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.SuperThickSkin)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PowerfulJaws)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.HardHead)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.PowerfulLegs)));
        //MyPowerUps.Add(new PowerUps(PowerUpStat.None, 0, new Reward(AttributeName.LongTongue)));
        //// Types: fire, water, earth, air, life, death, blood, arcane
        //creatureTypePercents = new CreatureTypePercents(0, 20, 20, 0, 15, 0, 45, 0);
        //AllCreatureList.Add(new BaseCreature("King Toad", "King Toad", CreatureDescription, MyTypes, CreatureSize.Medium, MyPowerUps, 25, Rating.Powerful, creatureTypePercents));

        return AllCreatureList;
    }

    public static int LevelToXpRequired(int level)
    {
        return (int)Mathf.Pow((float)level, 2f) * 10;
    }
    public static int XpToLevel(int xp)
    {
        return (int)Mathf.Pow((float)xp / 10, .5f);
    }
}
