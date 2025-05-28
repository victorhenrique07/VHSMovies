using Xunit;

namespace VHSMovies.Tests.Unit.Setup
{
    [CollectionDefinition("TMDbClientCollection")]
    public class TMDbClientCollection : ICollectionFixture<TMDbSetupFixture> { }
}
