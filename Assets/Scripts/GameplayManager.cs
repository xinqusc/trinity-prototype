using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    private static GameObject m_character;
    private static CrisisManager m_crisisManager;
    private static float m_timer;
    private static SendToGoogle m_googleSender;
    private static Disarmed characterDisarmed;
    private static bool infiniteChallengeMode;
    private static float highestRecord;
    private static bool matrixSent;

    // Start is called before the first frame update
    void Start()
    {
        m_character = GameObject.FindWithTag("Character");
        m_crisisManager = GetComponent<CrisisManager>();
        m_googleSender = GetComponent<SendToGoogle>();
        UIManager.m_completePanel = GameObject.FindWithTag("CompletePanel");
        UIManager.m_levelSelectionPanel = GameObject.FindWithTag("LevelSelectionPanel");
        UIManager.m_losePanel = GameObject.FindWithTag("LosePanel");
        UIManager.m_timerText = GameObject.FindWithTag("TimerText").GetComponent<TextMeshProUGUI>();
        UIManager.m_gameplayPanel = GameObject.FindWithTag("GameplayPanel");
        UIManager.m_shopPanel = GameObject.FindWithTag("ShopPanel");
        UIManager.m_completePanel = GameObject.FindWithTag("CompletePanel");
        UIManager.m_activeSkillPanel = GameObject.FindWithTag("ActiveSkillPanel");
        UIManager.m_levelSelectionPanel = GameObject.FindWithTag("LevelSelectionPanel");
        UIManager.m_infiniteModePanel = GameObject.FindWithTag("InfiniteModePanel");
        UIManager.m_recordsPanel = GameObject.FindWithTag("RecordsPanel");

        Camera.main.GetComponent<CameraFocus>().SetFocus(m_character);
        LevelManager.Reset();
        LevelManager.SetLevelNum(LevelManager.GetLevelNum());
        m_timer = float.PositiveInfinity;
        MapManager.Initialize(LevelManager.GetMapSize(), LevelManager.GetTile());
        UIManager.m_gameplayPanel.SetActive(false);
        UIManager.m_shopPanel.SetActive(false);
        UIManager.m_completePanel.SetActive(false);
        UIManager.m_losePanel.SetActive(false);
        UIManager.m_infiniteModePanel.SetActive(false);
        UIManager.m_levelSelectionPanel.SetActive(true);
        UIManager.m_recordsPanel.SetActive(false);
        CrisisManager.Deactivate();
        infiniteChallengeMode = false;
        matrixSent = false;
        highestRecord = 0.0f;
        // Pause();
        // LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!HaltTimer.ExistInstance())
            m_timer = infiniteChallengeMode ? m_timer + Time.deltaTime : m_timer - Time.deltaTime;
        UIManager.UpdateTimerText();
        UIManager.UpdateActiveSkills(m_character.GetComponent<Character>().GetKeyCodeActiveItemPairs());
        if (m_character.GetComponent<Character>().GetHealth() <= 0.0f)
        {
            GameOver();
            return;

        }

        if ((!infiniteChallengeMode && m_timer <= 0) || (infiniteChallengeMode && m_timer >= 999.0f))
        {
            if (!matrixSent)
            {
                // Debug.Log("sending matrix");
                m_googleSender.SendMatrix3(LevelManager.GetLevelName(), 0, ActiveItem_0.activateCounter, false, m_character.GetComponent<ActiveItem_0>() != null);
                matrixSent = true;
            }
            //Debug.Log("num of shop options = " + LevelManager.GetShopOptions().Count);
            if (LevelManager.GetLevelNum() == LevelButtonsManager.numOfLevels - 1)
            {
                Pause();
                // Debug.Log("infiniteChallengeMode = " + infiniteChallengeMode);
                if (!infiniteChallengeMode)
                    UIManager.m_completePanel.SetActive(true);
                LevelButtonsManager.AddCompletedLevel();
                LevelButtonsManager.updated = false;

                if (infiniteChallengeMode)
                {
                    Character script = m_character.GetComponent<Character>();
                    script.SetHealth(script.GetMaxHealth());
                    highestRecord = Math.Max(highestRecord, m_timer);
                    // Debug.Log("current record = "+ m_timer + ", Highest record = "+ highestRecord);
                    // DisplayScores(m_timer);
                    UIManager.m_infiniteModePanel.SetActive(true);
                    DisplayScores(m_timer);
                }
                infiniteChallengeMode = true;
            }
            else
            {
                if (LevelManager.GetShopConfig().Count > 0)
                {
                    OpenShop();
                }
                else
                {
                    Pause();
                    UIManager.m_activeSkillPanel.SetActive(false);
                    UIManager.m_levelSelectionPanel.SetActive(true);
                }
                LevelButtonsManager.AddCompletedLevel();
                LevelButtonsManager.updated = false;
                LoadLevel();

            }



        }
    }

    public static GameObject getCharacter() => m_character;
    public static SendToGoogle GetGoogleSender() => m_googleSender;

    public static void LoadLevel()
    {
        Clear();
        matrixSent = false;
        // Debug.Log("LoadLevel() infiniteChallengeMode = " + infiniteChallengeMode);
        if (LevelManager.GetLevelNum() < 12)
        {
            infiniteChallengeMode = false;
        }

        m_timer = infiniteChallengeMode ? 0.0f : LevelManager.GetTimeLimit();
        MapManager.Initialize(LevelManager.GetMapSize(), LevelManager.GetTile());
        foreach (KeyValuePair<Vector2, Type[]> kvp in LevelManager.GetInitEneimies())
        {
            Enemy.Instantiate(kvp.Key, Quaternion.identity, kvp.Value);
        }
        if (LevelManager.GetCharaterDisarm() && characterDisarmed == null)
            characterDisarmed = getCharacter().AddComponent<Disarmed>();
        if (!LevelManager.GetCharaterDisarm() && characterDisarmed != null)
            Destroy(characterDisarmed);
    }

    private static void GameOver()
    {
        CrisisManager.Deactivate();
        Clear();

        Character script = m_character.GetComponent<Character>();
        script.SetHealth(script.GetMaxHealth());
        if (!infiniteChallengeMode)
        {

            LevelManager.Reset();
            LevelButtonsManager.ResetCompletedLevels();
            foreach (PassiveItem item in script.GetPassiveItems().ToList())
                script.RemoveItem(item);
            foreach (ActiveItem item in script.GetActiveItemKeyCodePairs().Keys.ToList())
                script.RemoveItem(item);
        }
        Pause();
        if (!infiniteChallengeMode)
            UIManager.m_losePanel.SetActive(true);
        else
        {
            UIManager.m_infiniteModePanel.SetActive(true);
            highestRecord = Math.Max(highestRecord, m_timer);
            // Debug.Log("current record = "+ m_timer + ", Highest record = "+ highestRecord);
            DisplayScores(m_timer);
        }


    }

    private static void Clear()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Disposable"))
            Destroy(o);
        foreach (Component comp in FindObjectsOfType<MonoBehaviour>(true).OfType<IDisposable>().ToArray())
            Destroy(comp);
        m_character.transform.position = Vector3.zero;
        foreach (TrailRenderer trailRenderer in m_character.GetComponentsInChildren<TrailRenderer>())
            trailRenderer.Clear();
        foreach (ActiveItem item in m_character.GetComponent<Character>().GetActiveItemKeyCodePairs().Keys.ToList())
            item.ResetCharge();
        ActiveItem_0.activateCounter = 0;
        m_crisisManager.StopAllCoroutines();
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
        UIManager.SetShopOptions(LevelManager.GetShopConfig());
    }

    public static void CloseShop()
    {
        UIManager.m_shopPanel.SetActive(false);
        // Continue();
        UIManager.m_activeSkillPanel.SetActive(false);
        UIManager.m_levelSelectionPanel.SetActive(true);


    }

    public void BackToMain()
    {
        Pause();
        Clear();
        CrisisManager.Deactivate();
        UIManager.m_completePanel.SetActive(false);
        UIManager.m_losePanel.SetActive(false);
        UIManager.m_infiniteModePanel.SetActive(false);
        UIManager.m_recordsPanel.SetActive(false);
        Debug.Log("set to false: m_completePanel, m_losePanel, m_recordsPanel");
        UIManager.m_levelSelectionPanel.SetActive(true);
    }

    public void ContinueInfiniteChallenge()
    {
        Pause();
        Clear();
        UIManager.m_completePanel.SetActive(false);
        UIManager.m_recordsPanel.SetActive(false);
        UIManager.m_infiniteModePanel.SetActive(false);
        CrisisManager.Activate();
        LoadLevel();
        Continue();
    }

    public static bool IsInInfiniteChallengeMode()
    {
        return infiniteChallengeMode;
    }

    private static void DisplayScores(float currentScore)
    {
        UIManager.m_recordsPanel.SetActive(true);
        UIManager.m_currentRecordText = GameObject.FindWithTag("CurrentScoreText").GetComponent<TextMeshProUGUI>();
        UIManager.m_highestRecordText = GameObject.FindWithTag("HighestScoreText").GetComponent<TextMeshProUGUI>();
        UIManager.UpdateScoresText(currentScore, 3);
    }

    public static float GetHighestRecord()
    {
        return highestRecord;
    }
}
