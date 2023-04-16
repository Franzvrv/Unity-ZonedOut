using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public ItemType itemType;
    [Header("Inventory")]
    [SerializeField] private string itemName;
    [Header("Battery")]
    [SerializeField] private float batteryAmount;
    public InventoryItem InteractItem() {
        onInteract.Invoke();
        InventoryItem item = new InventoryItem();

        item.itemType = this.itemType;
        item.sprite = this.GetComponent<SpriteRenderer>();
        item.name = this.itemName;
        item.batteryAmount = this.batteryAmount;
        Destroy(this.gameObject);
        Debug.Log(item.name);
        return item;
    }

    override public void LookAt() {
        if (itemType == ItemType.Inventory) {
            Player.Instance.splashText.ShowText(itemName);
        }
        else if (itemType == ItemType.Battery) {
            Player.Instance.splashText.ShowText("Flashlight battery");
        }
        else {
            Player.Instance.splashText.ShowText(onLookSplashText);
        }
        onLook.Invoke();
    }
}

public class InventoryItem {
    public ItemType itemType;
    public SpriteRenderer sprite;
    public string name;
    public float batteryAmount;
}

public enum ItemType {
    Inventory,
    Battery,
}

