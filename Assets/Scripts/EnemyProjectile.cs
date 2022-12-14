using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private GameObject explosionPrefab; //  폭발 효과

    private void OnTriggerEnter2D(Collider2D collision)
    {   // 발사체에 부딪힌 오브젝트의 태그가 "Player"이면
        if (collision.CompareTag("Player"))
        {// 부딪힌 오브젝트에 의해 체력 감소(Player)
            collision.GetComponent<PlayerHP>().TakeDamage(damage);
            // 내 오브젝트 삭제(발사체)
            Destroy(gameObject);
        }
    }
    public void OnDie()
    {   //폭발 효과 생성
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        // 적/보스 발사체 삭제
        Destroy(gameObject);
    }
}