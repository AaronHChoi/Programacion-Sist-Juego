using UnityEngine;

namespace _2.Factory
{
    public class Instantiator : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                EnemyFactory.CreateEnemy("Zombie");
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                EnemyFactory.CreateEnemy("Dragon");
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                EnemyFactory.CreateEnemy("Golem");
            }
        
        }
    }
}