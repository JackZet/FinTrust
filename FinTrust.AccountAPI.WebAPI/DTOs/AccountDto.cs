using System.Text.Json.Serialization;

namespace FinTrust.AccountAPI.WebAPI.DTOs;

public record AccountDto
{
    [JsonPropertyName("accountId")]
    public required string AccountId { get; init; }
    
    [JsonPropertyName("balance")]
    public required int Balance { get; init; }
};