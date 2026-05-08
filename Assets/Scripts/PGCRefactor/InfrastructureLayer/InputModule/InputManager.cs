using PGCRefactor.AssetsLoadModule.InputModule.Enum;
using PGCRefactor.AssetsLoadModule.InputModule.Interface;
using PGCRefactor.AssetsLoadModule.InputModule.TM;
using UnityEngine.InputSystem;

namespace PGCRefactor.AssetsLoadModule.InputModule
{
    public class InputManager : IInputProvider
    {
        private readonly PCGInput               _input;
        private readonly PCGInput.SquareActions _square;

        private InputModel<int> _horizontalMove;
        private InputModel<int> _verticalMove;
        private InputModel<int> _rotate;
        private bool            _isChange;
        private bool            _isHold;

        public InputModel<int> HorizontalMove => _horizontalMove;
        public InputModel<int> VerticalMove   => _verticalMove;
        public InputModel<int> Rotate         => _rotate;
        public bool            IsChange       => _isChange;
        public bool            IsHold       => _isHold;

        public InputManager()
        {
            _input  = new PCGInput();
            _square = _input.Square;
            _input.Enable();
        }

        public void Update(float dt)
        {
            var left   = GetStatus(_square.MoveLeft);
            var right  = GetStatus(_square.MoveRight);
            var down   = GetStatus(_square.MoveDown);
            var rotate = GetStatus(_square.Rotate);

            if (left is InputStatus.Pressed or InputStatus.Held)
            {
                _horizontalMove.value  = -1;
                _horizontalMove.status = left;
            }
            else if (right is InputStatus.Pressed or InputStatus.Held)
            {
                _horizontalMove.value  = 1;
                _horizontalMove.status = right;
            }

            if (down is InputStatus.Pressed or InputStatus.Held)
            {
                _verticalMove.value  = -1;
                _verticalMove.status = down;
            }

            if (rotate == InputStatus.Pressed)
            {
                _rotate.value  = 1;
                _rotate.status = rotate;
            }

            if (_square.Change.WasPressedThisFrame())
                _isChange = true;
            
            if (_square.Hold.WasPressedThisFrame())
                _isHold = true;
        }

        public void Consume()
        {
            _horizontalMove = default;
            _verticalMove   = default;
            _rotate         = default;
            _isChange       = false;
            _isHold         = false;
        }

        private static InputStatus GetStatus(InputAction action)
        {
            if (action.WasPressedThisFrame())  return InputStatus.Pressed;
            if (action.WasReleasedThisFrame()) return InputStatus.Released;
            if (action.IsPressed())            return InputStatus.Held;
            return InputStatus.None;
        }
    }
}
