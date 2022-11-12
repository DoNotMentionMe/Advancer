using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Adv
{

    public class SteamAchievement : PersistentSingleton<SteamAchievement>
    {
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
