using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    private float _mx = 0;

    void Update()
    {
        /*
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }

        if (!CameraManager.Focus)
        {
            return;
        }
        */

        float mouseX = Input.GetAxis("Mouse X");

        _mx += mouseX * _owner.Stat.RotationSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0f, _mx, 0);
    }
    public void ResetX()
    {
        _mx = 0;
    }
}