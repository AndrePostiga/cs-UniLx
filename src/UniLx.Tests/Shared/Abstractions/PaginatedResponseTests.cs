using UniLx.Shared.Abstractions;


namespace UniLx.Tests.Shared.Abstractions
{

    public class PaginatedQueryResponseTests
    {
        public class TestStub
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public TestStub(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        [Fact]
        public void PaginatedQueryResponse_WithContent_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var content = new List<TestStub>
        {
            new TestStub(1, "Item1"),
            new TestStub(2, "Item2"),
            new TestStub(3, "Item3")
        };
            int page = 2;
            int perPage = 10;
            int total = 50;

            // Act
            var response = PaginatedQueryResponse<TestStub>.WithContent(content, page, perPage, total);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(content, response.Data);
            Assert.Equal(page, response.Page);
            Assert.Equal(perPage, response.PerPage);
            Assert.Equal(total, response.Total);
        }

        [Fact]
        public void PaginatedQueryResponse_WithContent_ShouldAllowNullData()
        {
            // Arrange
            IEnumerable<TestStub>? content = null;
            int page = 1;
            int perPage = 5;
            int total = 0;

            // Act
            var response = PaginatedQueryResponse<TestStub>.WithContent(content, page, perPage, total);

            // Assert
            Assert.NotNull(response);
            Assert.Null(response.Data);
            Assert.Equal(page, response.Page);
            Assert.Equal(perPage, response.PerPage);
            Assert.Equal(total, response.Total);
        }

        [Fact]
        public void PaginatedQueryResponse_WithContent_ShouldHandleEmptyData()
        {
            // Arrange
            var content = new List<TestStub>();
            int page = 1;
            int perPage = 10;
            int total = 0;

            // Act
            var response = PaginatedQueryResponse<TestStub>.WithContent(content, page, perPage, total);

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Data!);
            Assert.Equal(page, response.Page);
            Assert.Equal(perPage, response.PerPage);
            Assert.Equal(total, response.Total);
        }

        [Fact]
        public void PaginatedQueryResponse_WithContent_ShouldWorkWithDifferentData()
        {
            // Arrange
            var content = new List<TestStub>
            {
                new TestStub(101, "Alpha"),
                new TestStub(102, "Beta")
            };
            int page = 3;
            int perPage = 15;
            int total = 100;

            // Act
            var response = PaginatedQueryResponse<TestStub>.WithContent(content, page, perPage, total);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(content, response.Data);
            Assert.Equal(page, response.Page);
            Assert.Equal(perPage, response.PerPage);
            Assert.Equal(total, response.Total);
        }
    }
}
