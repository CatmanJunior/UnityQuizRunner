using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CategoryVoteHandler : MonoBehaviour
{
    private List<string> categories = new List<string>();
    private Dictionary<int, int> categoryVotes = new Dictionary<int, int>();

    public bool HandleCategoryVote(int controller, int button)
    {
        if (button >= categories.Count)
        {
            return false;
        }
        //Checks if the player has already voted, else it adds the vote to the dictionary
        if (!categoryVotes.ContainsKey(controller))
        {
            CastVote(controller, button);
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
        return categories[categoryVotes.Values.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First()];
    }

    private void CastVote(int controller, int button)
    {
        categoryVotes[controller] = button;
        // uiManager.UpdateCategoryVote(currentPlayer);
    }

    public void StartCategoryVote(List<string> categories)
    {
        this.categories = categories;
    }
}
