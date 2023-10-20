using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProxy : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneLoader.LoadGameScene();
        }
    }

    public void LoadGameScene() => SceneLoader.LoadGameScene();
    public void LoadMenuScene() => SceneLoader.LoadMenuScene();
    public void QuitGame() => SceneLoader.QuitGame();
}
