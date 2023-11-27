using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameManager : Singleton<GameManager>
{
    private GameState _gameState;
    private void Awake()
    {
        ChangeState(GameState.MainMenu);
    }
    public void ChangeState(GameState state)
    {
        _gameState = state;
    }
    public bool IsState(GameState gameState)
    {
        return _gameState == gameState;
    }
}