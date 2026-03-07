using System;
using UnityEngine;
using UnityEngine.InputSystem;
using PGC.Module_Input;

namespace PGC {

    public class InputModule {

        PGCInput inputs;

        public InputModel<float> moveHorizontalInput;
        public InputModel<float> moveDownInput;
        public InputModel<float> transformInput;

        public InputModule() {
            inputs = new PGCInput();

            moveHorizontalInput = new InputModel<float>();
        }

        public void Tick(float dt) {
            var world = inputs.World;
            // Horizontal Movement
            var moveLeftStatus = GetStatus(world.MoveLeft);
            var moveRightStatus = GetStatus(world.MoveRight);

            moveHorizontalInput.status = moveLeftStatus == InputStatus.Held ? InputStatus.Held : moveRightStatus;
            float x = moveLeftStatus == InputStatus.Held ? -1 : moveRightStatus == InputStatus.Held ? 1 : 0;
            moveHorizontalInput.value = x;

            // Down Movement
            var moveDownStatus = GetStatus(world.MoveDown);
            moveDownInput.status = moveDownStatus;
            moveDownInput.value = moveDownStatus == InputStatus.Held || moveDownStatus == InputStatus.Pressed ? 1 : 0;

            // Transform
            var transformStatus = GetStatus(world.MoveUp);
            transformInput.status = transformStatus;
            transformInput.value = transformStatus == InputStatus.Pressed ? 1 : 0;
        }

        static InputStatus GetStatus(InputAction action) {
            if (action.WasPressedThisFrame()) {
                return InputStatus.Pressed;
            } else if (action.IsPressed()) {
                return InputStatus.Held;
            } else if (action.WasReleasedThisFrame()) {
                return InputStatus.Released;
            } else {
                return InputStatus.None;
            }
        }

    }

}