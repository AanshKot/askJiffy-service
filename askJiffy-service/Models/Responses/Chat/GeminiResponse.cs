namespace askJiffy_service.Models.Responses.Chat
{
    public class GeminiResponse
    {
        public List<Candidate> Candidates { get; set; } = new();
        public UsageMetadata UsageMetadata { get; set; } = new();
        public string ModelVersion { get; set; } = string.Empty;
    }

    public class Candidate
    {
        public Content Content { get; set; } = new();
        public string FinishReason { get; set; } = string.Empty;
        public double AvgLogprobs { get; set; }
    }

    public class Content
    {
        public List<Part> Parts { get; set; } = new();
        public string Role { get; set; } = string.Empty;
    }

    public class Part
    {
        public string Text { get; set; } = string.Empty;
    }

    public class UsageMetadata
    {
        public int PromptTokenCount { get; set; }
        public int CandidatesTokenCount { get; set; }
        public int TotalTokenCount { get; set; }
        public List<TokenDetail> PromptTokensDetails { get; set; } = new();
        public List<TokenDetail> CandidatesTokensDetails { get; set; } = new();
    }

    public class TokenDetail
    {
        public string Modality { get; set; } = string.Empty;
        public int TokenCount { get; set; }
    }
}
