using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCreature
{
    /* Base Creature
     * 
     * This class contains all the standard information needed to initialize a specific creature or to level it up with power ups.
     * In the future if we have players only knowing partial information about the creatures they are encountering then we will need
     * to have the master set of base creatures known by the sever program and then each player will have their own subset of known
     * creatures that they will slowly add to as they play and discover new creatures.
     * 
     * TODO: Need to add Armour and Resistance!
     */

    public string Name;
    public string Path;
    public string ShortName; // used for displays like the battle icons where space is limited.
    public string Description;
    public List<CreatureType> Types;
    public CreatureSize Size;
    public List<PowerUpGroup> PowerUpGroups;
    public Rating Rating;
    public CreatureTypePercents CreatureTypePercents;
    // Creatures Lvl goes up as they get xp. Rating is often closely related to rarity and defines how strong the creature will be within a power level.
    //     Power level is what class of striength a creature falls with in. For example a rat and a cow may both be common rating but the cow will have a higher power level.
    //     Finally the cow and the rat each havae their own level based on how much xp that perticular creature has earned.
    // needs a way to track other factors like uses of attacks to know if new moves can be learned.
    // needs a way to store all the above data in a database and quickly level up a random creature to an appropriate level. Speaking of which do zones have lvl ranges?

    public BaseCreature(string MyName, string MyPath, string MyShortName, string MyDescription, List<CreatureType> MyTypes, CreatureSize MySize, 
        List<PowerUpGroup> MyPowerUpGroups, int MyMaxLvl, Rating MyRating, CreatureTypePercents creatureTypePercents)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "BaseCreature constructor");
        Name = MyName;
        Path = MyPath;
        ShortName = MyShortName;
        Description = MyDescription;
        Types = MyTypes;
        Size = MySize;
        PowerUpGroups = MyPowerUpGroups;
        Rating = MyRating;
        CreatureTypePercents = creatureTypePercents;
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

public enum DamageType
{
    Phisical, // Blocked by Armour
    Impact, // Cannot be blocked, countered by high HP
    Mind, // Typically control effects, countered by INT generally but sometimes Will
    Fire,
    Water,
    Poison,
    Electric,
    Ice,
    Mental,
    Death
}

public enum CreatureSize
{
    VeryTiny, // -5 Base STR, Base AGI 5, Size of a large rat or pigeon 0 - 2 lb
    Tiny, // -4 Base STR, Base AGI 4, Size of a ??? 2 - 8 lb
    SuperSmall, // -3 Base STR, Base AGI 3, Size of a fox or eagle 8 - 20 lb
    VerySmall, // -2 Base STR, Base AGI 2, Size of a ??? 20 - 40 lb
    Small, // -1 Base STR, Base AGI 1, Size of a Wolf 40 - 120 lb
    Medium, // 0 Base STR, Base AGI 0, Size of a human 120 - 330 lb
    Large, // 1 Base STR, Base AGI -1, Size of Lion 330 - 700 lb
    VeryLarge, // 2 Base STR, Base AGI -2, Size of a grizzly bear 700 - 1,800 lb
    ExtremelyLarge, // 3 Base STR, Base AGI -3, Size of a large Bull 1,800 - 3,500 lb
    Huge, // 4 Base STR, Base AGI -4, Size of a Hippo 3,500 - 8,000
    Giant, // 5 Base STR, Base AGI -5, Size of an Elephant (or a T-Rex) 8,000 - 15,000
    Massive, // 6 Base STR, Base AGI -6, Size of a Spinosaurus 15,000 - 40,000 lb 
    VeryMassive, // 7 Base STR, Base AGI -7, Size of a ??? 40,000 - 80,000 lb 
    Collosal // 8 Base STR, Base AGI -8, Size of God Zilla / a Brachiosaurus 80,000 - 125,000++ lb (or bigger)
}

public enum CreatureIntelligence
{
    DumbLikeMud, // -3 Base INT, 3 Base Will, 
    VeryDumb, // -2 Base INT, 2 Base Will, 
    Dumb, // -2 Base INT, 2 Base Will, 
    BellowAverage, // -1 Base INT, 1 Base Will, 
    Average, // 0 Base INT, 0 Base Will, 
    AboveAverage, // -1 Base INT, 1 Base Will, 
    Smart, // 1 Base INT, -1 Base Will, 
    VerySmart, // 2 Base INT, -2 Base Will, 
    SuperSmart, // 2 Base INT, -2 Base Will, 
    Brilliant, // 3 Base INT, -3 Base Will, 
    Genious // 4 Base INT, -4 Base Will, 
}

public enum Rating // May not need so many with Power level also being used
{
    Pathetic,
    Weak,
    Average,
    AboveAverage,
    Powerful,
    VeryPowerfull,
    Epic,
    Legendary,
    Mythic
}

public class CreatureTypePercents
{
    public int Fire;
    public int Water;
    public int Earth;
    public int Air;
    public int Life;
    public int Death;
    public int Blood;
    public int Arcane;

    public CreatureTypePercents(int fire, int water, int earth, int air, int life, int death, int blood, int arcane)
    {
        Fire = fire;
        Water = water;
        Earth = earth;
        Air = air;
        Life = life;
        Death = death;
        Blood = blood;
        Arcane = arcane;
    }
}

