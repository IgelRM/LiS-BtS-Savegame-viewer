namespace SaveGameEditor
{
    public enum Episode
    {
        First,
        Second,
        Third,
        Bonus
    }

    public enum SaveSlot
    {
        First,
        Second,
        Third

    }

    public enum VariableScope
    {
        Global,
        CurrentMainCheckpoint,
        CurrentFarewellCheckpoint,
        LastCheckpoint,
        RegularCheckpoint
    }
}
