public class Constants
{
    public enum GameStates
    {
        Home,
        Playing,
        GameOver
    }

    public const string TagPlayer = "Player";
    public const string TagAsteroid = "Asteroid";
    public const string TagBullet = "Bullet";

    public enum PowerUpsType
    {
        Default,
        CrescentMoon,
        Shield
    }
}