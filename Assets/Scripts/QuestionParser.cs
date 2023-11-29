using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// A static class that provides methods for parsing and loading questions from a text file, and getting a random set of questions.
/// </summary>
public static class QuestionParser
{
    private static readonly List<Question> _questions = new List<Question>();
    private static readonly Dictionary<string, List<Question>> _categories = new Dictionary<string, List<Question>>();
    private const string DefaultCategory = "General";

    private static string QuestionPrefix = "Q:";
    private static string CategoryPrefix = "T:";
    private static string EndPrefix = "END";
    private static List<string> AnswerPrefixes = new List<string> { "A:", "B:", "C:", "D:" };

    /// <summary>
    /// Loads questions from a text file and returns a list of all loaded questions.
    /// </summary>
    /// <returns>A list of all loaded questions.</returns>
    public static List<Question> LoadQuestionsFromTxt()
    {
        _categories.Add(DefaultCategory, new List<Question>());

        var lines = LoadFromFile();

        var questionLines = new List<string>();
        var isParsingQuestion = false;
        var currentCategory = DefaultCategory;

        foreach (var line in lines)
        {
            if (line.StartsWith("END"))
            {
                if (isParsingQuestion)
                {
                    CreateQuestion(questionLines, currentCategory);
                    isParsingQuestion = false;
                    questionLines.Clear();
                    currentCategory = DefaultCategory;
                }
            }
            else if (line.StartsWith("T:"))
            {
                var category = line.Substring(3);
                if (string.IsNullOrEmpty(category))
                {
                    category = DefaultCategory;
                }
                else if (!_categories.ContainsKey(category))
                {
                    _categories.Add(category, new List<Question>());
                }
                currentCategory = category;
            }
            else if (line.StartsWith("Q:"))
            {   
                // If the parser is already parsing a question, it means that the previous question block was not ended with "END".
                // This is not allowed, so we skip the current question block and log an error.
                if (isParsingQuestion)
                {
                    Debug.LogError("Question block not ended with \"END\". Skipping question block.");
                    continue;
                }
                isParsingQuestion = true;
                questionLines.Add(line);
            }
            else if (isParsingQuestion && !string.IsNullOrEmpty(line))
            {
                questionLines.Add(line);
            }
        }

        if (isParsingQuestion)
        {
            CreateQuestion(questionLines, currentCategory);
        }

        return _questions;
    }

    /// <summary>
    /// Creates a new question object from a list of question lines and adds it to the list of loaded questions.
    /// </summary>
    /// <param name="questionLines">A list of question lines.</param>
    /// <param name="category">The category of the question.</param>
    private static void CreateQuestion(List<string> questionLines, string category)
    {
        var question = ParseQuestionBlock(questionLines, category);
        _questions.Add(question);
        _categories[category].Add(question);
    }

    // Load questions from all text files in the folder and combine the lines
    private static List<string> LoadFromFile()
    {

        var filePath = Path.Combine(Application.streamingAssetsPath, "Questions");
        var files = Directory.GetFiles(filePath, "*.txt");
        var lines = new List<string>();

        if (files.Length == 0)
        {
            Debug.LogError("No questions found in the Questions folder");
            return null;
        }

        foreach (var file in files)
        {
            lines.AddRange(File.ReadAllLines(file));
        }
        return lines;
    }

    /// <summary>
    /// Parses a list of question lines and returns a new question object.
    /// </summary>
    /// <param name="questionLines">A list of question lines.</param>
    /// <param name="category">The category of the question.</param>
    /// <returns>A new question object.</returns>
    private static Question ParseQuestionBlock(List<string> questionLines, string category)
    {
        var questionText = questionLines.First().Substring(3);
        var answers = new List<Answer>();
        var answerTypes = new List<char> { 'A', 'B', 'C', 'D' };

        foreach (var line in questionLines.Skip(1))
        {
            if (!string.IsNullOrEmpty(line) && answerTypes.Contains(line[0]))
            {
                var answerText = line.Substring(3);
                var isCorrect = line.EndsWith(" +");

                if (isCorrect)
                {
                    answerText = answerText.Substring(0, answerText.Length - 2);
                }

                answers.Add(new Answer(answerText, isCorrect, answerTypes.IndexOf(line[0])));
            }
        }

        return new Question(questionText, answers, category);
    }

    /// <summary>
    /// Returns a list of n random questions.
    /// </summary>
    /// <param name="n">The number of questions to return.</param>
    /// <param name="category">The category of the questions to return. If null, questions from all categories will be returned.</param>
    /// <returns>A list of n random questions.</returns>
    public static List<Question> GetRandomQuestions(int n, string category = null)
    {
        if (n > _questions.Count)
        {
            Debug.LogError("Not enough questions loaded to get n amount of random questions");
            n = _questions.Count;
        }

        var randomQuestions = new List<Question>();
        var questionsCopy = new List<Question>(_questions);

        if (category != null)
        {
            if (!_categories.ContainsKey(category))
            {
                Debug.LogError("Category not found");
                return null;
            }
            var questionsInCategory = _categories[category];
            if (n > questionsInCategory.Count)
            {
                n = questionsInCategory.Count;
                Debug.LogWarning("Not enough questions in category to get random questions. Returning all questions in category. " + n + " questions returned.");
                //list amount of questions in each category
                foreach (string cat in _categories.Keys)
                {
                    Debug.LogWarning(cat + ": " + _categories[cat].Count);
                }
            }
            questionsCopy = new List<Question>(questionsInCategory);
        }

        // Randomly select n questions from the list of loaded questions
        for (var i = 0; i < n; i++)
        {
            var randomIndex = Random.Range(0, questionsCopy.Count);
            randomQuestions.Add(questionsCopy[randomIndex]);
            questionsCopy.RemoveAt(randomIndex);
        }

        return randomQuestions;
    }


    /// <summary>
    /// Returns a list of all categories. If n is specified, a list of n random categories will be returned.
    /// </summary>
    /// <param name="n">The number of categories to return. If 0, all categories will be returned.</param>
    /// <returns>A list of all categories.</returns>
    public static List<string> GetCategories(int n = 0)
    {
        if (n == 0)
        {
            return _categories.Keys.ToList();
        }
        else
        {
            var categories = new List<string>();
            var categoriesCopy = new List<string>(_categories.Keys);
            for (var i = 0; i < n; i++)
            {
                var randomIndex = Random.Range(0, categoriesCopy.Count);
                categories.Add(categoriesCopy[randomIndex]);
                categoriesCopy.RemoveAt(randomIndex);
            }
            return categories;
        }
    }



}
