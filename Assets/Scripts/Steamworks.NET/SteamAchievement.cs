using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Adv
{

    public class SteamAchievement : PersistentSingleton<SteamAchievement>
    {
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
        public List<AchievementBtn> achievementList;

        public void Reach_Achievement(AchievementType ach_type)
        {
            Complete_Achievement(ach_type.ToString());
        }

        private void Complete_Achievement(string achievementName)
        {
            if (!SteamManager.Initialized)
            {
                return;
            }
            else
            {
                if (SteamUserStats.SetAchievement(achievementName))
                    SteamUserStats.StoreStats();
            }
        }

        public void Remove_Achievement(AchievementType ach_type)
        {
            Clear_Archievement(ach_type.ToString());
        }

        private void Clear_Archievement(string achievementName)
        {
            if (!SteamManager.Initialized)
            {
                return;
            }
            else
            {
                if (SteamUserStats.ClearAchievement(achievementName))
                    SteamUserStats.StoreStats();
            }
        }

        public void Clear_AllArchievements()
        {
            if (!SteamManager.Initialized)
            {
                return;
            }
            else
            {
                foreach (var achievementName in Enum.GetNames(typeof(AchievementType)))
                {
                    if (SteamUserStats.ClearAchievement(achievementName))
                        SteamUserStats.StoreStats();
                }
                for (var i = 0; i < achievementList.Count; i++)
                {
                    achievementList[i].LockAchievementIcon();
                }
            }
        }
    }

    public enum AchievementType
    {
        AllEasy_Clearance,
        AllMedium_Clearance,
        AllHard_Clearance,
        Level1Hard_Clearance,
        Level2Hard_Clearance,
        Level3Hard_Clearance,
        Level4Hard_Clearance,
        Level1Hard_AllCombo,
        Level2Hard_AllCombo,
        Level3Hard_AllCombo,
        Level4Hard_AllCombo,
        Level1Hard_NotDamage,
        Level2Hard_NotDamage,
        Level3Hard_NotDamage,
        Level4Hard_NotDamage,
        Survive_35seconds,
        Survive_60seconds,
        Survive_85seconds,
    }
}
