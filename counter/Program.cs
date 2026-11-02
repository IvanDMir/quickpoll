using System.Text.Json;
using Npgsql;
using StackExchange.Redis;

var redis = ConnectionMultiplexer.Connect("redis,abortConnect=false").GetDatabase();
var db = new NpgsqlConnection("Host=db;Username=postgres;Password=postgres;Database=postgres");

while (true)
{
    try
    {
        db.Open();
        break;
    }
    catch
    {
        Console.WriteLine("Waiting for database...");
        Thread.Sleep(2000);
    }
}

new NpgsqlCommand(
    "CREATE TABLE IF NOT EXISTS ballots (voter_id VARCHAR(64) PRIMARY KEY, choice VARCHAR(8) NOT NULL)",
    db).ExecuteNonQuery();

Console.WriteLine("Counter ready");

while (true)
{
    string? json = redis.ListLeftPop("ballots");
    if (json == null)
    {
        Thread.Sleep(100);
        continue;
    }

    var ballot = JsonSerializer.Deserialize<Ballot>(json)!;

    var cmd = new NpgsqlCommand(
        "INSERT INTO ballots (voter_id, choice) VALUES (@voter, @choice) " +
        "ON CONFLICT (voter_id) DO UPDATE SET choice = @choice",
        db);
    cmd.Parameters.AddWithValue("voter", ballot.voter_id);
    cmd.Parameters.AddWithValue("choice", ballot.choice);
    cmd.ExecuteNonQuery();

    Console.WriteLine($"Vote for {ballot.choice} from {ballot.voter_id}");
}

record Ballot(string voter_id, string choice);
