using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// REFERENCES
    /// </summary>
    public MBattleHUD refHud;
    public PhotonView photonView;
    public MAttackMenu refAttackMenu;
    public MHealMenu refHealMenu;
    public MPerformAttack refPerformAttack;
    public MultiplayerFighter[] fightersList; //contains all the fighter in the game in the game
    public AudioClip onAttackSucceeded; //this is the sound played when an attack succeed
    public AudioClip onAttackFailed; //this is the sound played when an attack failed
    public MSetter MSetter; //menu for selecting a Fighter
    /// <summary>
    /// COMBAT STATS
    /// </summary>
    public float appreciation;

    public BattleState currentState;
    public bool menuOpened = false;
    
    public MultiplayerFighter opponent;
    public MultiplayerFighter me;

    private bool isHostReady = false; //this variabe is set to true when host have selected a fighter
    private bool isGuestReady = false; // this variable is set to true when guest have selected a fighter
    
    private Dictionary<Effect, int> meEffects = new Dictionary<Effect, int>()
    {
        {Effect.Mistake, 0},
        {Effect.Stressed, 0},
        {Effect.FreakOut , 0}
    };


    /// <summary>
    /// OTHERS
    /// </summary>
    public UnityEvent<bool, AttackObject> onAttackComplete = new UnityEvent<bool, AttackObject>();

    //this is the usage of the consumable
    public Dictionary<Consumable, int> localConsumableUsage = new Dictionary<Consumable, int>();
    public Dictionary<AttackObject, int> localAttackUsage = new Dictionary<AttackObject, int>();
    


    [Serializable]
    public enum BattleState
    {
        SELECT_FIGHTER, //phase of selecting the player
        BEGINNING, //phase of beginning with all the message
        HOST_TURN, // phase of host turn
        GUEST_TURN, // phase of guest turn
        END_OF_COMBAT //eoc
    }


    [PunRPC]
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        IEnumerator routine()
        {
            Debug.Log("Le joueur " + otherPlayer.NickName + " a quitté la salle.");
            refHud.SetDialogText("[NETWORK] : the other player just left the room, you will be redirected to main menu.");
            yield return new WaitForSeconds(4f);
            OnClickForfeit();
        }
        StartCoroutine(routine());
    }

    
    
    
    void Start()
    {
        onAttackComplete.AddListener(OnAttackComplete);
        currentState = BattleState.SELECT_FIGHTER;
        appreciation = 50;
        refHud.UpdateAppreciation(appreciation);
        refHud.SetDialogText("Waiting for players to select their fighters.");
        //COMMENCE PAR AFFICHER LE MENU DE SELECTION DES PERSONNAGES
        MSetter.gameObject.SetActive(true);
        MSetter.onFighterSelected = (selectedFighter) =>
        {
            MSetter.gameObject.SetActive(false);
            
            
            //search in list for fighterid
            int fighterId = -1;
            for (int i = 0; i < fightersList.Length; i++)
            {
                if (fightersList[i].nickName == selectedFighter.nickName)
                {
                    fighterId = i;
                }
            }
            
            //set fighterid
            photonView.RPC("SelectFighter", RpcTarget.All, fighterId, PhotonNetwork.IsMasterClient);
        };
    }

    /// <summary>
    /// This function handle the select menu of the player
    /// </summary>
    /// <param name="fighter">the int corresponding to the index of the mf in the list</param>
    [PunRPC]
    private void SelectFighter(int fighter, bool isHost)
    {
        //set the fighter to the correct fighter
        if (PhotonNetwork.IsMasterClient)
        {
            if (isHost)
                me = fightersList[fighter];
            else
                opponent = fightersList[fighter];
        }
        else
        {
            if (!isHost)
                me = fightersList[fighter];
            else
                opponent = fightersList[fighter];
        }
        
        //check in host side if both the player have set their value
        if (PhotonNetwork.IsMasterClient)
        {
            //if host and guest is fighter-setted
            if (me != null && opponent != null)
            {
                photonView.RPC("UpdateState", RpcTarget.All, BattleState.BEGINNING);
            }
        }
    }

    public IEnumerator DisplayTemporaryMsg(string message)
    {
        refHud.SetDialogText(message);
        yield return new WaitForSeconds(3f);
        refHud.SetDialogText("It's your turn, please perform an attack or heal.");
        
    }

    /// <summary>
    /// This is the main function that will be called to managed the game each time the state changes.
    /// </summary>
    /// <param name="bs">The new battlestate.</param>
    /// <returns></returns>
    [PunRPC]
    IEnumerator UpdateState(BattleState bs)
    {
        currentState = bs;

        if (bs == BattleState.SELECT_FIGHTER)
        {
            //do nothing
        }
        else if (bs == BattleState.BEGINNING) {
            // INITIALISE THE INSPIRATION OF THE MULTIPLAYER_FIGHTER
            me.inspiration = me.maxInspiration;
            me.resistance = me.maxResistance;
            
            // INITIALISE THE 2 DICTIONARY FOR ATTACKS AND CONSUMABLE
            foreach (var c in me.consumables)
            {
                if (localConsumableUsage.Keys.Contains(c))
                {
                    localConsumableUsage[c] += 1;
                }
                else
                {
                    localConsumableUsage.Add(c,1);
                }
            }
            foreach (var c in me.attacks)
            {
                if (localAttackUsage.Keys.Contains(c))
                {
                    localAttackUsage[c] = 0;
                }
                else
                {
                    localAttackUsage.Add(c,0);
                }
            }
            
            // INITIALIZE THE UI
            refHud.Initialize(me, opponent);
            
            // FIRST SET THE WELCOMING DIALOGUE FOR 4 SECONDS
            refHud.SetDialogText(
                $"Welcome to this musical battle between {PhotonNetwork.NickName} and {PhotonNetwork.PlayerListOthers[0]}, it's going to be noisy, be careful ! ");
            yield return new WaitForSeconds(4);
            
            // DISPLAY A CUSTOM MESSAGE FOR HOST AND GUEST FOR THE BATTLE FOR 2 SECOND
            // AND THEN HOST SEND RPC UPDATE_STATE BattleState.HOST_TURN
            if (PhotonNetwork.IsMasterClient)
            {
                refHud.SetDialogText($"{PhotonNetwork.NickName} will start, let's hear what he has to offer !");
                yield return new WaitForSeconds(2);
                Debug.Log("CM effector -> updateState called");
                photonView.RPC("UpdateState", RpcTarget.All, BattleState.HOST_TURN);
            }
            else
            {
                refHud.SetDialogText(
                    $"{PhotonNetwork.PlayerListOthers[0].NickName} will start, let's hear what he has to offer !");
            }
        }
        else if (bs == BattleState.HOST_TURN) {
            Debug.Log("CM effector -> HOST TURN");
            // LOGIC FOR HOST SIDE AND FOR GUEST SIDE   
            if (PhotonNetwork.IsMasterClient)
            {
                // ON HOST, CHECK FOR EFFECT
                if (meEffects[Effect.FreakOut] > 0)
                {
                    Debug.Log("CM effector -> freaked message started");
                    // DISPLAY EFFECT MESSAGE FOR 4 SECONDS
                    photonView.RPC("DisplayEffect", RpcTarget.All, Effect.FreakOut, meEffects[Effect.FreakOut]);
                    
                    // DECREASE THE EFFECTS REMAININGS TURN
                    meEffects[Effect.FreakOut] -= 1;
                    
                    // APPLY WAITING
                    yield return new WaitForSeconds(4f);
                    Debug.Log("CM effector -> freaked out message ended");
                    
                    Debug.Log("CM effector -> updateState called");
                    // UPDATE_STATE TO GUEST_TURN
                    photonView.RPC("UpdateState", RpcTarget.All, BattleState.GUEST_TURN);
                    yield break;
                }

                // IF HOST IS NOT FREAKED OUT AND HAS MISTAKE LOGIC
                if (meEffects[Effect.Mistake] > 0)
                {
                    // SEND THE DISPLAY_EFFECT FREAKOUT TO EVERYONE FOR 2 SECOND
                    photonView.RPC("DisplayEffect", RpcTarget.All, Effect.FreakOut, meEffects[Effect.FreakOut]);
                    
                    // 
                    meEffects[Effect.Mistake] -= 1;
                    yield return new WaitForSeconds(4f);
                    photonView.RPC("UpdateAppreciation", RpcTarget.All, appreciation+10);
                }
                refHud.SetDialogText($"It's your turn, heal or perform an attack.");
                
            }
            else
            {
                //IF IT'S NOT PLAYER TURN, JUST DISPLAY A MESSAGE "WAITING FOR PLAYER"
                refHud.SetDialogText($"It's {PhotonNetwork.PlayerListOthers[0].NickName}'s turn, wait for him to act.");
            }
        }
        else if (bs == BattleState.GUEST_TURN) {
            Debug.Log("CM effector -> GUEST TURN");
            // IF IT'S GUEST TURN AND THE CLIENT IS GUEST
            if (!PhotonNetwork.IsMasterClient)
            {
                // CHECK FOR GUEST'S FREAKOUT EFFECT
                if (meEffects[Effect.FreakOut] > 0)
                {
                    Debug.Log("CM effector -> freaked message started");
                    //DISPLAY THE FREAKOUT EFFECT FOR 4 SECONDS
                    photonView.RPC("DisplayEffect", RpcTarget.All, Effect.FreakOut, meEffects[Effect.FreakOut]);
                    
                    // DECREASE THE EFFECTS REMAININGS TURN
                    meEffects[Effect.FreakOut] -= 1;
                    
                    //APPLY THE WAITING
                    yield return new WaitForSeconds(4f);
                    Debug.Log("CM effector -> freaked message ended");
                    //UPDATE_STATE TO HOST_TURN
                    Debug.Log("CM effector -> updateState called");
                    photonView.RPC("UpdateState", RpcTarget.All, BattleState.HOST_TURN);
                    yield break;
                }
                
                // CHECK IF GUEST HAS A MISTAKE EFFECT
                if (meEffects[Effect.Mistake] > 0)
                {
                    //DISPLAY MISTAKE EFFECT MESSAGE TO ALL
                    photonView.RPC("DisplayEffect", RpcTarget.All, Effect.Mistake, meEffects[Effect.Mistake]);
                    // DECREASE THE EFFECTS REMAININGS TURN
                    meEffects[Effect.Mistake] -= 1;
                    
                    //WAIT
                    yield return new WaitForSeconds(4f);
                    photonView.RPC("UpdateAppreciation", RpcTarget.All, appreciation+10);
                }
                
                //FINALLY DISPLAY TEXT TO ACT
                refHud.SetDialogText($"It's your turn, heal or perform an attack.");
            }
            else
            {
                //ELSE DISPLAY A TEXT SAYING WAITING FOR PLAYER TO ACT
                refHud.SetDialogText($"It's {PhotonNetwork.PlayerListOthers[0].NickName}'s turn, wait for him to act.");
            }
        }
        else
        {
            //JUST PRINT WHO WIN
            if (PhotonNetwork.IsMasterClient)
            {
                if (appreciation == 0)
                {  
                    refHud.SetDialogText("You loose.");
                }
                else
                {
                    refHud.SetDialogText("You win.");
                }
            }
            else
            {
                if (appreciation == 0)
                {  
                    refHud.SetDialogText("You win.");
                }
                else
                {
                    refHud.SetDialogText("You loose.");
                }
            }

            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Lobby");
        }
    }


    /// <summary>
    /// This function is triggered when the client click on the Forfeit button
    /// </summary>
    public void OnClickForfeit()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("ConnectToServer");
    }
    
    /// <summary>
    /// On click on the heal button.
    /// </summary>
    public void OnClickAttack()
    {
        if (menuOpened)
        {
            refAttackMenu.CloseMenu();
            menuOpened = false;
            return;
        }

        //check if it's player turn.
        if (currentState == BattleState.HOST_TURN && PhotonNetwork.IsMasterClient ||
            currentState == BattleState.GUEST_TURN && !PhotonNetwork.IsMasterClient)
        {
            if (!localAttackUsage.Any((i => i.Key.MaxUse-i.Value > 0)))
            {
                refHud.SetDialogText("You dont have any attack left, your only choice is to forfeit.");
                return;
            }
            
            menuOpened = true;
    
            foreach (var kv in localAttackUsage)
            {
                    Debug.Log($"atk : {kv.Key.name}, value : {kv.Value}");
            }
            refAttackMenu.OpenMenu((a) =>
            {
                if (a is null) return;
                menuOpened = false;
                StartCoroutine(refPerformAttack.StartAttack(a, onAttackComplete, meEffects[Effect.Stressed]>0));
            },  this);
        }
    }


    /// <summary>
    /// On click on the attack button.
    /// </summary>
    public void OnClickHeal()
    {
        if (menuOpened)
        {
            refHealMenu.CloseMenu();
            menuOpened = false;
            return;
        }

        //check if it's player turn.
        if (currentState == BattleState.HOST_TURN && PhotonNetwork.IsMasterClient ||
            currentState == BattleState.GUEST_TURN && !PhotonNetwork.IsMasterClient)
        {
            menuOpened = true;

            refHealMenu.OpenMenu((c =>
            {
                if (c is null) return;
                
                OnHealComplete(c);
            } ), localConsumableUsage);
        }
    }
    /// <summary>
    /// This is the triggered function called when PerformAttackUi end.
    /// </summary>
    public void OnAttackComplete(bool succeeded, AttackObject a)
    {
        int duration = Random.Range(1, 4);
        localAttackUsage[a] += 1;
        me.inspiration -= a.InspirationCost;
        refHud.UpdateInspiration(me.inspiration);
        if (succeeded)
        {
            
            Debug.Log("Succeded his attack");
            //IF THE ATTACK WAS A SUCCESS
            photonView.RPC("UsedAnAttack", RpcTarget.All, a.name, a.effect, duration, a.damage, a.ResistanceImpact);
        }
        else
        {
            Debug.Log("Failed an attack");
            //IF IT WAS NOT
            photonView.RPC("FailedAttack", RpcTarget.All, a.name);
        }
    }
    
    
    /// <summary>
    /// When a player perform an heal;
    /// </summary>
    public void OnHealComplete(Consumable c)
    {
        localConsumableUsage[c] -= 1;
        me.inspiration += c.addedInspiration;
        me.resistance += c.addedResistance;
        refHud.UpdateInspiration(me.inspiration);
        refHud.UpdateResistance(me.resistance);
        photonView.RPC("UsedAnConsumable", RpcTarget.All, c.name, c.addedInspiration, c.addedResistance);
    }
    
    
    /// <summary>
    /// THIS FUNCTION HANDLE THE LOGIC WHEN SOMEBODY PERFORM AN ATTACK
    ///
    /// note : the appreciation is updated by the person who is under the attack of
    /// 
    /// </summary>
    /// <param name="attackName"></param>
    /// <param name="effect"></param>
    /// <param name="round"></param>
    /// <param name="dmg"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [PunRPC]
    IEnumerator UsedAnAttack(string attackName, Effect effect, int round, int damage, int impactResistance)
    {
        // IF YOU ARE THE PERFORMER
        if (currentState == BattleState.HOST_TURN && PhotonNetwork.IsMasterClient || currentState == BattleState.GUEST_TURN && !PhotonNetwork.IsMasterClient)
        {
            switch (effect)
            {
                case Effect.FreakOut:
                    refHud.SetDialogText($"You use the {attackName} and makes the enemy freaked out for {round} turn(s). As a result he wont be able to perform attack during those round(s).");
                    break;
                case Effect.Mistake:
                    refHud.SetDialogText($"You use the {attackName} and makes the enemy makes mistakes for {round} turn(s). As a result, the public likes him less and he loses 5% appreciation during these rounds.");
                    break;
                case Effect.Stressed:
                    refHud.SetDialogText($"You use the {attackName} and makes the enemy stressed for {round} turn(s). As a result, he will need to play his attacks twice as fast.");
                    break;
                case Effect.None:
                    refHud.SetDialogText($"You use the {attackName}.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
            }
        }
        // IF YOU ARE NOT THE PERFORMER
        else if (currentState == BattleState.HOST_TURN && !PhotonNetwork.IsMasterClient ||
                  currentState == BattleState.GUEST_TURN && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ENTERED 2");
            switch (effect)
            {
                case Effect.FreakOut:
                    refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0].NickName} uses the {attackName} and makes you freaked out for {round} turn(s). As a result you wont be able to perform attack during those round(s).");
                    break;
                case Effect.Mistake:
                    refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0].NickName} uses the {attackName} and makes you make mistakes for {round} turn(s). As a result, the public likes you less and you lose 5% appreciation during these rounds.");
                    break;
                case Effect.Stressed:
                    refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0].NickName} uses the {attackName} and makes you feel stressed for {round} turn(s). As a result, you have to play your attacks twice as fast (so it's advisable to use attacks that are easier to play).");
                    break;
                case Effect.None:
                    refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0].NickName} uses the {attackName}.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effect), effect, null);
            }
            // HERE WE APPLY THE EFFECT OF THE ATTACK
            if (effect != Effect.None)
            {
                Debug.Log($"CM effector -> applying the effect for this client {effect}");
                meEffects[effect] = round;
            }
            
            // HERE WE PERFORM THE CALCULUS OF THE NEW APPRECIATION
            
            // APPLY RESISTANCE TO THE DAMAGE
            float afterResistanceDmg = damage - damage * (me.resistance / (20 + me.maxResistance));
            me.resistance -= impactResistance;
            refHud.UpdateResistance(me.resistance);
            
            //CALCULUS OF THE NEW INSPIRATION
            float newAppreciation = 0;
            if (PhotonNetwork.IsMasterClient)
            {
                newAppreciation = appreciation += afterResistanceDmg;
            }
            else
            {
                newAppreciation = appreciation -= afterResistanceDmg;
            }
            Debug.Log("ENTERED 3");
            //CALL UPDATE_RESISTANCE RPC FUNCTION
            photonView.RPC("UpdateAppreciation", RpcTarget.All, newAppreciation);
            yield return new WaitForSeconds(3f);
            Debug.Log("CM effector -> updateState called");
            photonView.RPC("UpdateState", RpcTarget.All, currentState==BattleState.HOST_TURN?BattleState.GUEST_TURN:BattleState.HOST_TURN);
        }
    }

    [PunRPC]
    IEnumerator UsedAnConsumable(string consumableName, int inspirationGain, int resistanceGain)
    {
         if (currentState == BattleState.HOST_TURN && PhotonNetwork.IsMasterClient || currentState == BattleState.GUEST_TURN && !PhotonNetwork.IsMasterClient)
        {
            refHud.SetDialogText($"You use {consumableName} and gain {inspirationGain} inspiration and {resistanceGain} resistance.");
            me.resistance += resistanceGain;
            me.inspiration += inspirationGain;
            refHud.UpdateInspiration(me.resistance);
            refHud.UpdateInspiration(me.inspiration);
        }else if (currentState == BattleState.HOST_TURN && !PhotonNetwork.IsMasterClient ||
                  currentState == BattleState.GUEST_TURN && PhotonNetwork.IsMasterClient)
        {
            refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0]} use {consumableName} and gain {inspirationGain} inspiration and {resistanceGain} resistance.");
        }

         yield return new WaitForSeconds(3f);   
         
         
         if (PhotonNetwork.IsMasterClient)
            photonView.RPC("UpdateState", RpcTarget.All, currentState==BattleState.HOST_TURN?BattleState.GUEST_TURN:BattleState.HOST_TURN);


    }
    
    [PunRPC]
    IEnumerator FailedAttack(string atk)
    {
        Debug.Log("entered FailedAnAttack function");
        if (currentState == BattleState.HOST_TURN && PhotonNetwork.IsMasterClient ||
            currentState == BattleState.GUEST_TURN && !PhotonNetwork.IsMasterClient)
        {
            refHud.SetDialogText($"You failed to to perform {atk}.");
        }
        else
        {
            refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0]} failed to perform {atk}.");
        }
        yield return new WaitForSeconds(3f);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("CM effector -> updateState called");
            photonView.RPC("UpdateState", RpcTarget.All, currentState==BattleState.HOST_TURN?BattleState.GUEST_TURN:BattleState.HOST_TURN);
        }
    }

    
    /// <summary>
    /// This function is called whenever there is an appreciation update
    /// </summary>
    /// <param name="Appreciation updt"></param>
    [PunRPC]
    void UpdateAppreciation(float _appreciation)
    {
        //UPDATE APPRECIATION VAR AND UI
        if (PhotonNetwork.IsMasterClient)
        {
            //IF IT'S ON HOST SIDE THE BAR DOESNT NEED TO BE INVERTED
            appreciation = _appreciation;
            refHud.UpdateAppreciation(100-appreciation);
        }
        else
        {
            //ON GUEST SIDE EVERY UPDATE NEED AN APPRECIATION UPDATE
            appreciation = _appreciation;
            refHud.UpdateAppreciation(appreciation);
        }
        //LOGIC ON THE HOST SIDE TO CHECK IF HOST OR GUEST WIN
        if (PhotonNetwork.IsMasterClient)
        {
            if (appreciation < 0.1 || appreciation > 99.9)
            {
                Debug.Log("CM effector -> updateState called");
                photonView.RPC("UpdateState", RpcTarget.All, BattleState.END_OF_COMBAT);
            }
        }
    }

    /// <summary>
    /// THIS FUNCTION HANDLE THE DISPLAY MESSAGE FOR THE EFFECT STATUS.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="rounds"></param>
    [PunRPC]
    void DisplayEffect(Effect e, int rounds)
    {
        Debug.Log($"DisplayEffect() -> rounds : {rounds}");
        // CLIENT TURN
        if (currentState == BattleState.HOST_TURN && PhotonNetwork.IsMasterClient || currentState == BattleState.GUEST_TURN && !PhotonNetwork.IsMasterClient)
        {
            if (e == Effect.FreakOut)
                refHud.SetDialogText($"You are freaked out for another {rounds-1} turns. As a result you cant play this round.");
            else if (e == Effect.Stressed)
                refHud.SetDialogText($"You are stressed for another {rounds-1} turns. As a result you need to play twice as fast.");
            else
                refHud.SetDialogText($"You make mistakes for another {rounds-1} turns. As a result, the public likes you less and you loses 5% appreciation during these rounds.");
        }
        // OBVIOUSLY NOT CLIENT TURN
        else if (currentState == BattleState.HOST_TURN && !PhotonNetwork.IsMasterClient ||
                  currentState == BattleState.GUEST_TURN && PhotonNetwork.IsMasterClient)
        {
            if (e == Effect.FreakOut)
                refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0]} is freaked out for another {rounds-1} turns. As a result he cant play this round.");
            else if (e == Effect.Stressed)
                refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0]} is stressed for another {rounds-1} turns. As a result he need to play twice as fast.");
            else
                refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0]} is mistakes for another {rounds-1} turns. As a result, the public likes him less and he loses 5% appreciation during these rounds.");
        }
    }

    /// <summary>
    /// This function is triggered when the opponent declare forfeit or leave the match.
    /// </summary>
    [PunRPC]
    IEnumerator DisplayForfeit()
    {
        refHud.SetDialogText("Your opponent forfeits, so you win.");
        yield return new WaitForSeconds(3f);
        
    }
    

}
