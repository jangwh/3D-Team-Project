using UnityEngine;

public enum ItemType
{
    Consumable, //소비 아이템
    Equip,
    Other, //기타 아이템

}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Itmes/Item")]
public class ItemData : ScriptableObject
{
    //public static int nextUID;
    public int uid; //아이템 데이터에 대한 고유 식별자
    public ItemType itemType;
    public Sprite icon;
    public GameObject modelPrefab;
    public string itemName;
    [TextArea(3, 3)]
    public string description;
    public int price;

    protected virtual void Reset()
    {
        //데이터 초기화 함수
        //uid = nextUID++;
        itemType = ItemType.Other;
        icon = null;
        itemName = "새 아이템";
        description = "아이템설명을 입력하세요.";
        price = 0;
    }
}
