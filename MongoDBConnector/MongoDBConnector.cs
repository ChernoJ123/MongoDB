 using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBConnector
{
    /// <summary>
    /// A simple helper class for checking MongoDB connectivity.
    /// </summary>
    public class MongoConnectionHelper
    {
        private readonly MongoClient _mongoClient;

        /// <summary>
        /// Initialize with a MongoDB connection string.
        /// </summary>
        /// <param name="connectionUri">MongoDB URI, e.g. "mongodb://localhost:27017"</param>
        public MongoConnectionHelper(string connectionUri)
        {
            if (string.IsNullOrEmpty(connectionUri))
            {
                throw new ArgumentNullException(nameof(connectionUri), "Connection string cannot be empty.");
            }

            _mongoClient = new MongoClient(connectionUri);
        }

        /// <summary>
        /// Attempts to ping the server to confirm connectivity.
        /// Returns true if the server responds, false otherwise.
        /// </summary>
        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                var adminDb = _mongoClient.GetDatabase("admin");
                var pingCommand = new BsonDocument { { "ping", 1 } };

                // Send command to MongoDB
                await adminDb.RunCommandAsync<BsonDocument>(pingCommand);
                return true;
            }
            catch (Exception)
            {
                // Any error means connectivity check failed
                return false;
            }
        }
    }
}
