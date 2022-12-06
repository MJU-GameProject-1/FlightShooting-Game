using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 50;
    private float currentHP;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    public bool isInvincible = false;

    public float MaxHP => maxHP; //maxHP변수에 접근할 수 있는 프로퍼티(Get만 가능)
    public float CurrentHP // currentHP변수에 접근할 수 있는 프로퍼티 (Set, Get 가능)
    {
        set => currentHP = Mathf.Clamp(value, 0, maxHP);
        get => currentHP;
    }

    private void Awake()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();   
        playerController = GetComponent<PlayerController>();     
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            currentHP -= 0;
        } 
        else 
        {
            //현재 체력을 damage만큼 감소
            currentHP -= damage;
            StopCoroutine("HitColorAnimation");
            StartCoroutine("HitColorAnimation");
        }

        if ( currentHP <= 0 )
        {
            //체력이 0이면 OnDie()함수를 호출해서 죽었을 때 처리를 한다
            playerController.OnDie();
        }
    }

    private IEnumerator HitColorAnimation()
    {
        //플레이어의 색상을 빨간색으로
        spriteRenderer.color = Color.red;

        //0.1초 동안 대기
        yield return new WaitForSeconds(0.1f);
        //플레이어의 색상을 원래 색상 하얀색으로
        spriteRenderer.color = Color.white;
    }

    public IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(3);
        isInvincible = false;
    }

}

/*
 * File : PlayerHP.cs
 *
 */