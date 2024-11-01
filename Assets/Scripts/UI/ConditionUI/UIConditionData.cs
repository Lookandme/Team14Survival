using UnityEngine;

public class UIConditionData : MonoBehaviour
{
    private PlayerConditions playerConditions;
    public Condition health
    {
        get
        {
            if (playerConditions == null) { playerConditions = CharacterManager.Instance.Player.condition; }
            if (playerConditions != null) { return playerConditions.health; }
            else { return null; }
        }
    }
    public Condition stamina
    {
        get
        {
            if (playerConditions == null) { playerConditions = CharacterManager.Instance.Player.condition; }
            if (playerConditions != null) { return playerConditions.stamina; }
            else { return null; }
        }
    }
    public Condition hunger
    {
        get
        {
            if (playerConditions == null) { playerConditions = CharacterManager.Instance.Player.condition; }
            if (playerConditions != null) { return playerConditions.hunger; }
            else { return null; }
        }
    }
    public Condition thirst
    {
        get
        {
            if (playerConditions == null) { playerConditions = CharacterManager.Instance.Player.condition; }
            if (playerConditions != null) { return playerConditions.thirst; }
            else { return null; }
        }
    }

    private void Start()
    {
        playerConditions = CharacterManager.Instance.Player.condition;
    }
}