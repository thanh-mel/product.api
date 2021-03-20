namespace Product.API.Services
{
    /// <summary>
    /// IMPORTANT: To avoid the LOST UPDATE problem in REST API where in some cases there will be a risk that multiple requests could 
    /// update or delete on the same product, therefore we need to implement the optimistic locking either at the Web API level 
    /// or database level. In this project it's on Web API level.
    /// </summary>
    public interface IOptimisticLockingResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputHash"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        bool IsRequestValid(string inputHash, object model);
    }
}
