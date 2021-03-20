using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Product.API.Helpers;

namespace Product.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class OptimisticLockingResolver : IOptimisticLockingResolver
    {
        private readonly ILogger<OptimisticLockingResolver> _logger;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public OptimisticLockingResolver(ILogger<OptimisticLockingResolver> logger)
        {
            _logger = logger;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputHash"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsRequestValid(string inputHash, object model)
        {
            var hash = HashFactory.GetHashSHA256(JsonConvert.SerializeObject(model));
            return HashFactory.VerifyHash(inputHash, hash);
        }
    }
}
