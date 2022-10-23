using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Switch this out with something similar to how I'm handling Creatures now!!!!
/// </summary>
public static class AllAbilities
{
    private static Dictionary<AbilityName, Ability> Abilities;

    static AllAbilities()
    {
        Abilities = new Dictionary<AbilityName, Ability>();
        List<Action> Actions = new List<Action>();
        List<Effect> Effects = new List<Effect>();


        Actions.Add(new Action(0, "waited", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "N/A"));
        Abilities.Add(AbilityName.Wait, new Ability(AbilityName.Wait, 0, "Wait", "Delay choosing an action for a short time.", 30, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, 1f));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Abilities.Add(AbilityName.Scratch_1, new Ability(AbilityName.Scratch_1, 1, "Scratch (Rank 1)", "Scratch the enemy with three quick swipes.", 90, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, 1.2f));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Abilities.Add(AbilityName.Scratch_2, new Ability(AbilityName.Scratch_2, 2, "Scratch (Rank 2)", "Scratch the enemy with four quick swipes.", 90, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, 1.3f));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Actions.Add(new Action(1, "scratched", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash01_red", true, true, AnimationLength.Short));
        Abilities.Add(AbilityName.Shred_4, new Ability(AbilityName.Shred_4, 4, "Shred (Rank 4)", "Shred the enemy with eight very quick swipes.", 90, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, 2.5f)); // probably needs adjustment, later it will apply a condition if this hits which is why it is here
        Actions.Add(new Action(1, "stung", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Slash02_red"));
        Abilities.Add(AbilityName.Swipe_2, new Ability(AbilityName.Swipe_2, 2, "Swipe (Rank 2)", "Lunge forward and sting an enemy.", 70, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.Penetration, AmountType.STR_Multiplier, 1f));
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, 1.5f));
        Actions.Add(new Action(1, "pecked", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit01_red"));
        Abilities.Add(AbilityName.Peck_1, new Ability(AbilityName.Peck_1, 1, "Peck (Rank 1)", "Two hard pecks with a Sharp Beak.", 80, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.Slow, AmountType.Constant, 20));
        Effects.Add(new Effect(EffectType.WaterDamage, AmountType.WILL_Multiplier, 1f));
        Actions.Add(new Action(1, "smacked", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit01_blue"));
        Abilities.Add(AbilityName.TongueSmack_1, new Ability(AbilityName.TongueSmack_1, 1, "Tongue Smack (Rank 1)", "Lash the enemy with a sticky toungue and draw them in.", 70, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.Slow, AmountType.Constant, 50));
        Effects.Add(new Effect(EffectType.WaterDamage, AmountType.WILL_Multiplier, 1.5f));
        Actions.Add(new Action(1, "grabbed", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit01_blue"));
        Abilities.Add(AbilityName.TongueHarpoon_2, new Ability(AbilityName.TongueHarpoon_2, 2, "Tongue Harpoon (Rank 2)", "Lash the enemy with a sticky toungue and draw them in.", 70, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.Slow, AmountType.Constant, 25));
        Effects.Add(new Effect(EffectType.WaterDamage, AmountType.WILL_Multiplier, 1.2f));
        Actions.Add(new Action(3, "lashed", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit01_blue"));
        Abilities.Add(AbilityName.TongueLash_2, new Ability(AbilityName.TongueLash_2, 2, "Tongue Lash (Rank 2)", "Lash the enemy with a sticky toungue and draw them in.", 90, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.Slow, AmountType.Constant, 30));
        Effects.Add(new Effect(EffectType.WaterDamage, AmountType.WILL_Multiplier, 2f));
        Actions.Add(new Action(1, "bashed", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit01_blue"));
        Abilities.Add(AbilityName.TongueBash_2, new Ability(AbilityName.TongueBash_2, 2, "Tongue Bash (Rank 2)", "Lash the enemy with a sticky toungue and draw them in.", 100, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, 3f));
        Actions.Add(new Action(1, "bitten", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit02_red"));
        Abilities.Add(AbilityName.Bite_1, new Ability(AbilityName.Bite_1,
            -1, "Bite (Rank 1)", "Lash the enemy with a sticky toungue and draw them in.", 120, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, 3f));
        Effects.Add(new Effect(EffectType.PoisonDamage, AmountType.WILL_Multiplier, 1f));
        Actions.Add(new Action(1, "bitten", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit02_red"));
        Abilities.Add(AbilityName.VileBite_2, new Ability(AbilityName.VileBite_2, 2, "Vile Bite (Rank 2)", "Lash the enemy with a sticky toungue and draw them in.", 130, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.STR_Multiplier, .75f));
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.SizeDiff_Multiplier, 15f));
        Actions.Add(new Action(1, "bitten", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit02_red"));
        Abilities.Add(AbilityName.Crunch_1, new Ability(AbilityName.Crunch_1, 1, "Crunch (Rank 1)", "Lash the enemy with a sticky toungue and draw them in.", 120, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.STR_Multiplier, 1f));
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.SizeDiff_Multiplier, 20f));
        Actions.Add(new Action(1, "bitten", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit02_red"));
        Abilities.Add(AbilityName.Crunch_2, new Ability(AbilityName.Crunch_2, 2, "Crunch (Rank 2)", "Lash the enemy with a sticky toungue and draw them in.", 120, Pradictability.Unreadable, Actions, true, AbilityName.Crunch_1));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.SizeDiff_Multiplier_Plus, 15f, 10));
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.STR_Multiplier, 1f));
        Actions.Add(new Action(1, "butted", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit02_red"));
        Abilities.Add(AbilityName.HeadButt_1, new Ability(AbilityName.HeadButt_1, 1, "HeadButt (Rank 1)", "Lunge forward and bash the enemy with your head.", 120, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.SizeDiff_Multiplier_Plus, 15f, 10));
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.STR_Multiplier, 1f));
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, 1.5f));
        Actions.Add(new Action(1, "butted", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit03_red"));
        Abilities.Add(AbilityName.HornedHeadButt_2, new Ability(AbilityName.HornedHeadButt_2, 2, "Horned HeadButt (Rank 2)", "Lunge forward and bash the enemy with your head.", 120, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.ImpactDamage, AmountType.SizeDiff_Multiplier_Plus, 15f, 10));
        Actions.Add(new Action(1, "kicked", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit02_red"));
        Abilities.Add(AbilityName.Kick_1, new Ability(AbilityName.Kick_1, 1, "HeadButt (Rank 1)", "Lunge forward and bash the enemy with your head.", 120, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, .1f)); // probably needs adjustment, later it will apply a condition if this hits which is why it is here
        Effects.Add(new Effect(EffectType.PoisonDamage, AmountType.Constant, 20f));
        Actions.Add(new Action(1, "stung", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit02_green"));
        Abilities.Add(AbilityName.Sting_1, new Ability(AbilityName.Sting_1, 1, "Sting (Rank 1)", "Lunge forward and sting an enemy.", 80, Pradictability.Unreadable, Actions, true));

        Actions = new List<Action>();
        Effects = new List<Effect>();
        Effects.Add(new Effect(EffectType.PhisicalDamage, AmountType.STR_Multiplier, .1f)); // probably needs adjustment, later it will apply a condition if this hits which is why it is here
        Effects.Add(new Effect(EffectType.PoisonDamage, AmountType.Constant, 30f));
        Actions.Add(new Action(1, "stung", TargetGroup.Enemy, TargetType.Negative, Effects, AttackType.Melee, "Hit02_green"));
        Abilities.Add(AbilityName.Sting_2, new Ability(AbilityName.Sting_2, 2, "Sting (Rank 2)", "Lunge forward and sting an enemy.", 80, Pradictability.Unreadable, Actions, true));
    }

    public static Ability CloneAbility(AbilityName Name)
    {
        return Abilities[Name].Clone();
    }

    public static Ability GetAbility(AbilityName Name)
    {
        return Abilities[Name];
    }
}

public enum AbilityName
{
    Wait,
    Scratch_1,
    Scratch_2,
    Shred_4,
    Swipe_2,
    Peck_1,
    Bite_1,
    VileBite_2,
    Crunch_1,
    Crunch_2,
    TongueSmack_1,
    TongueLash_2,
    TongueHarpoon_2,
    TongueBash_2,
    HeadButt_1,
    HornedHeadButt_2,
    Bash_1,
    Bash_2,
    Bash_3,
    Spiked_Bash_2,
    Spiked_Bash_3,
    Kick_1,
    Sting_1,
    Sting_2
}
