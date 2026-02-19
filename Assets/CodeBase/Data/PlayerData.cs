using System;
using CodeBase.Data.PlayerDataComponents;
using Newtonsoft.Json;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerData
    {
        private PlayerId _id;
        private string _name;
        private int _score;
        // public Player Skin { get; set; } это префаб облика ака кота, но нужно иметь цельную дату из скинов, чтобы не иметь ссылку на скин, а на его id условно

        public PlayerData()
        {
            //_id = PlayerId.Unknown;
            _name = string.Empty;
            _score = 0;
        }

        [JsonConstructor]
        public PlayerData(PlayerId id, string name, int score)
        {
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