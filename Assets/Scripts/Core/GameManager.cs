using System.Collections.Generic;
using UnityEngine;

namespace Bynker.Core
{
    [CreateAssetMenu(fileName = "DisasterDefinition", menuName = "Bynker/Disaster Definition")]
    public class DisasterDefinition : ScriptableObject
    {
        public string DisasterName;
        public string Description;
        public int Month;
        public int Severity;
        public float FoodLoss;
        public float WaterLoss;
        public float OxygenLoss;
        public float PowerLoss;
        public float MoraleLoss;
        public int PopulationLoss;
    }

    public class GameManager : MonoBehaviour
    {
        public BunkerState bunkerState;
        public RefugeeSystem refugeeSystem;
        public DifficultySettings gameDifficulty;

        public List<DisasterDefinition> ApocalypseTimeline = new List<DisasterDefinition>();

        public float FoodConsumptionPerDay = 1.5f;
        public float WaterConsumptionPerDay = 1.2f;
        public float OxygenConsumptionPerDay = 1.8f;
        public float PowerConsumptionPerDay = 1.4f;

        private void Start()
        {
            if (bunkerState == null)
                bunkerState = FindObjectOfType<BunkerState>();

            if (refugeeSystem == null)
                refugeeSystem = FindObjectOfType<RefugeeSystem>();

            if (gameDifficulty == null)
                gameDifficulty = DifficultySettings.FromLevel(DifficultyLevel.Normal);

            ApplyDifficultyToGame();

            if (ApocalypseTimeline.Count == 0)
                DevelopDefaultTimeline();

            TriggerMonthlyCycle();
        }

        public void SetDifficulty(DifficultyLevel level)
        {
            gameDifficulty = DifficultySettings.FromLevel(level);
            ApplyDifficultyToGame();
        }

        private void ApplyDifficultyToGame()
        {
            FoodConsumptionPerDay = 1.5f * gameDifficulty.FoodConsumptionMultiplier;
            WaterConsumptionPerDay = 1.2f * gameDifficulty.WaterConsumptionMultiplier;
            OxygenConsumptionPerDay = 1.8f * gameDifficulty.OxygenConsumptionMultiplier;
            PowerConsumptionPerDay = 1.4f * gameDifficulty.PowerConsumptionMultiplier;
        }

        public void TriggerMonthlyCycle()
        {
            if (bunkerState == null)
                return;

            bunkerState.ConsumeResources(
                FoodConsumptionPerDay,
                WaterConsumptionPerDay,
                OxygenConsumptionPerDay,
                PowerConsumptionPerDay);

            var disaster = GetDisasterForMonth(bunkerState.CurrentMonth);
            if (disaster != null)
            {
                bunkerState.ApplyDisasterImpact(disaster);
                Debug.Log($"Disaster: {disaster.DisasterName} - {disaster.Description}");
            }

            if (refugeeSystem != null && Random.value < gameDifficulty.RefugeeArrivalChance)
            {
                var encounter = refugeeSystem.RollEncounter();
                Debug.Log($"Refugees arrived: {encounter.Name}. {encounter.Story}");
            }

            bunkerState.AdvanceMonth();

            if (bunkerState.IsDestroyed)
            {
                Debug.Log("The bunker has failed.");
            }
        }

        public DisasterDefinition GetDisasterForMonth(int month)
        {
            foreach (var disaster in ApocalypseTimeline)
            {
                if (disaster.Month == month)
                    return disaster;
            }

            return null;
        }

        private void DevelopDefaultTimeline()
        {
            ApocalypseTimeline.Clear();
            ApocalypseTimeline.Add(CreateDisaster("Flood", "The outside world is flooded by a sudden surge.", 1, 2, 12f, 8f, 15f, 5f, 12f, 1));
            ApocalypseTimeline.Add(CreateDisaster("Firestorm", "Ash and flames sweep across the surface.", 2, 3, 18f, 4f, 10f, 12f, 15f, 1));
            ApocalypseTimeline.Add(CreateDisaster("Blizzard", "Freezing temperatures and power failures hit the bunker.", 3, 2, 12f, 10f, 8f, 18f, 10f, 0));
            ApocalypseTimeline.Add(CreateDisaster("Solar Flare", "Electronics are damaged by intense radiation.", 4, 4, 6f, 5f, 10f, 18f, 20f, 0));
            ApocalypseTimeline.Add(CreateDisaster("Earthquake", "The bunker structure is shaken and unstable.", 5, 3, 10f, 8f, 14f, 12f, 15f, 1));
            ApocalypseTimeline.Add(CreateDisaster("Biohazard", "A microbial outbreak causes fear and illness.", 6, 5, 8f, 6f, 12f, 7f, 25f, 2));
            ApocalypseTimeline.Add(CreateDisaster("Heatwave", "The bunker is hit by a dangerous heatwave.", 7, 3, 10f, 4f, 16f, 15f, 12f, 1));
            ApocalypseTimeline.Add(CreateDisaster("Toxic Storm", "Poison-filled clouds block the air filters.", 8, 5, 5f, 5f, 20f, 12f, 20f, 2));
            ApocalypseTimeline.Add(CreateDisaster("Tsunami", "A massive wave damages bunker access and systems.", 9, 4, 14f, 10f, 18f, 16f, 18f, 2));
            ApocalypseTimeline.Add(CreateDisaster("Final Collapse", "The world enters a final collapse after the apocalypse.", 10, 6, 20f, 12f, 22f, 18f, 28f, 3));
        }

        private DisasterDefinition CreateDisaster(
            string name,
            string description,
            int month,
            int severity,
            float foodLoss,
            float waterLoss,
            float oxygenLoss,
            float powerLoss,
            float moraleLoss,
            int populationLoss)
        {
            var disaster = ScriptableObject.CreateInstance<DisasterDefinition>();
            disaster.DisasterName = name;
            disaster.Description = description;
            disaster.Month = month;
            disaster.Severity = severity;
            disaster.FoodLoss = foodLoss;
            disaster.WaterLoss = waterLoss;
            disaster.OxygenLoss = oxygenLoss;
            disaster.PowerLoss = powerLoss;
            disaster.MoraleLoss = moraleLoss;
            disaster.PopulationLoss = populationLoss;
            return disaster;
        }
    }
}
