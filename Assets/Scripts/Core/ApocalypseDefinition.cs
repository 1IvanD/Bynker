using UnityEngine;

namespace Bynker.Core
{
    [CreateAssetMenu(fileName = "ApocalypseDefinition", menuName = "Bynker/Apocalypse Definition")]
    public class ApocalypseDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string ApocalypseName;
        [TextArea(2, 4)]
        public string Description;
        public int Month;
        public int Severity; // 1-6 шкала опасности

        [Header("Resource Impact")]
        public float FoodLoss;
        public float WaterLoss;
        public float OxygenLoss;
        public float PowerLoss;
        public float MoraleLoss;
        public int PopulationLoss;

        [Header("Special Effects")]
        public bool IsContagious;
        public float ContagionChance; // 0-1
        public bool CanDestroyRooms;
        public float RoomDamageChance;
        public bool CausesStarvation;
        public bool CausesPowerLoss;

        [Header("Duration")]
        public int DurationMonths = 1;
        public bool IsMultiMonth;
    }
}
