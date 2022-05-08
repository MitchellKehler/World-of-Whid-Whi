using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreatureType : MonoBehaviour
{
    public CreatureType creatureType;
    public int Portion; //Major or Minor

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum CreatureType
{
    Normal, // the non life part of anything biological like a human. E.G. a human is part normal part life.
    Fire,
    Earth,
    Air,
    Water,
    Death,
    Life,
    Dragon // (Or Maybe Arcane?) the binding element from the elemental realm
}

/* Types Info
 * 
 * Striengths and Weaknesses
 * Normal: Strong VS Arcane; Very Weak VS Death; Weak VS Water, Earth, Air, Fire;
 * Water:  Very Strong VS Fire; Strong VS Normal; Weak VS Earth, Air, Arcane;
 * Fire:   Strong VS Earth, Air, Normal, Death; Very Weak VS Water; Very Weak VS Arcane;
 * Earth:  Strong VS Water, Normal; Weak VS Fire, Arcane;
 * Air:    Strong VS Water, Normal; Weak VS Fire; Arcane;
 * Life:   Very Strong VS Death; Very Weak VS Arcane;
 * Death:  Very Strong VS Normal; Strong VS Arcane; Very Weak VS Life; Weak VS Fire;
 * Arcane: Ver Strong VS Life, Fire; Strong VS Water, Earth, Air; Weak VS Normal, Death;
 * 
 * Standard Type Combinations
 * Human / Animal    = Normal +, Life -;
 * Elemental         = (What ever element the elemental is) +
 * Bug               = Earth +, Normal -, Life -;
 * Fish              = Water +, Normal -, Life -;
 * Bird              = Air +, Normal -, Life -;
 * Plant             = Earth +; Life -;
 * Schorchion        = Earth +, Normal -, Life -, Fire -; (Fire Scorpion)
 * Burn Bug          = Earth +, Fire -, Normal -, Life -, Air -; (Mostly Bug, A little bit of Fire)
 * Fire Fly (exc)    = Fire +, Earth -, Life -, Air -; (Mostly Fire, A little bit of Bug)
 * Pheonix (exc)     = Fire +, Life -;
 * Flying Bug        = Earth +, Normal -, Life -, Air -;
 * Ground Animal (like a mole) = Normal +, Life -, Earth -;
 * Flightless Bird   = Normal +, Air -, Life -;
 * Water Mamal       = Normal +, Water -, Life -;
 * Skeliton          = Death +, Normal -;
 * Zombie            = Death +, Normal -, Earth -;
 * Goast             = Death +;
 * Raith or Shade    = Death +; Arcane -;
 * Vampire           = Normal +; Death -, Life -, Air -;
 */
