using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeControllerBase : Singleton<HomeControllerBase>
{

    protected override void OnAwake()
    {
        GameController.Instance.currentScene = SceneType.MainHome;

    }

    private void Start()
    {
    }

}
