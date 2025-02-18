public class PlayerAnswer
{
    public Question Question { get; }
    public int AnswerId { get; }
    public bool IsCorrect { get; }
    public float TimeTaken { get; }

    public PlayerAnswer(Question question, int answerId, bool isCorrect, float timeTaken)
    {
        Question = question;
        AnswerId = answerId;
        IsCorrect = isCorrect;
        TimeTaken = timeTaken;
    }
}