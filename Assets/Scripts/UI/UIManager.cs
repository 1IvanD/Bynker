using UnityEngine;
using Bynker.Building;
using Bynker.Coop;
using Bynker.Core;

namespace Bynker.UI
{
    public class UIManager : MonoBehaviour
    {
        public BunkerState bunkerState;
        public GameManager gameManager;
        public CoopPlayerController coopPlayerController;
        public UpgradeManager upgradeManager;

        private string statusText = "Bunker status: stable";

        private void Start()
        {
            if (bunkerState == null)
                bunkerState = FindObjectOfType<BunkerState>();

            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();

            if (coopPlayerController == null)
                coopPlayerController = FindObjectOfType<CoopPlayerController>();

            if (upgradeManager == null)
                upgradeManager = FindObjectOfType<UpgradeManager>();
        }

        private void Update()
        {
            if (bunkerState == null)
                return;

            if (bunkerState.IsDestroyed)
                statusText = "Bunker collapse";
            else if (bunkerState.Resources.Morale < 30f)
                statusText = "Morale critical";
            else if (bunkerState.Resources.Oxygen < 40f)
                statusText = "Oxygen low";
            else
                statusText = "Bunker status: stable";
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(20, 20, 500, 500));
            GUILayout.Label("Bynker - Bunker Survival");
            GUILayout.Label(statusText);

            if (bunkerState != null)
            {
                GUILayout.Label($"Month: {bunkerState.CurrentMonth} / Year: {bunkerState.CurrentYear}");
                GUILayout.Label($"Food: {bunkerState.Resources.Food:F0}");
                GUILayout.Label($"Water: {bunkerState.Resources.Water:F0}");
                GUILayout.Label($"Oxygen: {bunkerState.Resources.Oxygen:F0}");
                GUILayout.Label($"Power: {bunkerState.Resources.Power:F0}");
                GUILayout.Label($"Materials: {bunkerState.Resources.Materials:F0}");
                GUILayout.Label($"Morale: {bunkerState.Resources.Morale:F0}");
                GUILayout.Label($"Population: {bunkerState.Resources.Population}");
            }

            if (GUILayout.Button("Next month"))
            {
                if (gameManager != null)
                    gameManager.TriggerMonthlyCycle();
            }

            if (upgradeManager != null)
            {
                GUILayout.Label("Rooms:");
                foreach (var room in upgradeManager.GetRooms())
                {
                    GUILayout.Label($"{room.RoomName} L{room.Level}/{room.MaxLevel}");
                    if (GUILayout.Button($"Upgrade {room.RoomName}"))
                    {
                        upgradeManager.TryUpgradeRoom(room.RoomName);
                    }
                }
            }

            if (coopPlayerController != null)
            {
                GUILayout.Label("Players:");
                foreach (var p in coopPlayerController.GetAllPlayers())
                {
                    GUILayout.Label($"{p.PlayerName} - {p.AssignedProfession} - AP: {p.ActionPoints:F0}");
                }
            }

            GUILayout.EndArea();
        }
    }
}
