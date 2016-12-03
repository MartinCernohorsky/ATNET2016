using System;
using System.Runtime.Serialization;

namespace SharedLibs.DataContracts
{
    /// <summary>
    /// User information
    /// </summary>
    [DataContract]
    public class Student : DTO
    {
        /// <summary>
        /// Unique user ID
        /// </summary>
        [DataMember]
        public Guid Id { set; get; }

        /// <summary>
        /// Name of user
        /// </summary>
        [DataMember]
        public string Name { set; get; }


        /// <summary>
        /// Surname of user
        /// </summary>
        [DataMember]
        public string Surename { set; get; }


        /// <summary>
        /// Name of company
        /// If the customer is company
        /// </summary>
        [DataMember]
        public string City { set; get; }


        /// <summary>
        /// E-mail address
        /// </summary>
        [DataMember]
        public int Age { set; get; }
    }
}
