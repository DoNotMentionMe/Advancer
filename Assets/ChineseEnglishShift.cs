using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Adv
{
    public enum Language
    {
        Chinese,
        English
    }
    public class ChineseEnglishShift : MonoBehaviour
    {
        public static Language language = Language.Chinese;
        private bool IsFirst;

        [SerializeField] Canvas LanguageSelectMenu;
        [SerializeField] Button ChineseBtn;
        [SerializeField] Button EnglishBtn;
        [SerializeField] LanguageEventChannel LanguageChange;

        private const string MainScene = "Main Scene";

        [SerializeField] Text TestText;

        #region Test
        // private void Update()
        // {
        //     TestText.text = "当前语言：" + language.ToString();
        // }

        // private string LanguageToString(Language language)
        // {
        //     var str = "";
        //     if (language == Language.English)
        //         str = "English";
        //     else if (language == Language.Chinese)
        //         str = "Chinese";
        //     return str;
        // }

        #endregion

        private void Start()
        {
            IsFirst = true;

            LanguageChange.AddListener((languageC) =>
            {
                language = languageC;
                SaveGame.SavePath = SaveGamePath.DataPath;
                SaveGame.Save<Language>("language", language);
            });

            //读档
            SaveGame.SavePath = SaveGamePath.DataPath;
            if (SaveGame.Exists("IsFirst"))
            {
                IsFirst = SaveGame.Load<bool>("IsFirst");
            }
            if (SaveGame.Exists("language"))
            {
                language = SaveGame.Load<Language>("language");
            }

            if (IsFirst)
            {
                //置否
                IsFirst = false;
                SaveGame.SavePath = SaveGamePath.DataPath;
                SaveGame.Save<bool>("IsFirst", IsFirst);
                //启动语言选择菜单
                LanguageSelectMenu.enabled = true;
                ChineseBtn.enabled = true;
                EnglishBtn.enabled = true;
            }
            else
            {
                //跳转主界面
                SceneManager.LoadScene(MainScene);
            }
        }

        private void Update()
        {
            if (Keyboard.current.anyKey.isPressed && EventSystem.current.currentSelectedGameObject == null)
            {
                ChineseBtn.Select();
            }

        }

        private void OnEnable()
        {
            ChineseBtn.onClick.AddListener(SwitchChinese);
            EnglishBtn.onClick.AddListener(SwitchEnglish);
        }

        private void OnDisable()
        {
            ChineseBtn.onClick.RemoveListener(SwitchChinese);
            EnglishBtn.onClick.RemoveListener(SwitchEnglish);
        }

        private void SwitchChinese()
        {
            SwitchLanguage(Language.Chinese);
            SaveGame.SavePath = SaveGamePath.DataPath;
            SaveGame.Save<Language>("language", language);
            LanguageSelectMenu.enabled = false;
            EnglishBtn.enabled = false;
            ChineseBtn.enabled = false;
            //跳转主界面
            SceneManager.LoadScene(MainScene);
        }

        private void SwitchEnglish()
        {
            SwitchLanguage(Language.English);
            SaveGame.SavePath = SaveGamePath.DataPath;
            SaveGame.Save<Language>("language", language);
            LanguageSelectMenu.enabled = false;
            ChineseBtn.enabled = false;
            EnglishBtn.enabled = false;

            //跳转主界面
            SceneManager.LoadScene(MainScene);
        }

        public void SwitchLanguage(Language languageC)
        {
            language = languageC;
        }
    }
}
