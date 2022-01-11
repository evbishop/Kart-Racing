using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayer : Car
{
    void LateUpdate()
    {
        PlayerMove();
        base.LateUpdate();
    }

    void PlayerMove()
    {
        float speedInput = Input.GetAxis("Vertical");
        ProcessPlayerSpeedInput(speedInput);

        TurnInput = Input.GetAxis("Horizontal");
        if (GroundChecker.OnGround)
            ProcessPlayerTurnInput(speedInput);
    }

    void ProcessPlayerSpeedInput(float speedInput)
    {
        if (!GroundChecker.OnGround)
            State = CarState.OffGround;
        else
        {
            if (speedInput > 0)
            {
                Speed = speedInput * ForwardAccel * 1000f;
                if (GroundChecker.OnGround)
                    State = CarState.OnGroundAndMovingForward;
            }
            else if (speedInput < 0)
            {
                Speed = speedInput * ReverseAccel * 1000f;
                if (GroundChecker.OnGround)
                    State = CarState.OnGroundAndMovingBackward;
            }
            else State = CarState.OnGroundAndNotMoving;
        }
    }

    void ProcessPlayerTurnInput(float speedInput)
    {
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles + new Vector3(
                0f,
                TurnInput * TurnStrength * Time.deltaTime * speedInput,
                0f));
    }
}
