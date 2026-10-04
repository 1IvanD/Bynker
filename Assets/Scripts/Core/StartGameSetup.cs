using System.Collections.Generic;
using UnityEngine;
using Bynker.Coop;
using Bynker.Core;

namespace Bynker.Setup
{
    public class StartGameSetup : MonoBehaviour
    {
        public BunkerState bunkerState;
        public GameManager gameManager;
        public CoopPlayerController coopPlayerController;
        public DifficultyLevel selectedDifficulty = DifficultyLevel.Normal;

        public List<ProfessionType> selectedProfessions = new List<ProfessionType>
        {
            ProfessionType.Engineer,
            ProfessionType.Farmer,
            ProfessionType.Doctor,
            ProfessionType.Soldier
        };

        private void Start()
        {
            if (bunkerState == null)
                bunkerState = FindObjectOfType<BunkerState>();

            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();

            if (coopPlayerController == null)
                coopPlayerController = FindObjectOfType<CoopPlayerController>();

            InitializeGame();
        }

        public void InitializeGame()
        {
            if (gameManager != null)
                gameManager.SetDifficulty(selectedDifficulty);

            if (coopPlayerController != null)
            {
                var players = new List<PlayerRole>();
                for (int i = 0; i < selectedProfessions.Count; i++)
                {
                    var name = "Player " + (i + 1);
                    players.Add(new PlayerRole(i, name, selectedProfessions[i]));
                }

                coopPlayerController.InitializeCoopPlayers(players);
            }

            if (bunkerState != null)
            {
                var settings = DifficultySettings.FromLevel(selectedDifficulty);
                bunkerState.Resources.Food *= settings.InitialResourcesMultiplier;
                bunkerState.Resources.Water *= settings.InitialResourcesMultiplier;
                bunkerState.Resources.Oxygen *= settings.InitialResourcesMultiplier;
                bunkerState.Resources.Power *= settings.InitialResourcesMultiplier;
            }

            Debug.Log("Game initialized with difficulty: " + selectedDifficulty);
        }
    }
}
