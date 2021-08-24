using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCellScript : MonoBehaviour
{
    private GameControllerScript _gameController;

    public Button GridCell;
    public Text CellText;

    public void SetGameControllerReference(GameControllerScript gameController)
    {
        _gameController = gameController;
    }

    public void TakeCell()
    {
        CellText.text = _gameController.GetPlayerTurn();
        GridCell.interactable = false;
        _gameController.EndTurn();
    }
}
