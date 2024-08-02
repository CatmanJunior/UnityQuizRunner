
using System;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Represents a question with an image.
/// </summary>
public class ImageQuestion : Question
{
    /// <summary>
    /// Gets or sets the image file associated with the question.
    /// </summary>
    public Image ImageFile { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageQuestion"/> class with an image file.
    /// </summary>
    /// <param name="questionText">The text of the question.</param>
    /// <param name="answers">The list of possible answers.</param>
    /// <param name="imageFile">The image file associated with the question.</param>
    /// <param name="category">The category of the question.</param>
    public ImageQuestion(string questionText, List<Answer> answers, Image imageFile, string category) : base(questionText, answers, category)
    {
        ImageFile = imageFile;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageQuestion"/> class without an image file.
    /// </summary>
    /// <param name="questionText">The text of the question.</param>
    /// <param name="answers">The list of possible answers.</param>
    /// <param name="category">The category of the question.</param>
    public ImageQuestion(string questionText, List<Answer> answers, string category) : base(questionText, answers, category)
    {
        Console.WriteLine("No image file provided");
        ImageFile = null;
    }
}