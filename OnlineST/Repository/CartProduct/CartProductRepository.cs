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

        public CartProduct FindCartProductByUserId(long userId, long productId)
        {
            var user = _dBContext.LiteDatabase.GetCollection<User>()
                .Include(p => p.CartProducts)
                .Include(BsonExpression.Create("$.CartProducts[*].Product"))
                .FindById(userId);

            return user.CartProducts.Find(p => p.Product.Id == productId);
        }


        public CartProduct FindCartProduct(long id)
        {
            return _dBContext.LiteDatabase.GetCollection<CartProduct>()
                .Include(p => p.Product)
                .FindById(id);
        }

        public CartProduct FindCartProductByProductId(long productId)
        {
            return _dBContext.LiteDatabase.GetCollection<CartProduct>()
                .Include(p => p.Product)
                .FindOne(p => p.Product.Id == productId);
        }

        /// <summary>
        /// Deleta o CartProduct e sua referência dentro de User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public bool Delete(long userId, long cartId)
        {
            var cartProductCollection = _dBContext.LiteDatabase.GetCollection<CartProduct>();
            var userCollection = _dBContext.LiteDatabase.GetCollection<User>();
            User user = userCollection.FindById(userId);

            bool deletedCartItem = cartProductCollection.Delete(cartId);
            int deletedAmoundCartItemReferences = user.CartProducts.RemoveAll(p => p.Id == cartId);
            bool updatedUser = userCollection.Update(user.Id, user);

            return deletedCartItem && deletedAmoundCartItemReferences > 0 && updatedUser;
        }
    }
}
