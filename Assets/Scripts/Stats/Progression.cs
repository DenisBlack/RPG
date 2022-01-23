using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "RPG Project/Stats/Progression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField] private ProgressionCharacterClass[] _progressionCharacterClasses = null;
    private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

    public float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
        BuildLookup();

        float[] levels = lookupTable[characterClass][stat];
        if (levels.Length < level)
        {
            return 0;
        }

        return levels[level - 1];
    }

    public int GetLevels(Stat stat, CharacterClass characterClass)
    {
        BuildLookup();
        
        float[] levels = lookupTable[characterClass][stat];
        return levels.Length;
    }
    
    private void BuildLookup()
    {
        if(lookupTable != null) return;

        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
        
        foreach (ProgressionCharacterClass progressionClass in _progressionCharacterClasses)
        {
            var statLookupTable = new Dictionary<Stat, float[]>();

            foreach (ProgressionStat progressionStat in progressionClass.Stats)
            {
                statLookupTable[progressionStat.Stat] = progressionStat.Levels;
            }
            
            lookupTable[progressionClass.CharacterClass] = statLookupTable;
        }
    }

    [Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass CharacterClass;
        public ProgressionStat[] Stats;
    }
    
    [System.Serializable]
    class ProgressionStat
    {
        public Stat Stat;
        public float[] Levels;
    }
}
