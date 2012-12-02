using System;

namespace Instagram.AuthService
{
    /// <summary>
    /// Simple singleton to hold the container since controller is instantiated per request
    /// </summary>
    public class InstagramCodeContainer
    {
        /// <summary>
        /// Self instance
        /// </summary>
        private static InstagramCodeContainer Self { get; set; }

        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly Object ThreadLock = new Object();

        /// <summary>
        /// This will hold instagram response access token after authenticated
        /// </summary>
        public string AccessToken;

        /// <summary>
        /// Make constructor private
        /// </summary>
        private InstagramCodeContainer()
        {
        }

        /// <summary>
        /// Make the get instance from the singleton thread safe
        /// </summary>
        /// <returns>A new or the previously instantiated class</returns>
        public static InstagramCodeContainer GetInstance()
        {
            lock (ThreadLock)
            {
                return Self ?? (Self = new InstagramCodeContainer());
            }
        }
    }

}
