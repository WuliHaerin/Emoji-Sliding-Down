using UnityEngine;
using System.Collections;


public class InputController : MonoBehaviour
{
    public void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (GameManager.Instance.GameState == GameState.Playing)
        //    {
        //        GameManager.Instance.rotationDirection = GameManager.Instance.rotationDirection == 0 ? GameManager.Instance.firstRotationDirection : -GameManager.Instance.rotationDirection;
        //    }
                
        //}
        if (GameManager.Instance.GameState == GameState.Playing && Input.GetMouseButtonDown(0))
        {
            if(GameManager.Instance.rotationDirection == 0)
            {
                if (Input.mousePosition.x < Screen.width / 2)
                {
                    GameManager.Instance.rotationDirection = 1;
                }
                else
                {
                    GameManager.Instance.rotationDirection = -1;
                }
            }
            else
            {
                GameManager.Instance.rotationDirection = -GameManager.Instance.rotationDirection;
            }
        }
    }

    public void OnEnable()
    {
        GameManager.GameStateChanged += OnGameStateChanged;
    }

    public void OnDisable()
    {
        GameManager.GameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState, GameState oldState)
    {
        if (oldState == GameState.Prepare && newState == GameState.Playing)
        {
            GameManager.Instance.rotationDirection = 0;
        }
    }
}
