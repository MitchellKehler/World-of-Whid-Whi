using MLAPI;
using MLAPI.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Battle
{
    /*
        Display Text (You have 2 minutes (or what ever) to review your opponent's creatures)
        NOTE we need a ready button to indicate you are ready early.
        Determine next creatures move (at this point no moves have been picked so it will just start with the creature that has the highest speed (or Agility?))
        If it is an enemy then pick thier move. If it is a player's creature then prompt them to pick an ability.
        Once the mvoe is picked they will likely no longer be the next creature to go because the ability will have a delay so it will pick the next fastest.
             Although tecnically it would be possible for a creature to execute it's first move before a different creature even selected one.
        As creatures that have selected an ability become the next fastest creature their selected ability will be executed first and then they will be prompted to pick another.
        If a creature has an effect on it from an earlier turn the effect is applied at the very beginning of their turn. Effects on a creature can be positive.
        May want to keep a battle time tracker of some kind that keeps track of the time spent based on initiative but not related to any perticular creatures turn. This could be used for things like damage over time effects / conditions.

        Selecting an ability.
            Once an ability has been selected it's speed cost is reduced from the creatures inititive. 
            After a creature has selected it's next ability the base speed (primarily based on agaility) is added to each of the other creature in the battle.

        executing an ability
            when a creature has the highest inititive and they already have an ability selected that ability will be executed before their next ability is selected.
            Each ability has
                a Name
                A Description Text
                A List of Actions
                A predictability rating

            Each action has
                A number of targets
                An indicator of weatehr the targets are frienly or enemy
                A list of effects (simplest effect would be to apply damage)
                An Animation

        TO Do
         - Replace thread logic
            - Battle timer. resets at the start of each turn and is tracked by the server Update function. (higher wait time for players then AI)
            - Once timer hits target the next turn is started. If it was a players turn and they haven't completed picking their action then it is finished by the server.
         - Add the Ready Button and program in the initial 2 minute wait. (Can Wait)
         - Set up Abilities (serializable)
         - Create selected and targeted (negative and positive) images. (White, Red, Green)
         - Add code for server to set and increment initiative and to determine highest initiative. 
         - Add code for server to pick an ability and target / targets for enemy. (keep it very basic for now. Probably everything just random)
         - Add code for server to prompt player to pick an ability and targets for all actions.
         - Add initiative line up (turn order)
         - Add action prompts and question mark button to replay prompt.
         - Add code to execute an ability before selecting another
         - Set up health bars properly to show damage
         - Add Active Effects to Creature Details Panel
        */

    // determine if it is a player controlled creature or a computer controlled creature
    GameManager GM;
    BattleManager BM;
    public Thread Battle_Thread;
    public List<BattleCreature> BattleCreatures;
    public List<BattleCreature> CurrentCreatures;
    public ulong Player1;
    public ulong Player2;
    public ulong Player1CharacterID;
    public ulong Player2CharacterID;
    public int BattleTime = 60;
    int PreviousInitiative;
    public BattleStage Stage;
    GameObject Selected_Creature_Highlight;
    public int Player1SelectedCreature = GameManager.CREATURE_ID_NOT_SET;
    public int Player2SelectedCreature = GameManager.CREATURE_ID_NOT_SET;
    public bool Counting = true;
    List<Action> ActionsToPerform;
    bool Creature_Has_Moved = false;
    bool Collected_Reactions = false;
    int[] previousTargets; // I don't like this much but don't want to think about it any more.

    public Battle(GameManager GM, BattleManager BM, ulong Player1ID, ulong Player1Character, InitializedCreatureData[] teamOneCreatures, ulong Player2ID, ulong Player2Character, InitializedCreatureData[] teamTwoCreatures)
    {
        this.GM = GM;
        this.BM = BM;
        ActionsToPerform = new List<Action>();
        BattleCreatures = new List<BattleCreature>();

        foreach (InitializedCreatureData creatureData in teamOneCreatures)
        {
            if (creatureData.CurrentHP > 0)
            {
                InitializedCreature creature = new InitializedCreature(creatureData);
                Debug.Log("New Creature " + creature.Name);
                Debug.Log("Creature ID " + creature.GetID());
                Debug.Log("creatureData ID " + creatureData.GetID());
                creatureData.battleCreatureID = BattleCreatures.Count;
                Debug.Log("Creature battleCreatureID " + creatureData.battleCreatureID);
                BattleCreature BattleCreature = new BattleCreature(BattleCreatures.Count, GetCreatureInitiative(creature), Player1ID, creature);
                BattleCreatures.Add(BattleCreature);
            }
        }
        foreach (InitializedCreatureData creatureData in teamTwoCreatures)
        {
            if (creatureData.CurrentHP > 0)
            {
                InitializedCreature creature = new InitializedCreature(creatureData);
                Debug.Log("New Creature " + creature.Name);
                Debug.Log("Creature ID " + creature.GetID());
                Debug.Log("creatureData ID " + creatureData.GetID());
                creatureData.battleCreatureID = BattleCreatures.Count;
                Debug.Log("Creature battleCreatureID " + creatureData.battleCreatureID);
                BattleCreature BattleCreature = new BattleCreature(BattleCreatures.Count, GetCreatureInitiative(creature), Player2ID, creature);
                BattleCreatures.Add(BattleCreature);
            }
        }
        Player1 = Player1ID;
        Player2 = Player2ID;
        Player1CharacterID = Player1Character;
        Player2CharacterID = Player2Character;

        NetworkManager.Singleton.ConnectedClients[Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
        if (Player2 != GameManager.SERVERID)
        {
            NetworkManager.Singleton.ConnectedClients[Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
        }

        PreviousInitiative = BattleCreatures.Aggregate((i, j) => i.Initiative < j.Initiative ? i : j).Initiative;

        Stage = BattleStage.BattleStarting;
        Debug.Log("In BattleStarting");

        //       BeginNextTurn();

    }

    public void NextAction()
    {
        Counting = false;
        Debug.Log("In NextAction, Next Stage is " + Stage);
        if (Stage == BattleStage.BattleStarting)
        {
            BeginNextTurn();
        }
        else if (Stage == BattleStage.ChooseAbility)
        {
            //!!!!!!!!!!! Need to make sure all targets are picked here before moving to the neXt turn.
            Debug.Log("ChooseAbility Stage is over");
            SetRandomTargets();
            Debug.Log("Random Targets set");
            IncreaseInitiative(CurrentCreature);
            // Need to tell players about the intiative increase
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { Player1 }
                }
            };
            UpdateClientInitiatives(clientRpcParams);

            Debug.Log("Current Creature's initiative is set");
            BeginNextTurn();
            Debug.Log("Next turn has begun");
        }
        else if (Stage == BattleStage.PerformingActions) // may need a way to determine if the move has more then one ability to set.
        {
            PerformActions();
        } else if (Stage == BattleStage.DonePerformingActions)
        {
            ChooseNextAbility();
        }
        else // This will change
        {
            BeginNextTurn();
        }

    }

    public void UpdateClientInitiatives(ClientRpcParams clientRpcParams)
    {

        List<int> Intiatives = new List<int>();
        int Highest = 0;
        int Lowest = BattleCreatures[0].Initiative;
        foreach (BattleCreature creature in BattleCreatures)
        {
            Intiatives.Add(creature.Initiative);
            if (creature.Initiative > Highest)
            {
                Highest = creature.Initiative;
            }
            if (creature.Initiative < Lowest)
            {
                Lowest = creature.Initiative;
            }
        }
        GM.IncreaseInitiativeClientRpc(Intiatives.ToArray(), Highest, Lowest, clientRpcParams);

    }

    public void BeginNextTurn()
    {
        Debug.Log("In BeginNextTurn");
        Stage = BattleStage.BeginNextTurn;

        ClientRpcParams clientRpcParams;
        if (CurrentCreature != null)
        {
            clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { CurrentCreature.Owner }
                }
            };
            GM.EndTurnClientRpc(clientRpcParams);
        }

        // Set CurrentCreature to creature lowest Initiative (Low Speeds and Initiative are fast)
        // Needs to pick creature at random when all speeds are equal
        CurrentCreature = BattleCreatures.Aggregate((i, j) => i.Initiative < j.Initiative ? i : j);

        // Needs to tell the client to highlight the selected creature regardless of if it is their turn.
        // Should send to both players in a PVP battle.

        if (CurrentCreature.Initiative > PreviousInitiative)
        {
            BattleTime += CurrentCreature.Initiative - PreviousInitiative;
            PreviousInitiative = CurrentCreature.Initiative; // only works if we add moves speed to everyone else instead of subtracting it from the one doing the move, I think.
        }

        // Execute previously picked ability if there is one.
        if (CurrentCreature.NextAbility != null)
        {
            ExecuteAbility();
        } else
        {
            ChooseNextAbility();
        }


    }

    // Take NextCreature's Turn.
    private void AIChooseAbility()
    {
        Debug.Log("In TakeAITurn");
        // needs to pick ability with higher chance for higher ranked abilities
        // get all abilities of this creature grouped by rank
        // set a percentage chance for each based on the number of abilities total and the rank of the ability
        // figure out what rank of ability to pick
        // pick a random ability of that rank.

        List<AbilityData> validAbilities = new List<AbilityData>();
        foreach(AbilityData abilityData in CurrentCreature.Creature.KnownAbilities)
        {
            validAbilities.Add(abilityData);
        }

        if (validAbilities.Count > 0)
        {
            Debug.Log("CurrentCreature.Creature.KnownAbilities.Count: " + CurrentCreature.Creature.KnownAbilities.Count);
            int nextAbilityIndex = UnityEngine.Random.Range(0, CurrentCreature.Creature.KnownAbilities.Count);

            CurrentCreature.NextAbility = AllAbilities.CloneAbility(validAbilities[nextAbilityIndex].AbilityName); // later we will want to also check cool downs when applicable.
        }
        else
        {
            CurrentCreature.NextAbility = AllAbilities.GetAbility(AbilityName.Wait).Clone();
        }

        // needs to highlight creature and display picked ability next to it. (once targets are added they should also be highlighted.

        // Update player that creature is taking it's turn
        // Pick targets (from valid targets)
        // update player
    }

    // Can also be used to set unset targets at the end of player actions.
    public void SetRandomTargets()
    {
        Debug.Log("SetRandomTargets");
        List<int> ValidTargets = new List<int>();
        List<int> Targets = new List<int>();
        for (int ActionIndex = 0; ActionIndex < CurrentCreature.NextAbility.Actions.Count; ActionIndex++)
        {
            if (ActionIndex != 0 && CurrentCreature.NextAbility.LockedTargets)
            {
                CurrentCreature.NextAbility.Actions[ActionIndex].Targets = (int[])CurrentCreature.NextAbility.Actions[0].Targets.Clone();
            } else
            {
                ValidTargets.Clear();
                ValidTargets = GetValidTargets(ActionIndex);

                //Debug.Log("ValidTargets.Count: " + ValidTargets.Count);
                //foreach (int target in ValidTargets)
                //{
                //    Debug.Log("target " + target);
                //}

                foreach (int target in CurrentCreature.NextAbility.Actions[ActionIndex].Targets)
                {
                    //Debug.Log("target = " + target);
                    if (target != GameManager.CREATURE_ID_NOT_SET)
                    {
                        //Debug.Log("removing target = " + target);
                        ValidTargets.Remove(target);
                    }
                }

                //Debug.Log("ValidTargets.Count: " + ValidTargets.Count);
                //foreach (int target in ValidTargets)
                //{
                //    Debug.Log("target " + target);
                //}

                for (int TargetIndex = 0; TargetIndex < CurrentCreature.NextAbility.Actions[ActionIndex].Targets.Length; TargetIndex++)
                {
                    //Debug.Log("TargetIndex = " + TargetIndex);
                    if (CurrentCreature.NextAbility.Actions[ActionIndex].Targets[TargetIndex] == GameManager.CREATURE_ID_NOT_SET && ValidTargets.Count != 0)
                    {
                        //Debug.Log("CurrentCreature.NextAbility.Actions[ActionIndex].Targets[TargetIndex] == GameManager.CREATURE_ID_NOT_SET");
                        //Debug.Log("ValidTargets.Count = " + ValidTargets.Count);
                        int ValidTargetIndex = UnityEngine.Random.Range(0, ValidTargets.Count);
                        //Debug.Log("ValidTargets[ValidTargetIndex] = " + ValidTargets[ValidTargetIndex]);
                        CurrentCreature.NextAbility.Actions[ActionIndex].Targets[TargetIndex] = ValidTargets[ValidTargetIndex];
                        ValidTargets.RemoveAt(ValidTargetIndex);
                    }
                }
            }
        }
    }

    // Execute NextCreature.NextAbility
    private void ExecuteAbility()
    {
        Debug.Log("In ExecuteAbility");
        // foreach action in next ability
        // foreach target in action
        // perform effect on target

        //float animationLength = 0f;
        //switch (CurrentCreature.NextAbility.Actions[0].AnimationLength)
        //{
        //    case AnimationLength.Short:
        //        animationLength = GameManager.BATTLE_ANIMATION_TIME_SHORT;
        //        break;
        //    case AnimationLength.Normal:
        //        animationLength = GameManager.BATTLE_ANIMATION_TIME_NORMAL;
        //        break;
        //    case AnimationLength.Long:
        //        animationLength = GameManager.BATTLE_ANIMATION_TIME_LONG;
        //        break;
        //}
        //TimeTillNextAction = animationLength + GameManager.BATTLE_CREATURE_MOVE_TIME;

        //Counting = true;

        if (CurrentCreature.NextAbility.Name == AbilityName.Wait)
        {
            if (CurrentCreature.Owner == Player1)
            {
                GM.SetInstructionText("Your " + CurrentCreature.Creature.Name + " is Waiting.", Player1);
                GM.SetInstructionText("Their " + CurrentCreature.Creature.Name + " is Waiting.", Player2);
            }
            else if (Player2 != GameManager.SERVERID)
            {
                GM.SetInstructionText("Your " + CurrentCreature.Creature.Name + " is Waiting.", Player1);
                GM.SetInstructionText("Their " + CurrentCreature.Creature.Name + " is Waiting.", Player2);
            }
            Stage = BattleStage.PerformingActions;
            ActionsToPerform = new List<Action>();
            BM.StartCoroutine(BM.SetNextBattleUpdateTime(GameManager.BATTLE_ANIMATION_TIME_NORMAL));
            Counting = true;
        }
        else
        {
            ActionsToPerform = new List<Action>();
            foreach (Action action in CurrentCreature.NextAbility.Actions)
            {
                ActionsToPerform.Add(action.Clone());
                ActionsToPerform.Last().Targets = (int[])action.Targets.Clone();
            }

            //ActionsToPerform = CurrentCreature.NextAbility.Actions;
            Stage = BattleStage.PerformingActions;
            PerformActions();
        }

    }

    private void RemoveKilledCreatures()
    {
        // Should move these to a second list in case they need to be reserected.
        //Debug.Log("Removing Creatures");
        BattleCreature creatureToRemove = null;
        while ((creatureToRemove = BattleCreatures.Find(creature => creature.Creature.CurrentHP <= 0)) != null)
        {
            //Debug.Log("Removing Creature " + creatureToRemoveIndex);
            UpdateTargets(creatureToRemove.ID);
            UpdateActionsToPerformTargets(creatureToRemove.ID);
            BattleCreatures.Remove(creatureToRemove);
        }
    }

    // Return 1 for Player1, 2 for Player2 or 0 for no winner yet
    public int GetWinner()
    {
        // needs to check if both players have not creatures to determine for a draw in PVP battles.

        Debug.Log("BattleCreatures.Find(creature => creature.Owner == Player1) " + BattleCreatures.Find(creature => creature.Owner == Player1));
        Debug.Log("BattleCreatures.Find(creature => creature.Owner == Player2) " + BattleCreatures.Find(creature => creature.Owner == Player2));
        if (BattleCreatures.Find(creature => creature.Owner == Player1) == null)
        {
            return 2;
        } else if (BattleCreatures.Find(creature => creature.Owner == Player2) == null)
        {
            return 1;
        }
        return 0;
    }

    public void PerformActions()
    {
        // Is there some uneque identifier we could use and pass between the client and server other then just the index number which changes to determine which creature is which? can it just be an int?
        // if empty creature had an int ID and a Vector3 Location?
        // if the initialized creature script and the details button are both part of the empty creature prefab and then we don't need the index to handle those clicks
        // Need to consider how selected highlight is handled and targets
        // Targets could probably be put behind the creatures identified by the uneque identifer rather then the array index.
        //
        // Needs to decide if it is moving or performing the action.
        // It should tell the client to move or animate not let the client decide what it should do.
        // Later it will also need to handle reactions between moving and performing the action.

        Debug.Log("int PerformActions");
        Counting = false;

        ulong[] players;
        if (Player2 != GameManager.SERVERID)
        {
            players = new ulong[] { Player1, Player2 };
        }
        else
        {
            players = new ulong[] { Player1 };
        }

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = players
            }
        };

        if (ActionsToPerform.Count > 0)
        {
            List<int> existingTargets = ((int[])(ActionsToPerform[0].Targets)).ToList();
            existingTargets.RemoveAll(target => target != GameManager.CREATURE_ID_NOT_SET);
            int currentActionIndex = CurrentCreature.NextAbility.Actions.IndexOf(ActionsToPerform[0]);
            // should check if it is a ranged attack here and not move if it is
            if (Creature_Has_Moved || (currentActionIndex > 0 && Enumerable.SequenceEqual(previousTargets, ActionsToPerform[0].Targets)))
            {
                // Needs to check for reactions here.
                if (!Collected_Reactions)
                {
                    CollectReactions();
                } else
                {
                    PerformAction(clientRpcParams);
                }
            }
            else
            {
                // Moving to the right spot on the screen first.
                Debug.Log("Moving");
                if (ActionsToPerform[0].Targets.Length > 1)
                {
                    // Go to the middle
                    GM.MoveBattleCreature(CurrentCreature.ID, Vector3.zero, clientRpcParams);
                } else {
                    // go to the specific target
                    Debug.Log("Got single target. target = " + ActionsToPerform[0].Targets[0]);
                    GM.MoveBattleCreature(CurrentCreature.ID, ActionsToPerform[0].Targets[0], clientRpcParams);
                }
                // May want different lengths of time based on how far the creature has to move later.
                Creature_Has_Moved = true;
                BM.StartCoroutine(BM.SetNextBattleUpdateTime(GameManager.BATTLE_CREATURE_MOVE_TIME));
            }

            Debug.Log("Counting");
            Counting = true;

        }
        else
        {
            Counting = false;
            Creature_Has_Moved = false;
            Collected_Reactions = false;
            int winner = GetWinner();
            if (winner == 1)
            {
                BM.EndBattle(Player1);
            } else if (winner == 2)
            {
                BM.EndBattle(Player2);
            } else
            {
                GM.MoveBattleCreature(CurrentCreature.ID, clientRpcParams);
                Stage = BattleStage.DonePerformingActions;
                BM.StartCoroutine(BM.SetNextBattleUpdateTime(GameManager.BATTLE_CREATURE_MOVE_TIME));
                Counting = true;
            }
        }
        Debug.Log("Done ActionsToPerform");
    }

    public void CollectReactions()
    {
        if (CurrentCreature.Owner != Player1)
        {
            GM.SetInstructionText("Their " + CurrentCreature.Creature.Name + " is executing " + CurrentCreature.NextAbility.DisplayName + ". Choose your reactions now.", Player1);
            //if (Player2 != GameManager.SERVERID) // && they have something that can react to it's own creature attacking.
            //{
            //    SetInstructionText("Your " + CurrentCreature.Creature.Name + " is executing " + CurrentCreature.NextAbility.DisplayName + ". Choose your reactions now.", Player2);
            //}
        }
        else if (Player2 != GameManager.SERVERID)
        {
            GM.SetInstructionText("Their " + CurrentCreature.Creature.Name + " is executing " + CurrentCreature.NextAbility.DisplayName + ". Choose your reactions now.", Player2);
            //if (true) // you have something that can react to your own creature attacking
            //{
            //    SetInstructionText("Your " + CurrentCreature.Creature.Name + " is executing " + CurrentCreature.NextAbility.DisplayName + ". Choose your reactions now.", Player1);
            //}
        }
        BM.StartCoroutine(BM.SetNextBattleUpdateTime(GameManager.TIME_TO_REACT));
        Collected_Reactions = true;
    }

    public void PerformAction(ClientRpcParams clientRpcParams)
    {

        previousTargets = ActionsToPerform[0].Targets;
        Debug.Log("Performing the action, next Action is " + ActionsToPerform[0].UseDescription);
        int penetration = 0;
        foreach (Effect effect in ActionsToPerform[0].Effects)
        {
            //Debug.Log("Totat targets: " + ActionsToPerform[0].Targets.Length);
            foreach (int target in ActionsToPerform[0].Targets)
            {
                BattleCreature targetCreature = BattleCreatures.Find(creature => creature.ID == target);
                Debug.Log("target / creature.ID: " + target);

                if (targetCreature != null)
                {
                    int amount = 0;
                    Debug.Log("Amount Type: " + effect.AmountType);
                    if (effect.AmountType == AmountType.Constant)
                        amount = (int)effect.Amount;
                    else if (effect.AmountType == AmountType.STR_Multiplier)
                        amount = (int)(CurrentCreature.Creature.GetTotalStrength() * effect.Amount);
                    else if (effect.AmountType == AmountType.WILL_Multiplier)
                        amount = (int)(CurrentCreature.Creature.GetTotalWill() * effect.Amount);
                    else if (effect.AmountType == AmountType.SizeDiff_Multiplier)
                    {
                        int sizeDiff = (int)InitializedCreature.SizeToNumber(CurrentCreature.Creature.Size) - (int)InitializedCreature.SizeToNumber(targetCreature.Creature.Size);
                        sizeDiff = (sizeDiff > 0 ? sizeDiff : 0);
                        amount = (int)(effect.Amount * sizeDiff);
                    }
                    else if (effect.AmountType == AmountType.SizeDiff_Multiplier_Plus)
                    {
                        int sizeDiff = (int)InitializedCreature.SizeToNumber(CurrentCreature.Creature.Size) - (int)InitializedCreature.SizeToNumber(targetCreature.Creature.Size);
                        sizeDiff = (sizeDiff > 0 ? sizeDiff : 0);
                        amount = (int)effect.Amount2 + (int)(effect.Amount * sizeDiff);
                    }
                    Debug.Log("Amount : " + amount);
                    if (effect.EffectType == EffectType.Penetration)
                    {
                        penetration += amount;
                    }
                    if (effect.EffectType == EffectType.PhisicalDamage)
                    {
                        int armour = (targetCreature.Creature.Armor - penetration > 0 ? targetCreature.Creature.Armor - penetration : 0);
                        targetCreature.Creature.CurrentHP -= amount - armour;
                    }
                    if (effect.EffectType == EffectType.ImpactDamage)
                    {
                        targetCreature.Creature.CurrentHP -= amount;
                    }
                    if (effect.EffectType == EffectType.FireDamage)
                    {
                        // Needs to handle creature types
                        int resistance = ((targetCreature.Creature.General_Resistance + targetCreature.Creature.Fire_Resistance) - penetration > 0 ? (targetCreature.Creature.General_Resistance + targetCreature.Creature.Fire_Resistance) - penetration : 0);
                        targetCreature.Creature.CurrentHP -= amount - resistance;
                    }
                    if (effect.EffectType == EffectType.WaterDamage)
                    {
                        // Needs to handle creature types
                        int resistance = ((targetCreature.Creature.General_Resistance + targetCreature.Creature.Water_Resistance) - penetration > 0 ? (targetCreature.Creature.General_Resistance + targetCreature.Creature.Water_Resistance) - penetration : 0);
                        targetCreature.Creature.CurrentHP -= amount - resistance;
                    }
                    if (effect.EffectType == EffectType.PoisonDamage)
                    {
                        // Needs to handle creature types
                        int resistance = ((targetCreature.Creature.General_Resistance + targetCreature.Creature.Poison_Resistance) - penetration > 0 ? (targetCreature.Creature.General_Resistance + targetCreature.Creature.Poison_Resistance) - penetration : 0);
                        targetCreature.Creature.CurrentHP -= amount - resistance;
                    }
                    if (effect.EffectType == EffectType.ElectricDamage)
                    {
                        // Needs to handle creature types
                        int resistance = ((targetCreature.Creature.General_Resistance + targetCreature.Creature.Electric_Resistance) - penetration > 0 ? (targetCreature.Creature.Electric_Resistance + targetCreature.Creature.Electric_Resistance) - penetration : 0);
                        targetCreature.Creature.CurrentHP -= amount - resistance;
                    }
                    if (effect.EffectType == EffectType.DeathDamage)
                    {
                        // Needs to handle creature types
                        int resistance = ((targetCreature.Creature.General_Resistance + targetCreature.Creature.Death_Resistance) - penetration > 0 ? (targetCreature.Creature.General_Resistance + targetCreature.Creature.Death_Resistance) - penetration : 0);
                        targetCreature.Creature.CurrentHP -= amount - resistance;
                    }
                    // Arcane Resistance Not Added Yet!
                    //if (effect.EffectType == EffectType.ArcaneDamage)
                    //{
                    //    // Needs to handle creature types
                    //    int resistance = ((targetCreature.Creature.General_Resistance + targetCreature.Creature.Arcane_Resistance) - penetration > 0 ? (targetCreature.Creature.General_Resistance + targetCreature.Creature.Arcane_Resistance) - penetration : 0);
                    //    targetCreature.Creature.CurrentHP -= amount - resistance;
                    //}
                    if (effect.EffectType == EffectType.Slow)
                    {
                        targetCreature.Initiative += amount;
                    }
                    Debug.Log("targetCreature.Creature.CurrentHP = " + targetCreature.Creature.CurrentHP);
                }
            }
        }
        InitializedCreatureData[] initializedCreatures = new InitializedCreatureData[BattleCreatures.Count];

        for (int CreatureCount = 0; CreatureCount < BattleCreatures.Count; CreatureCount++)
        {
            //Debug.Log(BattleCreatures[CreatureCount].Creature.Name + ", creature number " + CreatureCount + ", has " + BattleCreatures[CreatureCount].Creature.CurrentHP + " HP.");
            initializedCreatures[CreatureCount] = new InitializedCreatureData(BattleCreatures[CreatureCount].Creature, BattleCreatures[CreatureCount].Creature.GetID());
            initializedCreatures[CreatureCount].battleCreatureID = BattleCreatures[CreatureCount].ID;
        }

        //Debug.Log("ActionsToPerform[0].Targets");
        //LogArray(ActionsToPerform[0].Targets);
        //ClientDisplayActionData targetData = new ClientDisplayActionData(CurrentCreature.ID, CurrentCreature.Owner, ActionsToPerform[0].Targets, (ActionsToPerform.Count <= 1));

        ActionTargetsData targetData = new ActionTargetsData(ActionsToPerform[0].Targets, ActionsToPerform[0].AnimationName); // may be able to get rid of this later when only the impacted creatures information is being passed to the clients


        Debug.Log("Calling GM.PerformAction in Batle.cs");
        GM.PerformAction(initializedCreatures, CurrentCreature.ID, targetData, clientRpcParams);
        UpdateClientInitiatives(clientRpcParams);

        RemoveKilledCreatures();

        float animationLength = 0f;
        switch (ActionsToPerform[0].AnimationLength)
        {
            case AnimationLength.Short:
                Debug.Log("Got SHORT");
                animationLength = GameManager.BATTLE_ANIMATION_TIME_SHORT;
                break;
            case AnimationLength.Normal:
                Debug.Log("Got NORMAL");
                animationLength = GameManager.BATTLE_ANIMATION_TIME_NORMAL;
                break;
            case AnimationLength.Long:
                Debug.Log("Got LONG");
                animationLength = GameManager.BATTLE_ANIMATION_TIME_LONG;
                break;
        }
        BM.StartCoroutine(BM.SetNextBattleUpdateTime(animationLength));

        ActionsToPerform.RemoveAt(0);
    }

    public void UpdateTargets(int TargetToClear)
    {
        for (int CreatureCount = 0; CreatureCount < BattleCreatures.Count; CreatureCount++)
        {
            for (int ActionCount = 0; ActionCount < BattleCreatures[CreatureCount].NextAbility.Actions.Count; ActionCount++)
            {
                //Debug.Log("Action Number " + ActionCount + " of Creature Number " + CreatureCount + " has these targets ");
                //LogArray(BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets);
                for (int TargetCount = 0; TargetCount < BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets.Length; TargetCount++)
                {
                    if (BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets[TargetCount] == TargetToClear)
                    {
                        //Debug.Log("Removing Target " + BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets[TargetCount] + " from Action Number " + ActionCount + " of Creature Number " + CreatureCount + "'s next ability.");
                        BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets[TargetCount] = GameManager.CREATURE_ID_NOT_SET;
                    }
                    //else if (BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets[TargetCount] > TargetToClear)
                    //{
                    //    //Debug.Log("Reducing Target " + BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets[TargetCount] + " from Action Number " + ActionCount + " of Creature Number " + CreatureCount + "'s next ability by 1.");
                    //    BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets[TargetCount]--;
                    //}
                }
                //Debug.Log("Now Action Number " + ActionCount + " of Creature Number " + CreatureCount + " has these targets ");
                //LogArray(BattleCreatures[CreatureCount].NextAbility.Actions[ActionCount].Targets);
            }
        }
    }

    public void UpdateActionsToPerformTargets(int TargetToClear)
    {
        if (ActionsToPerform.Count > 1)
        {
            for (int actionCount = 1; actionCount < ActionsToPerform.Count; actionCount++)
            {
                //Debug.Log("Action Number " + actionCount + " in ActionsToPerform has these targets ");
                //LogArray(ActionsToPerform[actionCount].Targets);
                for (int targetCount = 0; targetCount < ActionsToPerform[actionCount].Targets.Length; targetCount++)
                {
                    if (ActionsToPerform[actionCount].Targets[targetCount] == TargetToClear)
                    {
                        //Debug.Log("Removing Target " + ActionsToPerform[actionCount].Targets[targetCount] + " from Action Number " + actionCount + " in ActionsToPerform.");
                        ActionsToPerform[actionCount].Targets[targetCount] = GameManager.CREATURE_ID_NOT_SET;
                    }
                    //else if (ActionsToPerform[actionCount].Targets[targetCount] > TargetToClear)
                    //{
                    //    //Debug.Log("Reducing Target " + ActionsToPerform[actionCount].Targets[targetCount] + " from Action Number " + actionCount + " in ActionsToPerform by 1.");
                    //    ActionsToPerform[actionCount].Targets[targetCount]--;
                    //}
                }
                //Debug.Log("Now Action Number " + actionCount + " in ActionsToPerform has these targets ");
                //LogArray(ActionsToPerform[actionCount].Targets);
            }
        }

    }

    private void LogArray(int[] array) // just for debugging. Can delete this method later
    {
        foreach (int i in array)
        {
            Debug.Log(i + ",");
        }
    }

    public void Reload_AbilityPick_Panel(ulong clientId, int CreatureNumber)
    {
        Debug.Log("In Reload_AbilityPick_Panel");

        //int BattleCreatureNumber = CreatureNumber - 1;
        //int NumberOfOwnedCreatures = 0;
        //if (BattleCreatureNumber >= 4) // I hate this but don't have time to change it right now.
        //{
        //    for (int i = 0; i < BattleCreatures.Count; i++)
        //    {
        //        if (BattleCreatures[i].Owner == clientId)
        //        {
        //            NumberOfOwnedCreatures++;
        //        }
        //    }
        //    BattleCreatureNumber = BattleCreatureNumber - (4 - NumberOfOwnedCreatures);
        //}

        int WaitSpeed = GetInitiative(BattleCreatures.Find(creature => creature.ID == CreatureNumber), AllAbilities.GetAbility(AbilityName.Wait));

        GM.Reload_AbilityPick_Panel(clientId, CreatureNumber, (Stage == BattleStage.ChooseAbility && CurrentCreature.Owner == clientId && CurrentCreature.ID == CreatureNumber), BattleCreatures.Find(creature => creature.ID == CreatureNumber).Creature.KnownAbilities.ToArray(), WaitSpeed);
    }

    public void Close_AbilityPick_Panel(ulong clientId)
    {
        if (Player1 == clientId)
        {
            Player1SelectedCreature = GameManager.CREATURE_ID_NOT_SET;
        }
        else
        {
            Player2SelectedCreature = GameManager.CREATURE_ID_NOT_SET;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void EncounterCreatureClickedServerRpc(ulong clientId, int clickedCreature)
    {
        if (Player1 == clientId)
        {
            Player1SelectedCreature = clickedCreature;
        }
        else
        {
            Player2SelectedCreature = clickedCreature;
        }
    }


    public void ChooseNextAbility()
    {
        Stage = BattleStage.ChooseAbility;
        Debug.Log("In ChooseNextAbility");

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { CurrentCreature.Owner }
            }
        };

        GM.SetSelectedCratures(CurrentCreatures, clientRpcParams);
        //Debug.Log("Client " + CurrentCreature.Owner + " selected creature set.");

        if (Player1SelectedCreature != GameManager.CREATURE_ID_NOT_SET)
        {
            Debug.Log("Player1SelectedCreature is " + Player1SelectedCreature);
            Reload_AbilityPick_Panel(Player1, Player1SelectedCreature);
        }
        if (Player2 != GameManager.SERVERID && Player2SelectedCreature != GameManager.CREATURE_ID_NOT_SET)
        {
            Reload_AbilityPick_Panel(Player2, Player2SelectedCreature);
        }

        if (CurrentCreature.Owner == Player1)
        {
            Debug.Log("Player1 " + CurrentCreature.Creature.Name + " is picking it's next ability.");
            GM.SetInstructionText("Pick " + CurrentCreature.Creature.Name + "'s next ability!", Player1);
            GM.SetInstructionText("Their " + CurrentCreature.Creature.Name + " is picking it's next ability.", Player2);
        }
        else if (Player2 != GameManager.SERVERID)
        {
            Debug.Log("Player2 " + CurrentCreature.Creature.Name + " is picking it's next ability.");
            GM.SetInstructionText("Pick " + CurrentCreature.Creature.Name + "'s next ability!", Player2);
            GM.SetInstructionText("Their " + CurrentCreature.Creature.Name + " is picking it's next ability.", Player1);
        }

        // AI creature. Server decides it's move
        if (CurrentCreature.Owner == GameManager.SERVERID)
        {
            BM.StartCoroutine(BM.SetNextBattleUpdateTime(GameManager.SERVER_ABILITY_PICK_TIME));
            Counting = true;
            AIChooseAbility();
        }
        else // Human Creature. Prompt client for a move.
        {
            BM.StartCoroutine(BM.SetNextBattleUpdateTime(GameManager.PLAYER_ABILITY_PICK_TIME));
            Counting = true;
            CurrentCreature.NextAbility = AllAbilities.CloneAbility(AbilityName.Wait); // later we will want to also check cool downs when applicable.
        }
        Debug.Log("Done ChooseNextAbility()");

    }

    public void RequestTargets(AbilityData ability)
    {
        CurrentCreature.NextAbility = AllAbilities.CloneAbility(ability.AbilityName);

        //GM.ClearTargetsClientRpc(clientRpcParams);

        if (CurrentCreature.NextAbility.Name != AbilityName.Wait)
        {
            int CurrentAction = GetCurrentAction();
            if (CurrentAction != -1)
            {
                // Request targets for this action.
                List<int> validTargets = GetValidTargets(CurrentAction);

                int NumberOfTargets = validTargets.Count < CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Length ? validTargets.Count : CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Length;
                ActionTargetsData targetData = new ActionTargetsData(CurrentCreature.NextAbility.Actions[CurrentAction].TargetGroup, CurrentCreature.NextAbility.Actions[CurrentAction].TargetType, validTargets.ToArray(), NumberOfTargets);
                Debug.Log("CurrentCreature.Owner " + CurrentCreature.Owner);
                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { CurrentCreature.Owner }
                    }
                };
                GM.PickNextTargets(targetData, clientRpcParams);
            }
        }
    }

    public void TargetClicked(int TargetNumber)
    {
        Debug.Log("In Battle, TargetClicked");
        int CurrentAction = GetCurrentAction();
        if (CurrentAction != -1) // needs to check if we are undoing a target
        {
            int NextTargetIndex = GetNextTargetIndex(CurrentAction);
            if (NextTargetIndex != GameManager.CREATURE_ID_NOT_SET)
            {
                if (CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Contains(TargetNumber))
                {
                    for (int i = 0; i < CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Length; i++)
                    {
                        if (CurrentCreature.NextAbility.Actions[CurrentAction].Targets[i] == TargetNumber)
                        {
                            CurrentCreature.NextAbility.Actions[CurrentAction].Targets[i] = GameManager.CREATURE_ID_NOT_SET;
                        }
                    }
                } else
                {
                    CurrentCreature.NextAbility.Actions[CurrentAction].Targets[NextTargetIndex] = TargetNumber;
                }

            }
            else
            {
                Debug.Log("Error! No Valid Target, we should never get here!");
            }
        } else {
            CurrentAction = 0;
            if (!CurrentCreature.NextAbility.LockedTargets)
            {
                while (CurrentAction < CurrentCreature.NextAbility.Actions.Count)
                {
                    if (CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Contains(GameManager.CREATURE_ID_NOT_SET)) //
                    {
                        break;
                    }
                    else
                    {
                        CurrentAction++;
                    }
                }
            }

            for (int i = 0; i < CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Length; i++)
            {
                if (CurrentCreature.NextAbility.Actions[CurrentAction].Targets[i] == TargetNumber)
                {
                    CurrentCreature.NextAbility.Actions[CurrentAction].Targets[i] = GameManager.CREATURE_ID_NOT_SET;
                }
            }
        }

        List<int> validTargets = GetValidTargets(CurrentAction);

        int NumberOfTargets = validTargets.Count < CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Length ? validTargets.Count : CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Length;
        ActionTargetsData targetData = new ActionTargetsData(CurrentCreature.NextAbility.Actions[CurrentAction].TargetGroup, CurrentCreature.NextAbility.Actions[CurrentAction].TargetType, validTargets.ToArray(), CurrentCreature.NextAbility.Actions[CurrentAction].Targets, NumberOfTargets);

        //ClientRpcParams clientRpcParams;
        //if (Player2 == GameManager.SERVERID)
        //{
        //    clientRpcParams = new ClientRpcParams
        //    {
        //        Send = new ClientRpcSendParams
        //        {
        //            TargetClientIds = new ulong[] { Player1 }
        //        }
        //    };
        //} else
        //{
        //    clientRpcParams = new ClientRpcParams
        //    {
        //        Send = new ClientRpcSendParams
        //        {
        //            TargetClientIds = new ulong[] { Player1, Player2 }
        //        }
        //    };
        //}

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { CurrentCreature.Owner }
            }
        };
        GM.UpdateTargets_Server(targetData, clientRpcParams);

    }

    public int GetCurrentAction()
    {
        for (int i = 0; i < CurrentCreature.NextAbility.Actions.Count; i++)
        {
            if (CurrentCreature.NextAbility.Actions[i].Targets.Contains(GameManager.CREATURE_ID_NOT_SET) && (i == 0 || !CurrentCreature.NextAbility.LockedTargets)) //
            {
                return i;
            }
        }
        return GameManager.CREATURE_ID_NOT_SET; // all targets for the current ability have been set.
    }

    public int GetNextTargetIndex(int CurrentAction)
    {
        for (int i = 0; i < CurrentCreature.NextAbility.Actions[CurrentAction].Targets.Length; i++)
        {
            if (CurrentCreature.NextAbility.Actions[CurrentAction].Targets[i] == GameManager.CREATURE_ID_NOT_SET)
            {
                return i;
            }
        }
        return GameManager.CREATURE_ID_NOT_SET; // All targets for this action have been picked.
    }

    public List<int> GetValidTargets(int CurrentAction)
    {

        List<int> validTargets = new List<int>();

        if (CurrentCreature.NextAbility.Actions[CurrentAction].TargetGroup == TargetGroup.Friendly)
        {
            for (int creatureIndex = 0; creatureIndex < BattleCreatures.Count; creatureIndex++)
            {
                if (BattleCreatures[creatureIndex].Owner == CurrentCreature.Owner)
                {
                    validTargets.Add(BattleCreatures[creatureIndex].ID);
                }
            }
        }
        else if (CurrentCreature.NextAbility.Actions[CurrentAction].TargetGroup == TargetGroup.Friendly_Other)
        {
            for (int creatureIndex = 0; creatureIndex < BattleCreatures.Count; creatureIndex++)
            {
                if (BattleCreatures[creatureIndex].Owner == CurrentCreature.Owner && !BattleCreatures[creatureIndex].Equals(CurrentCreature))
                {
                    validTargets.Add(BattleCreatures[creatureIndex].ID);
                }
            }
        }
        else if (CurrentCreature.NextAbility.Actions[CurrentAction].TargetGroup == TargetGroup.Enemy)
        {
            for (int creatureIndex = 0; creatureIndex < BattleCreatures.Count; creatureIndex++)
            {
                if (BattleCreatures[creatureIndex].Owner != CurrentCreature.Owner)
                {
                    validTargets.Add(BattleCreatures[creatureIndex].ID);
                }
            }
        }
        else // All
        {
            for (int creatureIndex = 0; creatureIndex < BattleCreatures.Count; creatureIndex++)
            {
                validTargets.Add(BattleCreatures[creatureIndex].ID);
            }
        }

        //foreach (int target in validTargets)
        //{
        //    Debug.Log(target + ", ");
        //}

        return validTargets;

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public int GetCreatureInitiative(InitializedCreature creature)
    {
        // needs to also look at attributes for things like Prepared (or what ever we call it that makes creatures like archers start with extra).
        // I was thinking having this set here instead of in the actual initialized creature as StartingInitiative similar to MaxHP because things like ambushes could frequently effect it.
        return -creature.Agility;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseInitiative(BattleCreature creature)
    {
        // Should also consider Mind
        // Maybe have a min value of 10?
        if (GetsInitiative(creature))
        {
            creature.Initiative += GetInitiative(creature, creature.NextAbility);
        }
    }

    public int GetInitiative(BattleCreature creature, Ability ability)
    {
        // Should also consider Mind
        // Maybe have a min value of 10?
        return ability.Speed - creature.Creature.Agility;
    }

    public bool GetsInitiative(BattleCreature creature)
    {
        // Needs to consider if the creature is alive and certain conditions the creature may have like being stunned.
        return true;
    }

    public void Run(ulong PlayerID) // player that is trying to run
    {
        // TODO should check if run was successful
        // Selecting Run should put a flag that limits all player controlled creature's actions to quick and defensive actions.
        // Once each of the player controlled creatures, and the player, have had a turn the result of the run attempt is calculated.
        // The run may not be successful, this will be dependant on the speed of the player's creatures VS the speed of the enemy in
        // combination with the temperment and anger level of the enemy. Generarlly at the very least a run attempt will reduce the 
        // anger level of the enemy making the next run attempt more likely to succeed (especially against non aggressive enemies).

        if (PlayerID == Player1)
        {
            BM.EndBattle(Player2);
        } else
        {
            BM.EndBattle(Player1);
        }
    }

    public void SetInstructionText(string text)
    {
        ClientRpcParams clientRpcParams;
        if (Player2 == GameManager.SERVERID)
        {
            clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { Player1 }
                }
            };

        }
        else
        {
            clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { Player1, Player2 }
                }
            };
        }

        GM.SetInstructionText(text, true, clientRpcParams);
    }

    public static float GetHighilghtSize(CreatureSize creatureSize)
    {
        float highLightSize;

        switch (creatureSize)
        {
            case CreatureSize.Collosal:
                highLightSize = 2;
                break;
            case CreatureSize.VeryMassive:
                highLightSize = 1.4f;
                break;
            case CreatureSize.Massive:
                highLightSize = 1;
                break;
            case CreatureSize.Huge:
                highLightSize = 0.8f;
                break;
            case CreatureSize.Giant:
                highLightSize = 1f;
                break;
            case CreatureSize.ExtremelyLarge:
                highLightSize = 0.9f;
                break;
            case CreatureSize.VeryLarge:
                highLightSize = 0.8f;
                break;
            case CreatureSize.Large:
                highLightSize = 0.7f;
                break;
            case CreatureSize.Medium:
                highLightSize = 0.6f;
                break;
            case CreatureSize.Small:
                highLightSize = 0.5f;
                break;
            case CreatureSize.VerySmall:
                highLightSize = 0.425f;
                break;
            case CreatureSize.SuperSmall:
                highLightSize = 0.3f;
                break;
            case CreatureSize.Tiny:
                highLightSize = 0.275f;
                break;
            default: // Very Tiny
                highLightSize = 0.2f;
                break;
        }

        return highLightSize;
    }

    /// <summary>
    /// Position 4 (Small) would have
    /// -7.9, 2.9 and -6.715, 2.015
    /// Position 2 (Smallest) would have
    /// -4.25, 2.95 and -3.15, 2.95 and -4.25, 1.9 and -3.15, 1.9
    /// </summary>
    /// <param name="creatureSize"></param>
    /// <returns></returns>
    public static BattlePositionSize GetBattlePositionSize(CreatureSize creatureSize)
    {
        BattlePositionSize positionSize;

        switch (creatureSize)
        {
            case CreatureSize.Collosal:
                positionSize = BattlePositionSize.Largest;
                break;
            case CreatureSize.VeryMassive:
                positionSize = BattlePositionSize.Largest;
                break;
            case CreatureSize.Massive:
                positionSize = BattlePositionSize.Largest;
                break;
            case CreatureSize.Huge:
                positionSize = BattlePositionSize.Large;
                break;
            case CreatureSize.Giant:
                positionSize = BattlePositionSize.Large;
                break;
            case CreatureSize.ExtremelyLarge:
                positionSize = BattlePositionSize.Medium;
                break;
            case CreatureSize.VeryLarge:
                positionSize = BattlePositionSize.Medium;
                break;
            case CreatureSize.Large:
                positionSize = BattlePositionSize.Medium;
                break;
            case CreatureSize.Medium:
                positionSize = BattlePositionSize.Medium;
                break;
            case CreatureSize.Small:
                positionSize = BattlePositionSize.Small;
                break;
            case CreatureSize.VerySmall:
                positionSize = BattlePositionSize.Small;
                break;
            case CreatureSize.SuperSmall:
                positionSize = BattlePositionSize.Smallest;
                break;
            case CreatureSize.Tiny:
                positionSize = BattlePositionSize.Smallest;
                break;
            default: // Very Tiny
                positionSize = BattlePositionSize.Smallest;
                break;
        }

        return positionSize;
    }

}

public enum BattleStage
{
    BattleStarting,
    BeginNextTurn,
    ChooseAbility, // if there are multiple abilities to pick is there a new timer for each? if so then we need a way to know if there is another ability after the current one.
    PerformingActions,
    DonePerformingActions,
    BattleEnding // May not need this one.
}

public enum BattlePositionSize
{
    Smallest,
    Small,
    Medium,
    Large,
    Largest
}

public class BattleCreature
{
    public int ID;
    public int Initiative;
    public ulong Owner; // Client ID, GameManager.SERVERID if it is the Server
    public Ability NextAbility; // may need to be a serializable Prepped Ability instead.

    public InitializedCreature Creature;

    public BattleCreature(int iD, int initiative, ulong owner, InitializedCreature creature)
    {
        ID = iD;
        Initiative = initiative;
        Owner = owner;
        Creature = creature;
    }

    


}

public class Condition
{

}

