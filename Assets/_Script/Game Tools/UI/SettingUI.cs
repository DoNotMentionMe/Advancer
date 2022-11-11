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

        public bool IsOpen = false;

        private void Awake()
        {
            SettingUICanvas.enabled = IsOpen;
            AwakeCall_LanguageChange();
            AwakeCall_ScreenChange();
        }

        private void Start()
        {
            input.onCloseUI += SwitchSettingUI;
            StartCall_ScreenChange();
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
            SettingUISwitchCall(IsOpen);
        }

        private void OnDestroy()
        {
            input.onCloseUI -= SwitchSettingUI;
        }

        #region LanguageChange
        [Space]
        [SerializeField] Button Englishbtn;
        [SerializeField] Button Chinesebtn;
        [SerializeField] LanguageEventChannel LanguageChange;

        private void AwakeCall_LanguageChange()
        {
            //加载按键时间
            Englishbtn.onClick.AddListener(() =>
            {
                ChineseEnglishShift.language = Language.English;
                SaveGame.SavePath = SaveGamePath.DataPath;
                SaveGame.Save<Language>("language", ChineseEnglishShift.language);
                LanguageChange.Broadcast(ChineseEnglishShift.language);
                EventSystem.current.SetSelectedGameObject(null);
            });
            Chinesebtn.onClick.AddListener(() =>
            {
                ChineseEnglishShift.language = Language.Chinese;
                SaveGame.SavePath = SaveGamePath.DataPath;
                SaveGame.Save<Language>("language", ChineseEnglishShift.language);
                LanguageChange.Broadcast(ChineseEnglishShift.language);
                EventSystem.current.SetSelectedGameObject(null);
            });

            //关闭按键
            Englishbtn.enabled = false;
            Chinesebtn.enabled = false;
        }

        private void SettingUISwitchCall(bool Switch)
        {
            Englishbtn.enabled = Switch;
            Chinesebtn.enabled = Switch;
        }

        #endregion

        #region 屏幕修改
        [Space]
        [SerializeField] Button FullScreen;
        [SerializeField] Button Windowed;
        [SerializeField] Button Resolution1920X1080;
        [SerializeField] Button Resolution1366X768;

        public static FullScreenMode currentScreenSet = FullScreenMode.Windowed;
        public static int currentScreenWidth = 1366;
        public static int currentScreenHeight = 768;

        private void AwakeCall_ScreenChange()
        {
            SaveGame.SavePath = SaveGamePath.DataPath;

            FullScreen.onClick.AddListener(() =>
            {
                Screen.SetResolution(currentScreenWidth, currentScreenHeight, FullScreenMode.FullScreenWindow);
                currentScreenSet = FullScreenMode.FullScreenWindow;
                SaveGame.Save<FullScreenMode>("currentScreenSet", currentScreenSet);
            });
            Windowed.onClick.AddListener(() =>
            {
                Screen.SetResolution(currentScreenWidth, currentScreenHeight, FullScreenMode.Windowed);
                currentScreenSet = FullScreenMode.Windowed;
                SaveGame.Save<FullScreenMode>("currentScreenSet", currentScreenSet);
            });
            Resolution1920X1080.onClick.AddListener(() =>
            {
                Screen.SetResolution(1920, 1080, currentScreenSet);
                currentScreenWidth = 1920;
                currentScreenHeight = 1080;
                SaveGame.Save<int>("currentScreenWidth", currentScreenWidth);
                SaveGame.Save<int>("currentScreenHeight", currentScreenHeight);
            });
            Resolution1366X768.onClick.AddListener(() =>
            {
                Screen.SetResolution(1366, 768, currentScreenSet);
                currentScreenWidth = 1366;
                currentScreenHeight = 768;
                SaveGame.Save<int>("currentScreenWidth", currentScreenWidth);
                SaveGame.Save<int>("currentScreenHeight", currentScreenHeight);
            });
        }

        private void StartCall_ScreenChange()
        {
            //加载
        }

        #endregion

    }
}
