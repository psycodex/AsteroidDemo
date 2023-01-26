public class Constants
{
    public enum GameStates
    {
        Home,
        Playing,
        GameOver
    }

    public const string TagPlayer = "ship";
    public const string TagAsteroid = "asteroid";
    public const string TagBullet = "bullet";

    public enum PowerUpsType
    {
        Default,
        CrescentMoon,
        Shield
    }
}