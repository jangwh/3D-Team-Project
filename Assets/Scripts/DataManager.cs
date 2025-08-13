using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class FileData
{
    public List<ItemStatus> items = new List<ItemStatus>();
    public List<ConsumableStatus> consumables = new List<ConsumableStatus>();

    public void AddItem(ItemStatus status)
    {
        if (status is ConsumableStatus)
        {
            consumables.Add(status as ConsumableStatus);
        }
        else
        {
            items.Add(status);
        }
    }
}
public class DataManager : MonoBehaviour
{
    public DataManager Instance { get; private set; }

    public string saveFileName;

    public List<ItemData> itemDatas;

    // Use persistentDataPath for save file location
    public string SaveFilePath => $"{Application.persistentDataPath}/{saveFileName}.json";
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Load();
    }
    void OnApplicationQuit()
    {
        Save();
    }
    public void Save()
    {
        FileData save = new FileData();
        foreach (ItemStatus status in InventoryManager.Items)
        {
            save.AddItem(status);
        }
        File.WriteAllText(SaveFilePath, JsonUtility.ToJson(save));
    }
    public void Load()
    {
        FileData save = null;
        try
        {
            string json = File.ReadAllText(SaveFilePath);
            save = JsonUtility.FromJson<FileData>(json);
        }
        catch (System.Exception ex)
        {
            Debug.Log($"Failed to load save file: {ex.Message}\n{ex.StackTrace}");
            return;
        }

        // Clear current inventory before loading
        InventoryManager.Items.Clear();
        if (save != null)
        {
            InventoryManager.Items.AddRange(save.items);
            InventoryManager.Items.AddRange(save.consumables);
        }

        foreach (ItemStatus item in InventoryManager.Items)
        {
            item.Data = itemDatas.Find(x => x.uid == item.uid);
        }

        // Update UI directly and refresh MyItems UI after loading
        var myItems = FindObjectOfType<MyItems>();
        if (myItems != null)
        {
            myItems.Refresh(InventoryManager.Items);
        }
        else
        {
            InventoryManager.Refresh();
        }
    }
}
