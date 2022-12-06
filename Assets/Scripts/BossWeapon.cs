using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { HalfCircleFire = 0, SingleFireToCenterPosition,Attack}

public class BossWeapon : MonoBehaviour
{
    public float speed;
    public GameObject Player;

    [SerializeField]
    private GameObject projectilePrefab; // 공격할 때 생성되는 발사체 프리팹
    
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    public void StartFiring(AttackType attackType)
    {
        // attackType 열거형의 이름과 같은 코루틴을 실행
        StartCoroutine(attackType.ToString());
    }

    public void StopFiring(AttackType attackType)
    {   // attackType 열거형의 이름과 같은 코루틴을 중지
        StopCoroutine(attackType.ToString());
    }

    private IEnumerator HalfCircleFire()
    {
        float attackRate = 1.5f;            // 공격 주기
        int count = 30;                      // 발사체 생성 개수
        float intervalAngle = -180 / count;    // 발사체 사이의 각도
        float weightAngle = 0;              // 가중되는 각도 (항상 같은 위치로 발사하지 않도록 설정)
        // 원형태로 방사하는 발사체 생성 (count 개수만큼)
        while (true)
        {
            for (int i = 0; i < count; i++)
            {   // 발사체 생성
                GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

                // 발사체 이동 방향(각도)
                float angle = weightAngle + intervalAngle * i;
                // 발사체 이동 방향(벡터)
                float x = Mathf.Cos(angle * Mathf.PI / 180.0f); // Cos(각도), PI / 180을 곱함
                float y = Mathf.Sin(angle * Mathf.PI / 180.0f); // Sin(각도), PI / 180을 곱함
                // 발사체 이동 방향 설정
                clone.GetComponent<Movement2D>().MoveTo(new Vector2(x, y));
            }
            // 발사체가 생성되는 시작 각도 설정을 위한 변수
            weightAngle += 1;
            // attackRate 시간만큼 대기
            yield return new WaitForSeconds(attackRate);
        }
    }
    private IEnumerator SingleFireToCenterPosition()
    {
        Vector3 targetPosition = Vector3.zero; // 목표 위치 중앙
        float attackRate = 0.2f;

        while (true)
        {   // 발사체 생성
            GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // 발사체 이동방향
            Vector3 direction = (targetPosition - clone.transform.position).normalized;

            // 발사체 이동 방향 설정
            clone.GetComponent<Movement2D>().MoveTo(direction);
            // attackRate 시간만큼 대기
            yield return new WaitForSeconds(attackRate);
        }
    }
    private IEnumerator Attack()
    {
        float attackRate = 0.3f;
        while (true)
        {
            GameObject clone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector3 direction = (Player.transform.position - clone.transform.position).normalized;
            float vx = direction.x * speed;
            float vy = direction.y * speed;
            //x축을 넘어가면 반전하기
            this.GetComponent<SpriteRenderer>().flipX = (vx < 0);
            clone.GetComponent<Movement2D>().MoveTo(direction);
            yield return new WaitForSeconds(attackRate);
        }
    }
}