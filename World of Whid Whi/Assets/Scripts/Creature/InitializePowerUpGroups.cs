using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InitializePowerUpGroups
{
    public static Dictionary<string, PowerUpGroup> AllPowerUps;

    //public static PowerUpGroup GetPowerUpGroup(string name)
    //{
    //    PowerUpGroup powerUpGroup = null;
    //    switch (name)
    //    {
    //        case "GiantRatPowerups":
    //            powerUpGroup = new GiantRatPowerups();
    //            break;
    //        case "Claws":
    //            powerUpGroup = new Claws();
    //            break;
    //        default:
    //            break;
    //    }
    //    return powerUpGroup;
    //}
    public static Dictionary<string, PowerUpGroup> SetAllPowerUps()
    {
        AllPowerUps = new Dictionary<string, PowerUpGroup>();


        ////////////////// Creature power up groups //////////////////

        // need to add regular level ups of reward type level!!!
        List<PowerUp> powerups = new List<PowerUp>();
        List<PowerUpCondition> conditions;
        Reward reward;

        List<PowerUp> levelup_powerups = new List<PowerUp>();
        // later we may want to replace this with code built int applying power ups but I don't know for sure.
        for (int i = 1; i < 10; i++)
        {
            conditions = new List<PowerUpCondition>();
            conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(i)));
            reward = new Reward(RewardType.Lvl, "Level Up!");
            levelup_powerups.Add(new PowerUp(conditions, reward));
        }

        // Giant Rat

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(0)));
        reward = new Reward(RewardStat.HP_Multiplier, 0.25f);
        powerups.Add(new PowerUp(conditions, reward));

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(3)));
        reward = new Reward(RewardStat.Poison_Resistance, 1f);
        powerups.Add(new PowerUp(conditions, reward));

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(5)));
        reward = new Reward(RewardStat.HP_Multiplier, 0.25f);
        powerups.Add(new PowerUp(conditions, reward));

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(7)));
        reward = new Reward(RewardStat.General_Resistance, 1f);
        powerups.Add(new PowerUp(conditions, reward));

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(9)));
        reward = new Reward(RewardStat.Poison_Resistance, 1f);
        powerups.Add(new PowerUp(conditions, reward));

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(10)));
        reward = new Reward(RewardStat.HP_Multiplier, 0.25f);
        powerups.Add(new PowerUp(conditions, reward));

        AllPowerUps.Add("Giant Rat", new PowerUpGroup("Giant Rat", powerups.Concat(levelup_powerups).ToList()));

        // Create Grey Wolf
        // May need to create a clone of the list for creatures without any unique powerups
        AllPowerUps.Add("Grey Wolf", new PowerUpGroup("Grey Wolf", levelup_powerups));

        // Create White Wolf

        AllPowerUps.Add("White Wolf", new PowerUpGroup("White Wolf", levelup_powerups));

        // Create Wasp

        AllPowerUps.Add("Wasp", new PowerUpGroup("Wasp", levelup_powerups));

        // Create Bear

        AllPowerUps.Add("Bear", new PowerUpGroup("Bear", levelup_powerups));

        // Create Moo Beast

        AllPowerUps.Add("Moo Beast", new PowerUpGroup("Moo Beast", levelup_powerups));

        // Create Greater Moo Beast

        AllPowerUps.Add("Greater Moo Beast", new PowerUpGroup("Greater Moo Beast", levelup_powerups));


        ////////////////// Anatomy Power Up Groups //////////////////

        // Claws

        powerups = new List<PowerUp>();
        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(0)));
        reward = new Reward(AbilityName.Scratch_1);
        powerups.Add(new PowerUp(conditions, reward));

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.MIND, 15)); // need to check these requirements
        conditions.Add(new PowerUpCondition(PowerUpStat.AGI, 20)); // need to check these requirements
        reward = new Reward(AbilityName.Shred_4); // look into a better way to keep track of ability levels (there has to be a way to seriolize a simple ability class
        powerups.Add(new PowerUp(conditions, reward));

        AllPowerUps.Add("Claws", new PowerUpGroup("Claws", powerups));

        // Sharp Claws

        powerups = new List<PowerUp>();
        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(0)));
        reward = new Reward(AbilityName.Scratch_2);
        powerups.Add(new PowerUp(conditions, reward));

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.MIND, 15)); // need to check these requirements
        conditions.Add(new PowerUpCondition(PowerUpStat.AGI, 18)); // need to check these requirements
        reward = new Reward(AbilityName.Shred_4); // look into a better way to keep track of ability levels (there has to be a way to seriolize a simple ability class
        powerups.Add(new PowerUp(conditions, reward));

        AllPowerUps.Add("Sharp Claws", new PowerUpGroup("Sharp Claws", powerups));

        // Stinger

        powerups = new List<PowerUp>();
        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(0)));
        reward = new Reward(AbilityName.Sting_1);
        powerups.Add(new PowerUp(conditions, reward));

        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.MIND, 18)); // need to check these requirements
        reward = new Reward(AbilityName.Sting_2); // look into a better way to keep track of ability levels (there has to be a way to seriolize a simple ability class
        powerups.Add(new PowerUp(conditions, reward));

        AllPowerUps.Add("Stinger", new PowerUpGroup("Stinger", powerups));

        // Hard Head

        powerups = new List<PowerUp>();
        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(0)));
        reward = new Reward(AbilityName.HeadButt_1);
        powerups.Add(new PowerUp(conditions, reward));

        AllPowerUps.Add("Hard Head", new PowerUpGroup("Hard Head", powerups));

        // Horns

        powerups = new List<PowerUp>();
        conditions = new List<PowerUpCondition>();
        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(0)));
        reward = new Reward(AbilityName.HornedHeadButt_2); // maybe make a more generic scure that can be used for other things too
        powerups.Add(new PowerUp(conditions, reward));

        AllPowerUps.Add("Horns", new PowerUpGroup("Horns", powerups));



        ////////////////// Natural Armor //////////////////

        // I think I need to instead handle natural armor bonuses based on size with it's own special logic like level ups.

        //// Fur

        //powerups = new List<PowerUp>();
        //conditions = new List<PowerUpCondition>();
        //conditions.Add(new PowerUpCondition(PowerUpStat.SIZE, InitializedCreature.SizeToNumber(CreatureSize.Small)));
        //reward = new Reward(RewardStat.Armour, 1); // should be based on size
        //powerups.Add(new PowerUp(conditions, reward));

        //powerups = new List<PowerUp>();
        //conditions = new List<PowerUpCondition>();
        //conditions.Add(new PowerUpCondition(PowerUpStat.SIZE, 1));
        //reward = new Reward(RewardStat.General_Resistance, 1); // should be based on size
        //powerups.Add(new PowerUp(conditions, reward));

        //powerups = new List<PowerUp>();
        //conditions = new List<PowerUpCondition>();
        //conditions.Add(new PowerUpCondition(PowerUpStat.SIZE, 2));
        //reward = new Reward(RewardStat.Armour, 2); // should be based on size
        //powerups.Add(new PowerUp(conditions, reward));

        //powerups = new List<PowerUp>();
        //conditions = new List<PowerUpCondition>();
        //conditions.Add(new PowerUpCondition(PowerUpStat.SIZE, 2));
        //reward = new Reward(RewardStat.General_Resistance, 2); // should be based on size
        //powerups.Add(new PowerUp(conditions, reward));

        //powerups = new List<PowerUp>();
        //conditions = new List<PowerUpCondition>();
        //conditions.Add(new PowerUpCondition(PowerUpStat.SIZE, 3));
        //reward = new Reward(RewardStat.Armour, 3); // should be based on size
        //powerups.Add(new PowerUp(conditions, reward));

        //powerups = new List<PowerUp>();
        //conditions = new List<PowerUpCondition>();
        //conditions.Add(new PowerUpCondition(PowerUpStat.SIZE, 3));
        //reward = new Reward(RewardStat.General_Resistance, 3); // should be based on size
        //powerups.Add(new PowerUp(conditions, reward));

        //AllPowerUps.Add("Fur", new PowerUpGroup("Fur", powerups));


        return AllPowerUps;
    }
}



//public class GiantRatPowerups : PowerUpGroup
//{
//    public GiantRatPowerups()
//    {
//        // need to add regular level ups of reward type level!!!
//        description = "Giant Rat";
//        powerups = new List<PowerUp>();
//        List<PowerUpCondition> conditions = new List<PowerUpCondition>();

//        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(0)));
//        Reward reward = new Reward(RewardStat.HP_Multiplier, 1.25f);
//        powerups.Add(new PowerUp(conditions, reward));

//        conditions = new List<PowerUpCondition>();
//        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(3)));
//        reward = new Reward(RewardStat.Poison_Resistance, 1f);
//        powerups.Add(new PowerUp(conditions, reward));

//        conditions = new List<PowerUpCondition>();
//        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(5)));
//        reward = new Reward(RewardStat.HP_Multiplier, 1.25f);
//        powerups.Add(new PowerUp(conditions, reward));

//        conditions = new List<PowerUpCondition>();
//        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(7)));
//        reward = new Reward(RewardStat.General_Resistance, 1f);
//        powerups.Add(new PowerUp(conditions, reward));

//        conditions = new List<PowerUpCondition>();
//        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(9)));
//        reward = new Reward(RewardStat.Poison_Resistance, 1f);
//        powerups.Add(new PowerUp(conditions, reward));

//        conditions = new List<PowerUpCondition>();
//        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(10)));
//        reward = new Reward(RewardStat.HP_Multiplier, 1.25f);
//        powerups.Add(new PowerUp(conditions, reward));
//    }
//}

//public class Claws : PowerUpGroup
//{
//    public Claws()
//    {
//        description = "Claws";
//        powerups = new List<PowerUp>();
//        List<PowerUpCondition> conditions = new List<PowerUpCondition>();

//        conditions.Add(new PowerUpCondition(PowerUpStat.XP, InitializeCreatures.LevelToXpRequired(0)));
//        Reward reward = new Reward(AbilityName.Scratch_1);
//        powerups.Add(new PowerUp(conditions, reward));

//        conditions = new List<PowerUpCondition>();
//        conditions.Add(new PowerUpCondition(PowerUpStat.STR, 10)); // need to check these requirements
//        conditions.Add(new PowerUpCondition(PowerUpStat.AGI, 10)); // need to check these requirements
//        reward = new Reward(AbilityName.Scratch_2); // look into a better way to keep track of ability levels (there has to be a way to seriolize a simple ability class
//        powerups.Add(new PowerUp(conditions, reward));

//        conditions = new List<PowerUpCondition>();
//        conditions.Add(new PowerUpCondition(PowerUpStat.AGI, 20)); // need to check these requirements
//        reward = new Reward(AbilityName.Shred_4); // look into a better way to keep track of ability levels (there has to be a way to seriolize a simple ability class
//        powerups.Add(new PowerUp(conditions, reward));

//        // add claw based powerups here
//    }
//}