using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BayatGames.SaveGameFree;
using UnityEngine.UI;

namespace Adv
{
    public class AchievementBtn : MonoBehaviour,
                                IPointerEnterHandler,
                                IPointerExitHandler,
                                ISelectHandler,
                                IDeselectHandler
    {
        [SerializeField] Sprite Unlock;
        [SerializeField] Sprite Lock;
        [SerializeField] AudioData EnterSound;
        [SerializeField] AchiveveShow achiveveShow;
        private bool IsUnlocked = false;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Start()
        {
            SaveGame.SavePath = SaveGamePath.DataPath;
            if (SaveGame.Exists(gameObject.name + "IsUnlocked"))
                IsUnlocked = SaveGame.Load<bool>(gameObject.name + "IsUnlocked");
            if (IsUnlocked)
                image.sprite = Unlock;
            else
                image.sprite = Lock;
        }

        //解锁steam成就时同步调用
        public void UnlockAchievementIcon()
        {
            // if (IsUnlocked == false)
            //     achiveveShow.StartShowAchieve(ChineseEnglishShift.language == Language.Chinese ? commentChinese : commentEnglish, Unlock);
            IsUnlocked = true;
            image.sprite = Unlock;
            SaveGame.SavePath = SaveGamePath.DataPath;
            SaveGame.Save<bool>(gameObject.name + "IsUnlocked", IsUnlocked);
        }

        public void LockAchievementIcon()
        {
            IsUnlocked = false;
            image.sprite = Lock;
            SaveGame.SavePath = SaveGamePath.DataPath;
            SaveGame.Save<bool>(gameObject.name + "IsUnlocked", IsUnlocked);
        }


        [SerializeField] Text Comment;
        [SerializeField, TextArea(3, 8)] string commentChinese;
        [SerializeField, TextArea(3, 8)] string commentEnglish;

        public void OnDeselect(BaseEventData eventData)
        {
            //Comment.text = "";
        }

        public void OnSelect(BaseEventData eventData)
        {
            Comment.text = ChineseEnglishShift.language == Language.Chinese ? commentChinese : commentEnglish;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Comment.text = "";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlayRandomSFX(EnterSound);
            Comment.text = ChineseEnglishShift.language == Language.Chinese ? commentChinese : commentEnglish;
        }
    }
}
