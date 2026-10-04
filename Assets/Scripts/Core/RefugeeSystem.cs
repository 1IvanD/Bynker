using System.Collections.Generic;
using UnityEngine;

namespace Bynker.Core
{
    public enum RefugeeDecision
    {
        Accept,
        Refuse,
        Trade
    }

    [System.Serializable]
    public class RefugeeEncounter
    {
        public string Name;
        public int PeopleCount;
        public ProfessionType Profession;
        public string Story;
        public int FoodDemand;
        public int WaterDemand;
        public int MedicineDemand;
        public int SecurityRisk;

        public RefugeeEncounter(string name, int peopleCount, ProfessionType profession, string story,
            int foodDemand, int waterDemand, int medicineDemand, int securityRisk)
        {
            Name = name;
            PeopleCount = peopleCount;
            Profession = profession;
            Story = story;
            FoodDemand = foodDemand;
            WaterDemand = waterDemand;
            MedicineDemand = medicineDemand;
            SecurityRisk = securityRisk;
        }
    }

    public class RefugeeSystem : MonoBehaviour
    {
        public List<RefugeeEncounter> PossibleEncounters = new List<RefugeeEncounter>();

        private void Awake()
        {
            BuildEncounters();
        }

        public RefugeeEncounter RollEncounter()
        {
            if (PossibleEncounters.Count == 0)
                BuildEncounters();

            int index = Random.Range(0, PossibleEncounters.Count);
            return PossibleEncounters[index];
        }

        public void ResolveEncounter(BunkerState bunkerState, RefugeeEncounter encounter, RefugeeDecision decision)
        {
            if (bunkerState == null || encounter == null)
                return;

            switch (decision)
            {
                case RefugeeDecision.Accept:
                    bunkerState.Resources.Population += encounter.PeopleCount;
                    bunkerState.Resources.Morale += 8f;
                    bunkerState.Resources.Food -= encounter.FoodDemand;
                    bunkerState.Resources.Water -= encounter.WaterDemand;
                    bunkerState.Resources.Medicine -= encounter.MedicineDemand;
                    Debug.Log($"Accepted {encounter.Name}: +{encounter.PeopleCount} survivors.");
                    break;

                case RefugeeDecision.Refuse:
                    bunkerState.Resources.Morale -= 5f;
                    Debug.Log($"Refused {encounter.Name}. Morale dropped.");
                    break;

                case RefugeeDecision.Trade:
                    bunkerState.Resources.Food -= encounter.FoodDemand / 2;
                    bunkerState.Resources.Water -= encounter.WaterDemand / 2;
                    bunkerState.Resources.Materials -= 5;
                    bunkerState.Resources.Population += encounter.PeopleCount / 2;
                    bunkerState.Resources.Morale += 3f;
                    Debug.Log($"Traded with {encounter.Name}.");
                    break;
            }

            bunkerState.Resources.Morale = Mathf.Clamp(bunkerState.Resources.Morale, 0f, 100f);
        }

        private void BuildEncounters()
        {
            PossibleEncounters.Clear();
            PossibleEncounters.Add(new RefugeeEncounter("Wandering family", 3, ProfessionType.Worker,
                "A family asks for shelter and food.", 12, 10, 3, 1));
            PossibleEncounters.Add(new RefugeeEncounter("Mechanic team", 2, ProfessionType.Engineer,
                "A small engineering crew begs to enter.", 8, 6, 2, 2));
            PossibleEncounters.Add(new RefugeeEncounter("Medics", 2, ProfessionType.Doctor,
                "Doctors ask for protection and medicine.", 6, 6, 10, 1));
            PossibleEncounters.Add(new RefugeeEncounter("Soldiers", 4, ProfessionType.Soldier,
                "Armed survivors want to share defense duty.", 14, 9, 4, 4));
            PossibleEncounters.Add(new RefugeeEncounter("Farmers", 3, ProfessionType.Farmer,
                "A group with agricultural knowledge wants a place to stay.", 10, 12, 2, 1));
        }
    }
}
