using Godot;

namespace PoseidonAncient.PoseidonAncientCode.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{
    public static string ImagePath(this string path)
    {
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", path);
    }

    public static string CardImagePath(this string path)
    {
        path = Path.Join(PoseidonAncientMainFile.ResPath, "images", "card_portraits", path);
        if (ResourceLoader.Exists(path)) return path;

        PoseidonAncientMainFile.Logger.Info("Could not find card image path: " + path);
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "card_portraits", "card.png");
    }

    public static string BigCardImagePath(this string path)
    {
        path = Path.Join(PoseidonAncientMainFile.ResPath, "images", "card_portraits", "big", path);
        if (ResourceLoader.Exists(path)) return path;

        PoseidonAncientMainFile.Logger.Info("Could not find big card image path: " + path);
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "card_portraits", "big", "card.png");
    }

    public static string PowerImagePath(this string path)
    {
        path = Path.Join(PoseidonAncientMainFile.ResPath, "images", "powers", path);
        if (ResourceLoader.Exists(path)) return path;

        PoseidonAncientMainFile.Logger.Info("Could not find power image path: " + path);
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "powers", "power.png");
    }

    public static string BigPowerImagePath(this string path)
    {
        path = Path.Join(PoseidonAncientMainFile.ResPath, "images", "powers", "big", path);
        if (ResourceLoader.Exists(path)) return path;

        PoseidonAncientMainFile.Logger.Info("Could not find big power image path: " + path);
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "powers", "big", "power.png");
    }

    public static string RelicImagePath(this string path)
    {
        path = Path.Join(PoseidonAncientMainFile.ResPath, "images", "relics", path);
        if (ResourceLoader.Exists(path)) return path;

        PoseidonAncientMainFile.Logger.Info("Could not find relic image path: " + path);
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "relics", "relic.png");
    }

    public static string RelicOutlineImagePath(this string path)
    {
        path = Path.Join(PoseidonAncientMainFile.ResPath, "images", "relics", "outline", path);
        if (ResourceLoader.Exists(path)) return path;

        PoseidonAncientMainFile.Logger.Info("Could not find relic image path: " + path);
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "relics", "outline", "relic.png");
    }

    public static string BigRelicImagePath(this string path)
    {
        path = Path.Join(PoseidonAncientMainFile.ResPath, "images", "relics", "big", path);
        if (ResourceLoader.Exists(path)) return path;

        PoseidonAncientMainFile.Logger.Info("Could not find big relic image path: " + path);
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "relics", "big", "relic.png");
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "charui", path);
    }

    public static string AncientImagePath(this string path)
    {
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "ancients", path);
    }

    public static string SoundPath(this string path)
    {
        return Path.Join(PoseidonAncientMainFile.ResPath, "sounds", path);
    }

    public static string EnchantmentImagePath(this string path)
    {
        return Path.Join(PoseidonAncientMainFile.ResPath, "images", "enchantments", path);
    }
}