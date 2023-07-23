using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductIdAsync(int id);
        Task<ProductBrand> GetProductBrandIdAsync(int id);
        Task<ProductType> GetProductTypeIdAsync(int id);

        // Pq usamos IReadOnlyList? Pq a gnt não precisa das funções que um List tem nesse caso. Queremos apenas mostrar/trazer dados
        // e não manipular eles.
        Task<IReadOnlyList<Product>> GetProductsdAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsdAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
    }
}