using System.Threading.Tasks;

namespace Aggregator.WebApi.Product;

public interface IProductService
{
    Task<ProductApi> GetProductByIdAsync(int productId, int cancelTimeout);
}
