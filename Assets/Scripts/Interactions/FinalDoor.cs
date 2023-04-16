 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalDoor : Door
{
    public GameObject youEscaped;
    public FirstPersonController firstPersonControllerScript;
    public Player playerScript;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    override public void InteractDoor() {
        if (interactable) {
            switch (doorType) {
                case DoorType.Unlocked:
                    if (doorState == DoorState.Close) {
                        OpenDoor();
                        Escape();
                    }
                    else {
                        CloseDoor();
                    }
                    break;
                case DoorType.Locked:
                if(Player.Instance.CheckItem(keyName)) {
                    Player.Instance.RemoveItem(keyName);
                    }
                    UnlockDoor();
                    Debug.Log("Door has been unlocked");

                    break;
            }
        }
        
    }

    override public void OpenDoor() {
        AudioManager.Instance.PlaySpatialAudio("DoorOpen", transform.position);
        animator.SetTrigger("Open");
        doorState = DoorState.Open;
        StartCoroutine(Escape());
    }

    override public void UnlockDoor() {
        doorType = DoorType.Unlocked;
        OpenDoor();
    }

    override public void CloseDoor() {
        AudioManager.Instance.PlaySpatialAudio("DoorOpen", transform.position);
        animator.SetTrigger("Close");
        doorState = DoorState.Close;
    }
    public IEnumerator Escape()
    {
        firstPersonControllerScript.enabled = false;
        playerScript.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(1f);
        youEscaped.SetActive(true);
    }

}
