using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image Panel;
    public Text PanelText;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color PanelColor;
    public Color TextColor;
}

public class GameControllerScript : MonoBehaviour
{
    private string _playerText;
    private int _moveCount;
    private string _botText;

    public Text[] GridCellsList;
    public GameObject GameOverPanel;
    public Text GameOverText;
    public GameObject RestartButton;
    public GameObject SideChoice;

    public Player PlayerX;
    public Player PlayerO;
    public PlayerColor ActivePlayerColor;
    public PlayerColor InactivePlayerColor;

    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
        GameOverPanel.SetActive(false);
        _moveCount = 0;
        RestartButton.SetActive(false);
    }

    private void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < GridCellsList.Length; i++)
        {
            GridCellsList[i].GetComponentInParent<GridCellScript>().SetGameControllerReference(this);
        }
    }

    private void StarGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        SideChoice.SetActive(false);
    }

    // A function for 2 active players
    //private void ChangePlayer()
    //{
    //    _playerCellText = (_playerCellText == "X") ? "O" : "X";

    //    if (_playerCellText == "X")
    //    {
    //        SetPlayerColors(PlayerX, PlayerO);
    //    }
    //    else
    //    {
    //        SetPlayerColors(PlayerO, PlayerX);
    //    }
    //}

    private void GameOver(string gameResult)
    {
        SetBoardInteractable(false);

        if (gameResult == "draw")
        {
            SetGameOverText("It's a draw!");
            SetPlayerColorsInactive();
        }
        else
        {
            SetGameOverText($"{gameResult} Wins!");
        }

        RestartButton.SetActive(true);
    }

    private void SetGameOverText(string gameOverText)
    {
        GameOverPanel.SetActive(true);
        GameOverText.text = gameOverText;
    }

    private void SetPlayerColors(Player currentPlayer, Player previousPlayer)
    {
        currentPlayer.Panel.color = ActivePlayerColor.PanelColor;
        currentPlayer.PanelText.color = ActivePlayerColor.TextColor;
        previousPlayer.Panel.color = InactivePlayerColor.PanelColor;
        previousPlayer.PanelText.color = InactivePlayerColor.TextColor;
    }

    private bool CheckThreeInRow(int cellIndex1, int cellIndex2, int cellIndex3, string playerText)
    {
        if (
            GridCellsList[cellIndex1].text == playerText &&
            GridCellsList[cellIndex2].text == playerText &&
            GridCellsList[cellIndex3].text == playerText
            )
        {
            return true;
        }

        return false;
    }

    private void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < GridCellsList.Length; i++)
        {
            GridCellsList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    private void SetPlayerButtons(bool toggle)
    {
        PlayerX.button.interactable = toggle;
        PlayerO.button.interactable = toggle;
    }

    private void SetPlayerColorsInactive()
    {
        PlayerX.Panel.color = InactivePlayerColor.PanelColor;
        PlayerX.PanelText.color = InactivePlayerColor.TextColor;
        PlayerO.Panel.color = InactivePlayerColor.PanelColor;
        PlayerO.PanelText.color = InactivePlayerColor.TextColor;
    }

    private void CheckWins(string playerText)
    {
        _moveCount++;

        if (CheckThreeInRow(0, 1, 2, playerText))
        {
            GameOver(playerText);
        }
        else if (CheckThreeInRow(3, 4, 5, playerText))
        {
            GameOver(playerText);
        }
        else if (CheckThreeInRow(6, 7, 8, playerText))
        {
            GameOver(playerText);
        }
        else if (CheckThreeInRow(0, 3, 6, playerText))
        {
            GameOver(playerText);
        }
        else if (CheckThreeInRow(1, 4, 7, playerText))
        {
            GameOver(playerText);
        }
        else if (CheckThreeInRow(2, 5, 8, playerText))
        {
            GameOver(playerText);
        }
        else if (CheckThreeInRow(0, 4, 8, playerText))
        {
            GameOver(playerText);
        }
        else if (CheckThreeInRow(2, 4, 6, playerText))
        {
            GameOver(playerText);
        }
        else if (_moveCount >= 9)
        {
            GameOver("draw");
        }
    }

    public void SetStartingPlayer(string startingPlayer)
    {
        _playerText = startingPlayer;

        if (_playerText == "X")
        {
            SetPlayerColors(PlayerX, PlayerO);
            _botText = PlayerO.PanelText.text;
        }
        else
        {
            SetPlayerColors(PlayerO, PlayerX);
            _botText = PlayerX.PanelText.text;
        }

        StarGame();
    }

    public string GetPlayerTurn()
    {
        return _playerText;
    }

    public void EndTurn()
    {
        CheckWins(_playerText);

        if (_moveCount < 9)
        {
            MakeMoveOfBot();
            CheckWins(_botText);
        }
    }

    public void MakeMoveOfBot()
    {
        List<Text> availableCells = new List<Text>();

        for (int i = 0; i < GridCellsList.Length; i++)
        {
            if (GridCellsList[i].text == "")
            {
                availableCells.Add(GridCellsList[i]);
            }
        }

        int cellToTake = Random.Range(0, availableCells.Count);
        availableCells[cellToTake].text = _botText;
        availableCells[cellToTake].GetComponentInParent<Button>().interactable = false;
    }

    public void RestartGame()
    {
        _moveCount = 0;

        GameOverPanel.SetActive(false);
        RestartButton.SetActive(false);
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
        SideChoice.SetActive(true);

        for (int i = 0; i < GridCellsList.Length; i++)
        {
            GridCellsList[i].text = "";
        }
    }
}
