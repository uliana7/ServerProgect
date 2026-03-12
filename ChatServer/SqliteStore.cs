using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ChatServer;

public sealed class SqliteStore
{
    private readonly string connectionString;

    public SqliteStore(string dbPath)
    {
        string? folder = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrWhiteSpace(folder))
        {
            Directory.CreateDirectory(folder);
        }

        connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = dbPath
        }.ToString();
    }

    public async Task InitAsync()
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Messages (
                MessageID INTEGER PRIMARY KEY AUTOINCREMENT,
                SenderUsername TEXT NOT NULL,
                Content TEXT NOT NULL,
                Timestamp TEXT NOT NULL
            );
            """;

        await command.ExecuteNonQueryAsync();
    }

    public async Task AddMessageAsync(ChatMessage message)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Messages (SenderUsername, Content, Timestamp)
            VALUES ($sender, $content, $timestamp);
            """;

        command.Parameters.AddWithValue("$sender", message.From);
        command.Parameters.AddWithValue("$content", message.Text);
        command.Parameters.AddWithValue("$timestamp", message.Timestamp.ToString("O"));

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<ChatMessage>> GetLastMessagesAsync(int limit)
    {
        var messages = new List<ChatMessage>();

        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT SenderUsername, Content, Timestamp
            FROM Messages
            ORDER BY MessageID DESC
            LIMIT $limit;
            """;
        command.Parameters.AddWithValue("$limit", limit);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            messages.Add(new ChatMessage
            {
                From = reader.GetString(0),
                Text = reader.GetString(1),
                Timestamp = DateTime.Parse(reader.GetString(2), null, DateTimeStyles.RoundtripKind)
            });
        }

        messages.Reverse();
        return messages;
    }
}