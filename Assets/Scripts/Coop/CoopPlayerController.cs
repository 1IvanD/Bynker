using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bynker.Coop
{
    [Serializable]
    public class PlayerRole
    {
        public int PlayerId;
        public string PlayerName;
        public ProfessionType AssignedProfession;
        public float ActionPoints = 100f;
        public float MaxActionPoints = 100f;
        public bool IsActive = true;
        public float ContributionScore = 0f;

        public PlayerRole(int id, string name, ProfessionType profession)
        {
            PlayerId = id;
            PlayerName = name;
            AssignedProfession = profession;
            ActionPoints = MaxActionPoints;
        }
    }

    public class CoopPlayerController : MonoBehaviour
    {
        [SerializeField] private List<PlayerRole> Players = new List<PlayerRole>();
        [SerializeField] private int CurrentPlayerIndex = 0;
        [SerializeField] private BunkerState SharedBunkerState;

        [Header("Action System")]
        [SerializeField] private float ActionPointsPerBuildAction = 20f;
        [SerializeField] private float ActionPointsPerRepairAction = 15f;
        [SerializeField] private float ActionPointsPerResourceAction = 10f;
        [SerializeField] private float ActionPointRegenPerTurn = 30f;

        public event Action<int> OnPlayerTurnChanged;
        public event Action<int> OnPlayerActionPointsChanged;
        public event Action<PlayerRole> OnPlayerDamaged;

        private void Start()
        {
            if (SharedBunkerState == null)
                SharedBunkerState = FindObjectOfType<BunkerState>();
        }

        /// <summary>
        /// Инициализация игроков в кооперативной сессии
        /// </summary>
        public void InitializeCoopPlayers(List<PlayerRole> playerList)
        {
            Players.Clear();
            Players.AddRange(playerList);
            CurrentPlayerIndex = 0;

            Debug.Log($"Initialized {Players.Count} players for coop gameplay");
        }

        /// <summary>
        /// Добавить одного игрока в сессию
        /// </summary>
        public void AddPlayer(string playerName, ProfessionType profession)
        {
            if (Players.Count >= 4)
            {
                Debug.LogWarning("Maximum 4 players allowed in coop");
                return;
            }

            var newPlayer = new PlayerRole(Players.Count, playerName, profession);
            Players.Add(newPlayer);
            Debug.Log($"Player {playerName} ({profession}) joined the bunker");
        }

        /// <summary>
        /// Получить текущего активного игрока
        /// </summary>
        public PlayerRole GetCurrentPlayer()
        {
            if (Players.Count == 0)
                return null;
            return Players[CurrentPlayerIndex];
        }

        /// <summary>
        /// Переключиться на следующего игрока
        /// </summary>
        public void SwitchToNextPlayer()
        {
            if (Players.Count == 0)
                return;

            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            RegenerateActionPoints();

            Debug.Log($"Switched to {GetCurrentPlayer().PlayerName}");
            OnPlayerTurnChanged?.Invoke(CurrentPlayerIndex);
        }

        /// <summary>
        /// Потратить очки действия на строительство
        /// </summary>
        public bool SpendActionPointsForBuild(int buildCost)
        {
            var currentPlayer = GetCurrentPlayer();
            if (currentPlayer == null)
                return false;

            if (currentPlayer.ActionPoints >= buildCost)
            {
                currentPlayer.ActionPoints -= buildCost;
                currentPlayer.ContributionScore += buildCost * 1.5f;
                OnPlayerActionPointsChanged?.Invoke(CurrentPlayerIndex);
                return true;
            }

            Debug.LogWarning($"{currentPlayer.PlayerName} doesn't have enough action points");
            return false;
        }

        /// <summary>
        /// Потратить очки действия на ремонт
        /// </summary>
        public bool SpendActionPointsForRepair(int repairCost)
        {
            var currentPlayer = GetCurrentPlayer();
            if (currentPlayer == null)
                return false;

            if (currentPlayer.ActionPoints >= repairCost)
            {
                currentPlayer.ActionPoints -= repairCost;
                currentPlayer.ContributionScore += repairCost * 1.2f;
                OnPlayerActionPointsChanged?.Invoke(CurrentPlayerIndex);
                return true;
            }

            Debug.LogWarning($"{currentPlayer.PlayerName} doesn't have enough action points");
            return false;
        }

        /// <summary>
        /// Потратить очки действия на работу с ресурсами
        /// </summary>
        public bool SpendActionPointsForResource(int resourceCost)
        {
            var currentPlayer = GetCurrentPlayer();
            if (currentPlayer == null)
                return false;

            if (currentPlayer.ActionPoints >= resourceCost)
            {
                currentPlayer.ActionPoints -= resourceCost;
                currentPlayer.ContributionScore += resourceCost * 0.8f;
                OnPlayerActionPointsChanged?.Invoke(CurrentPlayerIndex);
                return true;
            }

            Debug.LogWarning($"{currentPlayer.PlayerName} doesn't have enough action points");
            return false;
        }

        /// <summary>
        /// Восстановить очки действия в начале хода
        /// </summary>
        public void RegenerateActionPoints()
        {
            var currentPlayer = GetCurrentPlayer();
            if (currentPlayer != null)
            {
                currentPlayer.ActionPoints = Mathf.Min(
                    currentPlayer.ActionPoints + ActionPointRegenPerTurn,
                    currentPlayer.MaxActionPoints
                );
            }
        }

        /// <summary>
        /// Нанести урон игроку (при катастрофе, атаке и т.д.)
        /// </summary>
        public void DamagePlayer(int playerId, float damageAmount)
        {
            if (playerId >= 0 && playerId < Players.Count)
            {
                var player = Players[playerId];
                player.ActionPoints -= damageAmount;
                player.ActionPoints = Mathf.Max(0, player.ActionPoints);

                OnPlayerDamaged?.Invoke(player);
                Debug.Log($"Player {player.PlayerName} took {damageAmount} damage");
            }
        }

        /// <summary>
        /// Получить бонус профессии для текущего игрока
        /// </summary>
        public float GetProfessionBonus(ProfessionType profession, string bonusType)
        {
            var currentPlayer = GetCurrentPlayer();
            if (currentPlayer == null || currentPlayer.AssignedProfession != profession)
                return 0f;

            switch (profession)
            {
                case ProfessionType.Engineer:
                    return bonusType == "power" ? 1.3f : 1f;

                case ProfessionType.Farmer:
                    return (bonusType == "food" || bonusType == "water") ? 1.35f : 1f;

                case ProfessionType.Doctor:
                    return (bonusType == "health" || bonusType == "morale") ? 1.25f : 1f;

                case ProfessionType.Scientist:
                    return (bonusType == "resistance" || bonusType == "research") ? 1.3f : 1f;

                case ProfessionType.Soldier:
                    return (bonusType == "defense" || bonusType == "risk") ? 1.4f : 1f;

                case ProfessionType.Worker:
                    return 1.1f;

                default:
                    return 1f;
            }
        }

        /// <summary>
        /// Получить все игроков
        /// </summary>
        public List<PlayerRole> GetAllPlayers()
        {
            return new List<PlayerRole>(Players);
        }

        /// <summary>
        /// Получить количество активных игроков
        /// </summary>
        public int GetPlayerCount()
        {
            return Players.Count;
        }

        /// <summary>
        /// Получить рейтинг вклада каждого игрока
        /// </summary>
        public Dictionary<string, float> GetContributionScores()
        {
            var scores = new Dictionary<string, float>();
            foreach (var player in Players)
            {
                scores[player.PlayerName] = player.ContributionScore;
            }
            return scores;
        }

        /// <summary>
        /// Завершить месячный цикл и пересчитать вклады
        /// </summary>
        public void EndMonthCycle()
        {
            foreach (var player in Players)
            {
                // Бонус за выживание месяца
                player.ContributionScore += 50f;

                // Штраф за бездействие
                if (player.ContributionScore < 50f)
                {
                    player.ContributionScore += 25f;  // минимум поощрение
                }
            }

            Debug.Log("Month cycle completed. Scores recalculated.");
        }
    }
}
