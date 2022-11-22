using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BossState { MoveToApperPoint = 0, Phase01, Phase02, Phase03 }

public class Boss : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private string nextSceneName; //���� �� �̸�( Ŭ���� or ���� ����������)
    [SerializeField]
    private float bossAppearPoint = 2.5f;
    private BossState bossState = BossState.MoveToApperPoint;
    private Movement2D movement2D;
    private BossWeapon bossweapon;
    private BossHP bossHP;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        bossweapon = GetComponent<BossWeapon>();
        bossHP = GetComponent<BossHP>();
    }

    public void ChangeState(BossState newState)
    {
        // ������ ������̴� ���� ����
        StopCoroutine(bossState.ToString());
        // ���� ����
        bossState = newState;
        // ���ο� ���� ���
        StartCoroutine(bossState.ToString());
    }

    private IEnumerator MoveToApperPoint()
    {
        // �̵����� ���� [�ڷ�ƾ ���� �� 1ȸ ȣ��]
        movement2D.MoveTo(Vector3.down);

        while (true)
        {
            if (transform.position.y <= bossAppearPoint)
            {
                //�̵������� (0,0,0)���� ������ �������� �����.
                movement2D.MoveTo(Vector3.zero);
                // Phase01 ���·� ����
                ChangeState(BossState.Phase01);
            }

            yield return null;
        }
    }
    private IEnumerator Phase01()
    {
        //�� ������ ��� ���� ����
        bossweapon.StartFiring(AttackType.CircleFire);

        while (true)
        {
            while (true)
            {   // ������ ���� ü���� 70% ���ϰ� �Ǹ�
                if (bossHP.CurrentHP <= bossHP.MaxHP * 0.7f)
                {
                    // �� ��� ������ ���� ����
                    bossweapon.StopFiring(AttackType.CircleFire);
                    // phase02�� ����
                    ChangeState(BossState.Phase02);
                }
                yield return null;
            }
        }
    }
    private IEnumerator Phase02()
        {
            // �÷��̾� ��ġ�� �������� ���� �߻�ü ���� ����
            bossweapon.StartFiring(AttackType.SingleFireToCenterPosition);

            // ó�� �̵� ������ ���������� ����
            Vector3 direction = Vector3.right;
            movement2D.MoveTo(direction);

            while (true)
            {   // ��-�� �̵� �� ���� ���� �����ϰ� �Ǹ� ������ �ݴ�� ����
                if (transform.position.x < stageData.LimitMin.x ||
                    transform.position.x > stageData.LimitMax.x)
                {
                    direction *= -1;
                    movement2D.MoveTo(direction);
                }
                // ������ ���� ü���� 30%���ϰ� �Ǹ�
                if (bossHP.CurrentHP <= bossHP.MaxHP * 0.3f)
            {
                // �÷��̾� ��ġ�� �������� ���� �߻�ü ���� ����
                bossweapon.StopFiring(AttackType.SingleFireToCenterPosition);
                // phase03���� ����
                ChangeState(BossState.Phase03);
            }
            yield return null;

            }
        }
    private IEnumerator Phase03()
    {
        //  �� ��� ������ ���� ����
        bossweapon.StartFiring(AttackType.CircleFire);
        // �÷��̾� ��ġ�� �������� ���� �߻�ü ���� ����
        bossweapon.StartFiring(AttackType.SingleFireToCenterPosition);

        // ó�� �̵� ������ ���������� ����
        Vector3 direction = Vector3.right;
        movement2D.MoveTo(direction);

        while (true)
        {   // ��-�� �̵� �� ���� ���� �����ϰ� �Ǹ� ������ �ݴ�� ���� 
            if (transform.position.x <= stageData.LimitMin.x || 
                transform.position.x >= stageData.LimitMax.x)
            {
                direction *= -1;
                movement2D.MoveTo(direction);
            }

            yield return null;
        }

    }
    public void OnDie()
    {
        //���� �ı� ��ƼŬ ����
        GameObject clone = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // ��ƼŬ ��� �Ϸ� �� �� ��ȯ�� ���� ����
        clone.GetComponent<BossExplosion>().SetUp(playerController, nextSceneName);
        // ���� ������Ʈ ����
        Destroy(gameObject);
    }
}