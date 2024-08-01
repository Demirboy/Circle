using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Canvas menuCanvas;
    public XRRayInteractor rayInteractor;
    public XRNode xrNode = XRNode.RightHand;

    private bool isMenuActive = false;
    private bool wasMenuButtonPressed = false;

    void Start()
    {
        // Initially hide the menu and disable the ray interactor
        menuCanvas.gameObject.SetActive(false);
        rayInteractor.gameObject.SetActive(false);
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(xrNode);

        if (device.TryGetFeatureValue(CommonUsages.menuButton, out bool isMenuPressed))
        {
            if (isMenuPressed && !wasMenuButtonPressed)
            {
                ToggleMenu();
            }
            wasMenuButtonPressed = isMenuPressed;
        }
        else
        {
            // Handle cases where the device does not provide a menu button value
            wasMenuButtonPressed = false;
        }
    }

    void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        menuCanvas.gameObject.SetActive(isMenuActive);
        rayInteractor.gameObject.SetActive(isMenuActive);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
