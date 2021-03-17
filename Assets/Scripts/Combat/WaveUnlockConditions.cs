using System;

[Flags]
public enum WaveUnlockConditions
{
    BLANK = 0,
    KillEnemies = 1, 
    Time = 4,
    KillEnemiesOfType = 8
}
