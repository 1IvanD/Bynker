using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bynker.Core
{
    public enum AnimalType
    {
        Chicken,
        Cow,
        Pig,
        Sheep,
        Rabbit,
        Goat,
        Bee,
        Dog,
        Horse,
        Wolf,
        Boar,
        Duck,
        Turkey
    }

    [Serializable]
    public class AnimalState
    {
        public AnimalType Type;
        public int Count;
        public bool IsInfected;
        public float Hunger;
        public float Health;
        public float GrowthRate;

        public AnimalState(AnimalType type, int count, bool infected = false)
        {
            Type = type;
            Count = count;
            IsInfected = infected;
            Hunger = 50f;
            Health = 100f;
            GrowthRate = 1f;
        }

        public void Feed(float foodAmount)
        {
            Hunger = Mathf.Clamp(Hunger - foodAmount, 0f, 100f);
            Health = Mathf.Clamp(Health + foodAmount * 0.2f, 0f, 100f);
        }

        public void UpdateGrowth()
        {
            if (!IsInfected)
            {
                GrowthRate = Mathf.Clamp(GrowthRate + 0.05f, 0f, 10f);
                Count += Mathf.FloorToInt(GrowthRate * 0.1f);
            }
            else
            {
                Health -= 10f;
                Count = Mathf.Max(0, Count - 1);
            }
        }
    }

    [Serializable]
    public class EventEncounter
    {
        public string EventName;
        public string Description;
        public EventType Type;
        public int FoodReward;
        public int WaterReward;
        public int MaterialReward;
        public int MedicineReward;
        public int PopulationGain;
        public int PopulationLoss;
        public float MoraleDelta;
        public float RiskChance;
        public bool CanInfect;
        public bool IsHelpful;
        public bool IsDangerous;

        public EventEncounter(
            string eventName,
            string description,
            EventType type,
            int foodReward,
            int waterReward,
            int materialReward,
            int medicineReward,
            int populationGain,
            int populationLoss,
            float moraleDelta,
            float riskChance,
            bool canInfect,
            bool isHelpful,
            bool isDangerous)
        {
            EventName = eventName;
            Description = description;
            Type = type;
            FoodReward = foodReward;
            WaterReward = waterReward;
            MaterialReward = materialReward;
            MedicineReward = medicineReward;
            PopulationGain = populationGain;
            PopulationLoss = populationLoss;
            MoraleDelta = moraleDelta;
            RiskChance = riskChance;
            CanInfect = canInfect;
            IsHelpful = isHelpful;
            IsDangerous = isDangerous;
        }
    }

    public enum EventType
    {
        Trader,
        Survivor,
        InfectedSurvivor,
        Farmer,
        Medic,
        Raider,
        Scientist,
        Engineer,
        Smuggler,
        Scout
    }

    public class EncounterSystem : MonoBehaviour
    {
        public List<EventEncounter> PossibleEvents = new List<EventEncounter>();

        private void Awake()
        {
            BuildDefaultEvents();
        }

        public EventEncounter RollEvent()
        {
            if (PossibleEvents.Count == 0)
                BuildDefaultEvents();

            int index = Random.Range(0, PossibleEvents.Count);
            return PossibleEvents[index];
        }

        public void ResolveEvent(BunkerState bunkerState, EventEncounter encounter)
        {
            if (bunkerState == null || encounter == null)
                return;

            bunkerState.Resources.Food += encounter.FoodReward;
            bunkerState.Resources.Water += encounter.WaterReward;
            bunkerState.Resources.Materials += encounter.MaterialReward;
            bunkerState.Resources.Medicine += encounter.MedicineReward;
            bunkerState.Resources.Population += encounter.PopulationGain;
            bunkerState.Resources.Population = Mathf.Max(0, bunkerState.Resources.Population - encounter.PopulationLoss);
            bunkerState.Resources.Morale += encounter.MoraleDelta;

            if (encounter.CanInfect && Random.value <= encounter.RiskChance)
            {
                bunkerState.Resources.Morale -= 10f;
                Debug.Log("The event caused infection risk in the bunker.");
            }

            if (encounter.IsDangerous && Random.value <= encounter.RiskChance)
            {
                bunkerState.Resources.Power -= 20f;
                bunkerState.Resources.Morale -= 15f;
                Debug.Log("The event caused a dangerous outcome.");
            }

            bunkerState.Resources.Morale = Mathf.Clamp(bunkerState.Resources.Morale, 0f, 100f);
        }

        private void BuildDefaultEvents()
        {
            PossibleEvents.Clear();

            PossibleEvents.Add(new EventEncounter(
                "Trader",
                "A caravan approaches the bunker with food and tools.",
                EventType.Trader,
                30, 20, 18, 4, 0, 0, 12f, 0.2f, false, true, false));

            PossibleEvents.Add(new EventEncounter(
                "Farmer",
                "A lone farmer offers seeds and fresh food.",
                EventType.Farmer,
                25, 10, 8, 2, 0, 0, 8f, 0.15f, false, true, false));

            PossibleEvents.Add(new EventEncounter(
                "Infected Survivor",
                "A sick wanderer reaches the bunker gates.",
                EventType.InfectedSurvivor,
                0, 0, 5, 2, 1, 1, -15f, 0.7f, true, false, true));

            PossibleEvents.Add(new EventEncounter(
                "Medic",
                "A doctor offers treatment and medicine.",
                EventType.Medic,
                0, 0, 3, 18, 0, 0, 15f, 0.2f, false, true, false));

            PossibleEvents.Add(new EventEncounter(
                "Raider",
                "A hostile gang demands supplies.",
                EventType.Raider,
                0, 0, 0, 0, 0, 2, -20f, 0.6f, false, false, true));

            PossibleEvents.Add(new EventEncounter(
                "Scientist",
                "A researcher offers rare knowledge and lab tools.",
                EventType.Scientist,
                5, 5, 12, 8, 0, 0, 10f, 0.3f, false, true, false));

            PossibleEvents.Add(new EventEncounter(
                "Engineer",
                "A mechanic arrives with spare parts and repair notes.",
                EventType.Engineer,
                0, 0, 16, 3, 0, 0, 10f, 0.25f, false, true, false));

            PossibleEvents.Add(new EventEncounter(
                "Scout",
                "A scout returns with a map and rumors of a safer route.",
                EventType.Scout,
                8, 5, 6, 2, 0, 0, 7f, 0.18f, false, true, false));

            PossibleEvents.Add(new EventEncounter(
                "Smuggler",
                "A smuggler offers rare goods for a risky bargain.",
                EventType.Smuggler,
                15, 8, 14, 6, 0, 1, 5f, 0.45f, false, true, true));

            PossibleEvents.Add(new EventEncounter(
                "Survivor",
                "A desperate survivor asks to join the bunker.",
                EventType.Survivor,
                10, 10, 4, 2, 1, 0, 7f, 0.35f, false, true, false));
        }
    }
}
