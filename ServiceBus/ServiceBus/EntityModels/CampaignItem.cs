
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace ServiceBus.EntityModels
{

using System;
    using System.Collections.Generic;
    
public partial class CampaignItem
{

    public System.Guid Id { get; set; }

    public System.Guid BasketId { get; set; }

    public System.Guid CampaignId { get; set; }



    public virtual Basket Basket { get; set; }

    public virtual Campaign Campaign { get; set; }

}

}
