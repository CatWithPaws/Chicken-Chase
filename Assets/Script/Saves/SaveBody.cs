using System;
using System.Collections;

[Serializable]
public class SaveBody
{
    public PlayerSaveBody PlayerData { get; private set; }

    public bool IsFirstLauch { get; private set; }

    public int ChickenSkinID { get; set; }
    public int PlayerSkinID { get; set; }

    public long BestDistance { get; set; }

    public SaveBody()
    {
        PlayerData = new PlayerSaveBody();

        ChickenSkinID = 0;
        PlayerSkinID = 0;
        BestDistance = 0;
        IsFirstLauch = true;
    }
}
