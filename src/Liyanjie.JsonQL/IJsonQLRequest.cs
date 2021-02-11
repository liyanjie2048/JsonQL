using System.Threading.Tasks;

namespace Liyanjie.JsonQL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJsonQLRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<string> GetQueryAsync();
    }
}
