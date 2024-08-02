/// <summary>
/// Represents an answer to a question.
/// </summary>
/// <param name="answerText">The text of the answer.</param>
/// <param name="isCorrect">Indicates whether the answer is correct or not.</param>
/// <param name="answerId">The unique identifier of the answer.</param>
[System.Serializable]
public class Answer
{
    public int answerId;
    public string AnswerText;
    public bool IsCorrect;

    public Answer(string answerText, bool isCorrect, int answerId)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
    }
}