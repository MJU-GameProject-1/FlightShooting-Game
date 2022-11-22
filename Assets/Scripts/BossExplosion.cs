using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossExplosion : MonoBehaviour
{
    private PlayerController playerController;
    private string sceneName;

    public void SetUp(PlayerController playerController, string sceneName)
    {
        this.playerController = playerController;
        this.sceneName = sceneName;
    }

    /*
     * ParticleAudoDestroy ������Ʈ���� ��ƼŬ ����� �Ϸ�Ǹ� ��ƼŬ�� �����ϱ� ������
     * ������Ʈ�� ������ �� ȣ��Ǵ� OnDestroy() �Լ��� �̿��� ��ƼŬ �����
     * �Ϸ�Ǿ��� �� �ʿ��� ó���� �����Ѵ�.
     */

    private void OnDestroy()
    {   // ���� óġ ���ʽ� ����
        playerController.Score += 10000;
        // Player�� ������ Score�� ����
        PlayerPrefs.SetInt("Score", playerController.Score);
        // sceneName���� �� ����
        SceneManager.LoadScene(sceneName);
    }
}