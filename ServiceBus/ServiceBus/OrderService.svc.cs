﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using SharedLibs.DataContracts;
using SharedLibs.Enums;
using System.Net.Mail;

namespace ServiceBus
{
    public class OrderService : IOrderService
    {

        // Implementation and other parts - I will make an effort to do it per partes :-) KZ
        // At this time is not created table order and that is part of code hidden

        /// <summary>
        /// Method returns order according requested ID number
        /// </summary>
        /// <param name="guid">Order ID number</param>
        /// <returns>Order object</returns>
        public SharedLibs.DataContracts.Order GetOrder(Guid guid)
        {
            try
            {
                using (var context = new ServiceBusDatabaseEntities())
                {
                    /************************************************************hidden code**********************
                    var order = context.Orders.FirstOrDefault(o => o.ID == guid);

                    if (order == null)
                    {
                        return new SharedLibs.DataContracts.Order
                        {
                            Result = SharedLibs.DataContracts.Result.ErrorFormat("Requested order ID number {0} was not found.", guid)
                        };
                    }

                    return new SharedLibs.DataContracts.Order
                    {
                        Id = guid,
                        Basket = order.Basket,
                        DeliveryAddress = order.DeliveryAddress,
                        BillingInformation = order.BillingInformation,
                        OrderDate = order.OrderDate,
                        DeliveryDate = order.DeliveryDate,
                        OrderState = order.OrderState,
                        Result = SharedLibs.DataContracts.Result.SuccessFormat("Requested order ID number {0} was found.", guid)

                    };
                    */
                }
            }
            catch (Exception exception)
            {
                
                return new SharedLibs.DataContracts.Order
                {
                    Result = SharedLibs.DataContracts.Result.FatalFormat("In method OrderService.GetOrder was thrown exception: {0}", exception.Message)
                };
                
            }

//********* delete this code after finishing implementation method GetOrder ************
            return new SharedLibs.DataContracts.Order
            {
                Result = Result.Fatal("Not finish")
            };
        }


        /// <summary>
        /// Method returns list of orders in collection
        /// </summary>
        /// <returns>List of orders</returns>
        public SharedLibs.DataContracts.Orders GetAllOrders()
        {
            try
            {
                using (var context = new ServiceBusDatabaseEntities())
                {
                    var orderList = new List<Order>();
                    /***********************************************hidden code***************************
                    foreach (var order in context.Orders)
                    {
                        orderList.Add(new SharedLibs.DataContracts.Order
                        {
                            Id = order.Id,
                            Basket = order.Basket,
                            DeliveryAddress = order.DeliveryAddress,
                            BillingInformation = order.BillingInformation,
                            OrderDate = order.OrderDate,
                            DeliveryDate = order.DeliveryDate,
                            OrderState = order.OrderState
                        });
                    }

                    if (orderList.Count == 0)
                    {
                        return new SharedLibs.DataContracts.Orders
                        {
                            Result = SharedLibs.DataContracts.Result.WarningFormat("It was not found any item of orders."),
                            Items = orderList
                        };
                    }
                    else
                    {
                        return new SharedLibs.DataContracts.Orders
                        {
                            Result = SharedLibs.DataContracts.Result.Success(),
                            Items = orderList
                        };
                    }
                    */
                }
            }
            catch (Exception exception)
            {
                return new SharedLibs.DataContracts.Orders
                {
                    Result = SharedLibs.DataContracts.Result.FatalFormat("In method OrderService.GetAllOrders was thrown exception: {0}",
                                                                         exception.Message)
                };
            }

//********* delete this code after finishing implementation method GetOrder ************
            return new SharedLibs.DataContracts.Orders
            {
                Result = Result.Fatal("Not finish")
            };
        }


        /// <summary>
        /// Method adds new order to system
        /// <param name="guid">ID of a order</param>
        /// <param name="basket">Reference to basket object</param>
        /// <param name="address">Reference to address object</param>
        /// <param name="billingInformation">Reference to billingInformation object</param>
        /// <param name="orderDate">Order creation date</param>
        /// <param name="deliveryDate">Order delivery date</param>
        /// </summary>
        /// <returns>Result object</returns>
        public SharedLibs.DataContracts.Result AddOrder(Guid guid, Basket basket, Address deliveryAddress, BillingInformation billingInformation,
                                                 DateTime orderDate, DateTime deliveryDate, OrderStateType orderState)
        {
            try
            {
                using (var context = new ServiceBusDatabaseEntities())
                {
                    if (basket != null && deliveryAddress != null && billingInformation != null &&
                        orderDate == DateTime.Now && deliveryDate >= DateTime.Now)
                    {
                        var newOrder = new SharedLibs.DataContracts.Order
                        {
                            Id = guid,
                            Basket = basket,
                            DeliveryAddress = deliveryAddress,
                            BillingInformation = billingInformation,
                            OrderDate = orderDate,
                            DeliveryDate = deliveryDate,
                            OrderState = orderState
                        };

//********************* context.Orders.Add(newOrder); ************************hidden code***********************
                        context.SaveChanges();

                        return SharedLibs.DataContracts.Result.SuccessFormat("New order ID number {0} was stored successfully.", guid);
                    }
                    else
                    {
                        return SharedLibs.DataContracts.Result.ErrorFormat("Input parameters have not correct value.");
                    }
                }
            }
            catch (Exception exception)
            {
                return SharedLibs.DataContracts.Result.FatalFormat("In method OrderService.AddOrder was thrown exception: {0}",
                                                                         exception.Message);
            }           
        }


        /// <summary>
        /// Method adds new order to system using order object
        /// <param name="order">Order object</param>
        /// </summary>
        /// <returns>Result object</returns>
        public SharedLibs.DataContracts.Result AddOrder(Order order)
        {
            return AddOrder(order.Id, order.Basket, order.DeliveryAddress, order.BillingInformation,
                            order.OrderDate, order.DeliveryDate, order.OrderState);
        }


        /// <summary>
        /// Method allows editing items of order
        /// </summary>
        /// <param name="guid">ID of a order</param>
        /// <param name="basket">Reference to basket object</param>
        /// <param name="address">Reference to address object</param>
        /// <param name="billingInformation">Reference to billingInformation object</param>
        /// <param name="deliveryDate">Order delivery date</param>
        /// <param name="orderState">Signals current state of order</param>
        /// <returns>New Order object</returns>
        public SharedLibs.DataContracts.Order EditOrder(Guid guid, Basket basket, Address deliveryAddress, BillingInformation billingInformation,
                                                        DateTime deliveryDate, OrderStateType orderState)
        {
            try
            {
                var storedOrder = this.GetOrder(guid);

                if (storedOrder.Result.ResultType == SharedLibs.Enums.ResultType.Success)
                {
                    var alteredOrder = new Order
                    {
                        Id = storedOrder.Id,
                        Basket = storedOrder.Basket,
                        DeliveryAddress = storedOrder.DeliveryAddress,
                        BillingInformation = storedOrder.BillingInformation,
                        OrderDate = storedOrder.OrderDate,
                        DeliveryDate = storedOrder.DeliveryDate,
                        OrderState = storedOrder.OrderState
                    };


                    using (var context = new ServiceBusDatabaseEntities())
                    {
//********************* context.Orders.Attach(alteredOrder); *****************hidden code***************

                        // Test data for change of basket object
                        if (basket != null && basket != storedOrder.Basket)
                        {
                            alteredOrder.Basket = basket;
                        }

                        // Test data for change of address object
                        if (deliveryAddress != null && deliveryAddress != storedOrder.DeliveryAddress)
                        {
                            alteredOrder.DeliveryAddress = deliveryAddress;
                        }

                        // Test data for change of billing information object
                        if (billingInformation != null && billingInformation != storedOrder.BillingInformation)
                        {
                            alteredOrder.BillingInformation = billingInformation;
                        }

                        // Test data for change of delivery date
                        if (deliveryDate.Year != 0 && deliveryDate.Month != 0 && deliveryDate.Day != 0 && deliveryDate < DateTime.Now)
                        {
                            alteredOrder.DeliveryDate = deliveryDate;
                        }

                        // If no change was performed it is returned origin order object
                        if (context.Entry(alteredOrder).State == System.Data.Entity.EntityState.Unchanged)
                        {
                            storedOrder.Result = SharedLibs.DataContracts.Result.WarningFormat("Order ID number {0} was not changed.", guid);
                            return storedOrder;
                        }
                        else
                        {
                            alteredOrder.OrderState = SharedLibs.Enums.OrderStateType.Changed;
                            context.SaveChanges();

                            return new SharedLibs.DataContracts.Order
                            {
                                Id = alteredOrder.Id,
                                Basket = alteredOrder.Basket,
                                DeliveryAddress = alteredOrder.DeliveryAddress,
                                BillingInformation = alteredOrder.BillingInformation,
                                OrderDate = alteredOrder.OrderDate,
                                DeliveryDate = alteredOrder.DeliveryDate,
                                OrderState = alteredOrder.OrderState
                            };
                        }
                    }
                }
                else
                {
                    return storedOrder;
                }
            }
            catch (Exception exception)
            {
                return new SharedLibs.DataContracts.Order
                {
                    Result = SharedLibs.DataContracts.Result.FatalFormat("In method OrderService.EditOrder was thrown exception: {0}.",
                                                                         exception.Message)
                };
            }
        }


        /// <summary>
        /// Order state change
        /// </summary>
        /// <param name="guid">ID of a order</param>
        /// <param name="state">State of order</param>
        /// /// <returns>Result object</returns>
        public SharedLibs.DataContracts.Result ChangeOrderState(Guid guid, SharedLibs.Enums.OrderStateType orderState)
        {
            try
            {
                var storedOrder = this.GetOrder(guid);

                if (storedOrder.Result.ResultType == SharedLibs.Enums.ResultType.Success)
                {
                    var alteredOrder = new Order
                    {
                        Id = storedOrder.Id,
                        Basket = storedOrder.Basket,
                        DeliveryAddress = storedOrder.DeliveryAddress,
                        BillingInformation = storedOrder.BillingInformation,
                        OrderDate = storedOrder.OrderDate,
                        DeliveryDate = storedOrder.DeliveryDate,
                        OrderState = storedOrder.OrderState
                    };

                    using (var context = new ServiceBusDatabaseEntities())
                    {
                        
//********************* context.Orders.Attach(alteredOrder); ***********************hidden code*******************

                        alteredOrder.OrderState = orderState;

                        if (context.Entry(alteredOrder).State == System.Data.Entity.EntityState.Unchanged)
                        {
                            return SharedLibs.DataContracts.Result.WarningFormat("State of order ID number {0} was not changed.", guid);
                        }
                        else
                        {
                            context.SaveChanges();                            
                            return SharedLibs.DataContracts.Result.SuccessFormat("State of order ID number {0} was changed");
                        }                        
                    }
                }
                else
                {
                    return SharedLibs.DataContracts.Result.WarningFormat("Result of requested order ID number {0} is not success." +
                                                                          " Order state cannot be changed.", guid);
                }
            }
            catch (Exception exception)
            {
                return SharedLibs.DataContracts.Result.FatalFormat("In method OrderService.ChangeOrderState was thrown exception: {0}",
                                                                        exception.Message);           
            }
        }


        /// <summary>
        /// Delete order with specific Guid
        /// </summary>
        /// <param name="guid">guid of a order</param>
        /// <returns>Result object</returns>
        public SharedLibs.DataContracts.Result DeleteOrder(Guid guid)
        {
            try
            {
                using (var context = new ServiceBusDatabaseEntities())
                {
                    var storedOrder = this.GetOrder(guid);

                    if (storedOrder == null)
                    {
                        return SharedLibs.DataContracts.Result.ErrorFormat("Requested order ID number {0} was not found.", guid);
                    }

//***************** context.Orders.Remove(storedOrder); ***********************hidden code*******************
                    context.SaveChanges();

                    return SharedLibs.DataContracts.Result.SuccessFormat("Requested order ID number {0} was deleted successfully.", guid);
                }
            }
            catch (Exception exception)
            {
                return SharedLibs.DataContracts.Result.FatalFormat("In method OrderService.DeleteOrder was thrown exception: {0}.",
                                                                         exception.Message);
            }
        }


        /// <summary>
        /// Send e-mail to client
        /// </summary>
        /// <param name="user">Reference to user</param>
        /// <param name="emailText">Formated text of e-mail</param>
        /// <returns>Result object</returns>
        public SharedLibs.DataContracts.Result SendEmail(User user, Order order, string emailText)
        {
            try
            {
                if (user != null && order != null && !String.IsNullOrEmpty(emailText))
                {
                    string from = "our_company@vsb.cz";   // Temporary sender
                    string subject = "Order number " + order.Id;
                    string smtpServer = "smtpServer";

                    MailMessage message = new MailMessage(from, user.EmailAddress, subject, emailText);
                    SmtpClient client = new SmtpClient(smtpServer); // Temporary server name

                    if (message != null && client != null)
                    {
                        client.Send(message);
                        return SharedLibs.DataContracts.Result.SuccessFormat("E-mail was correctly sent to {0}.", user.EmailAddress);
                    }
                    else
                    {
                        throw new Exception("Memory allocation failure of MailMessage or SmtpClient.");
                    }                    
                }
                else
                {
                    return SharedLibs.DataContracts.Result.ErrorFormat("Input parameters to create e-mail to {0} have not correct value.",
                                                                        user.EmailAddress);                                                                      
                }

            }
            catch (Exception exception)
            {
                return SharedLibs.DataContracts.Result.FatalFormat("In method OrderService.SendEmail was thrown exception: {0}.",
                                                                         exception.Message);
            }
        }


        /// <summary>
        /// Create invoice
        /// </summary>
        /// <param name="user">Reference to user</param>
        /// <param name="order">Reference to order</param>
        /// <returns>Result object</returns>
        public SharedLibs.DataContracts.Result CreateInvoice(User user, Order order)
        {
            return Result.Fatal("Not finish");
        }

        // ********



    }
}
