using UnityEngine;

namespace Command
{
    public class MoveRight: ICommand
    {
        private CommandMovement _player;
        private Vector3 _previousPosition;
        public MoveRight(CommandMovement player)
        {
            this._player = player;
        }

        public void Execute()
        {
            _previousPosition = _player.transform.position;
            _player.Move(Vector3.right);
        }
        
        public void Undo()
        {
            _player.transform.position = _previousPosition;
        }
    }
}