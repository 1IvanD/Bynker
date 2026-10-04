using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bynker.Building
{
    [Serializable]
    public class RoomDefinition
    {
        public string RoomName;
        public int Level;
        public int MaxLevel;
        public float UpgradeCost;

        [Header("Bonuses")]
        public float FoodBonus;
        public float WaterBonus;
        public float OxygenBonus;
        public float PowerBonus;
        public float DefenseBonus;
        public float MoraleBonus;

        public RoomDefinition(string roomName, int level, int maxLevel, float upgradeCost,
            float foodBonus = 0f, float waterBonus = 0f, float oxygenBonus = 0f,
            float powerBonus = 0f, float defenseBonus = 0f, float moraleBonus = 0f)
        {
            RoomName = roomName;
            Level = level;
            MaxLevel = maxLevel;
            UpgradeCost = upgradeCost;
            FoodBonus = foodBonus;
            WaterBonus = waterBonus;
            OxygenBonus = oxygenBonus;
            PowerBonus = powerBonus;
            DefenseBonus = defenseBonus;
            MoraleBonus = moraleBonus;
        }
    }

    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private List<RoomDefinition> rooms = new List<RoomDefinition>();
        [SerializeField] private SharedResourceState sharedResources;

        private void Awake()
        {
            if (sharedResources == null)
                sharedResources = FindObjectOfType<SharedResourceState>();

            InitializeDefaultRooms();
        }

        public void InitializeDefaultRooms()
        {
            rooms.Clear();
            rooms.Add(new RoomDefinition("Generator", 1, 5, 25f, 0f, 0f, 0f, 10f, 0f, 2f));
            rooms.Add(new RoomDefinition("Water Purifier", 1, 5, 20f, 0f, 12f, 0f, 0f, 0f, 2f));
            rooms.Add(new RoomDefinition("Greenhouse", 1, 5, 30f, 15f, 0f, 0f, 0f, 0f, 5f));
            rooms.Add(new RoomDefinition("Air Filters", 1, 5, 28f, 0f, 0f, 12f, 0f, 0f, 3f));
            rooms.Add(new RoomDefinition("Medbay", 1, 5, 35f, 0f, 0f, 0f, 0f, 0f, 8f));
            rooms.Add(new RoomDefinition("Wall", 1, 5, 40f, 0f, 0f, 0f, 0f, 12f, 4f));
            rooms.Add(new RoomDefinition("Workshop", 1, 5, 26f, 0f, 0f, 0f, 5f, 4f, 2f));
        }

        public bool TryUpgradeRoom(string roomName)
        {
            var room = rooms.Find(r => r.RoomName == roomName);
            if (room == null)
            {
                Debug.LogWarning($"Room '{roomName}' not found");
                return false;
            }

            if (room.Level >= room.MaxLevel)
            {
                Debug.Log($"{roomName} already at max level");
                return false;
            }

            if (sharedResources == null)
            {
                Debug.LogWarning("SharedResourceState is missing");
                return false;
            }

            if (!sharedResources.TrySpendResources(room.UpgradeCost, 0f, 0f, 0f, 0f, 0f, 0f))
            {
                Debug.LogWarning($"Not enough resources to upgrade {roomName}");
                return false;
            }

            room.Level++;
            ApplyRoomBonuses(room);
            Debug.Log($"Room '{roomName}' upgraded to level {room.Level}");
            return true;
        }

        public void ApplyRoomBonuses(RoomDefinition room)
        {
            if (sharedResources == null)
                return;

            sharedResources.FoodProductionPerMonth += room.FoodBonus;
            sharedResources.WaterProductionPerMonth += room.WaterBonus;
            sharedResources.OxygenProductionPerMonth += room.OxygenBonus;
            sharedResources.PowerProductionPerMonth += room.PowerBonus;
            sharedResources.DefenseBonus += room.DefenseBonus;
            sharedResources.MoraleBonus += room.MoraleBonus;
        }

        public List<RoomDefinition> GetRooms()
        {
            return rooms;
        }
    }

    public class SharedResourceState : MonoBehaviour
    {
        [Header("Resources")]
        public float Food;
        public float Water;
        public float Oxygen;
        public float Power;
        public float Medicine;
        public float Materials;
        public float Morale;
        public int Population;

        [Header("Production per month")]
        public float FoodProductionPerMonth;
        public float WaterProductionPerMonth;
        public float OxygenProductionPerMonth;
        public float PowerProductionPerMonth;

        [Header("Defense and morale")]
        public float DefenseBonus;
        public float MoraleBonus;

        public bool TrySpendResources(float food, float water, float oxygen, float power, float medicine, float materials, float morale)
        {
            if (Food >= food && Water >= water && Oxygen >= oxygen && Power >= power &&
                Medicine >= medicine && Materials >= materials && Morale >= morale)
            {
                Food -= food;
                Water -= water;
                Oxygen -= oxygen;
                Power -= power;
                Medicine -= medicine;
                Materials -= materials;
                Morale -= morale;
                return true;
            }

            return false;
        }

        public void AddMonthlyProduction()
        {
            Food += FoodProductionPerMonth;
            Water += WaterProductionPerMonth;
            Oxygen += OxygenProductionPerMonth;
            Power += PowerProductionPerMonth;
            Morale += MoraleBonus;
        }
    }
}
