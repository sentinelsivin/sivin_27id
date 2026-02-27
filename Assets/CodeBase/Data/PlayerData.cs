using System;
using CodeBase.Data.PlayerDataComponents;
using Newtonsoft.Json;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        [JsonProperty("id")]
        private PlayerId _id;

        [JsonProperty("name")]
        private string _name;

        [JsonProperty("score")]
        private int _score;

        public PlayerData()
        {
            _id = new PlayerId(1); // или PlayerId.Unknown, если сделаешь
            _name = string.Empty;
            _score = 0;
        }

        [JsonConstructor]
        public PlayerData(PlayerId id, string name, int score)
        {
            _id = id;
            _name = name ?? string.Empty;
            Score = score; // через валидацию
        }

        public PlayerId Id => _id;

        public string Name
        {
            get => _name;
            set => _name = value ?? string.Empty;
        }

        public int Score
        {
            get => _score;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _score = value;
            }
        }
    }
}