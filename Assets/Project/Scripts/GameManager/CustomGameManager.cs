using RedSilver2.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public sealed class CustomGameManager : GameManager
{
    private UnityEvent<int>      onOrbCollected;
    private UnityEvent<Gamemode> onGamemodeSelected;
    private List<Orb>            orbs;

    private const string FIRST_GAME_OPENING = "FIRST GAME";
    private const string UNLOCKED_GAMEMODE  = "UNLOCKED GAMEMODE";
    private const string SELECTED_GAMEMODE  = "SELECTED GAMEMODE";

    protected override void Awake()
    {
        base.Awake();
        orbs = new List<Orb>();
       
        onOrbCollected     = new UnityEvent<int>();
        onGamemodeSelected = new UnityEvent<Gamemode>();

    }

    private void Start() 
    {
        if (Application.isEditor)
        {
            PlayerPrefs.DeleteKey(FIRST_GAME_OPENING);
            PlayerPrefs.DeleteKey(UNLOCKED_GAMEMODE);
            PlayerPrefs.DeleteKey(SELECTED_GAMEMODE);
        }

        SceneLoaderManager?.AddOnSingleSceneLoadStartedListener(sceneIndex => { orbs?.Clear(); });
        CheckSceneLoad();
    }

    private void CheckSceneLoad()
    {
        Debug.Log("1");

        if (WasFirstGameLaunch()) {

            Debug.Log("1.5");

            PlayerPrefs.SetInt(FIRST_GAME_OPENING, 1);
            SetUnlockedGameMode(0);

            SetGameMode(0);
            LoadGameMode();
        }
    }

    public void AddOrb(Orb orb)
    {
        if (orb == null || orbs == null) return;
        if (!orbs.Contains(orb)) orbs?.Add(orb);
    }

    public void AddOnOrbCollectedListener(UnityAction<int> action){
        if (action != null) onOrbCollected?.AddListener(action);
    }

    public void RemoveOnOrbCollectedListener(UnityAction<int> action) {
        if (action != null) onOrbCollected?.RemoveListener(action);
    }

    public void AddOnGamemodeSelectedListener(UnityAction<Gamemode> action)
    {
        if (action != null) onGamemodeSelected?.AddListener(action);
    }

    public void RemoveOnGamemodeSelectedListener(UnityAction<Gamemode> action)
    {
        if (action != null) onGamemodeSelected?.RemoveListener(action);
    }


    public void CollectOrb(Orb orb, out bool isCollected)
    {     
        isCollected = false;
        if (orb == null || orbs == null) return;

        isCollected = orbs.Contains(orb);
       
        if (isCollected) {
            orbs.Remove(orb);
            onOrbCollected.Invoke(orbs.Count);
        }

        orbs = orbs.Where(x => x != null).ToList();
    }

    public void SetGameMode(Gamemode gamemode)
    {
        if (IsGameModeUnlocked(gamemode)) {
            PlayerPrefs.SetInt(SELECTED_GAMEMODE, (int)gamemode);
            onGamemodeSelected?.Invoke(gamemode);
        }
    }

    public void LoadGameMode() {
        SceneLoaderManager?.LoadSingleScene(((int)GetSelectedGameMode() + 1));
    }

    public void UnlockNextGameMode() {
        if (PlayerPrefs.HasKey(UNLOCKED_GAMEMODE)) {
            SetUnlockedGameMode(PlayerPrefs.GetInt(UNLOCKED_GAMEMODE) + 1);
        }
    }

    public int GetUnlockedGameModeIndex()
    {
        return PlayerPrefs.GetInt(UNLOCKED_GAMEMODE, 0);
    }

    private void SetUnlockedGameMode(Gamemode gamemode)
    {
        SetUnlockedGameMode((int)gamemode);
    }

    private void SetUnlockedGameMode(int index) {
        int maxIndex = GetMaxGameModeCount() - 1;

        if (PlayerPrefs.HasKey(UNLOCKED_GAMEMODE)) {
            if (PlayerPrefs.GetInt(UNLOCKED_GAMEMODE, 0) > maxIndex)  {
                PlayerPrefs.SetInt(UNLOCKED_GAMEMODE, maxIndex);
                return;
            }
        }

        PlayerPrefs.SetInt(UNLOCKED_GAMEMODE, Mathf.Clamp(index, 0, maxIndex));
    }

    public bool WasFirstGameLaunch()
    {
        return PlayerPrefs.GetInt(FIRST_GAME_OPENING, 0) == 0;
    }

    public bool IsGameModeUnlocked(Gamemode gamemode)
    {
        return IsGameModeUnlocked((int)gamemode);
    }

    public bool IsGameModeUnlocked(int gameModeIndex) {
       return gameModeIndex <= PlayerPrefs.GetInt(UNLOCKED_GAMEMODE, 0);
    }

    public int GetSelectedGameModeIndex()
    {
        return (int)GetSelectedGameMode();
    }

    public Gamemode GetSelectedGameMode() {
        return (Gamemode)PlayerPrefs.GetInt(SELECTED_GAMEMODE, 0);
    }

    public static int GetMaxGameModeCount()
    {
        Gamemode[] gamemodes = GetGameModes();
        if (gamemodes == null) return 0;
        return gamemodes.Length;
    }

    public static string GetGameModeName(Gamemode gamemode)
    {
        return gamemode.ToString();
    }

    public static Gamemode[] GetGameModes()
    {
        return (Gamemode[])Enum.GetValues(typeof(Gamemode));
    }

    public static CustomGameManager GetInstance()
    {
        return instance as CustomGameManager;
    }
}
