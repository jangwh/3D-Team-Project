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

    public string SaveFilePath => $"{Application.dataPath}/{saveFileName}.json";
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
        FileData save;
        try
        {
            string json = File.ReadAllText(SaveFilePath);
            save = JsonUtility.FromJson<FileData>(json);
        }
        catch
        {
            print("save 파일이 없음.");
            return;
        }

        InventoryManager.Items.AddRange(save.items);
        InventoryManager.Items.AddRange(save.consumables);

        foreach (ItemStatus item in InventoryManager.Items)
        {
            item.Data = itemDatas.Find(x => x.uid == item.uid);
        }
        InventoryManager.Refresh();
    }
}
