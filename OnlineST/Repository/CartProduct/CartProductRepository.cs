using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
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
            int elementsCount = 0;

            var task = Task.Run(() =>
            {
                var user = _dBContext.LiteDatabase.GetCollection<User>()
                .Include(p => p.CartProducts)
                .Include(BsonExpression.Create("$.CartProducts[*].Product"))
                .FindById(userId);

                elementsCount = user.CartProducts?.Count ?? 0;

                return user.CartProducts?.Skip(skip).Take(elementsPerPage);
            });

            var cartProducts = await task;

            PaginatedCollection<CartProduct> collection = cartProducts.ToPaginationCollection(elementsCount, pageNumber, elementsPerPage);
            //TODO: testar esse método quando estiver com mais dados de produtos no banco
            return collection;
        }

        private object GetProducts()
        {
            throw new NotImplementedException();
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
