using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CategoryVoteHandler : MonoBehaviour
{
    public static CategoryVoteHandler Instance;
    public static string[] Categories
    {
        get => Instance._categories;
    }

    private string[] _categories;
    private Dictionary<int, int> _categoryVotes = new();

    public string topCategory;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void InitCategories(string[] categories)
    {
        _categories = categories;
        Debug.Log("Categories: " + string.Join(", ", _categories));
    }

    public int GetIndex(string category)
    {
        return System.Array.IndexOf(_categories, category);
    }

    public bool HandleCategoryVote(int player, int button)
    {
        if (button < 0 || button >= _categories.Length)
        {
            Logger.Log("Button " + button + " is not a valid category");
            return false;
        }
        // Checks if the player has already voted, else it adds the vote to the dictionary
        if (!_categoryVotes.ContainsKey(player))
        {
            Debug.Log("Player " + player + " voted for " + _categories[button]);
            CastVote(player, button);
            return true;
        }
        else
        {
            Logger.Log("Player already voted");
            return false;
        }
    }

    public string GetTopCategory()
    {
        if (SettingsManager.UserSettings.tablet)
        {
            return topCategory;
        }

        if (_categoryVotes.Count == 0)
            return _categories[Random.Range(0, _categories.Length)];

        var _votedCategory = _categories[
            _categoryVotes
                .Values.GroupBy(i => i)
                .OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key)
                .First()
        ];
        Logger.Log("Top category is " + _votedCategory);
        return _votedCategory;
    }

    private void CastVote(int player, int button)
    {
        _categoryVotes[player] = button;
        Logger.Log("Player " + player + " voted for " + _categories[button]);
    }
}
