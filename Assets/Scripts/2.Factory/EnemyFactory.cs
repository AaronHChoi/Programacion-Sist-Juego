using System;
using System.Collections.Generic;
using UnityEngine;

namespace _2.Factory
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private Enemy[] enemies;
        private static Dictionary<string,Enemy> _enemiesDictionary;

        private void Awake()
        {
            _enemiesDictionary = new Dictionary<string, Enemy>();

            foreach (var enemy in enemies)
            {
                _enemiesDictionary.Add(enemy.Id, enemy);
            }
        }

        public static Enemy CreateEnemy(string id)
        {
            if (!_enemiesDictionary.TryGetValue(id, out Enemy enemy))
            {
                return null;
            }

            return Instantiate(enemy);
        }
    }
}
