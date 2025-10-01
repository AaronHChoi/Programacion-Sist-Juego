using UnityEngine;

namespace Command
{
    public class CommandMovement : MonoBehaviour
    {
        public int _distance = 10;
        public void Move(Vector3 direction)
        {
            transform.Translate(direction*_distance);
        }
    }
}
