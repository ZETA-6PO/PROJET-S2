using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MPerformAttack : MonoBehaviour
{
    public Text timer;
    public Text touch;
    private List<KeyCode> sequences;
    private UnityEvent<bool,AttackObject> onCompleteAttack;
    private AttackObject attack;
    public Animator animator;
    private bool started;
    private bool end;
    private float timeLeft;
        /// <summary>
    /// This function start an attack
    /// </summary>
    public IEnumerator StartAttack(List<KeyCode> touchSequences, AttackObject attack, UnityEvent<bool,AttackObject> onCompleteAttack)
    {
        sequences = touchSequences;
        this.onCompleteAttack = onCompleteAttack;
        this.attack = attack;
        gameObject.SetActive(true);
        timer.gameObject.SetActive(false); // on affiche pas le timer tant que ca a pas commemce
        int count = 5;
        while (count > 0)
        {
            touch.text = $"STARTING IN {count} SECONDS.";
            yield return new WaitForSeconds(1);
            count -= 1;
        }
        
        SoundManager.Instance.PlaySound(attack.sound);
        touch.text = sequences[0].ToString();
        started = true;
        timer.gameObject.SetActive(true);
        timeLeft = attack.sound.length;
        yield return StartCoroutine(Timer());

    }

    public IEnumerator  Timer()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
            timer.text = $"{timeLeft}s";
        }
        end = true;

        
    }

    private void Update()
    {
        if (started)
        {
            if (sequences.Count == 0)
            {
                started = false;
                end = true;
                return;
            }

            
            
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (sequences[0] == KeyCode.DownArrow)
                {
                    sequences.RemoveAt(0);
                    if (sequences.Count > 0)
                    {
                        animator.SetTrigger("Start");
                        touch.text = sequences[0].ToString();
                    }
                }
                else
                {
                    end = true;
                    started = false;
                }
                return;

            }
            
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (sequences[0] == KeyCode.UpArrow)
                {
                    sequences.RemoveAt(0);
                    if (sequences.Count > 0)
                    {
                        animator.SetTrigger("Start");
                        touch.text = sequences[0].ToString();
                    }
                }
                else
                {
                    end = true;
                    started = false;
                }
                return;
            }
            
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Debug.Log("merde");
                if (sequences[0] == KeyCode.LeftArrow)
                {
                    Debug.Log("dick");
                    sequences.RemoveAt(0);
                    if (sequences.Count > 0)
                    {
                        animator.SetTrigger("Start");
                        touch.text = sequences[0].ToString();
                    }
                }
                else
                {
                    end = true;
                    started = false;
                }
                return;
            }
            
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (sequences[0] == KeyCode.RightArrow)
                {
                    sequences.RemoveAt(0);
                    if (sequences.Count > 0)
                    {
                        animator.SetTrigger("Start");
                        touch.text = sequences[0].ToString();
                    }
                }
                else
                {
                    end = true;
                    started = false;
                }
                return;
                
            }
            
            
        }

        if (end)
        {
            end = false;
            CombatManager cm = FindObjectOfType<CombatManager>();
            if (sequences.Count == 0)
            {
                onCompleteAttack.Invoke(true, attack);
                SoundManager.Instance.StopMusic();
                SoundManager.Instance.PlaySound(cm.onAttackSucceeded);
                gameObject.SetActive(false);
            }
            else
            {
                onCompleteAttack.Invoke(false, attack);
                SoundManager.Instance.StopMusic();
                SoundManager.Instance.PlaySound(cm.onAttackFailed);
                gameObject.SetActive(false);
            }
            
        }
    }
}
