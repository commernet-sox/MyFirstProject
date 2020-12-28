namespace CPC.Logger
{
    public interface ITemplate<T>
    {
        string Name { get; set; }

        T Layout { get; set; }
        RabbitExternal RabbitSetting { get; set; }

    }
}
