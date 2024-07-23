using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringHelper
{
    public const string IS_CLICK_JIGSAW_EVENT = "IS_CLICK_JIGSAW_EVENT";
    public const string ONOFF_SOUND = "ONOFF_SOUND";
    public const string ONOFF_MUSIC = "ONOFF_MUSIC";
    public const string ONOFF_VIBRATION = "ONOFF_VIBRATION";
    public const string FIRST_TIME_INSTALL = "FIRST_TIME_INSTALL";

    public const string VERSION_FIRST_INSTALL = "VERSION_FIRST_INSTALL";
    public const string FIRST_PLAY = "FIRST_PLAY";
    public const string FIRST_PLAY_INDEX = "FIRST_PLAY_INDEX";
    public const string IN_LEVEL_1 = "IN_LEVEL_1";
    public const string IS_BACK_HOME = "IS_BACK_HOME";
    public const string STORY_TO_HOME = "STORY_TO_HOME";
    public const string JIGSAW_TO_HOME = "JIGSAW_TO_HOME";
    public const string IS_OPEN_POPUPCOMPLETE = "IS_OPEN_POPUPCOMPLETE";
    public const string REMOVE_ADS = "REMOVE_ADS";
    public const string CURRENT_LEVEL = "CURRENT_LEVEL";
    public const string CURRENT_CHAPTER = "CURRENT_CHAPTER";
    public const string CURRENT_STEP_CHAPTER = "CURRENT_STEP_CHAPTER";
    public const string CURRENT_JIGSAW_LEVEL_PLAY = "CURRENT_JIGSAW_LEVEL_PLAY";
    public const string JIGSAW_LEVEL_UNLOCKED = "JIGSAW_LEVEL_UNLOCKED";
    public const string LEVEL_UNLOCK = "CURRENT_LEVEL_PLAY_1";
    public const string NEWGAMEPLAY_CURRENT_LEVEL = "NEWGAMEPLAY_CURRENT_LEVEL";
    public const string PATH_CONFIG_LEVEL = "Levels/Level_";
    public const string PATH_CONFIG_LEVEL_TEST = "LevelsTest/Level_{0}";
    public const string LEVEL_DEMO_IMAGE_WIN = "Demo/lv";
    public const string LEVEL_DEMO_IMAGE_NOT_WIN = "Demo/bw_lv";
    public const string ANIM_LEVEL = "AnimLevel/AnimLv";

    public const string SALE_IAP = "_sale";

    public const string RETENTION_D = "retent_type";
    public const string DAYS_PLAYED = "days_played";
    public const string PAYING_TYPE = "retent_type";
    public const string LEVEL = "level";

    public const string LAST_TIME_OPEN_GAME = "LAST_TIME_OPEN_GAME";
    public const string FIRST_TIME_OPEN_GAME = "FIRST_TIME_OPEN_GAME";

    public const string CAN_SHOW_RATE = "CAN_SHOW_RATE";

    public const string IS_TUTED_RETURN = "IS_TUTED_RETURN";
    public const string CURRENT_NUM_RETURN = "CURRENT_NUM_RETURN";
    public const string CURRENT_NUM_ADD_STAND = "CURRENT_NUM_ADD_STAND";
    public const string CURRENT_NUM_REMOVE_BOMB = "CURRENT_NUM_REMOVE_BOMB";
    public const string CURRENT_NUM_REMOVE_CAGE = "CURRENT_NUM_REMOVE_CAGE";
    public const string CURRENT_NUM_REMOVE_EGG = "CURRENT_NUM_REMOVE_EGG";
    public const string CURRENT_NUM_REMOVE_SLEEP = "CURRENT_NUM_REMOVE_SLEEP";
    public const string CURRENT_NUM_REMOVE_JAIL = "CURRENT_NUM_REMOVE_JAIL";


    public const string IS_TUTED_BUY_STAND = "IS_TUTED_BUY_STAND";
    public const string ACCUMULATION_REWARD = "ACCUMULATION_REWARD";
    public const string CURRENT_BIRD_SKIN = "CURRENT_BIRD_SKIN";
    public const string CURRENT_BRANCH_SKIN = "CURRENT_BRANCH_SKIN";
    public const string CURRENT_THEME = "CURRENT_THEME";
    public const string OWNED_BIRD_SKIN = "OWNED_BIRD_SKIN";
    public const string OWNED_BRANCH_SKIN = "OWNED_BRANCH_SKIN";
    public const string OWNED_THEME = "OWNED_THEME";
    public const string RANDOM_BIRD_SKIN_IN_SHOP = "RANDOM_BIRD_SKIN_IN_SHOP";
    public const string RANDOM_BRANCH_IN_SHOP = "RANDOM_BRANCH_IN_SHOP";
    public const string RANDOM_THEME_IN_SHOP = "RANDOM_THEME_IN_SHOP";

    public const string RANDOM_BIRD_SKIN_SALE_WEEKEND_1 = "RANDOM_BIRD_SKIN_SALE_WEEKEND_1";

    public const string CURRENT_RANDOM_BIRD_SKIN = "CURRENT_RANDOM_BIRD_SKIN";
    public const string CURRENT_RANDOM_BRANCH_SKIN = "CURRENT_RANDOM_BRANCH_SKIN";
    public const string CURRENT_RANDOM_THEME = "CURRENT_RANDOM_THEME";


    public const string NUM_SHOWED_ACCUMULATION_REWARD_RANDOM = "NUM_SHOWED_ACCUMULATION_REWARD_RANDOM";

    public const string NUMBER_OF_ADS_IN_DAY = "NUMBER_OF_ADS_IN_DAY";
    public const string NUMBER_OF_ADS_IN_PLAY = "NUMBER_OF_ADS_IN_PLAY";

    public const string IS_PACK_PURCHASED_ = "IS_PACK_PURCHASED_";
    public const string IS_PACK_ACTIVATED_ = "IS_PACK_ACTIVATED_";
    public const string LAST_TIME_PACK_ACTIVE_ = "LAST_TIME_PACK_ACTIVE_";
    public const string LAST_TIME_PACK_SHOW_HOME_ = "LAST_TIME_PACK_SHOW_HOME_";
    public const string STARTER_PACK_IS_COMPLETED = "STARTER_PACK_IS_COMPLETED";

    public const string LAST_TIME_RESET_SALE_PACK_SHOP = "LAST_TIME_RESET_SALE_PACK_SHOP";

    public const string NEW_USER = "new_user";

    public const string LAST_TIME_ONLINE = "LAST_TIME_ONLINE";
    public const string CURRENT_ID_MINI_GAME = "current_id_mini_game";
    public const string ITEM_HINT = "item_hint";
    public const string ITEM_SHUFFE = "item_shuffe";
    public const string DATE_RECIVE_GIFT_DAILY = "date_recive_gift_daily";
    public const string ITEM_JIGSAW_HINT = "item_jigsaw_hint";
    public const string ITEM_JIGSAW_SHUFFE = "item_jigsaw_shuffe";
    public const string ITEM_JIGSAW_SEE = "item_jigsaw_see";
    public const string JIGSAW_LEVEL_REWARDED = "JIGSAW_LEVEL_REWARDED";

    public const string LINKED_ID = "linked_id";

    public const string ITEM_COIN = "ITEM_COIN";
    public const string CURRENT_DAY_DAILYQUEST = "CURRENT_DAY_DAILYQUEST";
    public const string CURRENT_TODAY_COMPLETED_LEVELS = "CURRENT_TODAY_COMPLETED_LEVELS";
    public const string IS_CLAIM_DAILY_QUEST_REWARD_TODAY = "IS_CLAIM_DAILY_QUEST_REWARD_TODAY";

    public const string FIREBASE_CONFIG_CONTROLLER_KEY = "config_controller";
    public const string IS_TRACKED_PREMISSION = "is_tracked_premission";
    public const string IS_ACCEPT_TRACKED_PREMISSION = "is_accept_tracked_premission";


    public const string NUMBER_ADS_LEFT_TO_UNLOCK = "number_ads_left_to_unlock";
    public const string ALL_ADS_LEVEL_UNLOCK = "all_ads_level_unlock";
    public const string AVATAR_URL = "playfab_avatar_url";
    public const string Region_Code = "playfab_region_code";
    public const string IQ_POINT = "iq_point";
    public const string NEWGAMEPLAY_POINT = "NewGamePlayPoint";
    public const string IQ_LEADERBOARD_VERSION = "iq_leaderboard_version";
    public const string CURRENT_LEVEL_SPECIAL = "current_level_special";
    public const string LEVEL_COMPLETE = "level_complete_{0}";
    public const string CURRENT_TODAY_COMPLETED_LEVELS_EDITOR = "current_today_completed_levels_editor";
    public const string CURRENT_NORMAL_CONFIG = "current_normal_config";
    public const string CURRENT_EDITOR_CONFIG = "current_editor_config";
    public const string MIN_JIGSAW_HELP_NEED = "MIN_JIGSAW_HELP_NEED";

    public const string TIME_EVENT_NOEL_END_IN = "time_event_noel_end_in";
    public const string ENABLE_EVENT_NOEL = "enable_event_noel";
    public const string ENABLE_GIFT_INPROGRESS = "enable_gift_inprogress";
    public const string JIGSAW_LEVEL_SAVE = "Jigsaw_Level_Save";
}

public class PathPrefabs
{
    public const string POPUP_REWARD_BASE = "UI/Popups/PopupRewardBase";
    public const string CONFIRM_POPUP = "UI/Popups/ConfirmBox";
    public const string WAITING_BOX = "UI/Popups/WaitingBox";
    public const string WIN_BOX = "UI/Popups/WinBox";
    public const string REWARD_IAP_BOX = "UI/Popups/RewardIAPBox";
    public const string SHOP_BOX = "UI/ShopBox";
    public const string RATE_GAME_BOX = "UI/Popups/RateGameBox";
    public const string SETTING_BOX = "UI/Popups/SettingBox";
    public const string LOSE_BOX = "UI/Popups/LoseBox";
    public const string SETTINGS_BOX = "UI/Popups/SettingsBox";
    public const string GIFT_BOX = "UI/Popups/GiftBox";
    public const string FAIL_CONNECTION_BOX = "UI/Popups/FailConnectionBox";
    public const string SELECT_LEVEL_BOX = "UI/Popups/SelectLevelPopups";

    public const string REWARD_CONGRATULATION_BOX = "UI/Popups/RewardCongratulationBox";
    public const string SHOP_GAME_BOX = "UI/Popups/ShopBox";
    public const string CONGRATULATION_BOX = "UI/Popups/CongratulationBox";

    public const string STARTER_PACK_BOX = "UI/Popups/PackBoxes/StarterPackBox";
    public const string THREE_SKIN_BIRD_PACK_BOX = "UI/Popups/PackBoxes/ThreeSkinBirdPackBox";
    public const string BRANCH_AND_THEME_PACK_BOX = "UI/Popups/PackBoxes/BranchAndThemePackBox";


    public const string BIG_REMOVE_ADS_PACK_BOX = "UI/Popups/PackBoxes/BigRemoveAdsPackBox";
    public const string SALE_WEEKEND_1_PACK_BOX = "UI/Popups/PackBoxes/SaleWeekend1PackBox";
    public const string MINI_GAME_CONNECT_BIRD_BOX = "UI/Popups/ConnectBirdMGBox";
    public const string CONNECT_BIRD_MINI_GAME_SHOP_BOX = "UI/Popups/ConnectBirdMiniGameShop";
    public const string REWARD_CONNECT_BIRD_MN_BOX = "UI/Popups/RewardConnectBirdMNBox";
    public const string POPUP_DAILY_REWARD = "UI/Popups/PopupDailyReward";
    public const string JIGSAW_EVENT_UI = "UI/JigsawEventUI";
    public const string INVITE_JIGSAW_EVENT_UI = "UI/Popups/PopupInviteJigsaw";
    public const string BLOCK_EVENT_UI = "UI/Popups/PopupBlockEvent";
    public const string PIG_BANK_UI = "UI/PigBankUI";
    public const string FREE_COIN_UI = "UI/FreeCoinUI";
    public const string COIN_EFFECT_MOVE = "UI/CoinEffectMove";
    public const string COIN_PREFAB = "UI/CoinPrefab";
    public const string RewardDailyQuestBox = "UI/RewardDailyQuestBox";
    public const string TRACKING_BOX = "UI/TrackingBox";
    public const string NO_INTERNET_BOX = "UI/NoInternetBox";
    public const string AD_BREAK_BOX = "UI/AdBreakBox";
    public const string CHECK_INTERNET_POP_UP = "UI/Popups/CheckInternetPopUp";
    public const string POPUP_RENAME = "UI/Popups/PopupRename";
    public const string POPUP_VIP = "UI/PopupVip";
    public const string POPUP_SUGGEST_STORY = "UI/PopupSuggestStory";
    public const string NEW_GAMEPLAY_LEVEL = "NewGamePlayLevel/NewGamePlayLevel_";
    public const string MINI_STORY_LEVEL = "MiniStoryLevel/";
    public const string POPUP_COMPLETE_STORY = "UI/PopupCompleteStory";
    public const string JIGSAW_LEVEL = "JigsawLevel/Level";
    public const string JIGSAW_LEVEL_TEXTURE = "JigsawLevelTexture/Level";
    public const string ADS_BREAK_POP_UP = "Assets/Prefabs/AdsBreakPopUp";
    
}

public class SceneName
{
    public const string LOADING_SCENE = "Loading Scene";
    public const string HOME_SCENE = "HomeScene";
    public const string GAME_PLAY = "Gameplay";
    public const string STORY_GAMEPLAY = "StoryGamePlay";
}

public class AudioName
{
    //public const string bgMainHome = "Music_BG_MainHome";
}

public class KeyPref
{
    //public const string SERVER_INDEX = "SERVER_INDEX";

}
public class PackIAP
{
    //public const string REMOVE_ADS = "remove_ads";
}
public static class CategoryConst
{
    //public const int POPULAR = 10002;
}
public class FirebaseConfig
{

   public const string DELAY_SHOW_INTER_INGAME = "delay_show_inter_ingame";
    public const string DELAY_SHOW_INITSTIALL = "delay_show_initi_ads";//Thời gian giữa 2 lần show inital 30
    public const string LEVEL_START_SHOW_INITSTIALL = "level_start_show_initstiall";//Level bắt đầu show initial//3
    public const string COUNT_PICES_DONE_SHOW_INTER = "count_pices_done_show_inter";
    public const string DELAY_SHOW_INTER_BUTTON_CLICK = "delay_show_inter_button_click";
    public const string LEVEL_START_SHOW_RATE = "level_start_show_rate";//Level bắt đầu show popuprate

    public const string DEFAULT_NUM_ADD_BRANCH = "default_num_add_branch";//2
    public const string DEFAULT_NUM_REMOVE_BOMB = "default_num_remove_bomb";//0
    public const string DEFAULT_NUM_REMOVE_EGG = "default_num_remove_egg";//0
    public const string DEFAULT_NUM_REMOVE_JAIL = "default_num_remove_jail";//0
    public const string DEFAULT_NUM_REMOVE_SLEEP = "default_num_remove_sleep";//0
    public const string DEFAULT_NUM_REMOVE_CAGE = "default_num_remove_cage";//0

    public const string DEFAULT_NUM_RETURN = "default_num_return";//2
    public const string NUM_RETURN_CLAIM_VIDEO_REWARD = "num_return_claim_video_reward";//3

    public const string LEVEL_START_TUT_RETURN = "level_start_tut_return";//4
    public const string LEVEL_START_TUT_BUY_STAND = "level_start_tut_buy_stand";//5

    public const string ON_OFF_REMOVE_ADS = "on_off_remove_ads_2";//5
    public const string MAX_LEVEL_SHOW_RATE = "max_level_show_rate";//30

    public const string TEST_LEVEL_CAGE_BOOM = "test_level_cage_boom";//30
    public const string SHOW_INTER_PER_TIME = "show_inter_per_time";
    public const string ON_OFF_ACCUMULATION_REWARD_LEVEL_START = "on_off_accumulation_reward_level_start";//true
    public const string ACCUMULATION_REWARD_LEVEL_START = "accumulation_reward_level_start";//6
    public const string ACCUMULATION_REWARD_END_LEVEL = "accumulation_reward_end_level_{0}";//
    public const string ACCUMULATION_REWARD_TIME_SHOW_NEXT_BUTTON = "accumulation_reward_time_show_next_button";//1.5
    public const string ACCUMULATION_REWARD_END_LEVEL_RANDOM = "accumulation_reward_end_level_random";//10
    public const string MAX_TURN_ACCUMULATION_REWARD_END_LEVEL_RANDOM = "max_turn_accumulation_reward_end_level_random";//150

    public const string ON_OFF_SALE_INAPP = "on_off_sale_inapp";//true

    public const string LEVEL_UNLOCK_SALE_PACK = "level_unlock_sale_pack"; //11
    public const string LEVEL_UNLOCK_PREMIUM_PACK = "level_unlock_premium_pack"; //25
    public const string TIME_LIFE_STARTER_PACK = "time_life_starter_pack"; // 3DAY
    public const string TIME_LIFE_PREMIUM_PACK = "time_life_premium_pack"; // 2DAY
    public const string TIME_LIFE_SALE_PACK = "time_life_premium_pack"; // 1DAY
    public const string TIME_LIFE_BIG_REMOVE_ADS_PACK = "time_life_big_remove_ads_pack"; // 3h

    public const string NUMBER_OF_ADS_IN_DAY_TO_SHOW_PACK = "number_of_ads_in_day_to_show_pack"; //5ADS
    public const string NUMBER_OF_ADS_IN_PLAY_TO_SHOW_PACK = "number_of_ads_in_play_to_show_pack"; //3ADS
    public const string TIME_DELAY_SHOW_POPUP_SALE_PACK_ = "time_delay_show_popup_sale_pack_"; // 6H
    public const string TIME_DELAY_ACTIVE_SALE_PACK = "time_delay_active_sale_pack_"; // 6H
#if TESTER
    public const string CONFIG_CONTROLLER = "config_controller_tester";
#else
    public const string CONFIG_CONTROLLER = "config_controller";
#endif
    public const string REVIEW_IAP_VERSION = "review_iap_version"; // 6H
    public const string APP_OPEN_ADS_ENABLE = "app_open_ads_enable";
    public const string LEVEL_NORMAL_CONFIG = "level_normal_config";


    public const string FIRST_OPEN_STORY = "first_open_story";

    public const string LEVEL_EDITOR_CONFIG = "level_editor_config";
    public const string LEVEL_AB_TEST = "level_ab_test";

    public const string TIME_EVENT_NOEL_END_IN = "time_event_noel_end_in";
    public const string ENABLE_EVENT_NOEL = "enable_event_noel";
    public const string ENABLE_GIFT_INPROGRESS = "enable_gift_inprogress";
    public const string ENABLE_ADMOB_BANNER = "enable_admob_banner";
    public const string IS_SHOW_UMP = "is_show_ump";
    public const string MINIMUM_TIME_SHOW_COLLAPSE = "minimum_time_show_collapse";
    public const string COOLDOWN_ADMOB_REFRESH = "cooldown_admob_refresh";
    public const string COOLDOWN_INTER_IN_JIGSAW_GAME = "cooldown_inter_in_jigsaw_game";
    public const string CHECK_INTERNET = "check_internet";
}

public class LevelGameData
{
    public const string LEVEL_SAVE = "Datas/DataLevelSave";
}

public class ChapterGameData
{
    public const string CHAPTER_SAVE = "Datas/ChapterDataSave";
}

