using System.Reflection;

public static class TrainerBotHandler {
    /// <summary>
    /// Find a Behaviour by name. The behaviour specifies its name using the TrainerBotBehaviour Attribute.
    /// If none is found, the default is returned.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static ITrainerBotBehaviour FindByName(string name) {
        var type = (from t in typeof(TrainerBotBehaviourAttribute).Assembly.GetTypes()
            where t.GetCustomAttribute<TrainerBotBehaviourAttribute>(true) is { } attr
                  && attr.BehaviourName.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                  && typeof(ITrainerBotBehaviour).IsAssignableFrom(t)
            select t).FirstOrDefault();

        return type != null ? (ITrainerBotBehaviour)Activator.CreateInstance(type)! : GetDefault();
    }

    public static ITrainerBotBehaviour GetDefault() => new RandomTrainerBot();
}