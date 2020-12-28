namespace CPC
{
    /// <summary>
    /// interface which should be implemented by tasks run on startup
    /// </summary>
    public interface IEngineService
    {
        /// <summary>
        /// start a service
        /// </summary>
        void Start();

        void Stop();
    }
}
