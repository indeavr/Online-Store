﻿using Online_Store.Core.Factories;
using Online_Store.Core.Providers;
using Online_Store.Data;
using Online_Store.Models;
using System.Collections.Generic;
using System.Linq;

namespace Online_Store.Commands.CartCommands
{
    public class AddToCartCommand : Command, ICommand
    {
        private readonly IModelFactory factory;
        
        public AddToCartCommand(IModelFactory factory, IStoreContext context, IWriter writer, IReader reader)
            : base(context, writer, reader)
        {
            this.factory = factory;
        }

        public override string Execute()
        {
            IList<string> parameters = TakeInput();

            int cartId = int.Parse(parameters[0]);
            string productName = parameters[1];

            //Cart cart = this.factory.CreateCart();
            //Product product = this.factory.CreateProduct();
            //product.ProductName = productName;

            //this.context.Carts.Add(cart);

            Cart cart = base.context.Carts.Single(c => c.UserId == cartId);
            Product product = base.context.Products.Single(p => p.ProductName == productName);
            cart.Products.Add(product);

            return $"Product successfully added to cart";
        }

        private IList<string> TakeInput()
        {
            var cart = base.ReadOneLine("Cart ID: ");
            var product = base.ReadOneLine("Product: ");

            return new List<string>() { cart, product };
        }
    }
}
