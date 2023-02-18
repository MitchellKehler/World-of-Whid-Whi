using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;
using System.Linq;
using System;
using Assets.FantasyMonsters.Scripts;
using UnityEngine.SceneManagement;
using MLAPI.Spawning;
using TMPro;
using MLAPI.Prototyping;

/* ///////////////////////////////////     TO DO     ///////////////////////////////////
 * 
 * To Do List
 * 
 * Basic play thourgh testing and Bug fixes
 *  First time loading a newly created character the creature menu outside fo combat won't open
 *  Need to open a menu before you can move on the phone?
 *  Fix heal button
 *  settings has the option to sign out, and 
 *  Test multi player battles
 *  proper player challenge prompt when fighting another player
 *  player names show up correctly in battles
 *  
 * Fix minor bugs with displays so that 
 *  creature details displays correctly, 
 *      Make Health bar work correctly when viewing your monsters out of combat
 *      Update creature details menu and add an option to change a creatures nick name
 *  all other menu displays at least don't look broken (coming soon or something)
 *  battle zones fixed
 *  rework combat visuals to make it more clear what is happening and what you need to do
 * Move combat stuff out of updates where possible in favor of events and fix minor issues with combat like animation timings and clearing dead creatures
 * Add reactions
 * Add creature leveling and evelutions
 * Add basic NPCs and dialog to show patch notes and coming next info ect when talking to main healer NPC
 * 
 * Make server into a git container or windows service
 * Make sure that the server crashing is not a common occurence (fix known bugs that cuase this)
 * Make sure I have some way of being notifyed when the server crashes and restarting it remotely
 ***************************** Add alpha version to the play store !!!!!!!!!! *************************************
 * Add mini map
***************************** New version in the play store !!!!!!!!!! *************************************
 * Do some combat balancing and maybe add a bit more content (e.g. more abilities and evolution groups)
 * Add basic jurnal with achievements
 ***************************** New version in the play store !!!!!!!!!! *************************************
 * Add Chat
 ***************************** New version in the play store !!!!!!!!!! *************************************
 * Add Items
 * player model customization options
 * Add player as a creature on the battle field
 ***************************** New version in the play store !!!!!!!!!! *************************************
 * Fix terrain (hide trees when behind and remove masking stuff)
 * Figure out a easy and established way to quickly add to the map
 * Add additional map areas like caves and area over the bridge
 ***************************** New version in the play store !!!!!!!!!! *************************************
 * Add more advanded NPC and NPC dialog set up and additional NPCs
 * Add Quests
 * Add map for looking at larger regions / areas
 ***************************** New version in the play store !!!!!!!!!! *************************************
 * Make brush move when you walk through it
 * Switch out creature spawning to add creatures to map areas instead of coliding while walking through brush
 * switch to 4D (creatures if available) and characters
 ***************************** New version in the play store !!!!!!!!!! *************************************
 * More content
 * Currency and shops
 * Start actually adding story line
 * Optomizing if neccessary
 * Website
 * Start acutally looking for users / Beta?
 * 
 * Look for !FIX for areas where I know work needs to happen.
 * 
 * //////////////////     Playability (Make it easier for the user to play)     //////////////////
 * 
 * Check in Code!!!
 * 
 * Fix creature details menu
 * 
 * Rework battles to use events to track turns instead of updates (apply any other events in place of updates code that is found during this process)
 * Add Reactions
 * Fix Run
     * Stop server breaking if client disconnects in battle by having them run first then handle the rest of the disconnect
     * Add consiquences of running, Maybe just set amount of damage recieved based on enemy power for now? later you should have a chance to fail the run away attempt and the enemy keeps getting turns while you try to run
 * 
 * Clean up general battle feel and visuals to make it easy to understand what is going on
     * Add message to indicate enemy is taking their turn.
     * Clear Creatures After Death
     * Fix attack timing
     * Small creatures health bar and name closer to sprite (not a big deal)
     * Fix Backgrounds to cover whole battle field
 * 
 * add leveling of creatures
 * add capturing creatures
 * 
 * Basic AI for picking attack to have a higher chance to pick higher ranked attacks.
 * 
 * 
 * Clean up comments!!!!!
 * 
 * Add Icons for first 6 creatures in sign in menu
 * Add Player Details
    * Known Creatures List
    * Reputations (worry about later)
    * Achievements and completed quest list (worry about later)
    * Map Expoloration record (worry about later)
    * Initialized Creatures List (with some indicator of which are in current squad and starting lineup layout
    * 
    * Battle Display toggle setting
 *
 * Add creatures when space opens up
 * Add ranged, reduced speed (this is a good thing) for first attack/attacks not moving.
 * Add power up conditions
 * Add powerup stat tracking (leveling up) in battles
 * not notifying the client when an attack isn't performed because all targets are missing / dead.
 * Fix out of combat viewing creatures
 * Fix Ability Pick Panel Open And Close Timing
 * Add Creature Capture after win
 * Add Creature Damage / death remains after fight
 * Add code to implement pradictability and properly show player the information they should see and not the information they shouldn't see with regards to which moves the enemy is performing.
 * Add information about abilities in ability pick screen and make stats like speed and damage acurate.
 * Add health bar correctly in creature details
 * Add Evolutions
 * Add Substitute creatures, stashed creatures 
 * Creature dictionary (All Creatures - what they have seen)
 *
 * Set attacing creature to first or last so that it is always in front of the other creatures it might be moving over.
 * Improve information text (larger, color coded text so it is more readable)
 * Add Ability Type Passive, Active, OutOfCombat
 * Add new combat stats / features
     * Add creature type based bonus damage
     * Add stuns and roots
     * Add Focus
     * Vision
     * Add Off Balance
     * Add low health debufs
     * Add heavy damage of one kind bonuses
 * Add talk to old man
    * creature healing
    * How to play
    * User info like release notes, and Game Objectives (the stuff that will eventually be on a web site)
 * Add Map
 * Add Chat System
 * Add Quests
 * Seperate Client and Server Applications
 * Add Terrain
    * Could impact out of combat movement speed
    * Impacts Battle Field Background
    * Impacts Ability Use
        * Hard to charge in a dense forest
    * Vision Score
        * Impacts Ability Use
        * Impacts Flying
        * Impacts Acuracy and Reactions
 * Add Weather Impacts
    * Vision Score
    * Battle Field Background
    * Ability Use
    * Flying
 * Add Lighting
    * Impacts Vision Score
    * Some creatures use other things like sonar or telepathy for vision. Their vision score will not be impacted by lighting but could be impacted by sound or mind based attacks.
 *
 * * New terrain / encounter system
 * Encounter Terrain (Needs a Prefab and a script) - root object, contains lists of Creature Groups to be spawned and list of Spawning Areas.
 *  - Script needs to spawn new Creature Groups within the Spawning Areas at regular intervals based on the Creature Group's spawn frequency (assuming it isn't at it's max capacity)
 * Spawning Area (just needs a Prefab I think) - indicates where creatures from an Encounter Train Creature Group can be spawned.
 *  - litereally just a 2d rectangle (collored and visible for level creation but set to be culled when the game starts) that is used to specify where creature groups can spawn and the bounds they must remain in.
 *  - later aggressive creature groups will be able to leave the bounds up to a certain distance (based on aggressivenes level) if they are chasing someone.
 * Creature Group (Needs a Prefab and a Script) - Contains information about how creatures will be displayed in the world and in combat
 *  - Script 
 *      - Creates a new creature prefab in the world within the spawn area of it's encounter terrain.
 *      - Creature group moves randomly when no player is near (later possibley based on an energy level factor or instincts or other things), but may move towards or away from near by players based on aggressiveness if it detects the player that is.
 *      - If there is a collision between the player and a creature group then a battle is triggered
 *  - in the world - indicates sprite to use, aggressivness, movement speed and types (e.g. walking, swimming, flying), vision and other senses, spawn max and frequency (later one per time of day)
 *      - aggressivness, movement speed and types, vision and other senses can be calculated by the creatures making up the creature group on creation / at start
 *  - in combat - indicates the same information as is currently contained in a creature group (what creatures, their levels, and quantities can be encountered)
 * Encounter Creature (Just a script) - used by Creture Group for storing in combat details.
 * 

 * //////////////////     Functionality (Add new features to the game)     //////////////////
 * 
 * 
 * 
 * //////////////////     Content (Add content within the exiting features)     //////////////////
 * 
 * 
 * 
 * //////////////////     Beautification (Make it look pretty)     //////////////////
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * ///////////////////////////////////     Ideas     ///////////////////////////////////
 * 
 * Add global powerups (like an increase to your focus ability once you hit certain intellegence thresh holds
 * 
 * Add Reactions
 * Dodges, guards, and other reactionary moves should be of a separate class called reactions that are used in response to an attack being executed on your creatures.
 * In general they will add some to your initiative (slowing your next attack) and have cool downs but will give you a chance to dodge or mitigate damage and negative effects coming your way.
 * There may still be active moves that give you a chance to dodge all incoming attacks or increase your likely hood to have successful reactions.
 * 
 * Need to add full dodge / block bool to determine if it acts on an entire ability or just one action.
 * Need to add Focus
 * it would be a uneaque ability like wait but to choose creatures that your creature is paying extra attention to.
 * higher levels would increase the number of creatures that can be focused on and reduce negative effects while amplifying possitive effects.
 * Some skills like combat training my specifically improve focus. Also higher intelegence improves focus.
 * 
 * Damage Type Bonuses
 * Each damage type should have a it's own bonus effects that are applied when a certain percentage of an opponents health removed by a single damage type in a single ability or maybe action.
 * Creatures should also get a debuf of wounded or something similar automatically when they get down to a low amount of health like 25%.
 * For Damage types I'm tentatively thinking
 *  Phisical / Blade - Bleed effect (constant damage over time)
 *  Poison - Weakened (reduce strength)
 *  Fire - ???
 *  Electric - Paralized? (chance to mess up any ability, could be active or reation)?
 *  Ice - Slow, reduced initiative, maybe reduced chance to react
 *  Impact - Chance to stun
 *  Death - ???
 *  Undead Damage through Life Healing - ???
 *  
 *  Map Changes
 *  I need to recreate the map with a solid layer of green at the bottom and then all the other pieces being transparent except for what they are actually adding. Even the bottom layer of grass should be prefabs to allow changing it's image easily, unless this would be hard on the phone but I doubt it.
 *  I would like to add in at least four times of day, dawn, day, dusk, and night. Time of day would change the creatures that can be found, the appearence of the map, give combat bonuses or weaknesses, and impact NPC behaviour.
 *  I would at the very least have the image the prefabs point at change based on the time of day but if possible it would be great if we could have the image slowly become lighter or darker based on time rather then just sudenly change.
 *  We would still need to keep track of the times of day for the aspects of the game other then appearence and certain appeaarence based changes may still happen based on an actual stage of the day like night to dawn.
 *  It would be even more amazing if we could have things like amount of moonlight or weather constanly changing and also impact appearence and maybe even other factors like combat stats and NPC behaviour.
 * 
 * Add Focus
 * 
 * Add Off Balance
 * Creatures get knocked off balance when they take hits which impacts their ability to dodge and their acuracy on their next ability.
 * Some attacks my have bonuses to how much they knock a character off balance. (kind of like a milder stun)
 * 
 * 
 */

public class GameManager : NetworkBehaviour
{
    public const ulong SERVERID = 999999;
    public const float BATTLE_CREATURE_MOVE_TIME = 2f;
    public const float BATTLE_ANIMATION_TIME_SHORT = .4f;
    public const float BATTLE_ANIMATION_TIME_NORMAL = .6f;
    public const float BATTLE_ANIMATION_TIME_LONG = 1f;
    public const float TEXT_FADE_TIME = 2f;
    public const float TIME_TO_REACT = 2.5f;
    public const float TIME_BETWEEN_BATTLE_UPDATES = .1f;
    public const float SERVER_ABILITY_PICK_TIME = 2f;
    public const float PLAYER_ABILITY_PICK_TIME = 30f;
    public const int CREATURE_ID_NOT_SET = -1;
    public const string HIT_ANIMATION_DIRECTORY = @"Inguz Media Studio\The 2DFX Hit and Slashes Vol.1\Prefabs\";
    public const float CAMERA_SIZE_IN_BATTLE = 5f;
    public const float CAMERA_SIZE_IN_WORLD = 5f;

    public static readonly Color SELECTED_COLOR = new Color(1,1,1,1); // white
    public static readonly Color NOT_SELECTED_COLOR = new Color(.5f, .5f, .5f, 1); // grey
    public static readonly Color TARGET_NEGATIVE_COLOR = new Color(1,0,0,1); // red
    public static readonly Color TARGETED_NEGATIVE_COLOR = new Color(.5f, 0,0,1); // dark red
    public static readonly Color TARGET_POSITIVE_COLOR = new Color(0,1,0,1); // green
    public static readonly Color TARGETED_POSITIVE_COLOR = new Color(0, .5f, 0,1); // dark green
    public static readonly Color TARGET_FOCUS_COLOR = new Color(0,0,1,1); // blue
    public static readonly Color TARGETED_FOCUS_COLOR = new Color(0,0, .5f, 1); // dark blue
    public static readonly Color PROVIDING_BUFF_COLOR = new Color(); // yellow
    public static readonly Color PROVIDING_DEBUFF_COLOR = new Color(); // orange
    public static readonly Color SUMMONED_BY_COLOR = new Color(); // purple

    // -11.8, 5 to 11.8, -3 = 23.6 by 8 or 11.8 by 8 per side
    // Smallest would be 2.95 by 2

    public static readonly Vector3 BATTLE_OFFSET = new Vector3(0,30,0);

    // this is here as a temporary fix until the correct code is added. this should be removed when spawn points are handled correctly.
    //public SpawnPoint spawnPoint;

    public Dictionary<ulong, CharacterData> charactersInGame;

    public string logtext;
    public int Player_Side_Toggle;
    private float SecondsTimer = 0;
    private float BattleSecondsTimer = 0;
    ClientDisplayActionData clientDisplayActionData;
    InitializedCreatureData[] NewEncounterCreatures;
    Vector3 CenterOfBattleMap;
    bool ReturnBattleCreature;
    BattleCreatureClient CreatureToReturn;

    // Game Objects
    public GameObject Player;
    public GameObject PlayerPrefab;

    // UI Elements
    public GameObject Detail_Panel_Close_Button;

    // UI - Battle Objects
    public GameObject EmptyCreature;
    public GameObject HealthBar;
    public GameObject Battle_Action_Panel;

    public List<GameObject> Selected_Creature_Highlights; // should really be named Current_Creature_Highlight
    public GameObject Creature_Highlight; // should really be named Current_Creature_Highlight
    public Button Battle_GO_Button;
    public GameObject Ability_Pick_Panel;
    public GameObject AbilityPick_ScrollView_Content;
    public GameObject Ability_Button_Prefab;
    List<GameObject> AbilityButtons;
    public GameObject Ability_Pick_WaitButton;
    public int Selected_Creature;
    public List<GameObject> Target_Creature_Highlights;
    public GameObject Creature_Details_Button;

    public GameObject PlayerSideDark;
    public GameObject PlayerSideLight;
    public Button Toggle_PlayerSides_Button;
    public Text Battle_Player_1_Name;
    public Text Battle_Player_2_Name;

    public GameObject InstructionsText;

    // UI - Creature Details Canvise - Creature Details Main Panel
    public GameObject Creature_Details_Background_Panel;
    public GameObject Creature_Details_Name_Text;
    public GameObject Creature_Details_HP_Text;
    public GameObject Creature_Details_Types_Text;
    public GameObject Creature_Details_PowerLvl_Text;
    public GameObject Creature_Details_Rating_Text;
    public GameObject Creature_Details_Lvl_Text;
    public GameObject Creature_Details_Str_Text;
    public GameObject Creature_Details_Agi_Text;
    public GameObject Creature_Details_Mnd_Text;
    public GameObject Creature_Details_Wil_Text;
    public GameObject Creature_Details_Size_Text;
    public GameObject Creature_Details_Armour_Text;
    public GameObject Creature_Details_General_RES_Text;
    public GameObject Creature_Details_Fire_RES_Text;
    public GameObject Creature_Details_Water_RES_Text;
    public GameObject Creature_Details_Poison_RES_Text;
    public GameObject Creature_Details_Electric_RES_Text;
    public GameObject Creature_Details_Death_RES_Text;
    public Dropdown Creature_Details_Attributes_Dropdown;
    public Dropdown Creature_Details_Abilities_Dropdown;

    // UI - Creature Details Canvise - Creature Details General Details Panel
    public GameObject Creature_Details_Description_Panel;
    public Image Creature_Details_Image;
    public Text Creature_Details_Description;

    // UI - Creature Details Canvise - Creature Details General Details Panel
    public GameObject Creature_Details_Attributes_Panel;
    public Text Creature_Details_Attribute_Description;

    // UI - Creature Details Canvise - Creature Details General Details Panel
    public GameObject Creature_Details_Abilities_Panel;

    // UI - Player Options Canvise
    public GameObject MainCanvise;
    public GameObject SettingsPanel;
    public GameObject MapPanel;
    public GameObject CreaturesPanel;
    public GameObject InventoryPanel;
    public GameObject DiaryPanel;

    // Creatures Panel
    public Button SelectCreatureButton;
    public GameObject Creatures_Panel_ScrollViewContent;
    //public Button Squad_Starting_1_Button;
    //public Button Squad_Starting_2_Button;
    //public Button Squad_Starting_3_Button;
    //public Button Squad_Starting_4_Button;
    //public Button Squad_Subs_1_Button;
    //public Button Squad_Subs_2_Button;
    //public Button Squad_Subs_3_Button;
    //public Button Squad_Subs_4_Button;

    public Server Server;

    // Other Objects
    public Dictionary<string, BaseCreature> AllCreatures;
    public List<GameObject> BattleCreatures;
    //public List<GameObject> BattleCreatureDetailsButtons;

    public List<Battle> Battles;
    public List<int> Battles_To_Remove;

    float Timer = 0.0f;
    public PositionManager position_manager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Started");

        InitializePowerUpGroups.SetAllPowerUps();
        AllCreatures = InitializeCreatures.GetInitializedCreatures();

        Server.Connect();

        if (Server.isServer) //Server.GetLocalIPAddress() == "10.0.0.38" //NetworkManager.Singleton.IsServer
        {
            Debug.Log("I'm the Server");
            //LogToServerRpc(0, "I'm the Server");

            SceneManager.LoadScene("Starting_Area", LoadSceneMode.Additive);
        }

        Battles = new List<Battle>();
        BattleCreatures = new List<GameObject>();
        Battles_To_Remove = new List<int>();
        AbilityButtons = new List<GameObject>();
        Ability_Pick_WaitButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            PickNextAbilityServerRpc(NetworkManager.Singleton.LocalClientId, new AbilityData(AllAbilities.GetAbility(AbilityName.Wait), 30));
        });
        Target_Creature_Highlights = new List<GameObject>();
        Selected_Creature_Highlights = new List<GameObject>();

        Player_Side_Toggle = 0; // Later this should be taken from the player's preferences.
        ReturnBattleCreature = false;
        CenterOfBattleMap = new Vector3(1, 30.7f, 0);

        charactersInGame = new Dictionary<ulong, CharacterData>();

        //LogToServerRpc(0, "I'm NOT the Server");
        ////Server.ErrorText.text = "Made it through GM Constructor!";

    }

    // Update is called once per frame
    // Should split the individual functions out of this into their own methods as it is getting pretty long and complicated.
    // This will also make it easy to transition things that should only happen each second out into being called by a timer or something.
    void Update()
    {
        if (NetworkManager.Singleton.IsServer) ///// NetworkManager.Singleton.IsServer!!!!!!!!!!!
        {
            Timer += Time.deltaTime;
            if (Timer >= TIME_BETWEEN_BATTLE_UPDATES) // a second has elapsed
            {
                // Server and Client
                Timer -= TIME_BETWEEN_BATTLE_UPDATES;
                BattleSecondsTimer += TIME_BETWEEN_BATTLE_UPDATES;

                //// Client
                //if (SecondsTimer > 0f)
                //{
                //    SecondsTimer-=.1f;
                //    if (SecondsTimer <= 0)
                //    {
                //        SecondsTimer = 0;
                //        NextAction();
                //    }
                //}
            }

            if (BattleSecondsTimer >= TIME_BETWEEN_BATTLE_UPDATES)
            {
                BattleSecondsTimer -= .1f;
                foreach (Battle battle in Battles)
                {
                    //Needs to check if player has disconnected. Should probably just end the battle in that case for now.
                    if (!NetworkManager.Singleton.ConnectedClients.Keys.Contains(battle.Player1))
                    {
                        battle.Winner = battle.Player2;
                        battle.End_Battle = true;
                    }
                    else if (battle.Player2 != SERVERID && !NetworkManager.Singleton.ConnectedClients.Keys.Contains(battle.Player2))
                    {
                        battle.Winner = battle.Player1;
                        battle.End_Battle = true;
                    }
                    else if (battle.Stage == BattleStage.BattleStarting)
                    {
                        if (NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go
                            && (battle.Player2 == SERVERID || NetworkManager.Singleton.ConnectedClients[battle.Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go))
                        {
                            NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                            if (battle.Player2 != SERVERID)
                            {
                                NetworkManager.Singleton.ConnectedClients[battle.Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                            }
                            battle.TimeTillNextAction = .1f;
                        }
                    }
                    else
                    {
                        if (battle.CurrentCreature.Owner == battle.Player1 && NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go)
                        {
                            battle.TimeTillNextAction = .1f;
                        }
                        else if (battle.Player2 != SERVERID && battle.CurrentCreature.Owner == battle.Player2 && NetworkManager.Singleton.ConnectedClients[battle.Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go)
                        {
                            battle.TimeTillNextAction = .1f;
                        }
                    }
                }

                for (int i = 0; i < Battles.Count; i++)
                {
                    UpdateBattle(Battles[i]);
                }
            }
            foreach (int BattleNumber in Battles_To_Remove)
            {
                if (BattleNumber >= 0 && BattleNumber < Battles.Count)
                {
                    NetworkManager.Singleton.ConnectedClients[Battles[BattleNumber].Player1].PlayerObject.gameObject.GetComponent<Player>().inBattle = false;
                    if (Battles[BattleNumber].Player2 != GameManager.SERVERID)
                        NetworkManager.Singleton.ConnectedClients[Battles[BattleNumber].Player2].PlayerObject.gameObject.GetComponent<Player>().inBattle = false;
                    Battles.RemoveAt(BattleNumber);
                }
            }
            Battles_To_Remove.Clear();
        }


        // Deal with target colors

        //foreach (GameObject targetHighlight in Target_Creature_Highlights)
        //{
        //    ParticleSystem.MainModule ps_main = targetHighlight.GetComponentInChildren<ParticleSystem>().main;
        //    ps_main.startColor = targetHighlight.GetComponentInChildren<Target_Script>().Color;
        //}
    }

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(25, 40, 100, 30), "Red");
    //    GUI.Label(new Rect(25, 70, 100, 30), "Green");
    //    GUI.Label(new Rect(25, 100, 100, 30), "Blue");
    //    GUI.Label(new Rect(25, 130, 100, 30), "Alpha");

    //    hSliderValueR = GUI.HorizontalSlider(new Rect(95, 45, 100, 30), hSliderValueR, 0.0F, 1.0F);
    //    hSliderValueG = GUI.HorizontalSlider(new Rect(95, 75, 100, 30), hSliderValueG, 0.0F, 1.0F);
    //    hSliderValueB = GUI.HorizontalSlider(new Rect(95, 105, 100, 30), hSliderValueB, 0.0F, 1.0F);
    //    hSliderValueA = GUI.HorizontalSlider(new Rect(95, 135, 100, 30), hSliderValueA, 0.0F, 1.0F);
    //}


    public void UpdateBattleTimer(Battle battle)
    {
        bool IsAI;
        if (battle.CurrentCreature == null || battle.CurrentCreature.Owner == GameManager.SERVERID)
        { // if CurrentCreature is null then we are still in the BattleStarting Stage of the battle and it doesn't matter if it is an AI or a Player's turn.
            IsAI = true;
        }
        else
        {
            IsAI = false;
        }
        if ((!IsAI && battle.Stage == BattleStage.ChooseAbility) || battle.Stage == BattleStage.BattleStarting)
        {
            // Also not super efficient way of doing this
            ClientRpcParams clientRpcParams;
            if (battle.Player2 == SERVERID)
            {
                clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { battle.Player1 }
                    }
                };
            } else
            {
                clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { battle.Player1, battle.Player2 }
                    }
                };
            }
            UpdateBattleTimerClientRpc("Done? (" + ((int)battle.TimeTillNextAction).ToString() + ")", clientRpcParams);
        }
    }

    public void UpdateBattle(Battle battle)
    {
        if (battle.End_Battle) // Check if the battle should end
        {
            // soon I need to revamp all of this and to use events instead of updates and at that point I should have a more official end to
            // battles where XP can be added and so on

            // for now just update hps here by setting initialized creatures in battle creatures HP to player's creature data hp
            foreach(InitializedCreatureData creature in charactersInGame[battle.Player1CharacterID].CurrentCreatureTeam)
            {
                BattleCreature battleCreature = battle.BattleCreatures.Find(battleCreature => battleCreature.ID == creature.battleCreatureID);
                if (battleCreature != null)
                    creature.CurrentHP = battleCreature.Creature.CurrentHP;
                else creature.CurrentHP = 0;
                creature.battleCreatureID = -1;
            }
            ExitBattle(battle, battle.Player1, battle.Winner);
            if (battle.Player2 != GameManager.SERVERID)
            {
                foreach (InitializedCreatureData creature in charactersInGame[battle.Player2CharacterID].CurrentCreatureTeam)
                {
                    BattleCreature battleCreature = battle.BattleCreatures.Find(battleCreature => battleCreature.ID == creature.battleCreatureID);
                    if (battleCreature != null)
                        creature.CurrentHP = battleCreature.Creature.CurrentHP;
                    else creature.CurrentHP = 0;
                    creature.battleCreatureID = -1;
                }
                ExitBattle(battle, battle.Player2, battle.Winner);
            }
            Battles_To_Remove.Add(Battles.IndexOf(battle));
        }
        else if (battle.TimeTillNextAction > 0)
        {
            battle.TimeTillNextAction -= TIME_BETWEEN_BATTLE_UPDATES;
            if (battle.TimeTillNextAction <= 0)
            {
                battle.TimeTillNextAction = 0;
                ClientRpcParams clientRpcParams;
                if (battle.Player2 == SERVERID)
                {
                    NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                    clientRpcParams = new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            TargetClientIds = new ulong[] { battle.Player1 }
                        }
                    };
                }
                else
                {
                    NetworkManager.Singleton.ConnectedClients[battle.Player1].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                    NetworkManager.Singleton.ConnectedClients[battle.Player2].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = false;
                    clientRpcParams = new ClientRpcParams
                    {
                        Send = new ClientRpcSendParams
                        {
                            TargetClientIds = new ulong[] { battle.Player1, battle.Player2 }
                        }
                    };
                }
                SetGoButtonClientRpc(false, "", clientRpcParams);
                battle.NextAction();
            }
            else
            {
                UpdateBattleTimer(battle);
            }
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void DB_SignIn_ServerRpc(ulong clientId, string email, string password)
    {
        Server.Handle_Signin_Request(clientId, email, password);
    }

    [ClientRpc]
    public void SignIn_Response_ClientRpc(int AccountId, CharacterData[] characters, ClientRpcParams rpcParams = default)
    {
        Server.Handle_Signin_Response(AccountId, characters);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SignOut_ServerRpc(ulong clientId)
    {
        Server.Handle_SignOut_Request(clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DB_CreateAccount_ServerRpc(ulong clientId, string email, string password)
    {
        Server.Handle_CreateAccount_Request(clientId, email, password);
    }

    [ClientRpc]
    public void CreateAccount_Response_ClientRpc(int responseCode, ClientRpcParams rpcParams = default)
    {
        Debug.Log("new account creation attempt returned = " + responseCode);
        Server.Handle_CreateAccount_Response(responseCode);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DB_DeleteCharacter_ServerRpc(ulong clientId, ulong id)
    {
        Server.Handle_DeleteCharacter_Request(clientId, id);
    }

    [ClientRpc]
    public void DeleteCharacter_Response_ClientRpc(bool success, ClientRpcParams rpcParams = default)
    {
        Server.Handle_DeleteCharacter_Response(success);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DB_CreateCharacter_ServerRpc(ulong clientId, string name)
    {
        Server.Handle_CreateCharacter_Request(clientId, name);
    }

    [ClientRpc]
    public void CreateCharacter_Response_ClientRpc(CharacterData character, ClientRpcParams rpcParams = default)
    {
        Debug.Log("new character = " + character.ToString());
        Server.Handle_CreateCharacter_Response(character);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnPlayer_ServerRpc(ulong clientId, ulong characterId) // later this should just be passed clientId / accountId and character ID
    {
        // Get Spawn.  Stop if there are no spawn points in the seen
        //Transform spawn = GetSpawnPoint();
        //if (spawn == null) { Debug.Log("No Spawn Points in Scene!"); return; }

        Debug.Log("In SpawnPlayerServerRpc");
        CharacterData characterData = Server.DB_GetCharacterData(clientId, characterId);

        // later this will be set based on data from the database
        //characterData.spawnPoint = spawnPoint;

        if (characterData != null)
        {
            Server.activeCharacters[clientId] = characterData;
        } else
        {
            Debug.Log("characterData is null!");
            SetInstructionText("You cannot access this character. Please contact ??? for assistance.", clientId);
        }
        
        // Spawn on Client
        GameObject playerObject = Instantiate(PlayerPrefab, new Vector3(100.5f, -88, 0), Quaternion.identity);

        Debug.Log("clientId = " + clientId);
        playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        //ulong objectId = go.GetComponent<NetworkObject>().NetworkObjectId;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        SpawnPlayer_ClientRpc(clientRpcParams);
        NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().GetComponent<Player_Movement>().MyClientId = clientId;
        NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().characterId = characterId;

        Debug.Log("Getting creatures");
        // Get creatures from the database
        List<InitializedCreatureData> creatures = Server.DB_GetPlayerCreatures(characterId);
        Debug.Log("Got Creatures: " + creatures);
        characterData.OwnedCreatures = creatures.ToArray();
        // should not necessarily get all creatures once players start having more then can fit in a starting line up
        characterData.CurrentCreatureTeam = creatures.ToArray(); // was creating a new list before. I think this will create a new array to but if not this may cause issues because it hasn't created a clone!!!
        Debug.Log("Finished SpawnPlayerServerRpc");

        charactersInGame.Add(characterId, characterData);
        Debug.Log("Finished Adding Character");
    }

    // A ClientRpc can be invoked by the server to be executed on a client
    [ClientRpc]
    private void SpawnPlayer_ClientRpc(ClientRpcParams rpcParams = default)
    {
        Player = NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId].PlayerObject.gameObject;
        //SceneManager.LoadScene("Starting_Area", LoadSceneMode.Additive);
        CharacterData characterData = Server.ClientOwnedCharacters[Server.CurrentCharacterIndex];
        LogToServerRpc(NetworkManager.Singleton.LocalClientId, characterData.Location + ", " + characterData.Position_X + ", " + characterData.Position_Y);
        //NetworkObject player = NetworkSpawnManager.SpawnedObjects[objectId];
        //SceneManager.LoadScene(characterData.Location, LoadSceneMode.Additive);
        Player.GetComponent<Player>().SetUpPlayer(characterData.Location, characterData.Position_X, characterData.Position_Y); // should get dynamically
        
    }

    ////////////////  SpawnPoint Methods  ////////////////

    /// <summary>
    /// ///needs to be in GM!!!! 
    /// Later we may need to confirm that the heal is allowed but at that point this code will likely be moved to a conversation item with an NPC that costs sole points.
    /// </summary>
    /// <param name="clientId"></param>
    [ServerRpc(RequireOwnership = false)]
    public void HealAllCreatures_ServerRpc(ulong clientId)
    {
        Debug.Log("Sending Heal Request");
        HealAllCreatures(clientId);
    }

    public void HealAllCreatures(ulong clientId)
    {
        Debug.Log("In HealAllCreatures");
        InitializedCreatureData[] creatures = charactersInGame[NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().characterId].OwnedCreatures;
        foreach (InitializedCreatureData creatureData in creatures)
        {
            InitializedCreature creature = new InitializedCreature(creatureData);
            creatureData.CurrentHP = creature.GetMaxHp();
        }
    }


    //////////////////////////////////////////////////////

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player1"></param>
    /// <param name="player2"></param>
    public void StartPvpBattle(ulong player1, ulong player2)
    {
        StartPvpBattleServerRpc(player1, player2);
    }

    [ClientRpc]
    public void StopMovingClientRpc(ulong clientId, ClientRpcParams rpcParams = default)
    {
        NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player_Movement>().IsAllowedToMove = false;
    }

    public void UpdateEncounterCreatures(InitializedCreatureData[] newEncounterCreatures, ClientDisplayActionData action, ClientRpcParams rpcParams = default)
    {
        UpdateEncounterCreaturesClientRpc(newEncounterCreatures, action, rpcParams);
    }

    [ClientRpc]
    public void UpdateEncounterCreaturesClientRpc(InitializedCreatureData[] newEncounterCreatures, ClientDisplayActionData action, ClientRpcParams rpcParams = default)
    {
        NewEncounterCreatures = newEncounterCreatures;
        clientDisplayActionData = action;
        NextAction();
    }

    public void NextAction()
    {
        List<int> Targets = clientDisplayActionData.GetTargets().ToList<int>();
        Targets.RemoveAll(target => target == -1);

        //logtext = "Targets: ";
        //foreach (int target in Targets)
        //{
        //    logtext += target + ", ";
        //}

        Vector3 NextPosition;
        if (Targets.Count > 1)
        {
            logtext = "Target[0] is " + Targets[0].ToString();
            NextPosition = CenterOfBattleMap;
        }
        else if (Targets.Count == 1)
        {
            logtext = "Target[0] is " + Targets[0].ToString();
            GameObject targetCreature = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == clientDisplayActionData.GetTargets()[0]);
            if (targetCreature != null)
            {
                Vector3 targetPosition = targetCreature.transform.position;
                if (clientDisplayActionData.GetCreatureOwner() == NetworkManager.Singleton.LocalClientId)
                {
                    NextPosition = new Vector3(targetPosition.x - 2.75f, targetPosition.y, 1);
                }
                else
                {
                    NextPosition = new Vector3(targetPosition.x + 2.75f, targetPosition.y, 1);
                }
            }
            else
            {
                logtext = "Targets is empty. ";
                NextPosition = Vector3.zero;
            }
        } else
        {
            logtext = "Targets is empty. ";
            NextPosition = Vector3.zero;
        }

        if (NextPosition != Vector3.zero)
        {
            if (BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == clientDisplayActionData.GetCurrentCreature()).transform.position != NextPosition)
            {
                logtext += "; Moving to x = " + NextPosition.x + ", y = " + NextPosition.y;
                //MoveBattleCreature(NextPosition);
                SecondsTimer = BATTLE_CREATURE_MOVE_TIME;
            }
            else
            {
                //if (clientDisplayActionData.IsLast())
                //{
                //    CreatureToReturn = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == clientDisplayActionData.GetCurrentCreature()).GetComponentInChildren<BattleCreatureClient>();
                //    ReturnBattleCreature = true;
                //}
                switch (clientDisplayActionData.GetAnimationLength())
                {
                    case AnimationLength.Short:
                        SecondsTimer = BATTLE_ANIMATION_TIME_SHORT;
                        break;
                    case AnimationLength.Normal:
                        SecondsTimer = BATTLE_ANIMATION_TIME_NORMAL;
                        break;
                    case AnimationLength.Long:
                        SecondsTimer = BATTLE_ANIMATION_TIME_LONG;
                        break;
                }
                //AnimateBattleCreature();
            }
        }
        else
        {
            // !!!!!! Got rid of EncounterCreatures, need to add this back in getting the initiallized creature from battle creatures.
            //InstructionsText.GetComponent<FadingText>().FadeInText(EncounterCreatures[clientDisplayActionData.GetCurrentCreature()].Name + " no longer has a target!", true);
        }
    }

    public void MoveBattleCreature(int creatureID, ClientRpcParams rpcParams = default)
    {
        Debug.Log("Calling MoveBattleCreatureClientRpc 1");
        MoveBattleCreatureClientRpc(creatureID, rpcParams);
    }

    [ClientRpc]
    public void MoveBattleCreatureClientRpc(int creatureID, ClientRpcParams rpcParams = default)
    {
        LogToServerRpc(NetworkManager.LocalClientId, "In MoveBattleCreatureClientRpc 1");
        BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == creatureID).GetComponentInChildren<BattleCreatureClient>().ReturnToAncher();
        LogToServerRpc(NetworkManager.LocalClientId, "Done MoveBattleCreatureClientRpc 1");
    }

    public void MoveBattleCreature(int creatureID, int targetId, ClientRpcParams rpcParams = default)
    {
        Debug.Log("Calling MoveBattleCreatureClientRpc 2");
        MoveBattleCreatureClientRpc(creatureID, targetId, rpcParams);
        Debug.Log("Done MoveBattleCreatureClientRpc 2");
    }

    [ClientRpc]
    public void MoveBattleCreatureClientRpc(int creatureID, int targetId, ClientRpcParams rpcParams = default)
    {
        BattleCreatureClient creatureToMove = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == creatureID).GetComponentInChildren<BattleCreatureClient>();
        LogToServerRpc(NetworkManager.LocalClientId, "creatureToMove = " + creatureToMove);
        Vector3 NextPosition;
        GameObject target = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == targetId);
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            if (creatureToMove.GetOwner() == NetworkManager.Singleton.LocalClientId)
            {
                NextPosition = new Vector3(targetPosition.x - 2.75f, targetPosition.y, 1);
            }
            else
            {
                NextPosition = new Vector3(targetPosition.x + 2.75f, targetPosition.y, 1);
            }
            LogToServerRpc(NetworkManager.LocalClientId, "move to = " + NextPosition.x + ", " + NextPosition.y);
            creatureToMove.Move(NextPosition);
        }
    }

    public void MoveBattleCreature(int creatureID, Vector3 NextPosition, ClientRpcParams rpcParams = default)
    {
        Debug.Log("Calling MoveBattleCreatureClientRpc 3");
        MoveBattleCreatureClientRpc(creatureID, NextPosition, rpcParams);
        Debug.Log("Done MoveBattleCreatureClientRpc 3");
    }

    [ClientRpc]
    public void MoveBattleCreatureClientRpc(int creatureID, Vector3 NextPosition, ClientRpcParams rpcParams = default)
    {
        LogToServerRpc(NetworkManager.LocalClientId, "In MoveBattleCreatureClientRpc 3");
        BattleCreatureClient creatureToMove = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == creatureID).GetComponentInChildren<BattleCreatureClient>();
        LogToServerRpc(NetworkManager.LocalClientId, "creatureToMove = " + creatureToMove);
        if (NextPosition.Equals(Vector3.zero))
        {
            creatureToMove.Move(CenterOfBattleMap);
        }
        else
        {
            creatureToMove.Move(NextPosition);
        }
    }

    public void PerformAction(InitializedCreatureData[] newEncounterCreatures, int CurrentCreatureID, ActionTargetsData targetsData, ClientRpcParams rpcParams = default)
    {
        Debug.Log("In GM.PerformAction");
        PerformActionClientRpc(newEncounterCreatures, CurrentCreatureID, targetsData, rpcParams);
    }

    [ClientRpc]
    public void PerformActionClientRpc(InitializedCreatureData[] newEncounterCreatures, int CurrentCreatureID, ActionTargetsData targetsData, ClientRpcParams rpcParams = default)
    {
        /* Next Visuals
         * 
         * Need to figure out the different vectors for where to go.
         * Need to add a script to empty creature to take a new destnation vector and move there.
         * Need to figure out how to display and animation.
         * 
         * if the creature's current location is not the same as where it should go based on the number of targets (and RANGE!!!), one means go to the target, more then one means go to the middle
         * Move to the new location
         * then display the action animations (may need to have special movement animations for some actions later)
         * if it is the last action then return to your starting spot.
         */

        LogToServerRpc(NetworkManager.LocalClientId, "In PerformActionClientRpc");

        // should handle health, death, and other changes here as well. Need to only pass impacted creature information for that.
        LogToServerRpc(NetworkManager.LocalClientId, "Getting Current Creature. CurrentCreatureID: " + CurrentCreatureID);
        GameObject currentCreature = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == CurrentCreatureID);
        LogToServerRpc(NetworkManager.LocalClientId, "Playing Attack Animation");

        bool foundTarget = false;
        foreach (int target in targetsData.PickedTargets)
        {
            if (target != GameManager.CREATURE_ID_NOT_SET)
            {
                foundTarget = true;
                LogToServerRpc(NetworkManager.LocalClientId, "Getting Target");
                GameObject targetGameObject = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == target);
                LogToServerRpc(NetworkManager.LocalClientId, "Playing Hit Animation");
                GameObject HitAnimation = Resources.Load(HIT_ANIMATION_DIRECTORY + targetsData.AnimationName) as GameObject;
                Vector2 capsuleOffset = targetGameObject.GetComponentInChildren<CapsuleCollider2D>().offset;
                Instantiate(HitAnimation, new Vector3(targetGameObject.transform.position.x - (capsuleOffset.x * targetGameObject.transform.localScale.x), targetGameObject.transform.position.y + (capsuleOffset.y * targetGameObject.transform.localScale.y), 0), Quaternion.identity);
            }
        }
        if (foundTarget)
        {
            currentCreature.GetComponent<Monster>().Attack();
        }

        //List<GameObject> objectsToDestry = new List<GameObject>();
        //EncounterCreatures.Clear();
        //Debug.Log("BattleCreatures.Count: " + BattleCreatures.Count);
        LogToServerRpc(NetworkManager.LocalClientId, "Going through newEncounterCreatures");
        for (int i = 0; i < newEncounterCreatures.Length; i++)
        {
            LogToServerRpc(NetworkManager.LocalClientId, "i = " + i);
            LogToServerRpc(NetworkManager.LocalClientId, "Getting Creature To Update");
            GameObject CreatureToUpdate = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == newEncounterCreatures[i].battleCreatureID);
            if (CreatureToUpdate != null)
            {
                LogToServerRpc(NetworkManager.LocalClientId, "CreatureToUpdate not null, name is " + CreatureToUpdate.name);
                LogToServerRpc(NetworkManager.LocalClientId, "CreatureToUpdate battleCreatureID is " + newEncounterCreatures[i].battleCreatureID);
                LogToServerRpc(NetworkManager.LocalClientId, "CreatureToUpdate ID is " + newEncounterCreatures[i].GetID());
                LogToServerRpc(NetworkManager.LocalClientId, "CreatureToUpdate HP was " + CreatureToUpdate.GetComponentInChildren<BattleCreatureClient>().InitilizedCreature.CurrentHP);
                LogToServerRpc(NetworkManager.LocalClientId, "CreatureToUpdate HP now is " + newEncounterCreatures[i].CurrentHP);
                if (newEncounterCreatures[i].CurrentHP <= 0) // Died this turn
                {
                    LogToServerRpc(NetworkManager.LocalClientId, "Removing " + CreatureToUpdate.GetComponentInChildren<BattleCreatureClient>().InitilizedCreature.Name);
                    LogToServerRpc(NetworkManager.LocalClientId, "CreatureToUpdate.GetComponent<Monster>() " + CreatureToUpdate.GetComponent<Monster>());
                    //Destroy(CreatureToUpdate.GetComponentInChildren<SelectedCreature>().gameObject);
                    BattleCreatures.Remove(CreatureToUpdate);
                    LogToServerRpc(NetworkManager.LocalClientId, "Done Removing " + CreatureToUpdate.GetComponentInChildren<BattleCreatureClient>().InitilizedCreature.Name);
                    // Needs to move this creature to a "to be destroyed" list

                    CreatureToUpdate.GetComponentInChildren<BattleCreatureClient>().Die();
                    //Destroy(CreatureToUpdate);
                    //Destroy(BattleCreatureDetailsButtons[i]);
                    //BattleCreatureDetailsButtons.RemoveAt(i);

                }
                CreatureToUpdate.GetComponentInChildren<BattleCreatureClient>().InitilizedCreature.CurrentHP = newEncounterCreatures[i].CurrentHP;
                CreatureToUpdate.GetComponentInChildren<HealthBarScript>().SetHealthPercent((float)CreatureToUpdate.GetComponentInChildren<BattleCreatureClient>().InitilizedCreature.CurrentHP
                    / (float)CreatureToUpdate.GetComponentInChildren<BattleCreatureClient>().InitilizedCreature.GetMaxHp());
            }
        }
        //Debug.Log("BattleCreatures.Count: " + BattleCreatures.Count);
        LogToServerRpc(NetworkManager.LocalClientId, "Finished PerformActionClientRpc");
    }

    [ClientRpc]
    public void IncreaseInitiativeClientRpc(int[] creatureInitiatives, int highestInitiative, int lowestInitiative, ClientRpcParams rpcParams = default)
    {
        // Highest Initiative (First Creature To Go)
        // Lowest Initiative (Last Creature To Go)
        // Set New Intiatives Bars

        if (highestInitiative != lowestInitiative)
        {
            for (int i = 0; i < BattleCreatures.Count; i++)
            {
                BattleCreatures[i].GetComponentInChildren<HealthBarScript>().SetInitiativePercent(1 - ((float)(creatureInitiatives[i] - lowestInitiative) / (float)(highestInitiative - lowestInitiative)));
            }
        } else
        {
            for (int i = 0; i < BattleCreatures.Count; i++)
            {
                BattleCreatures[i].GetComponentInChildren<HealthBarScript>().SetInitiativePercent(1);
            }
        }
    }
    public void IncreaseInitiative(int[] creatureInitiatives, int highestInitiative, int lowestInitiative, ClientRpcParams rpcParams = default)
    {
        IncreaseInitiativeClientRpc(creatureInitiatives, highestInitiative, lowestInitiative, rpcParams);
    }


    /// <summary>
    /// Initializes all the creatures for an encounter creature.
    /// Encounters have an encounter group which can have one or more encounter creatures.
    /// Each encounter creature can add 0 or more creatures to the battle but they will all be the same type of creature.
    /// </summary>
    /// <param name="creature"></param>
    /// <returns></returns>
    public List<InitializedCreatureData> InitializeNewCreatures(EncounterCreature creature)
    {
        List<InitializedCreatureData> newCreatures = new List<InitializedCreatureData>();

        Debug.Log("GM.InitializeNewCreatures");
        Debug.Log("creature.StatRangeList[PowerUpStat.XP] = " + creature.StatRangeList[PowerUpStat.XP].Low + ", " + creature.StatRangeList[PowerUpStat.XP].High);

        int NumberOfCreature = UnityEngine.Random.Range(creature.Min, creature.Max);
        for (int i = 0; i < NumberOfCreature; i++)
        {
            // Add Enemy
            InitializedCreatureData NewCreature = new InitializedCreatureData(AllCreatures[creature.CreatureName]);

            Dictionary<PowerUpStat, int> randomlyAssignedStats = new Dictionary<PowerUpStat, int>();
            for (int statCount = 0; statCount < creature.StatRangeList.Count; statCount++)
            {
                randomlyAssignedStats.Add(creature.StatRangeList.Keys.ElementAt(statCount), UnityEngine.Random.Range(creature.StatRangeList.ElementAt(statCount).Value.Low, creature.StatRangeList.ElementAt(statCount).Value.High));
                if (randomlyAssignedStats.ElementAt(statCount).Key.Equals(PowerUpStat.XP))
                    NewCreature.CurrentXP = randomlyAssignedStats.ElementAt(statCount).Value;
                Debug.Log("New Stat: " + randomlyAssignedStats.ElementAt(statCount).Key + " gets " + randomlyAssignedStats.ElementAt(statCount).Value);
            }
            // will need some version of this once we add more stats
            //NewCreature.UpdatePowerupStats(randomlyAssignedStats);

            newCreatures.Add(NewCreature);
        }

        // needs to pass stat to track list boosts. Create the creature normally then boost it's stats the same way you would after a battle. Need a function for this!

        Debug.Log("newCreatures.Count: " + newCreatures.Count);

        return newCreatures;
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartPvpBattleServerRpc(ulong player1, ulong player2)
    {
        // should really be handled when the challenged player accepts but we will leave it here for now.
        NetworkManager.Singleton.ConnectedClients[player1].PlayerObject.gameObject.GetComponent<Player>().inBattle = true;
        NetworkManager.Singleton.ConnectedClients[player2].PlayerObject.gameObject.GetComponent<Player>().inBattle = true;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { player1 }
            }
        };
        StopMovingClientRpc(player1, clientRpcParams);
        CharacterData player1CharacterData = charactersInGame[NetworkManager.Singleton.ConnectedClients[player1].PlayerObject.gameObject.GetComponent<Player>().characterId];
        CharacterData player2CharacterData = charactersInGame[NetworkManager.Singleton.ConnectedClients[player2].PlayerObject.gameObject.GetComponent<Player>().characterId];
        StartBattleClientRpc(player1CharacterData.CurrentCreatureTeam.ToArray(), player2CharacterData.CurrentCreatureTeam.ToArray(), "Other Player", clientRpcParams); // BattleStartingCreatures

        clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { player2 }
            }
        };
        StopMovingClientRpc(player2, clientRpcParams);
        StartBattleClientRpc(player2CharacterData.CurrentCreatureTeam.ToArray(), player1CharacterData.CurrentCreatureTeam.ToArray(), "Other Player", clientRpcParams); // BattleStartingCreatures

        Battles.Add(new Battle(player1, player1CharacterData.ID, player1CharacterData.CurrentCreatureTeam.ToArray(), player2, player2CharacterData.ID, player2CharacterData.CurrentCreatureTeam.ToArray()));
    }


    public void StartClientEncounter(ulong clientId, InitializedCreatureData[] PassedEnemyCreatures, string EnemyName)
    {
        Debug.Log("In StartClientEncounter");
        Debug.Log("clientId = " + clientId);
        Debug.Log("My network ID is " + NetworkManager.Singleton.LocalClientId);

        StopMovingClientRpc(clientId);
        Debug.Log("My characterId ID is " + NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().characterId);
        CharacterData playerCharacterData = charactersInGame[NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().characterId];

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        Debug.Log("NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().Squad_Starting = " + playerCharacterData.CurrentCreatureTeam);
        Debug.Log("PassedEnemyCreatures.Length " + PassedEnemyCreatures.Length);

        // Probably should send this from in the battle and pass the IDs with it.
        StartBattleClientRpc(playerCharacterData.CurrentCreatureTeam.ToArray(), PassedEnemyCreatures, EnemyName, clientRpcParams); // BattleStartingCreatures
        Battles.Add(new Battle(clientId, playerCharacterData.ID, playerCharacterData.CurrentCreatureTeam.ToArray(), SERVERID, SERVERID, PassedEnemyCreatures));

    }


    [ClientRpc]
    public void StartBattleClientRpc(InitializedCreatureData[] PlayerCreatures, InitializedCreatureData[] EnemyCreatures, string enemyName, ClientRpcParams rpcParams = default) //InitializedCreatureData[] PassedEnemyCreatures
    {
        LogToServerRpc(NetworkManager.LocalClientId, "StartBattleClientRpc");
        LogToServerRpc(NetworkManager.LocalClientId, "PlayerCreatures.Length = " + PlayerCreatures.Length);
        LogToServerRpc(NetworkManager.LocalClientId, "EnemyCreatures.Length = " + EnemyCreatures.Length);

        Player.GetComponent<Player>().inBattle = true;
        //EncounterCreatures.Clear();

        Battle_Action_Panel.SetActive(true);
        //Battle_Friendly_Icon_Panel.SetActive(true);
        //Battle_Enemy_Icon_Panel.SetActive(true);

        Battle_Player_1_Name.text = "Player 1"; // Will be players actual name once we have the database set up.
        Battle_Player_2_Name.text = enemyName;

        //LogToServerRpc(NetworkManager.LocalClientId, "After setting initial veriables");

        Show_Battle_Details();

        //LogToServerRpc(NetworkManager.LocalClientId, "After Show_Battle_Details()");

        for (int i = 0; i < PlayerCreatures.Length; i++)
        {
            //Vector3 creatureLocation = position_manager.FRIENDLY_BATTLE_POSITION_MEDIUM[i] + position_manager.MEDIUM_SPRITE_OFFSET + GameManager.BATTLE_OFFSET;
            if (PlayerCreatures[i].CurrentHP > 0)
            {
                bool success = SetUpBattleCreature(PlayerCreatures[i], NetworkManager.LocalClientId);
                if (!success)
                {
                    break;
                }
            }
        }
        LogToServerRpc(NetworkManager.LocalClientId, "Finished adding player creatures.");

        for (int i = 0; i < EnemyCreatures.Length; i++)
        {
            //Vector3 creatureLocation = position_manager.ENEMY_BATTLE_POSITION_MEDIUM[i] + position_manager.MEDIUM_SPRITE_OFFSET + GameManager.BATTLE_OFFSET;
            if (EnemyCreatures[i].CurrentHP > 0)
            {
                bool success = SetUpBattleCreature(EnemyCreatures[i], GameManager.SERVERID);
                if (!success)
                {
                    break;
                }
            }
        }

        LogToServerRpc(NetworkManager.LocalClientId, "Finished adding enemy creatures.");

        Text[] texts = Battle_GO_Button.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            if (text.name.Equals("Go_Button_Info_Text"))
            {
                text.text = "Pre-Battle Delay";
            }
        }

        Battle_GO_Button.gameObject.SetActive(true);

        //foreach (InitializedCreatureData creature in BattleStartingCreatures)
        //{
        //    Debug.Log("Inside Loop! creature: " + creature);
        //    if (creature != null && creature.Name != null)
        //    {
        //        Debug.Log("creature.Name: " + creature.Name);
        //        EncounterCreature5 = new InitializedCreature(creature);
        //    }
        //}


        //string name = PassedEnemyCreatures.Name;
        //Debug.Log("The creatuer's name is " + name);
        //EncounterCreature5 = new InitializedCreature(AllCreatures.Find(thisCreature => thisCreature.Name.Equals(name)));



        // Later we need to deal with multiple enemies.
        LogToServerRpc(NetworkManager.LocalClientId, "Out of StartBattleClientRpc");

    }

    public bool SetUpBattleCreature(InitializedCreatureData creatureData, ulong Owner)
    {
        LogToServerRpc(NetworkManager.LocalClientId, "In SetUpBattleCreature");
        InitializedCreature new_InitilizedCreature = new InitializedCreature(creatureData);

        LogToServerRpc(NetworkManager.LocalClientId, "Creature Initialized");

        Vector3 position;
        if (Owner == NetworkManager.LocalClientId)
        {
            position = position_manager.GetPosition(BattleCreatures.Count, new_InitilizedCreature.Size, 0);
        }
        else
        {
            position = position_manager.GetPosition(BattleCreatures.Count, new_InitilizedCreature.Size, 1);
        }


        if (position != Vector3.negativeInfinity)
        {
            LogToServerRpc(NetworkManager.LocalClientId, "Valid position");

            string BaseImagePath = ("BattleCreature/" + new_InitilizedCreature.Path);

            //Sprite creature_sprite;
            //if (Owner == NetworkManager.LocalClientId)
            //{
            //    creature_sprite = Resources.Load<Sprite>(BaseImagePath + "_Friendly");
            //}
            //else
            //{
            //    creature_sprite = Resources.Load<Sprite>(BaseImagePath + "_Enemy");
            //}

            //Vector2 sprite_size = creature_sprite.rect.size;
            //Vector2 local_sprite_size = sprite_size / creature_sprite.pixelsPerUnit;
            //Vector3 spritePosition = new Vector3(position.x - (local_sprite_size.x / 2) + position_manager.MEDIUM_SPRITE_OFFSET.x, position.y + (local_sprite_size.y / 2) + position_manager.MEDIUM_SPRITE_OFFSET.y, 0);//MEDIUM_SPRITE_OFFSET
            //LogToServerRpc(NetworkManager.LocalClientId, "position (" + position.x + ", " + position.y + ")");
            //LogToServerRpc(NetworkManager.LocalClientId, "spritePosition (" + spritePosition.x + ", " + spritePosition.y + ")");

            LogToServerRpc(NetworkManager.LocalClientId, "Before Instantiate");
            GameObject BattleCreature = Instantiate(Resources.Load(BaseImagePath)) as GameObject;
            LogToServerRpc(NetworkManager.LocalClientId, "After Instantiate");
            if (Owner == NetworkManager.LocalClientId)
            {
                Vector3 newScale = BattleCreature.transform.localScale;
                newScale.x *= -1;
                BattleCreature.transform.localScale = newScale;
            }

            BattleCreatureClient battleCreatureClient = BattleCreature.AddComponent<BattleCreatureClient>();
            battleCreatureClient.InitilizedCreature = new_InitilizedCreature;
            LogToServerRpc(NetworkManager.LocalClientId, "After add battle creature client");
            //InitializedCreature InitializedCreature = BattleCreature.GetComponent<InitializedCreature>();
            //InitializedCreature = new_InitilizedCreature;

            Vector3 offset = position_manager.GetSpriteOffset(new_InitilizedCreature.Size);
            Vector2 capsuleOffset = BattleCreature.GetComponentInChildren<CapsuleCollider2D>().offset;
            LogToServerRpc(NetworkManager.LocalClientId, "Position Set: " + position.x + ", " + position.y);
            LogToServerRpc(NetworkManager.LocalClientId, "offset: " + offset.x + ", " + offset.y);
            LogToServerRpc(NetworkManager.LocalClientId, "capsuleOffset: " + capsuleOffset.x + ", " + capsuleOffset.y);
            LogToServerRpc(NetworkManager.LocalClientId, "BattleCreature.transform.localScale: " + BattleCreature.transform.localScale.x + ", " + BattleCreature.transform.localScale.y);
            Vector3 creaturePosition = new Vector3(position.x - (capsuleOffset.x * BattleCreature.transform.localScale.x) - offset.x, position.y - (capsuleOffset.y * BattleCreature.transform.localScale.y) - offset.y, 0);
            BattleCreature.transform.position = creaturePosition;
            LogToServerRpc(NetworkManager.LocalClientId, "After set position");

            //BattleCreature.GetComponent<SpriteRenderer>().sprite = creature_sprite;

            //LogToServerRpc(NetworkManager.LocalClientId, "New Creature " + EncounterCreature1.Name);
            //LogToServerRpc(NetworkManager.LocalClientId, "Creature has " + EncounterCreature1.HPMultiplier + " HPMultiplier.");
            //LogToServerRpc(NetworkManager.LocalClientId, "Creature has " + EncounterCreature1.GetMaxHp() + " Max HP.");
            //LogToServerRpc(NetworkManager.LocalClientId, "Creature has " + EncounterCreature1.CurrentHP + " CurrentHP.");
            //LogToServerRpc(NetworkManager.LocalClientId, "Creature Level = " + EncounterCreature1.CurrentLvl);

            //Player.GetComponent<PlayerMovement_Fluid>().IsAllowedToMove = false;

            // Get CreatureInfo From SQL

            //Friendly_Creature_1_Icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(BaseImagePath + "_Icon");
            //Friendly_Creature_1_Icon.SetActive(true);
            //Friendly_Creature_1_ShortName.GetComponent<Text>().text = EncounterCreature1.ShortName;
            //Friendly_Creature_1_ShortName.SetActive(true);
            battleCreatureClient.SetAncher(creaturePosition);
            battleCreatureClient.SetId(BattleCreatures.Count);
            
            Debug.Log("New Creature " + creatureData.Name);
            Debug.Log("creatureData ID " + creatureData.GetID());
            Debug.Log("creatureData battleCreatureID " + creatureData.battleCreatureID);
            Debug.Log("new_InitilizedCreature ID " + new_InitilizedCreature.GetID());
            Debug.Log("battleCreatureClient ID " + battleCreatureClient.ID);

            battleCreatureClient.SetOwner(Owner);

            Vector2 healthBarSize = new Vector2();
            foreach (SpriteRenderer renderer in HealthBar.GetComponentsInChildren<SpriteRenderer>())
            {
                if (renderer.gameObject.name.Equals("HealthBarBackground"))
                {
                    healthBarSize = (renderer.sprite.rect.size / renderer.sprite.pixelsPerUnit) * HealthBar.transform.localScale * position_manager.HealthBarScale(new_InitilizedCreature.Size) / 2; // need to mutiply this by health bar scale if we scale the health bar later
                }
            }

            Vector2 detailsButtonSize = (Creature_Details_Button.GetComponent<SpriteRenderer>().sprite.rect.size / Creature_Details_Button.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit) * Creature_Details_Button.transform.localScale;
            //float detailsButtonSizeY = ((position.y - (detailsButtonSize.y / 2)) > (position.y - (capsuleOffset.y * BattleCreature.transform.localScale.y) - offset.y + detailsButtonSize.y * .2f) ? (position.y - (detailsButtonSize.y / 2)) : (position.y - (capsuleOffset.y * BattleCreature.transform.localScale.y) - offset.y + detailsButtonSize.y * .2f));
            float detailsButtonY_1 = position.y - (detailsButtonSize.y / 2);
            LogToServerRpc(NetworkManager.LocalClientId, "detailsButtonY_1 = " + detailsButtonY_1);
            float detailsButtonY_2 = ((capsuleOffset.y + BattleCreature.GetComponentInChildren<CapsuleCollider2D>().size.y / 2) * BattleCreature.transform.localScale.y) + detailsButtonSize.y * 1.2f;
            LogToServerRpc(NetworkManager.LocalClientId, "detailsButtonY_2 = " + detailsButtonY_2);
            //float detailsButtonY = detailsButtonY_2 > detailsButtonY_1 ? detailsButtonY_1 : detailsButtonY_2;
            float detailsButtonY = detailsButtonY_1;
            LogToServerRpc(NetworkManager.LocalClientId, "detailsButtonY = " + detailsButtonY);
            Vector3 detailsButtonPosition = new Vector3(position.x + (detailsButtonSize.x / 2), detailsButtonY, 0);

            float healthBarPositionY_1 = position.y - (offset.y * 2) + healthBarSize.y;
            float healthBarPositionY_2 = position.y - (capsuleOffset.y * BattleCreature.transform.localScale.y) - (offset.y * 1.5f);
            //float healthBarPositionY = healthBarPositionY_2 > healthBarPositionY_1 ? healthBarPositionY_1 : healthBarPositionY_2;
            float healthBarPositionY = healthBarPositionY_1;
            Vector3 healthBarPosition = new Vector3(position.x + healthBarSize.x, healthBarPositionY, 0);
            //Vector3 healthBarPosition = new Vector3(position.x + healthBarSize.x, position.y - (offset.y * 2) + healthBarSize.y, 0);

            GameObject CreatureName = Instantiate(Resources.Load("CreatureName")) as GameObject;
            CreatureName.GetComponentInChildren<MeshRenderer>().sortingOrder = BattleCreature.layer;
            CreatureName.transform.parent = BattleCreature.transform;
            string nameToUse = "";
            if (new_InitilizedCreature.NickName != null) // sould also check the battle detail level for this. Battle detail should also determine if the text is white or black.
            {
                nameToUse = new_InitilizedCreature.NickName;
            }
            else
            {
                nameToUse = new_InitilizedCreature.ShortName;
            }

            BattlePositionSize positionSize = Position.GetPositionSize(new_InitilizedCreature.Size);
            if (positionSize == BattlePositionSize.Small)
            {
                CreatureName.transform.position = new Vector3(position.x + (detailsButtonSize.x * 1.2f), detailsButtonY + detailsButtonSize.y * .5f, 0);
                CreatureName.GetComponent<TextMesh>().text = "Lvl: " + InitializeCreatures.XpToLevel(new_InitilizedCreature.CurrentXP);
                GameObject CreatureName2 = Instantiate(Resources.Load("CreatureName")) as GameObject;
                CreatureName2.GetComponentInChildren<MeshRenderer>().sortingOrder = BattleCreature.layer;
                CreatureName2.transform.position = new Vector3(position.x - offset.x * .5f, healthBarPosition.y + (CreatureName.GetComponent<MeshRenderer>().bounds.size.y * 1.1f), 0);
                //CreatureName2.transform.position = new Vector3(position.x, position.y - (CreatureName.GetComponent<MeshRenderer>().bounds.size.y * 2.1f), 0);
                CreatureName2.transform.parent = BattleCreature.transform;
                CreatureName2.GetComponent<TextMesh>().text = nameToUse;
            }
            else if (positionSize == BattlePositionSize.Smallest)
            {
                CreatureName.transform.position = new Vector3(position.x + (detailsButtonSize.x * 1.2f), position.y, 0);
                CreatureName.GetComponent<TextMesh>().text = "Lvl: " + InitializeCreatures.XpToLevel(new_InitilizedCreature.CurrentXP);
                healthBarPosition = new Vector3(healthBarPosition.x, healthBarPosition.y + (offset.y * .2f), healthBarPosition.z);
            }
            else
            {
                CreatureName.transform.position = new Vector3(position.x + (detailsButtonSize.x * 1.2f), position.y, 0);
                CreatureName.GetComponent<TextMesh>().text = "Lvl: " + InitializeCreatures.XpToLevel(new_InitilizedCreature.CurrentXP) + " " + nameToUse;
            }
            
            GameObject EncounterCreature_HealthBar = Instantiate(HealthBar, healthBarPosition, Quaternion.identity) as GameObject;
            EncounterCreature_HealthBar.transform.localScale = new Vector3(EncounterCreature_HealthBar.transform.localScale.x * position_manager.HealthBarScale(new_InitilizedCreature.Size), EncounterCreature_HealthBar.transform.localScale.y * position_manager.HealthBarScale(new_InitilizedCreature.Size), 1);
            LogToServerRpc(NetworkManager.LocalClientId, "new_InitilizedCreature.CurrentHP: " + new_InitilizedCreature.CurrentHP);
            LogToServerRpc(NetworkManager.LocalClientId, "new_InitilizedCreature.GetMaxHp(): " + new_InitilizedCreature.GetMaxHp());
            EncounterCreature_HealthBar.GetComponent<HealthBarScript>().SetHealthPercent((float)new_InitilizedCreature.CurrentHP / (float)new_InitilizedCreature.GetMaxHp());
            EncounterCreature_HealthBar.transform.SetParent(BattleCreature.transform);
            LogToServerRpc(NetworkManager.LocalClientId, "Getting Details Button");
            GameObject Details_Button = Instantiate(Creature_Details_Button, detailsButtonPosition, Quaternion.identity) as GameObject;
            Details_Button.transform.SetParent(BattleCreature.transform);
            BattleCreatures.Add(BattleCreature);
            //BattleCreatureDetailsButtons.Add(Details_Button);
            //Details_Button.GetComponent<SelectedCreature>().SetCreatureNumber(battleCreatureClient.ID);
            LogToServerRpc(NetworkManager.LocalClientId, "returning true");
            return true; // creature successfully added
        } else
        {
            return false;// couldn't find a spot to put the creature
        }

    }

    [ClientRpc]
    public void SetGoButtonClientRpc(bool SetTo, string newText, ClientRpcParams rpcParams = default)
    {
        Text[] texts = Battle_GO_Button.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            if (text.name.Equals("Go_Button_Info_Text"))
            {
                text.text = newText;
            }
        }

        Battle_GO_Button.gameObject.SetActive(SetTo);
    }

    public void Set_GO(bool SetTo)
    {
        Text[] texts = Battle_GO_Button.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            if (text.name.Equals("Go_Button_Info_Text"))
            {
                text.text = "Waiting on Opponent";
            }
            else if (text.name.Equals("Go_Button_Timer_Text"))
            {
                text.text = "";
            }
        }
        InstructionsText.GetComponent<FadingText>().FadeOutText();
        Battle_GO_Button.gameObject.SetActive(!SetTo);
        Set_GOServerRpc(NetworkManager.Singleton.LocalClientId, SetTo);
    }

    [ServerRpc(RequireOwnership = false)]
    public void Set_GOServerRpc(ulong clientId, bool SetTo)
    {
        Debug.Log("Got Go Request");
        NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject.GetComponent<Player>().Battle_Go = SetTo;
    }

    public void Run()
    {
        RunServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RunServerRpc(ulong clientId)
    {
        //Debug.Log("Got a Run Request.");
        //Debug.Log("Got a Run Request.");
        //Debug.Log("clientId " + clientId);
        //Debug.Log("Battles[0] " + Battles[0]);
        //Debug.Log("Battles[0] Player1 " + Battles[0].Player1);
        //Debug.Log("Battles[0] Player2 " + Battles[0].Player2);
        //Debug.Log("Battle = " + Battles.Find(battle => battle.Player1 == clientId || battle.Player2 == clientId));
        Battle battle = Battles.Find(battle => battle.Player1 == clientId || battle.Player2 == clientId);
        if (battle != null)
        {
            //Debug.Log("Found the battle");
            battle.Run(clientId);
            //while (battle.Battle_Thread != null && battle.Battle_Thread.IsAlive)
            //{
            //    Debug.Log("End_Battle: " + Battles.Find(battle => battle.Player1 == clientId || battle.Player2 == clientId).End_Battle);
            //}
            //Debug.Log("Battle is over!");
        }

    }

    public void ExitBattle(Battle battle, ulong clientID, ulong Winner)
    {
        Debug.Log("in ExitBattle, Client " + clientID + " exiting Battle.");
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID }
            }
        };

        // Still needs to add XP and other stat increases.
        Close_AbilityPick_Panel();
        // Add XP and other stats here
        Player player = NetworkManager.Singleton.ConnectedClients[clientID].PlayerObject.gameObject.GetComponent<Player>();
        CharacterData character = charactersInGame[player.characterId];
        bool needHealing = false;
        bool respawnNotNeeded = false;

        foreach (InitializedCreatureData creatureData in character.CurrentCreatureTeam)
        {
            if (creatureData.CurrentHP == 0)
            {
                // at least one creature is hurt so healing buttons should become active
                needHealing = true;
            } else
            {
                // at least one creature is not dead so there is no need to respawn the character
                respawnNotNeeded = true;
                
                InitializedCreature creature = new InitializedCreature(creatureData);
                if (creatureData.CurrentHP < creature.GetMaxHp())
                {
                    // at least one creature is hurt so healing buttons should become active
                    needHealing = true;
                }

                // No need to keep looping through creatures if we have already found the info we need.
                if (needHealing && respawnNotNeeded)
                    break;
            }

        }

        // respawn is needed
        if (!respawnNotNeeded)
        {
            // all creatures in current team are dead
            // Later it needs to get the spawn point from the character data (and the database) and load a new scene if the spawn point is not in the currently loaded scene.
            // healed creatures and didn't crash server but didn't move player. Also server crashed when player disconnected while in battle but that might be a thing to resolve after the refactor to move code out of update
            Debug.Log("player.gameObject.transform.position = " + player.gameObject.transform.position);
            ChangeMapLocation_ClientRpc("Starting_Area", 100, -87, clientRpcParams);
            Debug.Log("player.gameObject.transform.position = " + player.gameObject.transform.position);
            Server.activeCharacters[clientID].Position_X = 100;
            Server.activeCharacters[clientID].Position_Y = -87;
            Debug.Log("Done changing activeCharacter position");
            //player.gameObject.transform.position = new Vector3(100, -87, 0); //GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<SpawnPoint>().SpawnPosition;
            HealAllCreatures(clientID);
        }

        Debug.Log("before ExitBattleClientRpc");
        ExitBattleClientRpc(Winner, needHealing, clientRpcParams);
        Debug.Log("Finished ExitBattle");
    }

    [ClientRpc]
    public void ExitBattleClientRpc(ulong Winner, bool needHealing, ClientRpcParams rpcParams = default)
    {
        // TODO should destroy prviously created creatures
        Hide_Battle_Details();

        Player.GetComponent<Player_Movement>().IsAllowedToMove = true;
        // for using Host not Server only
        if (Player.GetComponent<PlayerMovement_Fluid>() != null)
        {
            Player.GetComponent<PlayerMovement_Fluid>().IsAllowedToMove = true;
        }

        Player.GetComponent<Player>().inBattle = false;
        //BattleCamera.enabled = false;
        Battle_Action_Panel.SetActive(false);
        //Battle_Friendly_Icon_Panel.SetActive(false);
        //Battle_Enemy_Icon_Panel.SetActive(false);
        CloseCreatureDetails();

        foreach (GameObject gameObject in BattleCreatures)
        {
            Destroy(gameObject);

        }
        BattleCreatures.Clear();

        //foreach (GameObject gameObject in BattleCreatureDetailsButtons)
        //{
        //    Destroy(gameObject);

        //}
        //BattleCreatureDetailsButtons.Clear();

        Battle_GO_Button.gameObject.SetActive(false);
        ClearSelectedCreatureHighlights();
        ClearTargets();
        position_manager.ClearIds();

        //Selected_Creature_Highlight.gameObject.SetActive(false);
        if (NetworkManager.Singleton.LocalClientId == Winner)
        {
            InstructionsText.GetComponent<FadingText>().FadeInText("You Won the battle!!!", true);
        }
        else
        {
            InstructionsText.GetComponent<FadingText>().FadeInText("You Lost the battle : (", true);
        }

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        foreach (GameObject spawnPointObject in spawnPoints)
            spawnPointObject.GetComponent<SpawnPoint>().HealButton.SetActive(needHealing);

    }

    public void SetInstructionText(string text, ulong clientId)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        SetInstructionText(text, true, clientRpcParams);
    }

    public void SetInstructionText(string text, bool AutoClose, ClientRpcParams rpcParams = default)
    {
        SetInstructionTextClientRpc(text, AutoClose, rpcParams);
    }

    [ClientRpc]
    public void SetInstructionTextClientRpc(string text, bool AutoClose, ClientRpcParams rpcParams = default)
    {
        InstructionsText.GetComponent<FadingText>().FadeInText(text, AutoClose);
    }

    [ClientRpc]
    public void UpdateBattleTimerClientRpc(string newText, ClientRpcParams rpcParams = default)
    {
        Text[] texts = Battle_GO_Button.GetComponentsInChildren<Text>();
        foreach(Text text in texts)
        {
            if (text.name.Equals("Go_Button_Timer_Text"))
            {
                text.text = newText;
            }
        }
        //Player.GetComponent<Player>().Battle_Go = false;
        //Battle_GO_Button.gameObject.SetActive(true);
    }

    public void OpenInfoPanel(GameObject SelectedPanel)
    {
        LogToServerRpc(NetworkManager.LocalClientId, "OpenInfoPanel");
        if (Player != null)
        {
            Player.GetComponent<Player_Movement>().IsAllowedToMove = false;
        }

        if (SelectedPanel.Equals(CreaturesPanel))
        {
            SetUpSelectionPanel();
        }

        SelectedPanel.GetComponent<RectTransform>().SetAsLastSibling();
        SelectedPanel.SetActive(true);
        Detail_Panel_Close_Button.GetComponent<RectTransform>().SetAsLastSibling();
        Detail_Panel_Close_Button.SetActive(true);
        // TODO set text and other details
    }

    public void SetUpSelectionPanel()
    {
        Debug.Log("Opening SetUpSelectionPanel");
        foreach (Button button in Creatures_Panel_ScrollViewContent.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }

        foreach (InitializedCreatureData creatureData in Server.ClientOwnedCharacters[Server.CurrentCharacterIndex].CurrentCreatureTeam)
        {
            Debug.Log("Found Creature " + creatureData.Name);
            Button selectCreatureButton = Instantiate(SelectCreatureButton);

            selectCreatureButton.transform.parent = Creatures_Panel_ScrollViewContent.transform;
            selectCreatureButton.transform.localScale = Vector3.one;
            InitializedCreature creature = new InitializedCreature(creatureData);
            selectCreatureButton.onClick.AddListener(delegate {
                OpenCreatureDetails(creature);
            });

            Image[] images = selectCreatureButton.GetComponentsInChildren<Image>();
            foreach(Image image in images)
            {
                if (image.gameObject.name.Equals("HealthBarImage"))
                {
                    // do healthbar stuff
                } else if (image.gameObject.name.Equals("CreatureImage"))
                {
                    // set creature image
                    Debug.Log("Setting Creature Image to " + "BattleCreature\\" + creature.Path + "Image");
                    image.sprite = Resources.Load<Sprite>("BattleCreature\\" + creature.Path + "Image");
                }
            }
            TMP_Text[] texts = selectCreatureButton.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text text in texts)
            {
                if (text.gameObject.name.Equals("NickNameText"))
                {
                    Debug.Log("creatureData.NickName " + creatureData.NickName);
                    Debug.Log("creature.ShortName " + creature.ShortName);
                    string nickName = creatureData.NickName.Equals("") ? creature.ShortName : creatureData.NickName;
                    Debug.Log("Setting Nickname to " + nickName);
                    text.text = nickName;
                }
                else if (text.gameObject.name.Equals("NameText"))
                {
                    Debug.Log("Setting Name to " + creatureData.Name);
                    text.text = creatureData.Name;
                }
                else if (text.gameObject.name.Equals("PowerText"))
                {
                    // need to decide how to calculate this first
                    Debug.Log("Setting PowerText to " + "???");
                    text.text = "???";
                }
                else if (text.gameObject.name.Equals("LevelText"))
                {
                    Debug.Log("Setting LevelText to " + InitializeCreatures.XpToLevel(creatureData.CurrentXP).ToString());
                    text.text = InitializeCreatures.XpToLevel(creatureData.CurrentXP).ToString();
                }
            }
        }
        //    // !!!!!!!!!!!!!!!!!!!!!!!!! BROKEN: This needs to get the starting squad from the server or we need to create it on both. !!!!!!!!!!!!!!!!!!!!!!!!
        //    LogToServerRpc(NetworkManager.LocalClientId, "SetUpSelectionPanel");
        //    LogToServerRpc(NetworkManager.LocalClientId, "Player.gameObject.GetComponent<Player>().Squad_Starting[0].Name: " + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[0].Name);
        //    if (Player.gameObject.GetComponent<Player>().CurrentCreatureTeam.Count > 0)
        //    {
        //        Squad_Starting_1_Button.gameObject.SetActive(true);
        //        Squad_Starting_1_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("BattleCreatureImages/" + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[0].Name + "/" + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[0].Name + "_Enemy");
        //    } else
        //    {
        //        Squad_Starting_1_Button.gameObject.SetActive(false);
        //    }
        //    if (Player.gameObject.GetComponent<Player>().CurrentCreatureTeam.Count > 1)
        //    {
        //        Squad_Starting_2_Button.gameObject.SetActive(true);
        //        Squad_Starting_2_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("BattleCreatureImages/" + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[1].Name + "/" + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[1].Name + "_Enemy");
        //    }
        //    else
        //    {
        //        Squad_Starting_2_Button.gameObject.SetActive(false);
        //    }
        //    if (Player.gameObject.GetComponent<Player>().CurrentCreatureTeam.Count > 2)
        //    {
        //        Squad_Starting_3_Button.gameObject.SetActive(true);
        //        Squad_Starting_3_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("BattleCreatureImages/" + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[2].Name + "/" + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[2].Name + "_Enemy");
        //    }
        //    else
        //    {
        //        Squad_Starting_3_Button.gameObject.SetActive(false);
        //    }
        //    if (Player.gameObject.GetComponent<Player>().CurrentCreatureTeam.Count > 3)
        //    {
        //        Squad_Starting_4_Button.gameObject.SetActive(true);
        //        Squad_Starting_4_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("BattleCreatureImages/" + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[3].Name + "/" + Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[3].Name + "_Enemy");
        //    }
        //    else
        //    {
        //        Squad_Starting_4_Button.gameObject.SetActive(false);
        //    }


        //    //if (Player.gameObject.GetComponent<Player>().Squad_Subs.Count > 0)
        //    //{
        //    //    Squad_Subs_1_Button.gameObject.SetActive(true);
        //    //    Squad_Subs_1_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("BattleCreatureImages/" + Player.gameObject.GetComponent<Player>().Squad_Subs[0].Name + "/" + Player.gameObject.GetComponent<Player>().Squad_Subs[0].Name + "_Enemy");
        //    //}
        //    //else
        //    //{
        //    //    Squad_Subs_1_Button.gameObject.SetActive(false);
        //    //}
        //    //if (Player.gameObject.GetComponent<Player>().Squad_Subs.Count > 1)
        //    //{
        //    //    Squad_Subs_2_Button.gameObject.SetActive(true);
        //    //    Squad_Subs_2_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("BattleCreatureImages/" + Player.gameObject.GetComponent<Player>().Squad_Subs[1].Name + "/" + Player.gameObject.GetComponent<Player>().Squad_Subs[1].Name + "_Enemy");
        //    //}
        //    //else
        //    //{
        //    //    Squad_Subs_2_Button.gameObject.SetActive(false);
        //    //}
        //    //if (Player.gameObject.GetComponent<Player>().Squad_Subs.Count > 2)
        //    //{
        //    //    Squad_Subs_3_Button.gameObject.SetActive(true);
        //    //    Squad_Subs_3_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("BattleCreatureImages/" + Player.gameObject.GetComponent<Player>().Squad_Subs[2].Name + "/" + Player.gameObject.GetComponent<Player>().Squad_Subs[2].Name + "_Enemy");
        //    //}
        //    //else
        //    //{
        //    //    Squad_Subs_3_Button.gameObject.SetActive(false);
        //    //}
        //    //if (Player.gameObject.GetComponent<Player>().Squad_Subs.Count > 3)
        //    //{
        //    //    Squad_Subs_4_Button.gameObject.SetActive(true);
        //    //    Squad_Subs_4_Button.GetComponent<Image>().sprite = Resources.Load<Sprite>("BattleCreatureImages/" + Player.gameObject.GetComponent<Player>().Squad_Subs[3].Name + "/" + Player.gameObject.GetComponent<Player>().Squad_Subs[3].Name + "_Enemy");
        //    //}
        //    //else
        //    //{
        //    //    Squad_Subs_4_Button.gameObject.SetActive(false);
        //    //}

    }

    public void ClosePanels()
    {
        //Debug.Log("MainCanvise.transform.childCount: " + MainCanvise.transform.childCount);
        //Debug.Log("Creature_Details_Background_Panel.transform.hierarchyCount: " + Creature_Details_Background_Panel.transform.GetSiblingIndex());
        if (!(MainCanvise.transform.childCount - 2 == Creature_Details_Background_Panel.transform.GetSiblingIndex()))
        {
            SettingsPanel.SetActive(false);
            MapPanel.SetActive(false);
            InventoryPanel.SetActive(false);
            CreaturesPanel.SetActive(false);
            DiaryPanel.SetActive(false);
            Detail_Panel_Close_Button.SetActive(false);
            if (Player != null)
            {
                Player.GetComponent<Player_Movement>().IsAllowedToMove = true;
            }
        } else
        {
            Creature_Details_Background_Panel.SetActive(false);
            if (SettingsPanel.activeSelf)
            {
                SettingsPanel.GetComponent<RectTransform>().SetAsLastSibling();
                Detail_Panel_Close_Button.GetComponent<RectTransform>().SetAsLastSibling();
            }
            else if (MapPanel.activeSelf)
            {
                MapPanel.GetComponent<RectTransform>().SetAsLastSibling();
                Detail_Panel_Close_Button.GetComponent<RectTransform>().SetAsLastSibling();
            }
            else if (InventoryPanel.activeSelf)
            {
                InventoryPanel.GetComponent<RectTransform>().SetAsLastSibling();
                Detail_Panel_Close_Button.GetComponent<RectTransform>().SetAsLastSibling();
            }
            else if (CreaturesPanel.activeSelf)
            {
                CreaturesPanel.GetComponent<RectTransform>().SetAsLastSibling();
                Detail_Panel_Close_Button.GetComponent<RectTransform>().SetAsLastSibling();
            }
            else if (DiaryPanel.activeSelf)
            {
                DiaryPanel.GetComponent<RectTransform>().SetAsLastSibling();
                Detail_Panel_Close_Button.GetComponent<RectTransform>().SetAsLastSibling();
            }
            else
            {
                Detail_Panel_Close_Button.SetActive(false);
            }
        }
    }

    //public void DisplayStartingCreatureDetails(int StartingCreatureCount) // should we just be storing Initialized Creatures rather then storing Initialized Create Data?
    //{
    //    LogToServerRpc(NetworkManager.LocalClientId, "DisplayStartingCreatureDetails");

    //    switch (StartingCreatureCount)
    //    {
    //        case 0:
    //            OpenCreatureDetails(new InitializedCreature(Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[0]));
    //            break;
    //        case 1:
    //            OpenCreatureDetails(new InitializedCreature(Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[1]));
    //            break;
    //        case 2:
    //            OpenCreatureDetails(new InitializedCreature(Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[2]));
    //            break;
    //        case 3:
    //            OpenCreatureDetails(new InitializedCreature(Player.gameObject.GetComponent<Player>().CurrentCreatureTeam[3]));
    //            break;
    //    }
    //}

    public void DisplaySubstitutionCreatureDetails(int StartingCreatureCount) // should we just be storing Initialized Creatures rather then storing Initialized Create Data?
    {
        //switch (StartingCreatureCount)
        //{
        //    case 0:
        //        OpenCreatureDetails(new InitializedCreature(Player.gameObject.GetComponent<Player>().Squad_Subs[0]));
        //        break;
        //    case 1:
        //        OpenCreatureDetails(new InitializedCreature(Player.gameObject.GetComponent<Player>().Squad_Subs[1]));
        //        break;
        //    case 2:
        //        OpenCreatureDetails(new InitializedCreature(Player.gameObject.GetComponent<Player>().Squad_Subs[2]));
        //        break;
        //    case 3:
        //        OpenCreatureDetails(new InitializedCreature(Player.gameObject.GetComponent<Player>().Squad_Subs[3]));
        //        break;
        //}
    }


    public void EncounterCreatureClicked(int CreatureNumber)
    {
        LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Got Selected Creature Click: " + CreatureNumber);
        EncounterCreatureClickedServerRpc(NetworkManager.Singleton.LocalClientId, CreatureNumber);
    }

    [ServerRpc(RequireOwnership = false)]
    public void EncounterCreatureClickedServerRpc(ulong clientId, int CreatureNumber)
    {
        Debug.Log("In EncounterCreatureClickedServerRpc, CreatureNumber is " + CreatureNumber);
        //needs to pass the following to the client
        //  CreatureNumber
        //  Is clicked creature owned by player and picking an ability
        //  List of abilities of clicked creature
        //      For each we will need to know: the name of the ability, is the ability currently selected, is the ability not available and if so then why? (could be on cool down or need to much mana for example)

        // may want to put battles into a dictionary later if we have a lot of them. Use player names as keys?
        Battle battle = Battles.Find(battle => battle.Player1 == clientId || battle.Player2 == clientId);
        if (battle != null)
        {
            Debug.Log("battle is not null");
            BattleCreature battleCreature = battle.BattleCreatures.Find(creature => creature.ID == CreatureNumber);
            if (battleCreature != null)
            {
                Debug.Log("battleCreature is not null");
                battle.EncounterCreatureClickedServerRpc(clientId, CreatureNumber);

                //Debug.Log("In EncounterCreatureClickedServerRpc");
                //Debug.Log("CreatureNumber is " + CreatureNumber);
                //Debug.Log("Player1SelectedCreature " + battle.Player1SelectedCreature);

                // !!!!!!!!!!!!!!!!!!!!!! Keeps getting an Object reference not set to an instance of an object error here at random times !!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // Don't need to pass this if we decide not to change back to sending the actual speed of abilities (after agility calculations and what not).
                int WaitSpeed = battle.GetInitiative(battleCreature, AllAbilities.GetAbility(AbilityName.Wait));
                Debug.Log("WaitSpeed Set");

                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { clientId }
                    }
                };
                bool AllowAbilityPick = false;
                if (battle.CurrentCreature != null) // if it is null then the battle has not started yet.
                {
                    bool InChooseAbilityStage = battle.Stage == BattleStage.ChooseAbility;
                    bool ClientIsCreatureOwner = battle.CurrentCreature.Owner == clientId;
                    bool IsCreaturesTurn = battleCreature.Equals(battle.CurrentCreature);
                    AllowAbilityPick = InChooseAbilityStage && ClientIsCreatureOwner && IsCreaturesTurn;
                }
                Debug.Log("bools set");

                Display_AbilityPick_PanelClientRpc(CreatureNumber, AllowAbilityPick, battleCreature.Creature.KnownAbilities.ToArray(), WaitSpeed, clientRpcParams);
            }
        }
        Debug.Log("Finished EncounterCreatureClickedServerRpc");

    }

    public void Reload_AbilityPick_Panel(ulong clientId, int CreatureNumber, bool AllowAbilityPick, AbilityData[] abilites, int WaitSpeed)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        //Debug.Log("GM in Reload_AbilityPick_Panel");
        //Debug.Log("AllowAbilityPick " + AllowAbilityPick);
        Close_AbilityPick_Panel_ClientRpc(clientRpcParams);
        Display_AbilityPick_PanelClientRpc(CreatureNumber, AllowAbilityPick, abilites, WaitSpeed, clientRpcParams);
    }

    [ClientRpc]
    public void Display_AbilityPick_PanelClientRpc(int CreatureNumber, bool AllowAbilityPick, AbilityData[] abilites, int WaitSpeed, ClientRpcParams rpcParams = default) // Should pass a list of ablitiy objects instead of strings so other infor can be passed for each ability
    {
        // Set Dails button on click to -> OpenCreatureDetails(EncounterCreature#);
        // 
        // Wait button might be able to be generic, just set current creature's next ability to wait
        // create ability buttons
        //  Buttons should set the text color based on if the ability name matches the clicked creture's next ability (later we might do something fancier like change the look of the button.
        //  Buttons should only be clickable if the clicked creature is owned by the player who clicked on it and it is the clicked creature's turn to pick an ability.
        //  Buttons should set the clicked creature's next ability (which should result in switching to picking targets) and hide the ability pick panel.

        //Ability_Button_Prefab.transform.parent = AbilityPick_ScrollView_Content.transform;
        //Ability_Button_Prefab.transform.SetParent(AbilityPick_ScrollView_Content.transform, false);
        Selected_Creature = CreatureNumber;

        RectTransform rt = (RectTransform)Ability_Button_Prefab.transform;

        for (int i = 0; i < abilites.Length; i++)
        {
            AbilityButtons.Add(Instantiate(Ability_Button_Prefab, new Vector3(((i % 2) * (rt.rect.width + 25)) - 180, (-(int)((i / 2) * (rt.rect.height + 10))) + 95, 0), Quaternion.identity) as GameObject);
            AbilityButtons[i].GetComponentInChildren<Text>().text = abilites[i].DisplayName + " (" + abilites[i].Speed + ")";
            AbilityButtons[i].transform.SetParent(AbilityPick_ScrollView_Content.transform, false);
            if (AllowAbilityPick)
            {
                AbilityData ability = abilites[i];
                AbilityButtons[i].GetComponent<Button>().onClick.AddListener(delegate {
                    Text[] texts = Battle_GO_Button.GetComponentsInChildren<Text>();
                    foreach (Text text in texts)
                    {
                        if (text.name.Equals("Go_Button_Info_Text"))
                        {
                            text.text = ability.AbilityName.ToString();
                        }
                    }
                    PickNextAbilityServerRpc(NetworkManager.Singleton.LocalClientId, ability);
                    Close_AbilityPick_Panel_NotifyServer();
                });
            } else
            {
                AbilityButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        if (AllowAbilityPick)
        {
            Ability_Pick_WaitButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            Ability_Pick_WaitButton.GetComponent<Button>().interactable = false;
        }
        Ability_Pick_WaitButton.GetComponentInChildren<Text>().text = "Wait (30)";
        // Set Ability_Pick_WaitButton and Ability_Pick_DetailsButton
        Ability_Pick_Panel.gameObject.SetActive(true);

    }

    [ClientRpc]
    public void EndTurnClientRpc(ClientRpcParams rpcParams = default)
    {
        ClearSelectedCreatureHighlights();
        //Selected_Creature_Highlight.gameObject.SetActive(false);
        ClearTargets();
    }

    public void ClearSelectedCreatureHighlights()
    {
        foreach (GameObject gameObject in Selected_Creature_Highlights)
        {
            gameObject.transform.parent.GetComponentInChildren<BattleCreatureClient>().acting = false;
            Destroy(gameObject);
        }
        Selected_Creature_Highlights.Clear();
    }

    public void ClearTargets()
    {
        foreach (GameObject highlight in Target_Creature_Highlights)
        {
            highlight.transform.parent.GetComponentInChildren<BattleCreatureClient>().target = false;
            Destroy(highlight);
        }
        Target_Creature_Highlights.Clear();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PickNextAbilityServerRpc(ulong clientId, AbilityData NewNextAbility)
    {
        Battle battle = Battles.Find(battle => battle.Player1 == clientId || battle.Player2 == clientId);

        //Debug.Log("NewNextAbility.AbilityName " + NewNextAbility.AbilityName.ToString());
        //Debug.Log("battle.CurrentCreature.NextAbility " + battle.CurrentCreature.NextAbility.DisplayName);
        battle.RequestTargets(NewNextAbility);
        //Debug.Log("battle.CurrentCreature.NextAbility " + battle.CurrentCreature.NextAbility.DisplayName);
    }

    public void SetSelectedCrature(int creatureId, CreatureSize size, ClientRpcParams rpcParams = default)
    {
        Debug.Log("creatureId: " + creatureId);
        Debug.Log("size: " + size);
        SetSelectedCratureClientRpc(creatureId, size, rpcParams);
    }

    [ClientRpc]
    public void SetSelectedCratureClientRpc(int creatureId, CreatureSize size, ClientRpcParams rpcParams = default)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Ability_Pick_Panel.gameObject.SetActive(false);
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Close_AbilityPick_Panel();
        //if (Selected_Creature_Highlight == null)
        //{
        //    Selected_Creature_Highlight = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Selected_Creature_Highlight;
        //}

        //Debug.Log("Selected_Creature_Highlight.GetCreatureNumber() " + Selected_Creature_Highlight.creatureId.GetComponent<SelectedCreature>().GetCreatureNumber());
        GameObject BattleCreature = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == creatureId);
        Vector2 capsuleOffset = BattleCreature.GetComponentInChildren<CapsuleCollider2D>().offset;
        Vector3 creature_position = new Vector3(BattleCreature.transform.position.x + (capsuleOffset.x * BattleCreature.transform.localScale.x), BattleCreature.transform.position.y + (capsuleOffset.y * BattleCreature.transform.localScale.y), 1);

        Selected_Creature_Highlights.Add(Instantiate(Creature_Highlight, new Vector3(creature_position.x, creature_position.y, 0), Quaternion.identity) as GameObject);
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1] " + Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1]);
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1].GetComponent<SpriteRenderer>() " + Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1].GetComponentInChildren<SpriteRenderer>());
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1].GetComponent<SpriteRenderer>().sortingOrder " + Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1].GetComponentInChildren<SpriteRenderer>().sortingOrder);
        //Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1].GetComponentInChildren<SpriteRenderer>().sortingOrder = -100;
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1].GetComponent<SpriteRenderer>().sortingOrder " + Selected_Creature_Highlights[Selected_Creature_Highlights.Count - 1].GetComponentInChildren<SpriteRenderer>().sortingOrder);

        SelectedCreature newScript = Selected_Creature_Highlights[0].GetComponentInChildren<CapsuleCollider2D>().gameObject.AddComponent<SelectedCreature>() as SelectedCreature;
        Selected_Creature_Highlights[0].transform.parent = BattleCreature.transform;
        BattleCreature.GetComponentInChildren<BattleCreatureClient>().acting = true;
        //newScript.SetCreatureNumber(creatureId);
        //Selected_Creature_Highlight.gameObject.GetComponentInChildren<SelectedCreature>().SetCreatureNumber(creatureId);
        //Debug.Log("Selected_Creature_Highlight.GetCreatureNumber() " + Selected_Creature_Highlight.gameObject.GetComponent<SelectedCreature>().GetCreatureNumber());


        // Set highlight here
        //GameObject Selected_Creature_Highlight = Instantiate(Resources.Load("Selected_Enemy_Highlight") as GameObject, EncounterCreature.transform.position, Quaternion.identity) as GameObject;

        float Highlight_Scale = Battle.GetHighilghtSize(size);

        ParticleSystem particleSystem = Selected_Creature_Highlights[0].GetComponentInChildren<ParticleSystem>();
        particleSystem.transform.localScale = new Vector3(Highlight_Scale, Highlight_Scale, 1);
        particleSystem.GetComponent<Renderer>().sortingOrder = -199;
        //Selected_Creature_Highlights[0].gameObject.transform.localScale = new Vector3(Highlight_Scale, Highlight_Scale, 1f);
        //Selected_Creature_Highlight_Small.gameObject.transform.position = new Vector3(Selected_Creature_Highlight_Small.gameObject.transform.position.x, Selected_Creature_Highlight_Small.gameObject.transform.position.y, Selected_Creature_Highlight_Small.gameObject.transform.position.z);


        Text[] texts = Battle_GO_Button.GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            if (text.name.Equals("Go_Button_Info_Text"))
            {
                text.text = "Wait";
            }
        }
        Battle_GO_Button.gameObject.SetActive(true);
        // if creature is player owned then create ability pick dropdown.
    }

    public void TargetCreatureClicked(ulong clientId, int TargetNumber)
    {
        TargetCreatureClickedServerRpc(clientId, TargetNumber);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TargetCreatureClickedServerRpc(ulong clientId, int TargetNumber)
    {
        Debug.Log("Got Target Click!, Target Number is " + TargetNumber);
        Battle battle = Battles.Find(battle => battle.Player1 == clientId || battle.Player2 == clientId);
        if (battle != null)
        {
            battle.TargetClicked(TargetNumber);
        }
    }

    public void PickNextTargets(ActionTargetsData data, ClientRpcParams rpcParams = default)
    {
        PickNextTargetsClientRpc(data, rpcParams);
    }

    [ClientRpc]
    public void PickNextTargetsClientRpc(ActionTargetsData data, ClientRpcParams rpcParams = default)
    {
        // needs some way of still viewing creature stats and stuff once an ability is chosen.
        // still need to send a message to the server adding the target in the CurrentCreature.NextAbility
        LogToServerRpc(NetworkManager.Singleton.LocalClientId, "In PickNextTargetsClientRpc");
        ClearTargets();
        LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Total Valid Targets = " + data.ValidTargets.Length);
        for (int i = 0; i < data.ValidTargets.Length; i++)
        {
            LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Finding Next Target");
            GameObject ValidTargetGameObject = BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().GetId() == data.ValidTargets[i]);

            if (ValidTargetGameObject != null)
            {
                LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Got new target, ValidTargetGameObject != null");
                Vector2 capsuleOffset = ValidTargetGameObject.GetComponentInChildren<CapsuleCollider2D>().offset;
                Vector3 creature_position = new Vector3(ValidTargetGameObject.transform.position.x + (capsuleOffset.x * ValidTargetGameObject.transform.localScale.x), ValidTargetGameObject.transform.position.y + (capsuleOffset.y * ValidTargetGameObject.transform.localScale.y), 1);
                GameObject NewTarget = Instantiate(Creature_Highlight, new Vector3(creature_position.x, creature_position.y, 0), Quaternion.identity) as GameObject;
                Target_Script newScript = NewTarget.GetComponentInChildren<CapsuleCollider2D>().gameObject.AddComponent<Target_Script>();

                NewTarget.transform.parent = ValidTargetGameObject.transform;
                ValidTargetGameObject.GetComponentInChildren<BattleCreatureClient>().target = true;

                ParticleSystem particleSystem = NewTarget.GetComponentInChildren<ParticleSystem>();
                float Highlight_Scale = Battle.GetHighilghtSize(ValidTargetGameObject.GetComponentInChildren<BattleCreatureClient>().InitilizedCreature.Size);
                //ParticleSystem particleSystem = Selected_Creature_Highlights[0].GetComponentInChildren<ParticleSystem>();
                particleSystem.transform.localScale = new Vector3(Highlight_Scale, Highlight_Scale, 1);
                particleSystem.GetComponent<Renderer>().sortingOrder = -199;


                //ParticleSystem.MainModule ps_Main = NewTarget.GetComponentInChildren<ParticleSystem>().main;
                //ps_Main.startLifetime = Highlight_Scale;
                //NewTarget.transform.localScale = new Vector3(Highlight_Scale, Highlight_Scale, 1f);
                //NewTarget.GetComponentInChildren<Target_Script>().SetCreatureNumber(data.ValidTargets[i]);
                Target_Creature_Highlights.Add(NewTarget);
                LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Target Added");
            }
            else
            {
                LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Got new target, ValidTargetGameObject == null");
                GameObject NewTarget = Instantiate(Creature_Highlight, new Vector3(BattleCreatures[0].transform.position.x, BattleCreatures[0].transform.position.y, 0), Quaternion.identity) as GameObject;
                //NewTarget.GetComponentInChildren<Target_Script>().SetCreatureNumber(data.ValidTargets[i]);
                //Target_Script newScript = NewTarget.GetComponentInChildren<CapsuleCollider2D>().gameObject.AddComponent<Target_Script>();
                //newScript.SetCreatureNumber(data.ValidTargets[i]);
                ParticleSystem particleSystem = NewTarget.GetComponentInChildren<ParticleSystem>();
                float Highlight_Scale = .5f;
                particleSystem.transform.localScale = new Vector3(Highlight_Scale, Highlight_Scale, 1);
                //ParticleSystem.MainModule ps_Main = NewTarget.GetComponentInChildren<ParticleSystem>().main;
                //ps_Main.startLifetime = Highlight_Scale;
                Target_Creature_Highlights.Add(NewTarget);
                LogToServerRpc(NetworkManager.Singleton.LocalClientId, "Target Added");
            }
        }
        UpdateTargets(data);
    }

    public void UpdateTargets_Server(ActionTargetsData data, ClientRpcParams rpcParams = default)
    {
        Debug.Log("in GM UpdateTargets_Server");
        //Debug.Log("Updating Targets");
        //foreach (int target in data.ValidTargets)
        //{
        //    Debug.Log("Got Valid Target " + target);
        //}
        //foreach (int target in data.PickedTargets)
        //{
        //    Debug.Log("Got Picked Target " + target);
        //}
        UpdateTargetsClientRpc(data, rpcParams);
    }

    [ClientRpc]
    public void UpdateTargetsClientRpc(ActionTargetsData data, ClientRpcParams rpcParams = default)
    {
        LogToServerRpc(NetworkManager.LocalClientId, "In GM, UpdateTargetsClientRpc");
        UpdateTargets(data);
    }

    public void UpdateTargets(ActionTargetsData data)
    {
        LogToServerRpc(NetworkManager.LocalClientId, "In GM, UpdateTargets");
        Color targetColor;
        for (int i = 0; i < data.ValidTargets.Length; i++)
        {
            targetColor = Color.grey;
            ParticleSystem.MainModule ps_Main = Target_Creature_Highlights[i].GetComponentInChildren<ParticleSystem>().main;
            if (!data.PickedTargets.Contains(-1))
            {
                if (data.PickedTargets.Contains(data.ValidTargets[i]))
                {
                    if (data.TargetType == TargetType.Negative)
                    {
                        LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " in if, gets TARGETED_NEGATIVE_COLOR = " + TARGETED_NEGATIVE_COLOR.r + ", " + TARGETED_NEGATIVE_COLOR.g + ", " + TARGETED_NEGATIVE_COLOR.b + ", " + TARGETED_NEGATIVE_COLOR.a);
                        targetColor = TARGETED_NEGATIVE_COLOR;
                    }
                    else
                    {
                        LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " in if, gets TARGETED_POSITIVE_COLOR = " + TARGETED_POSITIVE_COLOR.r + ", " + TARGETED_POSITIVE_COLOR.g + ", " + TARGETED_POSITIVE_COLOR.b + ", " + TARGETED_POSITIVE_COLOR.a);
                        targetColor = TARGETED_POSITIVE_COLOR;
                    }
                }
            }
            else
            {
                if (data.TargetType == TargetType.Negative)
                {
                    if (data.PickedTargets.Contains(data.ValidTargets[i]))
                    {
                        LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " in else, gets TARGETED_NEGATIVE_COLOR = " + TARGETED_NEGATIVE_COLOR.r + ", " + TARGETED_NEGATIVE_COLOR.g + ", " + TARGETED_NEGATIVE_COLOR.b + ", " + TARGETED_NEGATIVE_COLOR.a);
                        targetColor = TARGETED_NEGATIVE_COLOR;
                    }
                    else
                    {
                        LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " in else, gets TARGET_NEGATIVE_COLOR = " + TARGET_NEGATIVE_COLOR.r + ", " + TARGET_NEGATIVE_COLOR.g + ", " + TARGET_NEGATIVE_COLOR.b + ", " + TARGET_NEGATIVE_COLOR.a);
                        targetColor = TARGET_NEGATIVE_COLOR;
                    }
                }
                else
                {
                    if (data.PickedTargets.Contains(data.ValidTargets[i]))
                    {
                        LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " in else, gets TARGETED_POSITIVE_COLOR = " + TARGETED_POSITIVE_COLOR.r + ", " + TARGETED_POSITIVE_COLOR.g + ", " + TARGETED_POSITIVE_COLOR.b + ", " + TARGETED_POSITIVE_COLOR.a);
                        targetColor = TARGETED_POSITIVE_COLOR;
                    }
                    else
                    {
                        LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " in else, gets TARGET_POSITIVE_COLOR = " + TARGET_POSITIVE_COLOR.r + ", " + TARGET_POSITIVE_COLOR.g + ", " + TARGET_POSITIVE_COLOR.b + ", " + TARGET_POSITIVE_COLOR.a);
                        targetColor = TARGET_POSITIVE_COLOR;
                    }
                }
            }

            LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " has color = " + targetColor.r + ", " + targetColor.g + ", " + targetColor.b + ", " + targetColor.a);
            if (!targetColor.Equals(Color.gray))
            {
                LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " is active");
                //ps_Main.startColor = new Color(targetColor.r, targetColor.g, targetColor.b, 255);
                ps_Main.startColor = new ParticleSystem.MinMaxGradient(new Color(targetColor.r, targetColor.g, targetColor.b, 255));
                LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " color is " + ps_Main.startColor.color);
                Target_Creature_Highlights[i].SetActive(true);
            } else
            {
                LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " is NOT active");
                LogToServerRpc(NetworkManager.LocalClientId, data.ValidTargets[i] + " color is " + ps_Main.startColor.color);
                Target_Creature_Highlights[i].SetActive(false);
            }

        }

    }

    [ClientRpc]
    public void Close_AbilityPick_Panel_ClientRpc(ClientRpcParams rpcParams = default)
    {
        Close_AbilityPick_Panel();
    }

    public void Close_AbilityPick_Panel()
    {
        foreach (GameObject gameObject in AbilityButtons)
        {
            Destroy(gameObject);
        }
        AbilityButtons.Clear();
        Ability_Pick_Panel.gameObject.SetActive(false);
    }

    public void Close_AbilityPick_Panel_NotifyServer()
    {
        foreach (GameObject gameObject in AbilityButtons)
        {
            Destroy(gameObject);
        }
        AbilityButtons.Clear();
        Ability_Pick_Panel.gameObject.SetActive(false);
        Close_AbilityPick_Panel_ServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    public void Close_AbilityPick_Panel_ServerRpc(ulong clientId)
    {
        Battle battle = Battles.Find(battle => battle.Player1 == clientId || battle.Player2 == clientId);
        battle.Close_AbilityPick_Panel(clientId);
    }


    public void DisplayEncounterCreatureDetails()
    {
        
        OpenCreatureDetails(BattleCreatures.Find(creature => creature.GetComponentInChildren<BattleCreatureClient>().ID == Selected_Creature).GetComponentInChildren<BattleCreatureClient>().InitilizedCreature);
        //switch (Selected_Creature)
        //{
        //    case 1:
        //        OpenCreatureDetails(EncounterCreature1);
        //        break;
        //    case 2:
        //        OpenCreatureDetails(EncounterCreature2);
        //        break;
        //    case 3:
        //        OpenCreatureDetails(EncounterCreature3);
        //        break;
        //    case 4:
        //        OpenCreatureDetails(EncounterCreature4);
        //        break;
        //    case 5:
        //        OpenCreatureDetails(EncounterCreature5);
        //        break;
        //    case 6:
        //        OpenCreatureDetails(EncounterCreature6);
        //        break;
        //    case 7:
        //        OpenCreatureDetails(EncounterCreature7);
        //        break;
        //    case 8:
        //        OpenCreatureDetails(EncounterCreature8);
        //        break;
        //}
    }

    public void OpenCreatureDetails(InitializedCreature SelectedCreature) // IdentifyCreature just sets which creature was clicked and should show it's details
    {
        LogToServerRpc(NetworkManager.LocalClientId, "OpenCreatureDetails");
        BaseCreature baseCreature = AllCreatures[SelectedCreature.Name];

        //int MaxHpForBattle = (int)Mathf.Floor(SelectedCreature.MaxHP * SelectedCreature.HPMultiplier); // need to move this, I'm no longer sure why I have this.
        Creature_Details_Name_Text.GetComponent<Text>().text = SelectedCreature.Name;
        Creature_Details_HP_Text.GetComponent<Text>().text = SelectedCreature.CurrentHP + " / " + SelectedCreature.GetMaxHp();

        Creature_Details_Types_Text.GetComponent<Text>().text = "";
        //foreach (CreatureType type in EnemyCreature1.Types)
        //{
        //    Battle_Creature_Types_Text.GetComponent<Text>().text += EnemyCreature1.Name;
        //}

        Creature_Details_Rating_Text.GetComponent<Text>().text = SelectedCreature.Rating.ToString();
        Creature_Details_Lvl_Text.GetComponent<Text>().text = InitializeCreatures.XpToLevel(SelectedCreature.CurrentXP).ToString();
        Creature_Details_Str_Text.GetComponent<Text>().text = SelectedCreature.GetTotalStrength().ToString();
        Creature_Details_Agi_Text.GetComponent<Text>().text = SelectedCreature.GetTotalAgility().ToString();
        Creature_Details_Mnd_Text.GetComponent<Text>().text = SelectedCreature.Mind.ToString();
        Creature_Details_Wil_Text.GetComponent<Text>().text = SelectedCreature.GetTotalWill().ToString();
        Creature_Details_Size_Text.GetComponent<Text>().text = SelectedCreature.Size.ToString();
        Creature_Details_Armour_Text.GetComponent<Text>().text = (SelectedCreature.Armor != 0 ? SelectedCreature.Armor.ToString() : "");
        Creature_Details_General_RES_Text.GetComponent<Text>().text = (SelectedCreature.General_Resistance != 0 ? SelectedCreature.General_Resistance.ToString() : "");
        Creature_Details_Fire_RES_Text.GetComponent<Text>().text = (SelectedCreature.Fire_Resistance != 0 ? SelectedCreature.Fire_Resistance.ToString() : "");
        Creature_Details_Water_RES_Text.GetComponent<Text>().text = (SelectedCreature.Water_Resistance != 0 ? SelectedCreature.Water_Resistance.ToString() : "");
        Creature_Details_Poison_RES_Text.GetComponent<Text>().text = (SelectedCreature.Poison_Resistance != 0 ? SelectedCreature.Poison_Resistance.ToString() : "");
        Creature_Details_Electric_RES_Text.GetComponent<Text>().text = (SelectedCreature.Electric_Resistance != 0 ? SelectedCreature.Electric_Resistance.ToString() : "");
        Creature_Details_Death_RES_Text.GetComponent<Text>().text = (SelectedCreature.Death_Resistance != 0 ? SelectedCreature.Death_Resistance.ToString() : "");

        // I think I want it to adjust the text size based on the percent but I will add that later.
        string typeText = "";
        if (baseCreature.CreatureTypePercents.Fire > 0)
        {
            // Orange Text
            typeText += "Fire (" + baseCreature.CreatureTypePercents.Fire + ")";
        }
        if (baseCreature.CreatureTypePercents.Earth > 0)
        {
            // Green Text
            typeText += (!typeText.Equals("") ? ", ": "") + "Earth (" + baseCreature.CreatureTypePercents.Earth + ")";
        }
        if (baseCreature.CreatureTypePercents.Air > 0)
        {
            // Grey Text (or White with an outline)
            typeText += (!typeText.Equals("") ? ", " : "") + "Air (" + baseCreature.CreatureTypePercents.Air + ")";
        }
        if (baseCreature.CreatureTypePercents.Water > 0)
        {
            // Blue Text
            typeText += (!typeText.Equals("") ? ", " : "") + "Water (" + baseCreature.CreatureTypePercents.Water + ")";
        }
        if (baseCreature.CreatureTypePercents.Life > 0)
        {
            // Yellow / Gold Text
            typeText += (!typeText.Equals("") ? ", " : "") + "Life (" + baseCreature.CreatureTypePercents.Life + ")";
        }
        if (baseCreature.CreatureTypePercents.Death > 0)
        {
            // Black Text
            typeText += (!typeText.Equals("") ? ", " : "") + "Death (" + baseCreature.CreatureTypePercents.Death + ")";
        }
        if (baseCreature.CreatureTypePercents.Blood > 0)
        {
            // Red Text
            typeText += (!typeText.Equals("") ? ", " : "") + "Blood (" + baseCreature.CreatureTypePercents.Blood + ")";
        }
        if (baseCreature.CreatureTypePercents.Arcane > 0)
        {
            // Purple Text
            typeText += (!typeText.Equals("") ? ", " : "") + "Arcane (" + baseCreature.CreatureTypePercents.Arcane + ")";
        }

        Creature_Details_Types_Text.GetComponent<Text>().text = typeText;

        Creature_Details_Attributes_Dropdown.ClearOptions();
        List<Dropdown.OptionData> DropDownData = new List<Dropdown.OptionData>();

        // Probably want to replace this with power up groups
        //DropDownData.Add(new Dropdown.OptionData("Attributes"));
        //foreach (AttributeName attribute in SelectedCreature.CurrentAttributes)
        //{
        //    DropDownData.Add(new Dropdown.OptionData(attribute.ToString()));
        //}
        //Creature_Details_Attributes_Dropdown.AddOptions(DropDownData);

        Creature_Details_Abilities_Dropdown.ClearOptions();
        DropDownData = new List<Dropdown.OptionData>();

        DropDownData.Add(new Dropdown.OptionData("Abilities"));
        foreach (AbilityData ability in SelectedCreature.KnownAbilities)
        {
            DropDownData.Add(new Dropdown.OptionData(ability.AbilityName.ToString()));
        }
        Creature_Details_Abilities_Dropdown.AddOptions(DropDownData);

        ChangeToCreatureDescriptionPanel(SelectedCreature);

        OpenInfoPanel(Creature_Details_Background_Panel);
    }

    public void CloseCreatureDetails()
    {
        Creature_Details_Background_Panel.SetActive(false);
    }

    public void ChangeToCreatureDescriptionPanel(InitializedCreature SelectedCreature)
    {
        Creature_Details_Description_Panel.SetActive(true);
        Creature_Details_Attributes_Panel.SetActive(false);
        Creature_Details_Abilities_Panel.SetActive(false);

        Debug.Log("BattleCreature/" + SelectedCreature.Name.Replace(" ", "") + "/" + SelectedCreature.Name.Replace(" ", "") + "Image");
        Creature_Details_Image.sprite = Resources.Load<Sprite>("BattleCreature/" + SelectedCreature.Name.Replace(" ", "") + "/" + SelectedCreature.Name.Replace(" ", "") + "Image");
        Creature_Details_Description.text = SelectedCreature.Description;
    }

    //public void OpenAttributeDescriptionPanel()
    //{
    //    Attribute SelectedAttribute = new Attribute(AttributeName.Tough);

    //    Creature_Details_Description_Panel.SetActive(false);
    //    Creature_Details_Attributes_Panel.SetActive(true);
    //    Creature_Details_Abilities_Panel.SetActive(false);

    //    Creature_Details_Attribute_Description.text = SelectedAttribute.Description;
    //}
    //public void OpenAbilityDescriptionPanel()
    //{
    //    Attribute SelectedAttribute = new Attribute(AttributeName.Tough);

    //    Creature_Details_Description_Panel.SetActive(false);
    //    Creature_Details_Attributes_Panel.SetActive(true);
    //    Creature_Details_Abilities_Panel.SetActive(false);

    //    Creature_Details_Attribute_Description.text = SelectedAttribute.Description;
    //}

    public void Show_Battle_Details()
    {
        if (Player_Side_Toggle == 0) // dark
        {
            PlayerSideDark.SetActive(true);
            PlayerSideLight.SetActive(false);
            Battle_Player_1_Name.transform.parent.gameObject.SetActive(true);
            Battle_Player_1_Name.gameObject.SetActive(true);
            Battle_Player_2_Name.gameObject.SetActive(true);
        }
        else if (Player_Side_Toggle == 1) // light
        {
            PlayerSideDark.SetActive(false);
            PlayerSideLight.SetActive(true);
            Battle_Player_1_Name.transform.parent.gameObject.SetActive(true);
            Battle_Player_1_Name.gameObject.SetActive(true);
            Battle_Player_2_Name.gameObject.SetActive(true);
        }
        else // None
        {
            PlayerSideDark.SetActive(false);
            PlayerSideLight.SetActive(false);
            Battle_Player_1_Name.transform.parent.gameObject.SetActive(true);
            Battle_Player_1_Name.gameObject.SetActive(false);
            Battle_Player_2_Name.gameObject.SetActive(false);
        }
    }

    public void Hide_Battle_Details()
    {
        PlayerSideDark.SetActive(false);
        PlayerSideLight.SetActive(false);
        Battle_Player_1_Name.transform.parent.gameObject.SetActive(false);
    }

    public void Toggle_PlayerSides()
    {
        if (Player_Side_Toggle < 2) // Was dark now set to light
        {
            Player_Side_Toggle++;
        }
        else // was none go back to dark
        {
            Player_Side_Toggle = 0;
        }
        Show_Battle_Details();
    }

    public void RemoveScene(string Scene_Name)
    {
        SceneManager.UnloadSceneAsync(Scene_Name);
    }

    public void SetInitialMapLocation(string Scene_Name, float x, float y)
    {
        LogToServerRpc(NetworkManager.LocalClientId, "In ChangeScene");
        LogToServerRpc(NetworkManager.LocalClientId, "Scene_Name: " + Scene_Name);
        SceneManager.LoadScene(Scene_Name, LoadSceneMode.Additive);
        LogToServerRpc(NetworkManager.LocalClientId, "Loaded sceneToLoad");
        //ChangeSceneServerRpc(NetworkManager.LocalClientId, Scene_Name, x, y);
        Player.gameObject.transform.position = new Vector3(x, y, y);
        LogToServerRpc(NetworkManager.LocalClientId, "Moved Player");
        NetworkManager.Singleton.ConnectedClients[NetworkManager.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().currentLocation = Scene_Name;
    }

    [ClientRpc]
    public void ChangeMapLocation_ClientRpc(string Scene_Name, float x, float y, ClientRpcParams rpcParams = default)
    {
        ChangeMapLocation(Scene_Name, x, y);
    }

    public void ChangeMapLocation(string Scene_Name, float x, float y)
    {
        LogToServerRpc(NetworkManager.LocalClientId, "In ChangeScene");
        string previousLocation = NetworkManager.Singleton.ConnectedClients[NetworkManager.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().currentLocation;
        LogToServerRpc(NetworkManager.LocalClientId, "previousLocation was " + previousLocation);
        LogToServerRpc(NetworkManager.LocalClientId, "new Scene is: " + Scene_Name);
        SceneManager.UnloadSceneAsync(previousLocation);
        SceneManager.LoadScene(Scene_Name, LoadSceneMode.Additive);
        LogToServerRpc(NetworkManager.LocalClientId, "Loaded sceneToLoad");
        //ChangeSceneServerRpc(NetworkManager.LocalClientId, Scene_Name, x, y);
        Player.gameObject.transform.position = new Vector3(x, y, y);
        LogToServerRpc(NetworkManager.LocalClientId, "Moved Player to " + x + ", " + y);
        NetworkManager.Singleton.ConnectedClients[NetworkManager.LocalClientId].PlayerObject.gameObject.GetComponent<Player>().currentLocation = Scene_Name;
        //SceneManager.UnloadSceneAsync(previousLocation);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeSceneServerRpc(ulong clientId, String Scene_Name, float x, float y)
    {
        Scene sceneToMoveTo = SceneManager.GetSceneByName(Scene_Name);
        //SceneManager.LoadScene(Scene_Name, LoadSceneMode.Additive);
        SceneManager.MoveGameObjectToScene(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject, sceneToMoveTo);
        Debug.Log("Moved Player to " + x + ", " + y);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LogToServerRpc(ulong clientId, string message)
    {
        Debug.Log("Client " + clientId + ": " + message);
    }

}

public class PositionManager
{
    // vector position is to top left corner. Add offsets above to get the position of each component.
    BattlePositionSide[] Sides;
    float screen_width_reduction;
    float screen_height_reduction;
    float screen_width_reduced;
    float screen_height_reduced;
    float padding_width;
    float padding_height;

    public PositionManager(float screen_width, float screen_height)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "In PositionManager Constructor");
        screen_width_reduction = screen_width * .05f;
        screen_height_reduction = screen_height * .3f;
        screen_width_reduced = screen_width - screen_width_reduction;
        screen_height_reduced = screen_height - screen_height_reduction;

        padding_width = screen_width * .0f;
        padding_height = screen_height * .0f;

        Sides = new BattlePositionSide[2];

        Sides[0] = new BattlePositionSide();
        Sides[1] = new BattlePositionSide();

        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Creating Side 1");

        Sides[0].Largest = new Position(BattlePositionSize.Largest, new Vector3(-((screen_width_reduced / 2) - padding_width) - (screen_width_reduction / 2), ((screen_height / 2) - padding_height), 0));
        Sides[0].Large = new Position[2];
        Sides[0].Large[0] = new Position(BattlePositionSize.Large, new Vector3(Sides[0].Largest.Location.x, Sides[0].Largest.Location.y, 0));
        Sides[0].Large[0].Parent = Sides[0].Largest;
        Sides[0].Large[1] = new Position(BattlePositionSize.Large, new Vector3(Sides[0].Largest.Location.x + (screen_width_reduced / 4), Sides[0].Largest.Location.y, 0));
        Sides[0].Large[1].Parent = Sides[0].Largest;
        Sides[0].Medium = new Position[4];
        Sides[0].Medium[0] = new Position(BattlePositionSize.Medium, new Vector3(Sides[0].Large[1].Location.x, Sides[0].Large[0].Location.y, 0));
        Sides[0].Medium[0].Parent = Sides[0].Large[0];
        Sides[0].Medium[1] = new Position(BattlePositionSize.Medium, new Vector3(Sides[0].Large[1].Location.x, Sides[0].Large[0].Location.y - (screen_height_reduced / 2), 0));
        Sides[0].Medium[1].Parent = Sides[0].Large[0];
        Sides[0].Medium[2] = new Position(BattlePositionSize.Medium, new Vector3(Sides[0].Large[0].Location.x, Sides[0].Large[0].Location.y, 0));
        Sides[0].Medium[2].Parent = Sides[0].Large[1];
        Sides[0].Medium[3] = new Position(BattlePositionSize.Medium, new Vector3(Sides[0].Large[0].Location.x, Sides[0].Large[0].Location.y - (screen_height_reduced / 2), 0));
        Sides[0].Medium[3].Parent = Sides[0].Large[1];
        Sides[0].Small = new Position[8];
        Sides[0].Small[0] = new Position(BattlePositionSize.Small, new Vector3(Sides[0].Medium[0].Location.x, Sides[0].Medium[0].Location.y, 0));
        Sides[0].Small[0].Parent = Sides[0].Medium[0];
        Sides[0].Small[1] = new Position(BattlePositionSize.Small, new Vector3(Sides[0].Medium[0].Location.x + (screen_width_reduced / 8), Sides[0].Medium[0].Location.y, 0));
        Sides[0].Small[1].Parent = Sides[0].Medium[0];
        Sides[0].Small[2] = new Position(BattlePositionSize.Small, new Vector3(Sides[0].Medium[1].Location.x, Sides[0].Medium[1].Location.y, 0));
        Sides[0].Small[2].Parent = Sides[0].Medium[1];
        Sides[0].Small[3] = new Position(BattlePositionSize.Small, new Vector3(Sides[0].Medium[1].Location.x + (screen_width_reduced / 8), Sides[0].Medium[1].Location.y, 0));
        Sides[0].Small[3].Parent = Sides[0].Medium[1];
        Sides[0].Small[4] = new Position(BattlePositionSize.Small, new Vector3(Sides[0].Medium[2].Location.x, Sides[0].Medium[2].Location.y, 0));
        Sides[0].Small[4].Parent = Sides[0].Medium[2];
        Sides[0].Small[5] = new Position(BattlePositionSize.Small, new Vector3(Sides[0].Medium[2].Location.x + (screen_width_reduced / 8), Sides[0].Medium[2].Location.y, 0));
        Sides[0].Small[5].Parent = Sides[0].Medium[2];
        Sides[0].Small[6] = new Position(BattlePositionSize.Small, new Vector3(Sides[0].Medium[3].Location.x, Sides[0].Medium[3].Location.y, 0));
        Sides[0].Small[6].Parent = Sides[0].Medium[3];
        Sides[0].Small[7] = new Position(BattlePositionSize.Small, new Vector3(Sides[0].Medium[3].Location.x + (screen_width_reduced / 8), Sides[0].Medium[3].Location.y, 0));
        Sides[0].Small[7].Parent = Sides[0].Medium[3];
        Sides[0].Smallest = new Position[16];
        Sides[0].Smallest[0] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[0].Location.x, Sides[0].Small[0].Location.y, 0));
        Sides[0].Smallest[0].Parent = Sides[0].Small[0];
        Sides[0].Smallest[1] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[0].Location.x, Sides[0].Small[0].Location.y - (screen_height_reduced / 4), 0));
        Sides[0].Smallest[1].Parent = Sides[0].Small[1];
        Sides[0].Smallest[2] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[1].Location.x, Sides[0].Small[1].Location.y, 0));
        Sides[0].Smallest[2].Parent = Sides[0].Small[0];
        Sides[0].Smallest[3] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[1].Location.x, Sides[0].Small[1].Location.y - (screen_height_reduced / 4), 0));
        Sides[0].Smallest[3].Parent = Sides[0].Small[1];
        Sides[0].Smallest[4] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[2].Location.x, Sides[0].Small[2].Location.y, 0));
        Sides[0].Smallest[4].Parent = Sides[0].Small[2];
        Sides[0].Smallest[5] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[2].Location.x, Sides[0].Small[2].Location.y - (screen_height_reduced / 4), 0));
        Sides[0].Smallest[5].Parent = Sides[0].Small[3];
        Sides[0].Smallest[6] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[3].Location.x, Sides[0].Small[3].Location.y, 0));
        Sides[0].Smallest[6].Parent = Sides[0].Small[2];
        Sides[0].Smallest[7] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[3].Location.x, Sides[0].Small[3].Location.y - (screen_height_reduced / 4), 0));
        Sides[0].Smallest[7].Parent = Sides[0].Small[3];
        Sides[0].Smallest[8] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[4].Location.x, Sides[0].Small[4].Location.y, 0));
        Sides[0].Smallest[8].Parent = Sides[0].Small[4];
        Sides[0].Smallest[9] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[4].Location.x, Sides[0].Small[4].Location.y - (screen_height_reduced / 4), 0));
        Sides[0].Smallest[9].Parent = Sides[0].Small[5];
        Sides[0].Smallest[10] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[5].Location.x, Sides[0].Small[5].Location.y, 0));
        Sides[0].Smallest[10].Parent = Sides[0].Small[4];
        Sides[0].Smallest[11] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[5].Location.x, Sides[0].Small[5].Location.y - (screen_height_reduced / 4), 0));
        Sides[0].Smallest[11].Parent = Sides[0].Small[5];
        Sides[0].Smallest[12] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[6].Location.x, Sides[0].Small[6].Location.y, 0));
        Sides[0].Smallest[12].Parent = Sides[0].Small[6];
        Sides[0].Smallest[13] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[6].Location.x, Sides[0].Small[6].Location.y - (screen_height_reduced / 4), 0));
        Sides[0].Smallest[13].Parent = Sides[0].Small[7];
        Sides[0].Smallest[14] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[7].Location.x, Sides[0].Small[7].Location.y, 0));
        Sides[0].Smallest[14].Parent = Sides[0].Small[6];
        Sides[0].Smallest[15] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[0].Small[7].Location.x, Sides[0].Small[7].Location.y - (screen_height_reduced / 4), 0));
        Sides[0].Smallest[15].Parent = Sides[0].Small[7];

        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Adding Children");

        List<Position> children = new List<Position>();
        children.Add(Sides[0].Smallest[0]);
        children.Add(Sides[0].Smallest[2]);
        Sides[0].Small[0].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Smallest[1]);
        children.Add(Sides[0].Smallest[3]);
        Sides[0].Small[1].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Smallest[4]);
        children.Add(Sides[0].Smallest[6]);
        Sides[0].Small[2].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Smallest[5]);
        children.Add(Sides[0].Smallest[7]);
        Sides[0].Small[3].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Smallest[8]);
        children.Add(Sides[0].Smallest[10]);
        Sides[0].Small[4].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Smallest[9]);
        children.Add(Sides[0].Smallest[11]);
        Sides[0].Small[5].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Smallest[12]);
        children.Add(Sides[0].Smallest[14]);
        Sides[0].Small[6].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Smallest[13]);
        children.Add(Sides[0].Smallest[15]);
        Sides[0].Small[7].Children = children;

        children = new List<Position>();
        children.Add(Sides[0].Small[0]);
        children.Add(Sides[0].Small[1]);
        Sides[0].Medium[0].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Small[2]);
        children.Add(Sides[0].Small[3]);
        Sides[0].Medium[1].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Small[4]);
        children.Add(Sides[0].Small[5]);
        Sides[0].Medium[2].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Small[6]);
        children.Add(Sides[0].Small[7]);
        Sides[0].Medium[3].Children = children;

        children = new List<Position>();
        children.Add(Sides[0].Medium[0]);
        children.Add(Sides[0].Medium[2]);
        Sides[0].Large[0].Children = children;
        children = new List<Position>();
        children.Add(Sides[0].Medium[1]);
        children.Add(Sides[0].Medium[3]);
        Sides[0].Large[1].Children = children;

        children = new List<Position>();
        children.Add(Sides[0].Large[0]);
        children.Add(Sides[0].Large[1]);
        Sides[0].Largest.Children = children;

        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Creating Side 2");

        Sides[1].Largest = new Position(BattlePositionSize.Largest, new Vector3(padding_width + (screen_width_reduction / 2), ((screen_height / 2) - padding_height), 0));
        Sides[1].Large = new Position[2];
        Sides[1].Large[0] = new Position(BattlePositionSize.Large, new Vector3(Sides[1].Largest.Location.x, Sides[1].Largest.Location.y, 0));
        Sides[1].Large[0].Parent = Sides[1].Largest;
        Sides[1].Large[1] = new Position(BattlePositionSize.Large, new Vector3(Sides[1].Largest.Location.x + (screen_width_reduced / 4), Sides[1].Largest.Location.y, 0));
        Sides[1].Large[1].Parent = Sides[1].Largest;
        Sides[1].Medium = new Position[4];
        Sides[1].Medium[0] = new Position(BattlePositionSize.Medium, new Vector3(Sides[1].Large[1].Location.x, Sides[1].Large[0].Location.y, 0));
        Sides[1].Medium[0].Parent = Sides[1].Large[0];
        Sides[1].Medium[1] = new Position(BattlePositionSize.Medium, new Vector3(Sides[1].Large[1].Location.x, Sides[1].Large[0].Location.y - (screen_height_reduced / 2), 0));
        Sides[1].Medium[1].Parent = Sides[1].Large[0];
        Sides[1].Medium[2] = new Position(BattlePositionSize.Medium, new Vector3(Sides[1].Large[0].Location.x, Sides[1].Large[0].Location.y, 0));
        Sides[1].Medium[2].Parent = Sides[1].Large[1];
        Sides[1].Medium[3] = new Position(BattlePositionSize.Medium, new Vector3(Sides[1].Large[0].Location.x, Sides[1].Large[0].Location.y - (screen_height_reduced / 2), 0));
        Sides[1].Medium[3].Parent = Sides[1].Large[1];
        Sides[1].Small = new Position[8];
        Sides[1].Small[0] = new Position(BattlePositionSize.Small, new Vector3(Sides[1].Medium[0].Location.x, Sides[1].Medium[0].Location.y, 0));
        Sides[1].Small[0].Parent = Sides[1].Medium[0];
        Sides[1].Small[1] = new Position(BattlePositionSize.Small, new Vector3(Sides[1].Medium[0].Location.x + (screen_width_reduced / 8), Sides[1].Medium[0].Location.y, 0));
        Sides[1].Small[1].Parent = Sides[1].Medium[0];
        Sides[1].Small[2] = new Position(BattlePositionSize.Small, new Vector3(Sides[1].Medium[1].Location.x, Sides[1].Medium[1].Location.y, 0));
        Sides[1].Small[2].Parent = Sides[1].Medium[1];
        Sides[1].Small[3] = new Position(BattlePositionSize.Small, new Vector3(Sides[1].Medium[1].Location.x + (screen_width_reduced / 8), Sides[1].Medium[1].Location.y, 0));
        Sides[1].Small[3].Parent = Sides[1].Medium[1];
        Sides[1].Small[4] = new Position(BattlePositionSize.Small, new Vector3(Sides[1].Medium[2].Location.x, Sides[1].Medium[2].Location.y, 0));
        Sides[1].Small[4].Parent = Sides[1].Medium[2];
        Sides[1].Small[5] = new Position(BattlePositionSize.Small, new Vector3(Sides[1].Medium[2].Location.x + (screen_width_reduced / 8), Sides[1].Medium[2].Location.y, 0));
        Sides[1].Small[5].Parent = Sides[1].Medium[2];
        Sides[1].Small[6] = new Position(BattlePositionSize.Small, new Vector3(Sides[1].Medium[3].Location.x, Sides[1].Medium[3].Location.y, 0));
        Sides[1].Small[6].Parent = Sides[1].Medium[3];
        Sides[1].Small[7] = new Position(BattlePositionSize.Small, new Vector3(Sides[1].Medium[3].Location.x + (screen_width_reduced / 8), Sides[1].Medium[3].Location.y, 0));
        Sides[1].Small[7].Parent = Sides[1].Medium[3];
        Sides[1].Smallest = new Position[16];
        Sides[1].Smallest[0] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[0].Location.x, Sides[1].Small[0].Location.y, 0));
        Sides[1].Smallest[0].Parent = Sides[1].Small[0];
        Sides[1].Smallest[1] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[0].Location.x, Sides[1].Small[0].Location.y - (screen_height_reduced / 4), 0));
        Sides[1].Smallest[1].Parent = Sides[1].Small[1];
        Sides[1].Smallest[2] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[1].Location.x, Sides[1].Small[1].Location.y, 0));
        Sides[1].Smallest[2].Parent = Sides[1].Small[0];
        Sides[1].Smallest[3] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[1].Location.x, Sides[1].Small[1].Location.y - (screen_height_reduced / 4), 0));
        Sides[1].Smallest[3].Parent = Sides[1].Small[1];
        Sides[1].Smallest[4] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[2].Location.x, Sides[1].Small[2].Location.y, 0));
        Sides[1].Smallest[4].Parent = Sides[1].Small[2];
        Sides[1].Smallest[5] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[2].Location.x, Sides[1].Small[2].Location.y - (screen_height_reduced / 4), 0));
        Sides[1].Smallest[5].Parent = Sides[1].Small[3];
        Sides[1].Smallest[6] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[3].Location.x, Sides[1].Small[3].Location.y, 0));
        Sides[1].Smallest[6].Parent = Sides[1].Small[2];
        Sides[1].Smallest[7] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[3].Location.x, Sides[1].Small[3].Location.y - (screen_height_reduced / 4), 0));
        Sides[1].Smallest[7].Parent = Sides[1].Small[3];
        Sides[1].Smallest[8] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[4].Location.x, Sides[1].Small[4].Location.y, 0));
        Sides[1].Smallest[8].Parent = Sides[1].Small[4];
        Sides[1].Smallest[9] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[4].Location.x, Sides[1].Small[4].Location.y - (screen_height_reduced / 4), 0));
        Sides[1].Smallest[9].Parent = Sides[1].Small[5];
        Sides[1].Smallest[10] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[5].Location.x, Sides[1].Small[5].Location.y, 0));
        Sides[1].Smallest[10].Parent = Sides[1].Small[4];
        Sides[1].Smallest[11] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[5].Location.x, Sides[1].Small[5].Location.y - (screen_height_reduced / 4), 0));
        Sides[1].Smallest[11].Parent = Sides[1].Small[5];
        Sides[1].Smallest[12] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[6].Location.x, Sides[1].Small[6].Location.y, 0));
        Sides[1].Smallest[12].Parent = Sides[1].Small[6];
        Sides[1].Smallest[13] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[6].Location.x, Sides[1].Small[6].Location.y - (screen_height_reduced / 4), 0));
        Sides[1].Smallest[13].Parent = Sides[1].Small[7];
        Sides[1].Smallest[14] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[7].Location.x, Sides[1].Small[7].Location.y, 0));
        Sides[1].Smallest[14].Parent = Sides[1].Small[6];
        Sides[1].Smallest[15] = new Position(BattlePositionSize.Smallest, new Vector3(Sides[1].Small[7].Location.x, Sides[1].Small[7].Location.y - (screen_height_reduced / 4), 0));
        Sides[1].Smallest[15].Parent = Sides[1].Small[7];

        children = new List<Position>();
        children.Add(Sides[1].Smallest[0]);
        children.Add(Sides[1].Smallest[2]);
        Sides[1].Small[0].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Smallest[1]);
        children.Add(Sides[1].Smallest[3]);
        Sides[1].Small[1].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Smallest[4]);
        children.Add(Sides[1].Smallest[6]);
        Sides[1].Small[2].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Smallest[5]);
        children.Add(Sides[1].Smallest[7]);
        Sides[1].Small[3].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Smallest[8]);
        children.Add(Sides[1].Smallest[10]);
        Sides[1].Small[4].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Smallest[9]);
        children.Add(Sides[1].Smallest[11]);
        Sides[1].Small[5].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Smallest[12]);
        children.Add(Sides[1].Smallest[14]);
        Sides[1].Small[6].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Smallest[13]);
        children.Add(Sides[1].Smallest[15]);
        Sides[1].Small[7].Children = children;

        children = new List<Position>();
        children.Add(Sides[1].Small[0]);
        children.Add(Sides[1].Small[1]);
        Sides[1].Medium[0].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Small[2]);
        children.Add(Sides[1].Small[3]);
        Sides[1].Medium[1].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Small[4]);
        children.Add(Sides[1].Small[5]);
        Sides[1].Medium[2].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Small[6]);
        children.Add(Sides[1].Small[7]);
        Sides[1].Medium[3].Children = children;

        children = new List<Position>();
        children.Add(Sides[1].Medium[0]);
        children.Add(Sides[1].Medium[2]);
        Sides[1].Large[0].Children = children;
        children = new List<Position>();
        children.Add(Sides[1].Medium[1]);
        children.Add(Sides[1].Medium[3]);
        Sides[1].Large[1].Children = children;

        children = new List<Position>();
        children.Add(Sides[1].Large[0]);
        children.Add(Sides[1].Large[1]);
        Sides[1].Largest.Children = children;

    }

    public void ClearIds()
    {
        ClearPositionIDs(Sides[0].Largest);
        ClearPositionIDs(Sides[1].Largest);
    }

    private void ClearPositionIDs(Position position)
    {
        position.ID = -1;
        if (position.Children != null)
        {
            foreach (Position child in position.Children)
            {
                ClearPositionIDs(child);
            }
        }
    }

    public Vector3 GetSpriteOffset(CreatureSize size)
    {
        BattlePositionSize PositionSize = Position.GetPositionSize(size);
        switch (PositionSize)
        {
            case BattlePositionSize.Largest:
                return new Vector3(-(screen_width_reduced / 4), (screen_height_reduced / 2));
            case BattlePositionSize.Large:
                return new Vector3(-(screen_width_reduced / 8), (screen_height_reduced / 2));
            case BattlePositionSize.Medium:
                return new Vector3(-(screen_width_reduced / 8), (screen_height_reduced / 4));
            case BattlePositionSize.Small:
                return new Vector3(-(screen_width_reduced / 16), (screen_height_reduced / 4));
            default:
                return new Vector3(-(screen_width_reduced / 16), (screen_height_reduced / 8));
        }
    }

    public float HealthBarScale(CreatureSize size)
    {
        BattlePositionSize PositionSize = Position.GetPositionSize(size);
        switch (PositionSize)
        {
            case BattlePositionSize.Largest:
                return 1.5f;
            case BattlePositionSize.Large:
                return 1.2f;
            case BattlePositionSize.Medium:
                return 1f;
            case BattlePositionSize.Small:
                return .7f;
            default:
                return .6f;
        }
    }

    //public Vector3 GetHealthOffset(CreatureSize size)
    //{
    //    BattlePositionSize PositionSize = Position.GetPositionSize(size);
    //    switch (PositionSize)
    //    {
    //        case BattlePositionSize.Largest:
    //            return new Vector3();
    //        case BattlePositionSize.Large:
    //            return new Vector3();
    //        case BattlePositionSize.Medium:
    //            return new Vector3();
    //        case BattlePositionSize.Small:
    //            return new Vector3();
    //        default:
    //            return new Vector3();
    //    }
    //}

    public Vector3 GetPosition(int id, CreatureSize size, int side)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "In GetPosition");

        BattlePositionSize PositionSize = Position.GetPositionSize(size);
        if (PositionSize == BattlePositionSize.Largest)
        {
            int index = GetNextLargestIndex(side);
            if (index != -1)
            {
                Sides[side].Largest.ID = id;
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Returning Largest " + Sides[side].Largest.Location);
                return Sides[side].Largest.Location + GameManager.BATTLE_OFFSET;
            }
        }
        else if (PositionSize == BattlePositionSize.Large)
        {
            int index = GetNextLargeIndex(side);
            if (index != -1)
            {
                Sides[side].Large[index].ID = id;
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Returning Large[" + index + "] " + Sides[side].Large[index].Location);
                return Sides[side].Large[index].Location + GameManager.BATTLE_OFFSET;
            }
        }
        else if (PositionSize == BattlePositionSize.Medium)
        {
            int index = GetNextMediumIndex(side);
            if (index != -1)
            {
                Sides[side].Medium[index].ID = id;
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Returning Medium[" + index + "] " + Sides[side].Medium[index].Location);
                return Sides[side].Medium[index].Location + GameManager.BATTLE_OFFSET;
            }
        }
        else if (PositionSize == BattlePositionSize.Small)
        {
            int index = GetNextSmallIndex(side);
            if (index != -1)
            {
                Sides[side].Small[index].ID = id;
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Returning Small[" + index + "] " + Sides[side].Small[index].Location);
                return Sides[side].Small[index].Location + GameManager.BATTLE_OFFSET;
            }
        }
        else if (PositionSize == BattlePositionSize.Smallest)
        {
            int index = GetNextSmallestIndex(side);
            if (index != -1)
            {
                Sides[side].Smallest[index].ID = id;
                //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Returning Smallest[" + index + "] " + Sides[side].Smallest[index].Location);
                return Sides[side].Smallest[index].Location + GameManager.BATTLE_OFFSET;
            }
        }

        return Vector3.negativeInfinity;

    }

    public bool IsSpaceFree(Position position)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "In IsSpaceFree");
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "position Location = " + position.Location.x + ", " + position.Location.y);
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "position Size = " + position.Size);

        int ID = position.ID;
        if (ID != -1)
        {
            return false;
        } else
        {
            if (position.Children != null)
            {
                foreach (Position child in position.Children)
                {
                    //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "Has Children = " + position.Children.Count);
                    if (!IsSpaceFree(child))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public int GetNextLargestIndex(int side)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "In GetNextLargestIndex");
        // find the best spot for a new creature of the Largest size and return the index of that spot.

        if (IsSpaceFree(Sides[side].Largest))
        {
            return 0;
        }

        return -1;
    }


    public int GetNextLargeIndex(int side)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "In GetNextLargeIndex");
        // find the best spot for a new creature of the Large size and return the index of that spot.
        if (Sides[side].Largest.ID != -1)
        {
            return -1;
        }

        if (IsSpaceFree(Sides[side].Large[0]))
        {
            // Largest is filled nothing else fits
            return 0;
        } 
        
        if (IsSpaceFree(Sides[side].Large[1]))
        {
            return 1;
        }

        return -1;
    }
    public int GetNextMediumIndex(int side)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "In GetNextMediumIndex");
        // find the best spot for a new creature of the medium size and return the index of that spot.

        if (Sides[side].Largest.ID != -1)
        {
            return -1;
        }

        if (Sides[side].Large[0].ID == -1)
        {
            if (IsSpaceFree(Sides[side].Medium[0]))
            {
                return 0;
            } 
            
            if (IsSpaceFree(Sides[side].Medium[1]))
            {
                return 1;
            }
        } 
        
        if (Sides[side].Large[1].ID == -1)
        {
            if (IsSpaceFree(Sides[side].Medium[2]))
            {
                return 2;
            }
            
            if (IsSpaceFree(Sides[side].Medium[3]))
            {
                return 3;
            }
        }

        return -1;
    }
    public int GetNextSmallIndex(int side)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "In GetNextSmallIndex");
        // find the best spot for a new creature of the small size and return the index of that spot.
        if (Sides[side].Largest.ID != -1)
        {
            return -1;
        }

        if (Sides[side].Large[0].ID == -1)
        {
            if (Sides[side].Medium[0].ID == -1)
            {
                if (IsSpaceFree(Sides[side].Small[0])){
                    return 0;
                } 
                
                if (IsSpaceFree(Sides[side].Small[1]))
                {
                    return 1;
                }
            }
            
            if (Sides[side].Medium[1].ID == -1)
            {
                if (IsSpaceFree(Sides[side].Small[2]))
                {
                    return 2;
                }
                
                if (IsSpaceFree(Sides[side].Small[3]))
                {
                    return 3;
                }
            }
        }
        
        if (Sides[side].Large[1].ID == -1)
        {
            if (Sides[side].Medium[2].ID == -1)
            {
                if (IsSpaceFree(Sides[side].Small[4]))
                {
                    return 4;
                }
                
                if (IsSpaceFree(Sides[side].Small[5]))
                {
                    return 5;
                }
            }
            
            if (Sides[side].Medium[3].ID == -1)
            {
                if (IsSpaceFree(Sides[side].Small[6]))
                {
                    return 6;
                }
                
                if (IsSpaceFree(Sides[side].Small[7]))
                {
                    return 7;
                }
            }
        }

        return -1;
    }
    public int GetNextSmallestIndex(int side)
    {
        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().LogToServerRpc(0, "In GetNextSmallestIndex");
        // find the best spot for a new creature of the smallest size and return the index of that spot.
        // find the best spot for a new creature of the small size and return the index of that spot.
        if (Sides[side].Largest.ID != -1)
        {
            return -1;
        }

        if (Sides[side].Large[0].ID == -1)
        {
            if (Sides[side].Medium[0].ID == -1)
            {
                if (Sides[side].Small[0].ID == -1)
                {
                    if (IsSpaceFree(Sides[side].Smallest[0]))
                    {
                        return 0;
                    } 
                    if (IsSpaceFree(Sides[side].Smallest[1]))
                    {
                        return 1;
                    }
                }
                
                if (Sides[side].Small[1].ID == -1)
                {
                    if (IsSpaceFree(Sides[side].Smallest[2]))
                    {
                        return 2;
                    }
                    
                    if (IsSpaceFree(Sides[side].Smallest[3]))
                    {
                        return 3;
                    }
                }
            }
            
            if (Sides[side].Medium[1].ID == -1)
            {
                if (Sides[side].Small[2].ID == -1)
                {
                    if (IsSpaceFree(Sides[side].Smallest[4]))
                    {
                        return 4;
                    }
                    
                    if (IsSpaceFree(Sides[side].Smallest[5]))
                    {
                        return 5;
                    }
                }
                
                if (Sides[side].Small[3].ID == -1)
                {
                    if (IsSpaceFree(Sides[side].Smallest[6]))
                    {
                        return 6;
                    }
                    
                    if (IsSpaceFree(Sides[side].Smallest[7]))
                    {
                        return 7;
                    }
                }
            }
        }
        
        if (Sides[side].Large[1].ID == -1)
        {
            if (Sides[side].Medium[2].ID == -1)
            {
                if (Sides[side].Small[4].ID == -1)
                {
                    if (IsSpaceFree(Sides[side].Smallest[8]))
                    {
                        return 8;
                    }
                    
                    if (IsSpaceFree(Sides[side].Smallest[9]))
                    {
                        return 9;
                    }
                }
                
                if (Sides[side].Small[5].ID == -1)
                {
                    if (IsSpaceFree(Sides[side].Smallest[10]))
                    {
                        return 10;
                    }
                    
                    if (IsSpaceFree(Sides[side].Smallest[11]))
                    {
                        return 11;
                    }
                }
            }
            
            if (Sides[side].Medium[3].ID == -1)
            {
                if (Sides[side].Small[6].ID == -1)
                {
                    if (IsSpaceFree(Sides[side].Smallest[12]))
                    {
                        return 12;
                    }
                    
                    if (IsSpaceFree(Sides[side].Smallest[13]))
                    {
                        return 13;
                    }
                }
                
                if (Sides[side].Small[7].ID == -1)
                {
                    if (IsSpaceFree(Sides[side].Smallest[14]))
                    {
                        return 14;
                    }
                    
                    if (IsSpaceFree(Sides[side].Smallest[15]))
                    {
                        return 15;
                    }
                }
            }
        }

        return -1;
    }

}
public class Position
{
    public BattlePositionSize Size;
    public int ID;
    public Vector3 Location;
    public Position Parent;
    public List<Position> Children;

    public Position(BattlePositionSize size, Vector3 location)
    {
        Size = size;
        ID = -1;
        Location = location;
    }

    public static BattlePositionSize GetPositionSize(CreatureSize size)
    {
        switch (size)
        {
            case CreatureSize.Collosal:
            case CreatureSize.VeryMassive:
            case CreatureSize.Massive:
                return BattlePositionSize.Largest;
            case CreatureSize.Huge:
            case CreatureSize.Giant:
                return BattlePositionSize.Large;
            case CreatureSize.ExtremelyLarge:
            case CreatureSize.VeryLarge:
            case CreatureSize.Large:
            case CreatureSize.Medium:
                return BattlePositionSize.Medium;
            case CreatureSize.Small:
            case CreatureSize.VerySmall:
                return BattlePositionSize.Small;
            default:
                return BattlePositionSize.Smallest;
        }
    }
}

public class BattlePositionSide {
    public Position Largest;
    public Position[] Large;
    public Position[] Medium;
    public Position[] Small;
    public Position[] Smallest;

}
