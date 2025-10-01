using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Command
{
    public class InputHandler : MonoBehaviour
    {
        public CommandMovement player;
        List<ICommand> _recordedCommands = new List<ICommand>();
        private Stack<ICommand> _commandHistory = new Stack<ICommand>();
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ICommand moveForward = new MoveForward(player);
                moveForward.Execute();
                _recordedCommands.Add(moveForward);
                _commandHistory.Push(moveForward);

            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ICommand moveBackwards = new MoveBackwards(player);
                moveBackwards.Execute();
                _recordedCommands.Add(moveBackwards);
                _commandHistory.Push(moveBackwards);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ICommand moveLeft = new MoveLeft(player);
                moveLeft.Execute();
                _recordedCommands.Add(moveLeft);
                _commandHistory.Push(moveLeft);
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ICommand moveRight = new MoveRight(player);
                moveRight.Execute();
                _recordedCommands.Add(moveRight);
                _commandHistory.Push(moveRight);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(ReplayActions());
            }

            if (Input.GetKeyDown(KeyCode.E) && _commandHistory.Count > 0)
            {
                ICommand lastCommand = _commandHistory.Pop();
                lastCommand.Undo();
            }
        }

        private IEnumerator ReplayActions()
        {
            foreach (var command in _recordedCommands)
            {
                command.Execute();
                yield return new WaitForSeconds(0.5f);
            }

            _recordedCommands.Clear();
        }
    }
}
