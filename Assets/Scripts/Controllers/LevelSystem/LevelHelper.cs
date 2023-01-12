using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHelper
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private int level;
    private int experience;
    private int increasedExperience = 50;
    private int experienceToNextLevel;


    private bool isAnimating;

    public LevelHelper(int level, int experience, int experienceToNextLevel)
    {
        this.level = level;
        this.experience = experience;
        this.experienceToNextLevel = experienceToNextLevel;
        
    }

    public int Level { get => level; set => level = value; }
    public int Experience { get => experience; set => experience = value; }
    public int ExperienceToNextLevel { get => experienceToNextLevel; set => experienceToNextLevel = value; }

    public void AddExperience(int amount)
    {
        Experience += amount;
        while (Experience >= ExperienceToNextLevel)
        {
            Level++;
            Experience -= ExperienceToNextLevel;
            ExperienceToNextLevel += increasedExperience * Level;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);

        
    }

}
