namespace SigobMobile.Common.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Response when request an API EndPoint
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:SigobMobile.Models.Response"/> is sucess.
        /// </summary>
        /// <value><c>true</c> if is sucess; otherwise, <c>false</c>.</value>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the message resultant API call
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the result object (JSON)
        /// </summary>
        /// <value>The result.</value>
        public object Result { get; set; }
    }

    /// <summary>
    /// Transaction response (Put Endpoint)
    /// </summary>
    public class TransactionResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the transaction message.
        /// </summary>
        /// <value>The transaction message.</value>
        [JsonProperty("message")]
        public string TransactionMessage { get; set;  }
    }
}
