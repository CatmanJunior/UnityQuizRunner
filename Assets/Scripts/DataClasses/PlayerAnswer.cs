
public class PlayerAnswer
{
    public Question Question;
    public int AnswerId { get; set; }
    public bool IsCorrect { get; set; }
    public float TimeTaken { get; set; }

    public PlayerAnswer(Question question, int answerId, bool isCorrect, float timeTaken)
    {
        Question = question;
        AnswerId = answerId;
        IsCorrect = isCorrect;
        TimeTaken = timeTaken;
    }
}