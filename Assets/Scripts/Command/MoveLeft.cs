using UnityEngine;

namespace Command
{
    public class MoveLeft: ICommand
    {
        private CommandMovement _player;
        private Vector3 _previousPosition;
        public MoveLeft(CommandMovement player)
        {
            this._player = player;
        }

        public void Execute()
        {
            _previousPosition = _player.transform.position;
            _player.Move(Vector3.left);
        }
        
        public void Undo()
        {
            _player.transform.position = _previousPosition;
        }
    }
}