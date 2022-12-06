using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int damage = 1; //적 공격력

    [SerializeField]
    private int scorePoint = 100; //적 처치시 획득 점수
    
    [SerializeField]
    private GameObject explosionPrefab; //폭발 효과

    [SerializeField]
    private GameObject[] itemPrefabs;  // 적을 죽였을 때 획득 가능한 아이템

    private PlayerController playerController; //플레이어 점수(Score)에 접근하기 위해 

    public void Awake()
    {
        //Tip. 현재 코드에서는 한번만 호출하기 때문에 OnDie()에서 바로 호출해도 되지만
        //오브젝트 풀링을 이용해 오브젝트를 재사용할 경우에는 최초 1번만 Find를 이용해 
        //PlayerController의 정보를 저장해두고 사용하는 것이 연산에 효율적이다
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적에게 부딪힌 오브젝트의 태그가 "Player"
        if (collision.CompareTag("Player"))
        {
            //적 공격력만큼 플레이어 체력 감소
            collision.GetComponent<PlayerHP>().TakeDamage(damage);
            // 적 사망
            OnDie();
        }
    }

    public void OnDie()
    {
        //player의 점수를 scorePoint만큼 증가시킨다.
        playerController.Score += scorePoint;
        //폭발 이팩트 생성
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // 일정 확률로 아이템 생성
        SpawnItem();
        //적 오브젝트 삭제
        Destroy(gameObject);
    }

    private void SpawnItem()
    {
        // 파워업(5%), 폭탄+1(5%), 체력회복(15%), 얼음 효과(10%), 이동속도증가(10%), 일시무적(5%)
        int spawnItem = Random.Range(0, 100);
        if (spawnItem < 5)
        {
            Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
        }
        else if (spawnItem < 10)
        {
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
        }
        else if (spawnItem < 25)
        {
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);
        }
        else if (spawnItem < 35)
        {
            Instantiate(itemPrefabs[3], transform.position, Quaternion.identity);
        }
        else if (spawnItem < 45)
        {
            Instantiate(itemPrefabs[4], transform.position, Quaternion.identity);
        }
        else if (spawnItem < 50)
        {
            Instantiate(itemPrefabs[5], transform.position, Quaternion.identity);
        }
    }
}

/*
* File: Enemy.cs
* Desc: 적 캐릭터 오브젝트에 부착해서 사용
*/
