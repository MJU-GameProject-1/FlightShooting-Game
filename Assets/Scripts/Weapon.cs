using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; // 공격할 때 생성되는 발사체 프리팹
    [SerializeField]
    private float attackRate = 0.1f; // 공격속도
    [SerializeField]
    private int maxAttackLevel = 3;  // 공격 최대 레벨
    private int attackLevel = 1;  // 공격 레벨

    [SerializeField]
    private GameObject boomPrefab;  // 폭탄 프리팹
    private int boomCount = 3;  // 생성 가능한 폭탄

    public int AttackLevel
    {
        set => attackLevel = Mathf.Clamp(value, 1, maxAttackLevel);
        get => attackLevel;
    }

    public int BoomCount
    {
        set => boomCount = Mathf.Max(0, value);
        get => boomCount;
    }
    
    public void StartFiring()
    {
        StartCoroutine("TryAttack");
    }

    public void StopFiring()
    {
        StopCoroutine("TryAttack");
    }

    public void StartBoom()
    {
        if (boomCount > 0)
        {
            boomCount--;
            Instantiate(boomPrefab, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator TryAttack()
    {
        while (true)
        {
            //발사체 오브젝트 생성
            //Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // 공격 레벨에 따라 발사체 생성
            AttackByLevel();

            // attackRate 시간만큼 대기
            yield return new WaitForSeconds(attackRate);
        }
    }

    private void AttackByLevel()
    {
        GameObject cloneProjectile = null;

        switch (attackLevel)
        {
            case 1:  // Level 01: 기존과 같이 발사체 1개 생성
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                break;
            case 2:  // Level 02: 간격을 두고 전방으로 발사체 2개 생성
                Instantiate(projectilePrefab, transform.position + Vector3.left * 0.2f, Quaternion.identity);
                Instantiate(projectilePrefab, transform.position + Vector3.right * 0.2f, Quaternion.identity);
                break;
            case 3:  // Level 03: 전방으로 발사체 1개, 좌우 대각선 방향으로 발사체 각 1개
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                // 왼쪽 대각선 방향으로 발사되는 발사체
                cloneProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                cloneProjectile.GetComponent<Movement2D>().MoveTo(new Vector3(-0.2f, 1, 0));
                // 오른쪽 대각선 방향으로 발사되는 발사체
                cloneProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                cloneProjectile.GetComponent<Movement2D>().MoveTo(new Vector3(0.2f, 1, 0));
                break;
        }

        // Tip. Movement2D에 접근한 방식과 같은 방식으로
        // Projectile 클래스의 damage 변수에 접근할 수 있도록 설정한 후 공격력도 다르게 설정 가능
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
