using UnityEngine;

public class UIConditionData : MonoBehaviour
{
    private PlayerCoditions playerConditions;

    public Condition health { get { return playerConditions.health; } }
    public Condition stamina { get { return playerConditions.stamina; } }   
    public Condition hunger { get { return playerConditions.hunger; } }
    public Condition thirst { get { return playerConditions.thirst; } }

    private void Awake()
    {
        //playerCoditions = CharacterManager.instance.player.playerconditions;
    }

}