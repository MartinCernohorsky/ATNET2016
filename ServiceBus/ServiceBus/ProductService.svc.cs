﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using SharedLibs.DataContracts;

namespace ServiceBus
{
    public class ProductService : IProductService
    {
        /// <summary>
        /// This method should return a product with requested guid
        /// </summary>
        /// <param name="guid">Guid of a requested product</param>
        /// <returns>requested product</returns>
        public SharedLibs.DataContracts.Product GetProduct(Guid guid)
        {
            try
            {
                using (var context = new ServiceBusDatabaseEntities())
                {
                    var product = context.Products.FirstOrDefault(p => p.Id == guid);

                    if (product == null)
                    {
                        return new SharedLibs.DataContracts.Product
                        {
                            Result = SharedLibs.DataContracts.Result.ErrorFormat("Product {0} not found.", guid)
                        };
                    }

                    return new SharedLibs.DataContracts.Product
                    {
                        Result = SharedLibs.DataContracts.Result.SuccessFormat("Product {0} found", guid),
                        ID = guid,
                        Name = product.Name,
                        Price = product.Price
                    };
                }
            }
            catch (Exception exception)
            {
                return new SharedLibs.DataContracts.Product
                {
                    Result = SharedLibs.DataContracts.Result.FatalFormat("ProductService.GetProduct Exception: {0}", exception.Message)
                };
            }
        }

        /// <summary>
        /// This method returns all defined products
        /// </summary>
        /// <returns>collection of all products</returns>
        public SharedLibs.DataContracts.Products GetAllProducts()
        {
            try
            {
                using (var context = new ServiceBusDatabaseEntities())
                {
                    var returnCollection = new List<SharedLibs.DataContracts.Product>();

                    foreach (var product in context.Products)
                    {
                        returnCollection.Add(new SharedLibs.DataContracts.Product
                        {
                            ID = product.Id,
                            Name = product.Name,
                            Price = product.Price
                        });
                    }

                    return new SharedLibs.DataContracts.Products
                    {
                        Result = SharedLibs.DataContracts.Result.Success("All products returned"),
                        Items = returnCollection
                    };
                }
            }
            catch (Exception exception)
            {
                return new SharedLibs.DataContracts.Products
                {
                    Result = SharedLibs.DataContracts.Result.FatalFormat("ProductService.GetProduct Exception: {0}", exception.Message)
                };
            }
        }

        /// <summary>
        /// This method adds product into the datasource
        /// </summary>
        /// <param name="product">Product object</param>
        /// <returns>Result object</returns>
        public Result AddProduct(SharedLibs.DataContracts.Product product)
        {
            return AddProduct(product.Name, product.Price, product.ID);
        }

        /// <summary>
        /// This method adds product into datasource
        /// </summary>
        /// <param name="name">Name of a new product</param>
        /// <param name="price">Price of a new product</param>
        /// <param name="guid">ID of a new prodcut</param>
        /// <returns>Result object</returns>
        public Result AddProduct(string name, double price, Guid guid)
        {
            try
            {
                if ((!String.IsNullOrWhiteSpace(name) && name.Length <= 50) && price >= 0)
                {
                    using (var context = new ServiceBusDatabaseEntities())
                    {

                        context.Products.Add(new Product() { Id = guid, Name = name, Price = price});
                        context.SaveChanges();

                        return Result.SuccessFormat("Product {0} | {1} has been added", guid, name);
                    }
                }
                else
                {
                    return Result.Error("Provided parameters are unacceptable.");
                }
            }
            catch (Exception ex)
            {
                return Result.FatalFormat("ProductService.AddProduct Exception: {0}", ex.Message);
            }
            
        }

        public SharedLibs.DataContracts.Product EditProduct(Guid guid, string name, double price)
        {
            try
            {   //TODO: Do I have to create an objcet of the service like this one in case I do want to use a method GetProduct?
                var service = new ProductServiceProxyClass.ProductServiceClient();
                var changesName = false;
                var changesPrice = false;

                var originalProduct = service.GetProduct(guid);

                var editableProduct = new Product()
                {
                    Id = originalProduct.ID,
                    Name = originalProduct.Name,
                    Price = originalProduct.Price
                };

                if (!String.IsNullOrWhiteSpace(name) && name.Length <= 50 && name != editableProduct.Name)
                {
                    changesName = true;
                }

                if (price != editableProduct.Price && price >=0)
                {
                    changesPrice = true;
                }

                if (changesName || changesPrice)
                {
                    using (var context = new ServiceBusDatabaseEntities())
                    {   //TODO In case of more columns find a way to simplify this! 21.10.2016
                        if (changesName)
                        {
                            editableProduct.Name = name;
                        }
                        if (changesPrice)
                        {
                            editableProduct.Price = price;
                        }
                        //context.Products.Attach(editableProduct);
                        //Since there are only few columns I would prefer this
                        context.Entry(editableProduct).State = EntityState.Modified;
                        context.SaveChanges();
                    }

                    return new SharedLibs.DataContracts.Product()
                    {
                        ID = editableProduct.Id,
                        Name = editableProduct.Name,
                        Price = editableProduct.Price,
                        Result = Result.Success()
                    };
                }
                else
                {
                    originalProduct.Result = Result.Warning("This product was not modified.");
                    return originalProduct;
                }

            }
            catch (Exception exception)
            {
                return new SharedLibs.DataContracts.Product()
                {
                    Result =
                        Result.FatalFormat("ProductService.EditProduct exception has occured : {0}", exception.Message)
                };
            }
        }
    }
}
