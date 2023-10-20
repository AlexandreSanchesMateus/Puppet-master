using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class timer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI _textTimer;
    [SerializeField] private float _RemainingTime;
    private bool hasload;
    void Start()
    {
        _textTimer.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _textTimer.text = ((int)_RemainingTime).ToString();
        _RemainingTime -= Time.deltaTime;
        if (_RemainingTime < 0&& !hasload)
        {
            hasload = true;
            SceneLoader.LoadMenuScene();
            
        }
    }
}
