using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BuffsTimer;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal new static ManualLogSource Logger { get; private set; } = null!;
    private Harmony _harmony;
    public static List<ItemBuff> Buffs = [];

    private void Awake()
    {
        Logger = base.Logger;
        
        _harmony = new Harmony($"{MyPluginInfo.PLUGIN_GUID}.harmony");
        _harmony.PatchAll();
        
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");
        gameObject.hideFlags = HideFlags.DontSaveInEditor;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        _harmony.UnpatchSelf();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject go = GameObject.Find("GameManager/Canvas/Game UI/GameStateTrackers");
        if (go != null)
        {
            Buffs.Clear();
            var buffTimerGO = new GameObject("BuffTimer");
            buffTimerGO.transform.SetParent(go.transform, false);
            Text text = buffTimerGO.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 18;
            text.color = Color.white;
            RectTransform component = buffTimerGO.GetComponent<RectTransform>();
            Vector2 vector = new Vector2(0f, 1f);
            component.pivot = vector;
            component.sizeDelta = new Vector2(500f, 500f);
            buffTimerGO.AddComponent<BuffsMonitorComponent>().Setup(text);
        }
    }
}