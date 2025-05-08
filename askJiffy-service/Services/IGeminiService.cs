namespace askJiffy_service.Services
{
    public interface IGeminiService
    {
        //streaming generated answers to questions
        IAsyncEnumerable<string> StreamAnswerAsync(string question);

        //for one-shot title generation
        Task<string> GenerateTitleAsync(string prompt);
    }
}
