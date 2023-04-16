using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public LayerMask interactableLayerMask = 6;
    [SerializeField] private GameObject flashLight, flashLightBar, splashTextObject, walkingObject, runningObject;
    [SerializeField] private Font inventoryFont;
    
    private List<InventoryItem> inventory = new List<InventoryItem>();
    private FirstPersonController controllerScript;
    public int itemSpacing = 25;
    public float maxBatteryCapacity = 200;
    public static float batteryDepleteRate = 1;
    public float batteryCapacity;
    public bool flashlightOn = true;
    private float batteryBarMaxScale = 1f;
    public SplashText splashText;
    private PlayerMovementState previousMovementState = PlayerMovementState.Idle;

    void Awake() {
        splashText = splashTextObject.GetComponent<SplashText>();
        controllerScript = this.GetComponent<FirstPersonController>();
    }

    void Start() {
        if (!Instance) {
            Instance = this;
        }
        batteryCapacity = maxBatteryCapacity;
        splashText.ShowText("Find the exit");
    }

    void Update() {
        //Interaction using raycast
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, 2, interactableLayerMask)) {

            //Interact Key
            if (Input.GetKeyDown(KeyCode.E)) {
                if(hit.collider.GetComponent<Item>()) {
                    InventoryItem item = hit.collider.GetComponent<Item>().InteractItem();
                    //InventoryItem item = hit.collider.GetComponent<InventoryItem>().InteractItem();
                    switch (item.itemType) {
                        case ItemType.Inventory:
                            inventory.Add(item);
                            Debug.Log("Player picked up " + item.name + ", there are now " + inventory.Count + " items in the inventory");
                            break;
                        case ItemType.Battery:
                            ReplenishBattery(item.batteryAmount);
                            break;
                    }
                }
                else if(hit.collider.GetComponent<Interactable>()) {
                    hit.collider.GetComponent<Interactable>().Interact();
                    Debug.Log("Player has interacted");
                }
            }

            else {
                if(hit.collider.GetComponent<Interactable>()) {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    interactable.LookAt();
                }

                // if(hit.collider.GetComponent<Item>()) {
                //     Item item = hit.collider.GetComponent<Item>();
                //     switch (item.itemType) {
                //         case ItemType.Inventory:
                //             splashText.ShowText(item.name);
                //             break;
                //         case ItemType.Battery:
                //             splashText.ShowText("Battery");
                //             break;
                //     }
                // }

                if(hit.collider.GetComponent<Door>()) {
                    Door door = hit.collider.GetComponent<Door>();
                    if (door.doorState == DoorState.Close) {
                        switch (door.doorType) {
                            case DoorType.Locked:
                                splashText.ShowText("Locked Door");
                                break;
                            case DoorType.Unlocked:
                                splashText.ShowText("Open Door");
                                break;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (flashlightOn) {
                TurnFlashlightOff();
            } else {
                if (batteryCapacity > 0) {
                    TurnFlashlightOn();
                }
            }
        }

        if (flashlightOn) {
            batteryCapacity -= batteryDepleteRate * Time.deltaTime;
            if (batteryCapacity <= 0) {
                TurnFlashlightOff();
            }
        }

        FlashLightGUI();
    }

    void FixedUpdate() {
        switch(controllerScript.movementState) {
            case PlayerMovementState.Idle:
                if (previousMovementState != controllerScript.movementState) {
                    AudioManager.Instance.PauseLoopingAudio(walkingObject);
                    AudioManager.Instance.PauseLoopingAudio(runningObject);
                    previousMovementState = controllerScript.movementState;
                }
                break;
            case PlayerMovementState.Walking:
                if (previousMovementState != controllerScript.movementState) {
                    AudioManager.Instance.PlayLoopingAudio(walkingObject);
                    AudioManager.Instance.PauseLoopingAudio(runningObject);
                    previousMovementState = controllerScript.movementState;
                }
                break;
            case PlayerMovementState.sprinting:
                if (previousMovementState != controllerScript.movementState) {
                    AudioManager.Instance.PauseLoopingAudio(walkingObject);
                    AudioManager.Instance.PlayLoopingAudio(runningObject);
                    previousMovementState = controllerScript.movementState;
                }
                break;
        }
    }

    public void FlashLightGUI() {
        flashLightBar.GetComponent<RectTransform>().localScale = new Vector3(batteryBarMaxScale * (batteryCapacity / maxBatteryCapacity),1,1);
    }

    public void RemoveItem(string itemName) {
        for (int i = 0; i < inventory.Count; i++) {
            if(inventory[i].name == itemName) {
                inventory.Remove(inventory[i]);
                Debug.Log("item removed");
                return;
            }
        }
        Debug.Log("removing item doesn't exist");
    }

    public bool CheckItem(string itemName) {
        for (int i = 0; i < inventory.Count; i++) {
            Debug.Log(inventory[i].name);
            if(inventory[i].name == itemName) {
                Debug.Log("item exists");
                return true;
            }
        }
        Debug.Log("checked item doesn't exist");
        return false;
    }

    public void ReplenishBattery(float amount) {
        if (amount + batteryCapacity > maxBatteryCapacity) {
            batteryCapacity = maxBatteryCapacity;
        }
        else {
            batteryCapacity += amount;
        }
        Debug.Log("Battery has been replenished");
    }

    public void TurnFlashlightOn() {
        AudioManager.Instance.PlayAudio("Click");
        Light[] lights = flashLight.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++) {
            lights[i].enabled = true;
        }
        flashlightOn = true;
    }

    public void TurnFlashlightOff() {
        AudioManager.Instance.PlayAudio("Clack");
        Light[] lights = flashLight.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++) {
            lights[i].enabled = false;
        }
        flashlightOn = false;
    }

    private void OnGUI()
    {
        var style = new GUIStyle();
        style.font = inventoryFont;
        style.normal.textColor = Color.white;
        for (int i = 0; i < inventory.Count; i++)
        {
            GUI.Label(new Rect(50, 10 + (itemSpacing * i), 200, 32), inventory[i].name, style);
        }
    }
}

public enum PlayerMovementState {
    Idle,
    Walking,
    sprinting
}
