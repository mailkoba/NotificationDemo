
namespace NotificationDemo.Domain
{
    /// <summary>
    /// Database representation of a push subscription
    /// </summary>
    public class Subscription : Entity
    {
        public Subscription() { }

        public Subscription(long userId, string endpoint, string p256Dh, string auth)
        {
            UserId = userId;
            Endpoint = endpoint;
            ExpirationTime = null;
            P256Dh = p256Dh;
            Auth = auth;
        }

        /// <summary>
        /// User id associated with the push subscription.
        /// </summary>
        public virtual User User { get; set; }

        public long UserId { get; set; }

        /// <summary>
        /// The endpoint associated with the push subscription.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The subscription expiration time associated with the push subscription, if there is one, or null otherwise.
        /// </summary>
        public double? ExpirationTime { get; set; }

        /// <summary>
        /// An
        /// <see href="https://en.wikipedia.org/wiki/Elliptic_curve_Diffie%E2%80%93Hellman">Elliptic curve Diffie–Hellman</see>
        /// public key on the P-256 curve (that is, the NIST secp256r1 elliptic curve).
        /// The resulting key is an uncompressed point in ANSI X9.62 format.
        /// </summary>
        public string P256Dh { get; set; }

        /// <summary>
        /// An authentication secret, as described in
        /// <see href="https://tools.ietf.org/html/draft-ietf-webpush-encryption-08">Message Encryption for Web Push</see>.
        /// </summary>
        public string Auth { get; set; }
    }
}
