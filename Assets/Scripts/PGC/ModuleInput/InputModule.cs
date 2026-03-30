using PGC.Enum;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PGC.ModuleInput
{
    public class InputModule
    {
        PCGInput inputs;
        public PCGInput.SquareActions squareActions;
        public InputModel<int> horizontalMove = new InputModel<int>();
        public InputModel<int> verticalMove = new InputModel<int>();
        public InputModel<int> rotate = new InputModel<int>();
        
        public bool isPressedChange = false;

        public float holdTimer = 0;
        
        

        public InputModule()
        {
            inputs = new PCGInput();
            squareActions = inputs.Square;
            inputs.Enable();
        }

        public void Update(float dt)
        {
            var moveLeftStatus = GetStatus(squareActions.MoveLeft);
            var moveRightStatus = GetStatus(squareActions.MoveRight);
            var moveDownStatus = GetStatus(squareActions.MoveDown);
            var rotateStatus = GetStatus(squareActions.Rotate);
            // 输入状态保持,直到moveSystem消费之后才Rest
            if (moveLeftStatus is InputStatus.Pressed or InputStatus.Held)
            {
                horizontalMove.value = -1;
                horizontalMove.status = moveLeftStatus;
            }
            else if (moveRightStatus is InputStatus.Pressed or InputStatus.Held)
            {
                horizontalMove.value = 1;
                horizontalMove.status = moveRightStatus;
            }
            
            //value 用于保存输入状态，外部根据value是否有值判断是否移动，value只在外部消费之后重置
            // 玩家体感可能只是按一下按钮，但实际一次输入可能保持在十多帧里都是按住状态，而tick根据间隔不同在这十多帧中执行的次数也不同,0.02s间隔大部分情况下是执行了两次。
            // 为防止按一下移动多次,故而设置holdTimer记录按住时间，从第一次移动到第二次移动间隔一定时间才能继续移动
            // 当tick消费一次输入将value置0，这一次消费可能发生在十多帧之间,后续input中输入还是pressed状态则value将会重新置1进行连续移动
            // holdTimer是为了防止这种连续移动而做的延时处理,若根据Action的状态重置则会出现按钮released状态，但value有值，导致再次变成初始移动允许移动
            if(horizontalMove.value == 0)
            {
                holdTimer = 0;
            }


            if (moveDownStatus is InputStatus.Pressed or InputStatus.Held)
            {
                verticalMove.value = -1;
                verticalMove.status = moveDownStatus;
            }

            if (rotateStatus is InputStatus.Pressed)
            {
                rotate.value = 1;
                rotate.status = rotateStatus;
            }

            if (squareActions.Change.WasPressedThisFrame())
            {
                isPressedChange = true;
            }
        }

        static InputStatus GetStatus(InputAction action)
        {
            if (action.WasPressedThisFrame())
            {
                return InputStatus.Pressed;
            }
            if (action.WasReleasedThisFrame())
            {
                return InputStatus.Released;
            }
            if (action.IsPressed())
            {
                return InputStatus.Held;
            }
            return InputStatus.None;
        }

        public void RestInput()
        {
            horizontalMove.value = 0;
            horizontalMove.status = InputStatus.None;
            
            verticalMove.value = 0;
            verticalMove.status = InputStatus.None;
            
            rotate.value = 0;
            rotate.status = InputStatus.None;
            
            isPressedChange = false;
        }

    }
}