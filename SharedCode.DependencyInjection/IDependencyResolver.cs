namespace SharedCode.DependencyInjection;

/// <summary>
/// The dependency resolver interface.
/// </summary>
public interface IDependencyResolver
{
    /// <summary>
    /// Sets up the dependency register.
    /// </summary>
    /// <param name="dependencyRegister">The dependency register.</param>
    void SetUp(IDependencyRegister dependencyRegister);
}
