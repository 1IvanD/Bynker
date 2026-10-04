using System.Collections.Generic;
using UnityEngine;

namespace Bynker.Core
{
    public class ApocalypseManager : MonoBehaviour
    {
        public List<ApocalypseDefinition> ApocalypseList = new List<ApocalypseDefinition>();
        public int CurrentIndex = 0;

        private void Awake()
        {
            BuildDefaultApocalypses();
        }

        public void BuildDefaultApocalypses()
        {
            ApocalypseList.Clear();

            AddApocalypse("Tsunami", "A giant wave crashes over the world and destroys coastal shelters.", 1, 3, 20f, 18f, 10f, 12f, 18f, 2, true, 0.2f, true, 0.35f, false, false, 1);
            AddApocalypse("Hurricane", "A violent hurricane tears across the region and breaks infrastructure.", 2, 3, 18f, 12f, 8f, 18f, 16f, 1, false, 0f, true, 0.5f, false, true, 1);
            AddApocalypse("Earthquake", "The ground shakes violently and ruptures the bunker walls.", 3, 4, 16f, 10f, 15f, 15f, 20f, 2, false, 0f, true, 0.55f, false, false, 1);
            AddApocalypse("Solar Flare", "A solar storm burns electronics and disrupts the power grid.", 4, 5, 8f, 6f, 10f, 22f, 18f, 1, false, 0f, true, 0.7f, false, true, 1);
            AddApocalypse("Meteor Impact", "A meteor strikes the atmosphere and causes mass destruction.", 5, 6, 24f, 14f, 18f, 20f, 25f, 3, false, 0f, true, 0.8f, false, false, 1);
            AddApocalypse("Zombie Outbreak", "The dead begin to walk and the bunker is under siege.", 6, 6, 22f, 10f, 12f, 8f, 30f, 4, true, 0.8f, false, 0f, true, false, 1);
            AddApocalypse("Nuclear Fallout", "Radiation poisons the air and spreads across the region.", 7, 6, 12f, 12f, 22f, 10f, 28f, 3, true, 0.65f, false, 0f, true, false, 1);
            AddApocalypse("Volcanic Eruption", "The sky fills with ash and the world turns into a furnace.", 8, 5, 18f, 8f, 16f, 16f, 22f, 2, false, 0f, true, 0.4f, false, false, 1);
            AddApocalypse("Acid Rain", "The rain eats through metal and destroys exposed structures.", 9, 4, 10f, 14f, 18f, 12f, 18f, 2, false, 0f, true, 0.4f, false, false, 1);
            AddApocalypse("Dark Winter", "An endless cold traps the bunker in survival mode.", 10, 5, 14f, 16f, 15f, 18f, 24f, 2, false, 0f, true, 0.45f, true, false, 1);
            AddApocalypse("Toxic Fog", "An invisible fog spreads death and contaminates everything.", 11, 5, 10f, 10f, 20f, 10f, 24f, 2, true, 0.7f, false, 0f, true, false, 1);
            AddApocalypse("Black Hole Event", "A gravity event destabilizes the whole planet.", 12, 6, 20f, 18f, 22f, 18f, 30f, 4, false, 0f, true, 0.9f, false, false, 1);
            AddApocalypse("Artificial Sun", "A massive plasma flare makes the sky burn alive.", 13, 5, 12f, 8f, 18f, 20f, 20f, 2, false, 0f, true, 0.5f, false, true, 1);
            AddApocalypse("Planetary Dust", "The atmosphere is consumed by dust and dark particles.", 14, 4, 12f, 10f, 16f, 16f, 18f, 2, false, 0f, true, 0.3f, false, false, 1);
            AddApocalypse("Swarms", "Insects, birds, and other creatures begin to devour everything.", 15, 4, 26f, 8f, 8f, 10f, 18f, 1, false, 0f, false, 0f, false, false, 1);
            AddApocalypse("Superstorm", "The sky itself becomes hostile and the bunker is battered nonstop.", 16, 5, 18f, 16f, 14f, 18f, 24f, 2, false, 0f, true, 0.6f, false, true, 1);
            AddApocalypse("Mass Infection", "A contagious disease spreads among the survivors.", 17, 6, 12f, 8f, 16f, 8f, 28f, 3, true, 0.85f, false, 0f, true, false, 1);
            AddApocalypse("Grid Failure", "The power network collapses and everything goes dark.", 18, 4, 6f, 4f, 8f, 30f, 16f, 1, false, 0f, true, 0.5f, false, true, 1);
            AddApocalypse("Final Collapse", "Civilization fails completely and the bunker must endure the end.", 19, 6, 22f, 18f, 18f, 24f, 30f, 4, true, 0.9f, true, 0.8f, true, true, 1);
            AddApocalypse("Solar Eclipse of Doom", "The world loses light and temperature changes catastrophically.", 20, 5, 14f, 10f, 16f, 14f, 22f, 2, false, 0f, false, 0f, true, false, 1);
        }

        public void AddApocalypse(
            string name,
            string description,
            int month,
            int severity,
            float foodLoss,
            float waterLoss,
            float oxygenLoss,
            float powerLoss,
            float moraleLoss,
            int populationLoss,
            bool isContagious,
            float contagionChance,
            bool canDestroyRooms,
            float roomDamageChance,
            bool causesStarvation,
            bool causesPowerLoss,
            int durationMonths)
        {
            var apoc = ScriptableObject.CreateInstance<ApocalypseDefinition>();
            apoc.ApocalypseName = name;
            apoc.Description = description;
            apoc.Month = month;
            apoc.Severity = severity;
            apoc.FoodLoss = foodLoss;
            apoc.WaterLoss = waterLoss;
            apoc.OxygenLoss = oxygenLoss;
            apoc.PowerLoss = powerLoss;
            apoc.MoraleLoss = moraleLoss;
            apoc.PopulationLoss = populationLoss;
            apoc.IsContagious = isContagious;
            apoc.ContagionChance = contagionChance;
            apoc.CanDestroyRooms = canDestroyRooms;
            apoc.RoomDamageChance = roomDamageChance;
            apoc.CausesStarvation = causesStarvation;
            apoc.CausesPowerLoss = causesPowerLoss;
            apoc.DurationMonths = durationMonths;
            apoc.IsMultiMonth = durationMonths > 1;

            ApocalypseList.Add(apoc);
        }

        public ApocalypseDefinition GetCurrentApocalypse()
        {
            if (ApocalypseList.Count == 0)
                BuildDefaultApocalypses();

            if (CurrentIndex >= ApocalypseList.Count)
                CurrentIndex = 0;

            return ApocalypseList[CurrentIndex];
        }

        public ApocalypseDefinition GetApocalypseForMonth(int month)
        {
            foreach (var apoc in ApocalypseList)
            {
                if (apoc.Month == month)
                    return apoc;
            }

            return null;
        }

        public void AdvanceToNextApocalypse()
        {
            CurrentIndex = (CurrentIndex + 1) % ApocalypseList.Count;
        }
    }
}
