using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    [SerializeField]
    private bool isFrozen = false;

    private void Update()
    {
        if (isFrozen)
        {
            return;
        }
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }

    public IEnumerator Freeze()
    {
        EnemySpawner enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        enemySpawner.SetPause(true);
        isFrozen = true;
        yield return new WaitForSeconds(1);
        enemySpawner.SetPause(false);
        isFrozen = false;
    }
}

/*
* File: Movement2D.cs
* Desc: 이동 가능한 모든 오브젝트에게 부착
* Functions: MoveTo() - 외부에서 호출해 이동 방향 설정 
*/