using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeViewer : MonoBehaviour
{
    public GameObject Player;
    private Movement2D movement2D;
    private Weapon weapon;
    private Animator animator;
    [SerializeField]
    private TextMeshProUGUI textTime;
    float time = 100.0f;
    private void Awake()
    {
        Player = GameObject.Find("Player");
        this.textTime = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        this.time -= Time.deltaTime;
        textTime.text = "Time " + Mathf.Round(time);
        if (this.time < 0)
        {
            this.time = 0;
            Player.GetComponent<PlayerController>().OnDie();
        }
    }
}
