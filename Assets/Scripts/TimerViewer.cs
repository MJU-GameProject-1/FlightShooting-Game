using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerViewer : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField]
    private TextMeshProUGUI textTime;
    float time = 100.0f;

    public void SetUp(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    private void Awake()
    {
        this.textTime = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        this.time -= Time.deltaTime;
        textTime.text = "Time " + this.time.ToString("F1");
        if (this.time < 0)
        {
            this.time = 0;
            SceneManager.LoadScene("GameOver");
        }
    }
}
