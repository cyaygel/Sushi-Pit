using System;
using UnityEngine;
using Utility;

public class StreakManager: MonoSingleton<StreakManager>
{
        [SerializeField] private int m_StreakThreshold = 5;

        private int _streakCounter;
        private bool _isStreakActive;
        private void OnEnable()
        { 
                PlayerManager.Instance.EarnedStars += CheckForStreak;
                _streakCounter = 0;
        }

        private void CheckForStreak(int value)
        {
                if (value > 0)
                {
                        if (value < 4)
                        {
                                if (_streakCounter >= m_StreakThreshold)
                                {
                                        StarUIManager.Instance.PlayStreakNotification(_streakCounter);
                                        PlayerManager.Instance.AddStarSpecial(value);
                                }
                                _streakCounter++;
                        }
                }
                else
                {
                        if (_streakCounter > m_StreakThreshold)
                        {
                                StarUIManager.Instance.PlayStreakLostNotification();
                        }
                        _streakCounter = 0;
                }

        }

        public bool IsStreakActive()
        {
                return _streakCounter >= m_StreakThreshold;
        }
}