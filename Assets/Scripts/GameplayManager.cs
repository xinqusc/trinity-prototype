using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static GameObject m_character;
    private static readonly HashSet<HaltTimer> halts = new();
    private static float m_timer;
    private static SendToGoogle m_googleSender;

    // Start is called before the first frame update
    void Start()
    {
        m_character = GameObject.FindWithTag("Character");
        m_googleSender = GetComponent<SendToGoogle>();
        UIManager.m_timerText = GameObject.FindWithTag("TimerText").GetComponent<TextMeshProUGUI>();
        UIManager.m_gameplayPanel = GameObject.FindWithTag("GameplayPanel");
        UIManager.m_shopPanel = GameObject.FindWithTag("ShopPanel");
        UIManager.m_completePanel = GameObject.FindWithTag("CompletePanel");
        UIManager.m_activeSkillPanel = GameObject.FindWithTag("ActiveSkillPanel");

        Camera.main.GetComponent<CameraFocus>().SetFocus(m_character);
        LevelManager.Reset();
        m_timer = float.PositiveInfinity;
        MapManager.Initialize(LevelManager.GetMapSize(), LevelManager.GetTile(), LevelManager.GetWatermarks());
        //UIManager.m_gameplayPanel.SetActive(false);
        UIManager.m_shopPanel.SetActive(false);
        UIManager.m_completePanel.SetActive(false);
        //Pause();
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (halts.Count == 0)
            m_timer -= Time.deltaTime;
        UIManager.UpdateTimerText();
        UIManager.UpdateActiveSkills(m_character.GetComponent<Character>().GetKeyCodeActiveItemPairs());
        if (m_character.GetComponent<Character>().GetHealth() <= 0.0f)
        {
            GameOver();
            return;
        }
        if (m_timer <= 0)
        {
            //m_googleSender.SendMatrix3(LevelManager.GetLevelName(), ActiveItem_2.activateCounter, ActiveItem_0.activateCounter, m_character.GetComponent<ActiveItem_2>() != null, m_character.GetComponent<ActiveItem_0>() != null);
            if (LevelManager.GetShopOptions().Count > 0)
                OpenShop();
            LevelManager.MoveNext();
            LoadLevel();
        }
    }

    public static GameObject getCharacter() => m_character;
    public static SendToGoogle GetGoogleSender() => m_googleSender;

    private static void LoadLevel()
    {
        Clear();
        m_timer = LevelManager.GetTimeLimit();
        MapManager.Initialize(LevelManager.GetMapSize(), LevelManager.GetTile(), LevelManager.GetWatermarks());
        foreach (KeyValuePair<Vector2, Type[]> kvp in LevelManager.GetInitEneimies())
        {
            GameObject enemy = Enemy.Instantiate(kvp.Key, Quaternion.identity);
            foreach (Type componentType in kvp.Value)
            {
                enemy.AddComponent(componentType);
            }
        }
    }

    private static void GameOver()
    {
        Clear();
        LevelManager.Reset();
        m_character.GetComponent<Character>().SetHealth(m_character.GetComponent<Character>().GetMaxHealth());
        LoadLevel();
    }

    private static void Clear()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Disposable"))
            Destroy(o);
        m_character.transform.position = Vector3.zero;
        foreach (TrailRenderer trailRenderer in m_character.GetComponentsInChildren<TrailRenderer>())
            trailRenderer.Clear();
        ActiveItem_0.activateCounter = 0;
        ActiveItem_2.activateCounter = 0;
    }

    public static float getTimer() { return m_timer; }
    public static void Pause()
    {
        UIManager.m_gameplayPanel.SetActive(false);
        Time.timeScale = 0.0f;
    }
    public static void Continue()
    {
        UIManager.m_gameplayPanel.SetActive(true);
        Time.timeScale = 1.0f;
    }
    public static void OpenShop()
    {
        Pause();
        UIManager.m_shopPanel.SetActive(true);
        UIManager.SetShopOptions(LevelManager.GetShopOptions());
    }

    public static void CloseShop()
    {
        UIManager.m_shopPanel.SetActive(false);
        Continue();
    }

    public static void AddHalt(HaltTimer halt)
    {
        halts.Add(halt);
    }

    public static void RemoveHalt(HaltTimer halt)
    {
        halts.Remove(halt);
    }
}
