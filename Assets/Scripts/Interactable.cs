using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected UnityEvent onInteract;
    [SerializeField] protected UnityEvent onLook;
    [SerializeField] protected string onLookSplashText;
    
    //Destroys this gameobject
    virtual public string Interact() {
        onInteract.Invoke();
        return null;
    }

    virtual public void LookAt() {
        if (onLookSplashText != "") {
            Player.Instance.splashText.ShowText(onLookSplashText);
        }
        onLook.Invoke();
    }
}
