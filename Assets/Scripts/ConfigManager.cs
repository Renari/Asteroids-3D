public class ConfigManager {
    public int numberOfAsteroids { get; private set; }
    public float shieldRechargeSpeed { get; private set; }
    public int asteroidMaxSpeed { get; private set; }
    public int trackingAccuracy { get; private set; }
    public float mouseSensitivity { get; set; }
    public float rollSpeed { get; set; }

    public Difficulty difficulty { get; private set; }

    private static ConfigManager instance;

    public enum Difficulty
    {
        Easy, Medium, Hard
    }

    public static ConfigManager getInstance()
    {
        if (instance == null)
        {
            instance = new ConfigManager();
        }
        return instance;
    }

    private ConfigManager() {
        numberOfAsteroids = 10;
        shieldRechargeSpeed = 0.005f;
        asteroidMaxSpeed = 20;
        trackingAccuracy = 150;
        difficulty = Difficulty.Easy;
        mouseSensitivity = 1;
        rollSpeed = 40;
        difficulty = Difficulty.Easy;
    }

    public void SetDifficulty(Difficulty value)
    {
        difficulty = value;
        switch (value)
        {
            case Difficulty.Easy:
                numberOfAsteroids = 10;
                shieldRechargeSpeed = 0.005f;
                asteroidMaxSpeed = 20;
                trackingAccuracy = 150;
                break;
            case Difficulty.Medium:
                numberOfAsteroids = 20;
                shieldRechargeSpeed = 0f;
                asteroidMaxSpeed = 25;
                trackingAccuracy = 125;
                break;
            case Difficulty.Hard:
                numberOfAsteroids = 20;
                shieldRechargeSpeed = 0f;
                asteroidMaxSpeed = 38;
                trackingAccuracy = 100;
                break;
            default:
                break;
        }
    }
}
