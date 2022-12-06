using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BossState {MoveToApperPoint = 0, Phase01,Phase02, Phase03, Phase04}

public class Boss : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private string nextSceneName; //다음 씬 이름( 클리어 or 다음 스테이지로)
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
        // 이전에 재생중이던 상태 종료
        StopCoroutine(bossState.ToString());
        // 상태 변경
        bossState = newState;
        // 새로운 상태 재생
        StartCoroutine(bossState.ToString());
    }

    private IEnumerator MoveToApperPoint()
    {
        // 이동방향 설정 [코루틴 실행 시 1회 호출]
        movement2D.MoveTo(Vector3.down);

        while (true)
        {
            if (transform.position.y <= bossAppearPoint)
            {
                //이동방향을 (0,0,0)으로 지정해 움직임을 멈춘다.
                movement2D.MoveTo(Vector3.zero);
                // Phase01 상태로 변경
                ChangeState(BossState.Phase01);
            }

            yield return null;
        }
    }
    private void LateUpdate()
    {
        // 보스 캐릭터가 화면 범위 밖으로 나가지 못하도록 설정
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
                                        Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }

    private IEnumerator Phase01()
    {
        //원 형태의 방사 공격 시작
        bossweapon.StartFiring(AttackType.Attack);

        while (true)
        {
            while (true)
            {   // 보스의 현재 체력이 75% 이하가 되면
                if (bossHP.CurrentHP <= bossHP.MaxHP * 0.75f)
                {
                    // 원 방사 형태의 공격 중지
                    bossweapon.StopFiring(AttackType.Attack);
                    // phase02로 변경
                    ChangeState(BossState.Phase02);
                }
                yield return null;
            }
        }
    }
    private IEnumerator Phase02()
    {
        //원 형태의 방사 공격 시작
        bossweapon.StartFiring(AttackType.HalfCircleFire);

        while (true)
        {
            while (true)
            {   // 보스의 현재 체력이 50% 이하가 되면
                if (bossHP.CurrentHP <= bossHP.MaxHP * 0.5f)
                {
                    // 원 방사 형태의 공격 중지
                    bossweapon.StopFiring(AttackType.HalfCircleFire);
                    // phase02로 변경
                    ChangeState(BossState.Phase03);
                }
                yield return null;
            }
        }
    }
    private IEnumerator Phase03()
        {
            // 플레이어 위치를 기준으로 단일 발사체 공격 시작
            bossweapon.StartFiring(AttackType.SingleFireToCenterPosition);

            // 처음 이동 방향을 오른쪽으로 설정
            Vector3 direction = Vector3.right;
            movement2D.MoveTo(direction);

            while (true)
            {   // 좌-우 이동 중 양쪽 끝에 도달하게 되면 방향을 반대로 설정
                if (transform.position.x < stageData.LimitMin.x ||
                    transform.position.x > stageData.LimitMax.x)
                {
                    direction *= -1;
                    movement2D.MoveTo(direction);
                }
                // 보스의 현재 체력이 25%이하가 되면
                if (bossHP.CurrentHP <= bossHP.MaxHP * 0.3f)
            {
                // 플레이어 위치를 기준으로 단일 발사체 공격 시작
                bossweapon.StopFiring(AttackType.SingleFireToCenterPosition);
                // phase03으로 변경
                ChangeState(BossState.Phase04);
            }
            yield return null;

            }
        }
    private IEnumerator Phase04()
    {
        //  원 방사 형태의 공격 시작
        bossweapon.StartFiring(AttackType.HalfCircleFire);
        // 플레이어 위치를 기준으로 단일 발사체 공격 시작
        bossweapon.StartFiring(AttackType.SingleFireToCenterPosition);

        // 처음 이동 방향을 왼쪽으로 설정
        Vector3 direction = Vector3.left;
        movement2D.MoveTo(direction);

        while (true)
        {   // 좌-우 이동 중 양쪽 끝에 도달하게 되면 방향을 반대로 설정 
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
        //보스 파괴 파티클 생성
        GameObject clone = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // 파티클 재생 완료 후 씬 전환을 위한 설정
        clone.GetComponent<BossExplosion>().SetUp(playerController, nextSceneName);
        // 보스 오브젝트 삭제
        Destroy(gameObject);
    }
}