using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 1000; // �ִ� ü��
    private float currentHP; //���� ü��
    private SpriteRenderer spriteRenderer;
    private Boss boss;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP; //���� ü���� �ִ� ü�°� ���� ����
        spriteRenderer = GetComponent<SpriteRenderer>();
        boss = GetComponent<Boss>();
    }

    public void TakeDamage(int damage)
    {   // ���� ü���� damage��ŭ ����
        currentHP -= damage;

        StopCoroutine("HitColorAnimation");
        StartCoroutine("HitColorAnimation");
        // ü���� 0���� = �÷��̾� ĳ���� ���
        if (currentHP <= 0)
        {
            boss.OnDie();
        }

    }

    private IEnumerator HitColorAnimation()
    {   //�÷��̾��� ������ ����������
        spriteRenderer.color = Color.red;
        //0.05�� ���� ���
        yield return new WaitForSeconds(0.05f);
        //�÷��̾��� ������ ���� ������ �Ͼ������
        // (���� ������ �Ͼ���� �ƴ� ��� ���� ���� ���� ����)
        spriteRenderer.color = Color.white;
    }
}