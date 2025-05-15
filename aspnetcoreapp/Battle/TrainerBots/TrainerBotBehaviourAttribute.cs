public class TrainerBotBehaviourAttribute(string name) : Attribute
{
    public string BehaviourName { get; } = name;
}