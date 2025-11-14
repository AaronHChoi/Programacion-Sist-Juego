public interface IPowerUpStrategy
{
    string Name { get; }
    void PowerUp(Simulacro.PlayerMovement player);
}