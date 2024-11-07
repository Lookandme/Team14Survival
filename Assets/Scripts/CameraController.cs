using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject maincam;
    public GameObject secondcam;

    public void ChangeCam(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && maincam.activeSelf)
        {
            Thirdcam();
            Debug.Log("secondcam");
        }
        else if (context.phase == InputActionPhase.Started && secondcam.activeSelf)
        {
            Maincam();
            Debug.Log("maincam");
        }
    }
    void Maincam()
    {
        maincam.gameObject.SetActive(true);
        secondcam.gameObject.SetActive(false);
    }

    void Thirdcam()
    {
        maincam.gameObject.SetActive(false);
        secondcam.gameObject.SetActive(true);
    }
}
