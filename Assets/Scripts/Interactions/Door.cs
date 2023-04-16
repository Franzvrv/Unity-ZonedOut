 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected Animator animator;
    public DoorType doorType;
    [SerializeField] protected string keyName;
    public DoorState doorState = DoorState.Close;
    public bool interactable = true;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void LookAtDoor() {
        if (doorState == DoorState.Close) {
            switch (doorType) {
                case DoorType.Locked:
                    Player.Instance.splashText.ShowText("Need " + keyName + " to unlock");
                    break;
                case DoorType.Unlocked:
                    Player.Instance.splashText.ShowText("Open Door");
                    break;
            }
        }
    }

    virtual public void InteractDoor() {
        if (interactable) {
            switch (doorType) {
                case DoorType.Unlocked:
                    if (doorState == DoorState.Close) {
                        OpenDoor();
                    }
                    else {
                        CloseDoor();
                    }
                    break;
                case DoorType.Locked:
                if(Player.Instance.CheckItem(keyName)) {
                    Player.Instance.RemoveItem(keyName);
                    UnlockDoor();
                    Debug.Log("Door has been unlocked");
                }
                break;
            }
        }
        
    }

    virtual public void OpenDoor() {
        AudioManager.Instance.PlaySpatialAudio("DoorOpen", transform.position);
        animator.SetTrigger("Open");
        doorState = DoorState.Open;
    }

    virtual public void UnlockDoor() {
        doorType = DoorType.Unlocked;
        Player.Instance.splashText.ShowText("Unlocked Door");
        OpenDoor();
    }

    virtual public void CloseDoor() {
        AudioManager.Instance.PlaySpatialAudio("DoorOpen", transform.position);
        animator.SetTrigger("Close");
        doorState = DoorState.Close;
    }
}

public enum DoorType {
    Unlocked,
    Locked,
}

public enum DoorState {
    Open,
    Close
}
