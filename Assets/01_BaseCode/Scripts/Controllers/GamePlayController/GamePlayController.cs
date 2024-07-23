// using Crystal;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
//
// public enum StateGame
// {
//     Loading = 0,
//     Playing = 1,
//     Win = 2,
//     Lose = 3,
//     Pause = 4
// }
//
// public class GamePlayController : Singleton<GamePlayController>
// {
//     public PlayerContain playerContain;
//     public GameScene gameScene;
//
//     public StateGame state;
//
//     private static bool isBannerShow;
//     [Header("Safe Area")]
//     public SafeArea safeArea;
//
//
//     protected override void OnAwake()
//     {
//         GameController.Instance.currentScene = SceneType.GamePlay;
//
//         Init();
//
//     }
//
//     public void Init()
//     {
// #if UNITY_IOS
// if (safeArea != null)
//             safeArea.enabled = true;
// #endif
//         //! WARNING: KHÔNG THAY ĐỔI THỨ TỰ INIT
//         playerContain.Init();
//         InitLevel();
//         gameScene.Init();
//
//         ResetDay();
//         // MusicManager.Instance.PlayBGMusic();
//         //if (AdmobAds.isLoadedBanner)
//         {
//             //if (!isBannerShow)
//             {
//                 // GameController.Instance.admobAds.DestroyBanner();
//                 // GameController.Instance.admobAds.ShowBanner();
//
//
//                 isBannerShow = true;
//             }
//         }
//
//     }
//
//     public void InitLevel()
//     {
//         //  level = Instantiate(Resources.Load<Level>("Levels/Level_" + indexLevel), null);
//         //Load ra level theo Json
//
//
//     }
//
//
//     public void PlayAnimFly()
//     {
//
//     }
//
//     public void PlayAnimFlyOut()
//     {
//
//     }
//
//
//     public void ResetDay()
//     {
//         /////Các loại ResetDay ở đây
//         //long time = TimeManager.CaculateTime(TimeManager.ParseTimeStartDay(UseProfile.LastTimeOnline), TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()));
//         //if (time >= 86400)
//         //{
//         //    UseProfile.LastTimeOnline = UnbiasedTime.Instance.Now();
//
//         //}
//
//     }
// }
