using UnityEngine;
using System.Collections;
using System;

public enum PlayMode
{
    EditorMode,
    PlayMode
}

[ExecuteInEditMode]
public class DestroyOnModeChange : MonoBehaviour
{
    public PlayMode destroyOnMode = PlayMode.EditorMode;
    public bool debug = false;

    void Awake()
    {
        switch (destroyOnMode)
        {
            case PlayMode.EditorMode:
                if (!Application.isPlaying)
                    if (PlayerPrefs.GetInt(PlayerPrefString()) == 1)
                    {
                        PlayerPrefs.SetInt(PlayerPrefString(), 0);
                        if (debug)
                            Debug.Log("DestroyOnModeChange: destroying object '" + gameObject.ToString() + "'");
                        DestroyImmediate(gameObject);
                    }
                break;
            case PlayMode.PlayMode:
                if (Application.isPlaying)
                    if (PlayerPrefs.GetInt(PlayerPrefString()) == 1)
                    {
                        PlayerPrefs.SetInt(PlayerPrefString(), 0);
                        if (debug)
                            Debug.Log("DestroyOnModeChange: destroying object '" + gameObject.ToString() + "'");
                        Destroy(gameObject);
                    }
                break;
        }
    }

    public string PlayerPrefString()
    {
        switch (destroyOnMode)
        {
            case PlayMode.PlayMode:
                return "playermodecheck_" + name + "_playmode";
            default:
            case PlayMode.EditorMode:
                return "playermodecheck_" + name + "_editormode";
        }
    }

    void OnDestroy()
    {
        switch (destroyOnMode)
        {
            case PlayMode.EditorMode:
                if (Application.isPlaying)
                    PlayerPrefs.SetInt(PlayerPrefString(), 1);
                break;
            case PlayMode.PlayMode:
                if (!Application.isPlaying)
                    PlayerPrefs.SetInt(PlayerPrefString(), 1);
                break;
        }
    }
}
