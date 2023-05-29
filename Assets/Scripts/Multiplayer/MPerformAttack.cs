using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MPerformAttack : MonoBehaviour
{
    public Text timer;
    public Text touch;
    private List<KeyCode> sequences = new List<KeyCode>();
    private UnityEvent<bool,AttackObject> onCompleteAttack;
    private AttackObject attack;
    public Animator animator;
    private bool started;
    private bool end;
    private float timeLeft;
    
    public Sprite leftArrow;
    public Sprite rightArrow;
    public Sprite upArrow;
    public Sprite downArrow;
    public Image refRenderedArrow;
    
        /// <summary>
    /// This function start an attack
    /// </summary>
    public IEnumerator StartAttack(AttackObject attack, UnityEvent<bool,AttackObject> onCompleteAttack, bool isStressed)
    {
        refRenderedArrow.gameObject.SetActive(false);
        this.onCompleteAttack = onCompleteAttack;
        this.attack = attack;
        gameObject.SetActive(true);
        timer.gameObject.SetActive(false);
        timer.color = Color.black;// on affiche pas le timer tant que ca a pas commemce
        int count = 5;
        while (count > 0)
        {
            touch.text = $"STARTING IN {count} SECONDS.";
            yield return new WaitForSeconds(1);
            count -= 1;
        }

        //SoundManager.Instance.effectsSource.pitch = 2;
        if (SoundManager.Instance == null)
            Debug.Log("ntm");
        SoundManager.Instance.PlaySound(
            attack.sound
        );
        float coef = 0;
        switch (attack.rarity)
        {
            case Rarity.Common:
                coef = 0.5f;
                break;
            case Rarity.Hyped:
                coef = 0.75f;
                break;
            case Rarity.Legendary:
                coef = 1.25f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        int nbInput = (int)(coef * attack.sound.length);
        KeyCode[] availableKey = new[]
        {
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow
        };
        for (int i = 0; i < nbInput; i++)
        {
            sequences.Add(availableKey[Random.Range(0,4)]);
        }
        touch.gameObject.SetActive(false);
        started = true;
        timer.gameObject.SetActive(true);
        refRenderedArrow.gameObject.SetActive(true);
        SetArrow(sequences[0]);

        if (isStressed)
            timeLeft = attack.sound.length;
        else
            timeLeft = this.attack.sound.length / 2;


        
        yield return StartCoroutine(Timer());

    }

    public IEnumerator  Timer()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
            timer.text = $"{timeLeft}s";
            if (timeLeft <= 3f && timer.color != Color.red)
            {
                timer.color = Color.red;
            }
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
                        SetArrow(sequences[0]);
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
                        SetArrow(sequences[0]);
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

                if (sequences[0] == KeyCode.LeftArrow)
                {
                    sequences.RemoveAt(0);
                    if (sequences.Count > 0)
                    {
                        animator.SetTrigger("Start");
                        SetArrow(sequences[0]);
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
                        SetArrow(sequences[0]);
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
            CombatManager bs = FindObjectOfType<CombatManager>();
            if (sequences.Count == 0)
            {
                onCompleteAttack.Invoke(true, attack);
                SoundManager.Instance.StopSound();
                SoundManager.Instance.effectsSource.pitch = 1;
                SoundManager.Instance.PlaySound(bs.onAttackSucceeded);
                gameObject.SetActive(false);
                touch.gameObject.SetActive(true);
                timer.gameObject.SetActive(false);
            }
            else
            {
                onCompleteAttack.Invoke(false, attack);
                SoundManager.Instance.StopSound();
                SoundManager.Instance.effectsSource.pitch = 1;
                SoundManager.Instance.PlaySound(bs.onAttackFailed);
                gameObject.SetActive(false);
                touch.gameObject.SetActive(true);
                timer.gameObject.SetActive(false);
            }
            
        }
    }


    void SetArrow(KeyCode arrow)
    {
        if (refRenderedArrow is null)
            return;

        switch (arrow)
        {
            case KeyCode.UpArrow:
                refRenderedArrow.sprite = upArrow;
                break;
            case KeyCode.DownArrow:
                refRenderedArrow.sprite = downArrow;
                break;
            case KeyCode.RightArrow:
                refRenderedArrow.sprite = rightArrow;
                break;
            case KeyCode.LeftArrow:
                refRenderedArrow.sprite = leftArrow;
                break;
            default:
                throw new InvalidEnumArgumentException();

        }
    }
}
