using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProxy : MonoBehaviour
{
    private bool pressed = false;

    private void Update()
    {
        if (Input.anyKeyDown && !pressed)
        {
            pressed = true;
            SceneLoader.LoadGameScene();
        }
    }

    public void LoadGameScene() => SceneLoader.LoadGameScene();
    public void LoadMenuScene() => SceneLoader.LoadMenuScene();
    public void QuitGame() => SceneLoader.QuitGame();
}
