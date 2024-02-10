using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShow : MonoBehaviour
{
    public static bool IsShown;

    private void Start()
    {
        if (!IsShown)
        {
            Time.timeScale = .000000003f;
            IsShown = true;
        }
    }

    private void Update()
    {
        if (IsShown)
            return;

        if (Input.anyKey || Input.touchCount > 0)
        {
            Time.timeScale = 1;

            gameObject.SetActive(false);
        }
    }

}
