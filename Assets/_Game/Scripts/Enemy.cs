using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private EnemyController controller;
    private string[] botNames = { "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel", "India", "Juliet"};
    public string GetRandomBotName()
    {
        int randomIndex = Random.Range(0, botNames.Length);
        return botNames[randomIndex];
    }
    public void Move(Vector3 direction)
    {
        transform.position += Speed * Time.deltaTime * direction;
    }
}
