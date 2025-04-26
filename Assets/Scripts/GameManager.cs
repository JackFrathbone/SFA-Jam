using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Sprite> _playerSprites = new();
    private List<Sprite> _currentRoundSprites = new();

    private void Awake()
    {
        RandomiseSprites();
    }

    private void RandomiseSprites()
    {
        // Create a copy of the original list to avoid modifying it directly while iterating
        List<Sprite> availableSprites = new(_playerSprites);
        _currentRoundSprites.Clear();

        for (int i = 0; i < 4; i++)
        {
            // Generate a random index within the bounds of the available sprites
            int randomIndex = Random.Range(0, availableSprites.Count);

            // Add the sprite at the random index to the selected list
            _currentRoundSprites.Add(availableSprites[randomIndex]);

            // Remove the selected sprite from the available list to avoid picking it again
            availableSprites.RemoveAt(randomIndex);
        }
    }

    public Sprite GetSpriteForPlayer(int playerNum)
    {
        return _currentRoundSprites[playerNum];
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
