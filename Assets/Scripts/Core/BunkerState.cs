using UnityEngine;

namespace Bynker.Core
{
    [System.Serializable]
    public class ResourceBundle
    {
        public float Food;
        public float Water;
        public float Oxygen;
        public float Power;
        public float Medicine;
        public float Materials;
        public float Morale;
        public int Population;
    }

    public class BunkerState : MonoBehaviour
    {
        [Header("Current State")]
        public ResourceBundle Resources = new ResourceBundle
        {
            Food = 120f,
            Water = 120f,
            Oxygen = 100f,
            Power = 90f,
            Medicine = 30f,
            Materials = 50f,
            Morale = 65f,
            Population = 8
        };

        [Header("Progress")]
        public int CurrentMonth = 1;
        public int CurrentYear = 1;
        public int TotalDaysPassed = 0;

        public bool IsDestroyed => Resources.Oxygen <= 0f || Resources.Food <= 0f || Resources.Water <= 0f || Resources.Population <= 0;

        public void AdvanceMonth()
        {
            CurrentMonth += 1;
            TotalDaysPassed += 30;

            if (CurrentMonth > 12)
            {
                CurrentMonth = 1;
                CurrentYear += 1;
            }
        }

        public void ConsumeResources(float foodPerDay, float waterPerDay, float oxygenPerDay, float powerPerDay)
        {
            Resources.Food = Mathf.Max(0f, Resources.Food - foodPerDay * 30f);
            Resources.Water = Mathf.Max(0f, Resources.Water - waterPerDay * 30f);
            Resources.Oxygen = Mathf.Max(0f, Resources.Oxygen - oxygenPerDay * 30f);
            Resources.Power = Mathf.Max(0f, Resources.Power - powerPerDay * 30f);
        }

        public void AddResource(ResourceType type, float amount)
        {
            switch (type)
            {
                case ResourceType.Food:
                    Resources.Food += amount;
                    break;
                case ResourceType.Water:
                    Resources.Water += amount;
                    break;
                case ResourceType.Oxygen:
                    Resources.Oxygen += amount;
                    break;
                case ResourceType.Power:
                    Resources.Power += amount;
                    break;
                case ResourceType.Medicine:
                    Resources.Medicine += amount;
                    break;
                case ResourceType.Materials:
                    Resources.Materials += amount;
                    break;
                case ResourceType.Morale:
                    Resources.Morale += amount;
                    break;
            }
        }

        public void ApplyDisasterImpact(DisasterDefinition disaster)
        {
            if (disaster == null)
                return;

            Resources.Oxygen = Mathf.Max(0f, Resources.Oxygen - disaster.OxygenLoss);
            Resources.Food = Mathf.Max(0f, Resources.Food - disaster.FoodLoss);
            Resources.Water = Mathf.Max(0f, Resources.Water - disaster.WaterLoss);
            Resources.Power = Mathf.Max(0f, Resources.Power - disaster.PowerLoss);
            Resources.Morale = Mathf.Clamp(Resources.Morale - disaster.MoraleLoss, 0f, 100f);

            if (disaster.PopulationLoss > 0)
            {
                Resources.Population = Mathf.Max(0, Resources.Population - disaster.PopulationLoss);
            }
        }
    }

    public enum ResourceType
    {
        Food,
        Water,
        Oxygen,
        Power,
        Medicine,
        Materials,
        Morale
    }
}
