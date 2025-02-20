﻿using UnityEngine;
using System.Collections;

public class DeathPlane : MonoBehaviour
{
    public float moveDownDistance;

    public void MoveDown()
    {
        transform.Translate(Vector3.down * moveDownDistance);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if(!GameManager.Instance.playerController.isCancelAd)
            {
                GameManager.Instance.playerController.PreDie();
            }
            else
            {
                GameManager.Instance.playerController.DieImmediately();
            }
        }
    }
}
