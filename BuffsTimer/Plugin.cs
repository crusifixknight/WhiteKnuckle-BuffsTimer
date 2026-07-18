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
    private static readonly List<ItemBuff> _buffs = [];
    public static IReadOnlyList<ItemBuff> Buffs => _buffs;

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
            _buffs.Clear();
            var buffTimerGO = new GameObject("BuffTimer");
            buffTimerGO.transform.SetParent(go.transform, false);
            Text text = buffTimerGO.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 18;
            text.color = Color.white;
            text.raycastTarget = false;
            RectTransform component = buffTimerGO.GetComponent<RectTransform>();
            Vector2 vector = new Vector2(0f, 1f);
            component.pivot = vector;
            component.anchorMin = component.anchorMax = vector;
            component.sizeDelta = new Vector2(400f, 700f);
            buffTimerGO.AddComponent<BuffsMonitorComponent>().Setup(text);
        }
    }

    /// <summary>
    /// Adds buff to list
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="buffContainer"></param>
    public static void AddBuff(string prefabName, BuffContainer buffContainer)
    {
        _buffs.Add(new ItemBuff(prefabName, buffContainer));
    }

    /// <summary>
    /// Remove buff from list
    /// </summary>
    /// <param name="buff"></param>
    public static void RemoveBuff(ItemBuff buff)
    {
        _buffs.Remove(buff);
    }
}