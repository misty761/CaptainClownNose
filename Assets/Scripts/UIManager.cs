﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject gotoSelectStageMenu;
    public GameObject jumpButton;
    public GameObject attackButton;
    public GameObject touchController;
    public GameObject healthBar;
    public GameObject score;
    public GameObject money;
    public GameObject playerLevel;

    private void Awake()
    {
        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 UI 가 존재합니다!");
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
