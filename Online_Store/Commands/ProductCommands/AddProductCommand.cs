﻿using Bytes2you.Validation;
using Online_Store.Core.ProductServices;
using Online_Store.Core.Providers;
using Online_Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Online_Store.Commands.ProductCommands
{
    public class AddProductCommand : Command
    {
        private readonly IProductService productService;

        public AddProductCommand(IStoreContext context, IWriter writer, IReader  reader,
            IProductService productService)
            : base(context, writer, reader)
        {
            Guard.WhenArgument(productService, "productService").IsNull().Throw();
            this.productService = productService;
        }

        public override string Execute()
        {
            IList<string> parameters = TakeInput();

            return this.productService.AddProduct(parameters);
        }
        
        private IList<string> TakeInput()
        {
            string productName = base.ReadOneLine("Enter product`s name: ").ToLower();
            string price = base.ReadOneLine("Enter price: ");
            while (true)
            {
                try
                {
                    decimal test = decimal.Parse(price);
                    if (test < 0)
                    {
                        throw new ArgumentException("Price cannot be negative number.");
                    }
                    break;
                }
                catch
                {
                    price = base.ReadOneLine("Invalid price, try again: ");
                }
            }
            string category = base.ReadOneLine("Enter category: ").ToLower();

            string[] paymentMethodsList = new string[] { "Cash", "VPay", "Visa", "MasterCard", "Kidney" };

            string paymentMethod = base.ReadOneLine(string.Format("{0}\n{1}\n",
            "Select payment method from list: ", string.Join(" ", paymentMethodsList))).ToLower();
            while (true)
            {
                try
                {   //малко е тъпа проверката после ще я оправям
                    if (!paymentMethodsList.Select(x=>x.ToLower()).Contains(paymentMethod.ToLower()))
                    {
                        throw new ArgumentException("Invalid payment method.");
                    }
                    break;
                }
                catch
                {
                    paymentMethod = base.ReadOneLine("Invalid payment method. Try again: ").ToLower();
                }
            }

            string shippingDetailsCost = base.ReadOneLine("Select shipping cost: ");
            while (true)
            {
                try
                {
                    decimal test = decimal.Parse(shippingDetailsCost);
                    if (test < 0)
                    {
                        throw new ArgumentException("Price cannot be negative number.");
                    }
                    break;
                }
                catch
                {
                    shippingDetailsCost = base.ReadOneLine("Invalid price, try again: ");
                }
            }

            string shippingDetailsDeliveryTIme = base.ReadOneLine("Select shipping delivery time in hours (integer): ");
            while (true)
            {
                try
                {
                    int test = int.Parse(shippingDetailsDeliveryTIme);
                    if (test < 0)
                    {
                        throw new ArgumentException("Delivery time cannot be negative number.");
                    }
                    break;
                }
                catch
                {
                    shippingDetailsDeliveryTIme = base.ReadOneLine("Invalid delivery time, try again: ");
                }
            }

            string saleBool = base.ReadOneLine("Is it true, this product is on sale? [true]/[false]? ").ToLower();
            string priceReduction = "0";
            while (true)
            {
                try
                {
                    bool isOnSale = bool.Parse(saleBool);
                    if (isOnSale)
                    {
                        priceReduction = base.ReadOneLine("Enter price reduction in [%]: ");
                        while (true)
                        {
                            try
                            {
                                decimal test = decimal.Parse(priceReduction);
                                if (test < 0)
                                {
                                    throw new ArgumentException("Reduction cannot be negative number.");
                                }
                                break;
                            }
                            catch
                            {
                                priceReduction = base.ReadOneLine("Please enter the price reduction in [%]: ");
                            }
                        }
                    }
                    break;
                }
                catch
                {
                    saleBool = base.ReadOneLine("Please enter [true] or [false]: ").ToLower();
                }
            }

            return new List<string>()
            {
                productName.ToLower(),
                price,
                category.ToLower(),
                paymentMethod,
                shippingDetailsCost,
                shippingDetailsDeliveryTIme,
                priceReduction
            };
        }
    }
}
//AUTO   int Id { get; set; }
//public string ProductName { get; set; }
//public decimal Price { get; set; }
//public virtual Category Categories { get; set; }
//AUTO   DateTime Date { get; set; }
//public PaymentMethodEnum PaymentMethod { get; set; }
//AUTO   bool Instock { get; set; }
//AUTO   virtual Seller Seller { get; set; }
//public virtual ShippingDetails ShippingDetails { get; set; }
//public virtual Feedback Feedback { get; set; }
//public virtual Sale Sale { get; set; }