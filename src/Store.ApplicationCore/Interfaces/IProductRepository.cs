using Store.ApplicationCore.DTOs;
using System.Collections.Generic;

namespace Store.ApplicationCore.Interfaces
{
    public interface IProductRepository
    {
        List<ProductResponse> GetProducts();

        SingleProductResponse GetProductById(int productId);

        void DeleteProductById(int productId);

        SingleProductResponse CreateProduct(CreateProductRequest request);

        SingleProductResponse UpdateProduct(int productId, UpdateProductRequest request);
    }
}