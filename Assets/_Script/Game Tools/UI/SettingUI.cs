using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adv
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] Canvas SettingUICanvas;
        [SerializeField] PlayerInput input;
        [SerializeField] LabelOptionsUI battleUI;
        [SerializeField] LabelOptionsUI ShopUI;
        [SerializeField] GameObject EvenSystem_NotMouse;
        [SerializeField] GameObject EvenSystem_Mouse;
        [SerializeField] BoolEventChannel CloseAllLabelOption;
        [SerializeField] Button 清除所有成就;
        [SerializeField] Button 退出游戏;
        [SerializeField] Button About;
        [SerializeField] Text Tips;
        [SerializeField] Image Icon;

        public bool IsOpen = false;

        private void Awake()
        {
            SettingUICanvas.enabled = IsOpen;
            Tips.enabled = true;
            Icon.enabled = true;
            SaveGame.SavePath = SaveGamePath.DataPath;
#if UNITY_EDITOR//编辑器中显示按钮
            清除所有成就.gameObject.SetActive(true);
#else //客户端内隐藏按钮
	        清除所有成就.gameObject.SetActive(false);  
#endif
            AwakeCall_LanguageChange();
            AwakeCall_ScreenChange();
            AwakeCall_InterfaceIcon();
            AwakeCall_Music();
            AwakeCall_SFX();
            AwakeCall_Achievement();
            退出游戏.onClick.AddListener(() =>
            {
#if UNITY_EDITOR //编辑器中退出游戏
                UnityEditor.EditorApplication.isPlaying = false;
#else //应用程序中退出游戏
	            UnityEngine.Application.Quit();
#endif
            });
        }

        private void Start()
        {
            input.onCloseUI += SwitchSettingUI;
            StartCall_InterfaceIcon();
            清除所有成就.enabled = false;
            退出游戏.enabled = false;
            About.enabled = false;
        }

        private void SwitchSettingUI()
        {
            if (!IsOpen && !battleUI.IsOpen && !ShopUI.IsOpen)
            {
                IsOpen = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (IsOpen)
            {
                IsOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            SettingUICanvas.enabled = IsOpen;
            if (IsOpen)
            {
                EvenSystem_NotMouse.SetActive(!IsOpen);
                EvenSystem_Mouse.SetActive(IsOpen);
            }
            else if (!IsOpen)
            {
                EvenSystem_Mouse.SetActive(IsOpen);
                EvenSystem_NotMouse.SetActive(!IsOpen);
            }
            Englishbtn.enabled = IsOpen;
            Chinesebtn.enabled = IsOpen;
            FullScreen.enabled = IsOpen;
            Windowed.enabled = IsOpen;
            Resolution1366X768.enabled = IsOpen;
            Resolution1920X1080.enabled = IsOpen;
            Display.enabled = IsOpen;
            Hide.enabled = IsOpen;
            MusicOff.enabled = IsOpen;
            MusicOn.enabled = IsOpen;
            SFXOff.enabled = IsOpen;
            SFXOn.enabled = IsOpen;
            for (var i = 0; i < achievementList.Count; i++)
            {
                achievementList[i].enabled = IsOpen;
            }
            if (清除所有成就.gameObject.activeSelf)
                清除所有成就.enabled = IsOpen;
            退出游戏.enabled = IsOpen;
            About.enabled = IsOpen;
        }

        private void OnDestroy()
        {
            input.onCloseUI -= SwitchSettingUI;
        }

        #region LanguageChange
        [Space]
        [Header("语言切换")]
        [SerializeField] Button Englishbtn;
        [SerializeField] Button Chinesebtn;
        [SerializeField] LanguageEventChannel LanguageChange;

        private void AwakeCall_LanguageChange()
        {
            //加载按键时间
            Englishbtn.onClick.AddListener(() =>
            {
                ChineseEnglishShift.language = Language.English;
                SaveGame.Save<Language>("language", ChineseEnglishShift.language);
                LanguageChange.Broadcast(ChineseEnglishShift.language);
                EventSystem.current.SetSelectedGameObject(null);
            });
            Chinesebtn.onClick.AddListener(() =>
            {
                ChineseEnglishShift.language = Language.Chinese;
                SaveGame.Save<Language>("language", ChineseEnglishShift.language);
                LanguageChange.Broadcast(ChineseEnglishShift.language);
                EventSystem.current.SetSelectedGameObject(null);
            });

            //关闭按键
            Englishbtn.enabled = false;
            Chinesebtn.enabled = false;
        }

        #endregion

        #region 屏幕修改
        [Space]
        [Header("屏幕修改")]
        [SerializeField] Button FullScreen;
        [SerializeField] Button Windowed;
        [SerializeField] Button Resolution1920X1080;
        [SerializeField] Button Resolution1366X768;

        public static FullScreenMode currentScreenSet = FullScreenMode.Windowed;
        public static int currentScreenWidth = 1366;
        public static int currentScreenHeight = 768;

        private void AwakeCall_ScreenChange()
        {


            FullScreen.onClick.AddListener(() =>
            {
                Screen.SetResolution(currentScreenWidth, currentScreenHeight, FullScreenMode.FullScreenWindow);
                currentScreenSet = FullScreenMode.FullScreenWindow;
                SaveGame.Save<FullScreenMode>("currentScreenSet", currentScreenSet);
                EventSystem.current.SetSelectedGameObject(null);
            });
            Windowed.onClick.AddListener(() =>
            {
                Screen.SetResolution(currentScreenWidth, currentScreenHeight, FullScreenMode.Windowed);
                currentScreenSet = FullScreenMode.Windowed;
                SaveGame.Save<FullScreenMode>("currentScreenSet", currentScreenSet);
                EventSystem.current.SetSelectedGameObject(null);
            });
            Resolution1920X1080.onClick.AddListener(() =>
            {
                Screen.SetResolution(1920, 1080, currentScreenSet);
                currentScreenWidth = 1920;
                currentScreenHeight = 1080;
                SaveGame.Save<int>("currentScreenWidth", currentScreenWidth);
                SaveGame.Save<int>("currentScreenHeight", currentScreenHeight);
                EventSystem.current.SetSelectedGameObject(null);
            });
            Resolution1366X768.onClick.AddListener(() =>
            {
                Screen.SetResolution(1366, 768, currentScreenSet);
                currentScreenWidth = 1366;
                currentScreenHeight = 768;
                SaveGame.Save<int>("currentScreenWidth", currentScreenWidth);
                SaveGame.Save<int>("currentScreenHeight", currentScreenHeight);
                EventSystem.current.SetSelectedGameObject(null);
            });
            FullScreen.enabled = false;
            Windowed.enabled = false;
            Resolution1366X768.enabled = false;
            Resolution1920X1080.enabled = false;
        }

        #endregion

        #region InterfaceIcon
        [Space]
        [Header("图标显示")]
        [SerializeField] List<Text> controlText;
        [SerializeField] List<Image> controlImage;
        [SerializeField] Button Display;
        [SerializeField] Button Hide;

        private bool IsIconDisplay = true;

        private void AwakeCall_InterfaceIcon()
        {
            Display.onClick.AddListener(() =>
            {
                DisplaySet(true);
                EventSystem.current.SetSelectedGameObject(null);
            });
            Hide.onClick.AddListener(() =>
            {
                DisplaySet(false);
                EventSystem.current.SetSelectedGameObject(null);
            });
            Display.enabled = false;
            Hide.enabled = false;
        }

        private void StartCall_InterfaceIcon()
        {
            if (SaveGame.Exists("IsIconDisplay"))
            {
                IsIconDisplay = SaveGame.Load<bool>("IsIconDisplay");
                for (var i = 0; i < controlText.Count; i++)
                {
                    var color = controlText[i].color;
                    color.a = IsIconDisplay ? 1 : 0;
                    controlText[i].color = color;
                }
                for (var j = 0; j < controlImage.Count; j++)
                {
                    var color = controlImage[j].color;
                    color.a = IsIconDisplay ? 1 : 0;
                    controlImage[j].color = color;
                }
            }
        }

        private void DisplaySet(bool IsDisplay)
        {
            IsIconDisplay = IsDisplay;
            for (var i = 0; i < controlText.Count; i++)
            {
                var color = controlText[i].color;
                color.a = IsDisplay ? 1 : 0;
                controlText[i].color = color;
            }
            for (var j = 0; j < controlImage.Count; j++)
            {
                var color = controlImage[j].color;
                color.a = IsDisplay ? 1 : 0;
                controlImage[j].color = color;
            }
            SaveGame.Save<bool>("IsIconDisplay", IsIconDisplay);
        }
        #endregion

        #region 音乐
        [Space]
        [Header("音乐")]
        [SerializeField] Button MusicOn;
        [SerializeField] Button MusicOff;

        private void AwakeCall_Music()
        {
            MusicOn.onClick.AddListener(() =>
            {
                AudioManager.Instance.canBGM = true;
                SaveGame.Save<bool>("canBGM", AudioManager.Instance.canBGM);
                AudioManager.Instance.BGMSwitch(true);
                EventSystem.current.SetSelectedGameObject(null);
            });
            MusicOff.onClick.AddListener(() =>
            {
                AudioManager.Instance.canBGM = false;
                SaveGame.Save<bool>("canBGM", AudioManager.Instance.canBGM);
                AudioManager.Instance.BGMSwitch(false);
                EventSystem.current.SetSelectedGameObject(null);
            });

            MusicOn.enabled = false;
            MusicOff.enabled = false;
        }
        #endregion 

        #region 音效
        [Space]
        [Header("音效")]
        [SerializeField] Button SFXOn;
        [SerializeField] Button SFXOff;

        private void AwakeCall_SFX()
        {
            SFXOn.onClick.AddListener(() =>
            {
                AudioManager.Instance.canSFX = true;
                SaveGame.Save<bool>("canSFX", AudioManager.Instance.canSFX);
                AudioManager.Instance.SFXSwitch(true);
                EventSystem.current.SetSelectedGameObject(null);
            });
            SFXOff.onClick.AddListener(() =>
            {
                AudioManager.Instance.canSFX = false;
                SaveGame.Save<bool>("canSFX", AudioManager.Instance.canSFX);
                AudioManager.Instance.SFXSwitch(false);
                EventSystem.current.SetSelectedGameObject(null);
            });
            SFXOn.enabled = false;
            SFXOff.enabled = false;
        }
        #endregion

        #region Achievement
        [SerializeField] List<Button> achievementList;
        [SerializeField] Text Comment;

        private void AwakeCall_Achievement()
        {
            Comment.text = "";

            for (var i = 0; i < achievementList.Count; i++)
            {
                achievementList[i].enabled = false;
            }

            // 0---AllEasy
            // 1---AllMedium
            // 2---AllHard
            // 3---Clear1
            // 4---Clear2
            // 5---Clear3
            // 6---Clear4
            // 7---AC1
            // 8---AC2
            // 9---AC3
            // 10---AC4
            // 11---ND1
            // 12---ND2
            // 13---ND3
            // 14---ND4
            // 15---35
            // 16---60
            // 17---85
        }



        #endregion






    }
}
