using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bunit;

using FluentAssertions;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using MudBlazor.Services;

using VHSMovies.Api.Integration.Main.Responses;
using VHSMovies.Web.Components.Pages.Titles;

namespace VHSMovies.Tests.Website.E2E.Components
{
    public class TitleCardTests : TestContext
    {
        public TitleCardTests()
        {
            CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");

            Services.AddMudServices();

            Services.AddSingleton<MudBlazor.IKeyInterceptorService>(new Mock<MudBlazor.IKeyInterceptorService>().Object);
        }

        [Fact]
        public void ShouldTitleCardRenderCorrectly()
        {
            // Arrange
            var title = new TitleResponse
            {
                Name = "O Poderoso Chefão",
                Description = "Um clássico do cinema.",
                AverageRating = 9.2m,
                BackdropImageUrl = "https://exemplo.com/poster.jpg",
                ReleaseDate = new DateOnly(1972, 3, 24),
                Genres = new List<GenreResponse>
                {
                    new GenreResponse { Id = 1, Name = "Crime" },
                    new GenreResponse { Id = 2, Name = "Drama" }
                }
            };

            var cut = RenderComponent<TitleCard>(parameters => parameters
                .Add(p => p.Title, title)
            );

            // Assert
            cut.FindAll(".movie-card").Should().NotBeNullOrEmpty();
            cut.Find(".movie-score").TextContent.Should().Contain("9,2");
            cut.Find(".movie-info h3").TextContent.Should().Contain("O Poderoso Chefão");
            cut.Find(".movie-meta").TextContent.Should().Contain("1972");
            cut.Find(".movie-desc").TextContent.Should().Contain("Um clássico do cinema.");
            cut.Find(".movie-poster img").GetAttribute("src").Should().Be("https://exemplo.com/poster.jpg");
            cut.FindAll(".movie-genres").Should().AllSatisfy(g =>
            {
                g.TextContent.Should().Contain("Crime");
                g.TextContent.Should().Contain("Drama");
            });
        }
    }
}