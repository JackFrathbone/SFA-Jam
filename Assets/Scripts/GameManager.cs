using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Sprite> _playerSprites = new();
    private List<Sprite> _currentRoundSprites = new();

    [SerializeField] GameObject _startScreen;
    [SerializeField] GameObject _winScreen;
    [SerializeField] TextMeshProUGUI _winScreenTextMesh;

    private PlayerInput _playerInput;

    public PlayerController _player1Controller;
    public PlayerController _player2Controller;
    public PlayerController _player3Controller;

    private void Awake()
    {
        RandomiseSprites();
        Time.timeScale = 0f;
        _startScreen.SetActive(true);
        _winScreen.SetActive(false);

        _playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (_startScreen.activeSelf)
        {
            if (_playerInput.actions.FindAction("AttackPlayer1").ReadValue<float>() != 0 && _playerInput.actions.FindAction("AttackPlayer2").ReadValue<float>() != 0 && _playerInput.actions.FindAction("AttackPlayer3").ReadValue<float>() != 0)
            {
                _startScreen.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        if (_player1Controller != null && (_player2Controller == null && _player3Controller == null))
        {
            _winScreen.SetActive(true);
            _winScreenTextMesh.text = "Player 1 wins!";
            Time.timeScale = 0f;
        }
        else if (_player2Controller != null && (_player3Controller == null && _player1Controller == null))
        {
            _winScreen.SetActive(true);
            _winScreenTextMesh.text = "Player 2 wins!";
            Time.timeScale = 0f;
        }
        else if (_player3Controller != null && (_player1Controller == null && _player2Controller == null))
        {
            _winScreen.SetActive(true);
            _winScreenTextMesh.text = "Player 3 wins!";
            Time.timeScale = 0f;
        }

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
}
