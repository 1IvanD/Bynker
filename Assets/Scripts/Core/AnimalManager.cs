using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bynker.Core
{
    [Serializable]
    public class AnimalInfo
    {
        public AnimalType Type;
        public int Count;
        public float Hunger;
        public float Health;
        public bool IsInfected;

        public AnimalInfo(AnimalType type, int count, bool infected = false)
        {
            Type = type;
            Count = count;
            Hunger = 50f;
            Health = 100f;
            IsInfected = infected;
        }
    }

    public class AnimalManager : MonoBehaviour
    {
        public List<AnimalInfo> Animals = new List<AnimalInfo>();

        public void InitializeDefaultAnimals()
        {
            Animals.Clear();
            Animals.Add(new AnimalInfo(AnimalType.Chicken, 10, false));
            Animals.Add(new AnimalInfo(AnimalType.Cow, 4, false));
            Animals.Add(new AnimalInfo(AnimalType.Pig, 5, false));
            Animals.Add(new AnimalInfo(AnimalType.Sheep, 6, false));
            Animals.Add(new AnimalInfo(AnimalType.Rabbit, 8, false));
        }

        public void AddAnimal(AnimalType type, int amount)
        {
            var animal = Animals.Find(a => a.Type == type);
            if (animal == null)
            {
                Animals.Add(new AnimalInfo(type, amount));
                return;
            }

            animal.Count += amount;
        }

        public void FeedAnimals(float amount)
        {
            foreach (var animal in Animals)
            {
                animal.Hunger = Mathf.Clamp(animal.Hunger - amount, 0f, 100f);
                animal.Health = Mathf.Clamp(animal.Health + amount * 0.1f, 0f, 100f);
            }
        }

        public void UpdateAnimalsMonthly()
        {
            foreach (var animal in Animals)
            {
                if (animal.IsInfected)
                {
                    animal.Health -= 12f;
                    animal.Count = Mathf.Max(0, animal.Count - 1);
                    continue;
                }

                animal.Hunger = Mathf.Clamp(animal.Hunger + 10f, 0f, 100f);
                animal.Health = Mathf.Clamp(animal.Health - (animal.Hunger > 60f ? 5f : 0f), 0f, 100f);

                if (animal.Hunger < 45f)
                    animal.Count = Mathf.Max(0, animal.Count - 1);
            }
        }

        public int GetTotalAnimalCount()
        {
            int total = 0;
            foreach (var animal in Animals)
                total += animal.Count;
            return total;
        }
    }
}
