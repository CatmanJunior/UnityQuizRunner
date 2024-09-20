using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CategoryVoteHandler : MonoBehaviour
{
    private string[] categories;
    private Dictionary<int, int> categoryVotes = new Dictionary<int, int>();

    public void InitCategories(string[] categoryList)
    {
        categories = categoryList;
    }

    public int GetIndex(string category)
    {
        print("Getting index of " + category + " in " + categories);
        int i = System.Array.IndexOf(categories, category);
        print("Index is " + i);
        return i;
    }

    public bool HandleCategoryVote(int controller, int button)
    {
        if (button >= categories.Length-1)
        {
            Debug.Log("Button " + button + " is not a valid category");
            return false;
        }
        //Checks if the player has already voted, else it adds the vote to the dictionary
        if (!categoryVotes.ContainsKey(controller))
        {
            CastVote(controller, button);
            Debug.Log("Player " + controller + " voted for " + categories[button]);
            return true;
        }
        else
        {
            print("Player already voted");

        }
        return false;
    }

    public string GetTopCategory()
    {
        //Returns the category with the most votes
        //TODO: Handle ties
        if (categoryVotes.Count == 0)
        {
            return categories[Random.Range(0, categories.Length)].ToString();
        }
        if (categoryVotes.Count == 1)
        {
            return categories[categoryVotes.Values.First()];
        }
        var _cat =categories[categoryVotes.Values.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First()];
        Debug.Log("Top category is " + _cat);
        return _cat;
    }

    private void CastVote(int controller, int button)
    {
        categoryVotes[controller] = button;
        UIManager.Instance.PlayerVoted(controller);
    }

}
