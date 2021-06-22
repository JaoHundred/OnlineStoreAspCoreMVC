using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineST.Database;
using OnlineST.Models;
using OnlineST.Models.Pagination;
using OnlineST.UTIL;

namespace OnlineST.Repository
{
    public class CartProductRepository : BaseRepository<CartProduct>
    {
        public CartProductRepository(ILiteDBContext dBContext) : base(dBContext)
        { }


        public async Task<PaginatedCollection<CartProduct>> GetUserCartProductsAsync(long userId, int pageNumber, int elementsPerPage = 20)
        {
            int skip = (pageNumber - 1) * elementsPerPage;

            //TODO: passos a ser feito, pegar a coleção de usuários, incluir cartproducts e talvez incluir também product(relação de 3 coleções)
            var task = Task.Run(() =>
            {
                return _dBContext.LiteDatabase
                .GetCollection<User>().Include(p => p.CartProducts).FindById(userId);
                //.Include(p => p.CartProducts)
                //.Query()
                //.Offset(skip)
                //.Limit(elementsPerPage);

                //return result.ToEnumerable().FirstOrDefault(p => p.Id == userId);
            });

            var result = await task;

            int elementsCount = result.CartProducts?.Count ?? 0 ;


            PaginatedCollection<CartProduct> collection = result.CartProducts.ToPaginationCollection(elementsCount, pageNumber, elementsPerPage);
            //TODO: testar esse método quando estiver com mais dados de produtos no banco
            return collection;
        }

        /// <summary>
        /// Procura por um CartProduct que tenha como filho um Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CartProduct FindByProductId(long id)
        {
            return _dBContext.LiteDatabase.GetCollection<CartProduct>().FindAll().FirstOrDefault(p => p.Product?.Id == id);
        }

        public CartProduct FindDataCartProduct(long id)
        {
            return _dBContext.LiteDatabase.GetCollection<CartProduct>().Include(p => p.Product).FindById(id);
        }
    }
}
