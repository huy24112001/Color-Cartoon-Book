using UnityEngine;
using System;
using System.Linq;
using MoreMountains.NiceVibrations;
using Newtonsoft.Json;

public class UseProfile : MonoBehaviour
{
    public static int FirstWinStoryLevel = -1;
    public static bool IsCompleteStoryPopupOpen = false;

    public static bool isReplay;
    public static bool isReciveJigsaw;
    public static int miniStoryPlaying = -1;

    public static int CurrentLevel
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL, 1); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL, value);
            PlayerPrefs.Save();
        }
    }
    
    public static int CurrentChapter
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_CHAPTER, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_CHAPTER, value);
            PlayerPrefs.Save();
        }
    }

    public static int CurrentStepChapter
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_STEP_CHAPTER, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_STEP_CHAPTER, value);
            PlayerPrefs.Save();
        }
    }

    public static string CurrentJigsawLevelPlay
    {
        get { return PlayerPrefs.GetString(StringHelper.CURRENT_JIGSAW_LEVEL_PLAY, string.Empty); }
        set
        {
            PlayerPrefs.SetString(StringHelper.CURRENT_JIGSAW_LEVEL_PLAY, value);
            PlayerPrefs.Save();
        }
    }
    
    public static bool GetLevelJigSawUnlocked(string levelID)
    {
        return PlayerPrefs.GetInt(StringHelper.JIGSAW_LEVEL_UNLOCKED + levelID, 0) == 1; 
    }

    public static void SetLevelJigSawUnlocked(string levelID, bool value)
    {
        PlayerPrefs.SetInt(StringHelper.JIGSAW_LEVEL_UNLOCKED + levelID, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static string GetJigsawLevelData(string levelName)
    {
        return PlayerPrefs.GetString(StringHelper.JIGSAW_LEVEL_SAVE + levelName,String.Empty);
    }

    public static void SetJigsawLevelData(string levelName, string value)
    {
        PlayerPrefs.SetString(StringHelper.JIGSAW_LEVEL_SAVE + levelName,value);
    }
    
    public static bool NewUser
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.NEW_USER) == 0;
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NEW_USER, value ? 0 : 1);
        }
    }
    public static void SetReplayLevel(int level, int value)
    {
        PlayerPrefs.SetInt("replay_level_" + level, value);
    }
    public static bool GetReplayLevel(int level)
    {
        return PlayerPrefs.GetInt("replay_level_" + level, 0) == 1;
    }
    public static void SetReplayChapter(int chapter, int value)
    {
        PlayerPrefs.SetInt("replay_chapter_" + chapter, value);
    }

    public static bool GetReplayChapter(int chapter)
    {
        return PlayerPrefs.GetInt("replay_chapter_" + chapter, 0) == 1;
    }

    public static int CurrentLevelSpecial
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL_SPECIAL, 1); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL_SPECIAL, value);
            PlayerPrefs.Save();
        }
    }
    

    public static bool IsTrackedPremission
    {
        get { return PlayerPrefs.GetInt(StringHelper.IS_TRACKED_PREMISSION, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_TRACKED_PREMISSION, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsAcceptTracker
    {
        get { return PlayerPrefs.GetInt(StringHelper.IS_ACCEPT_TRACKED_PREMISSION, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_ACCEPT_TRACKED_PREMISSION, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsRemoveAds
    {
        get { return PlayerPrefs.GetInt(StringHelper.REMOVE_ADS, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.REMOVE_ADS, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool OnVibration
    {
        get { return PlayerPrefs.GetInt(StringHelper.ONOFF_VIBRATION, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_VIBRATION, value ? 1 : 0);
            MMVibrationManager.SetHapticsActive(value);
            PlayerPrefs.Save();
        }
    }

    public static bool OnSound
    {
        get { return PlayerPrefs.GetInt(StringHelper.ONOFF_SOUND, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_SOUND, value ? 1 : 0);
            GameController.Instance.musicManager.SetSoundVolume(value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool OnMusic
    {
        get { return PlayerPrefs.GetInt(StringHelper.ONOFF_MUSIC, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ONOFF_MUSIC, value ? 1 : 0);
            GameController.Instance.musicManager.SetMusicVolume(value ? 0.7f : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsFirstTimeInstall
    {
        get { return PlayerPrefs.GetInt(StringHelper.FIRST_TIME_INSTALL, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.FIRST_TIME_INSTALL, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static int RetentionD
    {
        get { return PlayerPrefs.GetInt(StringHelper.RETENTION_D, 0); }
        set
        {
            if (value < 0)
                value = 0;

            PlayerPrefs.SetInt(StringHelper.RETENTION_D, value);
            PlayerPrefs.Save();
        }
    }

    public static int DaysPlayed
    {
        get { return PlayerPrefs.GetInt(StringHelper.DAYS_PLAYED, 1); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.DAYS_PLAYED, value);
            PlayerPrefs.Save();
        }
    }

    public static int PayingType
    {
        get { return PlayerPrefs.GetInt(StringHelper.PAYING_TYPE, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.PAYING_TYPE, value);
            PlayerPrefs.Save();
        }
    }



   

   


    public static bool CanShowRate
    {
        get { return PlayerPrefs.GetInt(StringHelper.CAN_SHOW_RATE, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CAN_SHOW_RATE, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool IsTutedReturn
    {
        get { return PlayerPrefs.GetInt(StringHelper.IS_TUTED_RETURN, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_TUTED_RETURN, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public int CurrentNumReturn
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_RETURN,
                RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_RETURN, 2));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_RETURN, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentNumAddBranch
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_ADD_STAND,
                RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_ADD_BRANCH, 1));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_ADD_STAND, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentNumRemoveBomb
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_BOMB,
                RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_BOMB, 0));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_BOMB, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentNumRemoveCage
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_CAGE,
                RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_CAGE, 0));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_CAGE, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentNumRemoveEgg
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_EGG,
                RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_EGG, 0));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_EGG, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentNumRemoveSleep
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_SLEEP,
                RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_SLEEP, 0));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_SLEEP, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentNumRemoveJail
    {
        get
        {
            return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_REMOVE_JAIL,
                RemoteConfigController.GetIntConfig(FirebaseConfig.DEFAULT_NUM_REMOVE_JAIL, 0));
        }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_REMOVE_JAIL, value);
            PlayerPrefs.Save();
        }
    }

    public bool IsTutedBuyStand
    {
        get { return PlayerPrefs.GetInt(StringHelper.IS_TUTED_BUY_STAND, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_TUTED_BUY_STAND, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public string CurrentBirdSkin
    {
        get { return PlayerPrefs.GetString(StringHelper.CURRENT_BIRD_SKIN, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.CURRENT_BIRD_SKIN, value);
            PlayerPrefs.Save();
        }
    }

    public string OwnedBirdSkin
    {
        get { return PlayerPrefs.GetString(StringHelper.OWNED_BIRD_SKIN, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.OWNED_BIRD_SKIN, value);
            PlayerPrefs.Save();
        }
    }

    public string RandomBirdSkinInShop
    {
        get { return PlayerPrefs.GetString(StringHelper.RANDOM_BIRD_SKIN_IN_SHOP, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.RANDOM_BIRD_SKIN_IN_SHOP, value);
            PlayerPrefs.Save();
        }
    }

    public string RandomBirdSkinSaleWeekend1
    {
        get { return PlayerPrefs.GetString(StringHelper.RANDOM_BIRD_SKIN_SALE_WEEKEND_1, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.RANDOM_BIRD_SKIN_SALE_WEEKEND_1, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentBranchSkin
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_BRANCH_SKIN, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_BRANCH_SKIN, value);
            PlayerPrefs.Save();
        }
    }

    public string OwnedBranchSkin
    {
        get { return PlayerPrefs.GetString(StringHelper.OWNED_BRANCH_SKIN, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.OWNED_BRANCH_SKIN, value);
            PlayerPrefs.Save();
        }
    }

    public string RandomBranchInShop
    {
        get { return PlayerPrefs.GetString(StringHelper.RANDOM_BRANCH_IN_SHOP, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.RANDOM_BRANCH_IN_SHOP, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentTheme
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_THEME, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_THEME, value);
            PlayerPrefs.Save();
        }
    }

    public string OwnedThemeSkin
    {
        get { return PlayerPrefs.GetString(StringHelper.OWNED_THEME, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.OWNED_THEME, value);
            PlayerPrefs.Save();
        }
    }

    public string RandomThemeInShop
    {
        get { return PlayerPrefs.GetString(StringHelper.RANDOM_THEME_IN_SHOP, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.RANDOM_THEME_IN_SHOP, value);
            PlayerPrefs.Save();
        }
    }

    public string CurrentRandomBird
    {
        get { return PlayerPrefs.GetString(StringHelper.CURRENT_RANDOM_BIRD_SKIN, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.CURRENT_RANDOM_BIRD_SKIN, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentRandomBranch
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_RANDOM_BRANCH_SKIN, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_RANDOM_BRANCH_SKIN, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentRandomTheme
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_RANDOM_THEME, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_RANDOM_THEME, value);
            PlayerPrefs.Save();
        }
    }

    public int NumShowedAccumulationRewardRandom //Khi có chim mới => bản mới sẽ NumShowedAccumulationRewardRandom = 0
    {
        get { return PlayerPrefs.GetInt(StringHelper.NUM_SHOWED_ACCUMULATION_REWARD_RANDOM, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NUM_SHOWED_ACCUMULATION_REWARD_RANDOM, value);
            PlayerPrefs.Save();
        }
    }

    public static bool StarterPackIsCompleted
    {
        get { return PlayerPrefs.GetInt(StringHelper.STARTER_PACK_IS_COMPLETED, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.STARTER_PACK_IS_COMPLETED, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static int NumberOfAdsInPlay;

    public static int NumberOfAdsInDay
    {
        get { return PlayerPrefs.GetInt(StringHelper.NUMBER_OF_ADS_IN_DAY, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NUMBER_OF_ADS_IN_DAY, value);
            PlayerPrefs.Save();
        }
    }

    
  
    public static int LevelUnlockStory
    {
        get
        {
            return PlayerPrefs.GetInt("level_unlock_story", 5001);
        }
        set
        {
            PlayerPrefs.SetInt("level_unlock_story", value);
        }
    }


    public static int Hint
    {
        get { return PlayerPrefs.GetInt(StringHelper.ITEM_HINT, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ITEM_HINT, value);
        }
    }

    public static int Shuffe
    {
        get { return PlayerPrefs.GetInt(StringHelper.ITEM_SHUFFE, 2); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ITEM_SHUFFE, value);
        }
    }
    
    public static int JHint
    {
        get { return PlayerPrefs.GetInt(StringHelper.ITEM_JIGSAW_HINT, 3); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ITEM_JIGSAW_HINT, value);
        }
    }

    public static int JShuffe
    {
        get { return PlayerPrefs.GetInt(StringHelper.ITEM_JIGSAW_SHUFFE, 1); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ITEM_JIGSAW_SHUFFE, value);
        }
    }
    
    public static int JSee
    {
        get { return PlayerPrefs.GetInt(StringHelper.ITEM_JIGSAW_SEE, 1); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ITEM_JIGSAW_SEE, value);
        }
    }

    // public static bool GetJigsawLevelRewarded(string level)
    // {
    //     return PlayerPrefs.GetInt(StringHelper.JIGSAW_LEVEL_REWARDED + level, 0) == 1;
    // }
    //
    // public static void SetJigsawLevelRewarded(string level, bool isReward)
    // {
    //     PlayerPrefs.SetInt(StringHelper.JIGSAW_LEVEL_REWARDED + level, isReward ? 1 : 0);
    // }
    
    public void SetDateTimeReciveDailyGift(DateTime value)
    {
        PlayerPrefs.SetString(StringHelper.DATE_RECIVE_GIFT_DAILY, value.ToBinary().ToString());
    }

    public DateTime GetDateTimeReciveDailyGift()
    {
        return GetDateTime(StringHelper.DATE_RECIVE_GIFT_DAILY, DateTime.MinValue);
    }

    public DateTime GetDateTime(string key, DateTime defaultValue)
    {
        string @string = PlayerPrefs.GetString(key);
        DateTime result = defaultValue;
        if (!string.IsNullOrEmpty(@string))
        {
            long dateData = Convert.ToInt64(@string);
            result = DateTime.FromBinary(dateData);
        }

        return result;
    }

    public static bool GetUnlockPic(int ID)
    {
        return PlayerPrefs.GetInt("unlock_pic" + ID) == 1;
    }

    public static void SetUnlockPic(int ID, int value)
    {
        PlayerPrefs.SetInt("unlock_pic" + ID, value);
    }

    public static string LinkedId
    {
        get => PlayerPrefs.GetString(StringHelper.LINKED_ID, "");
        set
        {
#if LOG_VERBOSE
            Debug.Log("Linked Id: " + value);
#endif
            PlayerPrefs.SetString(StringHelper.LINKED_ID, value);
            PlayerPrefs.Save();
        }
    }

    public static string PlayFabId
    {
        get => PlayerPrefs.GetString("PlayFabId", string.Empty);
        set
        {
            PlayerPrefs.SetString("PlayFabId", value);
            PlayerPrefs.Save();
        }
    }

    public static string DisplayName
    {
        get => PlayerPrefs.GetString("PlayFabName", "");
        set
        {
            PlayerPrefs.SetString("PlayFabName", value);
            PlayerPrefs.Save();
        }
    }

    //public static int Coin
    //{
    //    get { return PlayerPrefs.GetInt(StringHelper.ITEM_COIN, 10); }
    //    set
    //    {
    //        PlayerPrefs.SetInt(StringHelper.ITEM_COIN, Mathf.Max(value, 0));
    //        /*FindObjectOfType<CoinPanel>()?.UpdateUI();*/
    //    }
    //}

    public int CurrentDayDailyQuest
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_DAY_DAILYQUEST, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_DAY_DAILYQUEST, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentTodayCompletedLevels
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_TODAY_COMPLETED_LEVELS, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_TODAY_COMPLETED_LEVELS, value);
            PlayerPrefs.Save();
        }
    }

    public int CurrentTodayCompletedLevelsEditorChoice
    {
        get { return PlayerPrefs.GetInt(StringHelper.CURRENT_TODAY_COMPLETED_LEVELS_EDITOR, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.CURRENT_TODAY_COMPLETED_LEVELS_EDITOR, value);
            PlayerPrefs.Save();
        }
    }

    public bool IsClaimDailyQuestRewardToday
    {
        get { return PlayerPrefs.GetInt(StringHelper.IS_CLAIM_DAILY_QUEST_REWARD_TODAY, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_CLAIM_DAILY_QUEST_REWARD_TODAY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsClickJigsawEvent
    {
        get { return PlayerPrefs.GetInt(StringHelper.IS_CLICK_JIGSAW_EVENT, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_CLICK_JIGSAW_EVENT, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }


   

    public static bool IsVIP
    {
        get { return PlayerPrefs.GetInt("is_vip") == 1; }
        set { PlayerPrefs.SetInt("is_vip", value ? 1 : 0); }
    }


    public static string AvatarUrl
    {
        get => PlayerPrefs.GetString(StringHelper.AVATAR_URL, "");
        set
        {
            PlayerPrefs.SetString(StringHelper.AVATAR_URL, value);
            PlayerPrefs.Save();
        }
    }


    public static int IqPoint
    {
        get => PlayerPrefs.GetInt(StringHelper.IQ_POINT, 0);
        set
        {
            PlayerPrefs.SetInt(StringHelper.IQ_POINT, value);
            PlayerPrefs.Save();
        }
    }

    public static int IqLeaderboardVersion
    {
        get => PlayerPrefs.GetInt(StringHelper.IQ_LEADERBOARD_VERSION, -1);
        set
        {
            PlayerPrefs.SetInt(StringHelper.IQ_LEADERBOARD_VERSION, value);
            PlayerPrefs.Save();
        }
    }

    public static int CurrentNewGamePlayLevel
    {

        get { return PlayerPrefs.GetInt(StringHelper.NEWGAMEPLAY_CURRENT_LEVEL, 1); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NEWGAMEPLAY_CURRENT_LEVEL, value);
            PlayerPrefs.Save();
        }
    }

    public static int NewGamePlayPoint
    {
        get { return PlayerPrefs.GetInt(StringHelper.NEWGAMEPLAY_POINT, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.NEWGAMEPLAY_POINT, value);
            PlayerPrefs.Save();
        }
    }

    public static bool IsFirstPlay
    {
        get { return PlayerPrefs.GetInt(StringHelper.FIRST_PLAY, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.FIRST_PLAY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static int IsFirstPlayIndex
    {
        get { return PlayerPrefs.GetInt(StringHelper.FIRST_PLAY_INDEX, 0); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.FIRST_PLAY_INDEX, value);
            PlayerPrefs.Save();
        }
    }

    public static bool IsInFirtLevel
    {
        get { return PlayerPrefs.GetInt(StringHelper.IN_LEVEL_1, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IN_LEVEL_1, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsBackHome
    {
        get { return PlayerPrefs.GetInt(StringHelper.IS_BACK_HOME, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_BACK_HOME, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static bool BackFromStoryOpenPopUpComplete
    {
        get { return PlayerPrefs.GetInt(StringHelper.IS_OPEN_POPUPCOMPLETE, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.IS_OPEN_POPUPCOMPLETE, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static bool BackFromStoryToHome
    {
        get { return PlayerPrefs.GetInt(StringHelper.STORY_TO_HOME, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.STORY_TO_HOME, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    
    public static bool BackFromJigsawToHome
    {
        get { return PlayerPrefs.GetInt(StringHelper.JIGSAW_TO_HOME, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.JIGSAW_TO_HOME, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static int MinJigsawHelpNeed
    {
        get { return PlayerPrefs.GetInt(StringHelper.MIN_JIGSAW_HELP_NEED, 3); }
        set
        {
            PlayerPrefs.SetInt(StringHelper.MIN_JIGSAW_HELP_NEED, value);
            PlayerPrefs.Save();
        }
    }

    public static string CurrentTimeNoelEventEndIn
    {
        get => PlayerPrefs.GetString(StringHelper.TIME_EVENT_NOEL_END_IN, "10/01/2024");
        set
        {
            PlayerPrefs.SetString(StringHelper.TIME_EVENT_NOEL_END_IN, value);
            PlayerPrefs.Save();
        }
    }

    public static bool IsEnableEventNoel
    {
        get { return PlayerPrefs.GetInt(StringHelper.ENABLE_EVENT_NOEL, 1) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ENABLE_EVENT_NOEL, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static bool IsEnableGiftInProgress
    {
        get { return PlayerPrefs.GetInt(StringHelper.ENABLE_GIFT_INPROGRESS, 0) == 1; }
        set
        {
            PlayerPrefs.SetInt(StringHelper.ENABLE_EVENT_NOEL, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}