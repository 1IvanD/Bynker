using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bynker.Core
{
    public enum ProfessionType
    {
        Engineer,
        Farmer,
        Doctor,
        Scientist,
        Worker,
        Soldier
    }

    [Serializable]
    public class Survivor
    {
        public string Name;
        public ProfessionType Profession;
        public float Morale = 50f;
        public float Health = 100f;
        public bool IsAlive = true;
        public int DaysInBunker;

        public Survivor(string name, ProfessionType profession)
        {
            Name = name;
            Profession = profession;
            Morale = 50f;
            Health = 100f;
            IsAlive = true;
            DaysInBunker = 0;
        }
    }

    [CreateAssetMenu(fileName = "ProfessionDefinition", menuName = "Bynker/Profession Definition")]
    public class ProfessionDefinition : ScriptableObject
    {
        [Header("Identity")]
        public ProfessionType Type;
        public string ProfessionName;
        [TextArea(2, 4)]
        public string Description;

        [Header("Bonuses")]
        public float PowerProductionBonus;
        public float FoodProductionBonus;
        public float WaterProductionBonus;
        public float HealthBonus;
        public float CatastropheResistance;
        public float DefenseBonus;

        [Header("Penalties")]
        public float MoraleThreshold = 30f;

        public static List<ProfessionDefinition> GetDefaultDefinitions()
        {
            return new List<ProfessionDefinition>
            {
                Create("Engineer", ProfessionType.Engineer, "Improves energy systems and repairs.", 20f, 0f, 0f, 5f, 10f, 12f),
                Create("Farmer", ProfessionType.Farmer, "Improves food and water production.", 0f, 25f, 20f, 5f, 8f, 0f),
                Create("Doctor", ProfessionType.Doctor, "Improves health and reduces losses.", 0f, 0f, 0f, 15f, 15f, 2f),
                Create("Scientist", ProfessionType.Scientist, "Improves research and resilience.", 8f, 5f, 5f, 10f, 20f, 5f),
                Create("Worker", ProfessionType.Worker, "Balanced helper for all systems.", 8f, 8f, 8f, 5f, 5f, 5f),
                Create("Soldier", ProfessionType.Soldier, "Improves defense and survival under attack.", 0f, 0f, 0f, 5f, 12f, 18f)
            };
        }

        private static ProfessionDefinition Create(string name, ProfessionType type, string description,
            float power, float food, float water, float health, float resistance, float defense)
        {
            var def = ScriptableObject.CreateInstance<ProfessionDefinition>();
            def.ProfessionName = name;
            def.Type = type;
            def.Description = description;
            def.PowerProductionBonus = power;
            def.FoodProductionBonus = food;
            def.WaterProductionBonus = water;
            def.HealthBonus = health;
            def.CatastropheResistance = resistance;
            def.DefenseBonus = defense;
            def.MoraleThreshold = 30f;
            return def;
        }
    }
}
