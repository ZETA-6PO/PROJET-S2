using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    /// <summary>
    /// REFERENCES
    /// </summary>
    public MBattleHUD refHud;

    public PhotonView photonView;
    public MAttackMenu refAttackMenu;
    public MPerformAttack refPerformAttack;
    public AttackObject[] attackList; //contains all the attack in the game
    public AudioClip onAttackSucceeded; //this is the sound played when an attack succeed
    public AudioClip onAttackFailed; //this is the sound played when an attack failed
    
    /// <summary>
    /// COMBAT STATS
    /// </summary>
    public float appreciation;

    public BattleState currentState;
    public bool menuOpened = false;
    
    public MultiplayerFighter opponent;
    public MultiplayerFighter me;

    public UnityEvent<bool, AttackObject> onAttackComplete = new UnityEvent<bool, AttackObject>();

    [Serializable]
    public enum BattleState
    {
        BEGINNING,
        HOST_TURN,
        OTHER_TURN,
        END_OF_COMBAT
    }

    void Start()
    {
        appreciation = 50;
        refHud.UpdateAppreciation(appreciation);
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetStuff", RpcTarget.All, new int[] { 5, 6, 7, 8 }, new int[] { 1, 2, 3, 4 });
        }
    }

    [PunRPC]
    IEnumerator SetStuff(int[] attackFighter1, int[] attackFighter2)
    {
        AttackObject[] _tmp1 = new AttackObject[4];
        AttackObject[] _tmp2 = new AttackObject[4];
        for (int i = 0; i < attackFighter1.Length; i++)
        {
            _tmp1[i] = attackList[attackFighter1[i]];
            _tmp2[i] = attackList[attackFighter2[i]];
        }

        me = new MultiplayerFighter(PhotonNetwork.NickName, PhotonNetwork.IsMasterClient ? _tmp1 : _tmp2);
        opponent = new MultiplayerFighter(PhotonNetwork.PlayerListOthers[0].NickName,
            PhotonNetwork.IsMasterClient ? _tmp2 : _tmp1);
        yield return new WaitForSeconds(3);
        photonView.RPC("UpdateState", RpcTarget.All, BattleState.BEGINNING);

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
        if (bs == BattleState.BEGINNING)
        {
            refHud.Initialize(me, opponent);
            refHud.SetDialogText(
                $"Welcome to this musical battle between {PhotonNetwork.NickName} and {PhotonNetwork.PlayerListOthers[0]}, it's going to be noisy, be careful ! ");
            yield return new WaitForSeconds(4);
            if (PhotonNetwork.IsMasterClient)
            {
                refHud.SetDialogText($"{PhotonNetwork.NickName} will start, let's hear what he has to offer !");
                yield return new WaitForSeconds(2);
                photonView.RPC("UpdateState", RpcTarget.All, BattleState.HOST_TURN);
            }
            else
            {
                refHud.SetDialogText(
                    $"{PhotonNetwork.PlayerListOthers[0].NickName} will start, let's hear what he has to offer !");
            }
        }
        else if (bs == BattleState.HOST_TURN)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                refHud.SetDialogText($"It's your turn, please perform an attack or heal.");

            }
            else
            {
                refHud.SetDialogText($"It's {PhotonNetwork.PlayerListOthers[0].NickName}'s turn, wait for him to act.");
            }
        }
    }

    [PunRPC]
    void UsedAnAttack(AttackObject attackObject)
    {
        refHud.SetDialogText($"{PhotonNetwork.PlayerListOthers[0]} used {attackObject.name}.");
        refHud.UpdateAppreciation(appreciation);
    }

    [PunRPC]
    void UsedAnConsumable(Consumable consumableObject)
    {
        refHud.SetDialogText(
            $"{PhotonNetwork.PlayerListOthers[0]} used an consumable {consumableObject.name} and gain some appreciation.");
        appreciation -= consumableObject.addedInspiration;
        refHud.UpdateAppreciation(appreciation);
    }

    /// <summary>
    /// On click on the heal button.
    /// </summary>
    public void OnClickAttack()
    {
        if (menuOpened)
        {
            return;
        }

        //check if it's player turn.
        if (currentState == BattleState.HOST_TURN && PhotonNetwork.IsMasterClient ||
            currentState == BattleState.OTHER_TURN && !PhotonNetwork.IsMasterClient)
        {
            menuOpened = true;
            refAttackMenu.OpenMenu(me, (AttackObject a) =>
            {
                menuOpened = false;
                StartCoroutine(refPerformAttack.StartAttack(a.input.sequence.ToList(), a, onAttackComplete));
            });
        }
    }

    /// <summary>
    /// When a player perform an attack;
    /// </summary>
    public void OnAttackComplete(bool succeeded, AttackObject a)
    {
        photonView.RPC("UsedAnAttack", RpcTarget.All, a);
    }


    /// <summary>
    /// On click on the attack button.
    /// </summary>
    public void OnClickHeal()
    {
        if (menuOpened)
        {
            return;
        }
        /*//check if it's player turn.
        if (currentState == BattleState.HOST_TURN && PhotonNetwork.IsMasterClient || currentState == BattleState.OTHER_TURN && !PhotonNetwork.IsMasterClient)
        {
            menuOpened = true;
            refAttackMenu.OpenMenu(me, o => 
            {
                menuOpened = false;
            });
        }*/
    }
}