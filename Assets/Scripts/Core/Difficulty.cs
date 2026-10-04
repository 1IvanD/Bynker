using System;
using UnityEngine;

namespace Bynker.Core
{
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }

    [Serializable]
    public class DifficultySettings
    {
        public DifficultyLevel Level;

        [Header("Consumption Multiplier")]
        public float FoodConsumptionMultiplier = 1f;
        public float WaterConsumptionMultiplier = 1f;
        public float OxygenConsumptionMultiplier = 1f;
        public float PowerConsumptionMultiplier = 1f;

        [Header("Disaster Impact Multiplier")]
        public float DisasterDamageMultiplier = 1f;
        public float PopulationLossMultiplier = 1f;

        [Header("Starting Resources")]
        public float InitialResourcesMultiplier = 1f;

        [Header("Refugee Arrival Chance")]
        public float RefugeeArrivalChance = 0.3f;

        public static DifficultySettings FromLevel(DifficultyLevel level)
        {
            var settings = new DifficultySettings
            {
                Level = level
            };

            switch (level)
            {
                case DifficultyLevel.Easy:
                    settings.FoodConsumptionMultiplier = 0.7f;
                    settings.WaterConsumptionMultiplier = 0.7f;
                    settings.OxygenConsumptionMultiplier = 0.8f;
                    settings.PowerConsumptionMultiplier = 0.8f;
                    settings.DisasterDamageMultiplier = 0.6f;
                    settings.PopulationLossMultiplier = 0.5f;
                    settings.InitialResourcesMultiplier = 1.5f;
                    settings.RefugeeArrivalChance = 0.2f;
                    break;

                case DifficultyLevel.Normal:
                    settings.FoodConsumptionMultiplier = 1f;
                    settings.WaterConsumptionMultiplier = 1f;
                    settings.OxygenConsumptionMultiplier = 1f;
                    settings.PowerConsumptionMultiplier = 1f;
                    settings.DisasterDamageMultiplier = 1f;
                    settings.PopulationLossMultiplier = 1f;
                    settings.InitialResourcesMultiplier = 1f;
                    settings.RefugeeArrivalChance = 0.3f;
                    break;

                case DifficultyLevel.Hard:
                    settings.FoodConsumptionMultiplier = 1.3f;
                    settings.WaterConsumptionMultiplier = 1.3f;
                    settings.OxygenConsumptionMultiplier = 1.2f;
                    settings.PowerConsumptionMultiplier = 1.25f;
                    settings.DisasterDamageMultiplier = 1.4f;
                    settings.PopulationLossMultiplier = 1.5f;
                    settings.InitialResourcesMultiplier = 0.8f;
                    settings.RefugeeArrivalChance = 0.5f;
                    break;
            }

            return settings;
        }
    }
}
