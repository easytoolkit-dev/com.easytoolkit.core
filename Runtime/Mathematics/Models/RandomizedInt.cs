using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EasyToolkit.Core.Mathematics
{
    [Serializable]
    public class RandomizedInt
    {
        [SerializeField] private RandomizationMode _mode;
        [SerializeField] private int _baseValue;
        [SerializeField] private float _variance;

        public RandomizationMode Mode
        {
            get => _mode;
            set => _mode = value;
        }

        public int BaseValue
        {
            get => _baseValue;
            set => _baseValue = value;
        }

        public float Variance
        {
            get => _variance;
            set => _variance = value;
        }

        public RandomizedInt()
        {
        }

        public RandomizedInt(RandomizationMode mode, int baseValue, float variance)
        {
            _mode = mode;
            _baseValue = baseValue;
            _variance = variance;
        }

        public int Evaluate()
        {
            var variance = Random.Range(-_variance, _variance);

            return _mode switch
            {
                RandomizationMode.None => _baseValue,
                RandomizationMode.Absolute => _baseValue + Mathf.RoundToInt(variance),
                RandomizationMode.Percent => Mathf.RoundToInt(_baseValue * (1f + variance)),
                _ => throw new ArgumentOutOfRangeException(nameof(_mode), _mode, null)
            };
        }
    }
}
