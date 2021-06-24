using OnlineST.Database;
using OnlineST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineST.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ILiteDBContext liteDBContext) : base(liteDBContext)
        { }

        public User Find(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            //var products = _dBContext.LiteDatabase.GetCollection<Product>();
            //var cartProducts = _dBContext.LiteDatabase.GetCollection<CartProduct>();

            var user = _dBContext.LiteDatabase.GetCollection<User>()
                .FindOne(p => p.Email == email);

            //for (int i = 0; i < user.CartProducts.Count; i++)
            //{
            //    var cartProduct = user.CartProducts[i];

            //    user.CartProducts[i] = cartProducts.FindById(cartProduct.Id);
            //    user.CartProducts[i].Product = products.FindById(user.CartProducts[i].Product.Id);
            //}   

            
            return user;
            //cartProducts.ForEach(q => q.Product = products.FindById(q.Product.Id))


            //.FindOne(p => p.Email == email);
        }


        //TODO: testar a chamada desse abaixo em cardProductRepository no método GetUserCartProductsAsync
        public User Find(long id)
        {
            var products = _dBContext.LiteDatabase.GetCollection<Product>();
            var cartProducts = _dBContext.LiteDatabase.GetCollection<CartProduct>();

            var user = _dBContext.LiteDatabase.GetCollection<User>()
                .Include(p => p.CartProducts)
                .FindById(id);

            for (int i = 0; i < user.CartProducts.Count; i++)
            {
                var cartProduct = user.CartProducts[i];

                user.CartProducts[i] = cartProducts.FindById(cartProduct.Id);
                user.CartProducts[i].Product = products.FindById(user.CartProducts[i].Product.Id);
            }


            return user;
            //cartProducts.ForEach(q => q.Product = products.FindById(q.Product.Id))


            //.FindOne(p => p.Email == email);
        }
    }
}
