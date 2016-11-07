﻿using System;
using System.Runtime.Serialization;

namespace SharedLibs.DataContracts
{
    /// <summary>
    /// Datacontract for product
    /// </summary>
    //TODO Is obsolote datacontract or not? 
    [DataContract]
    public class Product : DTO
    {
        /// <summary>
        /// Identifier of the product
        /// </summary>
        [DataMember]
        public Guid ID { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        [DataMember]
        public double Price { get; set; }
    }
}
