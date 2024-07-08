using Cinema.Model;
using Cinema.Repository.Common;
using Npgsql;

namespace Cinema.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            var reviews = new List<Review>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = @"
                    SELECT r.*, u.""FirstName""
                    FROM ""Review"" r
                    JOIN ""User"" u ON r.""UserId"" = u.""Id"";";

                using (var command = new NpgsqlCommand(commandText, conn))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var review = new Review
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                            UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                            MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                            DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                            CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                            UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName"))
                        };
                        reviews.Add(review);
                    }
                }
            }
            return reviews;
        }

        public async Task<Review> GetReviewByIdAsync(Guid id)
        {
            Review review = null;
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = @"
                    SELECT r.*, u.""FirstName""
                    FROM ""Review"" r
                    JOIN ""User"" u ON r.""UserId"" = u.""Id""
                    WHERE r.""Id"" = @Id;";

                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            review = new Review
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                                UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                                UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName"))
                            };
                        }
                    }
                }
            }
            return review;
        }
        
        public async Task<IEnumerable<Review>> GetReviewsByMovieIdAsync(Guid id)
        {
            var reviews = new List<Review>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var commandText = @"
            SELECT r.*, u.""FirstName""
            FROM ""Review"" r
            JOIN ""User"" u ON r.""UserId"" = u.""Id""
            WHERE r.""MovieId"" = @MovieId;";

                using (var command = new NpgsqlCommand(commandText, conn))
                {
                    command.Parameters.AddWithValue("@MovieId", id); // Dodajemo parametar za MovieId

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var review = new Review
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                                UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                CreatedByUserId = reader.GetGuid(reader.GetOrdinal("CreatedByUserId")),
                                UpdatedByUserId = reader.GetGuid(reader.GetOrdinal("UpdatedByUserId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName"))
                            };
                            reviews.Add(review);
                        }
                    }
                }
            }
            return reviews;
        }


        public async Task AddReviewAsync(Review review)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO \"Review\" (\"Id\", \"Description\", \"Rating\", \"UserId\", \"MovieId\", \"IsActive\", \"DateCreated\", \"DateUpdated\", \"CreatedByUserId\", \"UpdatedByUserId\") " +
                    "VALUES (@Id, @Description, @Rating, @UserId, @MovieId, @IsActive, @DateCreated, @DateUpdated, @CreatedByUserId, @UpdatedByUserId)", conn))
                {
                    command.Parameters.AddWithValue("Id", review.Id);
                    command.Parameters.AddWithValue("Description", review.Description);
                    command.Parameters.AddWithValue("Rating", review.Rating);
                    command.Parameters.AddWithValue("UserId", review.UserId);
                    command.Parameters.AddWithValue("MovieId", review.MovieId);
                    command.Parameters.AddWithValue("IsActive", review.IsActive);
                    command.Parameters.AddWithValue("DateCreated", review.DateCreated);
                    command.Parameters.AddWithValue("DateUpdated", review.DateUpdated);
                    command.Parameters.AddWithValue("CreatedByUserId", review.CreatedByUserId);
                    command.Parameters.AddWithValue("UpdatedByUserId", review.UpdatedByUserId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateReviewAsync(Review review)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var command = new NpgsqlCommand(
                    "UPDATE \"Review\" SET " +
                    "\"Description\" = @Description, " +
                    "\"Rating\" = @Rating, " +
                    "\"UserId\" = @UserId, " +
                    "\"MovieId\" = @MovieId, " +
                    "\"IsActive\" = @IsActive, " +
                    "\"DateCreated\" = @DateCreated, " +
                    "\"DateUpdated\" = @DateUpdated, " +
                    "\"CreatedByUserId\" = @CreatedByUserId, " +
                    "\"UpdatedByUserId\" = @UpdatedByUserId " +
                    "WHERE \"Id\" = @Id", conn))
                {
                    command.Parameters.AddWithValue("Description", review.Description);
                    command.Parameters.AddWithValue("Rating", review.Rating);
                    command.Parameters.AddWithValue("UserId", review.UserId);
                    command.Parameters.AddWithValue("MovieId", review.MovieId);
                    command.Parameters.AddWithValue("IsActive", review.IsActive);
                    command.Parameters.AddWithValue("DateCreated", review.DateCreated);
                    command.Parameters.AddWithValue("DateUpdated", review.DateUpdated);
                    command.Parameters.AddWithValue("CreatedByUserId", review.CreatedByUserId);
                    command.Parameters.AddWithValue("UpdatedByUserId", review.UpdatedByUserId);
                    command.Parameters.AddWithValue("Id", review.Id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteReviewAsync(Guid id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var command = new NpgsqlCommand("DELETE FROM \"Review\" WHERE \"Id\" = @Id", conn))
                {
                    command.Parameters.AddWithValue("Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
