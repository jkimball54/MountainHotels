using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MountainHotels.DataAccess;
using MountainHotels.Models;
using System.Net;

namespace MountainHotels.FeatureTests
{
    public class MovieControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MovieControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private MountainHotelsContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MountainHotelsContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new MountainHotelsContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewWithHotels()
        {
            var context = GetDbContext();
            context.Hotels.Add(new Hotel { Name = "Fancy Hotel", Location = "Aspen" });
            context.Hotels.Add(new Hotel { Name = "Less Fancy Hotel", Location = "Aspen" });
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Hotels");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Fancy Hotel", html);
            Assert.Contains("Less Fancy Hotel", html);
            Assert.Contains("Aspen", html);
            Assert.DoesNotContain("Fancy Lodge", html);
        }

        [Fact]
        public async Task New_ReturnsFormView()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/hotels/new");
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("Add a Hotel", html);
            Assert.Contains("<form method=\"post\" action=\"/hotels\">", html);
        }

        [Fact]
        public async Task AddHotel_ReturnsRedirectToIndex()
        {
            // Context is only needed if you want to assert against the database
            var context = GetDbContext();

            // Arrange
            var client = _factory.CreateClient();
            var formData = new Dictionary<string, string>
            {
                { "Name", "Another Fancy Hotel" },
                { "Location", "Vail" }
            };

            // Act
            var response = await client.PostAsync("/hotels", new FormUrlEncodedContent(formData));
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("All Hotels", html);
            Assert.Contains("Another Fancy Hotel", html);
            Assert.Contains("Vail", html);
        }
    }
}
