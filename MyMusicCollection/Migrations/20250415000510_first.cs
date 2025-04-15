using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyMusicCollection.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bandName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    yearsOfActivity = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    AlbumId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlbumDuration = table.Column<int>(type: "int", nullable: false),
                    TrackCount = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Format = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.AlbumId);
                    table.ForeignKey(
                        name: "FK_Albums_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistGenre",
                columns: table => new
                {
                    ArtistsArtistId = table.Column<int>(type: "int", nullable: false),
                    GenresGenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistGenre", x => new { x.ArtistsArtistId, x.GenresGenreId });
                    table.ForeignKey(
                        name: "FK_ArtistGenre_Artists_ArtistsArtistId",
                        column: x => x.ArtistsArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistGenre_Genres_GenresGenreId",
                        column: x => x.GenresGenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayLists",
                columns: table => new
                {
                    PlayListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayListName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayLists", x => x.PlayListId);
                    table.ForeignKey(
                        name: "FK_PlayLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlbumGenre",
                columns: table => new
                {
                    AlbumsAlbumId = table.Column<int>(type: "int", nullable: false),
                    GenresGenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumGenre", x => new { x.AlbumsAlbumId, x.GenresGenreId });
                    table.ForeignKey(
                        name: "FK_AlbumGenre_Albums_AlbumsAlbumId",
                        column: x => x.AlbumsAlbumId,
                        principalTable: "Albums",
                        principalColumn: "AlbumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumGenre_Genres_GenresGenreId",
                        column: x => x.GenresGenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatingsAndReviews",
                columns: table => new
                {
                    RatingAndReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingsAndReviews", x => x.RatingAndReviewId);
                    table.ForeignKey(
                        name: "FK_RatingsAndReviews_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "AlbumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RatingsAndReviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    TrackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    NumberInList = table.Column<int>(type: "int", nullable: false),
                    LyricsAuthor = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MusicAuthor = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.TrackId);
                    table.ForeignKey(
                        name: "FK_Tracks_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "AlbumId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCollections",
                columns: table => new
                {
                    UserCollectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "wanted"),
                    AlbumId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollections", x => x.UserCollectionId);
                    table.ForeignKey(
                        name: "FK_UserCollections_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "AlbumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCollections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenreTrack",
                columns: table => new
                {
                    GenresGenreId = table.Column<int>(type: "int", nullable: false),
                    TracksTrackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreTrack", x => new { x.GenresGenreId, x.TracksTrackId });
                    table.ForeignKey(
                        name: "FK_GenreTrack_Genres_GenresGenreId",
                        column: x => x.GenresGenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreTrack_Tracks_TracksTrackId",
                        column: x => x.TracksTrackId,
                        principalTable: "Tracks",
                        principalColumn: "TrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistTrack",
                columns: table => new
                {
                    PlayListId = table.Column<int>(type: "int", nullable: false),
                    TrackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistTrack", x => new { x.PlayListId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_PlayLists_PlayListId",
                        column: x => x.PlayListId,
                        principalTable: "PlayLists",
                        principalColumn: "PlayListId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "TrackId");
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "ArtistId", "Biography", "Country", "bandName", "yearsOfActivity" },
                values: new object[,]
                {
                    { 1, "Canadian rock band formed in Norwood, Ontario.", "Canada", "Three Days Grace", "1997-present" },
                    { 2, "American heavy metal band, one of the 'Big Four' of thrash metal.", "USA", "Metallica", "1981-present" },
                    { 3, "American heavy metal band from Chicago.", "USA", "Disturbed", "1994-present" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "GenreId", "GenreName" },
                values: new object[,]
                {
                    { 1, "Post-Grunge" },
                    { 2, "Alternative Metal" },
                    { 3, "Thrash Metal" },
                    { 4, "Heavy Metal" },
                    { 5, "Nu Metal" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DateOfBirth", "Email", "Password", "UserName" },
                values: new object[] { 1, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "musicfan@example.com", "SecurePass123", "MusicFan" });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "AlbumId", "AlbumDuration", "AlbumName", "ArtistId", "Format", "Label", "ReleaseDate", "TrackCount" },
                values: new object[,]
                {
                    { 1, 934, "Three Days Grace", 1, "CD", "Jive Records", new DateTime(2003, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 2, 855, "One-X", 1, "CD", "Jive Records", new DateTime(2006, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 3, 1710, "Master of Puppets", 2, "Vinyl", "Elektra Records", new DateTime(1986, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 4, 1429, "Metallica", 2, "CD", "Elektra Records", new DateTime(1991, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, 1029, "Ten Thousand Fists", 3, "CD", "Reprise Records", new DateTime(2005, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 6, 988, "Immortalized", 3, "Digital", "Reprise Records", new DateTime(2015, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 }
                });

            migrationBuilder.InsertData(
                table: "ArtistGenre",
                columns: new[] { "ArtistsArtistId", "GenresGenreId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 4 },
                    { 3, 5 }
                });

            migrationBuilder.InsertData(
                table: "PlayLists",
                columns: new[] { "PlayListId", "DateCreated", "PlayListName", "UserId" },
                values: new object[] { 1, new DateTime(2025, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Metal Favorites", 1 });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "TrackId", "AlbumId", "Duration", "LyricsAuthor", "MusicAuthor", "NumberInList", "TrackName" },
                values: new object[,]
                {
                    { 1, 1, 258, "Adam Gontier", "Three Days Grace", 1, "Burn" },
                    { 2, 1, 192, "Adam Gontier", "Three Days Grace", 2, "Just Like You" },
                    { 3, 1, 229, "Adam Gontier", "Three Days Grace", 3, "I Hate Everything About You" },
                    { 4, 1, 255, "Adam Gontier", "Three Days Grace", 4, "Home" },
                    { 5, 2, 231, "Adam Gontier", "Three Days Grace", 1, "Animal I Have Become" },
                    { 6, 2, 209, "Adam Gontier", "Three Days Grace", 2, "Pain" },
                    { 7, 2, 208, "Adam Gontier", "Three Days Grace", 3, "Never Too Late" },
                    { 8, 2, 207, "Adam Gontier", "Three Days Grace", 4, "Riot" },
                    { 9, 3, 312, "James Hetfield", "Metallica", 1, "Battery" },
                    { 10, 3, 515, "James Hetfield", "Metallica", 2, "Master of Puppets" },
                    { 11, 3, 387, "James Hetfield", "Metallica", 3, "Welcome Home (Sanitarium)" },
                    { 12, 3, 496, "James Hetfield", "Metallica", 4, "Disposable Heroes" },
                    { 13, 4, 331, "James Hetfield", "Metallica", 1, "Enter Sandman" },
                    { 14, 4, 324, "James Hetfield", "Metallica", 2, "Sad But True" },
                    { 15, 4, 386, "James Hetfield", "Metallica", 3, "The Unforgiven" },
                    { 16, 4, 388, "James Hetfield", "Metallica", 4, "Nothing Else Matters" },
                    { 17, 5, 214, "David Draiman", "Disturbed", 1, "Ten Thousand Fists" },
                    { 18, 5, 245, "David Draiman", "Disturbed", 2, "Stricken" },
                    { 19, 5, 283, "David Draiman", "Disturbed", 3, "I'm Alive" },
                    { 20, 5, 287, "David Draiman", "Disturbed", 4, "Land of Confusion" },
                    { 21, 6, 253, "David Draiman", "Disturbed", 1, "The Vengeful One" },
                    { 22, 6, 263, "David Draiman", "Disturbed", 2, "Immortalized" },
                    { 23, 6, 245, "David Draiman", "Disturbed", 3, "The Sound of Silence" },
                    { 24, 6, 227, "David Draiman", "Disturbed", 4, "Fire It Up" }
                });

            migrationBuilder.InsertData(
                table: "PlaylistTrack",
                columns: new[] { "PlayListId", "TrackId" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 1, 13 },
                    { 1, 23 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumGenre_GenresGenreId",
                table: "AlbumGenre",
                column: "GenresGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ArtistId_AlbumName",
                table: "Albums",
                columns: new[] { "ArtistId", "AlbumName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_ReleaseDate",
                table: "Albums",
                column: "ReleaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistGenre_GenresGenreId",
                table: "ArtistGenre",
                column: "GenresGenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_GenreName",
                table: "Genres",
                column: "GenreName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GenreTrack_TracksTrackId",
                table: "GenreTrack",
                column: "TracksTrackId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayLists_DateCreated",
                table: "PlayLists",
                column: "DateCreated");

            migrationBuilder.CreateIndex(
                name: "IX_PlayLists_UserId_PlayListName",
                table: "PlayLists",
                columns: new[] { "UserId", "PlayListName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTrack_TrackId",
                table: "PlaylistTrack",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingsAndReviews_AlbumId",
                table: "RatingsAndReviews",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingsAndReviews_UserId",
                table: "RatingsAndReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_AlbumId",
                table: "Tracks",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollections_AlbumId",
                table: "UserCollections",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollections_DateAdded",
                table: "UserCollections",
                column: "DateAdded");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollections_UserId",
                table: "UserCollections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumGenre");

            migrationBuilder.DropTable(
                name: "ArtistGenre");

            migrationBuilder.DropTable(
                name: "GenreTrack");

            migrationBuilder.DropTable(
                name: "PlaylistTrack");

            migrationBuilder.DropTable(
                name: "RatingsAndReviews");

            migrationBuilder.DropTable(
                name: "UserCollections");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "PlayLists");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
