using UnityEngine;

public class PassiveItem_2 : PassiveItem
{
    private static readonly string itemName = "Last Stand";
    private static readonly string description = "For one time, when having damages that could have killed the character, the character recover 50% HP instead.";
    private static readonly string usage = "Passive";
    private static readonly string logoPath = "Sprites/Items/Stand Still";
    private static readonly string haloPrefab = "Prefabs/Halo";
    private GameObject halo;

    private void OnEnable()
    {
        halo = Instantiate(Resources.Load<GameObject>(haloPrefab), gameObject.transform);
        halo.transform.localPosition = Vector3.up * 0.8f;
    }
    public bool GetActive()
    {
        return halo;
    }

    public bool Consume()
    {
        if (halo)
        {
            Destroy(halo);
            return true;
        }
        return false;
    }

    public static string GetDescription() => description;

    public static Sprite GetLogo() => Resources.Load<Sprite>(logoPath);

    public static string GetName() => itemName;

    public static string GetUsage() => usage;

    public static GameObject GetShopOption()
    {
        GameObject shopOption = ShopOption.Instantiate();
        ShopOption script = shopOption.GetComponent<ShopOption>();
        script.SetIcon(GetLogo());
        script.SetItemName(GetName());
        script.SetUsage(GetUsage());
        script.SetDescription(GetDescription());
        script.SetOnClickAction(() =>
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_2>();
        });
        return shopOption;
    }
}
