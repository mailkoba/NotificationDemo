using System.Collections.Generic;

namespace NotificationDemo.Domain
{
    public class User : Entity
    {
        public User()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Subscriptions = new HashSet<Subscription>();
        }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Name { get; set; }

        public virtual ISet<Subscription> Subscriptions { get; set; }
    }
}
