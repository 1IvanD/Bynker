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
        public bool IsContagious;
        public float ContagionChance;
    }
}
