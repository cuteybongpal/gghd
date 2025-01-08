using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LoadingScene : UI_Base
{
    private void Awake()
    {
        EventManager.AssetAllLoading = LoadingComplete;
    }
    
    void LoadingComplete()
    {
        SceneManager.LoadScene(1);
    }

}
