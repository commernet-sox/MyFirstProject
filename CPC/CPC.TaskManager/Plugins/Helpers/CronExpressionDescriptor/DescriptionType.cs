namespace CPC.TaskManager.Plugins
{
    /// <summary>
    /// Enum to define the description "parts" of a Cron Expression  
    /// </summary>
    public enum DescriptionType
    {
        Full,
        TimeOfDay,
        Seconds,
        Minutes,
        Hours,
        DayOfWeek,
        Month,
        DayOfMonth,
        Year
    }
}
